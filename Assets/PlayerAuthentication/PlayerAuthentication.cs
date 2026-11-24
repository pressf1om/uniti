// Подключает доступ к основным типам .NET.
using System;
// Подключает доступ к типам асинхронных задач.
using System.Threading.Tasks;
// Подключает доступ к сервису входа Unity Authentication.
using Unity.Services.Authentication;
// Подключает доступ к инициализации облачных сервисов Unity.
using Unity.Services.Core;
// Подключает доступ к базовым типам игрового движка.
using UnityEngine;

// Added by `unity projects create`. Signs the player in anonymously before the
// first scene loads, so every service call has a player ID. The player sees
// nothing, and the account can be upgraded to a real sign-in later.
// Инициализирует Unity Services и выполняет анонимный вход.
public static class PlayerAuthentication
{  // Начало класса `PlayerAuthentication`.
    // Создаёт экземпляр `TaskCompletionSource<bool>` и сохраняет ссылку в `signedIn`.
    static TaskCompletionSource<bool> signedIn = new TaskCompletionSource<bool>();

    // True once the player is signed in, false when sign-in failed. Await it
    // before calling another Unity Gaming Service. It stays pending until the
    // sign-in finishes, whichever startup code reads it first.
    // Даёт доступ к `SignedIn` через `signedIn.Task`.
    public static Task<bool> SignedIn => signedIn.Task;

    // A new task for each Play session, ahead of every BeforeSceneLoad
    // callback, including when domain reload is off.
    // Регистрирует вызов Unity на этапе `RuntimeInitializeLoadType.SubsystemRegistration`.
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    // Сбрасывает задачу авторизации при начале сеанса.
    static void ResetForPlaySession()
    {  // Начало метода `ResetForPlaySession`.
        // Записывает в `signedIn` экземпляр `TaskCompletionSource<bool>`.
        signedIn = new TaskCompletionSource<bool>();
    }  // Конец метода `ResetForPlaySession`.

    // Регистрирует вызов Unity на этапе `RuntimeInitializeLoadType.BeforeSceneLoad`.
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    // Запускает вход перед загрузкой первой сцены и передаёт его результат.
    static async void SignInAtStartup()
    {  // Начало метода `SignInAtStartup`.
        // Задаёт `pending` значением `signedIn`.
        var pending = signedIn;
        // Вызывает `pending.TrySetResult` с аргументами `await SignInAsync()`.
        pending.TrySetResult(await SignInAsync());
    }  // Конец метода `SignInAtStartup`.

    // Инициализирует сервисы и возвращает признак успешного анонимного входа.
    static async Task<bool> SignInAsync()
    {  // Начало метода `SignInAsync`.
        // Выполняет операции, ошибки которых обработает ветка `catch`.
        try
        {  // Начало блока операций с обработкой исключений.
            // Also while another startup callback is initializing: a second
            // call waits for that initialization instead of starting one.
            // Проверяет, инициализировались ли облачные сервисы Unity.
            if (UnityServices.State != ServicesInitializationState.Initialized)
            {  // Начало ветки условия.
                // Запускает и ожидает инициализацию сервисов.
                await UnityServices.InitializeAsync();
            }  // Конец ветки условия.
            // Проверяет, выполнен ли вход игрока.
            if (!AuthenticationService.Instance.IsSignedIn)
            {  // Начало ветки условия.
                // Ожидает анонимный вход без ввода логина.
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
            }  // Конец ветки условия.
            // Пишет сообщение в консоль Unity через `Debug.Log` с аргументами `$"Signed in anonymously as player {AuthenticationService.Instance.PlayerId}"`.
            Debug.Log($"Signed in anonymously as player {AuthenticationService.Instance.PlayerId}");
            // Возвращает `true`.
            return true;
        }  // Конец блока операций с обработкой исключений.
        // Перехватывает исключение `(Exception e)`.
        catch (Exception e)
        {  // Начало обработчика исключения.
            // The game keeps running offline; services that need a player
            // ID stay unavailable until the next start.
            // Пишет предупреждение в консоль Unity через `Debug.LogWarning` с аргументами `$"Anonymous sign-in failed: {e.Message}"`.
            Debug.LogWarning($"Anonymous sign-in failed: {e.Message}");
            // Возвращает `false`.
            return false;
        }  // Конец обработчика исключения.
    }  // Конец метода `SignInAsync`.
}  // Конец класса `PlayerAuthentication`.

// Подключает доступ к основным типам .NET.
using System;
// Подключает доступ к обобщённым спискам, словарям и множествам.
using System.Collections.Generic;
// Подключает доступ к типам асинхронных задач.
using System.Threading.Tasks;
// Подключает доступ к сервису облачных сохранений Unity.
using Unity.Services.CloudSave;
// Подключает доступ к типам из `Unity.Services.CloudSave.Models`.
using Unity.Services.CloudSave.Models;
// Подключает доступ к базовым типам игрового движка.
using UnityEngine;

// Added by `unity projects create`. One Cloud Save slot for the signed-in
// player, keyed on their player ID by the service. Store the game's save as
// one serializable object, for example a class turned into JSON.
// Use it in Play mode: the sign-in it waits for starts when a scene loads.
// Предоставляет отдельные операции чтения и записи Unity Cloud Save.
public static class PlayerSave
{  // Начало класса `PlayerSave`.
    // Задаёт `SlotKey` значением `"progress"`.
    public const string SlotKey = "progress";

    // Saves `data` to the slot. False when the player is not signed in or the
    // save failed.
    // После входа сериализует переданные данные и пишет их в облако.
    public static async Task<bool> SaveAsync<T>(T data)
    {  // Начало метода `SaveAsync`.
        // Выполняет операции, ошибки которых обработает ветка `catch`.
        try
        {  // Начало блока операций с обработкой исключений.
            // Проверяет успешность входа прежде, чем записывать в облако.
            if (!await PlayerAuthentication.SignedIn)
            {  // Начало ветки условия.
                // Возвращает `false`.
                return false;
            }  // Конец ветки условия.
            // Передаёт данные в единственную облачную запись с ключом прогресса.
            await CloudSaveService.Instance.Data.Player.SaveAsync(
                // Формирует словарь с ключом сохранения и объектом данных.
                new Dictionary<string, object> { { SlotKey, data } });
            // Возвращает `true`.
            return true;
        }  // Конец блока операций с обработкой исключений.
        // Перехватывает исключение `(Exception e)`.
        catch (Exception e)
        {  // Начало обработчика исключения.
            // Пишет предупреждение в консоль Unity через `Debug.LogWarning` с аргументами `$"Cloud Save failed: {e.Message}"`.
            Debug.LogWarning($"Cloud Save failed: {e.Message}");
            // Возвращает `false`.
            return false;
        }  // Конец обработчика исключения.
    }  // Конец метода `SaveAsync`.

    // Loads the slot, or `fallback` when there is no save yet, the player is
    // not signed in, or the load failed.
    // Загружает и десериализует облачные данные либо возвращает запасное значение.
    public static async Task<T> LoadAsync<T>(T fallback = default)
    {  // Начало метода `LoadAsync`.
        // Выполняет операции, ошибки которых обработает ветка `catch`.
        try
        {  // Начало блока операций с обработкой исключений.
            // Ожидает входа перед запросом сохранения.
            if (!await PlayerAuthentication.SignedIn)
            {  // Начало ветки условия.
                // Возвращает `fallback`.
                return fallback;
            }  // Конец ветки условия.
            // Асинхронно получает `CloudSaveService.Instance.Data.Player.LoadAsync( new HashSet<string> { SlotKey })` и сохраняет результат в `items`.
            var items = await CloudSaveService.Instance.Data.Player.LoadAsync(
                // Создаёт экземпляр `HashSet<string>`.
                new HashSet<string> { SlotKey });
            // Выбирает возвращаемое значение по условию `items.TryGetValue(SlotKey, out Item item)`.
            return items.TryGetValue(SlotKey, out Item item) ? item.Value.GetAs<T>() : fallback;
        }  // Конец блока операций с обработкой исключений.
        // Перехватывает исключение `(Exception e)`.
        catch (Exception e)
        {  // Начало обработчика исключения.
            // Пишет предупреждение в консоль Unity через `Debug.LogWarning` с аргументами `$"Cloud Save load failed: {e.Message}"`.
            Debug.LogWarning($"Cloud Save load failed: {e.Message}");
            // Возвращает `fallback`.
            return fallback;
        }  // Конец обработчика исключения.
    }  // Конец метода `LoadAsync`.
}  // Конец класса `PlayerSave`.

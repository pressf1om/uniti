// Подключает доступ к основным типам .NET.
using System;
// Подключает доступ к базовым типам игрового движка.
using UnityEngine;
// Подключает доступ к загрузке и запросу сцен.
using UnityEngine.SceneManagement;

// Перечисляет состояния партии.
public enum RunState
{  // Начало перечисления `RunState`.
    // Задаёт значение перечисления «активная игра».
    Running,
    // Задаёт значение перечисления «победа».
    Won
}  // Конец перечисления `RunState`.

// Управляет состоянием партии и окном победы.
public class GameLoop : MonoBehaviour
{  // Начало класса `GameLoop`.
    // Объявляет свойство `Instance` и правила доступа к нему.
    public static GameLoop Instance { get; private set; }

    // Объявляет свойство `State` и правила доступа к нему.
    public RunState State { get; private set; } = RunState.Running;
    // Объявляет свойство признак получения клада `TreasureClaimed` и правила доступа к нему.
    public bool TreasureClaimed { get; private set; }
    // Объявляет свойство признак ожидания подтверждения победы `AwaitingTreasureAck` и правила доступа к нему.
    public bool AwaitingTreasureAck { get; private set; }
    // Объявляет событие `StateChanged` для подписчиков.
    public event Action<RunState> StateChanged;

    // Инициализирует компонент при пробуждении объекта Unity.
    void Awake()
    {  // Начало метода `Awake`.
        // Запоминает экземпляр контроллера текущей игры через `Touch`.
        Touch();
    }  // Конец метода `Awake`.

    // Запоминает экземпляр контроллера текущей игры.
    public void Touch()
    {  // Начало метода `Touch`.
        // Задаёт `Instance` значением текущий экземпляр (`this`).
        Instance = this;
    }  // Конец метода `Touch`.

    // Переносит в контроллер признак уже найденного клада.
    public void ApplySaveFlags(bool treasureTaken)
    {  // Начало метода `ApplySaveFlags`.
        // Проверяет условие `treasureTaken`.
        if (treasureTaken)
            // Устанавливает признак получения клада `TreasureClaimed` в `true`.
            TreasureClaimed = true;
    }  // Конец метода `ApplySaveFlags`.

    // Фиксирует победу, приостанавливает мир и показывает курсор.
    public void Win()
    {  // Начало метода `Win`.
        // Проверяет условие `TreasureClaimed`.
        if (TreasureClaimed)
            // Завершает выполнение текущего метода.
            return;
        // Устанавливает признак получения клада `TreasureClaimed` в `true`.
        TreasureClaimed = true;
        // Устанавливает признак ожидания подтверждения победы `AwaitingTreasureAck` в `true`.
        AwaitingTreasureAck = true;
        // Задаёт `State` значением состояние победы `RunState.Won`.
        State = RunState.Won;
        // При наличии `StateChanged` вызывает `Invoke(State)`.
        StateChanged?.Invoke(State);
        // Останавливает ход игрового времени.
        Time.timeScale = 0f;
        // Освобождает курсор для работы с интерфейсом.
        Cursor.lockState = CursorLockMode.None;
        // Показывает курсор.
        Cursor.visible = true;
    }  // Конец метода `Win`.

    // Снимает паузу после подтверждения победы.
    public void ContinueAfterWin()
    {  // Начало метода `ContinueAfterWin`.
        // Проверяет условие `!AwaitingTreasureAck`.
        if (!AwaitingTreasureAck)
            // Завершает выполнение текущего метода.
            return;
        // Устанавливает признак ожидания подтверждения победы `AwaitingTreasureAck` в `false`.
        AwaitingTreasureAck = false;
        // Возвращает нормальную скорость игрового времени.
        Time.timeScale = 1f;
        // Ищет любой объект типа через `UnityEngine.Object.FindAnyObjectByType<FirstPersonPlayer>` и записывает результат в `player`.
        var player = UnityEngine.Object.FindAnyObjectByType<FirstPersonPlayer>();
        // Проверяет условие `player != null`.
        if (player != null)
            // Вызывает `player.RelockForPlay`.
            player.RelockForPlay();
    }  // Конец метода `ContinueAfterWin`.

    // Повторно загружает текущую сцену.
    public void Restart()
    {  // Начало метода `Restart`.
        // Возвращает нормальную скорость игрового времени.
        Time.timeScale = 1f;
        // Загружает сцену через `SceneManager.LoadScene` с аргументами `SceneManager.GetActiveScene().buildIndex`.
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }  // Конец метода `Restart`.
}  // Конец класса `GameLoop`.
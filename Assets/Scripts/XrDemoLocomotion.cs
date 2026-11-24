// Подключает доступ к базовым типам игрового движка.
using UnityEngine;
// Подключает доступ к вводу с клавиатуры, мыши и других устройств.
using UnityEngine.InputSystem;
// Подключает доступ к типам из `Unity.XR.CoreUtils`.
using Unity.XR.CoreUtils;

/// <summary>
/// Учебный обход для ПР5: симулятор двигает «устройства», а не всегда заметно ползёт риг.
/// <b>Left Shift + WASD</b> — сдвиг всего XR Origin по полу.
/// </summary>
[DefaultExecutionOrder(100)]  // Задаёт порядок выполнения компонента относительно других компонентов Unity.
// Добавляет учебное перемещение XR Origin по полу с клавиатуры.
public class XrDemoLocomotion : MonoBehaviour
{  // Начало класса `XrDemoLocomotion`.
    // Задаёт `moveSpeed` значением `2.4`.
    public float moveSpeed = 2.4f;
    // Хранит поле типа `XROrigin` в `origin`.
    XROrigin origin;

    // Находит XR Origin для перемещения.
    void Awake()
    {  // Начало метода `Awake`.
        // Ищет любой объект типа через `FindAnyObjectByType<XROrigin>` и записывает результат в `origin`.
        origin = FindAnyObjectByType<XROrigin>();
    }  // Конец метода `Awake`.

    // Передвигает XR Origin при удержании Shift и нажатии WASD.
    void Update()
    {  // Начало метода `Update`.
        // Задаёт `keyboard` значением текущая клавиатура `Keyboard.current`.
        var keyboard = Keyboard.current;
        // Проверяет условие `keyboard == null || origin == null`.
        if (keyboard == null || origin == null)
            // Завершает выполнение текущего метода.
            return;
        // Проверяет условие `!keyboard.leftShiftKey.isPressed`.
        if (!keyboard.leftShiftKey.isPressed)
            // Завершает выполнение текущего метода.
            return;

        // Задаёт `x` значением `0`.
        float x = 0f;
        // Задаёт `z` значением `0`.
        float z = 0f;
        // Проверяет условие `keyboard.wKey.isPressed`.
        if (keyboard.wKey.isPressed)
            // Добавляет `1f` к текущему значению `z`.
            z += 1f;
        // Проверяет условие `keyboard.sKey.isPressed`.
        if (keyboard.sKey.isPressed)
            // Уменьшает `z` на `1f`.
            z -= 1f;
        // Проверяет условие `keyboard.aKey.isPressed`.
        if (keyboard.aKey.isPressed)
            // Уменьшает `x` на `1f`.
            x -= 1f;
        // Проверяет условие `keyboard.dKey.isPressed`.
        if (keyboard.dKey.isPressed)
            // Добавляет `1f` к текущему значению `x`.
            x += 1f;
        // Проверяет условие `Mathf.Approximately(x, 0f) && Mathf.Approximately(z, 0f)`.
        if (Mathf.Approximately(x, 0f) && Mathf.Approximately(z, 0f))
            // Завершает выполнение текущего метода.
            return;

        // Выбирает значение для `basis` по условию `origin.Camera` существует.
        Transform basis = origin.Camera != null ? origin.Camera.transform : origin.transform;
        // Задаёт `forward` значением `basis.forward`.
        var forward = basis.forward;
        // Задаёт `forward.y` значением `0`.
        forward.y = 0f;
        // Проверяет условие `forward.sqrMagnitude < 0.001f`.
        if (forward.sqrMagnitude < 0.001f)
            // Задаёт `forward` значением `basis.forward`.
            forward = basis.forward;
        // Нормализует вектор через `forward.Normalize`.
        forward.Normalize();
        // Вызывает `Vector3.Cross` с аргументами `Vector3.up, forward` и записывает результат в `right`.
        var right = Vector3.Cross(Vector3.up, forward);

        // Добавляет `(forward * z + right * x) * (moveSpeed * Time.deltaTime)` к текущему значению мировая позиция `origin.transform.position`.
        origin.transform.position += (forward * z + right * x) * (moveSpeed * Time.deltaTime);
    }  // Конец метода `Update`.
}  // Конец класса `XrDemoLocomotion`.
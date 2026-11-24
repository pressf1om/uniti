// Подключает доступ к базовым типам игрового движка.
using UnityEngine;
// Подключает доступ к обработке событий интерфейса.
using UnityEngine.EventSystems;
// Подключает доступ к привязке новой системы ввода к интерфейсу.
using UnityEngine.InputSystem.UI;
// Подключает доступ к типам из `Unity.XR.CoreUtils`.
using Unity.XR.CoreUtils;
// Подключает доступ к типам из `UnityEngine.XR.Interaction.Toolkit.Inputs.Simulation`.
using UnityEngine.XR.Interaction.Toolkit.Inputs.Simulation;

/// <summary>
/// В XrDemo: одна камера — на шлеме XR Origin; лишняя Main Camera отключается.
/// </summary>
public class XrDemoEnsure : MonoBehaviour  // Проверяет, что в учебной XR-сцене работает камера шлема.
{  // Начало класса `XrDemoEnsure`.
    // Подготавливает камеру и перемещение учебного XR-рига.
    void Awake()
    {  // Начало метода `Awake`.
        // Проверяет условие `GetComponent<XrDemoLocomotion>() == null`.
        if (GetComponent<XrDemoLocomotion>() == null)
            // Добавляет компонент через `gameObject.AddComponent<XrDemoLocomotion>`.
            gameObject.AddComponent<XrDemoLocomotion>();

        // Освобождает курсор для работы с интерфейсом.
        Cursor.lockState = CursorLockMode.None;
        // Показывает курсор.
        Cursor.visible = true;

        // Оставляет активной камеру XR Origin и проверяет слушатель звука через `BindHmdCameraOnly`.
        BindHmdCameraOnly();

        // Проверяет условие `FindAnyObjectByType<EventSystem>() == null`.
        if (FindAnyObjectByType<EventSystem>() == null)
        {  // Начало ветки условия.
            // Создаёт объект сцены с параметрами `"EventSystem"` и сохраняет ссылку в `events`.
            var events = new GameObject("EventSystem");
            // Добавляет компонент через `events.AddComponent<EventSystem>`.
            events.AddComponent<EventSystem>();
            // Добавляет компонент через `events.AddComponent<InputSystemUIInputModule>`.
            events.AddComponent<InputSystemUIInputModule>();
        }  // Конец ветки условия.

        // Проверяет условие `XRInteractionSimulator.instance == null`.
        if (XRInteractionSimulator.instance == null)
            // Пишет предупреждение в консоль Unity через `Debug.LogWarning` с аргументами `"XR Interaction Simulator не найден. Проверьте префаб в сцене или Project Settings → XR Interaction Toolkit."`.
            Debug.LogWarning("XR Interaction Simulator не найден. Проверьте префаб в сцене или Project Settings → XR Interaction Toolkit.");
    }  // Конец метода `Awake`.

    // Оставляет активной камеру XR Origin и проверяет слушатель звука.
    static void BindHmdCameraOnly()
    {  // Начало метода `BindHmdCameraOnly`.
        // Ищет любой объект типа через `FindAnyObjectByType<XROrigin>` и записывает результат в `origin`.
        var origin = FindAnyObjectByType<XROrigin>();
        // Проверяет условие `origin == null || origin.Camera == null`.
        if (origin == null || origin.Camera == null)
            // Завершает выполнение текущего метода.
            return;

        // Задаёт `hmd` значением `origin.Camera`.
        var hmd = origin.Camera;
        // Задаёт тег `hmd.tag` значением `"MainCamera"`.
        hmd.tag = "MainCamera";
        // Меняет активность объекта через `hmd.gameObject.SetActive` с аргументами `true`.
        hmd.gameObject.SetActive(true);
        // Проверяет условие `hmd.GetComponent<AudioListener>() == null`.
        if (hmd.GetComponent<AudioListener>() == null)
            // Добавляет компонент через `hmd.gameObject.AddComponent<AudioListener>`.
            hmd.gameObject.AddComponent<AudioListener>();

        // Собирает объекты типа через `FindObjectsByType<Camera>` с аргументами `FindObjectsInactive.Include, FindObjectsSortMode.None` и записывает результат в `cameras`.
        var cameras = FindObjectsByType<Camera>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        // Перебирает индексы начиная с `int i = 0` при условии `i < cameras.Length`.
        for (int i = 0; i < cameras.Length; i++)
        {  // Начало цикла по индексам.
            // Задаёт `cam` значением элемент `cameras` с индексом `i`.
            var cam = cameras[i];
            // Проверяет условие `cam == hmd`.
            if (cam == hmd)
                // Пропускает остаток этой итерации и переходит к следующей.
                continue;
            // Проверяет условие `cam.transform.IsChildOf(origin.transform)`.
            if (cam.transform.IsChildOf(origin.transform))
                // Пропускает остаток этой итерации и переходит к следующей.
                continue;
            // Устанавливает активность компонента `cam.enabled` в `false`.
            cam.enabled = false;
            // Ищет компонент через `cam.GetComponent<AudioListener>` и записывает результат в `listener`.
            var listener = cam.GetComponent<AudioListener>();
            // Проверяет условие `listener != null`.
            if (listener != null)
                // Устанавливает активность компонента `listener.enabled` в `false`.
                listener.enabled = false;
        }  // Конец цикла по индексам.
    }  // Конец метода `BindHmdCameraOnly`.
}  // Конец класса `XrDemoEnsure`.
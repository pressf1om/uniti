// Включает этот раздел при компиляции для редактора Unity.
#if UNITY_EDITOR
// Подключает доступ к чтению и записи файлов.
using System.IO;
// Подключает доступ к операциям выборки и фильтрации коллекций.
using System.Linq;
// Подключает доступ к инструментам редактора Unity.
using UnityEditor;
// Подключает доступ к типам из `UnityEditor.PackageManager.UI`.
using UnityEditor.PackageManager.UI;
// Подключает доступ к редактированию и сохранению сцен Unity.
using UnityEditor.SceneManagement;
// Подключает доступ к базовым типам игрового движка.
using UnityEngine;
// Подключает доступ к типам из `UnityEngine.XR.Interaction.Toolkit`.
using UnityEngine.XR.Interaction.Toolkit;
// Подключает доступ к типам из `UnityEngine.XR.Interaction.Toolkit.Interactables`.
using UnityEngine.XR.Interaction.Toolkit.Interactables;

/// <summary>
/// Отдельная учебная сцена для ПР5 (XR Device / Interaction Simulator). Не связана с двором.
/// </summary>
public static class XrPr5DemoSetup  // Собирает отдельную учебную сцену с симулятором XR.
{  // Начало класса `XrPr5DemoSetup`.
    // Задаёт `ScenePath` значением `"Assets/Scenes/XrDemo.unity"`.
    const string ScenePath = "Assets/Scenes/XrDemo.unity";
    // Задаёт `PackageId` значением `"com.unity.xr.interaction.toolkit"`.
    const string PackageId = "com.unity.xr.interaction.toolkit";

    // Добавляет команду редактора Unity `"Treasure Hunting/Создать XR-демо (ПР5)"`.
    [MenuItem("Treasure Hunting/Создать XR-демо (ПР5)")]
    // Запускает подготовку XR-демо из меню Unity.
    public static void CreateFromMenu()
    {  // Начало метода `CreateFromMenu`.
        // Проверяет условие `!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()`.
        if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            // Завершает выполнение текущего метода.
            return;
        // Импортирует нужные примеры и сохраняет собранную XR-сцену через `Create`.
        Create();
    }  // Конец метода `CreateFromMenu`.

    /// <summary>Для batchmode: unity command script eval 'XrPr5DemoSetup.CreateBatch();'</summary>
    public static void CreateBatch()  // Подготавливает XR-демо при вызове из пакетного режима Unity.
    {  // Начало метода `CreateBatch`.
        // Импортирует нужные примеры и сохраняет собранную XR-сцену через `Create`.
        Create();
        // Вызывает `EditorApplication.Exit` с аргументами `0`.
        EditorApplication.Exit(0);
    }  // Конец метода `CreateBatch`.

    // Запускает пересоздание учебной сцены.
    public static void RecreateScene()
    {  // Начало метода `RecreateScene`.
        // Импортирует нужные примеры и сохраняет собранную XR-сцену через `Create`.
        Create();
    }  // Конец метода `RecreateScene`.

    // Импортирует нужные примеры и сохраняет собранную XR-сцену.
    static void Create()
    {  // Начало метода `Create`.
        // Импортирует запрошенный образец XR Interaction Toolkit при необходимости через `ImportSampleIfNeeded` с аргументами `"Starter Assets"`.
        ImportSampleIfNeeded("Starter Assets");
        // Импортирует запрошенный образец XR Interaction Toolkit при необходимости через `ImportSampleIfNeeded` с аргументами `"XR Interaction Simulator"`.
        ImportSampleIfNeeded("XR Interaction Simulator");
        // Включает настройки симулятора устройств XR через `EnableSimulatorSettings`.
        EnableSimulatorSettings();

        // Находит папку установленной версии примеров XR через `FindSampleVersionRoot` и записывает результат в `sampleVersionRoot`.
        var sampleVersionRoot = FindSampleVersionRoot();
        // Проверяет условие `sampleVersionRoot == null`.
        if (sampleVersionRoot == null)
        {  // Начало ветки условия.
            // Пишет ошибку в консоль Unity через `Debug.LogError` с аргументами `"Не найдена папка Assets/Samples/XR Interaction Toolkit после импорта сэмплов."`.
            Debug.LogError("Не найдена папка Assets/Samples/XR Interaction Toolkit после импорта сэмплов.");
            // Завершает выполнение текущего метода.
            return;
        }  // Конец ветки условия.

        // Вызывает `EditorSceneManager.NewScene` с аргументами `NewSceneSetup.EmptyScene, NewSceneMode.Single`.
        EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

        // Создаёт объект сцены с параметрами `"Солнце"` и сохраняет ссылку в `lightGo`.
        var lightGo = new GameObject("Солнце");
        // Добавляет компонент через `lightGo.AddComponent<Light>` и записывает результат в `sun`.
        var sun = lightGo.AddComponent<Light>();
        // Задаёт `sun.type` значением `LightType.Directional`.
        sun.type = LightType.Directional;
        // Задаёт `sun.intensity` значением `1.1`.
        sun.intensity = 1.1f;
        // Задаёт цвет `sun.color` значением белый цвет `Color.white`.
        sun.color = Color.white;
        // Создаёт поворот из углов Эйлера через `Quaternion.Euler` с аргументами `50f, -30f, 0f` и записывает результат в `lightGo.transform.rotation`.
        lightGo.transform.rotation = Quaternion.Euler(50f, -30f, 0f);

        // Задаёт `originPath` значением `$"{sampleVersionRoot}/Starter Assets/Prefabs/XR Origin (XR Rig).prefab"`.
        var originPath = $"{sampleVersionRoot}/Starter Assets/Prefabs/XR Origin (XR Rig).prefab";
        // Загружает ассет редактора через `AssetDatabase.LoadAssetAtPath<GameObject>` с аргументами `originPath` и записывает результат в `originPrefab`.
        var originPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(originPath);
        // Проверяет условие `originPrefab == null`.
        if (originPrefab == null)
        {  // Начало ветки условия.
            // Пишет ошибку в консоль Unity через `Debug.LogError` с аргументами `"Нет префаба XR Origin: " + originPath`.
            Debug.LogError("Нет префаба XR Origin: " + originPath);
            // Завершает выполнение текущего метода.
            return;
        }  // Конец ветки условия.

        // Вызывает `PrefabUtility.InstantiatePrefab` с аргументами `originPrefab`.
        PrefabUtility.InstantiatePrefab(originPrefab);
        // Вызывает `Object.FindAnyObjectByType<Unity.XR.CoreUtils.XROrigin>` и записывает результат в `originInstance`.
        var originInstance = Object.FindAnyObjectByType<Unity.XR.CoreUtils.XROrigin>();
        // Проверяет условие `originInstance != null`.
        if (originInstance != null)
            // Записывает в `originInstance.transform.position` трёхмерный вектор с параметрами `0f, 0f, -0.35f`.
            originInstance.transform.position = new Vector3(0f, 0f, -0.35f);

        // Проверяет условие `Object.FindAnyObjectByType<XRInteractionManager>() == null`.
        if (Object.FindAnyObjectByType<XRInteractionManager>() == null)
        {  // Начало ветки условия.
            // Создаёт объект сцены с параметрами `"XR Interaction Manager"` и сохраняет ссылку в `mgr`.
            var mgr = new GameObject("XR Interaction Manager");
            // Добавляет компонент через `mgr.AddComponent<XRInteractionManager>`.
            mgr.AddComponent<XRInteractionManager>();
        }  // Конец ветки условия.

        // Настраивает камеру XR Origin при создании сцены через `BindHmdCameraOnly`.
        BindHmdCameraOnly();

        // Создаёт стандартный примитив через `GameObject.CreatePrimitive` с аргументами `PrimitiveType.Plane` и записывает результат в `plane`.
        var plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
        // Задаёт имя `plane.name` значением `"Пол"`.
        plane.name = "Пол";
        // Записывает в `plane.transform.localScale` трёхмерный вектор с параметрами `2.5f, 1f, 2.5f`.
        plane.transform.localScale = new Vector3(2.5f, 1f, 2.5f);

        // Создаёт стандартный примитив через `GameObject.CreatePrimitive` с аргументами `PrimitiveType.Cube` и записывает результат в `cube`.
        var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        // Задаёт имя `cube.name` значением `"Куб (схватить)"`.
        cube.name = "Куб (схватить)";
        // Записывает в `cube.transform.position` трёхмерный вектор с параметрами `0f, 1.08f, 0.72f`.
        cube.transform.position = new Vector3(0f, 1.08f, 0.72f);
        // Задаёт локальный масштаб `cube.transform.localScale` значением вектор (1, 1, 1) `Vector3.one` умножить на `0.32`.
        cube.transform.localScale = Vector3.one * 0.32f;
        // Добавляет компонент через `cube.AddComponent<Rigidbody>` и записывает результат в `rb`.
        var rb = cube.AddComponent<Rigidbody>();
        // Задаёт масса `rb.mass` значением `0.4`.
        rb.mass = 0.4f;
        // Добавляет компонент через `cube.AddComponent<XRGrabInteractable>` и записывает результат в `grab`.
        var grab = cube.AddComponent<XRGrabInteractable>();
        // Задаёт `grab.movementType` значением `XRBaseInteractable.MovementType.VelocityTracking`.
        grab.movementType = XRBaseInteractable.MovementType.VelocityTracking;

        // Ищет префаб симулятора среди импортированных примеров через `FindSimulatorPrefab` с аргументами `sampleVersionRoot` и записывает результат в `simPrefab`.
        var simPrefab = FindSimulatorPrefab(sampleVersionRoot);
        // Проверяет условие `simPrefab != null`.
        if (simPrefab != null)
            // Вызывает `PrefabUtility.InstantiatePrefab` с аргументами `simPrefab`.
            PrefabUtility.InstantiatePrefab(simPrefab);
        // Обрабатывает случай, когда предыдущее условие ложно.
        else
            // Пишет предупреждение в консоль Unity через `Debug.LogWarning` с аргументами `"Префаб симулятора не найден — включите симулятор в Project Settings → XR Interaction Toolkit."`.
            Debug.LogWarning("Префаб симулятора не найден — включите симулятор в Project Settings → XR Interaction Toolkit.");

        // Создаёт объект сцены с параметрами `"XrDemoEnsure"` и сохраняет ссылку в `bootstrap`.
        var bootstrap = new GameObject("XrDemoEnsure");
        // Добавляет компонент через `bootstrap.AddComponent<XrDemoEnsure>`.
        bootstrap.AddComponent<XrDemoEnsure>();

        // Добавляет систему событий для учебной сцены через `EnsureEventSystem`.
        EnsureEventSystem();
        // Добавляет экранную подсказку по управлению симулятором через `EnsureHintsCanvas`.
        EnsureHintsCanvas();

        // Проверяет условие `!AssetDatabase.IsValidFolder("Assets/Scenes")`.
        if (!AssetDatabase.IsValidFolder("Assets/Scenes"))
            // Вызывает `AssetDatabase.CreateFolder` с аргументами `"Assets", "Scenes"`.
            AssetDatabase.CreateFolder("Assets", "Scenes");

        // Получает активную сцену через `EditorSceneManager.GetActiveScene` и записывает результат в `scene`.
        var scene = EditorSceneManager.GetActiveScene();
        // Помечает сцену как изменённую через `EditorSceneManager.MarkSceneDirty` с аргументами `scene`.
        EditorSceneManager.MarkSceneDirty(scene);
        // Сохраняет сцену через `EditorSceneManager.SaveScene` с аргументами `scene, ScenePath`.
        EditorSceneManager.SaveScene(scene, ScenePath);
        // Добавляет путь сцены и сохраняет её флаг включения в сборку через `AddToBuildSettings` с аргументами `ScenePath, enabled: false`.
        AddToBuildSettings(ScenePath, enabled: false);

        // Записывает изменённые ассеты через `AssetDatabase.SaveAssets`.
        AssetDatabase.SaveAssets();
        // Пишет сообщение в консоль Unity через `Debug.Log` с аргументами `"XR-демо (ПР5) сохранено: " + ScenePath`.
        Debug.Log("XR-демо (ПР5) сохранено: " + ScenePath);
    }  // Конец метода `Create`.

    // Настраивает камеру XR Origin при создании сцены.
    static void BindHmdCameraOnly()
    {  // Начало метода `BindHmdCameraOnly`.
        // Вызывает `Object.FindAnyObjectByType<Unity.XR.CoreUtils.XROrigin>` и записывает результат в `origin`.
        var origin = Object.FindAnyObjectByType<Unity.XR.CoreUtils.XROrigin>();
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

        // Перебирает элементы `Object.FindObjectsByType<Camera>(FindObjectsInactive.Include, FindObjectsSortMode.None)` по одному в `cam`.
        foreach (var cam in Object.FindObjectsByType<Camera>(FindObjectsInactive.Include, FindObjectsSortMode.None))
        {  // Начало цикла по элементам.
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
                // Сразу удаляет объект через `Object.DestroyImmediate` с аргументами `listener`.
                Object.DestroyImmediate(listener);
        }  // Конец цикла по элементам.
    }  // Конец метода `BindHmdCameraOnly`.

    // Включает настройки симулятора устройств XR.
    static void EnableSimulatorSettings()
    {  // Начало метода `EnableSimulatorSettings`.
        // Задаёт `settingsPath` значением `"Assets/XRI/Settings/Resources/XRDeviceSimulatorSettings.asset"`.
        const string settingsPath = "Assets/XRI/Settings/Resources/XRDeviceSimulatorSettings.asset";
        // Загружает ассет редактора через `AssetDatabase.LoadAssetAtPath<ScriptableObject>` с аргументами `settingsPath` и записывает результат в `settings`.
        var settings = AssetDatabase.LoadAssetAtPath<ScriptableObject>(settingsPath);
        // Проверяет условие `settings == null`.
        if (settings == null)
            // Завершает выполнение текущего метода.
            return;
        // Ищет префаб симулятора среди импортированных примеров через `FindSimulatorPrefab` с аргументами `FindSampleVersionRoot()` и записывает результат в `sim`.
        var sim = FindSimulatorPrefab(FindSampleVersionRoot());
        // Создаёт экземпляр `SerializedObject` с параметрами `settings` и сохраняет ссылку в `so`.
        var so = new SerializedObject(settings);
        // Устанавливает `so.FindProperty("m_AutomaticallyInstantiateSimulatorPrefab").boolValue` в `true`.
        so.FindProperty("m_AutomaticallyInstantiateSimulatorPrefab").boolValue = true;
        // Устанавливает `so.FindProperty("m_AutomaticallyInstantiateInEditorOnly").boolValue` в `true`.
        so.FindProperty("m_AutomaticallyInstantiateInEditorOnly").boolValue = true;
        // Устанавливает `so.FindProperty("m_UseClassic").boolValue` в `false`.
        so.FindProperty("m_UseClassic").boolValue = false;
        // Проверяет условие `sim != null`.
        if (sim != null)
            // Задаёт `so.FindProperty("m_SimulatorPrefab").objectReferenceValue` значением `sim`.
            so.FindProperty("m_SimulatorPrefab").objectReferenceValue = sim;
        // Вызывает `so.ApplyModifiedPropertiesWithoutUndo`.
        so.ApplyModifiedPropertiesWithoutUndo();
    }  // Конец метода `EnableSimulatorSettings`.

    // Добавляет систему событий для учебной сцены.
    static void EnsureEventSystem()
    {  // Начало метода `EnsureEventSystem`.
        // Проверяет условие `Object.FindAnyObjectByType<UnityEngine.EventSystems.EventSystem>() != null`.
        if (Object.FindAnyObjectByType<UnityEngine.EventSystems.EventSystem>() != null)
            // Завершает выполнение текущего метода.
            return;
        // Создаёт объект сцены с параметрами `"EventSystem"` и сохраняет ссылку в `events`.
        var events = new GameObject("EventSystem");
        // Вызывает `events.AddComponent<UnityEngine.EventSystems.EventSystem>`.
        events.AddComponent<UnityEngine.EventSystems.EventSystem>();
        // Вызывает `events.AddComponent<UnityEngine.InputSystem.UI.InputSystemUIInputModule>`.
        events.AddComponent<UnityEngine.InputSystem.UI.InputSystemUIInputModule>();
    }  // Конец метода `EnsureEventSystem`.

    // Импортирует запрошенный образец XR Interaction Toolkit при необходимости.
    static void ImportSampleIfNeeded(string displayName)
    {  // Начало метода `ImportSampleIfNeeded`.
        // Вызывает `Sample.FindByPackage` с аргументами `PackageId, string.Empty` и записывает результат в `samples`.
        var samples = Sample.FindByPackage(PackageId, string.Empty);
        // Перебирает элементы `samples` по одному в `sample`.
        foreach (var sample in samples)
        {  // Начало цикла по элементам.
            // Проверяет условие `sample.displayName != displayName`.
            if (sample.displayName != displayName)
                // Пропускает остаток этой итерации и переходит к следующей.
                continue;
            // Проверяет условие `!sample.isImported`.
            if (!sample.isImported)
            {  // Начало ветки условия.
                // Вызывает `sample.Import` с аргументами `Sample.ImportOptions.HideImportWindow`.
                sample.Import(Sample.ImportOptions.HideImportWindow);
                // Обновляет данные через `AssetDatabase.Refresh`.
                AssetDatabase.Refresh();
            }  // Конец ветки условия.

            // Завершает выполнение текущего метода.
            return;
        }  // Конец цикла по элементам.

        // Пишет предупреждение в консоль Unity через `Debug.LogWarning` с аргументами `"Сэмпл пакета не найден: " + displayName`.
        Debug.LogWarning("Сэмпл пакета не найден: " + displayName);
    }  // Конец метода `ImportSampleIfNeeded`.

    // Находит папку установленной версии примеров XR.
    static string FindSampleVersionRoot()
    {  // Начало метода `FindSampleVersionRoot`.
        // Задаёт `root` значением `"Assets/Samples/XR Interaction Toolkit"`.
        const string root = "Assets/Samples/XR Interaction Toolkit";
        // Проверяет условие `!Directory.Exists(root)`.
        if (!Directory.Exists(root))
            // Возвращает `null`, то есть отсутствие результата поиска.
            return null;
        // Возвращает результат вызова `Directory.GetDirectories(root).OrderByDescending(d => d).FirstOrDefault()`.
        return Directory.GetDirectories(root).OrderByDescending(d => d).FirstOrDefault();
    }  // Конец метода `FindSampleVersionRoot`.

    // Ищет префаб симулятора среди импортированных примеров.
    static GameObject FindSimulatorPrefab(string sampleVersionRoot)
    {  // Начало метода `FindSimulatorPrefab`.
        // Проверяет условие `string.IsNullOrEmpty(sampleVersionRoot)`.
        if (string.IsNullOrEmpty(sampleVersionRoot))
            // Возвращает `null`, то есть отсутствие результата поиска.
            return null;
        // Вызывает `sampleVersionRoot.Replace` с аргументами `'\\', '/'` и записывает результат в `root`.
        var root = sampleVersionRoot.Replace('\\', '/');
        // Задаёт начальные элементы массива `candidates`.
        var candidates = new[]
        {  // Начало списка элементов массива.
            // Продолжает вычисление `root` плюс `"/XR Interaction Simulator/XR Interaction Simulator.prefab"`.
            root + "/XR Interaction Simulator/XR Interaction Simulator.prefab",
            // Продолжает вычисление `root` плюс `"/XR Device Simulator/XR Device Simulator.prefab"`.
            root + "/XR Device Simulator/XR Device Simulator.prefab",
        };  // Конец списка элементов массива.
        // Перебирает элементы `candidates` по одному в `path`.
        foreach (var path in candidates)
        {  // Начало цикла по элементам.
            // Проверяет условие `!File.Exists(path)`.
            if (!File.Exists(path))
                // Пропускает остаток этой итерации и переходит к следующей.
                continue;
            // Загружает ассет редактора через `AssetDatabase.LoadAssetAtPath<GameObject>` с аргументами `path` и записывает результат в `prefab`.
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            // Проверяет условие `prefab != null`.
            if (prefab != null)
                // Возвращает `prefab`.
                return prefab;
        }  // Конец цикла по элементам.

        // Задаёт `names` массивом `new[] { "XR Interaction Simulator.prefab", "XR Device Simulator.prefab" }`.
        var names = new[] { "XR Interaction Simulator.prefab", "XR Device Simulator.prefab" };
        // Перебирает элементы `names` по одному в `name`.
        foreach (var name in names)
        {  // Начало цикла по элементам.
            // Перебирает элементы `AssetDatabase.FindAssets(Path.GetFileNameWithoutExtension(name))` по одному в `guid`.
            foreach (var guid in AssetDatabase.FindAssets(Path.GetFileNameWithoutExtension(name)))
            {  // Начало цикла по элементам.
                // Вызывает `AssetDatabase.GUIDToAssetPath` с аргументами `guid` и записывает результат в `path`.
                var path = AssetDatabase.GUIDToAssetPath(guid);
                // Проверяет условие `path.EndsWith(name, System.StringComparison.OrdinalIgnoreCase) && path.Replace('\\', '/').Contains("/XR Interaction Toolkit/")`.
                if (path.EndsWith(name, System.StringComparison.OrdinalIgnoreCase)
                    // Дополняет проверку условием `path.Replace('\\', '/').Contains("/XR Interaction Toolkit/"))`.
                    && path.Replace('\\', '/').Contains("/XR Interaction Toolkit/"))
                    // Возвращает результат вызова `AssetDatabase.LoadAssetAtPath<GameObject>(path)`.
                    return AssetDatabase.LoadAssetAtPath<GameObject>(path);
            }  // Конец цикла по элементам.
        }  // Конец цикла по элементам.

        // Возвращает `null`, то есть отсутствие результата поиска.
        return null;
    }  // Конец метода `FindSimulatorPrefab`.

    // Добавляет экранную подсказку по управлению симулятором.
    static void EnsureHintsCanvas()
    {  // Начало метода `EnsureHintsCanvas`.
        // Создаёт объект сцены с параметрами `"Подсказка ПР5"` и сохраняет ссылку в `canvasGo`.
        var canvasGo = new GameObject("Подсказка ПР5");
        // Добавляет компонент через `canvasGo.AddComponent<Canvas>` и записывает результат в `canvas`.
        var canvas = canvasGo.AddComponent<Canvas>();
        // Задаёт `canvas.renderMode` значением `RenderMode.ScreenSpaceOverlay`.
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        // Вызывает `canvasGo.AddComponent<UnityEngine.UI.CanvasScaler>`.
        canvasGo.AddComponent<UnityEngine.UI.CanvasScaler>();
        // Вызывает `canvasGo.AddComponent<UnityEngine.UI.GraphicRaycaster>`.
        canvasGo.AddComponent<UnityEngine.UI.GraphicRaycaster>();

        // Создаёт объект сцены с параметрами `"Текст"` и сохраняет ссылку в `textGo`.
        var textGo = new GameObject("Текст");
        // Привязывает к родителю через `textGo.transform.SetParent` с аргументами `canvasGo.transform, false`.
        textGo.transform.SetParent(canvasGo.transform, false);
        // Добавляет компонент через `textGo.AddComponent<RectTransform>` и записывает результат в `rect`.
        var rect = textGo.AddComponent<RectTransform>();
        // Записывает в `rect.anchorMin` двумерный вектор с параметрами `0f, 1f`.
        rect.anchorMin = new Vector2(0f, 1f);
        // Записывает в `rect.anchorMax` двумерный вектор с параметрами `1f, 1f`.
        rect.anchorMax = new Vector2(1f, 1f);
        // Записывает в `rect.pivot` двумерный вектор с параметрами `0.5f, 1f`.
        rect.pivot = new Vector2(0.5f, 1f);
        // Записывает в `rect.anchoredPosition` двумерный вектор с параметрами `0f, -12f`.
        rect.anchoredPosition = new Vector2(0f, -12f);
        // Записывает в `rect.sizeDelta` двумерный вектор с параметрами `-24f, 120f`.
        rect.sizeDelta = new Vector2(-24f, 120f);

        // Вызывает `textGo.AddComponent<UnityEngine.UI.Text>` и записывает результат в `text`.
        var text = textGo.AddComponent<UnityEngine.UI.Text>();
        // Вызывает `Resources.GetBuiltinResource<Font>` с аргументами `"LegacyRuntime.ttf"` и записывает результат в `text.font`.
        text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        // Задаёт размер шрифта `text.fontSize` значением `15`.
        text.fontSize = 15;
        // Задаёт цвет `text.color` значением белый цвет `Color.white`.
        text.color = Color.white;
        // Задаёт выравнивание текста `text.alignment` значением `TextAnchor.UpperLeft`.
        text.alignment = TextAnchor.UpperLeft;
        // Записывает в `text.rectTransform.sizeDelta` двумерный вектор с параметрами `-24f, 200f`.
        text.rectTransform.sizeDelta = new Vector2(-24f, 200f);
        // Задаёт размер шрифта `text.fontSize` значением `14`.
        text.fontSize = 14;
        // Задаёт текст `text.text` значением `"ПР5 — пакеты: XR Interaction Toolkit 3.7 + XR Plugin Management.\n"` плюс `"Shift+WASD — шагать по участку (удобно для отчёта).\n"` плюс `"Симулятор справа: ] — прав. рука, T — триггер, схватить куб.\n"` плюс `"Альтернатива: H + WASD двигают шлем в симуляторе."`.
        text.text =
            // Продолжает вычисление `"ПР5 — пакеты: XR Interaction Toolkit 3.7 + XR Plugin Management.\n"` плюс `"Shift+WASD — шагать по участку (удобно для отчёта).\n"` плюс `"Симулятор справа: ] — прав. рука, T — триггер, схватить куб.\n"` плюс `"Альтернатива: H + WASD двигают шлем в симуляторе."`.
            "ПР5 — пакеты: XR Interaction Toolkit 3.7 + XR Plugin Management.\n" +
            // Добавляет строковое значение `"Shift+WASD — шагать по участку (удобно для отчёта).\n"`.
            "Shift+WASD — шагать по участку (удобно для отчёта).\n" +
            // Добавляет строковое значение `"Симулятор справа: ] — прав. рука, T — триггер, схватить куб.\n"`.
            "Симулятор справа: ] — прав. рука, T — триггер, схватить куб.\n" +
            // Добавляет строковое значение `"Альтернатива: H + WASD двигают шлем в симуляторе."`.
            "Альтернатива: H + WASD двигают шлем в симуляторе.";
    }  // Конец метода `EnsureHintsCanvas`.

    // Добавляет путь сцены и сохраняет её флаг включения в сборку.
    static void AddToBuildSettings(string scenePath, bool enabled)
    {  // Начало метода `AddToBuildSettings`.
        // Вызывает `EditorBuildSettings.scenes.ToList` и записывает результат в `scenes`.
        var scenes = EditorBuildSettings.scenes.ToList();
        // Вызывает `scenes.FindIndex` с аргументами `s => s.path == scenePath` и записывает результат в `existing`.
        var existing = scenes.FindIndex(s => s.path == scenePath);
        // Проверяет условие `existing >= 0`.
        if (existing >= 0)
            // Записывает в `scenes[existing]` описание сцены для списка сборки с параметрами `scenePath, enabled`.
            scenes[existing] = new EditorBuildSettingsScene(scenePath, enabled);
        // Обрабатывает случай, когда предыдущее условие ложно.
        else
            // Добавляет элемент через `scenes.Add` с аргументами `new EditorBuildSettingsScene(scenePath, enabled)`.
            scenes.Add(new EditorBuildSettingsScene(scenePath, enabled));
        // Копирует элементы в массив через `scenes.ToArray` и записывает результат в `EditorBuildSettings.scenes`.
        EditorBuildSettings.scenes = scenes.ToArray();
    }  // Конец метода `AddToBuildSettings`.
}  // Конец класса `XrPr5DemoSetup`.
// Заканчивает условный раздел для редактора Unity.
#endif
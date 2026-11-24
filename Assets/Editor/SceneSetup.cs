// Подключает доступ к инструментам редактора Unity.
using UnityEditor;
// Подключает доступ к редактированию и сохранению сцен Unity.
using UnityEditor.SceneManagement;
// Подключает доступ к базовым типам игрового движка.
using UnityEngine;

// Готовит сцену игры и устанавливает на неё нужные компоненты.
public static class SceneSetup
{  // Начало класса `SceneSetup`.
    // Добавляет команду редактора Unity `"GameObject/Treasure Hunting/Запечь двор в сцену"`.
    [MenuItem("GameObject/Treasure Hunting/Запечь двор в сцену", false, 49)]
    // Вызывает запекание двора через пункт меню.
    static void BakeYardMenu()
    {  // Начало метода `BakeYardMenu`.
        // Вызывает `YardSceneBake.BakeFromMenu`.
        YardSceneBake.BakeFromMenu();
    }  // Конец метода `BakeYardMenu`.

    // Устанавливает игровые компоненты и подготавливает сцену.
    public static void Install()
    {  // Начало метода `Install`.
        // Проверяет условие `Object.FindAnyObjectByType<YardBootstrap>() == null`.
        if (Object.FindAnyObjectByType<YardBootstrap>() == null)
        {  // Начало ветки условия.
            // Создаёт объект сцены с параметрами `"Game"` и сохраняет ссылку в `game`.
            var game = new GameObject("Game");
            // Добавляет компонент через `game.AddComponent<GameLoop>`.
            game.AddComponent<GameLoop>();
            // Добавляет компонент через `game.AddComponent<YardBootstrap>`.
            game.AddComponent<YardBootstrap>();
        }  // Конец ветки условия.

        // Ищет любой объект типа через `Object.FindAnyObjectByType<Light>` и записывает результат в `light`.
        var light = Object.FindAnyObjectByType<Light>();
        // Проверяет условие `light != null`.
        if (light != null)
        {  // Начало ветки условия.
            // Задаёт `light.type` значением `LightType.Directional`.
            light.type = LightType.Directional;
            // Задаёт цвет `light.color` значением `Palette.Sun`.
            light.color = Palette.Sun;
            // Задаёт `light.intensity` значением `1.15`.
            light.intensity = 1.15f;
            // Создаёт поворот из углов Эйлера через `Quaternion.Euler` с аргументами `52f, -28f, 0f` и записывает результат в `light.transform.rotation`.
            light.transform.rotation = Quaternion.Euler(52f, -28f, 0f);
        }  // Конец ветки условия.

        // Получает активную сцену через `EditorSceneManager.GetActiveScene` и записывает результат в `scene`.
        var scene = EditorSceneManager.GetActiveScene();
        // Помечает сцену как изменённую через `EditorSceneManager.MarkSceneDirty` с аргументами `scene`.
        EditorSceneManager.MarkSceneDirty(scene);
        // Сохраняет сцену через `EditorSceneManager.SaveScene` с аргументами `scene`.
        EditorSceneManager.SaveScene(scene);
    }  // Конец метода `Install`.

    // Приводит сцену двора к ожидаемой структуре перед работой.
    public static void Prepare()
    {  // Начало метода `Prepare`.
        // Проверяет условие `!AssetDatabase.IsValidFolder("Assets/Resources")`.
        if (!AssetDatabase.IsValidFolder("Assets/Resources"))
            // Вызывает `AssetDatabase.CreateFolder` с аргументами `"Assets", "Resources"`.
            AssetDatabase.CreateFolder("Assets", "Resources");
        // Проверяет условие `!AssetDatabase.IsValidFolder("Assets/Animations")`.
        if (!AssetDatabase.IsValidFolder("Assets/Animations"))
            // Вызывает `AssetDatabase.CreateFolder` с аргументами `"Assets", "Animations"`.
            AssetDatabase.CreateFolder("Assets", "Animations");

        // Задаёт `clipPath` значением `"Assets/Animations/SellerBob.anim"`.
        const string clipPath = "Assets/Animations/SellerBob.anim";
        // Задаёт `controllerPath` значением `"Assets/Resources/SellerIdle.controller"`.
        const string controllerPath = "Assets/Resources/SellerIdle.controller";
        // Проверяет условие `AssetDatabase.LoadAssetAtPath<AnimationClip>(clipPath) == null`.
        if (AssetDatabase.LoadAssetAtPath<AnimationClip>(clipPath) == null)
        {  // Начало ветки условия.
            // Создаёт экземпляр `AnimationClip` и сохраняет ссылку в `clip`.
            var clip = new AnimationClip { name = "SellerBob" };
            // Создаёт экземпляр `AnimationCurve` с параметрами ` new Keyframe(0f, 0.9f), new Keyframe(0.45f, 0.98f), new Keyframe(0.9f, 0.9f)` и сохраняет ссылку в `curve`.
            var curve = new AnimationCurve(
                // Создаёт экземпляр `Keyframe` с параметрами `0f, 0.9f`.
                new Keyframe(0f, 0.9f),
                // Создаёт экземпляр `Keyframe` с параметрами `0.45f, 0.98f`.
                new Keyframe(0.45f, 0.98f),
                // Создаёт экземпляр `Keyframe` с параметрами `0.9f, 0.9f`.
                new Keyframe(0.9f, 0.9f));
            // Вызывает `clip.SetCurve` с аргументами `"Body", typeof(Transform), "localPosition.y", curve`.
            clip.SetCurve("Body", typeof(Transform), "localPosition.y", curve);
            // Вызывает `AnimationUtility.GetAnimationClipSettings` с аргументами `clip` и записывает результат в `settings`.
            var settings = AnimationUtility.GetAnimationClipSettings(clip);
            // Устанавливает `settings.loopTime` в `true`.
            settings.loopTime = true;
            // Вызывает `AnimationUtility.SetAnimationClipSettings` с аргументами `clip, settings`.
            AnimationUtility.SetAnimationClipSettings(clip, settings);
            // Вызывает `AssetDatabase.CreateAsset` с аргументами `clip, clipPath`.
            AssetDatabase.CreateAsset(clip, clipPath);
        }  // Конец ветки условия.

        // Проверяет условие `AssetDatabase.LoadAssetAtPath<RuntimeAnimatorController>(controllerPath) == null`.
        if (AssetDatabase.LoadAssetAtPath<RuntimeAnimatorController>(controllerPath) == null)
        {  // Начало ветки условия.
            // Вызывает `UnityEditor.Animations.AnimatorController.CreateAnimatorControllerAtPath` с аргументами `controllerPath` и записывает результат в `controller`.
            var controller = UnityEditor.Animations.AnimatorController.CreateAnimatorControllerAtPath(controllerPath);
            // Загружает ассет редактора через `AssetDatabase.LoadAssetAtPath<AnimationClip>` с аргументами `clipPath` и записывает результат в `clip`.
            var clip = AssetDatabase.LoadAssetAtPath<AnimationClip>(clipPath);
            // Задаёт `stateMachine` значением `controller.layers[0].stateMachine`.
            var stateMachine = controller.layers[0].stateMachine;
            // Вызывает `stateMachine.AddState` с аргументами `"Шаг"` и записывает результат в `state`.
            var state = stateMachine.AddState("Шаг");
            // Задаёт `state.motion` значением `clip`.
            state.motion = clip;
            // Задаёт `stateMachine.defaultState` значением `state`.
            stateMachine.defaultState = state;
        }  // Конец ветки условия.

        // Задаёт `menuPath` значением `"Assets/Scenes/MainMenu.unity"`.
        const string menuPath = "Assets/Scenes/MainMenu.unity";
        // Задаёт `current` значением `EditorSceneManager.GetActiveScene().path`.
        string current = EditorSceneManager.GetActiveScene().path;
        // Проверяет условие `EditorSceneManager.GetActiveScene().isDirty`.
        if (EditorSceneManager.GetActiveScene().isDirty)
            // Сохраняет сцену через `EditorSceneManager.SaveScene` с аргументами `EditorSceneManager.GetActiveScene()`.
            EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
        // Проверяет условие `string.IsNullOrEmpty(AssetDatabase.AssetPathToGUID(menuPath))`.
        if (string.IsNullOrEmpty(AssetDatabase.AssetPathToGUID(menuPath)))
        {  // Начало ветки условия.
            // Вызывает `EditorSceneManager.NewScene` с аргументами `NewSceneSetup.EmptyScene, NewSceneMode.Single` и записывает результат в `menu`.
            var menu = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            // Создаёт объект сцены с параметрами `"Меню"` и сохраняет ссылку в `root`.
            var root = new GameObject("Меню");
            // Добавляет компонент через `root.AddComponent<MenuController>`.
            root.AddComponent<MenuController>();
            // Создаёт объект сцены с параметрами `"Солнце"` и сохраняет ссылку в `lightGo`.
            var lightGo = new GameObject("Солнце");
            // Добавляет компонент через `lightGo.AddComponent<Light>` и записывает результат в `sun`.
            var sun = lightGo.AddComponent<Light>();
            // Задаёт `sun.type` значением `LightType.Directional`.
            sun.type = LightType.Directional;
            // Задаёт цвет `sun.color` значением `Palette.Sun`.
            sun.color = Palette.Sun;
            // Задаёт `sun.intensity` значением `1.1`.
            sun.intensity = 1.1f;
            // Создаёт поворот из углов Эйлера через `Quaternion.Euler` с аргументами `48f, -24f, 0f` и записывает результат в `lightGo.transform.rotation`.
            lightGo.transform.rotation = Quaternion.Euler(48f, -24f, 0f);
            // Создаёт объект сцены с параметрами `"Камера"` и сохраняет ссылку в `camGo`.
            var camGo = new GameObject("Камера");
            // Добавляет компонент через `camGo.AddComponent<Camera>` и записывает результат в `camera`.
            var camera = camGo.AddComponent<Camera>();
            // Задаёт режим очистки фона камеры `camera.clearFlags` значением `CameraClearFlags.SolidColor`.
            camera.clearFlags = CameraClearFlags.SolidColor;
            // Записывает в `camera.backgroundColor` цвет RGBA с параметрами `0.62f, 0.78f, 0.92f`.
            camera.backgroundColor = new Color(0.62f, 0.78f, 0.92f);
            // Записывает в `camGo.transform.position` трёхмерный вектор с параметрами `2.2f, 1.7f, -6f`.
            camGo.transform.position = new Vector3(2.2f, 1.7f, -6f);
            // Создаёт поворот из углов Эйлера через `Quaternion.Euler` с аргументами `8f, -12f, 0f` и записывает результат в `camGo.transform.rotation`.
            camGo.transform.rotation = Quaternion.Euler(8f, -12f, 0f);
            // Добавляет компонент через `camGo.AddComponent<AudioListener>`.
            camGo.AddComponent<AudioListener>();
            // Сохраняет сцену через `EditorSceneManager.SaveScene` с аргументами `menu, menuPath`.
            EditorSceneManager.SaveScene(menu, menuPath);
        }  // Конец ветки условия.

        // Задаёт `EditorBuildSettings.scenes` значением массив `new[] { new EditorBuildSettingsScene(menuPath, true), new EditorBuildSettingsScene("Assets/Scenes/SampleScene.unity", true) }`.
        EditorBuildSettings.scenes = new[]
        {  // Начало списка элементов массива.
            // Создаёт описание сцены для списка сборки с параметрами `menuPath, true`.
            new EditorBuildSettingsScene(menuPath, true),
            // Создаёт описание сцены для списка сборки с параметрами `"Assets/Scenes/SampleScene.unity", true`.
            new EditorBuildSettingsScene("Assets/Scenes/SampleScene.unity", true)
        };  // Конец списка элементов массива.
        // Проверяет условие `!string.IsNullOrEmpty(current)`.
        if (!string.IsNullOrEmpty(current))
            // Вызывает `EditorSceneManager.OpenScene` с аргументами `current`.
            EditorSceneManager.OpenScene(current);
        // Записывает изменённые ассеты через `AssetDatabase.SaveAssets`.
        AssetDatabase.SaveAssets();
    }  // Конец метода `Prepare`.
}  // Конец класса `SceneSetup`.

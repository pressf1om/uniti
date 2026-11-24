// Подключает доступ к компонентам навигации Unity.
using Unity.AI.Navigation;
// Подключает доступ к инструментам редактора Unity.
using UnityEditor;
// Подключает доступ к редактированию и сохранению сцен Unity.
using UnityEditor.SceneManagement;
// Подключает доступ к базовым типам игрового движка.
using UnityEngine;
// Подключает доступ к типам из `UnityEngine.AI`.
using UnityEngine.AI;

// Запекает окружение двора и разметку в редактируемую сцену.
public static class YardSceneBake
{  // Начало класса `YardSceneBake`.
    // Задаёт `ScenePath` значением `"Assets/Scenes/SampleScene.unity"`.
    const string ScenePath = "Assets/Scenes/SampleScene.unity";

    // Запускает запекание из пакетного режима Unity.
    public static void BakeBatch()
    {  // Начало метода `BakeBatch`.
        // Собирает двор, размещает разметку, готовит навигацию и сохраняет сцену через `Bake`.
        Bake();
        // Вызывает `EditorApplication.Exit` с аргументами `0`.
        EditorApplication.Exit(0);
    }  // Конец метода `BakeBatch`.

    // Добавляет команду редактора Unity `"Treasure Hunting/Запечь двор в сцену %#&y"`.
    [MenuItem("Treasure Hunting/Запечь двор в сцену %#&y")]
    // Запускает запекание двора командой меню.
    public static void BakeFromMenu()
    {  // Начало метода `BakeFromMenu`.
        // Проверяет условие `!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()`.
        if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            // Завершает выполнение текущего метода.
            return;
        // Собирает двор, размещает разметку, готовит навигацию и сохраняет сцену через `Bake`.
        Bake();
    }  // Конец метода `BakeFromMenu`.

    // Собирает двор, размещает разметку, готовит навигацию и сохраняет сцену.
    public static void Bake()
    {  // Начало метода `Bake`.
        // Проверяет условие `!System.IO.File.Exists(ScenePath)`.
        if (!System.IO.File.Exists(ScenePath))
        {  // Начало ветки условия.
            // Пишет ошибку в консоль Unity через `Debug.LogError` с аргументами `"Не найдена сцена " + ScenePath`.
            Debug.LogError("Не найдена сцена " + ScenePath);
            // Завершает выполнение текущего метода.
            return;
        }  // Конец ветки условия.

        // Вызывает `EditorSceneManager.OpenScene` с аргументами `ScenePath, OpenSceneMode.Single` и записывает результат в `scene`.
        var scene = EditorSceneManager.OpenScene(ScenePath, OpenSceneMode.Single);
        // Ищет объект по имени через `GameObject.Find` с аргументами `"Game"` и записывает результат в `game`.
        var game = GameObject.Find("Game");
        // Проверяет условие `game == null`.
        if (game == null)
        {  // Начало ветки условия.
            // Пишет ошибку в консоль Unity через `Debug.LogError` с аргументами `"В сцене нет объекта Game."`.
            Debug.LogError("В сцене нет объекта Game.");
            // Завершает выполнение текущего метода.
            return;
        }  // Конец ветки условия.

        // Ищет объект по имени через `game.transform.Find` с аргументами `"Двор"` и записывает результат в `old`.
        var old = game.transform.Find("Двор");
        // Проверяет условие `old != null`.
        if (old != null)
            // Сразу удаляет объект через `Object.DestroyImmediate` с аргументами `old.gameObject`.
            Object.DestroyImmediate(old.gameObject);

        // Вызывает `YardLayout.Build` с аргументами `game.transform` и записывает результат в `yard`.
        var yard = YardLayout.Build(game.transform);
        // Вызывает `PropKit.FixShovelsUnder` с аргументами `yard`.
        PropKit.FixShovelsUnder(yard);
        // Создаёт точки для появления игрока и размещения продавца через `EnsureMarkers` с аргументами `game, yard`.
        EnsureMarkers(game, yard);
        // Готовит навигационную поверхность для двора через `BakeNavMesh` с аргументами `yard`.
        BakeNavMesh(yard);

        // Проверяет условие `game.GetComponent<YardBootstrap>() == null`.
        if (game.GetComponent<YardBootstrap>() == null)
            // Добавляет компонент через `game.AddComponent<YardBootstrap>`.
            game.AddComponent<YardBootstrap>();
        // Проверяет условие `game.GetComponent<GameLoop>() == null`.
        if (game.GetComponent<GameLoop>() == null)
            // Добавляет компонент через `game.AddComponent<GameLoop>`.
            game.AddComponent<GameLoop>();

        // Помечает сцену как изменённую через `EditorSceneManager.MarkSceneDirty` с аргументами `scene`.
        EditorSceneManager.MarkSceneDirty(scene);
        // Сохраняет сцену через `EditorSceneManager.SaveScene` с аргументами `scene`.
        EditorSceneManager.SaveScene(scene);
        // Пишет сообщение в консоль Unity через `Debug.Log` с аргументами `"Двор записан в " + ScenePath + ". Яма по-прежнему создаётся при Play."`.
        Debug.Log("Двор записан в " + ScenePath + ". Яма по-прежнему создаётся при Play.");
    }  // Конец метода `Bake`.

    // Создаёт точки для появления игрока и размещения продавца.
    static void EnsureMarkers(GameObject game, Transform yard)
    {  // Начало метода `EnsureMarkers`.
        // Ищет компонент через `game.GetComponent<YardMarkers>` и записывает результат в `markers`.
        var markers = game.GetComponent<YardMarkers>();
        // Проверяет условие `markers == null`.
        if (markers == null)
            // Добавляет компонент через `game.AddComponent<YardMarkers>` и записывает результат в `markers`.
            markers = game.AddComponent<YardMarkers>();

        // Ищет объект по имени через `game.transform.Find` с аргументами `"Метки"` и записывает результат в `root`.
        var root = game.transform.Find("Метки");
        // Проверяет условие `root == null`.
        if (root == null)
        {  // Начало ветки условия.
            // Создаёт объект сцены с параметрами `"Метки"` и сохраняет ссылку в `go`.
            var go = new GameObject("Метки");
            // Привязывает к родителю через `go.transform.SetParent` с аргументами `game.transform, false`.
            go.transform.SetParent(game.transform, false);
            // Задаёт `root` значением `go.transform`.
            root = go.transform;
        }  // Конец ветки условия.

        // Обновляет существующую точку или создаёт новую через `EnsurePoint` с аргументами `root, "Спавн игрока", new Vector3(5f, 0.15f, -3.2f)` и записывает результат в `spawn`.
        var spawn = EnsurePoint(root, "Спавн игрока", new Vector3(5f, 0.15f, -3.2f));
        // Обновляет существующую точку или создаёт новую через `EnsurePoint` с аргументами `root, "Продавец продажа", new Vector3(-2.2f, 0f, 4.35f)` и записывает результат в `north`.
        var north = EnsurePoint(root, "Продавец продажа", new Vector3(-2.2f, 0f, 4.35f));
        // Обновляет существующую точку или создаёт новую через `EnsurePoint` с аргументами `root, "Продавец улучшения", new Vector3(-6.5f, 0f, 3.1f)` и записывает результат в `south`.
        var south = EnsurePoint(root, "Продавец улучшения", new Vector3(-6.5f, 0f, 3.1f));
        // Вызывает `markers.Wire` с аргументами `spawn, north, south`.
        markers.Wire(spawn, north, south);
        // Помечает объект редактора как изменённый через `EditorUtility.SetDirty` с аргументами `markers`.
        EditorUtility.SetDirty(markers);
    }  // Конец метода `EnsureMarkers`.

    // Обновляет существующую точку или создаёт новую.
    static Transform EnsurePoint(Transform parent, string name, Vector3 worldPos)
    {  // Начало метода `EnsurePoint`.
        // Ищет объект по имени через `parent.Find` с аргументами `name` и записывает результат в `found`.
        var found = parent.Find(name);
        // Проверяет условие `found == null`.
        if (found == null)
        {  // Начало ветки условия.
            // Создаёт объект сцены с параметрами `name` и сохраняет ссылку в `go`.
            var go = new GameObject(name);
            // Привязывает к родителю через `go.transform.SetParent` с аргументами `parent, false`.
            go.transform.SetParent(parent, false);
            // Задаёт `found` значением `go.transform`.
            found = go.transform;
        }  // Конец ветки условия.
        // Задаёт мировая позиция `found.position` значением `worldPos`.
        found.position = worldPos;
        // Возвращает `found`.
        return found;
    }  // Конец метода `EnsurePoint`.

    // Готовит навигационную поверхность для двора.
    static void BakeNavMesh(Transform yard)
    {  // Начало метода `BakeNavMesh`.
        // Ищет объект по имени через `yard.Find` с аргументами `"Навигация"` и записывает результат в `navRoot`.
        var navRoot = yard.Find("Навигация");
        // Проверяет условие `navRoot == null`.
        if (navRoot == null)
        {  // Начало ветки условия.
            // Создаёт объект сцены с параметрами `"Навигация"` и сохраняет ссылку в `go`.
            var go = new GameObject("Навигация");
            // Привязывает к родителю через `go.transform.SetParent` с аргументами `yard, false`.
            go.transform.SetParent(yard, false);
            // Задаёт `navRoot` значением `go.transform`.
            navRoot = go.transform;
        }  // Конец ветки условия.

        // Ищет компонент через `navRoot.GetComponent<NavMeshSurface>` и записывает результат в `surface`.
        var surface = navRoot.GetComponent<NavMeshSurface>();
        // Проверяет условие `surface == null`.
        if (surface == null)
            // Добавляет компонент через `navRoot.gameObject.AddComponent<NavMeshSurface>` и записывает результат в `surface`.
            surface = navRoot.gameObject.AddComponent<NavMeshSurface>();

        // Задаёт `surface.collectObjects` значением `CollectObjects.All`.
        surface.collectObjects = CollectObjects.All;
        // Задаёт `surface.useGeometry` значением `NavMeshCollectGeometry.PhysicsColliders`.
        surface.useGeometry = NavMeshCollectGeometry.PhysicsColliders;
        // Задаёт `surface.layerMask` значением `1` со сдвигом влево на `8`.
        surface.layerMask = 1 << 8;
        // Вызывает `surface.BuildNavMesh`.
        surface.BuildNavMesh();

        // Проверяет условие `!NavMesh.SamplePosition(new Vector3(-4.2f, 0.5f, 3.55f), out _, 4f, NavMesh.AllAreas)`.
        if (!NavMesh.SamplePosition(new Vector3(-4.2f, 0.5f, 3.55f), out _, 4f, NavMesh.AllAreas))
            // Пишет предупреждение в консоль Unity через `Debug.LogWarning` с аргументами `"NavMesh не покрывает гараж — проверьте слой 8 и коллайдеры."`.
            Debug.LogWarning("NavMesh не покрывает гараж — проверьте слой 8 и коллайдеры.");
    }  // Конец метода `BakeNavMesh`.
}  // Конец класса `YardSceneBake`.
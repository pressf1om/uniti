// Включает этот раздел при компиляции для редактора Unity.
#if UNITY_EDITOR
// Подключает доступ к инструментам редактора Unity.
using UnityEditor;
// Подключает доступ к базовым типам игрового движка.
using UnityEngine;

// Сохраняет модели продавца как префабы для загрузки в сборке.
public static class TraderPrefabBake
{  // Начало класса `TraderPrefabBake`.
    // Добавляет команду редактора Unity `"Treasure Hunting/Запечь модели торговца (exe)"`.
    [MenuItem("Treasure Hunting/Запечь модели торговца (exe)")]
    // Запускает запекание из меню и сообщает результат.
    public static void BakeFromMenu()
    {  // Начало метода `BakeFromMenu`.
        // Обрабатывает исходные варианты модели торговца через `Bake`.
        Bake();
    }  // Конец метода `BakeFromMenu`.

    // Unity вызывает следующий метод при загрузке редактора.
    [InitializeOnLoadMethod]
    // Планирует автоматическую подготовку префабов после загрузки редактора.
    static void AutoBakeOnLoad()
    {  // Начало метода `AutoBakeOnLoad`.
        // Добавляет `() => { if (!System.IO.File.Exists("Assets/Resources/Characters/ShopkeeperBaked.prefab")) Bake(), }` к текущему значению `EditorApplication.delayCall`.
        EditorApplication.delayCall += () =>
        {  // Начало тела обработчика.
            // Проверяет условие `!System.IO.File.Exists("Assets/Resources/Characters/ShopkeeperBaked.prefab")`.
            if (!System.IO.File.Exists("Assets/Resources/Characters/ShopkeeperBaked.prefab"))
                // Обрабатывает исходные варианты модели торговца через `Bake`.
                Bake();
        };  // Конец тела обработчика.
    }  // Конец метода `AutoBakeOnLoad`.

    // Обрабатывает исходные варианты модели торговца.
    public static void Bake()
    {  // Начало метода `Bake`.
        // Проверяет условие `!BakeOne( "Assets/Resources/Characters/shopkeeper.glb", "Assets/Resources/Characters/ShopkeeperBaked.prefab")`.
        if (!BakeOne(
                // Передаёт аргумент `"Assets/Resources/Characters/shopkeeper.glb"`.
                "Assets/Resources/Characters/shopkeeper.glb",
                // Передаёт аргумент `"Assets/Resources/Characters/ShopkeeperBaked.prefab"`.
                "Assets/Resources/Characters/ShopkeeperBaked.prefab"))
            // Создаёт префаб из одной модели и возвращает признак успеха через `BakeOne` с аргументами `"Assets/Resources/Characters/vendor.glb", "Assets/Resources/Characters/VendorBaked.prefab"`.
            BakeOne("Assets/Resources/Characters/vendor.glb", "Assets/Resources/Characters/VendorBaked.prefab");
        // Обрабатывает случай, когда предыдущее условие ложно.
        else
            // Создаёт префаб из одной модели и возвращает признак успеха через `BakeOne` с аргументами `"Assets/Resources/Characters/vendor.glb", "Assets/Resources/Characters/VendorBaked.prefab"`.
            BakeOne("Assets/Resources/Characters/vendor.glb", "Assets/Resources/Characters/VendorBaked.prefab");
        // Записывает изменённые ассеты через `AssetDatabase.SaveAssets`.
        AssetDatabase.SaveAssets();
        // Обновляет данные через `AssetDatabase.Refresh`.
        AssetDatabase.Refresh();
    }  // Конец метода `Bake`.

    // Создаёт префаб из одной модели и возвращает признак успеха.
    static bool BakeOne(string sourcePath, string prefabPath)
    {  // Начало метода `BakeOne`.
        // Проверяет условие `!System.IO.File.Exists(sourcePath)`.
        if (!System.IO.File.Exists(sourcePath))
        {  // Начало ветки условия.
            // Пишет предупреждение в консоль Unity через `Debug.LogWarning` с аргументами `"Нет файла: " + sourcePath`.
            Debug.LogWarning("Нет файла: " + sourcePath);
            // Возвращает `false`.
            return false;
        }  // Конец ветки условия.

        // Загружает ассет редактора через `AssetDatabase.LoadAssetAtPath<GameObject>` с аргументами `sourcePath` и записывает результат в `source`.
        var source = AssetDatabase.LoadAssetAtPath<GameObject>(sourcePath);
        // Проверяет условие `source == null`.
        if (source == null)
        {  // Начало ветки условия.
            // Пишет ошибку в консоль Unity через `Debug.LogError` с аргументами `"Не удалось импортировать как GameObject: " + sourcePath`.
            Debug.LogError("Не удалось импортировать как GameObject: " + sourcePath);
            // Возвращает `false`.
            return false;
        }  // Конец ветки условия.

        // Задаёт `instance` значением `PrefabUtility.InstantiatePrefab(source)` как `GameObject` с `null` при несовпадении типа.
        var instance = PrefabUtility.InstantiatePrefab(source) as GameObject;
        // Проверяет условие `instance == null`.
        if (instance == null)
            // Создаёт копию объекта через `Object.Instantiate` с аргументами `source` и записывает результат в `instance`.
            instance = Object.Instantiate(source);
        // Вызывает `System.IO.Path.GetFileNameWithoutExtension` с аргументами `prefabPath` и записывает результат в `instance.name`.
        instance.name = System.IO.Path.GetFileNameWithoutExtension(prefabPath);

        // Проверяет условие `!System.IO.Directory.Exists("Assets/Resources/Characters")`.
        if (!System.IO.Directory.Exists("Assets/Resources/Characters"))
            // Вызывает `AssetDatabase.CreateFolder` с аргументами `"Assets/Resources", "Characters"`.
            AssetDatabase.CreateFolder("Assets/Resources", "Characters");

        // Записывает иерархию как префаб через `PrefabUtility.SaveAsPrefabAsset` с аргументами `instance, prefabPath`.
        PrefabUtility.SaveAsPrefabAsset(instance, prefabPath);
        // Сразу удаляет объект через `Object.DestroyImmediate` с аргументами `instance`.
        Object.DestroyImmediate(instance);
        // Пишет сообщение в консоль Unity через `Debug.Log` с аргументами `"Запечено: " + prefabPath`.
        Debug.Log("Запечено: " + prefabPath);
        // Возвращает `true`.
        return true;
    }  // Конец метода `BakeOne`.
}  // Конец класса `TraderPrefabBake`.
// Заканчивает условный раздел для редактора Unity.
#endif
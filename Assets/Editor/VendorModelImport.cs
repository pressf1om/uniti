// Подключает доступ к инструментам редактора Unity.
using UnityEditor;
// Подключает доступ к базовым типам игрового движка.
using UnityEngine;

// Unity инициализирует этот тип при загрузке редактора.
[InitializeOnLoad]
// Настраивает импорт модели продавца в редакторе.
public static class VendorModelImport
{  // Начало класса `VendorModelImport`.
    // Инициализирует редакторский класс `VendorModelImport` при первой загрузке типа.
    static VendorModelImport()
    {  // Начало метода `VendorModelImport`.
        // Добавляет `Reimport` к текущему значению `EditorApplication.delayCall`.
        EditorApplication.delayCall += Reimport;
    }  // Конец метода `VendorModelImport`.

    // Обновляет параметры импорта и переимпортирует модель при необходимости.
    static void Reimport()
    {  // Начало метода `Reimport`.
        // Перебирает элементы `new[] { "Assets/Resources/Characters/shopkeeper.glb", "Assets/Resources/Characters/vendor.glb" }` по одному в `path`.
        foreach (var path in new[]
        {  // Начало списка элементов массива.
            // Добавляет строковое значение `"Assets/Resources/Characters/shopkeeper.glb"`.
            "Assets/Resources/Characters/shopkeeper.glb",
            // Добавляет строковое значение `"Assets/Resources/Characters/vendor.glb"`.
            "Assets/Resources/Characters/vendor.glb"
        })  // Конец списка элементов массива.
        {  // Начало цикла по элементам.
            // Проверяет условие `!System.IO.File.Exists(path)`.
            if (!System.IO.File.Exists(path))
                // Пропускает остаток этой итерации и переходит к следующей.
                continue;
            // Вызывает `AssetDatabase.ImportAsset` с аргументами `path, ImportAssetOptions.ForceUpdate`.
            AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
        }  // Конец цикла по элементам.
    }  // Конец метода `Reimport`.
}  // Конец класса `VendorModelImport`.
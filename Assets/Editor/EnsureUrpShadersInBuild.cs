// Включает этот раздел при компиляции для редактора Unity.
#if UNITY_EDITOR
// Подключает доступ к инструментам редактора Unity.
using UnityEditor;
// Подключает доступ к базовым типам игрового движка.
using UnityEngine;

/// <summary>
/// Runtime materials use Shader.Find; player builds strip those shaders without a reference.
/// </summary>
[InitializeOnLoad]  // Unity инициализирует этот тип при загрузке редактора.
// Регистрирует шейдеры, которые игра ищет по имени, в настройках сборки.
static class EnsureUrpShadersInBuild
{  // Начало класса `EnsureUrpShadersInBuild`.
    // Задаёт начальные элементы массива `Shaders`.
    static readonly string[] Shaders =
    {  // Начало списка элементов массива.
        // Добавляет строковое значение `"Universal Render Pipeline/Lit"`.
        "Universal Render Pipeline/Lit",
        // Добавляет строковое значение `"Universal Render Pipeline/Particles/Unlit"`.
        "Universal Render Pipeline/Particles/Unlit",
    };  // Конец списка элементов массива.

    // Инициализирует редакторский класс `EnsureUrpShadersInBuild` при первой загрузке типа.
    static EnsureUrpShadersInBuild() => Register();

    // Добавляет команду редактора Unity `"Tools/Ensure URP shaders in build"`.
    [MenuItem("Tools/Ensure URP shaders in build")]
    // Запускает регистрацию из меню Unity.
    static void RegisterMenu() => Register();

    // Добавляет нужные URP-шейдеры в список включения в сборку.
    static void Register()
    {  // Начало метода `Register`.
        // Вызывает `AssetDatabase.LoadAllAssetsAtPath` с аргументами `"ProjectSettings/GraphicsSettings.asset"` и записывает результат в `assets`.
        var assets = AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/GraphicsSettings.asset");
        // Проверяет условие `assets == null || assets.Length == 0`.
        if (assets == null || assets.Length == 0)
            // Завершает выполнение текущего метода.
            return;
        // Создаёт экземпляр `SerializedObject` с параметрами `assets[0]` и сохраняет ссылку в `so`.
        var so = new SerializedObject(assets[0]);
        // Вызывает `so.FindProperty` с аргументами `"m_AlwaysIncludedShaders"` и записывает результат в `array`.
        var array = so.FindProperty("m_AlwaysIncludedShaders");
        // Проверяет условие `array == null`.
        if (array == null)
            // Завершает выполнение текущего метода.
            return;
        // Инициализирует `changed` значением «нет».
        var changed = false;
        // Перебирает элементы `Shaders` по одному в `name`.
        foreach (var name in Shaders)
        {  // Начало цикла по элементам.
            // Ищет шейдер по имени через `Shader.Find` с аргументами `name` и записывает результат в `shader`.
            var shader = Shader.Find(name);
            // Проверяет условие `shader == null`.
            if (shader == null)
                // Пропускает остаток этой итерации и переходит к следующей.
                continue;
            // Проверяет условие `Contains(array, shader)`.
            if (Contains(array, shader))
                // Пропускает остаток этой итерации и переходит к следующей.
                continue;
            // Вызывает `array.InsertArrayElementAtIndex` с аргументами `array.arraySize`.
            array.InsertArrayElementAtIndex(array.arraySize);
            // Задаёт `array.GetArrayElementAtIndex(array.arraySize - 1).objectReferenceValue` значением `shader`.
            array.GetArrayElementAtIndex(array.arraySize - 1).objectReferenceValue = shader;
            // Устанавливает `changed` в `true`.
            changed = true;
        }  // Конец цикла по элементам.
        // Проверяет условие `changed`.
        if (changed)
            // Вызывает `so.ApplyModifiedPropertiesWithoutUndo`.
            so.ApplyModifiedPropertiesWithoutUndo();
    }  // Конец метода `Register`.

    // Проверяет наличие шейдера в сериализованном массиве настроек.
    static bool Contains(SerializedProperty array, Shader shader)
    {  // Начало метода `Contains`.
        // Перебирает индексы начиная с `var i = 0` при условии `i < array.arraySize`.
        for (var i = 0; i < array.arraySize; i++)
        {  // Начало цикла по индексам.
            // Проверяет условие `array.GetArrayElementAtIndex(i).objectReferenceValue == shader`.
            if (array.GetArrayElementAtIndex(i).objectReferenceValue == shader)
                // Возвращает `true`.
                return true;
        }  // Конец цикла по индексам.
        // Возвращает `false`.
        return false;
    }  // Конец метода `Contains`.
}  // Конец класса `EnsureUrpShadersInBuild`.
// Заканчивает условный раздел для редактора Unity.
#endif
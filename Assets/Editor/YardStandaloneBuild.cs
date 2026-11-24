// Включает этот раздел при компиляции для редактора Unity.
#if UNITY_EDITOR
// Подключает доступ к инструментам редактора Unity.
using UnityEditor;
// Подключает доступ к типам из `UnityEditor.Build`.
using UnityEditor.Build;
// Подключает доступ к отчётам о сборке Unity.
using UnityEditor.Build.Reporting;
// Подключает доступ к базовым типам игрового движка.
using UnityEngine;

// Запускает сборку основной игры для Windows.
public static class YardStandaloneBuild
{  // Начало класса `YardStandaloneBuild`.
    // Добавляет команду редактора Unity `"Tools/Build Yard Windows"`.
    [MenuItem("Tools/Build Yard Windows")]
    // Формирует параметры Windows-сборки и запускает пайплайн Unity.
    public static void PerformBuild()
    {  // Начало метода `PerformBuild`.
        // Вызывает `TraderPrefabBake.Bake`.
        TraderPrefabBake.Bake();
        // Вызывает `GameBuildScenes.ApplyYardPlayerSettings`.
        GameBuildScenes.ApplyYardPlayerSettings();
        // Вызывает `PlayerSettings.SetScriptingBackend` с аргументами `NamedBuildTarget.Standalone, ScriptingImplementation.Mono2x`.
        PlayerSettings.SetScriptingBackend(NamedBuildTarget.Standalone, ScriptingImplementation.Mono2x);
        // Вызывает `PlayerSettings.SetManagedStrippingLevel` с аргументами `NamedBuildTarget.Standalone, ManagedStrippingLevel.Minimal`.
        PlayerSettings.SetManagedStrippingLevel(NamedBuildTarget.Standalone, ManagedStrippingLevel.Minimal);

        // Вызывает `GameBuildScenes.EnabledScenePaths` и записывает результат в `scenes`.
        var scenes = GameBuildScenes.EnabledScenePaths();
        // Задаёт `folder` значением `"Builds/TreasureHunting"`.
        const string folder = "Builds/TreasureHunting";
        // Проверяет условие `!System.IO.Directory.Exists(folder)`.
        if (!System.IO.Directory.Exists(folder))
            // Вызывает `System.IO.Directory.CreateDirectory` с аргументами `folder`.
            System.IO.Directory.CreateDirectory(folder);
        // Вычисляет `output` как `folder` плюс `"/TreasureHunting.exe"`.
        var output = folder + "/TreasureHunting.exe";

        // Вызывает `BuildPipeline.BuildPlayer` с аргументами `scenes, output, BuildTarget.StandaloneWindows64, BuildOptions.None` и записывает результат в `report`.
        var report = BuildPipeline.BuildPlayer(scenes, output, BuildTarget.StandaloneWindows64, BuildOptions.None);
        // Проверяет условие `report.summary.result != BuildResult.Succeeded`.
        if (report.summary.result != BuildResult.Succeeded)
        {  // Начало ветки условия.
            // Пишет ошибку в консоль Unity через `Debug.LogError` с аргументами `"Сборка не удалась: " + report.summary.totalErrors + " ошибок."`.
            Debug.LogError("Сборка не удалась: " + report.summary.totalErrors + " ошибок.");
            // Вызывает `EditorApplication.Exit` с аргументами `1`.
            EditorApplication.Exit(1);
        }  // Конец ветки условия.
        // Обрабатывает случай, когда предыдущее условие ложно.
        else
            // Пишет сообщение в консоль Unity через `Debug.Log` с аргументами `"Сборка: " + output`.
            Debug.Log("Сборка: " + output);
    }  // Конец метода `PerformBuild`.
}  // Конец класса `YardStandaloneBuild`.
// Заканчивает условный раздел для редактора Unity.
#endif
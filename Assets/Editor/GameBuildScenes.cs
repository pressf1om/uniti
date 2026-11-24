// Включает этот раздел при компиляции для редактора Unity.
#if UNITY_EDITOR
// Подключает доступ к обобщённым спискам, словарям и множествам.
using System.Collections.Generic;
// Подключает доступ к операциям выборки и фильтрации коллекций.
using System.Linq;
// Подключает доступ к инструментам редактора Unity.
using UnityEditor;

/// <summary>
/// Игровой exe — только меню и двор. Демо Photon/XR в билд не попадают (Wizard иногда включает всё подряд).
/// </summary>
public static class GameBuildScenes  // Задаёт список сцен основной сборки и дополнительных демонстраций.
{  // Начало класса `GameBuildScenes`.
    // Задаёт `MainMenu` значением `"Assets/Scenes/MainMenu.unity"`.
    public const string MainMenu = "Assets/Scenes/MainMenu.unity";
    // Задаёт `Yard` значением `"Assets/Scenes/SampleScene.unity"`.
    public const string Yard = "Assets/Scenes/SampleScene.unity";

    // Задаёт начальные элементы массива `YardPlayerScenes`.
    static readonly (string path, bool enabled)[] YardPlayerScenes =
    {  // Начало списка элементов массива.
        // Включает сцену `MainMenu` в списке сборки.
        (MainMenu, true),
        // Включает сцену `Yard` в списке сборки.
        (Yard, true),
    };  // Конец списка элементов массива.

    // Задаёт начальные элементы массива `ExtraForEditorOnly`.
    static readonly (string path, bool enabled)[] ExtraForEditorOnly =
    {  // Начало списка элементов массива.
        // Указывает как отключённую сцену `"Assets/Photon/PhotonUnityNetworking/Demos/PunBasics-Tutorial/Scenes/PunBasics-Launcher.unity"` в списке сборки.
        ("Assets/Photon/PhotonUnityNetworking/Demos/PunBasics-Tutorial/Scenes/PunBasics-Launcher.unity", false),
        // Указывает как отключённую сцену `"Assets/Photon/PhotonUnityNetworking/Demos/PunBasics-Tutorial/Scenes/PunBasics-Room for 1.unity"` в списке сборки.
        ("Assets/Photon/PhotonUnityNetworking/Demos/PunBasics-Tutorial/Scenes/PunBasics-Room for 1.unity", false),
        // Указывает как отключённую сцену `"Assets/Photon/PhotonUnityNetworking/Demos/PunBasics-Tutorial/Scenes/PunBasics-Room for 2.unity"` в списке сборки.
        ("Assets/Photon/PhotonUnityNetworking/Demos/PunBasics-Tutorial/Scenes/PunBasics-Room for 2.unity", false),
        // Указывает как отключённую сцену `"Assets/Photon/PhotonUnityNetworking/Demos/PunBasics-Tutorial/Scenes/PunBasics-Room for 3.unity"` в списке сборки.
        ("Assets/Photon/PhotonUnityNetworking/Demos/PunBasics-Tutorial/Scenes/PunBasics-Room for 3.unity", false),
        // Указывает как отключённую сцену `"Assets/Photon/PhotonUnityNetworking/Demos/PunBasics-Tutorial/Scenes/PunBasics-Room for 4.unity"` в списке сборки.
        ("Assets/Photon/PhotonUnityNetworking/Demos/PunBasics-Tutorial/Scenes/PunBasics-Room for 4.unity", false),
        // Указывает как отключённую сцену `"Assets/Scenes/XrDemo.unity"` в списке сборки.
        ("Assets/Scenes/XrDemo.unity", false),
    };  // Конец списка элементов массива.

    // Добавляет команду редактора Unity `"Treasure Hunting/Зафиксировать сцены билда (меню + двор)"`.
    [MenuItem("Treasure Hunting/Зафиксировать сцены билда (меню + двор)")]
    // Настраивает игровой список сцен командой из меню Unity.
    public static void ApplyFromMenu()
    {  // Начало метода `ApplyFromMenu`.
        // Назначает сцены сборки и помечает демонстрации как отключённые через `ApplyYardPlayerSettings`.
        ApplyYardPlayerSettings();
    }  // Конец метода `ApplyFromMenu`.

    // Назначает сцены сборки и помечает демонстрации как отключённые.
    public static void ApplyYardPlayerSettings()
    {  // Начало метода `ApplyYardPlayerSettings`.
        // Создаёт экземпляр `List<EditorBuildSettingsScene>` и сохраняет ссылку в `wanted`.
        var wanted = new List<EditorBuildSettingsScene>();
        // Перебирает элементы `YardPlayerScenes` по одному в `entry`.
        foreach (var entry in YardPlayerScenes)
        {  // Начало цикла по элементам.
            // Проверяет условие `System.IO.File.Exists(entry.path)`.
            if (System.IO.File.Exists(entry.path))
                // Добавляет элемент через `wanted.Add` с аргументами `new EditorBuildSettingsScene(entry.path, entry.enabled)`.
                wanted.Add(new EditorBuildSettingsScene(entry.path, entry.enabled));
        }  // Конец цикла по элементам.

        // Перебирает элементы `ExtraForEditorOnly` по одному в `entry`.
        foreach (var entry in ExtraForEditorOnly)
        {  // Начало цикла по элементам.
            // Проверяет условие `!System.IO.File.Exists(entry.path)`.
            if (!System.IO.File.Exists(entry.path))
                // Пропускает остаток этой итерации и переходит к следующей.
                continue;
            // Проверяет условие `wanted.Any(s => s.path == entry.path)`.
            if (wanted.Any(s => s.path == entry.path))
                // Пропускает остаток этой итерации и переходит к следующей.
                continue;
            // Добавляет элемент через `wanted.Add` с аргументами `new EditorBuildSettingsScene(entry.path, entry.enabled)`.
            wanted.Add(new EditorBuildSettingsScene(entry.path, entry.enabled));
        }  // Конец цикла по элементам.

        // Копирует элементы в массив через `wanted.ToArray` и записывает результат в `EditorBuildSettings.scenes`.
        EditorBuildSettings.scenes = wanted.ToArray();
    }  // Конец метода `ApplyYardPlayerSettings`.

    // Возвращает пути сцен, предназначенных для основной сборки.
    public static string[] EnabledScenePaths()
    {  // Начало метода `EnabledScenePaths`.
        // Назначает сцены сборки и помечает демонстрации как отключённые через `ApplyYardPlayerSettings`.
        ApplyYardPlayerSettings();
        // Возвращает результат вызова `EditorBuildSettings.scenes.Where(s => s.enabled).Select(s => s.path).ToArray()`.
        return EditorBuildSettings.scenes.Where(s => s.enabled).Select(s => s.path).ToArray();
    }  // Конец метода `EnabledScenePaths`.
}  // Конец класса `GameBuildScenes`.
// Заканчивает условный раздел для редактора Unity.
#endif
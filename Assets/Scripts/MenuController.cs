// Подключает доступ к базовым типам игрового движка.
using UnityEngine;
// Подключает доступ к обработке событий интерфейса.
using UnityEngine.EventSystems;
// Подключает доступ к вводу с клавиатуры, мыши и других устройств.
using UnityEngine.InputSystem;
// Подключает доступ к привязке новой системы ввода к интерфейсу.
using UnityEngine.InputSystem.UI;
// Подключает доступ к загрузке и запросу сцен.
using UnityEngine.SceneManagement;
// Подключает доступ к элементам интерфейса Unity UI.
using UnityEngine.UI;

// Строит главное меню и связывает его с запуском, настройками и продолжением игры.
public class MenuController : MonoBehaviour
{  // Начало класса `MenuController`.
    // Хранит ссылку на объект сцены в `main`.
    GameObject main;
    // Хранит ссылку на объект сцены в `settings`.
    GameObject settings;
    // Хранит ссылку на объект сцены в `confirm`.
    GameObject confirm;
    // Хранит ссылку на текстовую подпись в `volumeLabel`.
    Text volumeLabel;
    // Хранит ссылку на текстовую подпись в `sensitivityLabel`.
    Text sensitivityLabel;
    // Хранит ссылку на текстовую подпись в `codeStatusLabel`.
    Text codeStatusLabel;
    // Хранит ссылку на поле ввода в `menuCodeInput`.
    InputField menuCodeInput;
    // Хранит ссылку на кнопку в `continueButton`.
    Button continueButton;

    // Готовит меню, его интерфейс, фон и обработчики кнопок.
    void Awake()
    {  // Начало метода `Awake`.
        // Строит трёхмерный фон меню из объектов двора через `BuildWorldPreview`.
        BuildWorldPreview();
        // Устанавливает свет и параметры окружения фоновой сцены через `SetupLighting`.
        SetupLighting();

        // Проверяет условие `FindAnyObjectByType<EventSystem>() == null`.
        if (FindAnyObjectByType<EventSystem>() == null)
        {  // Начало ветки условия.
            // Создаёт объект сцены с параметрами `"События"` и сохраняет ссылку в `events`.
            var events = new GameObject("События");
            // Добавляет компонент через `events.AddComponent<EventSystem>`.
            events.AddComponent<EventSystem>();
            // Добавляет компонент через `events.AddComponent<InputSystemUIInputModule>`.
            events.AddComponent<InputSystemUIInputModule>();
        }  // Конец ветки условия.

        // Создаёт объект сцены с параметрами `"Меню"` и сохраняет ссылку в `canvasGo`.
        var canvasGo = new GameObject("Меню");
        // Добавляет компонент через `canvasGo.AddComponent<Canvas>` и записывает результат в `canvas`.
        var canvas = canvasGo.AddComponent<Canvas>();
        // Задаёт `canvas.renderMode` значением `RenderMode.ScreenSpaceOverlay`.
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        // Добавляет компонент через `canvasGo.AddComponent<CanvasScaler>` и записывает результат в `scaler`.
        var scaler = canvasGo.AddComponent<CanvasScaler>();
        // Задаёт `scaler.uiScaleMode` значением `CanvasScaler.ScaleMode.ScaleWithScreenSize`.
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        // Записывает в `scaler.referenceResolution` двумерный вектор с параметрами `1920f, 1080f`.
        scaler.referenceResolution = new Vector2(1920f, 1080f);
        // Добавляет компонент через `canvasGo.AddComponent<GraphicRaycaster>`.
        canvasGo.AddComponent<GraphicRaycaster>();

        // Создаёт контейнер с фоном для экрана меню через `Panel` с аргументами `canvasGo.transform, "Главное"` и записывает результат в `main`.
        main = Panel(canvasGo.transform, "Главное");
        // Размещает заголовок заданного размера через `Title` с аргументами `main.transform, "Раскоп во дворе", 64, 220f`.
        Title(main.transform, "Раскоп во дворе", 64, 220f);
        // Присваивает `continueButton` результат `ActionButton(main.transform, new Vector2(0f, 60f), new Vector2(520f, 58f), ContinueGame, "1 Продолжить")`.
        continueButton = ActionButton(main.transform, new Vector2(0f, 60f), new Vector2(520f, 58f), ContinueGame, "1  Продолжить");
        // Создаёт кнопку и назначает ей обработчик нажатия через `ActionButton` с аргументами `main.transform, new Vector2(0f, -20f), new Vector2(520f, 58f), AskNewGame, "2 Новая игра"`.
        ActionButton(main.transform, new Vector2(0f, -20f), new Vector2(520f, 58f), AskNewGame, "2  Новая игра");
        // Создаёт кнопку и назначает ей обработчик нажатия через `ActionButton` с аргументами `main.transform, new Vector2(0f, -100f), new Vector2(520f, 58f), ShowSettings, "3 Настройки"`.
        ActionButton(main.transform, new Vector2(0f, -100f), new Vector2(520f, 58f), ShowSettings, "3  Настройки");
        // Создаёт кнопку и назначает ей обработчик нажатия через `ActionButton` с аргументами `main.transform, new Vector2(0f, -180f), new Vector2(520f, 58f), QuitGame, "4 Выход"`.
        ActionButton(main.transform, new Vector2(0f, -180f), new Vector2(520f, 58f), QuitGame, "4  Выход");

        // Создаёт панель настроек, элементы ввода и подписи через `SettingsPanel` с аргументами `canvasGo.transform` и записывает результат в `settings`.
        settings = SettingsPanel(canvasGo.transform);
        // Меняет активность объекта через `settings.SetActive` с аргументами `false`.
        settings.SetActive(false);

        // Создаёт контейнер с фоном для экрана меню через `Panel` с аргументами `canvasGo.transform, "Подтверждение"` и записывает результат в `confirm`.
        confirm = Panel(canvasGo.transform, "Подтверждение");
        // Размещает заголовок заданного размера через `Title` с аргументами `confirm.transform, "Стереть сохранение?", 44, 80f`.
        Title(confirm.transform, "Стереть сохранение?", 44, 80f);
        // Создаёт кнопку и назначает ей обработчик нажатия через `ActionButton` с аргументами `confirm.transform, new Vector2(0f, -40f), new Vector2(420f, 54f), StartFresh, "Да, новая игра"`.
        ActionButton(confirm.transform, new Vector2(0f, -40f), new Vector2(420f, 54f), StartFresh, "Да, новая игра");
        // Создаёт кнопку и назначает ей обработчик нажатия через `ActionButton` с аргументами `confirm.transform, new Vector2(0f, -130f), new Vector2(420f, 54f), ShowMain, "Нет"`.
        ActionButton(confirm.transform, new Vector2(0f, -130f), new Vector2(420f, 54f), ShowMain, "Нет");
        // Меняет активность объекта через `confirm.SetActive` с аргументами `false`.
        confirm.SetActive(false);
        // Подставляет текущие значения в элементы настроек через `RefreshSettings`.
        RefreshSettings();
        // Обновляет доступность кнопки продолжения через `RefreshContinue`.
        RefreshContinue();
    }  // Конец метода `Awake`.

    // Строит трёхмерный фон меню из объектов двора.
    void BuildWorldPreview()
    {  // Начало метода `BuildWorldPreview`.
        // Ищет объект по имени через `GameObject.Find` с аргументами `"Main Camera"` и записывает результат в `existing`.
        var existing = GameObject.Find("Main Camera");
        // Проверяет условие `existing != null`.
        if (existing != null)
            // Меняет активность объекта через `existing.SetActive` с аргументами `false`.
            existing.SetActive(false);

        // Создаёт объект сцены с параметрами `"Обзор участка"` и сохраняет ссылку в `root`.
        var root = new GameObject("Обзор участка");
        // Вызывает `YardLayout.Build` с аргументами `root.transform` и записывает результат в `yard`.
        var yard = YardLayout.Build(root.transform);
        // Перебирает элементы `yard.GetComponentsInChildren<MonoBehaviour>(true)` по одному в `behaviour`.
        foreach (var behaviour in yard.GetComponentsInChildren<MonoBehaviour>(true))
        {  // Начало цикла по элементам.
            // Проверяет условие `behaviour is Seller or AmbientBed or FallReset or ShopShovelPreview or TrafficLight`.
            if (behaviour is Seller or AmbientBed or FallReset or ShopShovelPreview or TrafficLight)
                // Устанавливает активность компонента `behaviour.enabled` в `false`.
                behaviour.enabled = false;
        }  // Конец цикла по элементам.
        // Ищет объект по имени через `yard.Find` с аргументами `"Продавец"` и записывает результат в `seller`.
        var seller = yard.Find("Продавец");
        // Проверяет условие `seller != null`.
        if (seller != null)
            // Меняет активность объекта через `seller.gameObject.SetActive` с аргументами `false`.
            seller.gameObject.SetActive(false);

        // Рисует на фоне отметку поверхности раскопа через `BuildDigCrossPreview` с аргументами `yard`.
        BuildDigCrossPreview(yard);

        // Создаёт объект сцены с параметрами `"Камера меню"` и сохраняет ссылку в `camGo`.
        var camGo = new GameObject("Камера меню");
        // Добавляет компонент через `camGo.AddComponent<Camera>` и записывает результат в `cam`.
        var cam = camGo.AddComponent<Camera>();
        // Задаёт тег `cam.tag` значением `"MainCamera"`.
        cam.tag = "MainCamera";
        // Задаёт режим очистки фона камеры `cam.clearFlags` значением `CameraClearFlags.SolidColor`.
        cam.clearFlags = CameraClearFlags.SolidColor;
        // Записывает в `cam.backgroundColor` цвет RGBA с параметрами `0.55f, 0.72f, 0.9f`.
        cam.backgroundColor = new Color(0.55f, 0.72f, 0.9f);
        // Задаёт угол обзора камеры `cam.fieldOfView` значением `50`.
        cam.fieldOfView = 50f;
        // Вычисляет `pit` как `YardMetrics.Width` умножить на `YardMetrics.Cell`.
        float pit = YardMetrics.Width * YardMetrics.Cell;
        // Создаёт трёхмерный вектор с параметрами `pit * 0.5f, 0.25f, pit * 0.5f` и сохраняет ссылку в `focus`.
        var focus = new Vector3(pit * 0.5f, 0.25f, pit * 0.5f);
        // Записывает в `camGo.transform.position` трёхмерный вектор с параметрами `12.5f, 21f, -12f`.
        camGo.transform.position = new Vector3(12.5f, 21f, -12f);
        // Вызывает `camGo.transform.LookAt` с аргументами `focus`.
        camGo.transform.LookAt(focus);
        // Добавляет компонент через `camGo.AddComponent<AudioListener>`.
        camGo.AddComponent<AudioListener>();
    }  // Конец метода `BuildWorldPreview`.

    // Рисует на фоне отметку поверхности раскопа.
    static void BuildDigCrossPreview(Transform yard)
    {  // Начало метода `BuildDigCrossPreview`.
        // Вычисляет `pit` как `YardMetrics.Width` умножить на `YardMetrics.Cell`.
        float pit = YardMetrics.Width * YardMetrics.Cell;
        // Вычисляет `cx` как `pit` умножить на `0.5`.
        float cx = pit * 0.5f;
        // Вычисляет `cz` как `pit` умножить на `0.5`.
        float cz = pit * 0.5f;
        // Вызывает `TextureFactory.Lit` с аргументами `Palette.Grass, TextureFactory.Grass(), 0.1f` и записывает результат в `grass`.
        var grass = TextureFactory.Lit(Palette.Grass, TextureFactory.Grass(), 0.1f);
        // Вызывает `TextureFactory.Lit` с аргументами `Palette.Cross, TextureFactory.Grass(), 0.12f` и записывает результат в `cross`.
        var cross = TextureFactory.Lit(Palette.Cross, TextureFactory.Grass(), 0.12f);

        // Создаёт стандартный примитив через `GameObject.CreatePrimitive` с аргументами `PrimitiveType.Cube` и записывает результат в `plot`.
        var plot = GameObject.CreatePrimitive(PrimitiveType.Cube);
        // Задаёт имя `plot.name` значением `"Раскоп (трава)"`.
        plot.name = "Раскоп (трава)";
        // Привязывает к родителю через `plot.transform.SetParent` с аргументами `yard, false`.
        plot.transform.SetParent(yard, false);
        // Записывает в `plot.transform.position` трёхмерный вектор с параметрами `cx, 0.05f, cz`.
        plot.transform.position = new Vector3(cx, 0.05f, cz);
        // Записывает в `plot.transform.localScale` трёхмерный вектор с параметрами `pit - 0.35f, 0.1f, pit - 0.35f`.
        plot.transform.localScale = new Vector3(pit - 0.35f, 0.1f, pit - 0.35f);
        // Задаёт общий материал `plot.GetComponent<MeshRenderer>().sharedMaterial` значением `grass`.
        plot.GetComponent<MeshRenderer>().sharedMaterial = grass;
        // Планирует удаление объекта через `Object.Destroy` с аргументами `plot.GetComponent<Collider>()`.
        Object.Destroy(plot.GetComponent<Collider>());

        // Вычисляет `arm` как `YardMetrics.Cell` умножить на `4`.
        float arm = YardMetrics.Cell * 4f;
        // Вычисляет `thick` как `YardMetrics.Cell` умножить на `0.95`.
        float thick = YardMetrics.Cell * 0.95f;
        // Задаёт `y` значением `0.095`.
        const float y = 0.095f;
        // Добавляет одну полосу отметки через `AddCrossBar` с аргументами `yard, cross, new Vector3(cx, y, cz), new Vector3(arm * 2f, 0.02f, thick)`.
        AddCrossBar(yard, cross, new Vector3(cx, y, cz), new Vector3(arm * 2f, 0.02f, thick));
        // Добавляет одну полосу отметки через `AddCrossBar` с аргументами `yard, cross, new Vector3(cx, y, cz), new Vector3(thick, 0.02f, arm * 2f)`.
        AddCrossBar(yard, cross, new Vector3(cx, y, cz), new Vector3(thick, 0.02f, arm * 2f));
    }  // Конец метода `BuildDigCrossPreview`.

    // Добавляет одну полосу отметки.
    static void AddCrossBar(Transform yard, Material material, Vector3 position, Vector3 scale)
    {  // Начало метода `AddCrossBar`.
        // Создаёт стандартный примитив через `GameObject.CreatePrimitive` с аргументами `PrimitiveType.Cube` и записывает результат в `bar`.
        var bar = GameObject.CreatePrimitive(PrimitiveType.Cube);
        // Задаёт имя `bar.name` значением `"Перекрёсток"`.
        bar.name = "Перекрёсток";
        // Привязывает к родителю через `bar.transform.SetParent` с аргументами `yard, false`.
        bar.transform.SetParent(yard, false);
        // Задаёт мировая позиция `bar.transform.position` значением `position`.
        bar.transform.position = position;
        // Задаёт локальный масштаб `bar.transform.localScale` значением `scale`.
        bar.transform.localScale = scale;
        // Задаёт общий материал `bar.GetComponent<MeshRenderer>().sharedMaterial` значением `material`.
        bar.GetComponent<MeshRenderer>().sharedMaterial = material;
        // Планирует удаление объекта через `Object.Destroy` с аргументами `bar.GetComponent<Collider>()`.
        Object.Destroy(bar.GetComponent<Collider>());
    }  // Конец метода `AddCrossBar`.

    // Устанавливает свет и параметры окружения фоновой сцены.
    static void SetupLighting()
    {  // Начало метода `SetupLighting`.
        // Задаёт `RenderSettings.ambientMode` значением `UnityEngine.Rendering.AmbientMode.Trilight`.
        RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Trilight;
        // Записывает в `RenderSettings.ambientSkyColor` цвет RGBA с параметрами `0.55f, 0.66f, 0.82f`.
        RenderSettings.ambientSkyColor = new Color(0.55f, 0.66f, 0.82f);
        // Создаёт объект сцены с параметрами `"Солнце меню"` и сохраняет ссылку в `lightGo`.
        var lightGo = new GameObject("Солнце меню");
        // Добавляет компонент через `lightGo.AddComponent<Light>` и записывает результат в `light`.
        var light = lightGo.AddComponent<Light>();
        // Задаёт `light.type` значением `LightType.Directional`.
        light.type = LightType.Directional;
        // Задаёт `light.intensity` значением `1.05`.
        light.intensity = 1.05f;
        // Задаёт цвет `light.color` значением `Palette.Sun`.
        light.color = Palette.Sun;
        // Создаёт поворот из углов Эйлера через `Quaternion.Euler` с аргументами `48f, -38f, 0f` и записывает результат в `lightGo.transform.rotation`.
        lightGo.transform.rotation = Quaternion.Euler(48f, -38f, 0f);
    }  // Конец метода `SetupLighting`.

    // Создаёт панель настроек, элементы ввода и подписи.
    GameObject SettingsPanel(Transform parent)
    {  // Начало метода `SettingsPanel`.
        // Создаёт объект сцены с параметрами `"Настройки"` и сохраняет ссылку в `root`.
        var root = new GameObject("Настройки");
        // Привязывает к родителю через `root.transform.SetParent` с аргументами `parent, false`.
        root.transform.SetParent(parent, false);
        // Добавляет компонент через `root.AddComponent<RectTransform>` и записывает результат в `rootRect`.
        var rootRect = root.AddComponent<RectTransform>();
        // Задаёт нижняя левая привязка `rootRect.anchorMin` значением `Vector2.zero`.
        rootRect.anchorMin = Vector2.zero;
        // Задаёт верхняя правая привязка `rootRect.anchorMax` значением `Vector2.one`.
        rootRect.anchorMax = Vector2.one;
        // Задаёт нижний левый отступ `rootRect.offsetMin` значением `Vector2.zero`.
        rootRect.offsetMin = Vector2.zero;
        // Задаёт верхний правый отступ `rootRect.offsetMax` значением `Vector2.zero`.
        rootRect.offsetMax = Vector2.zero;
        // Добавляет компонент через `root.AddComponent<Image>` и записывает результат в `dim`.
        var dim = root.AddComponent<Image>();
        // Записывает в `dim.color` цвет RGBA с параметрами `0f, 0f, 0f, 0.35f`.
        dim.color = new Color(0f, 0f, 0f, 0.35f);

        // Создаёт объект сцены с параметрами `"Панель"` и сохраняет ссылку в `panel`.
        var panel = new GameObject("Панель");
        // Привязывает к родителю через `panel.transform.SetParent` с аргументами `root.transform, false`.
        panel.transform.SetParent(root.transform, false);
        // Добавляет компонент через `panel.AddComponent<RectTransform>` и записывает результат в `panelRect`.
        var panelRect = panel.AddComponent<RectTransform>();
        // Записывает в `panelRect.anchorMin` двумерный вектор с параметрами `0.5f, 0.5f`.
        panelRect.anchorMin = new Vector2(0.5f, 0.5f);
        // Записывает в `panelRect.anchorMax` двумерный вектор с параметрами `0.5f, 0.5f`.
        panelRect.anchorMax = new Vector2(0.5f, 0.5f);
        // Записывает в `panelRect.pivot` двумерный вектор с параметрами `0.5f, 0.5f`.
        panelRect.pivot = new Vector2(0.5f, 0.5f);
        // Записывает в `panelRect.sizeDelta` двумерный вектор с параметрами `SettingsUi.PanelWidth, SettingsUi.PanelHeight`.
        panelRect.sizeDelta = new Vector2(SettingsUi.PanelWidth, SettingsUi.PanelHeight);
        // Задаёт цвет `panel.AddComponent<Image>().color` значением `Palette.Paper`.
        panel.AddComponent<Image>().color = Palette.Paper;

        // Вызывает `Resources.GetBuiltinResource<Font>` с аргументами `"LegacyRuntime.ttf"` и записывает результат в `font`.
        var font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        // Создаёт и размещает текстовую подпись через `Caption` с аргументами `panel.transform, font, new Vector2(0f, 165f), 34, Palette.Ink` и записывает результат в `title`.
        var title = Caption(panel.transform, font, new Vector2(0f, 165f), 34, Palette.Ink);
        // Задаёт текст `title.text` значением `"Настройки"`.
        title.text = "Настройки";

        // Создаёт и размещает текстовую подпись через `Caption` с аргументами `panel.transform, font, new Vector2(0f, 75f), 26, Palette.Ink` и записывает результат в `volumeLabel`.
        volumeLabel = Caption(panel.transform, font, new Vector2(0f, 75f), 26, Palette.Ink);
        // Начинает вызов `ActionButton` с аргументами на следующих строках.
        ActionButton(panel.transform, new Vector2(-170f, 75f), new Vector2(72f, 48f), () =>
        {  // Начало тела обработчика.
            // Уменьшает `GameSettings.Volume` на `0.1f`.
            GameSettings.Volume -= 0.1f;
            // Подставляет текущие значения в элементы настроек через `RefreshSettings`.
            RefreshSettings();
        // Передаёт аргумент `"−"`.
        }, "−");
        // Начинает вызов `ActionButton` с аргументами на следующих строках.
        ActionButton(panel.transform, new Vector2(170f, 75f), new Vector2(72f, 48f), () =>
        {  // Начало тела обработчика.
            // Добавляет `0.1f` к текущему значению `GameSettings.Volume`.
            GameSettings.Volume += 0.1f;
            // Подставляет текущие значения в элементы настроек через `RefreshSettings`.
            RefreshSettings();
        // Передаёт аргумент `"+"`.
        }, "+");

        // Создаёт и размещает текстовую подпись через `Caption` с аргументами `panel.transform, font, new Vector2(0f, 10f), 26, Palette.Ink` и записывает результат в `sensitivityLabel`.
        sensitivityLabel = Caption(panel.transform, font, new Vector2(0f, 10f), 26, Palette.Ink);
        // Начинает вызов `ActionButton` с аргументами на следующих строках.
        ActionButton(panel.transform, new Vector2(-170f, 10f), new Vector2(72f, 48f), () =>
        {  // Начало тела обработчика.
            // Уменьшает `GameSettings.Sensitivity` на `0.02f`.
            GameSettings.Sensitivity -= 0.02f;
            // Подставляет текущие значения в элементы настроек через `RefreshSettings`.
            RefreshSettings();
        // Передаёт аргумент `"−"`.
        }, "−");
        // Начинает вызов `ActionButton` с аргументами на следующих строках.
        ActionButton(panel.transform, new Vector2(170f, 10f), new Vector2(72f, 48f), () =>
        {  // Начало тела обработчика.
            // Добавляет `0.02f` к текущему значению `GameSettings.Sensitivity`.
            GameSettings.Sensitivity += 0.02f;
            // Подставляет текущие значения в элементы настроек через `RefreshSettings`.
            RefreshSettings();
        // Передаёт аргумент `"+"`.
        }, "+");
        // Задаёт текст `Caption(panel.transform, font, new Vector2(0f, -60f), 24, Palette.Ink).text` значением `"Ввести код"`.
        Caption(panel.transform, font, new Vector2(0f, -60f), 24, Palette.Ink).text = "Ввести код";
        // Вызывает `SettingsUi.CodeField` с аргументами `panel.transform, font, new Vector2(0f, -110f), new Vector2(380f, 44f)` и записывает результат в `menuCodeInput`.
        menuCodeInput = SettingsUi.CodeField(panel.transform, font, new Vector2(0f, -110f), new Vector2(380f, 44f));
        // Создаёт кнопку и назначает ей обработчик нажатия через `ActionButton` с аргументами `panel.transform, new Vector2(0f, -165f), new Vector2(280f, 50f), TryMenuCheatCode, "Применить"`.
        ActionButton(panel.transform, new Vector2(0f, -165f), new Vector2(280f, 50f), TryMenuCheatCode, "Применить");
        // Создаёт и размещает текстовую подпись через `Caption` с аргументами `panel.transform, font, new Vector2(0f, -205f), 22, Palette.Ink` и записывает результат в `codeStatusLabel`.
        codeStatusLabel = Caption(panel.transform, font, new Vector2(0f, -205f), 22, Palette.Ink);
        // Задаёт цвет `codeStatusLabel.color` значением `Palette.Gold`.
        codeStatusLabel.color = Palette.Gold;
        // Создаёт кнопку и назначает ей обработчик нажатия через `ActionButton` с аргументами `panel.transform, new Vector2(0f, -230f), new Vector2(320f, 54f), ShowMain, "Назад"`.
        ActionButton(panel.transform, new Vector2(0f, -230f), new Vector2(320f, 54f), ShowMain, "Назад");

        // Возвращает `root`.
        return root;
    }  // Конец метода `SettingsPanel`.

    // Обрабатывает клавиши и ввод на активном экране меню.
    void Update()
    {  // Начало метода `Update`.
        // Задаёт `keyboard` значением текущая клавиатура `Keyboard.current`.
        var keyboard = Keyboard.current;
        // Проверяет условие `keyboard == null`.
        if (keyboard == null)
            // Завершает выполнение текущего метода.
            return;
        // Проверяет условие `settings.activeSelf`.
        if (settings.activeSelf)
        {  // Начало ветки условия.
            // Проверяет условие `keyboard.leftArrowKey.wasPressedThisFrame`.
            if (keyboard.leftArrowKey.wasPressedThisFrame)
                // Уменьшает `GameSettings.Volume` на `0.1f`.
                GameSettings.Volume -= 0.1f;
            // Проверяет условие `keyboard.rightArrowKey.wasPressedThisFrame`.
            if (keyboard.rightArrowKey.wasPressedThisFrame)
                // Добавляет `0.1f` к текущему значению `GameSettings.Volume`.
                GameSettings.Volume += 0.1f;
            // Проверяет условие `keyboard.downArrowKey.wasPressedThisFrame`.
            if (keyboard.downArrowKey.wasPressedThisFrame)
                // Уменьшает `GameSettings.Sensitivity` на `0.02f`.
                GameSettings.Sensitivity -= 0.02f;
            // Проверяет условие `keyboard.upArrowKey.wasPressedThisFrame`.
            if (keyboard.upArrowKey.wasPressedThisFrame)
                // Добавляет `0.02f` к текущему значению `GameSettings.Sensitivity`.
                GameSettings.Sensitivity += 0.02f;
            // Подставляет текущие значения в элементы настроек через `RefreshSettings`.
            RefreshSettings();
            // Проверяет условие `keyboard.escapeKey.wasPressedThisFrame`.
            if (keyboard.escapeKey.wasPressedThisFrame)
                // Возвращает на основной экран меню через `ShowMain`.
                ShowMain();
            // Завершает выполнение текущего метода.
            return;
        }  // Конец ветки условия.
        // Проверяет условие `confirm.activeSelf`.
        if (confirm.activeSelf)
        {  // Начало ветки условия.
            // Проверяет условие `keyboard.enterKey.wasPressedThisFrame`.
            if (keyboard.enterKey.wasPressedThisFrame)
                // Удаляет предыдущий прогресс и начинает новую партию через `StartFresh`.
                StartFresh();
            // Проверяет условие `keyboard.escapeKey.wasPressedThisFrame`.
            if (keyboard.escapeKey.wasPressedThisFrame)
                // Возвращает на основной экран меню через `ShowMain`.
                ShowMain();
            // Завершает выполнение текущего метода.
            return;
        }  // Конец ветки условия.
        // Проверяет условие `keyboard.digit1Key.wasPressedThisFrame`.
        if (keyboard.digit1Key.wasPressedThisFrame)
            // Загружает игровую сцену в режиме продолжения через `ContinueGame`.
            ContinueGame();
        // Проверяет условие `keyboard.digit2Key.wasPressedThisFrame`.
        if (keyboard.digit2Key.wasPressedThisFrame)
            // Просит подтвердить начало новой игры при наличии сохранения через `AskNewGame`.
            AskNewGame();
        // Проверяет условие `keyboard.digit3Key.wasPressedThisFrame`.
        if (keyboard.digit3Key.wasPressedThisFrame)
            // Открывает настройки и обновляет показанные значения через `ShowSettings`.
            ShowSettings();
        // Проверяет условие `keyboard.digit4Key.wasPressedThisFrame || keyboard.escapeKey.wasPressedThisFrame`.
        if (keyboard.digit4Key.wasPressedThisFrame || keyboard.escapeKey.wasPressedThisFrame)
            // Просит Unity закрыть приложение через `QuitGame`.
            QuitGame();
    }  // Конец метода `Update`.

    // Открывает настройки и обновляет показанные значения.
    public void ShowSettings()
    {  // Начало метода `ShowSettings`.
        // Меняет активность объекта через `main.SetActive` с аргументами `false`.
        main.SetActive(false);
        // Меняет активность объекта через `confirm.SetActive` с аргументами `false`.
        confirm.SetActive(false);
        // Меняет активность объекта через `settings.SetActive` с аргументами `true`.
        settings.SetActive(true);
        // Проверяет условие `menuCodeInput != null`.
        if (menuCodeInput != null)
            // Задаёт текст `menuCodeInput.text` значением `""`.
            menuCodeInput.text = "";
        // Проверяет условие `codeStatusLabel != null`.
        if (codeStatusLabel != null)
            // Задаёт текст `codeStatusLabel.text` значением `""`.
            codeStatusLabel.text = "";
        // Подставляет текущие значения в элементы настроек через `RefreshSettings`.
        RefreshSettings();
    }  // Конец метода `ShowSettings`.

    // Проверяет код монет, введённый в главном меню.
    void TryMenuCheatCode()
    {  // Начало метода `TryMenuCheatCode`.
        // Проверяет условие `menuCodeInput == null`.
        if (menuCodeInput == null)
            // Завершает выполнение текущего метода.
            return;
        // Проверяет условие `GameSettings.TryRedeemCoinCode(menuCodeInput.text, out var message)`.
        if (GameSettings.TryRedeemCoinCode(menuCodeInput.text, out var message))
        {  // Начало ветки условия.
            // Проверяет условие `codeStatusLabel != null`.
            if (codeStatusLabel != null)
                // Задаёт текст `codeStatusLabel.text` значением `message`.
                codeStatusLabel.text = message;
            // Задаёт текст `menuCodeInput.text` значением `""`.
            menuCodeInput.text = "";
        }  // Конец ветки условия.
        // Если предыдущая проверка не сработала, проверяет `message != null && codeStatusLabel != null`.
        else if (message != null && codeStatusLabel != null)
            // Задаёт текст `codeStatusLabel.text` значением `message`.
            codeStatusLabel.text = message;

        // Вызывает `menuCodeInput.DeactivateInputField`.
        menuCodeInput.DeactivateInputField();
        // Проверяет условие `EventSystem.current != null`.
        if (EventSystem.current != null)
            // Вызывает `EventSystem.current.SetSelectedGameObject` с аргументами `null`.
            EventSystem.current.SetSelectedGameObject(null);
    }  // Конец метода `TryMenuCheatCode`.

    // Просит подтвердить начало новой игры при наличии сохранения.
    public void AskNewGame()
    {  // Начало метода `AskNewGame`.
        // Проверяет условие `!GameSave.Exists`.
        if (!GameSave.Exists)
        {  // Начало ветки условия.
            // Удаляет предыдущий прогресс и начинает новую партию через `StartFresh`.
            StartFresh();
            // Завершает выполнение текущего метода.
            return;
        }  // Конец ветки условия.
        // Меняет активность объекта через `main.SetActive` с аргументами `false`.
        main.SetActive(false);
        // Меняет активность объекта через `settings.SetActive` с аргументами `false`.
        settings.SetActive(false);
        // Меняет активность объекта через `confirm.SetActive` с аргументами `true`.
        confirm.SetActive(true);
    }  // Конец метода `AskNewGame`.

    // Возвращает на основной экран меню.
    public void ShowMain()
    {  // Начало метода `ShowMain`.
        // Меняет активность объекта через `settings.SetActive` с аргументами `false`.
        settings.SetActive(false);
        // Меняет активность объекта через `confirm.SetActive` с аргументами `false`.
        confirm.SetActive(false);
        // Меняет активность объекта через `main.SetActive` с аргументами `true`.
        main.SetActive(true);
        // Обновляет доступность кнопки продолжения через `RefreshContinue`.
        RefreshContinue();
    }  // Конец метода `ShowMain`.

    // Загружает игровую сцену в режиме продолжения.
    void ContinueGame()
    {  // Начало метода `ContinueGame`.
        // Проверяет условие `!GameSave.Exists`.
        if (!GameSave.Exists)
            // Завершает выполнение текущего метода.
            return;
        // Устанавливает режим продолжения игры `PlaySession.Continue` в `true`.
        PlaySession.Continue = true;
        // Загружает сцену через `SceneManager.LoadScene` с аргументами `"SampleScene"`.
        SceneManager.LoadScene("SampleScene");
    }  // Конец метода `ContinueGame`.

    // Удаляет предыдущий прогресс и начинает новую партию.
    void StartFresh()
    {  // Начало метода `StartFresh`.
        // Удаляет локальное сохранение через `GameSave.Delete`.
        GameSave.Delete();
        // Устанавливает режим продолжения игры `PlaySession.Continue` в `false`.
        PlaySession.Continue = false;
        // Загружает сцену через `SceneManager.LoadScene` с аргументами `"SampleScene"`.
        SceneManager.LoadScene("SampleScene");
    }  // Конец метода `StartFresh`.

    // Просит Unity закрыть приложение.
    void QuitGame()
    {  // Начало метода `QuitGame`.
        // Запрашивает выход из приложения через `Application.Quit`.
        Application.Quit();
    }  // Конец метода `QuitGame`.

    // Обновляет доступность кнопки продолжения.
    void RefreshContinue()
    {  // Начало метода `RefreshContinue`.
        // Задаёт `has` значением `GameSave.Exists`.
        bool has = GameSave.Exists;
        // Задаёт доступность для взаимодействия `continueButton.interactable` значением `has`.
        continueButton.interactable = has;
        // Ищет компонент в объекте и его потомках через `continueButton.GetComponentInChildren<Text>` и записывает результат в `label`.
        var label = continueButton.GetComponentInChildren<Text>();
        // Проверяет условие `label != null`.
        if (label != null)
            // Выбирает текст `label.text` по условию `has`.
            label.text = has ? "1  Продолжить" : "1  Продолжить — нет сохранения";
    }  // Конец метода `RefreshContinue`.

    // Подставляет текущие значения в элементы настроек.
    void RefreshSettings()
    {  // Начало метода `RefreshSettings`.
        // Проверяет условие `volumeLabel != null`.
        if (volumeLabel != null)
            // Задаёт текст `volumeLabel.text` значением `"Громкость "` плюс вызывает `Mathf.RoundToInt` с аргументами `GameSettings.Volume * 100f` плюс `" %"`.
            volumeLabel.text = "Громкость " + Mathf.RoundToInt(GameSettings.Volume * 100f) + " %";
        // Проверяет условие `sensitivityLabel != null`.
        if (sensitivityLabel != null)
            // Задаёт текст `sensitivityLabel.text` значением `"Мышь "` плюс вызывает `GameSettings.Sensitivity.ToString` с аргументами `"0.00"`.
            sensitivityLabel.text = "Мышь " + GameSettings.Sensitivity.ToString("0.00");
    }  // Конец метода `RefreshSettings`.

    // Создаёт контейнер с фоном для экрана меню.
    static GameObject Panel(Transform parent, string name)
    {  // Начало метода `Panel`.
        // Создаёт объект сцены с параметрами `name` и сохраняет ссылку в `go`.
        var go = new GameObject(name);
        // Привязывает к родителю через `go.transform.SetParent` с аргументами `parent, false`.
        go.transform.SetParent(parent, false);
        // Добавляет компонент через `go.AddComponent<RectTransform>` и записывает результат в `rect`.
        var rect = go.AddComponent<RectTransform>();
        // Задаёт нижняя левая привязка `rect.anchorMin` значением `Vector2.zero`.
        rect.anchorMin = Vector2.zero;
        // Задаёт верхняя правая привязка `rect.anchorMax` значением `Vector2.one`.
        rect.anchorMax = Vector2.one;
        // Задаёт нижний левый отступ `rect.offsetMin` значением `Vector2.zero`.
        rect.offsetMin = Vector2.zero;
        // Задаёт верхний правый отступ `rect.offsetMax` значением `Vector2.zero`.
        rect.offsetMax = Vector2.zero;
        // Возвращает `go`.
        return go;
    }  // Конец метода `Panel`.

    // Размещает заголовок заданного размера.
    static void Title(Transform parent, string value, int size, float y)
    {  // Начало метода `Title`.
        // Создаёт и размещает текстовую подпись через `Caption` с аргументами `parent, Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf"), new Vector2(0f, y), size, Palette.Paper` и записывает результат в `text`.
        var text = Caption(parent, Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf"), new Vector2(0f, y), size, Palette.Paper);
        // Задаёт текст `text.text` значением `value`.
        text.text = value;
    }  // Конец метода `Title`.

    // Создаёт и размещает текстовую подпись.
    static Text Caption(Transform parent, Font font, Vector2 place, int size, Color color)
    {  // Начало метода `Caption`.
        // Создаёт объект сцены с параметрами `"Строка"` и сохраняет ссылку в `go`.
        var go = new GameObject("Строка");
        // Привязывает к родителю через `go.transform.SetParent` с аргументами `parent, false`.
        go.transform.SetParent(parent, false);
        // Добавляет компонент через `go.AddComponent<RectTransform>` и записывает результат в `rect`.
        var rect = go.AddComponent<RectTransform>();
        // Записывает в `rect.anchorMin` двумерный вектор с параметрами `0.5f, 0.5f`.
        rect.anchorMin = new Vector2(0.5f, 0.5f);
        // Записывает в `rect.anchorMax` двумерный вектор с параметрами `0.5f, 0.5f`.
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        // Записывает в `rect.pivot` двумерный вектор с параметрами `0.5f, 0.5f`.
        rect.pivot = new Vector2(0.5f, 0.5f);
        // Задаёт позиция относительно привязок `rect.anchoredPosition` значением `place`.
        rect.anchoredPosition = place;
        // Записывает в `rect.sizeDelta` двумерный вектор с параметрами `560f, 72f`.
        rect.sizeDelta = new Vector2(560f, 72f);
        // Добавляет компонент через `go.AddComponent<Text>` и записывает результат в `text`.
        var text = go.AddComponent<Text>();
        // Задаёт шрифт `text.font` значением `font`.
        text.font = font;
        // Задаёт размер шрифта `text.fontSize` значением `size`.
        text.fontSize = size;
        // Задаёт выравнивание текста `text.alignment` значением `TextAnchor.MiddleCenter`.
        text.alignment = TextAnchor.MiddleCenter;
        // Задаёт цвет `text.color` значением `color`.
        text.color = color;
        // Возвращает `text`.
        return text;
    }  // Конец метода `Caption`.

    // Создаёт кнопку и назначает ей обработчик нажатия.
    static Button ActionButton(Transform parent, Vector2 place, Vector2 size, UnityEngine.Events.UnityAction action, string label)
    {  // Начало метода `ActionButton`.
        // Создаёт объект сцены с параметрами `label` и сохраняет ссылку в `go`.
        var go = new GameObject(label);
        // Привязывает к родителю через `go.transform.SetParent` с аргументами `parent, false`.
        go.transform.SetParent(parent, false);
        // Добавляет компонент через `go.AddComponent<RectTransform>` и записывает результат в `rect`.
        var rect = go.AddComponent<RectTransform>();
        // Записывает в `rect.anchorMin` двумерный вектор с параметрами `0.5f, 0.5f`.
        rect.anchorMin = new Vector2(0.5f, 0.5f);
        // Записывает в `rect.anchorMax` двумерный вектор с параметрами `0.5f, 0.5f`.
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        // Записывает в `rect.pivot` двумерный вектор с параметрами `0.5f, 0.5f`.
        rect.pivot = new Vector2(0.5f, 0.5f);
        // Задаёт позиция относительно привязок `rect.anchoredPosition` значением `place`.
        rect.anchoredPosition = place;
        // Задаёт размер относительно привязок `rect.sizeDelta` значением `size`.
        rect.sizeDelta = size;
        // Добавляет компонент через `go.AddComponent<Image>` и записывает результат в `image`.
        var image = go.AddComponent<Image>();
        // Задаёт цвет `image.color` значением `Palette.Sun`.
        image.color = Palette.Sun;
        // Добавляет компонент через `go.AddComponent<Button>` и записывает результат в `button`.
        var button = go.AddComponent<Button>();
        // Задаёт `button.targetGraphic` значением `image`.
        button.targetGraphic = image;
        // Задаёт `colors` значением `button.colors`.
        var colors = button.colors;
        // Записывает в `colors.disabledColor` цвет RGBA с параметрами `0.55f, 0.52f, 0.46f, 0.7f`.
        colors.disabledColor = new Color(0.55f, 0.52f, 0.46f, 0.7f);
        // Задаёт `button.colors` значением `colors`.
        button.colors = colors;
        // Подписывает обработчик на событие через `button.onClick.AddListener` с аргументами `action`.
        button.onClick.AddListener(action);
        // Создаёт и размещает текстовую подпись через `Caption` с аргументами `go.transform, Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf"), Vector2.zero, 24, Palette.Ink` и записывает результат в `caption`.
        var caption = Caption(go.transform, Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf"), Vector2.zero, 24, Palette.Ink);
        // Задаёт текст `caption.text` значением `label`.
        caption.text = label;
        // Задаёт размер относительно привязок `caption.rectTransform.sizeDelta` значением `size`.
        caption.rectTransform.sizeDelta = size;
        // Возвращает `button`.
        return button;
    }  // Конец метода `ActionButton`.
}  // Конец класса `MenuController`.
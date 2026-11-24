// Подключает доступ к базовым типам игрового движка.
using UnityEngine;
// Подключает доступ к обработке событий интерфейса.
using UnityEngine.EventSystems;
// Подключает доступ к вводу с клавиатуры, мыши и других устройств.
using UnityEngine.InputSystem;
// Подключает доступ к привязке новой системы ввода к интерфейсу.
using UnityEngine.InputSystem.UI;
// Подключает доступ к элементам интерфейса Unity UI.
using UnityEngine.UI;

// Строит игровой интерфейс с рюкзаком, магазином, паузой и победой.
public class PlayHud : MonoBehaviour
{  // Начало класса `PlayHud`.
    // Задаёт начальные элементы массива `Goods`.
    static readonly ResourceKind[] Goods =
    {  // Начало списка элементов массива.
        // Использует значение `ResourceKind.Stone`.
        ResourceKind.Stone, ResourceKind.Coal, ResourceKind.Copper, ResourceKind.Iron, ResourceKind.Gold
    };  // Конец списка элементов массива.

    // Хранит ссылку на текстовую подпись в `goal`.
    Text goal;
    // Хранит список предметов в рюкзаке `slots`.
    Text slots;
    // Хранит ссылку на текстовую подпись в `hint`.
    Text hint;
    // Хранит ссылку на объект сцены в `pauseRoot`.
    GameObject pauseRoot;
    // Хранит ссылку на объект сцены в `pauseMain`.
    GameObject pauseMain;
    // Хранит ссылку на объект сцены в `pauseSettings`.
    GameObject pauseSettings;
    // Хранит ссылку на текстовую подпись в `pauseVolumeLabel`.
    Text pauseVolumeLabel;
    // Хранит ссылку на текстовую подпись в `pauseSensitivityLabel`.
    Text pauseSensitivityLabel;
    // Хранит ссылку на поле ввода в `pauseCodeInput`.
    InputField pauseCodeInput;
    // Хранит ссылку на объект сцены в `win`.
    GameObject win;
    // Хранит ссылку на объект сцены в `shopPanel`.
    GameObject shopPanel;
    // Хранит ссылку на объект сцены в `sellPage`.
    GameObject sellPage;
    // Хранит ссылку на объект сцены в `upgradePage`.
    GameObject upgradePage;
    // Хранит ссылку на текстовую подпись в `shopTitle`.
    Text shopTitle;
    // Хранит ссылку на кнопку в `shovelButton`.
    Button shovelButton;
    // Хранит ссылку на текстовую подпись в `shovelCaption`.
    Text shovelCaption;
    // Хранит ссылку на кнопку в `slotButton`.
    Button slotButton;
    // Хранит ссылку на текстовую подпись в `slotCaption`.
    Text slotCaption;
    // Хранит ссылку на кнопку в `bootButton`.
    Button bootButton;
    // Хранит ссылку на текстовую подпись в `bootCaption`.
    Text bootCaption;
    // Хранит ссылку на кнопку в `sellAllButton`.
    Button sellAllButton;
    // Хранит ссылку на текстовую подпись в `sellSummary`.
    Text sellSummary;

    // Создаёт холст и элементы игрового интерфейса во время запуска.
    public void Build()
    {  // Начало метода `Build`.
        // Создаёт объект сцены с параметрами `"Интерфейс"` и сохраняет ссылку в `canvasGo`.
        var canvasGo = new GameObject("Интерфейс");
        // Добавляет компонент через `canvasGo.AddComponent<Canvas>` и записывает результат в `canvas`.
        var canvas = canvasGo.AddComponent<Canvas>();
        // Задаёт `canvas.renderMode` значением `RenderMode.ScreenSpaceOverlay`.
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        // Задаёт `canvas.sortingOrder` значением `10`.
        canvas.sortingOrder = 10;
        // Добавляет компонент через `canvasGo.AddComponent<CanvasScaler>` и записывает результат в `scaler`.
        var scaler = canvasGo.AddComponent<CanvasScaler>();
        // Задаёт `scaler.uiScaleMode` значением `CanvasScaler.ScaleMode.ScaleWithScreenSize`.
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        // Записывает в `scaler.referenceResolution` двумерный вектор с параметрами `1920f, 1080f`.
        scaler.referenceResolution = new Vector2(1920f, 1080f);
        // Добавляет компонент через `canvasGo.AddComponent<GraphicRaycaster>`.
        canvasGo.AddComponent<GraphicRaycaster>();

        // Вызывает `Resources.GetBuiltinResource<Font>` с аргументами `"LegacyRuntime.ttf"` и записывает результат в `font`.
        var font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        // Присваивает `goal` результат `Label(canvasGo.transform, font, new Vector2(0.5f, 1f), new Vector2(0.5f, 1f), new Vector2(0f, -28f), 28)`.
        goal = Label(canvasGo.transform, font, new Vector2(0.5f, 1f), new Vector2(0.5f, 1f), new Vector2(0f, -28f), 28);
        // Задаёт цвет `goal.color` значением `Palette.Paper`.
        goal.color = Palette.Paper;
        // Присваивает список предметов в рюкзаке `slots` результат `Label(canvasGo.transform, font, new Vector2(1f, 1f), new Vector2(1f, 1f), new Vector2(-24f, -28f), 24)`.
        slots = Label(canvasGo.transform, font, new Vector2(1f, 1f), new Vector2(1f, 1f), new Vector2(-24f, -28f), 24);
        // Задаёт выравнивание текста `slots.alignment` значением `TextAnchor.UpperRight`.
        slots.alignment = TextAnchor.UpperRight;
        // Задаёт цвет `slots.color` значением `Palette.Paper`.
        slots.color = Palette.Paper;
        // Присваивает `hint` результат `Label(canvasGo.transform, font, new Vector2(0.5f, 0f), new Vector2(0.5f, 0f), new Vector2(0f, 48f), 26)`.
        hint = Label(canvasGo.transform, font, new Vector2(0.5f, 0f), new Vector2(0.5f, 0f), new Vector2(0f, 48f), 26);
        // Задаёт цвет `hint.color` значением `Palette.Sun`.
        hint.color = Palette.Sun;

        // Создаёт объект сцены с параметрами `"Прилавок"` и сохраняет ссылку в `shopPanel`.
        var shopPanel = new GameObject("Прилавок");
        // Привязывает к родителю через `shopPanel.transform.SetParent` с аргументами `canvasGo.transform, false`.
        shopPanel.transform.SetParent(canvasGo.transform, false);
        // Добавляет компонент через `shopPanel.AddComponent<RectTransform>` и записывает результат в `shopRect`.
        var shopRect = shopPanel.AddComponent<RectTransform>();
        // Записывает в `shopRect.anchorMin` двумерный вектор с параметрами `0.5f, 0.5f`.
        shopRect.anchorMin = new Vector2(0.5f, 0.5f);
        // Записывает в `shopRect.anchorMax` двумерный вектор с параметрами `0.5f, 0.5f`.
        shopRect.anchorMax = new Vector2(0.5f, 0.5f);
        // Записывает в `shopRect.pivot` двумерный вектор с параметрами `0.5f, 0.5f`.
        shopRect.pivot = new Vector2(0.5f, 0.5f);
        // Записывает в `shopRect.sizeDelta` двумерный вектор с параметрами `640f, 460f`.
        shopRect.sizeDelta = new Vector2(640f, 460f);
        // Добавляет компонент через `shopPanel.AddComponent<Image>` и записывает результат в `shopBack`.
        var shopBack = shopPanel.AddComponent<Image>();
        // Задаёт цвет `shopBack.color` значением `Palette.Paper`.
        shopBack.color = Palette.Paper;
        // Задаёт `this.shopPanel` значением `shopPanel`.
        this.shopPanel = shopPanel;
        // Создаёт и размещает текстовую подпись через `Caption` с аргументами `shopPanel.transform, font, new Vector2(0f, 170f), 36, Palette.Ink` и записывает результат в `shopTitle`.
        shopTitle = Caption(shopPanel.transform, font, new Vector2(0f, 170f), 36, Palette.Ink);
        // Записывает в `sellPage` объект сцены с параметрами `"Продажа"`.
        sellPage = new GameObject("Продажа");
        // Привязывает к родителю через `sellPage.transform.SetParent` с аргументами `shopPanel.transform, false`.
        sellPage.transform.SetParent(shopPanel.transform, false);
        // Добавляет компонент через `sellPage.AddComponent<RectTransform>` и записывает результат в `sellRect`.
        var sellRect = sellPage.AddComponent<RectTransform>();
        // Задаёт нижняя левая привязка `sellRect.anchorMin` значением `Vector2.zero`.
        sellRect.anchorMin = Vector2.zero;
        // Задаёт верхняя правая привязка `sellRect.anchorMax` значением `Vector2.one`.
        sellRect.anchorMax = Vector2.one;
        // Задаёт нижний левый отступ `sellRect.offsetMin` значением `Vector2.zero`.
        sellRect.offsetMin = Vector2.zero;
        // Задаёт верхний правый отступ `sellRect.offsetMax` значением `Vector2.zero`.
        sellRect.offsetMax = Vector2.zero;
        // Создаёт и размещает текстовую подпись через `Caption` с аргументами `sellPage.transform, font, new Vector2(0f, 40f), 26, Palette.Ink` и записывает результат в `sellSummary`.
        sellSummary = Caption(sellPage.transform, font, new Vector2(0f, 40f), 26, Palette.Ink);
        // Записывает в `sellSummary.rectTransform.sizeDelta` двумерный вектор с параметрами `520f, 160f`.
        sellSummary.rectTransform.sizeDelta = new Vector2(520f, 160f);
        // Присваивает `sellAllButton` результат `ActionButton(sellPage.transform, font, new Vector2(0f, -70f), new Vector2(420f, 72f), () => Player().SellAll())`.
        sellAllButton = ActionButton(sellPage.transform, font, new Vector2(0f, -70f), new Vector2(420f, 72f), () => Player().SellAll());
        // Записывает в `upgradePage` объект сцены с параметрами `"Улучшения"`.
        upgradePage = new GameObject("Улучшения");
        // Привязывает к родителю через `upgradePage.transform.SetParent` с аргументами `shopPanel.transform, false`.
        upgradePage.transform.SetParent(shopPanel.transform, false);
        // Добавляет компонент через `upgradePage.AddComponent<RectTransform>` и записывает результат в `upgradeRect`.
        var upgradeRect = upgradePage.AddComponent<RectTransform>();
        // Задаёт нижняя левая привязка `upgradeRect.anchorMin` значением `Vector2.zero`.
        upgradeRect.anchorMin = Vector2.zero;
        // Задаёт верхняя правая привязка `upgradeRect.anchorMax` значением `Vector2.one`.
        upgradeRect.anchorMax = Vector2.one;
        // Задаёт нижний левый отступ `upgradeRect.offsetMin` значением `Vector2.zero`.
        upgradeRect.offsetMin = Vector2.zero;
        // Задаёт верхний правый отступ `upgradeRect.offsetMax` значением `Vector2.zero`.
        upgradeRect.offsetMax = Vector2.zero;
        // Присваивает `shovelButton` результат `ActionButton(upgradePage.transform, font, new Vector2(0f, 80f), new Vector2(480f, 76f), () => Player().BuyShovel())`.
        shovelButton = ActionButton(upgradePage.transform, font, new Vector2(0f, 80f), new Vector2(480f, 76f), () => Player().BuyShovel());
        // Ищет компонент в объекте и его потомках через `shovelButton.GetComponentInChildren<Text>` и записывает результат в `shovelCaption`.
        shovelCaption = shovelButton.GetComponentInChildren<Text>();
        // Присваивает `slotButton` результат `ActionButton(upgradePage.transform, font, new Vector2(0f, -5f), new Vector2(480f, 76f), () => Player().BuySlot())`.
        slotButton = ActionButton(upgradePage.transform, font, new Vector2(0f, -5f), new Vector2(480f, 76f), () => Player().BuySlot());
        // Ищет компонент в объекте и его потомках через `slotButton.GetComponentInChildren<Text>` и записывает результат в `slotCaption`.
        slotCaption = slotButton.GetComponentInChildren<Text>();
        // Присваивает `bootButton` результат `ActionButton(upgradePage.transform, font, new Vector2(0f, -90f), new Vector2(480f, 76f), () => Player().BuyDoubleJumpBoots())`.
        bootButton = ActionButton(upgradePage.transform, font, new Vector2(0f, -90f), new Vector2(480f, 76f), () => Player().BuyDoubleJumpBoots());
        // Ищет компонент в объекте и его потомках через `bootButton.GetComponentInChildren<Text>` и записывает результат в `bootCaption`.
        bootCaption = bootButton.GetComponentInChildren<Text>();
        // Создаёт кнопку и назначает ей обработчик нажатия через `ActionButton` с аргументами `shopPanel.transform, font, new Vector2(0f, -170f), new Vector2(240f, 54f), () => Player().CloseShop()` и записывает результат в `close`.
        var close = ActionButton(shopPanel.transform, font, new Vector2(0f, -170f), new Vector2(240f, 54f), () => Player().CloseShop());
        // Задаёт текст `close.GetComponentInChildren<Text>().text` значением `"Закрыть"`.
        close.GetComponentInChildren<Text>().text = "Закрыть";
        // Меняет активность объекта через `shopPanel.SetActive` с аргументами `false`.
        shopPanel.SetActive(false);

        // Создаёт EventSystem, если сцене нужен обработчик событий интерфейса через `EnsureEvents`.
        EnsureEvents();

        // Записывает в `pauseRoot` объект сцены с параметрами `"Пауза"`.
        pauseRoot = new GameObject("Пауза");
        // Привязывает к родителю через `pauseRoot.transform.SetParent` с аргументами `canvasGo.transform, false`.
        pauseRoot.transform.SetParent(canvasGo.transform, false);
        // Добавляет компонент через `pauseRoot.AddComponent<RectTransform>` и записывает результат в `rootRect`.
        var rootRect = pauseRoot.AddComponent<RectTransform>();
        // Задаёт нижняя левая привязка `rootRect.anchorMin` значением `Vector2.zero`.
        rootRect.anchorMin = Vector2.zero;
        // Задаёт верхняя правая привязка `rootRect.anchorMax` значением `Vector2.one`.
        rootRect.anchorMax = Vector2.one;
        // Задаёт нижний левый отступ `rootRect.offsetMin` значением `Vector2.zero`.
        rootRect.offsetMin = Vector2.zero;
        // Задаёт верхний правый отступ `rootRect.offsetMax` значением `Vector2.zero`.
        rootRect.offsetMax = Vector2.zero;
        // Добавляет компонент через `pauseRoot.AddComponent<Image>` и записывает результат в `pauseDim`.
        var pauseDim = pauseRoot.AddComponent<Image>();
        // Записывает в `pauseDim.color` цвет RGBA с параметрами `0f, 0f, 0f, 0.45f`.
        pauseDim.color = new Color(0f, 0f, 0f, 0.45f);

        // Записывает в `pauseMain` объект сцены с параметрами `"Меню"`.
        pauseMain = new GameObject("Меню");
        // Привязывает к родителю через `pauseMain.transform.SetParent` с аргументами `pauseRoot.transform, false`.
        pauseMain.transform.SetParent(pauseRoot.transform, false);
        // Добавляет компонент через `pauseMain.AddComponent<RectTransform>` и записывает результат в `pauseRect`.
        var pauseRect = pauseMain.AddComponent<RectTransform>();
        // Записывает в `pauseRect.anchorMin` двумерный вектор с параметрами `0.5f, 0.5f`.
        pauseRect.anchorMin = new Vector2(0.5f, 0.5f);
        // Записывает в `pauseRect.anchorMax` двумерный вектор с параметрами `0.5f, 0.5f`.
        pauseRect.anchorMax = new Vector2(0.5f, 0.5f);
        // Записывает в `pauseRect.pivot` двумерный вектор с параметрами `0.5f, 0.5f`.
        pauseRect.pivot = new Vector2(0.5f, 0.5f);
        // Записывает в `pauseRect.sizeDelta` двумерный вектор с параметрами `640f, 400f`.
        pauseRect.sizeDelta = new Vector2(640f, 400f);
        // Добавляет компонент через `pauseMain.AddComponent<Image>` и записывает результат в `pauseBack`.
        var pauseBack = pauseMain.AddComponent<Image>();
        // Задаёт цвет `pauseBack.color` значением `Palette.Paper`.
        pauseBack.color = Palette.Paper;
        // Создаёт текст и привязывает его к области холста через `Label` с аргументами `pauseMain.transform, font, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0f, 130f), 36` и записывает результат в `pauseText`.
        var pauseText = Label(pauseMain.transform, font, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0f, 130f), 36);
        // Задаёт выравнивание текста `pauseText.alignment` значением `TextAnchor.MiddleCenter`.
        pauseText.alignment = TextAnchor.MiddleCenter;
        // Задаёт цвет `pauseText.color` значением `Palette.Ink`.
        pauseText.color = Palette.Ink;
        // Задаёт текст `pauseText.text` значением `"Пауза"`.
        pauseText.text = "Пауза";
        // Задаёт текст `ActionButton(pauseMain.transform, font, new Vector2(0f, 50f), new Vector2(400f, 58f), () => Player().Resume()).GetComponentInChildren<Text>().text` значением `"Продолжить"`.
        ActionButton(pauseMain.transform, font, new Vector2(0f, 50f), new Vector2(400f, 58f), () => Player().Resume()).GetComponentInChildren<Text>().text =
            // Добавляет строковое значение `"Продолжить"`.
            "Продолжить";
        // Задаёт текст `ActionButton(pauseMain.transform, font, new Vector2(0f, -30f), new Vector2(400f, 58f), ShowPauseSettings).GetComponentInChildren<Text>().text` значением `"Настройки"`.
        ActionButton(pauseMain.transform, font, new Vector2(0f, -30f), new Vector2(400f, 58f), ShowPauseSettings).GetComponentInChildren<Text>().text =
            // Добавляет строковое значение `"Настройки"`.
            "Настройки";
        // Задаёт текст `ActionButton(pauseMain.transform, font, new Vector2(0f, -110f), new Vector2(400f, 58f), () => Player().LeaveToMenu()).GetComponentInChildren<Text>().text` значением `"Сохранить и выйти в меню"`.
        ActionButton(pauseMain.transform, font, new Vector2(0f, -110f), new Vector2(400f, 58f), () => Player().LeaveToMenu()).GetComponentInChildren<Text>().text =
            // Добавляет строковое значение `"Сохранить и выйти в меню"`.
            "Сохранить и выйти в меню";

        // Записывает в `pauseSettings` объект сцены с параметрами `"Настройки"`.
        pauseSettings = new GameObject("Настройки");
        // Привязывает к родителю через `pauseSettings.transform.SetParent` с аргументами `pauseRoot.transform, false`.
        pauseSettings.transform.SetParent(pauseRoot.transform, false);
        // Добавляет компонент через `pauseSettings.AddComponent<RectTransform>` и записывает результат в `setRect`.
        var setRect = pauseSettings.AddComponent<RectTransform>();
        // Записывает в `setRect.anchorMin` двумерный вектор с параметрами `0.5f, 0.5f`.
        setRect.anchorMin = new Vector2(0.5f, 0.5f);
        // Записывает в `setRect.anchorMax` двумерный вектор с параметрами `0.5f, 0.5f`.
        setRect.anchorMax = new Vector2(0.5f, 0.5f);
        // Записывает в `setRect.pivot` двумерный вектор с параметрами `0.5f, 0.5f`.
        setRect.pivot = new Vector2(0.5f, 0.5f);
        // Записывает в `setRect.sizeDelta` двумерный вектор с параметрами `SettingsUi.PanelWidth, SettingsUi.PanelHeight`.
        setRect.sizeDelta = new Vector2(SettingsUi.PanelWidth, SettingsUi.PanelHeight);
        // Добавляет компонент через `pauseSettings.AddComponent<Image>` и записывает результат в `setBack`.
        var setBack = pauseSettings.AddComponent<Image>();
        // Задаёт цвет `setBack.color` значением `Palette.Paper`.
        setBack.color = Palette.Paper;
        // Создаёт и размещает текстовую подпись через `Caption` с аргументами `pauseSettings.transform, font, new Vector2(0f, 165f), 34, Palette.Ink` и записывает результат в `setTitle`.
        var setTitle = Caption(pauseSettings.transform, font, new Vector2(0f, 165f), 34, Palette.Ink);
        // Задаёт текст `setTitle.text` значением `"Настройки"`.
        setTitle.text = "Настройки";
        // Создаёт и размещает текстовую подпись через `Caption` с аргументами `pauseSettings.transform, font, new Vector2(0f, 75f), 26, Palette.Ink` и записывает результат в `pauseVolumeLabel`.
        pauseVolumeLabel = Caption(pauseSettings.transform, font, new Vector2(0f, 75f), 26, Palette.Ink);
        // Задаёт текст `ActionButton(pauseSettings.transform, font, new Vector2(-170f, 75f), new Vector2(72f, 48f), () => { GameSettings.Volume -= 0.1f, RefreshPauseSettings(), }).Get…` значением `"−"`.
        ActionButton(pauseSettings.transform, font, new Vector2(-170f, 75f), new Vector2(72f, 48f), () =>
        {  // Начало тела обработчика.
            // Уменьшает `GameSettings.Volume` на `0.1f`.
            GameSettings.Volume -= 0.1f;
            // Выводит текущие громкость и чувствительность на панели через `RefreshPauseSettings`.
            RefreshPauseSettings();
        // Заканчивает обработчик и ставит знак минуса на кнопку громкости.
        }).GetComponentInChildren<Text>().text = "−";
        // Задаёт текст `ActionButton(pauseSettings.transform, font, new Vector2(170f, 75f), new Vector2(72f, 48f), () => { GameSettings.Volume += 0.1f, RefreshPauseSettings(), }).GetC…` значением `"+"`.
        ActionButton(pauseSettings.transform, font, new Vector2(170f, 75f), new Vector2(72f, 48f), () =>
        {  // Начало тела обработчика.
            // Добавляет `0.1f` к текущему значению `GameSettings.Volume`.
            GameSettings.Volume += 0.1f;
            // Выводит текущие громкость и чувствительность на панели через `RefreshPauseSettings`.
            RefreshPauseSettings();
        // Заканчивает обработчик и ставит знак плюса на кнопку громкости.
        }).GetComponentInChildren<Text>().text = "+";
        // Создаёт и размещает текстовую подпись через `Caption` с аргументами `pauseSettings.transform, font, new Vector2(0f, 10f), 26, Palette.Ink` и записывает результат в `pauseSensitivityLabel`.
        pauseSensitivityLabel = Caption(pauseSettings.transform, font, new Vector2(0f, 10f), 26, Palette.Ink);
        // Задаёт текст `ActionButton(pauseSettings.transform, font, new Vector2(-170f, 10f), new Vector2(72f, 48f), () => { GameSettings.Sensitivity -= 0.02f, RefreshPauseSettings(),…` значением `"−"`.
        ActionButton(pauseSettings.transform, font, new Vector2(-170f, 10f), new Vector2(72f, 48f), () =>
        {  // Начало тела обработчика.
            // Уменьшает `GameSettings.Sensitivity` на `0.02f`.
            GameSettings.Sensitivity -= 0.02f;
            // Выводит текущие громкость и чувствительность на панели через `RefreshPauseSettings`.
            RefreshPauseSettings();
        // Заканчивает обработчик и ставит знак минуса на кнопку чувствительности.
        }).GetComponentInChildren<Text>().text = "−";
        // Задаёт текст `ActionButton(pauseSettings.transform, font, new Vector2(170f, 10f), new Vector2(72f, 48f), () => { GameSettings.Sensitivity += 0.02f, RefreshPauseSettings(), }…` значением `"+"`.
        ActionButton(pauseSettings.transform, font, new Vector2(170f, 10f), new Vector2(72f, 48f), () =>
        {  // Начало тела обработчика.
            // Добавляет `0.02f` к текущему значению `GameSettings.Sensitivity`.
            GameSettings.Sensitivity += 0.02f;
            // Выводит текущие громкость и чувствительность на панели через `RefreshPauseSettings`.
            RefreshPauseSettings();
        // Заканчивает обработчик и ставит знак плюса на кнопку чувствительности.
        }).GetComponentInChildren<Text>().text = "+";
        // Задаёт текст `Caption(pauseSettings.transform, font, new Vector2(0f, -60f), 24, Palette.Ink).text` значением `"Ввести код"`.
        Caption(pauseSettings.transform, font, new Vector2(0f, -60f), 24, Palette.Ink).text = "Ввести код";
        // Вызывает `SettingsUi.CodeField` с аргументами `pauseSettings.transform, font, new Vector2(0f, -110f), new Vector2(380f, 44f)` и записывает результат в `pauseCodeInput`.
        pauseCodeInput = SettingsUi.CodeField(pauseSettings.transform, font, new Vector2(0f, -110f), new Vector2(380f, 44f));
        // Задаёт текст `ActionButton(pauseSettings.transform, font, new Vector2(0f, -165f), new Vector2(280f, 50f), TrySubmitCheatCode) .GetComponentInChildren<Text>().text` значением `"Применить"`.
        ActionButton(pauseSettings.transform, font, new Vector2(0f, -165f), new Vector2(280f, 50f), TrySubmitCheatCode)
            // Подписывает кнопку применения кода.
            .GetComponentInChildren<Text>().text = "Применить";
        // Задаёт текст `ActionButton(pauseSettings.transform, font, new Vector2(0f, -230f), new Vector2(320f, 54f), ClosePauseSettings).GetComponentInChildren<Text>().text` значением `"Назад"`.
        ActionButton(pauseSettings.transform, font, new Vector2(0f, -230f), new Vector2(320f, 54f), ClosePauseSettings).GetComponentInChildren<Text>().text =
            // Добавляет строковое значение `"Назад"`.
            "Назад";
        // Меняет активность объекта через `pauseSettings.SetActive` с аргументами `false`.
        pauseSettings.SetActive(false);

        // Меняет активность объекта через `pauseRoot.SetActive` с аргументами `false`.
        pauseRoot.SetActive(false);

        // Создаёт текст и привязывает его к области холста через `Label` с аргументами `canvasGo.transform, font, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), Vector2.zero, 28` и записывает результат в `cross`.
        var cross = Label(canvasGo.transform, font, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), Vector2.zero, 28);
        // Задаёт текст `cross.text` значением `"+"`.
        cross.text = "+";
        // Задаёт цвет `cross.color` значением `Palette.Paper`.
        cross.color = Palette.Paper;
        // Записывает в `cross.rectTransform.sizeDelta` двумерный вектор с параметрами `40f, 40f`.
        cross.rectTransform.sizeDelta = new Vector2(40f, 40f);

        // Записывает в `win` объект сцены с параметрами `"Победа"`.
        win = new GameObject("Победа");
        // Привязывает к родителю через `win.transform.SetParent` с аргументами `canvasGo.transform, false`.
        win.transform.SetParent(canvasGo.transform, false);
        // Добавляет компонент через `win.AddComponent<RectTransform>` и записывает результат в `winRect`.
        var winRect = win.AddComponent<RectTransform>();
        // Задаёт нижняя левая привязка `winRect.anchorMin` значением `Vector2.zero`.
        winRect.anchorMin = Vector2.zero;
        // Задаёт верхняя правая привязка `winRect.anchorMax` значением `Vector2.one`.
        winRect.anchorMax = Vector2.one;
        // Задаёт нижний левый отступ `winRect.offsetMin` значением `Vector2.zero`.
        winRect.offsetMin = Vector2.zero;
        // Задаёт верхний правый отступ `winRect.offsetMax` значением `Vector2.zero`.
        winRect.offsetMax = Vector2.zero;
        // Добавляет компонент через `win.AddComponent<Image>` и записывает результат в `winDim`.
        var winDim = win.AddComponent<Image>();
        // Задаёт `dimColor` значением `Palette.Ink`.
        var dimColor = Palette.Ink;
        // Задаёт `dimColor.a` значением `0.72`.
        dimColor.a = 0.72f;
        // Задаёт цвет `winDim.color` значением `dimColor`.
        winDim.color = dimColor;
        // Создаёт текст и привязывает его к области холста через `Label` с аргументами `win.transform, font, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0f, 50f), 42` и записывает результат в `winText`.
        var winText = Label(win.transform, font, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new Vector2(0f, 50f), 42);
        // Задаёт текст `winText.text` значением `"Клад найден!"`.
        winText.text = "Клад найден!";
        // Задаёт цвет `winText.color` значением `Palette.Treasure`.
        winText.color = Palette.Treasure;
        // Задаёт выравнивание текста `winText.alignment` значением `TextAnchor.MiddleCenter`.
        winText.alignment = TextAnchor.MiddleCenter;
        // Задаёт текст `ActionButton(win.transform, font, new Vector2(0f, -40f), new Vector2(360f, 58f), DismissWin).GetComponentInChildren<Text>().text` значением `"Продолжить"`.
        ActionButton(win.transform, font, new Vector2(0f, -40f), new Vector2(360f, 58f), DismissWin).GetComponentInChildren<Text>().text =
            // Добавляет строковое значение `"Продолжить"`.
            "Продолжить";
        // Меняет активность объекта через `win.SetActive` с аргументами `false`.
        win.SetActive(false);

        // Проверяет условие `GameLoop.Instance != null`.
        if (GameLoop.Instance != null)
            // Добавляет `OnState` к текущему значению `GameLoop.Instance.StateChanged`.
            GameLoop.Instance.StateChanged += OnState;
    }  // Конец метода `Build`.

    // Обновляет показанные данные игрока и текст подсказки.
    public void Refresh(Inventory bag, bool hasShovel, int shovelTier, string hintLine, string shopLine)
    {  // Начало метода `Refresh`.
        // Задаёт текст `goal.text` значением `"Доберись до клада на дне ямы"`.
        goal.text = "Доберись до клада на дне ямы";
        // Задаёт текст `slots.text` значением `bag.Money` плюс `" мон.\nРюкзак "` плюс `bag.Count` плюс `"/"` плюс `bag.Capacity`.
        slots.text = bag.Money + " мон.\nРюкзак " + bag.Count + "/" + bag.Capacity;
        // Задаёт текст `hint.text` значением `hintLine`.
        hint.text = hintLine;
    }  // Конец метода `Refresh`.

    // Показывает панель нужного магазина и обновляет товары.
    public void PresentShop(ShopKind? kind, FirstPersonPlayer player)
    {  // Начало метода `PresentShop`.
        // Проверяет условие `shopPanel == null`.
        if (shopPanel == null)
            // Завершает выполнение текущего метода.
            return;
        // Вычисляет `open` как `kind.HasValue` и `player` отличается от отсутствующую ссылку (`null`).
        bool open = kind.HasValue && player != null;
        // Меняет активность объекта через `shopPanel.SetActive` с аргументами `open`.
        shopPanel.SetActive(open);
        // Проверяет условие `!open`.
        if (!open)
            // Завершает выполнение текущего метода.
            return;

        // Вычисляет `sell` как `kind.Value` равно продажа.
        bool sell = kind.Value == ShopKind.Sell;
        // Меняет активность объекта через `sellPage.SetActive` с аргументами `sell`.
        sellPage.SetActive(sell);
        // Меняет активность объекта через `upgradePage.SetActive` с аргументами `!sell`.
        upgradePage.SetActive(!sell);
        // Выбирает текст `shopTitle.text` по условию `sell`.
        shopTitle.text = sell ? "Магазин" : "Кузница";
        // Проверяет условие `sell`.
        if (sell)
            // Пересчитывает строки ресурсов и доступность продажи через `RefreshSell` с аргументами `player`.
            RefreshSell(player);
        // Обрабатывает случай, когда предыдущее условие ложно.
        else
            // Обновляет названия, цены, доступность и подсказки улучшений через `RefreshUpgrade` с аргументами `player`.
            RefreshUpgrade(player);
    }  // Конец метода `PresentShop`.

    // Передаёт код монет из настроек паузы контроллеру игрока.
    void TrySubmitCheatCode()
    {  // Начало метода `TrySubmitCheatCode`.
        // Проверяет условие `pauseCodeInput == null`.
        if (pauseCodeInput == null)
            // Завершает выполнение текущего метода.
            return;
        // Находит контроллер персонажа на сцене через `Player` и записывает результат в `player`.
        var player = Player();
        // Проверяет условие `player == null`.
        if (player == null)
            // Завершает выполнение текущего метода.
            return;
        // Проверяет условие `player.TryRedeemCode(pauseCodeInput.text)`.
        if (player.TryRedeemCode(pauseCodeInput.text))
            // Задаёт текст `pauseCodeInput.text` значением `""`.
            pauseCodeInput.text = "";
        // Вызывает `pauseCodeInput.DeactivateInputField`.
        pauseCodeInput.DeactivateInputField();
        // Проверяет условие `EventSystem.current != null`.
        if (EventSystem.current != null)
            // Вызывает `EventSystem.current.SetSelectedGameObject` с аргументами `null`.
            EventSystem.current.SetSelectedGameObject(null);
    }  // Конец метода `TrySubmitCheatCode`.

    // Обрабатывает клавиши и подтверждение ввода в интерфейсе.
    void Update()
    {  // Начало метода `Update`.
        // Проверяет условие `pauseSettings == null || !pauseSettings.activeInHierarchy`.
        if (pauseSettings == null || !pauseSettings.activeInHierarchy)
            // Завершает выполнение текущего метода.
            return;
        // Проверяет условие `pauseCodeInput != null && pauseCodeInput.isFocused`.
        if (pauseCodeInput != null && pauseCodeInput.isFocused)
            // Завершает выполнение текущего метода.
            return;
        // Задаёт `keyboard` значением текущая клавиатура `Keyboard.current`.
        var keyboard = Keyboard.current;
        // Проверяет условие `keyboard == null`.
        if (keyboard == null)
            // Завершает выполнение текущего метода.
            return;
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
        // Выводит текущие громкость и чувствительность на панели через `RefreshPauseSettings`.
        RefreshPauseSettings();
    }  // Конец метода `Update`.

    // Меняет видимость игрового интерфейса и панели паузы.
    public void SetPaused(bool value)
    {  // Начало метода `SetPaused`.
        // Проверяет условие `pauseRoot != null`.
        if (pauseRoot != null)
            // Меняет активность объекта через `pauseRoot.SetActive` с аргументами `value`.
            pauseRoot.SetActive(value);
        // Проверяет условие `!value`.
        if (!value)
            // Возвращается из настроек к кнопкам паузы через `ClosePauseSettings`.
            ClosePauseSettings();
    }  // Конец метода `SetPaused`.

    // Открывает настройки из меню паузы.
    void ShowPauseSettings()
    {  // Начало метода `ShowPauseSettings`.
        // Проверяет условие `pauseMain != null`.
        if (pauseMain != null)
            // Меняет активность объекта через `pauseMain.SetActive` с аргументами `false`.
            pauseMain.SetActive(false);
        // Проверяет условие `pauseSettings != null`.
        if (pauseSettings != null)
            // Меняет активность объекта через `pauseSettings.SetActive` с аргументами `true`.
            pauseSettings.SetActive(true);
        // Выводит текущие громкость и чувствительность на панели через `RefreshPauseSettings`.
        RefreshPauseSettings();
    }  // Конец метода `ShowPauseSettings`.

    // Возвращается из настроек к кнопкам паузы.
    public void ClosePauseSettings()
    {  // Начало метода `ClosePauseSettings`.
        // Проверяет условие `pauseSettings != null`.
        if (pauseSettings != null)
            // Меняет активность объекта через `pauseSettings.SetActive` с аргументами `false`.
            pauseSettings.SetActive(false);
        // Проверяет условие `pauseMain != null`.
        if (pauseMain != null)
            // Меняет активность объекта через `pauseMain.SetActive` с аргументами `true`.
            pauseMain.SetActive(true);
    }  // Конец метода `ClosePauseSettings`.

    // Даёт доступ к `PauseSettingsOpen` через `pauseSettings != null && pauseSettings.activeInHierarchy`.
    public bool PauseSettingsOpen => pauseSettings != null && pauseSettings.activeInHierarchy;

    // Выводит текущие громкость и чувствительность на панели.
    void RefreshPauseSettings()
    {  // Начало метода `RefreshPauseSettings`.
        // Проверяет условие `pauseVolumeLabel != null`.
        if (pauseVolumeLabel != null)
            // Задаёт текст `pauseVolumeLabel.text` значением `"Громкость "` плюс вызывает `Mathf.RoundToInt` с аргументами `GameSettings.Volume * 100f` плюс `" %"`.
            pauseVolumeLabel.text = "Громкость " + Mathf.RoundToInt(GameSettings.Volume * 100f) + " %";
        // Проверяет условие `pauseSensitivityLabel != null`.
        if (pauseSensitivityLabel != null)
            // Задаёт текст `pauseSensitivityLabel.text` значением `"Мышь "` плюс вызывает `GameSettings.Sensitivity.ToString` с аргументами `"0.00"`.
            pauseSensitivityLabel.text = "Мышь " + GameSettings.Sensitivity.ToString("0.00");
    }  // Конец метода `RefreshPauseSettings`.

    // Показывает панель победы при смене состояния партии.
    void OnState(RunState state)
    {  // Начало метода `OnState`.
        // Проверяет условие `state == RunState.Won && win != null`.
        if (state == RunState.Won && win != null)
            // Меняет активность объекта через `win.SetActive` с аргументами `true`.
            win.SetActive(true);
    }  // Конец метода `OnState`.

    // Закрывает панель победы и возобновляет игровой цикл.
    void DismissWin()
    {  // Начало метода `DismissWin`.
        // Проверяет условие `win != null`.
        if (win != null)
            // Меняет активность объекта через `win.SetActive` с аргументами `false`.
            win.SetActive(false);
        // Проверяет условие `GameLoop.Instance != null`.
        if (GameLoop.Instance != null)
            // Вызывает `GameLoop.Instance.ContinueAfterWin`.
            GameLoop.Instance.ContinueAfterWin();
    }  // Конец метода `DismissWin`.

    // Пересчитывает строки ресурсов и доступность продажи.
    void RefreshSell(FirstPersonPlayer player)
    {  // Начало метода `RefreshSell`.
        // Вызывает `player.Bag.TotalValue` и записывает результат в `value`.
        int value = player.Bag.TotalValue();
        // Проверяет условие `player.Bag.Count == 0`.
        if (player.Bag.Count == 0)
            // Задаёт текст `sellSummary.text` значением `"В рюкзаке пусто"`.
            sellSummary.text = "В рюкзаке пусто";
        // Обрабатывает случай, когда предыдущее условие ложно.
        else
        {  // Начало запасной ветки условия.
            // Создаёт экземпляр `System.Text.StringBuilder` и сохраняет ссылку в `lines`.
            var lines = new System.Text.StringBuilder();
            // Перебирает индексы начиная с `int i = 0` при условии `i < Goods.Length`.
            for (int i = 0; i < Goods.Length; i++)
            {  // Начало цикла по индексам.
                // Вызывает `player.Bag.CountOf` с аргументами `Goods[i]` и записывает результат в `count`.
                int count = player.Bag.CountOf(Goods[i]);
                // Проверяет условие `count > 0`.
                if (count > 0)
                    // Вызывает `lines.Append(ResourceInfo.Title(Goods[i])).Append(" × ").Append(count).Append` с аргументами `'\n'`.
                    lines.Append(ResourceInfo.Title(Goods[i])).Append(" × ").Append(count).Append('\n');
            }  // Конец цикла по индексам.
            // Вызывает `lines.ToString` и записывает результат в `sellSummary.text`.
            sellSummary.text = lines.ToString();
        }  // Конец запасной ветки условия.
        // Выбирает текст `sellAllButton.GetComponentInChildren<Text>().text` по условию `value` больше `0`.
        sellAllButton.GetComponentInChildren<Text>().text = value > 0 ? "Продать всё за " + value : "Продать всё";
        // Задаёт доступность для взаимодействия `sellAllButton.interactable` значением `value` больше `0`.
        sellAllButton.interactable = value > 0;
    }  // Конец метода `RefreshSell`.

    // Обновляет названия, цены, доступность и подсказки улучшений.
    void RefreshUpgrade(FirstPersonPlayer player)
    {  // Начало метода `RefreshUpgrade`.
        // Задаёт `tier` значением `player.Tier`.
        int tier = player.Tier;
        // Проверяет условие `!player.HasShovel`.
        if (!player.HasShovel)
        {  // Начало ветки условия.
            // Задаёт текст `shovelCaption.text` значением `"Сначала нужна лопата"`.
            shovelCaption.text = "Сначала нужна лопата";
            // Устанавливает доступность для взаимодействия `shovelButton.interactable` в `false`.
            shovelButton.interactable = false;
        }  // Конец ветки условия.
        // Если предыдущая проверка не сработала, проверяет `tier >= ShovelTiers.UpgradeCost.Length`.
        else if (tier >= ShovelTiers.UpgradeCost.Length)
        {  // Начало ветки условия.
            // Задаёт текст `shovelCaption.text` значением вызывает `ShovelTiers.LevelLabel` с аргументами `tier` плюс `" (максимум)"`.
            shovelCaption.text = ShovelTiers.LevelLabel(tier) + " (максимум)";
            // Устанавливает доступность для взаимодействия `shovelButton.interactable` в `false`.
            shovelButton.interactable = false;
        }  // Конец ветки условия.
        // Обрабатывает случай, когда предыдущее условие ложно.
        else
        {  // Начало запасной ветки условия.
            // Задаёт `cost` значением элемент `ShovelTiers.UpgradeCost` с индексом `tier`.
            int cost = ShovelTiers.UpgradeCost[tier];
            // Задаёт текст `shovelCaption.text` значением вызывает `ShovelTiers.LevelLabel` с аргументами `tier + 1` плюс `"\n"` плюс `cost` плюс `" монет"`.
            shovelCaption.text = ShovelTiers.LevelLabel(tier + 1) + "\n" + cost + " монет";
            // Задаёт доступность для взаимодействия `shovelButton.interactable` значением `player.Bag.Money` не меньше `cost`.
            shovelButton.interactable = player.Bag.Money >= cost;
        }  // Конец запасной ветки условия.

        // Проверяет условие `player.Bag.Capacity >= ShovelTiers.MaxSlots`.
        if (player.Bag.Capacity >= ShovelTiers.MaxSlots)
        {  // Начало ветки условия.
            // Задаёт текст `slotCaption.text` значением `"Рюкзак уже на "` плюс `player.Bag.Capacity` плюс `" слотах"`.
            slotCaption.text = "Рюкзак уже на " + player.Bag.Capacity + " слотах";
            // Устанавливает доступность для взаимодействия `slotButton.interactable` в `false`.
            slotButton.interactable = false;
        }  // Конец ветки условия.
        // Обрабатывает случай, когда предыдущее условие ложно.
        else
        {  // Начало запасной ветки условия.
            // Вызывает `ShovelTiers.NextSlotCost` с аргументами `player.Bag.Capacity` и записывает результат в `cost`.
            int cost = ShovelTiers.NextSlotCost(player.Bag.Capacity);
            // Задаёт текст `slotCaption.text` значением `"Рюкзак " + player.Bag.Capacity + " → "` плюс `(player.Bag.Capacity + 1)` плюс `"\n"` плюс `cost` плюс `" монет"`.
            slotCaption.text = "Рюкзак " + player.Bag.Capacity + " → " + (player.Bag.Capacity + 1)
                // Добавляет к формируемому значению `"\n" + cost + " монет"`.
                + "\n" + cost + " монет";
            // Задаёт доступность для взаимодействия `slotButton.interactable` значением `player.Bag.Money` не меньше `cost`.
            slotButton.interactable = player.Bag.Money >= cost;
        }  // Конец запасной ветки условия.

        // Задаёт `jump` значением `player.JumpBootsLevel`.
        int jump = player.JumpBootsLevel;
        // Проверяет условие `jump >= PlayerUpgrades.MaxJumpBootsLevel`.
        if (jump >= PlayerUpgrades.MaxJumpBootsLevel)
        {  // Начало ветки условия.
            // Задаёт текст `bootCaption.text` значением `"Четверной прыжок\nкуплен"`.
            bootCaption.text = "Четверной прыжок\nкуплен";
            // Устанавливает доступность для взаимодействия `bootButton.interactable` в `false`.
            bootButton.interactable = false;
        }  // Конец ветки условия.
        // Обрабатывает случай, когда предыдущее условие ложно.
        else
        {  // Начало запасной ветки условия.
            // Вычисляет `next` как `jump` плюс `1`.
            int next = jump + 1;
            // Задаёт `cost` значением элемент `PlayerUpgrades.JumpBootsCost` с индексом `next`.
            int cost = PlayerUpgrades.JumpBootsCost[next];
            // Задаёт текст `bootCaption.text` значением вызывает `PlayerUpgrades.JumpBootsCaption` с аргументами `next` плюс `"\n"` плюс `cost` плюс `" монет"`.
            bootCaption.text = PlayerUpgrades.JumpBootsCaption(next) + "\n" + cost + " монет";
            // Задаёт доступность для взаимодействия `bootButton.interactable` значением `player.Bag.Money` не меньше `cost`.
            bootButton.interactable = player.Bag.Money >= cost;
        }  // Конец запасной ветки условия.
    }  // Конец метода `RefreshUpgrade`.

    // Создаёт EventSystem, если сцене нужен обработчик событий интерфейса.
    static void EnsureEvents()
    {  // Начало метода `EnsureEvents`.
        // Проверяет условие `UnityEngine.Object.FindAnyObjectByType<EventSystem>() != null`.
        if (UnityEngine.Object.FindAnyObjectByType<EventSystem>() != null)
            // Завершает выполнение текущего метода.
            return;
        // Создаёт объект сцены с параметрами `"События"` и сохраняет ссылку в `events`.
        var events = new GameObject("События");
        // Добавляет компонент через `events.AddComponent<EventSystem>`.
        events.AddComponent<EventSystem>();
        // Добавляет компонент через `events.AddComponent<InputSystemUIInputModule>`.
        events.AddComponent<InputSystemUIInputModule>();
    }  // Конец метода `EnsureEvents`.

    // Находит контроллер персонажа на сцене.
    static FirstPersonPlayer Player()
    {  // Начало метода `Player`.
        // Возвращает результат вызова `UnityEngine.Object.FindAnyObjectByType<FirstPersonPlayer>()`.
        return UnityEngine.Object.FindAnyObjectByType<FirstPersonPlayer>();
    }  // Конец метода `Player`.

    // Выбирает цвет значка ресурса.
    static Color Swatch(ResourceKind kind)
    {  // Начало метода `Swatch`.
        // Выбирает ветку по значению `kind`.
        switch (kind)
        {  // Начало выбора по значению.
            // Обрабатывает значение камень. Возвращает `Palette.Stone`.
            case ResourceKind.Stone: return Palette.Stone;
            // Обрабатывает значение уголь. Возвращает `Palette.Coal`.
            case ResourceKind.Coal: return Palette.Coal;
            // Обрабатывает значение медь. Возвращает `Palette.Copper`.
            case ResourceKind.Copper: return Palette.Copper;
            // Обрабатывает значение железо. Возвращает `Palette.Iron`.
            case ResourceKind.Iron: return Palette.Iron;
            // Обрабатывает все остальные значения. Возвращает `Palette.Gold`.
            default: return Palette.Gold;
        }  // Конец выбора по значению.
    }  // Конец метода `Swatch`.

    // Создаёт и размещает текстовую подпись.
    static Text Caption(Transform parent, Font font, Vector2 place, int size, Color color)
    {  // Начало метода `Caption`.
        // Создаёт текст и привязывает его к области холста через `Label` с аргументами `parent, font, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), place, size` и записывает результат в `text`.
        var text = Label(parent, font, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), place, size);
        // Задаёт выравнивание текста `text.alignment` значением `TextAnchor.MiddleCenter`.
        text.alignment = TextAnchor.MiddleCenter;
        // Задаёт цвет `text.color` значением `color`.
        text.color = color;
        // Записывает в `text.rectTransform.pivot` двумерный вектор с параметрами `0.5f, 0.5f`.
        text.rectTransform.pivot = new Vector2(0.5f, 0.5f);
        // Возвращает `text`.
        return text;
    }  // Конец метода `Caption`.

    // Создаёт кнопку и назначает ей обработчик нажатия.
    static Button ActionButton(Transform parent, Font font, Vector2 place, Vector2 size, UnityEngine.Events.UnityAction action)
    {  // Начало метода `ActionButton`.
        // Создаёт объект сцены с параметрами `"Кнопка"` и сохраняет ссылку в `go`.
        var go = new GameObject("Кнопка");
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
        // Задаёт `colors.normalColor` значением `Palette.Sun`.
        colors.normalColor = Palette.Sun;
        // Задаёт `colors.highlightedColor` значением белый цвет `Color.white`.
        colors.highlightedColor = Color.white;
        // Задаёт `colors.pressedColor` значением `Palette.Gold`.
        colors.pressedColor = Palette.Gold;
        // Записывает в `colors.disabledColor` цвет RGBA с параметрами `0.55f, 0.52f, 0.46f, 0.55f`.
        colors.disabledColor = new Color(0.55f, 0.52f, 0.46f, 0.55f);
        // Задаёт `button.colors` значением `colors`.
        button.colors = colors;
        // Подписывает обработчик на событие через `button.onClick.AddListener` с аргументами `action`.
        button.onClick.AddListener(action);
        // Создаёт текст и привязывает его к области холста через `Label` с аргументами `go.transform, font, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), Vector2.zero, 24` и записывает результат в `caption`.
        var caption = Label(go.transform, font, new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), Vector2.zero, 24);
        // Задаёт выравнивание текста `caption.alignment` значением `TextAnchor.MiddleCenter`.
        caption.alignment = TextAnchor.MiddleCenter;
        // Задаёт цвет `caption.color` значением `Palette.Ink`.
        caption.color = Palette.Ink;
        // Записывает в `caption.rectTransform.pivot` двумерный вектор с параметрами `0.5f, 0.5f`.
        caption.rectTransform.pivot = new Vector2(0.5f, 0.5f);
        // Задаёт размер относительно привязок `caption.rectTransform.sizeDelta` значением `size`.
        caption.rectTransform.sizeDelta = size;
        // Возвращает `button`.
        return button;
    }  // Конец метода `ActionButton`.

    // Создаёт текст и привязывает его к области холста.
    static Text Label(Transform parent, Font font, Vector2 anchorMin, Vector2 anchorMax, Vector2 anchored, int size)
    {  // Начало метода `Label`.
        // Создаёт объект сцены с параметрами `"Текст"` и сохраняет ссылку в `go`.
        var go = new GameObject("Текст");
        // Привязывает к родителю через `go.transform.SetParent` с аргументами `parent, false`.
        go.transform.SetParent(parent, false);
        // Добавляет компонент через `go.AddComponent<RectTransform>` и записывает результат в `rect`.
        var rect = go.AddComponent<RectTransform>();
        // Задаёт нижняя левая привязка `rect.anchorMin` значением `anchorMin`.
        rect.anchorMin = anchorMin;
        // Задаёт верхняя правая привязка `rect.anchorMax` значением `anchorMax`.
        rect.anchorMax = anchorMax;
        // Задаёт точка опоры прямоугольника `rect.pivot` значением `anchorMax`.
        rect.pivot = anchorMax;
        // Задаёт позиция относительно привязок `rect.anchoredPosition` значением `anchored`.
        rect.anchoredPosition = anchored;
        // Записывает в `rect.sizeDelta` двумерный вектор с параметрами `900f, 120f`.
        rect.sizeDelta = new Vector2(900f, 120f);
        // Добавляет компонент через `go.AddComponent<Text>` и записывает результат в `text`.
        var text = go.AddComponent<Text>();
        // Задаёт шрифт `text.font` значением `font`.
        text.font = font;
        // Задаёт размер шрифта `text.fontSize` значением `size`.
        text.fontSize = size;
        // Задаёт выравнивание текста `text.alignment` значением `TextAnchor.UpperCenter`.
        text.alignment = TextAnchor.UpperCenter;
        // Задаёт `text.horizontalOverflow` значением `HorizontalWrapMode.Overflow`.
        text.horizontalOverflow = HorizontalWrapMode.Overflow;
        // Задаёт `text.verticalOverflow` значением `VerticalWrapMode.Overflow`.
        text.verticalOverflow = VerticalWrapMode.Overflow;
        // Возвращает `text`.
        return text;
    }  // Конец метода `Label`.
}  // Конец класса `PlayHud`.

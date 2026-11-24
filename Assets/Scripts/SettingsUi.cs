// Подключает доступ к базовым типам игрового движка.
using UnityEngine;
// Подключает доступ к элементам интерфейса Unity UI.
using UnityEngine.UI;

// Создаёт общее поле ввода кода для обеих панелей настроек.
public static class SettingsUi
{  // Начало класса `SettingsUi`.
    // Задаёт `PanelWidth` значением `640`.
    public const float PanelWidth = 640f;
    // Задаёт `PanelHeight` значением `480`.
    public const float PanelHeight = 480f;

    // Добавляет поле ввода с подсказкой и настройками ввода.
    public static InputField CodeField(Transform parent, Font font, Vector2 place, Vector2 size)
    {  // Начало метода `CodeField`.
        // Создаёт объект сцены с параметрами `"Поле кода"` и сохраняет ссылку в `go`.
        var go = new GameObject("Поле кода");
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
        // Добавляет компонент через `go.AddComponent<Image>` и записывает результат в `back`.
        var back = go.AddComponent<Image>();
        // Задаёт цвет `back.color` значением белый цвет `Color.white`.
        back.color = Color.white;

        // Создаёт объект сцены с параметрами `"Текст"` и сохраняет ссылку в `textGo`.
        var textGo = new GameObject("Текст");
        // Привязывает к родителю через `textGo.transform.SetParent` с аргументами `go.transform, false`.
        textGo.transform.SetParent(go.transform, false);
        // Добавляет компонент через `textGo.AddComponent<RectTransform>` и записывает результат в `textRect`.
        var textRect = textGo.AddComponent<RectTransform>();
        // Задаёт нижняя левая привязка `textRect.anchorMin` значением `Vector2.zero`.
        textRect.anchorMin = Vector2.zero;
        // Задаёт верхняя правая привязка `textRect.anchorMax` значением `Vector2.one`.
        textRect.anchorMax = Vector2.one;
        // Записывает в `textRect.offsetMin` двумерный вектор с параметрами `12f, 6f`.
        textRect.offsetMin = new Vector2(12f, 6f);
        // Записывает в `textRect.offsetMax` двумерный вектор с параметрами `-12f, -6f`.
        textRect.offsetMax = new Vector2(-12f, -6f);
        // Добавляет компонент через `textGo.AddComponent<Text>` и записывает результат в `text`.
        var text = textGo.AddComponent<Text>();
        // Задаёт шрифт `text.font` значением `font`.
        text.font = font;
        // Задаёт размер шрифта `text.fontSize` значением `22`.
        text.fontSize = 22;
        // Задаёт цвет `text.color` значением `Palette.Ink`.
        text.color = Palette.Ink;
        // Устанавливает `text.supportRichText` в `false`.
        text.supportRichText = false;
        // Задаёт выравнивание текста `text.alignment` значением `TextAnchor.MiddleLeft`.
        text.alignment = TextAnchor.MiddleLeft;

        // Создаёт объект сцены с параметрами `"Подсказка"` и сохраняет ссылку в `placeholderGo`.
        var placeholderGo = new GameObject("Подсказка");
        // Привязывает к родителю через `placeholderGo.transform.SetParent` с аргументами `go.transform, false`.
        placeholderGo.transform.SetParent(go.transform, false);
        // Добавляет компонент через `placeholderGo.AddComponent<RectTransform>` и записывает результат в `placeholderRect`.
        var placeholderRect = placeholderGo.AddComponent<RectTransform>();
        // Задаёт нижняя левая привязка `placeholderRect.anchorMin` значением `Vector2.zero`.
        placeholderRect.anchorMin = Vector2.zero;
        // Задаёт верхняя правая привязка `placeholderRect.anchorMax` значением `Vector2.one`.
        placeholderRect.anchorMax = Vector2.one;
        // Записывает в `placeholderRect.offsetMin` двумерный вектор с параметрами `12f, 6f`.
        placeholderRect.offsetMin = new Vector2(12f, 6f);
        // Записывает в `placeholderRect.offsetMax` двумерный вектор с параметрами `-12f, -6f`.
        placeholderRect.offsetMax = new Vector2(-12f, -6f);
        // Добавляет компонент через `placeholderGo.AddComponent<Text>` и записывает результат в `placeholder`.
        var placeholder = placeholderGo.AddComponent<Text>();
        // Задаёт шрифт `placeholder.font` значением `font`.
        placeholder.font = font;
        // Задаёт размер шрифта `placeholder.fontSize` значением `22`.
        placeholder.fontSize = 22;
        // Задаёт текст `placeholder.text` значением `"Код"`.
        placeholder.text = "Код";
        // Записывает в `placeholder.color` цвет RGBA с параметрами `0.45f, 0.42f, 0.38f, 0.85f`.
        placeholder.color = new Color(0.45f, 0.42f, 0.38f, 0.85f);
        // Задаёт выравнивание текста `placeholder.alignment` значением `TextAnchor.MiddleLeft`.
        placeholder.alignment = TextAnchor.MiddleLeft;

        // Добавляет компонент через `go.AddComponent<InputField>` и записывает результат в `input`.
        var input = go.AddComponent<InputField>();
        // Задаёт `input.textComponent` значением `text`.
        input.textComponent = text;
        // Задаёт `input.placeholder` значением `placeholder`.
        input.placeholder = placeholder;
        // Задаёт `input.lineType` значением `InputField.LineType.SingleLine`.
        input.lineType = InputField.LineType.SingleLine;
        // Задаёт `input.characterValidation` значением `InputField.CharacterValidation.Integer`.
        input.characterValidation = InputField.CharacterValidation.Integer;
        // Задаёт `input.contentType` значением `InputField.ContentType.IntegerNumber`.
        input.contentType = InputField.ContentType.IntegerNumber;
        // Возвращает `input`.
        return input;
    }  // Конец метода `CodeField`.
}  // Конец класса `SettingsUi`.
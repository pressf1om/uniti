// Подключает доступ к базовым типам игрового движка.
using UnityEngine;

// Задаёт цвета материалов и предметов игры.
public static class Palette
{  // Начало класса `Palette`.
    // Преобразует текстовый HTML-цвет в цвет Unity через `Hex` с аргументами `"#3F8F45"` и записывает результат в `Grass`.
    public static readonly Color Grass = Hex("#3F8F45");
    // Преобразует текстовый HTML-цвет в цвет Unity через `Hex` с аргументами `"#8A5A32"` и записывает результат в `Dirt`.
    public static readonly Color Dirt = Hex("#8A5A32");
    // Преобразует текстовый HTML-цвет в цвет Unity через `Hex` с аргументами `"#8E9196"` и записывает результат в `Concrete`.
    public static readonly Color Concrete = Hex("#8E9196");
    // Преобразует текстовый HTML-цвет в цвет Unity через `Hex` с аргументами `"#6B4A2B"` и записывает результат в `Fence`.
    public static readonly Color Fence = Hex("#6B4A2B");
    // Преобразует текстовый HTML-цвет в цвет Unity через `Hex` с аргументами `"#D9C7A6"` и записывает результат в `HouseWall`.
    public static readonly Color HouseWall = Hex("#D9C7A6");
    // Преобразует текстовый HTML-цвет в цвет Unity через `Hex` с аргументами `"#6E5344"` и записывает результат в `HouseRoof`.
    public static readonly Color HouseRoof = Hex("#6E5344");
    // Преобразует текстовый HTML-цвет в цвет Unity через `Hex` с аргументами `"#6E7884"` и записывает результат в `Garage`.
    public static readonly Color Garage = Hex("#6E7884");
    // Преобразует текстовый HTML-цвет в цвет Unity через `Hex` с аргументами `"#6E8F52"` и записывает результат в `Yard`.
    public static readonly Color Yard = Hex("#6E8F52");
    // Преобразует текстовый HTML-цвет в цвет Unity через `Hex` с аргументами `"#D12B2B"` и записывает результат в `Cross`.
    public static readonly Color Cross = Hex("#D12B2B");
    // Преобразует текстовый HTML-цвет в цвет Unity через `Hex` с аргументами `"#9AA0A6"` и записывает результат в `Stone`.
    public static readonly Color Stone = Hex("#9AA0A6");
    // Преобразует текстовый HTML-цвет в цвет Unity через `Hex` с аргументами `"#2A2A2A"` и записывает результат в `Coal`.
    public static readonly Color Coal = Hex("#2A2A2A");
    // Преобразует текстовый HTML-цвет в цвет Unity через `Hex` с аргументами `"#C47A3A"` и записывает результат в `Copper`.
    public static readonly Color Copper = Hex("#C47A3A");
    // Преобразует текстовый HTML-цвет в цвет Unity через `Hex` с аргументами `"#B7BDC6"` и записывает результат в `Iron`.
    public static readonly Color Iron = Hex("#B7BDC6");
    // Преобразует текстовый HTML-цвет в цвет Unity через `Hex` с аргументами `"#E2B340"` и записывает результат в `Gold`.
    public static readonly Color Gold = Hex("#E2B340");
    // Преобразует текстовый HTML-цвет в цвет Unity через `Hex` с аргументами `"#18D8FF"` и записывает результат в `Treasure`.
    public static readonly Color Treasure = Hex("#18D8FF");
    // Преобразует текстовый HTML-цвет в цвет Unity через `Hex` с аргументами `"#7A4E2D"` и записывает результат в `Handle`.
    public static readonly Color Handle = Hex("#7A4E2D");
    // Преобразует текстовый HTML-цвет в цвет Unity через `Hex` с аргументами `"#C5C8CE"` и записывает результат в `Blade`.
    public static readonly Color Blade = Hex("#C5C8CE");
    // Преобразует текстовый HTML-цвет в цвет Unity через `Hex` с аргументами `"#2C241C"` и записывает результат в `Ink`.
    public static readonly Color Ink = Hex("#2C241C");
    // Преобразует текстовый HTML-цвет в цвет Unity через `Hex` с аргументами `"#F3E6C8"` и записывает результат в `Paper`.
    public static readonly Color Paper = Hex("#F3E6C8");
    // Преобразует текстовый HTML-цвет в цвет Unity через `Hex` с аргументами `"#FFF1D6"` и записывает результат в `Sun`.
    public static readonly Color Sun = Hex("#FFF1D6");
    // Преобразует текстовый HTML-цвет в цвет Unity через `Hex` с аргументами `"#A45A48"` и записывает результат в `Brick`.
    public static readonly Color Brick = Hex("#A45A48");
    // Преобразует текстовый HTML-цвет в цвет Unity через `Hex` с аргументами `"#3A3D42"` и записывает результат в `Asphalt`.
    public static readonly Color Asphalt = Hex("#3A3D42");
    // Преобразует текстовый HTML-цвет в цвет Unity через `Hex` с аргументами `"#B7B3A8"` и записывает результат в `Sidewalk`.
    public static readonly Color Sidewalk = Hex("#B7B3A8");
    // Преобразует текстовый HTML-цвет в цвет Unity через `Hex` с аргументами `"#2F6A38"` и записывает результат в `Leaf`.
    public static readonly Color Leaf = Hex("#2F6A38");
    // Преобразует текстовый HTML-цвет в цвет Unity через `Hex` с аргументами `"#5A3A24"` и записывает результат в `Bark`.
    public static readonly Color Bark = Hex("#5A3A24");
    // Преобразует текстовый HTML-цвет в цвет Unity через `Hex` с аргументами `"#6FA0C4"` и записывает результат в `Window`.
    public static readonly Color Window = Hex("#6FA0C4");
    // Преобразует текстовый HTML-цвет в цвет Unity через `Hex` с аргументами `"#6B3A2A"` и записывает результат в `Door`.
    public static readonly Color Door = Hex("#6B3A2A");
    // Преобразует текстовый HTML-цвет в цвет Unity через `Hex` с аргументами `"#F4D27A"` и записывает результат в `Lamp`.
    public static readonly Color Lamp = Hex("#F4D27A");
    // Преобразует текстовый HTML-цвет в цвет Unity через `Hex` с аргументами `"#C43B2C"` и записывает результат в `CarRed`.
    public static readonly Color CarRed = Hex("#C43B2C");
    // Преобразует текстовый HTML-цвет в цвет Unity через `Hex` с аргументами `"#2F5FA8"` и записывает результат в `CarBlue`.
    public static readonly Color CarBlue = Hex("#2F5FA8");
    // Преобразует текстовый HTML-цвет в цвет Unity через `Hex` с аргументами `"#E8E4DC"` и записывает результат в `CarWhite`.
    public static readonly Color CarWhite = Hex("#E8E4DC");
    // Преобразует текстовый HTML-цвет в цвет Unity через `Hex` с аргументами `"#D6A31E"` и записывает результат в `CarYellow`.
    public static readonly Color CarYellow = Hex("#D6A31E");

    // Преобразует текстовый HTML-цвет в цвет Unity.
    public static Color Hex(string value)
    {  // Начало метода `Hex`.
        // Вызывает `ColorUtility.TryParseHtmlString` с аргументами `value, out var color`.
        ColorUtility.TryParseHtmlString(value, out var color);
        // Возвращает `color`.
        return color;
    }  // Конец метода `Hex`.
}  // Конец класса `Palette`.

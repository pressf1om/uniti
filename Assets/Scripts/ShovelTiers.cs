// Задаёт радиусы и цены уровней лопат и цену расширения рюкзака.
public static class ShovelTiers
{  // Начало класса `ShovelTiers`.
    // Задаёт радиусы копания по уровням лопаты `Radius` массивом `{ 0.14f, 0.28f, 0.48f, 0.72f }`.
    public static readonly float[] Radius = { 0.14f, 0.28f, 0.48f, 0.72f };
    // Задаёт цены улучшений лопаты `UpgradeCost` массивом `{ 100, 300, 1000 }`.
    public static readonly int[] UpgradeCost = { 100, 300, 1000 };
    // Задаёт предельная вместимость рюкзака `MaxSlots` значением `50`.
    public const int MaxSlots = 50;
    // Задаёт начальная вместимость рюкзака `BaseSlots` значением `5`.
    public const int BaseSlots = 5;

    // Вычисляет цену следующего слота по текущей вместимости.
    public static int NextSlotCost(int capacity)
    {  // Начало метода `NextSlotCost`.
        // Проверяет условие `capacity < BaseSlots`.
        if (capacity < BaseSlots)
            // Задаёт вместимость рюкзака `capacity` значением начальная вместимость рюкзака `BaseSlots`.
            capacity = BaseSlots;
        // Проверяет условие `capacity >= MaxSlots`.
        if (capacity >= MaxSlots)
            // Возвращает `0`.
            return 0;
        // Вычисляет `step` как вместимость рюкзака `capacity` минус начальная вместимость рюкзака `BaseSlots` плюс `1`.
        int step = capacity - BaseSlots + 1;
        // Возвращает `step` умножить на `5`.
        return step * 5;
    }  // Конец метода `NextSlotCost`.

    // Формирует текстовую подпись уровня лопаты.
    public static string LevelLabel(int nextLevel)
    {  // Начало метода `LevelLabel`.
        // Возвращает `"Уровень "` плюс `nextLevel`.
        return "Уровень " + nextLevel;
    }  // Конец метода `LevelLabel`.
}  // Конец класса `ShovelTiers`.
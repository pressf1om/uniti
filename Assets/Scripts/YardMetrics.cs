// Подключает доступ к базовым типам игрового движка.
using UnityEngine;

// Определяет размеры сетки раскопа и преобразования координат ячеек.
public static class YardMetrics
{  // Начало класса `YardMetrics`.
    // Задаёт размер стороны блока `Cell` значением `0.25`.
    public const float Cell = 0.25f;
    // Задаёт ширина квадратной сетки в ячейках `Width` значением `40`.
    public const int Width = 40;
    // Задаёт количество слоёв раскопа `Layers` значением `32`.
    public const int Layers = 32;
    // Задаёт первый столбец прохода `GateMin` значением `16`.
    public const int GateMin = 16;
    // Задаёт последний столбец прохода `GateMax` значением `23`.
    public const int GateMax = 23;
    // Задаёт дальность удара лопатой `DigReach` значением `3`.
    public const float DigReach = 3f;
    // Задаёт базовый радиус лопаты `FoundShovelRadius` значением `0.6`.
    public const float FoundShovelRadius = 0.6f;
    // Задаёт множитель глубины при упаковке ключа `PackY` значением `50`.
    public const int PackY = 50;
    // Задаёт множитель оси Z при упаковке ключа `PackZ` значением `2000`.
    public const int PackZ = 2000;

    // Вычисляет мировую позицию центра ячейки по трём индексам.
    public static Vector3 CellCenter(int ix, int iy, int iz)
    {  // Начало метода `CellCenter`.
        // Формирует вектор центра ячейки из координат X, Y и Z.
        return new Vector3(
            // Сдвигает X на половину ячейки, чтобы получить её центр.
            (ix + 0.5f) * Cell,
            // Отсчитывает каждый подземный слой вниз от уровня поверхности.
            -Cell * 0.5f - iy * Cell,
            // Сдвигает Z на половину ячейки, чтобы получить её центр.
            (iz + 0.5f) * Cell);
    }  // Конец метода `CellCenter`.

    // Проверяет, попадает ли столбец в диапазон калитки.
    public static bool IsGateColumn(int ix)
    {  // Начало метода `IsGateColumn`.
        // Возвращает индекс ячейки по X `ix` не меньше первый столбец прохода `GateMin` и индекс ячейки по X `ix` не превышает последний столбец прохода `GateMax`.
        return ix >= GateMin && ix <= GateMax;
    }  // Конец метода `IsGateColumn`.

    // Кодирует индексы ячейки одним целым числом.
    public static int Pack(int ix, int iy, int iz)
    {  // Начало метода `Pack`.
        // Укладывает координаты в отдельные разряды с помощью множителей для Y и Z.
        return (ix + 2) + iy * PackY + (iz + 2) * PackZ;
    }  // Конец метода `Pack`.

    // Восстанавливает индексы ячейки из числового ключа.
    public static void Unpack(int key, out int ix, out int iy, out int iz)
    {  // Начало метода `Unpack`.
        // Извлекает индекс Z и убирает добавленное при упаковке смещение.
        iz = key / PackZ - 2;
        // Отделяет часть ключа, где хранятся X и Y.
        int rest = key % PackZ;
        // Извлекает глубину из разрядов, выделенных под Y.
        iy = rest / PackY;
        // Извлекает индекс X и убирает добавленное при упаковке смещение.
        ix = rest % PackY - 2;
    }  // Конец метода `Unpack`.

    // Проверяет, входит ли поверхностная ячейка в крест-отметку.
    public static bool IsCrossGrass(int ix, int iy, int iz)
    {  // Начало метода `IsCrossGrass`.
        // Ограничивает рисование креста самым верхним слоем.
        if (iy != 0)
            // Возвращает `false`.
            return false;
        // Вычисляет `c` как ширина квадратной сетки в ячейках `Width` разделить на `2`.
        int c = Width / 2;
        // Задаёт `arm` значением `3`.
        int arm = 3;
        // Задаёт `thick` значением `0`.
        int thick = 0;
        // Проверяет, попала ли ячейка в горизонтальное плечо креста вдоль X.
        bool barX = Mathf.Abs(iz - c) <= thick && Mathf.Abs(ix - c) <= arm;
        // Проверяет, попала ли ячейка в перпендикулярное плечо креста вдоль Z.
        bool barZ = Mathf.Abs(ix - c) <= thick && Mathf.Abs(iz - c) <= arm;
        // Возвращает `barX` или `barZ`.
        return barX || barZ;
    }  // Конец метода `IsCrossGrass`.
}  // Конец класса `YardMetrics`.

// Подключает доступ к базовым типам игрового движка.
using UnityEngine;
// Подключает доступ к типам из `UnityEngine.Rendering`.
using UnityEngine.Rendering;

// Генерирует текстуры и материалы окружения во время работы игры.
public static class TextureFactory
{  // Начало класса `TextureFactory`.
    // Хранит ссылку на двумерную текстуру в `grass`.
    static Texture2D grass;
    // Хранит ссылку на двумерную текстуру в `brick`.
    static Texture2D brick;
    // Хранит ссылку на двумерную текстуру в `wood`.
    static Texture2D wood;
    // Хранит ссылку на двумерную текстуру в `asphalt`.
    static Texture2D asphalt;
    // Хранит ссылку на двумерную текстуру в `leaves`.
    static Texture2D leaves;
    // Хранит ссылку на двумерную текстуру в `speckled`.
    static Texture2D speckled;

    // Создаёт материал URP и настраивает цвет, текстуру и гладкость.
    public static Material Lit(Color color, Texture2D map = null, float smoothness = 0.16f, bool doubleSided = false)
    {  // Начало метода `Lit`.
        // Ищет шейдер по имени через `Shader.Find` с аргументами `"Universal Render Pipeline/Lit"` и записывает результат в `shader`.
        var shader = Shader.Find("Universal Render Pipeline/Lit");
        // Создаёт материал с параметрами `shader` и сохраняет ссылку в `material`.
        var material = new Material(shader);
        // Задаёт цветовой параметр через `material.SetColor` с аргументами `"_BaseColor", color`.
        material.SetColor("_BaseColor", color);
        // Проверяет условие `map != null`.
        if (map != null)
            // Задаёт текстуру через `material.SetTexture` с аргументами `"_BaseMap", map`.
            material.SetTexture("_BaseMap", map);
        // Задаёт числовой параметр через `material.SetFloat` с аргументами `"_Smoothness", smoothness`.
        material.SetFloat("_Smoothness", smoothness);
        // Устанавливает инстансинг материала `material.enableInstancing` в `true`.
        material.enableInstancing = true;
        // Проверяет условие `doubleSided`.
        if (doubleSided)
            // Задаёт числовой параметр через `material.SetFloat` с аргументами `"_Cull", (float)CullMode.Off`.
            material.SetFloat("_Cull", (float)CullMode.Off);
        // Возвращает `material`.
        return material;
    }  // Конец метода `Lit`.

    // Генерирует текстуру травы.
    public static Texture2D Grass()
    {  // Начало метода `Grass`.
        // Выбирает возвращаемое значение по условию `grass` существует.
        return grass != null ? grass : (grass = Noise(96, 18, 0.62f));
    }  // Конец метода `Grass`.

    // Рисует кирпичную кладку с рядами и швами.
    public static Texture2D Brick()
    {  // Начало метода `Brick`.
        // Проверяет условие `brick != null`.
        if (brick != null)
            // Возвращает `brick`.
            return brick;
        // Создаёт текстуру требуемого размера и задаёт режимы её выборки через `New` с аргументами `64` и записывает результат в `brick`.
        brick = New(64);
        // Задаёт `pixels` массивом `new Color[64 * 64]`.
        var pixels = new Color[64 * 64];
        // Перебирает индексы начиная с `int y = 0` при условии `y < 64`.
        for (int y = 0; y < 64; y++)
        {  // Начало цикла по индексам.
            // Вычисляет `row` как `y` разделить на `10`.
            int row = y / 10;
            // Вычисляет `shift` как (`row` по модулю `2`) умножить на `16`.
            int shift = (row % 2) * 16;
            // Перебирает индексы начиная с `int x = 0` при условии `x < 64`.
            for (int x = 0; x < 64; x++)
            {  // Начало цикла по индексам.
                // Вычисляет `mortar` как `y` по модулю `10` равно `0` или (`x + shift`) по модулю `32` равно `0`.
                bool mortar = y % 10 == 0 || (x + shift) % 32 == 0;
                // Вычисляет `n` как вычисляет воспроизводимое шумовое значение по координатам пикселя через `Hash` с аргументами `x, y` умножить на `0.12`.
                float n = Hash(x, y) * 0.12f;
                // Выбирает `pixels[y * 64 + x]` по условию `mortar`.
                pixels[y * 64 + x] = mortar
                    // При истинном условии выбирает `new Color(0.78f, 0.74f, 0.68f)`.
                    ? new Color(0.78f, 0.74f, 0.68f)
                    // При ложном условии выбирает `new Color(0.72f + n, 0.42f + n * 0.4f, 0.34f)`.
                    : new Color(0.72f + n, 0.42f + n * 0.4f, 0.34f);
            }  // Конец цикла по индексам.
        }  // Конец цикла по индексам.
        // Записывает цвета пикселей через `brick.SetPixels` с аргументами `pixels`.
        brick.SetPixels(pixels);
        // Вызывает `brick.Apply`.
        brick.Apply();
        // Возвращает `brick`.
        return brick;
    }  // Конец метода `Brick`.

    // Генерирует полосы и вариации цвета древесины.
    public static Texture2D Wood()
    {  // Начало метода `Wood`.
        // Проверяет условие `wood != null`.
        if (wood != null)
            // Возвращает `wood`.
            return wood;
        // Создаёт текстуру требуемого размера и задаёт режимы её выборки через `New` с аргументами `64` и записывает результат в `wood`.
        wood = New(64);
        // Задаёт `pixels` массивом `new Color[64 * 64]`.
        var pixels = new Color[64 * 64];
        // Перебирает индексы начиная с `int y = 0` при условии `y < 64`.
        for (int y = 0; y < 64; y++)
        {  // Начало цикла по индексам.
            // Перебирает индексы начиная с `int x = 0` при условии `x < 64`.
            for (int x = 0; x < 64; x++)
            {  // Начало цикла по индексам.
                // Вычисляет `grain` как вызывает `Mathf.Sin` с аргументами `(x + Hash(x, y) * 8f) * 0.4f` умножить на `0.08`.
                float grain = Mathf.Sin((x + Hash(x, y) * 8f) * 0.4f) * 0.08f;
                // Вычисляет `ring` как вызывает `Mathf.Sin` с аргументами `y * 0.35f` умножить на `0.05`.
                float ring = Mathf.Sin(y * 0.35f) * 0.05f;
                // Записывает в `pixels[y * 64 + x]` цвет RGBA с параметрами `0.55f + grain, 0.36f + ring, 0.2f`.
                pixels[y * 64 + x] = new Color(0.55f + grain, 0.36f + ring, 0.2f);
            }  // Конец цикла по индексам.
        }  // Конец цикла по индексам.
        // Записывает цвета пикселей через `wood.SetPixels` с аргументами `pixels`.
        wood.SetPixels(pixels);
        // Вызывает `wood.Apply`.
        wood.Apply();
        // Возвращает `wood`.
        return wood;
    }  // Конец метода `Wood`.

    // Генерирует мелкозернистую текстуру дороги.
    public static Texture2D Asphalt()
    {  // Начало метода `Asphalt`.
        // Выбирает возвращаемое значение по условию `asphalt` существует.
        return asphalt != null ? asphalt : (asphalt = Noise(96, 40, 0.22f));
    }  // Конец метода `Asphalt`.

    // Создаёт цветовые вариации для листвы.
    public static Texture2D Leaves()
    {  // Начало метода `Leaves`.
        // Выбирает возвращаемое значение по условию `leaves` существует.
        return leaves != null ? leaves : (leaves = Noise(64, 9, 0.5f));
    }  // Конец метода `Leaves`.

    // Создаёт пятнистую текстуру материала.
    public static Texture2D Speckled()
    {  // Начало метода `Speckled`.
        // Выбирает возвращаемое значение по условию `speckled` существует.
        return speckled != null ? speckled : (speckled = Noise(64, 22, 0.55f));
    }  // Конец метода `Speckled`.

    // Наполняет текстуру воспроизводимым шумом с указанным контрастом.
    static Texture2D Noise(int size, int seed, float contrast)
    {  // Начало метода `Noise`.
        // Создаёт текстуру требуемого размера и задаёт режимы её выборки через `New` с аргументами `size` и записывает результат в `texture`.
        var texture = New(size);
        // Задаёт `pixels` массивом `new Color[size * size]`.
        var pixels = new Color[size * size];
        // Перебирает индексы начиная с `int y = 0` при условии `y < size`.
        for (int y = 0; y < size; y++)
        {  // Начало цикла по индексам.
            // Перебирает индексы начиная с `int x = 0` при условии `x < size`.
            for (int x = 0; x < size; x++)
            {  // Начало цикла по индексам.
                // Вычисляет `n` как `0.5` плюс вызывает `Mathf.PerlinNoise` с аргументами `(x + seed) * 0.11f, (y + seed) * 0.11f` умножить на `0.35` плюс вычисляет воспроизводимое шумовое значение по координатам пикселя через `Hash` с аргументами `x + seed, y` умножить на `0.15`.
                float n = 0.5f
                    // Добавляет к формируемому значению `Mathf.PerlinNoise((x + seed) * 0.11f, (y + seed) * 0.11f) * 0.35f`.
                    + Mathf.PerlinNoise((x + seed) * 0.11f, (y + seed) * 0.11f) * 0.35f
                    // Добавляет к формируемому значению `Hash(x + seed, y) * 0.15f`.
                    + Hash(x + seed, y) * 0.15f;
                // Интерполирует между значениями через `Mathf.Lerp` с аргументами `0.5f, n, contrast` и записывает результат в `n`.
                n = Mathf.Lerp(0.5f, n, contrast);
                // Записывает в `pixels[y * size + x]` цвет RGBA с параметрами `n, n, n, 1f`.
                pixels[y * size + x] = new Color(n, n, n, 1f);
            }  // Конец цикла по индексам.
        }  // Конец цикла по индексам.
        // Записывает цвета пикселей через `texture.SetPixels` с аргументами `pixels`.
        texture.SetPixels(pixels);
        // Вызывает `texture.Apply`.
        texture.Apply();
        // Возвращает `texture`.
        return texture;
    }  // Конец метода `Noise`.

    // Создаёт текстуру требуемого размера и задаёт режимы её выборки.
    static Texture2D New(int size)
    {  // Начало метода `New`.
        // Создаёт двумерную текстуру с параметрами `size, size, TextureFormat.RGBA32, false` и сохраняет ссылку в `texture`.
        var texture = new Texture2D(size, size, TextureFormat.RGBA32, false);
        // Задаёт режим выборки за границами текстуры `texture.wrapMode` значением `TextureWrapMode.Repeat`.
        texture.wrapMode = TextureWrapMode.Repeat;
        // Задаёт фильтрация текстуры `texture.filterMode` значением `FilterMode.Bilinear`.
        texture.filterMode = FilterMode.Bilinear;
        // Возвращает `texture`.
        return texture;
    }  // Конец метода `New`.

    // Вычисляет воспроизводимое шумовое значение по координатам пикселя.
    static float Hash(int x, int y)
    {  // Начало метода `Hash`.
        // Вычисляет `h` как `x` умножить на `374761393` плюс `y` умножить на `668265263`.
        int h = x * 374761393 + y * 668265263;
        // Задаёт `h` значением (`h` исключающее ИЛИ с (`h >> 13`)) умножить на `1274126177`.
        h = (h ^ (h >> 13)) * 1274126177;
        // Возвращает (`h` побитовое И с `0x7ffffff`) разделить на `2147483647`.
        return (h & 0x7fffffff) / 2147483647f;
    }  // Конец метода `Hash`.
}  // Конец класса `TextureFactory`.

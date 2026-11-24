// Подключает доступ к базовым типам игрового движка.
using UnityEngine;

// Красит модели лопат в зависимости от уровня.
public static class ShovelTint
{  // Начало класса `ShovelTint`.
    // Выбирает цвет полотна лопаты указанного уровня.
    public static Color BladeColor(int tier)
    {  // Начало метода `BladeColor`.
        // Ограничивает значение диапазоном через `Mathf.Clamp` с аргументами `tier, 0, 3` и записывает результат в `tier`.
        tier = Mathf.Clamp(tier, 0, 3);
        // Выбирает ветку по значению `tier`.
        switch (tier)
        {  // Начало выбора по значению.
            // Обрабатывает значение `1`. Возвращает `Palette.Iron`.
            case 1: return Palette.Iron;
            // Обрабатывает значение `2`. Возвращает `Palette.Gold`.
            case 2: return Palette.Gold;
            // Обрабатывает значение `3`. Возвращает новый экземпляр `Color` с аргументами `0.48f, 0.84f, 0.98f`.
            case 3: return new Color(0.48f, 0.84f, 0.98f);
            // Обрабатывает все остальные значения. Возвращает `Palette.Blade`.
            default: return Palette.Blade;
        }  // Конец выбора по значению.
    }  // Конец метода `BladeColor`.

    // Определяет гладкость полотна по уровню.
    static float Shine(int tier)
    {  // Начало метода `Shine`.
        // Проверяет условие `tier <= 0`.
        if (tier <= 0)
            // Возвращает `0.18`.
            return 0.18f;
        // Проверяет условие `tier == 1`.
        if (tier == 1)
            // Возвращает `0.42`.
            return 0.42f;
        // Проверяет условие `tier == 2`.
        if (tier == 2)
            // Возвращает `0.58`.
            return 0.58f;
        // Возвращает `0.72`.
        return 0.72f;
    }  // Конец метода `Shine`.

    // Получает стандартный материал модели.
    static Material DefaultSkin()
    {  // Начало метода `DefaultSkin`.
        // Загружает ресурс через `Resources.Load<Texture2D>` с аргументами `"Props/shovel_texture"` и записывает результат в `map`.
        var map = Resources.Load<Texture2D>("Props/shovel_texture");
        // Возвращает результат вызова `TextureFactory.Lit(Color.white, map, 0.2f, true)`.
        return TextureFactory.Lit(Color.white, map, 0.2f, true);
    }  // Конец метода `DefaultSkin`.

    // Получает материал черенка.
    static Material HandleSkin()
    {  // Начало метода `HandleSkin`.
        // Возвращает результат вызова `TextureFactory.Lit(Palette.Handle, TextureFactory.Wood(), 0.22f, true)`.
        return TextureFactory.Lit(Palette.Handle, TextureFactory.Wood(), 0.22f, true);
    }  // Конец метода `HandleSkin`.

    // Создаёт и настраивает материал полотна.
    static Material BladeSkin(int tier)
    {  // Начало метода `BladeSkin`.
        // Выбирает цвет полотна лопаты указанного уровня через `BladeColor` с аргументами `tier` и записывает результат в `color`.
        var color = BladeColor(tier);
        // Вызывает `TextureFactory.Lit` с аргументами `color, null, Shine(tier), true` и записывает результат в `mat`.
        var mat = TextureFactory.Lit(color, null, Shine(tier), true);
        // Проверяет условие `tier >= 2`.
        if (tier >= 2)
        {  // Начало ветки условия.
            // Включает ключевое слово шейдера через `mat.EnableKeyword` с аргументами `"_EMISSION"`.
            mat.EnableKeyword("_EMISSION");
            // Задаёт цветовой параметр через `mat.SetColor` с аргументами `"_EmissionColor", color * (tier == 3 ? 0.35f : 0.18f)`.
            mat.SetColor("_EmissionColor", color * (tier == 3 ? 0.35f : 0.18f));
        }  // Конец ветки условия.
        // Возвращает `mat`.
        return mat;
    }  // Конец метода `BladeSkin`.

    // Устанавливает материалы черенка и полотна на модели лопаты.
    public static void Apply(Transform root, int tier)
    {  // Начало метода `Apply`.
        // Проверяет условие `root == null`.
        if (root == null)
            // Завершает выполнение текущего метода.
            return;
        // Проверяет условие `tier <= 0`.
        if (tier <= 0)
        {  // Начало ветки условия.
            // Восстанавливает исходные материалы модели через `RestoreDefault` с аргументами `root`.
            RestoreDefault(root);
            // Завершает выполнение текущего метода.
            return;
        }  // Конец ветки условия.
        // Получает материал черенка через `HandleSkin` и записывает результат в `handle`.
        var handle = HandleSkin();
        // Создаёт и настраивает материал полотна через `BladeSkin` с аргументами `tier` и записывает результат в `blade`.
        var blade = BladeSkin(tier);
        // Собирает компоненты в объекте и его потомках через `root.GetComponentsInChildren<Renderer>` с аргументами `true` и записывает результат в `renderers`.
        var renderers = root.GetComponentsInChildren<Renderer>(true);
        // Проверяет условие `renderers.Length == 0`.
        if (renderers.Length == 0)
            // Завершает выполнение текущего метода.
            return;
        // Задаёт `bladeRenderer` значением `-1`.
        int bladeRenderer = -1;
        // Перебирает индексы начиная с `int i = 0` при условии `i < renderers.Length`.
        for (int i = 0; i < renderers.Length; i++)
        {  // Начало цикла по индексам.
            // Проверяет условие `!IsBladePart(renderers[i].transform)`.
            if (!IsBladePart(renderers[i].transform))
                // Пропускает остаток этой итерации и переходит к следующей.
                continue;
            // Задаёт `bladeRenderer` значением `i`.
            bladeRenderer = i;
            // Выходит из текущего цикла или блока выбора.
            break;
        }  // Конец цикла по индексам.
        // Проверяет условие `bladeRenderer < 0 && renderers.Length > 1`.
        if (bladeRenderer < 0 && renderers.Length > 1)
        {  // Начало ветки условия.
            // Задаёт `top` значением `float.NegativeInfinity`.
            float top = float.NegativeInfinity;
            // Перебирает индексы начиная с `int i = 0` при условии `i < renderers.Length`.
            for (int i = 0; i < renderers.Length; i++)
            {  // Начало цикла по индексам.
                // Задаёт `y` значением `renderers[i].bounds.max.y`.
                float y = renderers[i].bounds.max.y;
                // Проверяет условие `y > top`.
                if (y > top)
                {  // Начало ветки условия.
                    // Задаёт `top` значением `y`.
                    top = y;
                    // Задаёт `bladeRenderer` значением `i`.
                    bladeRenderer = i;
                }  // Конец ветки условия.
            }  // Конец цикла по индексам.
        }  // Конец ветки условия.
        // Перебирает индексы начиная с `int i = 0` при условии `i < renderers.Length`.
        for (int i = 0; i < renderers.Length; i++)
        {  // Начало цикла по индексам.
            // Вычисляет `asBlade` как `bladeRenderer` меньше `0` или `i` равно `bladeRenderer`.
            bool asBlade = bladeRenderer < 0 || i == bladeRenderer;
            // Проверяет условие `renderers.Length == 1`.
            if (renderers.Length == 1)
                // Устанавливает `asBlade` в `true`.
                asBlade = true;
            // Назначает подходящие материалы видимой части модели через `PaintRenderer` с аргументами `renderers[i], handle, blade, asBlade`.
            PaintRenderer(renderers[i], handle, blade, asBlade);
        }  // Конец цикла по индексам.
    }  // Конец метода `Apply`.

    // Назначает подходящие материалы видимой части модели.
    static void PaintRenderer(Renderer renderer, Material handle, Material blade, bool asBlade)
    {  // Начало метода `PaintRenderer`.
        // Проверяет условие `renderer == null`.
        if (renderer == null)
            // Завершает выполнение текущего метода.
            return;
        // Задаёт список предметов в рюкзаке `slots` значением `renderer.sharedMaterials.Length`.
        int slots = renderer.sharedMaterials.Length;
        // Проверяет условие `slots <= 1`.
        if (slots <= 1)
        {  // Начало ветки условия.
            // Меняет материал выбранного рендера через `Assign` с аргументами `renderer, asBlade ? blade : handle`.
            Assign(renderer, asBlade ? blade : handle);
            // Завершает выполнение текущего метода.
            return;
        }  // Конец ветки условия.
        // Задаёт `mats` массивом `new Material[slots]`.
        var mats = new Material[slots];
        // Перебирает индексы начиная с `int i = 0` при условии `i < slots`.
        for (int i = 0; i < slots; i++)
            // Выбирает `mats[i]` по условию `i` равно `0`.
            mats[i] = i == 0 ? handle : blade;
        // Заполняет слоты материалов рендера через `AssignAll` с аргументами `renderer, mats`.
        AssignAll(renderer, mats);
    }  // Конец метода `PaintRenderer`.

    // Определяет, относится ли дочерний узел к полотну лопаты.
    static bool IsBladePart(Transform node)
    {  // Начало метода `IsBladePart`.
        // Перебирает индексы начиная с `var walk = node` при условии `walk != null`.
        for (var walk = node; walk != null; walk = walk.parent)
        {  // Начало цикла по индексам.
            // Задаёт `n` значением `walk.name`.
            string n = walk.name;
            // Проверяет условие `n == "Полотно" || n == "Лезвие"`.
            if (n == "Полотно" || n == "Лезвие")
                // Возвращает `true`.
                return true;
            // Проверяет условие `n.IndexOf("blade", System.StringComparison.OrdinalIgnoreCase) >= 0`.
            if (n.IndexOf("blade", System.StringComparison.OrdinalIgnoreCase) >= 0)
                // Возвращает `true`.
                return true;
            // Проверяет условие `n.IndexOf("metal", System.StringComparison.OrdinalIgnoreCase) >= 0`.
            if (n.IndexOf("metal", System.StringComparison.OrdinalIgnoreCase) >= 0)
                // Возвращает `true`.
                return true;
            // Проверяет условие `n.IndexOf("steel", System.StringComparison.OrdinalIgnoreCase) >= 0`.
            if (n.IndexOf("steel", System.StringComparison.OrdinalIgnoreCase) >= 0)
                // Возвращает `true`.
                return true;
            // Проверяет условие `n.IndexOf("scoop", System.StringComparison.OrdinalIgnoreCase) >= 0`.
            if (n.IndexOf("scoop", System.StringComparison.OrdinalIgnoreCase) >= 0)
                // Возвращает `true`.
                return true;
        }  // Конец цикла по индексам.
        // Возвращает `false`.
        return false;
    }  // Конец метода `IsBladePart`.

    // Восстанавливает исходные материалы модели.
    static void RestoreDefault(Transform root)
    {  // Начало метода `RestoreDefault`.
        // Получает стандартный материал модели через `DefaultSkin` и записывает результат в `skin`.
        var skin = DefaultSkin();
        // Собирает компоненты в объекте и его потомках через `root.GetComponentsInChildren<Renderer>` с аргументами `true` и записывает результат в `renderers`.
        var renderers = root.GetComponentsInChildren<Renderer>(true);
        // Перебирает индексы начиная с `int i = 0` при условии `i < renderers.Length`.
        for (int i = 0; i < renderers.Length; i++)
            // Заполняет слоты материалов рендера через `AssignAll` с аргументами `renderers[i], skin`.
            AssignAll(renderers[i], skin);
    }  // Конец метода `RestoreDefault`.

    // Меняет материал выбранного рендера.
    static void Assign(Renderer renderer, Material mat)
    {  // Начало метода `Assign`.
        // Заполняет слоты материалов рендера через `AssignAll` с аргументами `renderer, mat`.
        AssignAll(renderer, mat);
    }  // Конец метода `Assign`.

    // Заполняет слоты материалов рендера.
    static void AssignAll(Renderer renderer, Material mat)
    {  // Начало метода `AssignAll`.
        // Задаёт список предметов в рюкзаке `slots` значением `renderer.sharedMaterials.Length`.
        int slots = renderer.sharedMaterials.Length;
        // Проверяет условие `slots <= 1`.
        if (slots <= 1)
        {  // Начало ветки условия.
            // Проверяет условие `Application.isPlaying`.
            if (Application.isPlaying)
                // Задаёт материал `renderer.material` значением `mat`.
                renderer.material = mat;
            // Обрабатывает случай, когда предыдущее условие ложно.
            else
                // Задаёт общий материал `renderer.sharedMaterial` значением `mat`.
                renderer.sharedMaterial = mat;
            // Завершает выполнение текущего метода.
            return;
        }  // Конец ветки условия.
        // Задаёт `mats` массивом `new Material[slots]`.
        var mats = new Material[slots];
        // Перебирает индексы начиная с `int i = 0` при условии `i < slots`.
        for (int i = 0; i < slots; i++)
            // Задаёт `mats[i]` значением `mat`.
            mats[i] = mat;
        // Проверяет условие `Application.isPlaying`.
        if (Application.isPlaying)
            // Задаёт `renderer.materials` значением `mats`.
            renderer.materials = mats;
        // Обрабатывает случай, когда предыдущее условие ложно.
        else
            // Задаёт общие материалы `renderer.sharedMaterials` значением `mats`.
            renderer.sharedMaterials = mats;
    }  // Конец метода `AssignAll`.

    // Заполняет слоты материалов рендера.
    static void AssignAll(Renderer renderer, Material[] mats)
    {  // Начало метода `AssignAll`.
        // Проверяет условие `Application.isPlaying`.
        if (Application.isPlaying)
            // Задаёт `renderer.materials` значением `mats`.
            renderer.materials = mats;
        // Обрабатывает случай, когда предыдущее условие ложно.
        else
            // Задаёт общие материалы `renderer.sharedMaterials` значением `mats`.
            renderer.sharedMaterials = mats;
    }  // Конец метода `AssignAll`.

    // Показывает в магазине разные уровни лопат.
    public static void PaintShopRow(Transform shop)
    {  // Начало метода `PaintShopRow`.
        // Проверяет условие `shop == null`.
        if (shop == null)
            // Завершает выполнение текущего метода.
            return;
        // Создаёт экземпляр `System.Collections.Generic.List<Transform>` и сохраняет ссылку в `shovels`.
        var shovels = new System.Collections.Generic.List<Transform>();
        // Перебирает элементы `shop.GetComponentsInChildren<Transform>(true)` по одному в `part`.
        foreach (var part in shop.GetComponentsInChildren<Transform>(true))
        {  // Начало цикла по элементам.
            // Проверяет условие `part.name != "shovel"`.
            if (part.name != "shovel")
                // Пропускает остаток этой итерации и переходит к следующей.
                continue;
            // Добавляет элемент через `shovels.Add` с аргументами `part`.
            shovels.Add(part);
        }  // Конец цикла по элементам.
        // Сортирует элементы через `shovels.Sort` с аргументами `(a, b) => a.localPosition.x.CompareTo(b.localPosition.x)`.
        shovels.Sort((a, b) => a.localPosition.x.CompareTo(b.localPosition.x));
        // Перебирает индексы начиная с `int i = 0` при условии `i < shovels.Count && i < 3`.
        for (int i = 0; i < shovels.Count && i < 3; i++)
            // Устанавливает материалы черенка и полотна на модели лопаты через `Apply` с аргументами `shovels[i], i + 1`.
            Apply(shovels[i], i + 1);
    }  // Конец метода `PaintShopRow`.
}  // Конец класса `ShovelTint`.
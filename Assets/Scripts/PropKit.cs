// Подключает доступ к базовым типам игрового движка.
using UnityEngine;

// Загружает, подгоняет и раскрашивает модели предметов двора.
public static class PropKit
{  // Начало класса `PropKit`.
    // Создаёт экземпляр модели и размещает его в иерархии.
    public static Transform Spawn(string name, Transform parent, Vector3 position, Vector3 euler, float height, bool sitOnGround = true, bool alignBottom = true)
    {  // Начало метода `Spawn`.
        // Задаёт `resource` значением `name`.
        string resource = name;
        // Проверяет условие `name.StartsWith("flask")`.
        if (name.StartsWith("flask"))
            // Задаёт `resource` значением `"flask"`.
            resource = "flask";
        // Загружает ресурс через `Resources.Load<GameObject>` с аргументами `"Props/" + resource` и записывает результат в `prefab`.
        var prefab = Resources.Load<GameObject>("Props/" + resource);
        // Проверяет условие `prefab == null`.
        if (prefab == null)
            // Возвращает `null`, то есть отсутствие результата поиска.
            return null;
        // Создаёт копию объекта через `Object.Instantiate` с аргументами `prefab, parent` и записывает результат в `go`.
        var go = Object.Instantiate(prefab, parent);
        // Задаёт имя `go.name` значением `name`.
        go.name = name;
        // Задаёт локальная позиция `go.transform.localPosition` значением `position`.
        go.transform.localPosition = position;
        // Задаёт локальный поворот `go.transform.localRotation` значением нулевой поворот `Quaternion.identity`.
        go.transform.localRotation = Quaternion.identity;
        // Задаёт локальный масштаб `go.transform.localScale` значением вектор (1, 1, 1) `Vector3.one`.
        go.transform.localScale = Vector3.one;
        // Убирает коллайдеры из иерархии модели через `Strip` с аргументами `go`.
        Strip(go);
        // Проверяет условие `name == "anvil"`.
        if (name == "anvil")
            // Подбирает и назначает материал рендерам модели через `Paint` с аргументами `go, "Props/Iron Anvil", 0.32f`.
            Paint(go, "Props/Iron Anvil", 0.32f);
        // Если предыдущая проверка не сработала, проверяет `name == "mailbox"`.
        else if (name == "mailbox")
            // Подбирает и назначает материал рендерам модели через `Paint` с аргументами `go, "Props/LP_MailboxBase", 0.18f`.
            Paint(go, "Props/LP_MailboxBase", 0.18f);
        // Если предыдущая проверка не сработала, проверяет `name == "shovel"`.
        else if (name == "shovel")
            // Подбирает и назначает материал рендерам модели через `Paint` с аргументами `go, "Props/shovel_texture", 0.2f, true`.
            Paint(go, "Props/shovel_texture", 0.2f, true);
        // Если предыдущая проверка не сработала, проверяет `name.StartsWith("flask")`.
        else if (name.StartsWith("flask"))
            // Назначает материал колбы через `PaintFlask` с аргументами `go, name`.
            PaintFlask(go, name);
        // Если предыдущая проверка не сработала, проверяет `name.StartsWith("potion-")`.
        else if (name.StartsWith("potion-"))
            // Назначает материал бутылочки зелья через `PaintPotion` с аргументами `go, name`.
            PaintPotion(go, name);
        // Проверяет условие `height > 0.05f`.
        if (height > 0.05f)
            // Вызывает `KenneyKit.FitHeight` с аргументами `go.transform, height, false`.
            KenneyKit.FitHeight(go.transform, height, false);
        // Создаёт поворот из углов Эйлера через `Quaternion.Euler` с аргументами `euler` и записывает результат в `go.transform.localRotation`.
        go.transform.localRotation = Quaternion.Euler(euler);
        // Проверяет условие `alignBottom`.
        if (alignBottom)
        {  // Начало ветки условия.
            // Вызывает `KenneyKit.BoundsOf` с аргументами `go.transform` и записывает результат в `bounds`.
            var bounds = KenneyKit.BoundsOf(go.transform);
            // Выбирает значение для `targetY` по условию `sitOnGround`.
            float targetY = sitOnGround ? 0f : parent.TransformPoint(position).y;
            // Добавляет `Vector3.up * (targetY - bounds.min.y)` к текущему значению мировая позиция `go.transform.position`.
            go.transform.position += Vector3.up * (targetY - bounds.min.y);
        }  // Конец ветки условия.
        // Возвращает `go.transform`.
        return go.transform;
    }  // Конец метода `Spawn`.

    // Подбирает и назначает материал рендерам модели.
    static void Paint(GameObject go, string textureName, float smoothness, bool doubleSided = false)
    {  // Начало метода `Paint`.
        // Загружает ресурс через `Resources.Load<Texture2D>` с аргументами `textureName` и записывает результат в `map`.
        var map = Resources.Load<Texture2D>(textureName);
        // Вызывает `TextureFactory.Lit` с аргументами `Color.white, map, smoothness, doubleSided` и записывает результат в `material`.
        var material = TextureFactory.Lit(Color.white, map, smoothness, doubleSided);
        // Назначает один материал всем нужным слотам рендеров модели через `Apply` с аргументами `go, material`.
        Apply(go, material);
    }  // Конец метода `Paint`.

    // Настраивает материалы с видимостью обеих сторон поверхности.
    public static void EnsureDoubleSided(GameObject go)
    {  // Начало метода `EnsureDoubleSided`.
        // Собирает компоненты в объекте и его потомках через `go.GetComponentsInChildren<Renderer>` и записывает результат в `renderers`.
        var renderers = go.GetComponentsInChildren<Renderer>();
        // Перебирает индексы начиная с `int i = 0` при условии `i < renderers.Length`.
        for (int i = 0; i < renderers.Length; i++)
        {  // Начало цикла по индексам.
            // Задаёт `renderer` значением элемент `renderers` с индексом `i`.
            var renderer = renderers[i];
            // Проверяет условие `Application.isPlaying`.
            if (Application.isPlaying)
            {  // Начало ветки условия.
                // Задаёт `mats` значением `renderer.materials`.
                var mats = renderer.materials;
                // Перебирает индексы начиная с `int m = 0` при условии `m < mats.Length`.
                for (int m = 0; m < mats.Length; m++)
                    // Задаёт числовой параметр через `mats[m].SetFloat` с аргументами `"_Cull", (float)UnityEngine.Rendering.CullMode.Off`.
                    mats[m].SetFloat("_Cull", (float)UnityEngine.Rendering.CullMode.Off);
                // Задаёт `renderer.materials` значением `mats`.
                renderer.materials = mats;
            }  // Конец ветки условия.
            // Обрабатывает случай, когда предыдущее условие ложно.
            else
            {  // Начало запасной ветки условия.
                // Задаёт `mat` значением `renderer.sharedMaterial`.
                var mat = renderer.sharedMaterial;
                // Проверяет условие `mat != null`.
                if (mat != null)
                    // Задаёт числовой параметр через `mat.SetFloat` с аргументами `"_Cull", (float)UnityEngine.Rendering.CullMode.Off`.
                    mat.SetFloat("_Cull", (float)UnityEngine.Rendering.CullMode.Off);
            }  // Конец запасной ветки условия.
        }  // Конец цикла по индексам.
    }  // Конец метода `EnsureDoubleSided`.

    // Исправляет отображение моделей лопат внутри переданной иерархии.
    public static void FixShovelsUnder(Transform root)
    {  // Начало метода `FixShovelsUnder`.
        // Проверяет условие `root == null`.
        if (root == null)
            // Завершает выполнение текущего метода.
            return;
        // Перебирает элементы `root.GetComponentsInChildren<Transform>(true)` по одному в `transform`.
        foreach (var transform in root.GetComponentsInChildren<Transform>(true))
        {  // Начало цикла по элементам.
            // Задаёт `n` значением `transform.name`.
            string n = transform.name;
            // Проверяет условие `n.IndexOf("shovel", System.StringComparison.OrdinalIgnoreCase) >= 0 || n == "Лопата"`.
            if (n.IndexOf("shovel", System.StringComparison.OrdinalIgnoreCase) >= 0
                // Дополняет проверку условием `n == "Лопата")`.
                || n == "Лопата")
            {  // Начало ветки условия.
                // Настраивает материалы с видимостью обеих сторон поверхности через `EnsureDoubleSided` с аргументами `transform.gameObject`.
                EnsureDoubleSided(transform.gameObject);
            }  // Конец ветки условия.
        }  // Конец цикла по элементам.
    }  // Конец метода `FixShovelsUnder`.

    // Назначает материал колбы.
    static void PaintFlask(GameObject go, string name)
    {  // Начало метода `PaintFlask`.
        // Задаёт `color` значением `Palette.CarRed`.
        Color color = Palette.CarRed;
        // Проверяет условие `name.Contains("yellow")`. Задаёт `color` значением `Palette.Gold`, если условие выполнено.
        if (name.Contains("yellow")) color = Palette.Gold;
        // Если предыдущая проверка не сработала, проверяет `name.Contains("green")`.
        else if (name.Contains("green")) color = Palette.Leaf;
        // Если предыдущая проверка не сработала, проверяет `name.Contains("blue")`.
        else if (name.Contains("blue")) color = Palette.Window;
        // Если предыдущая проверка не сработала, проверяет `name.Contains("purple")`.
        else if (name.Contains("purple")) color = new Color(0.62f, 0.32f, 0.78f);
        // Загружает ресурс через `Resources.Load<Texture2D>` с аргументами `"Props/" + (name.Contains("-") ? name : "flask-red")` и записывает результат в `map`.
        var map = Resources.Load<Texture2D>("Props/" + (name.Contains("-") ? name : "flask-red"));
        // Назначает один материал всем нужным слотам рендеров модели через `Apply` с аргументами `go, TextureFactory.Lit(color, map, 0.45f)`.
        Apply(go, TextureFactory.Lit(color, map, 0.45f));
    }  // Конец метода `PaintFlask`.

    // Назначает материал бутылочки зелья.
    static void PaintPotion(GameObject go, string name)
    {  // Начало метода `PaintPotion`.
        // Задаёт `color` значением `Palette.Window`.
        Color color = Palette.Window;
        // Проверяет условие `name.Contains("red")`. Задаёт `color` значением `Palette.CarRed`, если условие выполнено.
        if (name.Contains("red")) color = Palette.CarRed;
        // Если предыдущая проверка не сработала, проверяет `name.Contains("yellow")`.
        else if (name.Contains("yellow")) color = Palette.Gold;
        // Если предыдущая проверка не сработала, проверяет `name.Contains("green")`.
        else if (name.Contains("green")) color = Palette.Leaf;
        // Если предыдущая проверка не сработала, проверяет `name.Contains("blue")`.
        else if (name.Contains("blue")) color = Palette.CarBlue;
        // Если предыдущая проверка не сработала, проверяет `name.Contains("purple")`.
        else if (name.Contains("purple")) color = new Color(0.55f, 0.28f, 0.72f);
        // Назначает один материал всем нужным слотам рендеров модели через `Apply` с аргументами `go, TextureFactory.Lit(color, null, 0.72f)`.
        Apply(go, TextureFactory.Lit(color, null, 0.72f));
    }  // Конец метода `PaintPotion`.

    // Назначает один материал всем нужным слотам рендеров модели.
    static void Apply(GameObject go, Material material)
    {  // Начало метода `Apply`.
        // Собирает компоненты в объекте и его потомках через `go.GetComponentsInChildren<Renderer>` и записывает результат в `renderers`.
        var renderers = go.GetComponentsInChildren<Renderer>();
        // Перебирает индексы начиная с `int i = 0` при условии `i < renderers.Length`.
        for (int i = 0; i < renderers.Length; i++)
        {  // Начало цикла по индексам.
            // Задаёт `renderer` значением элемент `renderers` с индексом `i`.
            var renderer = renderers[i];
            // Задаёт список предметов в рюкзаке `slots` значением `renderer.sharedMaterials.Length`.
            int slots = renderer.sharedMaterials.Length;
            // Проверяет условие `slots <= 1`.
            if (slots <= 1)
            {  // Начало ветки условия.
                // Задаёт общий материал `renderer.sharedMaterial` значением `material`.
                renderer.sharedMaterial = material;
                // Пропускает остаток этой итерации и переходит к следующей.
                continue;
            }  // Конец ветки условия.
            // Задаёт `mats` массивом `new Material[slots]`.
            var mats = new Material[slots];
            // Перебирает индексы начиная с `int m = 0` при условии `m < slots`.
            for (int m = 0; m < slots; m++)
                // Задаёт `mats[m]` значением `material`.
                mats[m] = material;
            // Задаёт общие материалы `renderer.sharedMaterials` значением `mats`.
            renderer.sharedMaterials = mats;
        }  // Конец цикла по индексам.
    }  // Конец метода `Apply`.

    // Убирает коллайдеры из иерархии модели.
    static void Strip(GameObject go)
    {  // Начало метода `Strip`.
        // Собирает компоненты в объекте и его потомках через `go.GetComponentsInChildren<Collider>` и записывает результат в `colliders`.
        var colliders = go.GetComponentsInChildren<Collider>();
        // Перебирает индексы начиная с `int i = 0` при условии `i < colliders.Length`.
        for (int i = 0; i < colliders.Length; i++)
            // Планирует удаление объекта через `Object.Destroy` с аргументами `colliders[i]`.
            Object.Destroy(colliders[i]);
    }  // Конец метода `Strip`.
}  // Конец класса `PropKit`.

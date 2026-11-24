// Подключает доступ к базовым типам игрового движка.
using UnityEngine;

// Размещает импортированные модели и подбирает для них материалы.
public static class AssetKit
{  // Начало класса `AssetKit`.
    // Создаёт экземпляр модели и размещает его в иерархии.
    public static Transform Spawn(string path, Transform parent, Vector3 localPos, Vector3 euler, float height, bool sit = true)
    {  // Начало метода `Spawn`.
        // Загружает ресурс через `Resources.Load<GameObject>` с аргументами `path` и записывает результат в `prefab`.
        var prefab = Resources.Load<GameObject>(path);
        // Проверяет условие `prefab == null`.
        if (prefab == null)
            // Возвращает `null`, то есть отсутствие результата поиска.
            return null;
        // Создаёт копию объекта через `Object.Instantiate` с аргументами `prefab, parent` и записывает результат в `go`.
        var go = Object.Instantiate(prefab, parent);
        // Вызывает `path.LastIndexOf` с аргументами `'/'` и записывает результат в `slash`.
        int slash = path.LastIndexOf('/');
        // Выбирает имя `go.name` по условию `slash` не меньше `0`.
        go.name = slash >= 0 ? path.Substring(slash + 1) : path;
        // Задаёт локальная позиция `go.transform.localPosition` значением `localPos`.
        go.transform.localPosition = localPos;
        // Создаёт поворот из углов Эйлера через `Quaternion.Euler` с аргументами `euler` и записывает результат в `go.transform.localRotation`.
        go.transform.localRotation = Quaternion.Euler(euler);
        // Задаёт локальный масштаб `go.transform.localScale` значением вектор (1, 1, 1) `Vector3.one`.
        go.transform.localScale = Vector3.one;
        // Убирает коллайдеры из иерархии модели через `Strip` с аргументами `go`.
        Strip(go);
        // Подбирает и назначает материал рендерам модели через `Paint` с аргументами `go, slash >= 0 ? path.Substring(0, slash) : ""`.
        Paint(go, slash >= 0 ? path.Substring(0, slash) : "");
        // Проверяет условие `height > 0.05f`.
        if (height > 0.05f)
            // Вызывает `KenneyKit.FitHeight` с аргументами `go.transform, height, false`.
            KenneyKit.FitHeight(go.transform, height, false);
        // Проверяет условие `sit`.
        if (sit)
        {  // Начало ветки условия.
            // Вызывает `KenneyKit.BoundsOf` с аргументами `go.transform` и записывает результат в `bounds`.
            var bounds = KenneyKit.BoundsOf(go.transform);
            // Задаёт `y` значением `parent.TransformPoint(localPos).y`.
            float y = parent.TransformPoint(localPos).y;
            // Добавляет `Vector3.up * (y - bounds.min.y)` к текущему значению мировая позиция `go.transform.position`.
            go.transform.position += Vector3.up * (y - bounds.min.y);
        }  // Конец ветки условия.
        // Возвращает `go.transform`.
        return go.transform;
    }  // Конец метода `Spawn`.

    // Ставит предмет на верхнюю поверхность опоры с заданным смещением.
    public static void RestOn(Transform item, Bounds support, Vector3 offset)
    {  // Начало метода `RestOn`.
        // Проверяет условие `item == null`.
        if (item == null)
            // Завершает выполнение текущего метода.
            return;
        // Вызывает `KenneyKit.BoundsOf` с аргументами `item` и записывает результат в `bounds`.
        var bounds = KenneyKit.BoundsOf(item);
        // Добавляет `new Vector3( support.center.x + offset.x - bounds.center.x, support.max.y + offset.y - bounds.min.y, support.center.z + offset.z - bounds.center.z)` к текущему значению мировая позиция `item.position`.
        item.position += new Vector3(
            // Передаёт аргумент `support.center.x + offset.x - bounds.center.x`.
            support.center.x + offset.x - bounds.center.x,
            // Передаёт аргумент `support.max.y + offset.y - bounds.min.y`.
            support.max.y + offset.y - bounds.min.y,
            // Передаёт аргумент `support.center.z + offset.z - bounds.center.z`.
            support.center.z + offset.z - bounds.center.z);
    }  // Конец метода `RestOn`.

    // Подбирает и назначает материал рендерам модели.
    static void Paint(GameObject go, string folder)
    {  // Начало метода `Paint`.
        // Инициализирует `map` значением `null`.
        Texture2D map = null;
        // Проверяет условие `!string.IsNullOrEmpty(folder)`.
        if (!string.IsNullOrEmpty(folder))
        {  // Начало ветки условия.
            // Загружает ресурс через `Resources.Load<Texture2D>` с аргументами `folder + "/colormap"` и записывает результат в `map`.
            map = Resources.Load<Texture2D>(folder + "/colormap");
            // Проверяет условие `map == null`.
            if (map == null)
                // Загружает ресурс через `Resources.Load<Texture2D>` с аргументами `folder + "/Textures/colormap"` и записывает результат в `map`.
                map = Resources.Load<Texture2D>(folder + "/Textures/colormap");
        }  // Конец ветки условия.
        // Вычисляет `shovel` как вызывает `go.name.IndexOf` с аргументами `"shovel", System.StringComparison.OrdinalIgnoreCase` не меньше `0`.
        bool shovel = go.name.IndexOf("shovel", System.StringComparison.OrdinalIgnoreCase) >= 0;
        // Объявляет локальную переменную `material` типа `Material`.
        Material material;
        // Проверяет условие `map != null`.
        if (map != null)
        {  // Начало ветки условия.
            // Задаёт режим выборки за границами текстуры `map.wrapMode` значением `TextureWrapMode.Clamp`.
            map.wrapMode = TextureWrapMode.Clamp;
            // Задаёт фильтрация текстуры `map.filterMode` значением `FilterMode.Point`.
            map.filterMode = FilterMode.Point;
            // Вызывает `TextureFactory.Lit` с аргументами `Color.white, map, 0.18f, shovel` и записывает результат в `material`.
            material = TextureFactory.Lit(Color.white, map, 0.18f, shovel);
        }  // Конец ветки условия.
        // Обрабатывает случай, когда предыдущее условие ложно.
        else
            // Вызывает `TextureFactory.Lit` с аргументами `Palette.Handle, TextureFactory.Wood(), 0.16f, shovel` и записывает результат в `material`.
            material = TextureFactory.Lit(Palette.Handle, TextureFactory.Wood(), 0.16f, shovel);
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
    }  // Конец метода `Paint`.

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
}  // Конец класса `AssetKit`.

// Подключает доступ к базовым типам игрового движка.
using UnityEngine;

// Загружает модели Kenney, подгоняет их габариты и добавляет физические границы.
public static class KenneyKit
{  // Начало класса `KenneyKit`.
    // Хранит ссылку на материал поверхности в `skin`.
    static Material skin;

    // Создаёт экземпляр модели и размещает его в иерархии.
    public static Transform Spawn(string name, Transform parent, Vector3 position, Vector3 euler, float scale)
    {  // Начало метода `Spawn`.
        // Загружает ресурс через `Resources.Load<GameObject>` с аргументами `"Kenney/" + name` и записывает результат в `prefab`.
        var prefab = Resources.Load<GameObject>("Kenney/" + name);
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
        // Создаёт поворот из углов Эйлера через `Quaternion.Euler` с аргументами `euler` и записывает результат в `go.transform.localRotation`.
        go.transform.localRotation = Quaternion.Euler(euler);
        // Задаёт локальный масштаб `go.transform.localScale` значением вектор (1, 1, 1) `Vector3.one` умножить на `scale`.
        go.transform.localScale = Vector3.one * scale;
        // Подбирает и назначает материал рендерам модели через `Paint` с аргументами `go`.
        Paint(go);
        // Убирает коллайдеры из иерархии модели через `StripColliders` с аргументами `go`.
        StripColliders(go);
        // Возвращает `go.transform`.
        return go.transform;
    }  // Конец метода `Spawn`.

    // Сдвигает нижнюю точку модели на уровень земли.
    public static bool SitOnGround(Transform root, float groundY = 0f)
    {  // Начало метода `SitOnGround`.
        // Подсчитывает общие габариты видимых дочерних частей через `BoundsOf` с аргументами `root` и записывает результат в `bounds`.
        var bounds = BoundsOf(root);
        // Проверяет условие `bounds.size.sqrMagnitude < 0.0001f`.
        if (bounds.size.sqrMagnitude < 0.0001f)
            // Возвращает `false`.
            return false;
        // Добавляет `Vector3.up * (groundY - bounds.min.y)` к текущему значению мировая позиция `root.position`.
        root.position += Vector3.up * (groundY - bounds.min.y);
        // Возвращает `true`.
        return true;
    }  // Конец метода `SitOnGround`.

    // Масштабирует модель с учётом ширины, глубины и предельной высоты.
    public static void FitFootprint(Transform root, float width, float depth, float maxHeight)
    {  // Начало метода `FitFootprint`.
        // Подсчитывает общие габариты видимых дочерних частей через `BoundsOf` с аргументами `root` и записывает результат в `bounds`.
        var bounds = BoundsOf(root);
        // Проверяет условие `bounds.size.x < 0.05f || bounds.size.z < 0.05f`.
        if (bounds.size.x < 0.05f || bounds.size.z < 0.05f)
            // Завершает выполнение текущего метода.
            return;
        // Выбирает наименьшее значение через `Mathf.Min` с аргументами `width / bounds.size.x, depth / bounds.size.z` и записывает результат в `scale`.
        float scale = Mathf.Min(width / bounds.size.x, depth / bounds.size.z);
        // Проверяет условие `bounds.size.y * scale > maxHeight`.
        if (bounds.size.y * scale > maxHeight)
            // Задаёт `scale` значением `maxHeight` разделить на выбирает наибольшее значение через `Mathf.Max` с аргументами `0.05f, bounds.size.y`.
            scale = maxHeight / Mathf.Max(0.05f, bounds.size.y);
        // Умножает локальный масштаб `root.localScale` на `scale`.
        root.localScale *= scale;
        // Сдвигает нижнюю точку модели на уровень земли через `SitOnGround` с аргументами `root`.
        SitOnGround(root);
    }  // Конец метода `FitFootprint`.

    // Подгоняет модель под нужную высоту и при необходимости ставит на землю.
    public static void FitHeight(Transform root, float height, bool sit = true)
    {  // Начало метода `FitHeight`.
        // Подсчитывает общие габариты видимых дочерних частей через `BoundsOf` с аргументами `root` и записывает результат в `bounds`.
        var bounds = BoundsOf(root);
        // Проверяет условие `bounds.size.y < 0.05f`.
        if (bounds.size.y < 0.05f)
            // Завершает выполнение текущего метода.
            return;
        // Умножает локальный масштаб `root.localScale` на `height / bounds.size.y`.
        root.localScale *= height / bounds.size.y;
        // Проверяет условие `sit`.
        if (sit)
            // Сдвигает нижнюю точку модели на уровень земли через `SitOnGround` с аргументами `root`.
            SitOnGround(root);
    }  // Конец метода `FitHeight`.

    // Добавляет физическое препятствие вокруг ствола.
    public static void TrunkBlock(Transform root, float radius = 0.28f)
    {  // Начало метода `TrunkBlock`.
        // Проверяет условие `root == null`.
        if (root == null)
            // Завершает выполнение текущего метода.
            return;
        // Подсчитывает общие габариты видимых дочерних частей через `BoundsOf` с аргументами `root` и записывает результат в `bounds`.
        var bounds = BoundsOf(root);
        // Добавляет компонент через `root.gameObject.AddComponent<BoxCollider>` и записывает результат в `box`.
        var box = root.gameObject.AddComponent<BoxCollider>();
        // Выбирает наибольшее значение через `Mathf.Max` с аргументами `1.2f, bounds.size.y * 0.55f` и записывает результат в `height`.
        float height = Mathf.Max(1.2f, bounds.size.y * 0.55f);
        // Вызывает `root.InverseTransformPoint` с аргументами `new Vector3(bounds.center.x, bounds.min.y + height * 0.5f, bounds.center.z)` и записывает результат в `box.center`.
        box.center = root.InverseTransformPoint(new Vector3(bounds.center.x, bounds.min.y + height * 0.5f, bounds.center.z));
        // Записывает в `box.size` трёхмерный вектор с параметрами ` (radius * 2f) / Mathf.Max(0.001f, Mathf.Abs(root.lossyScale.x)), height / Mathf.Max(0.001f, Mathf.Abs(root.lossyScale.y)), (radius * 2f) / Mathf.Max(0.001f, M…`.
        box.size = new Vector3(
            // Передаёт аргумент `(radius * 2f) / Mathf.Max(0.001f, Mathf.Abs(root.lossyScale.x))`.
            (radius * 2f) / Mathf.Max(0.001f, Mathf.Abs(root.lossyScale.x)),
            // Передаёт аргумент `height / Mathf.Max(0.001f, Mathf.Abs(root.lossyScale.y))`.
            height / Mathf.Max(0.001f, Mathf.Abs(root.lossyScale.y)),
            // Передаёт аргумент `(radius * 2f) / Mathf.Max(0.001f, Mathf.Abs(root.lossyScale.z))`.
            (radius * 2f) / Mathf.Max(0.001f, Mathf.Abs(root.lossyScale.z)));
    }  // Конец метода `TrunkBlock`.

    // Добавляет коробчатый коллайдер по габаритам модели.
    public static void Block(Transform root)
    {  // Начало метода `Block`.
        // Проверяет условие `root == null`.
        if (root == null)
            // Завершает выполнение текущего метода.
            return;
        // Подсчитывает общие габариты видимых дочерних частей через `BoundsOf` с аргументами `root` и записывает результат в `bounds`.
        var bounds = BoundsOf(root);
        // Проверяет условие `bounds.size.sqrMagnitude < 0.0001f`.
        if (bounds.size.sqrMagnitude < 0.0001f)
            // Завершает выполнение текущего метода.
            return;
        // Добавляет компонент через `root.gameObject.AddComponent<BoxCollider>` и записывает результат в `box`.
        var box = root.gameObject.AddComponent<BoxCollider>();
        // Вызывает `root.InverseTransformPoint` с аргументами `bounds.center` и записывает результат в `box.center`.
        box.center = root.InverseTransformPoint(bounds.center);
        // Записывает в `box.size` трёхмерный вектор с параметрами ` bounds.size.x / Mathf.Max(0.001f, Mathf.Abs(root.lossyScale.x)), bounds.size.y / Mathf.Max(0.001f, Mathf.Abs(root.lossyScale.y)), bounds.size.z / Mathf.Max(0.…`.
        box.size = new Vector3(
            // Передаёт аргумент `bounds.size.x / Mathf.Max(0.001f, Mathf.Abs(root.lossyScale.x))`.
            bounds.size.x / Mathf.Max(0.001f, Mathf.Abs(root.lossyScale.x)),
            // Передаёт аргумент `bounds.size.y / Mathf.Max(0.001f, Mathf.Abs(root.lossyScale.y))`.
            bounds.size.y / Mathf.Max(0.001f, Mathf.Abs(root.lossyScale.y)),
            // Передаёт аргумент `bounds.size.z / Mathf.Max(0.001f, Mathf.Abs(root.lossyScale.z))`.
            bounds.size.z / Mathf.Max(0.001f, Mathf.Abs(root.lossyScale.z)));
    }  // Конец метода `Block`.

    // Подсчитывает общие габариты видимых дочерних частей.
    public static Bounds BoundsOf(Transform root)
    {  // Начало метода `BoundsOf`.
        // Собирает компоненты в объекте и его потомках через `root.GetComponentsInChildren<Renderer>` и записывает результат в `renderers`.
        var renderers = root.GetComponentsInChildren<Renderer>();
        // Проверяет условие `renderers.Length == 0`.
        if (renderers.Length == 0)
            // Возвращает новый экземпляр `Bounds` с аргументами `root.position, Vector3.zero`.
            return new Bounds(root.position, Vector3.zero);
        // Задаёт `bounds` значением `renderers[0].bounds`.
        var bounds = renderers[0].bounds;
        // Перебирает индексы начиная с `int i = 1` при условии `i < renderers.Length`.
        for (int i = 1; i < renderers.Length; i++)
            // Вызывает `bounds.Encapsulate` с аргументами `renderers[i].bounds`.
            bounds.Encapsulate(renderers[i].bounds);
        // Возвращает `bounds`.
        return bounds;
    }  // Конец метода `BoundsOf`.

    // Подбирает и назначает материал рендерам модели.
    static void Paint(GameObject go)
    {  // Начало метода `Paint`.
        // Проверяет условие `skin == null`.
        if (skin == null)
        {  // Начало ветки условия.
            // Загружает ресурс через `Resources.Load<Texture2D>` с аргументами `"Kenney/colormap"` и записывает результат в `map`.
            var map = Resources.Load<Texture2D>("Kenney/colormap");
            // Проверяет условие `map == null`.
            if (map == null)
                // Загружает ресурс через `Resources.Load<Texture2D>` с аргументами `"Kenney/Textures/colormap"` и записывает результат в `map`.
                map = Resources.Load<Texture2D>("Kenney/Textures/colormap");
            // Проверяет условие `map != null`.
            if (map != null)
            {  // Начало ветки условия.
                // Задаёт режим выборки за границами текстуры `map.wrapMode` значением `TextureWrapMode.Clamp`.
                map.wrapMode = TextureWrapMode.Clamp;
                // Задаёт фильтрация текстуры `map.filterMode` значением `FilterMode.Point`.
                map.filterMode = FilterMode.Point;
            }  // Конец ветки условия.
            // Вызывает `TextureFactory.Lit` с аргументами `Color.white, map, 0.18f` и записывает результат в `skin`.
            skin = TextureFactory.Lit(Color.white, map, 0.18f);
        }  // Конец ветки условия.
        // Собирает компоненты в объекте и его потомках через `go.GetComponentsInChildren<Renderer>` и записывает результат в `renderers`.
        var renderers = go.GetComponentsInChildren<Renderer>();
        // Перебирает индексы начиная с `int i = 0` при условии `i < renderers.Length`.
        for (int i = 0; i < renderers.Length; i++)
            // Задаёт общий материал `renderers[i].sharedMaterial` значением `skin`.
            renderers[i].sharedMaterial = skin;
    }  // Конец метода `Paint`.

    // Убирает коллайдеры из иерархии модели.
    static void StripColliders(GameObject go)
    {  // Начало метода `StripColliders`.
        // Собирает компоненты в объекте и его потомках через `go.GetComponentsInChildren<Collider>` и записывает результат в `colliders`.
        var colliders = go.GetComponentsInChildren<Collider>();
        // Перебирает индексы начиная с `int i = 0` при условии `i < colliders.Length`.
        for (int i = 0; i < colliders.Length; i++)
            // Планирует удаление объекта через `Object.Destroy` с аргументами `colliders[i]`.
            Object.Destroy(colliders[i]);
    }  // Конец метода `StripColliders`.
}  // Конец класса `KenneyKit`.

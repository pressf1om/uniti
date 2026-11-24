// Подключает доступ к базовым типам игрового движка.
using UnityEngine;

// Загружает и подготавливает модели машин.
public static class CarKit
{  // Начало класса `CarKit`.
    // Хранит ссылку на материал поверхности в `skin`.
    static Material skin;

    // Создаёт экземпляр модели и размещает его в иерархии.
    public static Transform Spawn(string name, Transform parent, Vector3 position, Vector3 euler, float height)
    {  // Начало метода `Spawn`.
        // Загружает ресурс через `Resources.Load<GameObject>` с аргументами `"Cars/" + name` и записывает результат в `prefab`.
        var prefab = Resources.Load<GameObject>("Cars/" + name);
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
        // Задаёт локальный масштаб `go.transform.localScale` значением вектор (1, 1, 1) `Vector3.one`.
        go.transform.localScale = Vector3.one;
        // Подбирает и назначает материал рендерам модели через `Paint` с аргументами `go`.
        Paint(go);
        // Убирает коллайдеры из иерархии модели через `Strip` с аргументами `go`.
        Strip(go);
        // Проверяет условие `height > 0.05f`.
        if (height > 0.05f)
            // Вызывает `KenneyKit.FitHeight` с аргументами `go.transform, height`.
            KenneyKit.FitHeight(go.transform, height);
        // Обрабатывает случай, когда предыдущее условие ложно.
        else
            // Вызывает `KenneyKit.SitOnGround` с аргументами `go.transform`.
            KenneyKit.SitOnGround(go.transform);
        // Возвращает `go.transform`.
        return go.transform;
    }  // Конец метода `Spawn`.

    // Подбирает и назначает материал рендерам модели.
    static void Paint(GameObject go)
    {  // Начало метода `Paint`.
        // Проверяет условие `skin == null`.
        if (skin == null)
        {  // Начало ветки условия.
            // Загружает ресурс через `Resources.Load<Texture2D>` с аргументами `"Cars/colormap"` и записывает результат в `map`.
            var map = Resources.Load<Texture2D>("Cars/colormap");
            // Проверяет условие `map == null`.
            if (map == null)
                // Загружает ресурс через `Resources.Load<Texture2D>` с аргументами `"Cars/Textures/colormap"` и записывает результат в `map`.
                map = Resources.Load<Texture2D>("Cars/Textures/colormap");
            // Проверяет условие `map != null`.
            if (map != null)
            {  // Начало ветки условия.
                // Задаёт режим выборки за границами текстуры `map.wrapMode` значением `TextureWrapMode.Clamp`.
                map.wrapMode = TextureWrapMode.Clamp;
                // Задаёт фильтрация текстуры `map.filterMode` значением `FilterMode.Point`.
                map.filterMode = FilterMode.Point;
            }  // Конец ветки условия.
            // Вызывает `TextureFactory.Lit` с аргументами `Color.white, map, 0.22f` и записывает результат в `skin`.
            skin = TextureFactory.Lit(Color.white, map, 0.22f);
        }  // Конец ветки условия.
        // Собирает компоненты в объекте и его потомках через `go.GetComponentsInChildren<Renderer>` и записывает результат в `renderers`.
        var renderers = go.GetComponentsInChildren<Renderer>();
        // Перебирает индексы начиная с `int i = 0` при условии `i < renderers.Length`.
        for (int i = 0; i < renderers.Length; i++)
            // Задаёт общий материал `renderers[i].sharedMaterial` значением `skin`.
            renderers[i].sharedMaterial = skin;
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
}  // Конец класса `CarKit`.

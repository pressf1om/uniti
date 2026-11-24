// Подключает доступ к базовым типам игрового движка.
using UnityEngine;

// Подбирает модель, размер и материал найденного ресурса.
public static class RockKit
{  // Начало класса `RockKit`.
    // Создаёт экземпляр модели и размещает его в иерархии.
    public static Transform Spawn(Transform parent, Vector3 position, ResourceKind kind, float size)
    {  // Начало метода `Spawn`.
        // Загружает ресурс через `Resources.Load<GameObject>` с аргументами `"Nature/" + MeshName(kind)` и записывает результат в `prefab`.
        var prefab = Resources.Load<GameObject>("Nature/" + MeshName(kind));
        // Проверяет условие `prefab == null`.
        if (prefab == null)
            // Загружает ресурс через `Resources.Load<GameObject>` с аргументами `"Rocks/" + FallbackName(kind)` и записывает результат в `prefab`.
            prefab = Resources.Load<GameObject>("Rocks/" + FallbackName(kind));
        // Проверяет условие `prefab == null`.
        if (prefab == null)
            // Возвращает `null`, то есть отсутствие результата поиска.
            return null;
        // Создаёт копию объекта через `Object.Instantiate` с аргументами `prefab, parent` и записывает результат в `go`.
        var go = Object.Instantiate(prefab, parent);
        // Вызывает `kind.ToString` и записывает результат в `go.name`.
        go.name = kind.ToString();
        // Задаёт локальная позиция `go.transform.localPosition` значением нулевой вектор `Vector3.zero`.
        go.transform.localPosition = Vector3.zero;
        // Создаёт поворот из углов Эйлера через `Quaternion.Euler` с аргументами `0f, position.x * 40f + position.z * 17f, 0f` и записывает результат в `go.transform.localRotation`.
        go.transform.localRotation = Quaternion.Euler(0f, position.x * 40f + position.z * 17f, 0f);
        // Задаёт локальный масштаб `go.transform.localScale` значением вектор (1, 1, 1) `Vector3.one`.
        go.transform.localScale = Vector3.one;
        // Вызывает `TextureFactory.Lit` с аргументами `Tint(kind), kind == ResourceKind.Treasure ? null : TextureFactory.Speckled(), Smooth(kind)` и записывает результат в `material`.
        var material = TextureFactory.Lit(Tint(kind), kind == ResourceKind.Treasure ? null : TextureFactory.Speckled(), Smooth(kind));
        // Проверяет условие `kind == ResourceKind.Treasure`.
        if (kind == ResourceKind.Treasure)
        {  // Начало ветки условия.
            // Включает ключевое слово шейдера через `material.EnableKeyword` с аргументами `"_EMISSION"`.
            material.EnableKeyword("_EMISSION");
            // Задаёт цветовой параметр через `material.SetColor` с аргументами `"_EmissionColor", Palette.Treasure * 0.7f`.
            material.SetColor("_EmissionColor", Palette.Treasure * 0.7f);
        }  // Конец ветки условия.
        // Собирает компоненты в объекте и его потомках через `go.GetComponentsInChildren<Renderer>` и записывает результат в `renderers`.
        var renderers = go.GetComponentsInChildren<Renderer>();
        // Перебирает индексы начиная с `int i = 0` при условии `i < renderers.Length`.
        for (int i = 0; i < renderers.Length; i++)
            // Задаёт общий материал `renderers[i].sharedMaterial` значением `material`.
            renderers[i].sharedMaterial = material;
        // Собирает компоненты в объекте и его потомках через `go.GetComponentsInChildren<Collider>` и записывает результат в `colliders`.
        var colliders = go.GetComponentsInChildren<Collider>();
        // Перебирает индексы начиная с `int i = 0` при условии `i < colliders.Length`.
        for (int i = 0; i < colliders.Length; i++)
            // Планирует удаление объекта через `Object.Destroy` с аргументами `colliders[i]`.
            Object.Destroy(colliders[i]);
        // Вызывает `KenneyKit.BoundsOf` с аргументами `go.transform` и записывает результат в `bounds`.
        var bounds = KenneyKit.BoundsOf(go.transform);
        // Выбирает значение для `sx` по условию `bounds.size.x` меньше `0.01`.
        float sx = bounds.size.x < 0.01f ? 1f : size / bounds.size.x;
        // Выбирает значение для `sy` по условию `bounds.size.y` меньше `0.01`.
        float sy = bounds.size.y < 0.01f ? 1f : size / bounds.size.y;
        // Выбирает значение для `sz` по условию `bounds.size.z` меньше `0.01`.
        float sz = bounds.size.z < 0.01f ? 1f : size / bounds.size.z;
        // Умножает локальный масштаб `go.transform.localScale` на `Mathf.Min(sx, Mathf.Min(sy, sz))`.
        go.transform.localScale *= Mathf.Min(sx, Mathf.Min(sy, sz));
        // Вызывает `KenneyKit.BoundsOf` с аргументами `go.transform` и записывает результат в `bounds`.
        bounds = KenneyKit.BoundsOf(go.transform);
        // Задаёт мировая позиция `go.transform.position` значением `position` плюс `Vector3.up` умножить на (`position.y` минус `bounds.center.y`).
        go.transform.position = position + Vector3.up * (position.y - bounds.center.y);
        // Возвращает `go.transform`.
        return go.transform;
    }  // Конец метода `Spawn`.

    // Возвращает основной путь модели руды или клада.
    static string MeshName(ResourceKind kind)
    {  // Начало метода `MeshName`.
        // Выбирает ветку по значению `kind`.
        switch (kind)
        {  // Начало выбора по значению.
            // Обрабатывает значение уголь. Возвращает `"stone_smallC"`.
            case ResourceKind.Coal: return "stone_smallC";
            // Обрабатывает значение медь. Возвращает `"stone_smallE"`.
            case ResourceKind.Copper: return "stone_smallE";
            // Обрабатывает значение железо. Возвращает `"stone_smallG"`.
            case ResourceKind.Iron: return "stone_smallG";
            // Обрабатывает значение золото. Возвращает `"stone_smallA"`.
            case ResourceKind.Gold: return "stone_smallA";
            // Обрабатывает значение клад. Возвращает `"stone_smallA"`.
            case ResourceKind.Treasure: return "stone_smallA";
            // Обрабатывает все остальные значения. Возвращает `"stone_smallA"`.
            default: return "stone_smallA";
        }  // Конец выбора по значению.
    }  // Конец метода `MeshName`.

    // Выбирает запасную модель, если основная отсутствует.
    static string FallbackName(ResourceKind kind)
    {  // Начало метода `FallbackName`.
        // Выбирает ветку по значению `kind`.
        switch (kind)
        {  // Начало выбора по значению.
            // Обрабатывает значение уголь. Возвращает `"rock_smallB"`.
            case ResourceKind.Coal: return "rock_smallB";
            // Обрабатывает значение медь. Возвращает `"rock_smallC"`.
            case ResourceKind.Copper: return "rock_smallC";
            // Обрабатывает значение железо. Возвращает `"rock_smallD"`.
            case ResourceKind.Iron: return "rock_smallD";
            // Обрабатывает все остальные значения. Возвращает `"rock_smallA"`.
            default: return "rock_smallA";
        }  // Конец выбора по значению.
    }  // Конец метода `FallbackName`.

    // Возвращает гладкость материала для данного ресурса.
    static float Smooth(ResourceKind kind)
    {  // Начало метода `Smooth`.
        // Выбирает ветку по значению `kind`.
        switch (kind)
        {  // Начало выбора по значению.
            // Обрабатывает значение золото.
            case ResourceKind.Gold:
            // Обрабатывает значение клад.
            case ResourceKind.Treasure: return 0.55f;
            // Обрабатывает значение железо. Возвращает `0.4`.
            case ResourceKind.Iron: return 0.4f;
            // Обрабатывает значение уголь. Возвращает `0.08`.
            case ResourceKind.Coal: return 0.08f;
            // Обрабатывает все остальные значения. Возвращает `0.16`.
            default: return 0.16f;
        }  // Конец выбора по значению.
    }  // Конец метода `Smooth`.

    // Возвращает цвет материала для данного ресурса.
    static Color Tint(ResourceKind kind)
    {  // Начало метода `Tint`.
        // Выбирает ветку по значению `kind`.
        switch (kind)
        {  // Начало выбора по значению.
            // Обрабатывает значение уголь. Возвращает `Palette.Coal`.
            case ResourceKind.Coal: return Palette.Coal;
            // Обрабатывает значение медь. Возвращает `Palette.Copper`.
            case ResourceKind.Copper: return Palette.Copper;
            // Обрабатывает значение железо. Возвращает `Palette.Iron`.
            case ResourceKind.Iron: return Palette.Iron;
            // Обрабатывает значение золото. Возвращает `Palette.Gold`.
            case ResourceKind.Gold: return Palette.Gold;
            // Обрабатывает значение клад. Возвращает `Palette.Treasure`.
            case ResourceKind.Treasure: return Palette.Treasure;
            // Обрабатывает все остальные значения. Возвращает `Palette.Stone`.
            default: return Palette.Stone;
        }  // Конец выбора по значению.
    }  // Конец метода `Tint`.
}  // Конец класса `RockKit`.

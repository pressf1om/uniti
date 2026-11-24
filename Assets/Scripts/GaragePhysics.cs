// Подключает доступ к обобщённым спискам, словарям и множествам.
using System.Collections.Generic;
// Подключает доступ к базовым типам игрового движка.
using UnityEngine;

// Настраивает физические границы гаража, мебели и инструментов.
public static class GaragePhysics
{  // Начало класса `GaragePhysics`.
    // Задаёт `DisplayRoots` массивом `{ "Магазин" }`.
    static readonly string[] DisplayRoots = { "Магазин" };

    // Задаёт начальные элементы массива `GarageShell`.
    static readonly string[] GarageShell =
    {  // Начало списка элементов массива.
        // Добавляет строковое значение `"Гараж пол"`.
        "Гараж пол", "Гараж зад", "Гараж север", "Гараж юг", "Гараж крыша",
        // Добавляет строковое значение `"Порог"`.
        "Порог", "Окно гаража", "Рама гаража", "Балка", "Косяк"
    };  // Конец списка элементов массива.

    // Задаёт начальные элементы массива `Props`.
    static readonly string[] Props =
    {  // Начало списка элементов массива.
        // Добавляет строковое значение `"kitchenCabinet"`.
        "kitchenCabinet", "kitchenCabinetUpper", "kitchenCabinetDrawer", "bookcaseClosed",
        // Добавляет строковое значение `"workbench"`.
        "workbench", "table", "chairDesk", "desk", "sideTableDrawers", "coatRackStanding",
        // Добавляет строковое значение `"trashcan"`.
        "trashcan", "barrel", "chest", "box", "box-large", "bucket",
        // Добавляет строковое значение `"cardboardBoxClosed"`.
        "cardboardBoxClosed", "cardboardBoxOpen", "lampSquareCeiling", "kitchenBar",
        // Добавляет строковое значение `"cash-register"`.
        "cash-register", "clothing-table", "display-small", "shelf-boxes", "shelf-end",
        // Добавляет строковое значение `"shopping-basket"`.
        "shopping-basket", "stoolBar", "shovel", "tool-shovel", "tool-hammer", "Пень", "anvil",
        // Добавляет строковое значение `"traffic-light"`.
        "traffic-light", "Прилавок", "Столешница", "Тело", "Ручка", "Боёк"
    };  // Конец списка элементов массива.

    // Приводит коллайдеры и физику объектов к нужному виду.
    public static void Normalize(Transform yard)
    {  // Начало метода `Normalize`.
        // Проверяет условие `yard == null`.
        if (yard == null)
            // Завершает выполнение текущего метода.
            return;

        // Перебирает индексы начиная с `int i = 0` при условии `i < GarageShell.Length`.
        for (int i = 0; i < GarageShell.Length; i++)
        {  // Начало цикла по индексам.
            // Ищет объект по имени через `yard.Find` с аргументами `GarageShell[i]` и записывает результат в `part`.
            var part = yard.Find(GarageShell[i]);
            // Проверяет условие `part != null`.
            if (part != null)
                // Убирает коллайдеры по мешу и при необходимости добавляет коробку через `EnsureBoxCollider` с аргументами `part`.
                EnsureBoxCollider(part);
        }  // Конец цикла по индексам.

        // Перебирает индексы начиная с `int i = 0` при условии `i < DisplayRoots.Length`.
        for (int i = 0; i < DisplayRoots.Length; i++)
        {  // Начало цикла по индексам.
            // Ищет объект по имени через `yard.Find` с аргументами `DisplayRoots[i]` и записывает результат в `root`.
            var root = yard.Find(DisplayRoots[i]);
            // Проверяет условие `root != null`.
            if (root != null)
                // Пересобирает коллайдеры прилавка и декораций магазина, сохраняя взаимодействие с продавцом через `SolidifyShop` с аргументами `root`.
                SolidifyShop(root);
        }  // Конец цикла по индексам.

        // Ищет объект по имени через `yard.Find` с аргументами `"Улучшения"` и записывает результат в `smithy`.
        var smithy = yard.Find("Улучшения");
        // Проверяет условие `smithy != null`.
        if (smithy != null)
            // Обновляет коллайдеры окружения кузницы и сохраняет выделенные формы наковальни через `SolidifyUpgrade` с аргументами `smithy`.
            SolidifyUpgrade(smithy);

        // Перебирает индексы начиная с `int i = 0` при условии `i < Props.Length`.
        for (int i = 0; i < Props.Length; i++)
        {  // Начало цикла по индексам.
            // Перебирает элементы `yard.GetComponentsInChildren<Transform>(true)` по одному в `prop`.
            foreach (var prop in yard.GetComponentsInChildren<Transform>(true))
            {  // Начало цикла по элементам.
                // Проверяет условие `prop.name != Props[i] || IsProtected(prop)`.
                if (prop.name != Props[i] || IsProtected(prop))
                    // Пропускает остаток этой итерации и переходит к следующей.
                    continue;
                // Заменяет коллайдеры модели подходящей простой физической формой через `SolidifyProp` с аргументами `prop`.
                SolidifyProp(prop);
            }  // Конец цикла по элементам.
        }  // Конец цикла по индексам.
    }  // Конец метода `Normalize`.

    // Пересобирает коллайдеры прилавка и декораций магазина, сохраняя взаимодействие с продавцом.
    static void SolidifyShop(Transform shop)
    {  // Начало метода `SolidifyShop`.
        // Перебирает элементы `shop.GetComponentsInChildren<Collider>(true)` по одному в `col`.
        foreach (var col in shop.GetComponentsInChildren<Collider>(true))
        {  // Начало цикла по элементам.
            // Проверяет условие `col.GetComponentInParent<Seller>() != null`.
            if (col.GetComponentInParent<Seller>() != null)
                // Пропускает остаток этой итерации и переходит к следующей.
                continue;
            // Проверяет условие `col.GetComponent<SmithyPhysicsHull>() != null`.
            if (col.GetComponent<SmithyPhysicsHull>() != null)
                // Пропускает остаток этой итерации и переходит к следующей.
                continue;
            // Проверяет условие `col.GetComponentInParent<AnvilShop>() != null`.
            if (col.GetComponentInParent<AnvilShop>() != null)
                // Пропускает остаток этой итерации и переходит к следующей.
                continue;
            // Удаляет коллайдер способом, подходящим для текущего режима через `Drop` с аргументами `col`.
            Drop(col);
        }  // Конец цикла по элементам.

        // Создаёт экземпляр `HashSet<Transform>` и сохраняет ссылку в `seen`.
        var seen = new HashSet<Transform>();
        // Перебирает элементы `shop.GetComponentsInChildren<Renderer>(true)` по одному в `renderer`.
        foreach (var renderer in shop.GetComponentsInChildren<Renderer>(true))
        {  // Начало цикла по элементам.
            // Проверяет условие `renderer.GetComponentInParent<Seller>() != null`.
            if (renderer.GetComponentInParent<Seller>() != null)
                // Пропускает остаток этой итерации и переходит к следующей.
                continue;
            // Поднимается от видимой детали к корню модели непосредственно под магазином через `AssetRootUnder` с аргументами `shop, renderer.transform` и записывает результат в `piece`.
            var piece = AssetRootUnder(shop, renderer.transform);
            // Проверяет условие `piece == null || seen.Contains(piece)`.
            if (piece == null || seen.Contains(piece))
                // Пропускает остаток этой итерации и переходит к следующей.
                continue;
            // Добавляет элемент через `seen.Add` с аргументами `piece`.
            seen.Add(piece);
            // Заменяет коллайдеры модели подходящей простой физической формой через `SolidifyProp` с аргументами `piece`.
            SolidifyProp(piece);
        }  // Конец цикла по элементам.
    }  // Конец метода `SolidifyShop`.

    // Обновляет коллайдеры окружения кузницы и сохраняет выделенные формы наковальни.
    public static void SolidifyUpgrade(Transform smithy)
    {  // Начало метода `SolidifyUpgrade`.
        // Проверяет условие `smithy == null`.
        if (smithy == null)
            // Завершает выполнение текущего метода.
            return;

        // Перебирает элементы `smithy.GetComponentsInChildren<Collider>(true)` по одному в `col`.
        foreach (var col in smithy.GetComponentsInChildren<Collider>(true))
        {  // Начало цикла по элементам.
            // Проверяет условие `col.GetComponent<SmithyPhysicsHull>() != null`.
            if (col.GetComponent<SmithyPhysicsHull>() != null)
                // Пропускает остаток этой итерации и переходит к следующей.
                continue;
            // Проверяет условие `col.GetComponentInParent<AnvilShop>() != null`.
            if (col.GetComponentInParent<AnvilShop>() != null)
                // Пропускает остаток этой итерации и переходит к следующей.
                continue;
            // Удаляет коллайдер способом, подходящим для текущего режима через `Drop` с аргументами `col`.
            Drop(col);
        }  // Конец цикла по элементам.

        // Создаёт экземпляр `HashSet<Transform>` и сохраняет ссылку в `seen`.
        var seen = new HashSet<Transform>();
        // Перебирает элементы `smithy.GetComponentsInChildren<Renderer>(true)` по одному в `renderer`.
        foreach (var renderer in smithy.GetComponentsInChildren<Renderer>(true))
        {  // Начало цикла по элементам.
            // Проверяет условие `renderer.GetComponentInParent<SmithyPhysicsHull>() != null`.
            if (renderer.GetComponentInParent<SmithyPhysicsHull>() != null)
                // Пропускает остаток этой итерации и переходит к следующей.
                continue;
            // Поднимается от видимой детали к корню модели непосредственно под магазином через `AssetRootUnder` с аргументами `smithy, renderer.transform` и записывает результат в `piece`.
            var piece = AssetRootUnder(smithy, renderer.transform);
            // Проверяет условие `piece == null || seen.Contains(piece)`.
            if (piece == null || seen.Contains(piece))
                // Пропускает остаток этой итерации и переходит к следующей.
                continue;
            // Добавляет элемент через `seen.Add` с аргументами `piece`.
            seen.Add(piece);
            // Заменяет коллайдеры модели подходящей простой физической формой через `SolidifyProp` с аргументами `piece`.
            SolidifyProp(piece);
        }  // Конец цикла по элементам.
    }  // Конец метода `SolidifyUpgrade`.

    // Поднимается от видимой детали к корню модели непосредственно под магазином.
    static Transform AssetRootUnder(Transform shop, Transform leaf)
    {  // Начало метода `AssetRootUnder`.
        // Задаёт `cur` значением `leaf`.
        var cur = leaf;
        // Повторяет действия, пока выполняется `cur.parent` существует и `cur.parent` отличается от `shop`.
        while (cur.parent != null && cur.parent != shop)
            // Задаёт `cur` значением `cur.parent`.
            cur = cur.parent;
        // Выбирает возвращаемое значение по условию `cur.parent` равно `shop`.
        return cur.parent == shop ? cur : null;
    }  // Конец метода `AssetRootUnder`.

    // Заменяет коллайдеры модели подходящей простой физической формой.
    static void SolidifyProp(Transform prop)
    {  // Начало метода `SolidifyProp`.
        // Проверяет условие `prop == null || IsProtected(prop)`.
        if (prop == null || IsProtected(prop))
            // Завершает выполнение текущего метода.
            return;
        // Проверяет условие `prop.GetComponent<SmithyPhysicsHull>() != null`.
        if (prop.GetComponent<SmithyPhysicsHull>() != null)
            // Завершает выполнение текущего метода.
            return;
        // Проверяет условие `prop.GetComponent<ShopCounter>() != null`.
        if (prop.GetComponent<ShopCounter>() != null)
            // Завершает выполнение текущего метода.
            return;
        // Проверяет условие `prop.GetComponentInChildren<Renderer>() == null`.
        if (prop.GetComponentInChildren<Renderer>() == null)
            // Завершает выполнение текущего метода.
            return;

        // Перебирает элементы `prop.GetComponentsInChildren<Collider>(true)` по одному в `col`.
        foreach (var col in prop.GetComponentsInChildren<Collider>(true))
            // Удаляет коллайдер способом, подходящим для текущего режима через `Drop` с аргументами `col`.
            Drop(col);

        // Проверяет условие `prop.GetComponent<Collider>() == null`.
        if (prop.GetComponent<Collider>() == null)
            // Вызывает `KenneyKit.Block` с аргументами `prop`.
            KenneyKit.Block(prop);

        // Проверяет условие `prop.GetComponent<Collider>() == null`.
        if (prop.GetComponent<Collider>() == null)
            // Добавляет коробчатый коллайдер по габаритам модели с поправкой на масштаб через `FallbackFromBounds` с аргументами `prop`.
            FallbackFromBounds(prop);

        // Проверяет условие `prop.GetComponent<Collider>() == null && prop.name == "anvil"`.
        if (prop.GetComponent<Collider>() == null && prop.name == "anvil")
            // Добавляет запасную физическую форму с заданным центром и размером через `FallbackBox` с аргументами `prop, new Vector3(0.62f, 0.38f, 0.45f), new Vector3(0f, 0.2f, 0f)`.
            FallbackBox(prop, new Vector3(0.62f, 0.38f, 0.45f), new Vector3(0f, 0.2f, 0f));
        // Проверяет условие `prop.GetComponent<Collider>() == null && prop.name == "Пень"`.
        if (prop.GetComponent<Collider>() == null && prop.name == "Пень")
            // Добавляет запасную физическую форму с заданным центром и размером через `FallbackBox` с аргументами `prop, new Vector3(0.92f, 0.38f, 0.92f), new Vector3(0f, 0.36f, 0f)`.
            FallbackBox(prop, new Vector3(0.92f, 0.38f, 0.92f), new Vector3(0f, 0.36f, 0f));
        // Проверяет условие `prop.GetComponent<Collider>() == null && prop.name == "stoolBar"`.
        if (prop.GetComponent<Collider>() == null && prop.name == "stoolBar")
            // Добавляет запасную физическую форму с заданным центром и размером через `FallbackBox` с аргументами `prop, new Vector3(0.42f, 0.52f, 0.42f), new Vector3(0f, 0.26f, 0f)`.
            FallbackBox(prop, new Vector3(0.42f, 0.52f, 0.42f), new Vector3(0f, 0.26f, 0f));
    }  // Конец метода `SolidifyProp`.

    // Добавляет коробчатый коллайдер по габаритам модели с поправкой на масштаб.
    static void FallbackFromBounds(Transform prop)
    {  // Начало метода `FallbackFromBounds`.
        // Вызывает `KenneyKit.BoundsOf` с аргументами `prop` и записывает результат в `bounds`.
        var bounds = KenneyKit.BoundsOf(prop);
        // Проверяет условие `bounds.size.sqrMagnitude < 0.0001f`.
        if (bounds.size.sqrMagnitude < 0.0001f)
            // Завершает выполнение текущего метода.
            return;
        // Добавляет компонент через `prop.gameObject.AddComponent<BoxCollider>` и записывает результат в `box`.
        var box = prop.gameObject.AddComponent<BoxCollider>();
        // Вызывает `prop.InverseTransformPoint` с аргументами `bounds.center` и записывает результат в `box.center`.
        box.center = prop.InverseTransformPoint(bounds.center);
        // Задаёт `scale` значением `prop.lossyScale`.
        var scale = prop.lossyScale;
        // Записывает в `box.size` трёхмерный вектор с параметрами ` bounds.size.x / Mathf.Max(0.001f, Mathf.Abs(scale.x)), bounds.size.y / Mathf.Max(0.001f, Mathf.Abs(scale.y)), bounds.size.z / Mathf.Max(0.001f, Mathf.Abs(scal…`.
        box.size = new Vector3(
            // Передаёт аргумент `bounds.size.x / Mathf.Max(0.001f, Mathf.Abs(scale.x))`.
            bounds.size.x / Mathf.Max(0.001f, Mathf.Abs(scale.x)),
            // Передаёт аргумент `bounds.size.y / Mathf.Max(0.001f, Mathf.Abs(scale.y))`.
            bounds.size.y / Mathf.Max(0.001f, Mathf.Abs(scale.y)),
            // Передаёт аргумент `bounds.size.z / Mathf.Max(0.001f, Mathf.Abs(scale.z))`.
            bounds.size.z / Mathf.Max(0.001f, Mathf.Abs(scale.z)));
    }  // Конец метода `FallbackFromBounds`.

    // Убирает коллайдеры по мешу и при необходимости добавляет коробку.
    static void EnsureBoxCollider(Transform part)
    {  // Начало метода `EnsureBoxCollider`.
        // Перебирает элементы `part.GetComponentsInChildren<MeshCollider>(true)` по одному в `mesh`.
        foreach (var mesh in part.GetComponentsInChildren<MeshCollider>(true))
            // Удаляет коллайдер способом, подходящим для текущего режима через `Drop` с аргументами `mesh`.
            Drop(mesh);
        // Проверяет условие `part.GetComponent<BoxCollider>() == null`.
        if (part.GetComponent<BoxCollider>() == null)
            // Вызывает `KenneyKit.Block` с аргументами `part`.
            KenneyKit.Block(part);
    }  // Конец метода `EnsureBoxCollider`.

    // Добавляет запасную физическую форму с заданным центром и размером.
    static void FallbackBox(Transform prop, Vector3 size, Vector3 center)
    {  // Начало метода `FallbackBox`.
        // Добавляет компонент через `prop.gameObject.AddComponent<BoxCollider>` и записывает результат в `box`.
        var box = prop.gameObject.AddComponent<BoxCollider>();
        // Задаёт центр `box.center` значением `center`.
        box.center = center;
        // Задаёт размер `box.size` значением `size`.
        box.size = size;
    }  // Конец метода `FallbackBox`.

    // Проверяет, входит ли имя в список обрабатываемых предметов.
    static bool IsPropName(string name)
    {  // Начало метода `IsPropName`.
        // Перебирает индексы начиная с `int i = 0` при условии `i < Props.Length`.
        for (int i = 0; i < Props.Length; i++)
        {  // Начало цикла по индексам.
            // Проверяет условие `Props[i] == name`.
            if (Props[i] == name)
                // Возвращает `true`.
                return true;
        }  // Конец цикла по индексам.
        // Возвращает `false`.
        return false;
    }  // Конец метода `IsPropName`.

    // Проверяет, относится ли объект к иерархии, которую требуется сохранить.
    static bool IsProtected(Transform t)
    {  // Начало метода `IsProtected`.
        // Задаёт `p` значением `t`.
        var p = t;
        // Повторяет действия, пока выполняется `p` существует.
        while (p != null)
        {  // Начало цикла по условию.
            // Задаёт `n` значением `p.name`.
            string n = p.name;
            // Проверяет условие `n is "Город" or "Газон" or "Раскоп" or "Навигация" or "Продавец" or "Лопата" or "Атмосфера"`.
            if (n is "Город" or "Газон" or "Раскоп" or "Навигация" or "Продавец" or "Лопата" or "Атмосфера")
                // Возвращает `true`.
                return true;
            // Проверяет условие `n.StartsWith("Земля") || n.StartsWith("Дорога") || n.StartsWith("Тротуар") || n == "Бровка"`.
            if (n.StartsWith("Земля") || n.StartsWith("Дорога") || n.StartsWith("Тротуар") || n == "Бровка")
                // Возвращает `true`.
                return true;
            // Задаёт `p` значением `p.parent`.
            p = p.parent;
        }  // Конец цикла по условию.
        // Возвращает `false`.
        return false;
    }  // Конец метода `IsProtected`.

    // Удаляет коллайдер способом, подходящим для текущего режима.
    static void Drop(Collider col)
    {  // Начало метода `Drop`.
        // Проверяет условие `col == null`.
        if (col == null)
            // Завершает выполнение текущего метода.
            return;
        // Проверяет условие `Application.isPlaying`.
        if (Application.isPlaying)
            // Планирует удаление объекта через `Object.Destroy` с аргументами `col`.
            Object.Destroy(col);
        // Обрабатывает случай, когда предыдущее условие ложно.
        else
            // Сразу удаляет объект через `Object.DestroyImmediate` с аргументами `col`.
            Object.DestroyImmediate(col);
    }  // Конец метода `Drop`.
}  // Конец класса `GaragePhysics`.
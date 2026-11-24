// Подключает доступ к базовым типам игрового движка.
using UnityEngine;

/// <summary>Стабильные коллайдеры кузницы: наковальня, пень, молоток (мирная ориентация).</summary>
public class SmithyPhysicsHull : MonoBehaviour  // Помечает физическую форму детали кузницы.
{  // Начало класса `SmithyPhysicsHull`.
}  // Конец класса `SmithyPhysicsHull`.

// Делает коллайдеры кузницы устойчивыми и привязывает магазин к наковальне.
public static class SmithyPhysics
{  // Начало класса `SmithyPhysics`.
    // Задаёт `HullName` значением `"Наковальня-коллайдер"`.
    const string HullName = "Наковальня-коллайдер";
    // Задаёт `SmithyName` значением `"Улучшения"`.
    const string SmithyName = "Улучшения";

    // Устанавливает физические формы наковальни, пня и молотка.
    public static void Pin(Transform yard)
    {  // Начало метода `Pin`.
        // Ищет корень кузницы во дворе через `FindSmithy` с аргументами `yard` и записывает результат в `smithy`.
        var smithy = FindSmithy(yard);
        // Проверяет условие `smithy == null`.
        if (smithy == null)
            // Завершает выполнение текущего метода.
            return;

        // Убирает прежний триггер кузницы через `ClearLegacySmithyTrigger` с аргументами `smithy`.
        ClearLegacySmithyTrigger(smithy);
        // Создаёт или обновляет физическую форму конкретной детали через `PinPiece` с аргументами `smithy, FindAnvil(smithy), HullName, true, 0.94f`.
        PinPiece(smithy, FindAnvil(smithy), HullName, true, 0.94f);
        // Создаёт или обновляет физическую форму конкретной детали через `PinPiece` с аргументами `smithy, FindNamed(smithy, "Пень"), "Пень-коллайдер", false, 1f`.
        PinPiece(smithy, FindNamed(smithy, "Пень"), "Пень-коллайдер", false, 1f);
        // Создаёт или обновляет физическую форму конкретной детали через `PinPiece` с аргументами `smithy, FindNamed(smithy, "tool-hammer"), "Молоток-коллайдер", false, 0.92f`.
        PinPiece(smithy, FindNamed(smithy, "tool-hammer"), "Молоток-коллайдер", false, 0.92f);
    }  // Конец метода `Pin`.

    // Убирает прежний триггер кузницы.
    static void ClearLegacySmithyTrigger(Transform smithy)
    {  // Начало метода `ClearLegacySmithyTrigger`.
        // Ищет компонент через `smithy.GetComponent<ShopCounter>` и записывает результат в `rootCounter`.
        var rootCounter = smithy.GetComponent<ShopCounter>();
        // Проверяет условие `rootCounter != null && rootCounter is not AnvilShop`.
        if (rootCounter != null && rootCounter is not AnvilShop)
        {  // Начало ветки условия.
            // Проверяет условие `Application.isPlaying`.
            if (Application.isPlaying)
                // Планирует удаление объекта через `Object.Destroy` с аргументами `rootCounter`.
                Object.Destroy(rootCounter);
            // Обрабатывает случай, когда предыдущее условие ложно.
            else
                // Сразу удаляет объект через `Object.DestroyImmediate` с аргументами `rootCounter`.
                Object.DestroyImmediate(rootCounter);
        }  // Конец ветки условия.
        // Ищет компонент через `smithy.GetComponent<BoxCollider>` и записывает результат в `rootBox`.
        var rootBox = smithy.GetComponent<BoxCollider>();
        // Проверяет условие `rootBox != null`.
        if (rootBox != null)
        {  // Начало ветки условия.
            // Проверяет условие `Application.isPlaying`.
            if (Application.isPlaying)
                // Планирует удаление объекта через `Object.Destroy` с аргументами `rootBox`.
                Object.Destroy(rootBox);
            // Обрабатывает случай, когда предыдущее условие ложно.
            else
                // Сразу удаляет объект через `Object.DestroyImmediate` с аргументами `rootBox`.
                Object.DestroyImmediate(rootBox);
        }  // Конец ветки условия.
    }  // Конец метода `ClearLegacySmithyTrigger`.

    // Создаёт или обновляет физическую форму конкретной детали.
    static void PinPiece(Transform smithy, Transform visual, string hullName, bool shop, float scale)
    {  // Начало метода `PinPiece`.
        // Проверяет условие `visual == null || visual.GetComponentInChildren<Renderer>() == null`.
        if (visual == null || visual.GetComponentInChildren<Renderer>() == null)
            // Завершает выполнение текущего метода.
            return;

        // Вызывает `KenneyKit.BoundsOf` с аргументами `visual` и записывает результат в `bounds`.
        var bounds = KenneyKit.BoundsOf(visual);
        // Проверяет условие `bounds.size.sqrMagnitude < 0.0001f`.
        if (bounds.size.sqrMagnitude < 0.0001f)
            // Завершает выполнение текущего метода.
            return;

        // Ищет объект по имени через `smithy.Find` с аргументами `hullName` и записывает результат в `hull`.
        var hull = smithy.Find(hullName);
        // Проверяет условие `hull == null`.
        if (hull == null)
        {  // Начало ветки условия.
            // Создаёт объект сцены с параметрами `hullName` и сохраняет ссылку в `go`.
            var go = new GameObject(hullName);
            // Задаёт `hull` значением `go.transform`.
            hull = go.transform;
            // Добавляет компонент через `go.AddComponent<SmithyPhysicsHull>`.
            go.AddComponent<SmithyPhysicsHull>();
        }  // Конец ветки условия.
        // Привязывает к родителю через `hull.SetParent` с аргументами `smithy, true`.
        hull.SetParent(smithy, true);
        // Задаёт мировая позиция `hull.position` значением `bounds.center`.
        hull.position = bounds.center;
        // Задаёт мировой поворот `hull.rotation` значением нулевой поворот `Quaternion.identity`.
        hull.rotation = Quaternion.identity;
        // Задаёт локальный масштаб `hull.localScale` значением вектор (1, 1, 1) `Vector3.one`.
        hull.localScale = Vector3.one;

        // Ищет компонент через `hull.GetComponent<BoxCollider>` и записывает результат в `box`.
        var box = hull.GetComponent<BoxCollider>();
        // Проверяет условие `box == null`.
        if (box == null)
            // Добавляет компонент через `hull.gameObject.AddComponent<BoxCollider>` и записывает результат в `box`.
            box = hull.gameObject.AddComponent<BoxCollider>();
        // Устанавливает режим триггера `box.isTrigger` в `false`.
        box.isTrigger = false;
        // Задаёт центр `box.center` значением нулевой вектор `Vector3.zero`.
        box.center = Vector3.zero;
        // Задаёт размер `box.size` значением `bounds.size` умножить на `scale`.
        box.size = bounds.size * scale;

        // Проверяет условие `shop`.
        if (shop)
        {  // Начало ветки условия.
            // Ищет компонент через `hull.GetComponent<AnvilShop>` и записывает результат в `counter`.
            var counter = hull.GetComponent<AnvilShop>();
            // Проверяет условие `counter == null`.
            if (counter == null)
                // Добавляет компонент через `hull.gameObject.AddComponent<AnvilShop>` и записывает результат в `counter`.
                counter = hull.gameObject.AddComponent<AnvilShop>();
            // Задаёт `counter.Kind` значением улучшения.
            counter.Kind = ShopKind.Upgrade;

            // Ищет компонент через `visual.GetComponent<AnvilShop>` и записывает результат в `onAnvil`.
            var onAnvil = visual.GetComponent<AnvilShop>();
            // Проверяет условие `onAnvil != null`.
            if (onAnvil != null)
            {  // Начало ветки условия.
                // Проверяет условие `Application.isPlaying`.
                if (Application.isPlaying)
                    // Планирует удаление объекта через `Object.Destroy` с аргументами `onAnvil`.
                    Object.Destroy(onAnvil);
                // Обрабатывает случай, когда предыдущее условие ложно.
                else
                    // Сразу удаляет объект через `Object.DestroyImmediate` с аргументами `onAnvil`.
                    Object.DestroyImmediate(onAnvil);
            }  // Конец ветки условия.
        }  // Конец ветки условия.
    }  // Конец метода `PinPiece`.

    // Ищет корень кузницы во дворе.
    static Transform FindSmithy(Transform yard)
    {  // Начало метода `FindSmithy`.
        // Проверяет условие `yard == null`.
        if (yard == null)
            // Возвращает `null`, то есть отсутствие результата поиска.
            return null;
        // Ищет объект по имени через `yard.Find` с аргументами `SmithyName` и записывает результат в `direct`.
        var direct = yard.Find(SmithyName);
        // Проверяет условие `direct != null`.
        if (direct != null)
            // Возвращает `direct`.
            return direct;
        // Перебирает элементы `yard.GetComponentsInChildren<Transform>(true)` по одному в `child`.
        foreach (var child in yard.GetComponentsInChildren<Transform>(true))
        {  // Начало цикла по элементам.
            // Проверяет условие `child.name == SmithyName`.
            if (child.name == SmithyName)
                // Возвращает `child`.
                return child;
        }  // Конец цикла по элементам.
        // Возвращает `null`, то есть отсутствие результата поиска.
        return null;
    }  // Конец метода `FindSmithy`.

    // Ищет наковальню внутри кузницы.
    static Transform FindAnvil(Transform smithy)
    {  // Начало метода `FindAnvil`.
        // Ищет объект по имени через `smithy.Find` с аргументами `"anvil"` и записывает результат в `direct`.
        var direct = smithy.Find("anvil");
        // Проверяет условие `direct != null`.
        if (direct != null)
            // Возвращает `direct`.
            return direct;
        // Перебирает элементы `smithy.GetComponentsInChildren<Transform>(true)` по одному в `child`.
        foreach (var child in smithy.GetComponentsInChildren<Transform>(true))
        {  // Начало цикла по элементам.
            // Проверяет условие `child.name == "anvil" || child.name == "Тело"`.
            if (child.name == "anvil" || child.name == "Тело")
                // Возвращает `child`.
                return child;
        }  // Конец цикла по элементам.
        // Возвращает `null`, то есть отсутствие результата поиска.
        return null;
    }  // Конец метода `FindAnvil`.

    // Ищет деталь кузницы по имени.
    static Transform FindNamed(Transform smithy, string name)
    {  // Начало метода `FindNamed`.
        // Ищет объект по имени через `smithy.Find` с аргументами `name` и записывает результат в `direct`.
        var direct = smithy.Find(name);
        // Проверяет условие `direct != null`.
        if (direct != null)
            // Возвращает `direct`.
            return direct;
        // Перебирает элементы `smithy.GetComponentsInChildren<Transform>(true)` по одному в `child`.
        foreach (var child in smithy.GetComponentsInChildren<Transform>(true))
        {  // Начало цикла по элементам.
            // Проверяет условие `child.name == name`.
            if (child.name == name)
                // Возвращает `child`.
                return child;
        }  // Конец цикла по элементам.
        // Возвращает `null`, то есть отсутствие результата поиска.
        return null;
    }  // Конец метода `FindNamed`.
}  // Конец класса `SmithyPhysics`.
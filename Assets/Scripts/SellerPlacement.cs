// Подключает доступ к базовым типам игрового движка.
using UnityEngine;

// Позиционирует продавца с помощью специальной опоры магазина.
public static class SellerPlacement
{  // Начало класса `SellerPlacement`.
    // Задаёт `AnchorName` значением `"Продавец-точка"`.
    const string AnchorName = "Продавец-точка";

    // Находит или создаёт точку размещения продавца у прилавка.
    public static void EnsureAnchor(Transform shop)
    {  // Начало метода `EnsureAnchor`.
        // Проверяет условие `shop == null`.
        if (shop == null)
            // Завершает выполнение текущего метода.
            return;
        // Ищет объект по имени через `shop.Find` с аргументами `AnchorName` и записывает результат в `anchor`.
        var anchor = shop.Find(AnchorName);
        // Проверяет условие `anchor == null`.
        if (anchor == null)
        {  // Начало ветки условия.
            // Создаёт объект сцены с параметрами `AnchorName` и сохраняет ссылку в `go`.
            var go = new GameObject(AnchorName);
            // Задаёт `anchor` значением `go.transform`.
            anchor = go.transform;
            // Привязывает к родителю через `anchor.SetParent` с аргументами `shop, false`.
            anchor.SetParent(shop, false);
        }  // Конец ветки условия.
        // Ищет подходящий прилавок в иерархии магазина через `FindCounter` с аргументами `shop` и записывает результат в `counter`.
        var counter = FindCounter(shop);
        // Проверяет условие `counter == null`.
        if (counter == null)
        {  // Начало ветки условия.
            // Записывает в `anchor.localPosition` трёхмерный вектор с параметрами `0f, 0f, 0.54f`.
            anchor.localPosition = new Vector3(0f, 0f, 0.54f);
            // Создаёт поворот из углов Эйлера через `Quaternion.Euler` с аргументами `0f, 180f, 0f` и записывает результат в `anchor.localRotation`.
            anchor.localRotation = Quaternion.Euler(0f, 180f, 0f);
            // Завершает выполнение текущего метода.
            return;
        }  // Конец ветки условия.

        // Вызывает `KenneyKit.BoundsOf` с аргументами `counter` и записывает результат в `bounds`.
        var bounds = KenneyKit.BoundsOf(counter);
        // Задаёт `towardCustomer` значением `counter.forward`.
        Vector3 towardCustomer = counter.forward;
        // Вычисляет `foot` как `bounds.center` минус `towardCustomer` умножить на (`bounds.extents.z` плюс `0.28f`).
        Vector3 foot = bounds.center - towardCustomer * (bounds.extents.z + 0.28f);
        // Задаёт `foot.y` значением `shop.position.y`.
        foot.y = shop.position.y;
        // Задаёт мировая позиция `anchor.position` значением `foot`.
        anchor.position = foot;
        // Вызывает `Quaternion.LookRotation` с аргументами `towardCustomer, Vector3.up` и записывает результат в `anchor.rotation`.
        anchor.rotation = Quaternion.LookRotation(towardCustomer, Vector3.up);
    }  // Конец метода `EnsureAnchor`.

    // Привязывает позицию продавца к точке у прилавка.
    public static void SnapSeller(Transform seller, Transform shop)
    {  // Начало метода `SnapSeller`.
        // Проверяет условие `seller == null || shop == null`.
        if (seller == null || shop == null)
            // Завершает выполнение текущего метода.
            return;
        // Ищет объект по имени через `shop.Find` с аргументами `AnchorName` и записывает результат в `anchor`.
        var anchor = shop.Find(AnchorName);
        // Проверяет условие `anchor != null`.
        if (anchor != null)
        {  // Начало ветки условия.
            // Привязывает к родителю через `seller.SetParent` с аргументами `shop, true`.
            seller.SetParent(shop, true);
            // Задаёт мировая позиция `seller.position` значением `anchor.position`.
            seller.position = anchor.position;
            // Задаёт мировой поворот `seller.rotation` значением `anchor.rotation`.
            seller.rotation = anchor.rotation;
            // Завершает выполнение текущего метода.
            return;
        }  // Конец ветки условия.

        // Находит или создаёт точку размещения продавца у прилавка через `EnsureAnchor` с аргументами `shop`.
        EnsureAnchor(shop);
        // Привязывает позицию продавца к точке у прилавка через `SnapSeller` с аргументами `seller, shop`.
        SnapSeller(seller, shop);
    }  // Конец метода `SnapSeller`.

    // Ищет подходящий прилавок в иерархии магазина.
    static Transform FindCounter(Transform shop)
    {  // Начало метода `FindCounter`.
        // Ищет объект по имени через `shop.Find` с аргументами `"kitchenBar"` и записывает результат в `direct`.
        var direct = shop.Find("kitchenBar");
        // Проверяет условие `direct != null`.
        if (direct != null)
            // Возвращает `direct`.
            return direct;
        // Ищет объект по имени через `shop.Find` с аргументами `"Прилавок"` и записывает результат в `direct`.
        direct = shop.Find("Прилавок");
        // Проверяет условие `direct != null`.
        if (direct != null)
            // Возвращает `direct`.
            return direct;
        // Перебирает элементы `shop` по одному в `child`.
        foreach (Transform child in shop)
        {  // Начало цикла по элементам.
            // Проверяет условие `child.name == "kitchenBar" || child.name == "Прилавок"`.
            if (child.name == "kitchenBar" || child.name == "Прилавок")
                // Возвращает `child`.
                return child;
        }  // Конец цикла по элементам.
        // Возвращает `null`, то есть отсутствие результата поиска.
        return null;
    }  // Конец метода `FindCounter`.
}  // Конец класса `SellerPlacement`.
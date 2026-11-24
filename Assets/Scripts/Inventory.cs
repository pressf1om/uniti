// Подключает доступ к обобщённым спискам, словарям и множествам.
using System.Collections.Generic;
// Подключает доступ к базовым типам игрового движка.
using UnityEngine;

// Хранит предметы, вместимость и баланс игрока.
public class Inventory : MonoBehaviour
{  // Начало класса `Inventory`.
    // Задаёт вместимость рюкзака `Capacity` значением `5`.
    public int Capacity = 5;
    // Хранит баланс монет `Money`.
    public int Money;
    // Создаёт экземпляр `List<ResourceKind>` и сохраняет ссылку в `slots`.
    readonly List<ResourceKind> slots = new List<ResourceKind>();

    // Даёт доступ к `Count` через `slots.Count`.
    public int Count => slots.Count;
    // Даёт доступ к `IsFull` через `slots.Count >= Capacity`.
    public bool IsFull => slots.Count >= Capacity;
    // Даёт доступ к `Slots` через `slots`.
    public IReadOnlyList<ResourceKind> Slots => slots;

    // Кладёт обычный ресурс в рюкзак при наличии места.
    public bool TryAdd(ResourceKind kind)
    {  // Начало метода `TryAdd`.
        // Проверяет условие `kind == ResourceKind.None || kind == ResourceKind.Treasure || IsFull`.
        if (kind == ResourceKind.None || kind == ResourceKind.Treasure || IsFull)
            // Возвращает `false`.
            return false;
        // Добавляет элемент через `slots.Add` с аргументами `kind`.
        slots.Add(kind);
        // Возвращает `true`.
        return true;
    }  // Конец метода `TryAdd`.

    // Считает предметы выбранного вида.
    public int CountOf(ResourceKind kind)
    {  // Начало метода `CountOf`.
        // Задаёт `count` значением `0`.
        int count = 0;
        // Перебирает индексы начиная с `int i = 0` при условии `i < slots.Count`.
        for (int i = 0; i < slots.Count; i++)
        {  // Начало цикла по индексам.
            // Проверяет условие `slots[i] == kind`.
            if (slots[i] == kind)
                // Увеличивает `count` на единицу.
                count++;
        }  // Конец цикла по индексам.
        // Возвращает `count`.
        return count;
    }  // Конец метода `CountOf`.

    // Извлекает указанное количество ресурсов и возвращает фактическое число.
    public int Take(ResourceKind kind, int amount)
    {  // Начало метода `Take`.
        // Задаёт `removed` значением `0`.
        int removed = 0;
        // Перебирает индексы начиная с `int i = slots.Count - 1` при условии `i >= 0 && removed < amount`.
        for (int i = slots.Count - 1; i >= 0 && removed < amount; i--)
        {  // Начало цикла по индексам.
            // Проверяет условие `slots[i] != kind`.
            if (slots[i] != kind)
                // Пропускает остаток этой итерации и переходит к следующей.
                continue;
        // Удаляет элемент по индексу через `slots.RemoveAt` с аргументами `i`.
        slots.RemoveAt(i);
        // Увеличивает `removed` на единицу.
        removed++;
    }  // Конец цикла по индексам.
    // Возвращает `removed`.
    return removed;
}  // Конец метода `Take`.

    // Восстанавливает содержимое рюкзака по количествам ресурсов.
    public void Fill(int stone, int coal, int copper, int iron, int gold)
    {  // Начало метода `Fill`.
        // Очищает содержимое через `slots.Clear`.
        slots.Clear();
        // Добавляет несколько одинаковых предметов в пределах вместимости через `AddMany` с аргументами `ResourceKind.Stone, stone`.
        AddMany(ResourceKind.Stone, stone);
        // Добавляет несколько одинаковых предметов в пределах вместимости через `AddMany` с аргументами `ResourceKind.Coal, coal`.
        AddMany(ResourceKind.Coal, coal);
        // Добавляет несколько одинаковых предметов в пределах вместимости через `AddMany` с аргументами `ResourceKind.Copper, copper`.
        AddMany(ResourceKind.Copper, copper);
        // Добавляет несколько одинаковых предметов в пределах вместимости через `AddMany` с аргументами `ResourceKind.Iron, iron`.
        AddMany(ResourceKind.Iron, iron);
        // Добавляет несколько одинаковых предметов в пределах вместимости через `AddMany` с аргументами `ResourceKind.Gold, gold`.
        AddMany(ResourceKind.Gold, gold);
    }  // Конец метода `Fill`.

    // Суммирует продажную стоимость всех предметов рюкзака.
    public int TotalValue()
    {  // Начало метода `TotalValue`.
        // Задаёт `value` значением `0`.
        int value = 0;
        // Перебирает индексы начиная с `int i = 0` при условии `i < slots.Count`.
        for (int i = 0; i < slots.Count; i++)
            // Добавляет `ResourceInfo.Price(slots[i])` к текущему значению `value`.
            value += ResourceInfo.Price(slots[i]);
        // Возвращает `value`.
        return value;
    }  // Конец метода `TotalValue`.

    // Очищает рюкзак и возвращает суммарную стоимость.
    public int TakeAll()
    {  // Начало метода `TakeAll`.
        // Суммирует продажную стоимость всех предметов рюкзака через `TotalValue` и записывает результат в `value`.
        int value = TotalValue();
        // Очищает содержимое через `slots.Clear`.
        slots.Clear();
        // Возвращает `value`.
        return value;
    }  // Конец метода `TakeAll`.

    // Добавляет несколько одинаковых предметов в пределах вместимости.
    void AddMany(ResourceKind kind, int count)
    {  // Начало метода `AddMany`.
        // Перебирает индексы начиная с `int i = 0` при условии `i < count && slots.Count < Capacity`.
        for (int i = 0; i < count && slots.Count < Capacity; i++)
            // Добавляет элемент через `slots.Add` с аргументами `kind`.
            slots.Add(kind);
    }  // Конец метода `AddMany`.
}  // Конец класса `Inventory`.

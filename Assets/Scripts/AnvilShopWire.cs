// Подключает доступ к базовым типам игрового движка.
using UnityEngine;

/// <summary>Устарело: используйте SmithyPhysics.Pin.</summary>
public static class AnvilShopWire  // Передаёт настройку кузницы актуальному коду коллайдеров.
{  // Начало класса `AnvilShopWire`.
    // Вызывает настройку физических форм кузницы для переданного двора.
    public static void Apply(Transform yard)
    {  // Начало метода `Apply`.
        // Вызывает `SmithyPhysics.Pin` с аргументами `yard`.
        SmithyPhysics.Pin(yard);
    }  // Конец метода `Apply`.
}  // Конец класса `AnvilShopWire`.
// Подключает доступ к базовым типам игрового движка.
using UnityEngine;

/// <summary>Повторно ставит коллайдеры кузницы после прогрузки мешей.</summary>
public class SmithyPhysicsRefresh : MonoBehaviour  // Переустанавливает коллайдеры, когда меши кузницы доступны.
{  // Начало класса `SmithyPhysicsRefresh`.
    // Выполняет начальную настройку компонента перед первым кадром.
    void Start()
    {  // Начало метода `Start`.
        // Ищет объект по имени через `transform.Find` с аргументами `"Двор"` и записывает результат в `yard`.
        var yard = transform.Find("Двор");
        // Проверяет условие `yard == null`.
        if (yard == null)
            // Задаёт `yard` значением `transform`.
            yard = transform;
        // Вызывает `SmithyPhysics.Pin` с аргументами `yard`.
        SmithyPhysics.Pin(yard);
    }  // Конец метода `Start`.
}  // Конец класса `SmithyPhysicsRefresh`.
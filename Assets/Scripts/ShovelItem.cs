// Подключает доступ к базовым типам игрового движка.
using UnityEngine;

// Разрешает этому компоненту работать и при редактировании сцены.
[ExecuteAlways]
// Позволяет взять лежащую во дворе лопату.
public class ShovelItem : MonoBehaviour
{  // Начало класса `ShovelItem`.
    // Обновляет компонент, когда Unity включает объект.
    void OnEnable()
    {  // Начало метода `OnEnable`.
        // Вызывает `PropKit.EnsureDoubleSided` с аргументами `gameObject`.
        PropKit.EnsureDoubleSided(gameObject);
    }  // Конец метода `OnEnable`.

    // Передаёт лопату игроку и убирает её объект со сцены.
    public void Take(FirstPersonPlayer player)
    {  // Начало метода `Take`.
        // Вызывает `player.GiveShovel`.
        player.GiveShovel();
        // Планирует удаление объекта через `Destroy` с аргументами `gameObject`.
        Destroy(gameObject);
    }  // Конец метода `Take`.
}  // Конец класса `ShovelItem`.

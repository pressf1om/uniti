// Подключает доступ к базовым типам игрового движка.
using UnityEngine;

// Обрабатывает подбор добычи, отдельное получение клада и запись прогресса.
public class ResourcePickup : MonoBehaviour
{  // Начало класса `ResourcePickup`.
    // Хранит поле типа `ResourceKind` в `Kind`.
    public ResourceKind Kind;
    // Задаёт числовой ключ ячейки `CellKey` значением `-1`.
    public int CellKey = -1;
    // Хранит ссылку на раскоп в `Site`.
    public DigSite Site;

    // Пытается положить обычную находку в рюкзак или засчитать победу за клад.
    public bool TryTake(Inventory bag)
    {  // Начало метода `TryTake`.
        // Проверяет условие `Kind == ResourceKind.Treasure`.
        if (Kind == ResourceKind.Treasure)
        {  // Начало ветки условия.
            // При наличии `Site` вызывает `RegisterLooted(CellKey)`.
            Site?.RegisterLooted(CellKey);
            // Планирует удаление объекта через `Destroy` с аргументами `gameObject`.
            Destroy(gameObject);
            // Проверяет условие `GameLoop.Instance != null`.
            if (GameLoop.Instance != null)
                // Фиксирует победу через `GameLoop.Instance.Win`.
                GameLoop.Instance.Win();
            // Помечает прогресс к отложенному сохранению через `GameSave.MarkDirty`.
            GameSave.MarkDirty();
            // Возвращает `true`.
            return true;
        }  // Конец ветки условия.

        // Проверяет условие `bag == null || !bag.TryAdd(Kind)`.
        if (bag == null || !bag.TryAdd(Kind))
            // Возвращает `false`.
            return false;

        // При наличии `Site` вызывает `RegisterLooted(CellKey)`.
        Site?.RegisterLooted(CellKey);
        // Планирует удаление объекта через `Destroy` с аргументами `gameObject`.
        Destroy(gameObject);
        // Помечает прогресс к отложенному сохранению через `GameSave.MarkDirty`.
        GameSave.MarkDirty();
        // Возвращает `true`.
        return true;
    }  // Конец метода `TryTake`.
}  // Конец класса `ResourcePickup`.

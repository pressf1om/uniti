// Подключает доступ к базовым типам игрового движка.
using UnityEngine;

// Перечисляет типы блоков раскопа.
public enum BlockKind
{  // Начало перечисления `BlockKind`.
    // Задаёт значение перечисления «трава».
    Grass,
    // Задаёт значение перечисления «земля».
    Dirt,
    // Задаёт значение перечисления «бетон».
    Concrete
}  // Конец перечисления `BlockKind`.

// Хранит тип, координаты и возможный ресурс одного блока раскопа.
public class DigBlock : MonoBehaviour
{  // Начало класса `DigBlock`.
    // Хранит поле типа `BlockKind` в `Kind`.
    public BlockKind Kind;
    // Задаёт ресурс внутри блока `Buried` значением отсутствие ресурса.
    public ResourceKind Buried = ResourceKind.None;
    // Хранит целое число в `Ix`.
    public int Ix;
    // Хранит целое число в `Iy`.
    public int Iy;
    // Хранит целое число в `Iz`.
    public int Iz;

    // Выбрасывает скрытый ресурс, если он есть, и удаляет блок.
    public void Release(DigSite site)
    {  // Начало метода `Release`.
        // Проверяет условие `Buried != ResourceKind.None`.
        if (Buried != ResourceKind.None)
            // Вызывает `site.SpawnBuried` с аргументами `Buried, transform.position, DigSite.Pack(Ix, Iy, Iz)`.
            site.SpawnBuried(Buried, transform.position, DigSite.Pack(Ix, Iy, Iz));
        // Планирует удаление объекта через `Destroy` с аргументами `gameObject`.
        Destroy(gameObject);
    }  // Конец метода `Release`.
}  // Конец класса `DigBlock`.

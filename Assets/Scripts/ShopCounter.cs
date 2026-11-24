// Подключает доступ к базовым типам игрового движка.
using UnityEngine;

// Связывает интерактивную точку магазина с её назначением.
public class ShopCounter : MonoBehaviour
{  // Начало класса `ShopCounter`.
    // Хранит поле типа `ShopKind` в `Kind`.
    public ShopKind Kind;
}  // Конец класса `ShopCounter`.

// Перечисляет магазин продажи и магазин улучшений.
public enum ShopKind
{  // Начало перечисления `ShopKind`.
    // Задаёт значение перечисления «продажа».
    Sell,
    // Задаёт значение перечисления «улучшения».
    Upgrade
}  // Конец перечисления `ShopKind`.

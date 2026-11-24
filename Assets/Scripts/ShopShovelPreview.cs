// Подключает доступ к базовым типам игрового движка.
using UnityEngine;

/// <summary>
/// Лопаты в магазине красятся в YardBootstrap только в Play; в Edit mode делаем то же для обзора сцены.
/// </summary>
[ExecuteAlways]  // Разрешает этому компоненту работать и при редактировании сцены.
// Обновляет цвета выставленных лопат при редактировании и запуске сцены.
public class ShopShovelPreview : MonoBehaviour
{  // Начало класса `ShopShovelPreview`.
    // Обновляет компонент, когда Unity включает объект.
    void OnEnable()
    {  // Начало метода `OnEnable`.
        // Проверяет условие `name != "Магазин"`.
        if (name != "Магазин")
            // Завершает выполнение текущего метода.
            return;
        // Вызывает `ShovelTint.PaintShopRow` с аргументами `transform`.
        ShovelTint.PaintShopRow(transform);
    }  // Конец метода `OnEnable`.

    // Выполняет начальную настройку компонента перед первым кадром.
    void Start()
    {  // Начало метода `Start`.
        // Проверяет условие `!Application.isPlaying || name != "Магазин"`.
        if (!Application.isPlaying || name != "Магазин")
            // Завершает выполнение текущего метода.
            return;
        // Вызывает `ShovelTint.PaintShopRow` с аргументами `transform`.
        ShovelTint.PaintShopRow(transform);
    }  // Конец метода `Start`.
}  // Конец класса `ShopShovelPreview`.
// Подключает доступ к базовым типам игрового движка.
using UnityEngine;

// Ставит продавца у прилавка и делает его доступным для взаимодействия.
public class Seller : MonoBehaviour
{  // Начало класса `Seller`.
    // Задаёт `ShopCollider` значением `"Продавец-коллайдер"`.
    const string ShopCollider = "Продавец-коллайдер";

    // Хранит поле типа `Animator` в `animator`.
    Animator animator;
    // Хранит ссылку на положение, поворот и масштаб объекта в `Nameplate`.
    public Transform Nameplate;

    // Размещает продавца, подключает внешний вид и точку взаимодействия.
    public void Begin(Vector3 stand, Vector3 ignored, Transform yard = null)
    {  // Начало метода `Begin`.
        // Выбирает значение для `shop` по условию `yard` существует.
        var shop = yard != null ? yard.Find("Магазин") : null;
        // Проверяет условие `shop != null`.
        if (shop != null)
        {  // Начало ветки условия.
            // Вызывает `SellerPlacement.EnsureAnchor` с аргументами `shop`.
            SellerPlacement.EnsureAnchor(shop);
            // Вызывает `SellerPlacement.SnapSeller` с аргументами `transform, shop`.
            SellerPlacement.SnapSeller(transform, shop);
        }  // Конец ветки условия.
        // Обрабатывает случай, когда предыдущее условие ложно.
        else
        {  // Начало запасной ветки условия.
            // Привязывает к родителю через `transform.SetParent` с аргументами `yard != null ? yard : transform.parent, true`.
            transform.SetParent(yard != null ? yard : transform.parent, true);
            // Задаёт мировая позиция `transform.position` значением `stand`.
            transform.position = stand;
            // Создаёт поворот из углов Эйлера через `Quaternion.Euler` с аргументами `0f, 160f, 0f` и записывает результат в `transform.rotation`.
            transform.rotation = Quaternion.Euler(0f, 160f, 0f);
        }  // Конец запасной ветки условия.

        // Вызывает `GetComponent<UnityEngine.AI.NavMeshAgent>` и записывает результат в `agent`.
        var agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        // Проверяет условие `agent != null`.
        if (agent != null)
        {  // Начало ветки условия.
            // Проверяет условие `Application.isPlaying`.
            if (Application.isPlaying)
                // Планирует удаление объекта через `Destroy` с аргументами `agent`.
                Destroy(agent);
            // Обрабатывает случай, когда предыдущее условие ложно.
            else
                // Сразу удаляет объект через `DestroyImmediate` с аргументами `agent`.
                DestroyImmediate(agent);
        }  // Конец ветки условия.

        // Вызывает `SellerVisual.Build` с аргументами `transform`.
        SellerVisual.Build(transform);
        // Добавляет или настраивает область взаимодействия с продавцом через `EnsureShopHit`.
        EnsureShopHit();

        // Загружает ресурс через `Resources.Load<RuntimeAnimatorController>` с аргументами `"SellerIdle"` и записывает результат в `controller`.
        var controller = Resources.Load<RuntimeAnimatorController>("SellerIdle");
        // Проверяет условие `controller != null`.
        if (controller != null)
        {  // Начало ветки условия.
            // Ищет компонент через `GetComponent<Animator>` и записывает результат в `animator`.
            animator = GetComponent<Animator>();
            // Проверяет условие `animator == null`.
            if (animator == null)
                // Добавляет компонент через `gameObject.AddComponent<Animator>` и записывает результат в `animator`.
                animator = gameObject.AddComponent<Animator>();
            // Задаёт `animator.runtimeAnimatorController` значением `controller`.
            animator.runtimeAnimatorController = controller;
            // Устанавливает `animator.applyRootMotion` в `false`.
            animator.applyRootMotion = false;
        }  // Конец ветки условия.
    }  // Конец метода `Begin`.

    // Добавляет или настраивает область взаимодействия с продавцом.
    void EnsureShopHit()
    {  // Начало метода `EnsureShopHit`.
        // Ищет компонент через `GetComponent<SellerShop>` и записывает результат в `shop`.
        var shop = GetComponent<SellerShop>();
        // Проверяет условие `shop == null`.
        if (shop == null)
            // Добавляет компонент через `gameObject.AddComponent<SellerShop>` и записывает результат в `shop`.
            shop = gameObject.AddComponent<SellerShop>();
        // Задаёт `shop.Kind` значением продажа.
        shop.Kind = ShopKind.Sell;

        // Ищет объект по имени через `transform.Find` с аргументами `ShopCollider` и записывает результат в `hit`.
        var hit = transform.Find(ShopCollider);
        // Проверяет условие `hit == null`.
        if (hit == null)
        {  // Начало ветки условия.
            // Создаёт объект сцены с параметрами `ShopCollider` и сохраняет ссылку в `go`.
            var go = new GameObject(ShopCollider);
            // Задаёт `hit` значением `go.transform`.
            hit = go.transform;
            // Привязывает к родителю через `hit.SetParent` с аргументами `transform, false`.
            hit.SetParent(transform, false);
        }  // Конец ветки условия.
        // Записывает в `hit.localPosition` трёхмерный вектор с параметрами `0f, 0.95f, 0.08f`.
        hit.localPosition = new Vector3(0f, 0.95f, 0.08f);
        // Ищет компонент через `hit.GetComponent<BoxCollider>` и записывает результат в `box`.
        var box = hit.GetComponent<BoxCollider>();
        // Проверяет условие `box == null`.
        if (box == null)
            // Добавляет компонент через `hit.gameObject.AddComponent<BoxCollider>` и записывает результат в `box`.
            box = hit.gameObject.AddComponent<BoxCollider>();
        // Устанавливает режим триггера `box.isTrigger` в `false`.
        box.isTrigger = false;
        // Записывает в `box.size` трёхмерный вектор с параметрами `0.78f, 1.72f, 0.52f`.
        box.size = new Vector3(0.78f, 1.72f, 0.52f);
        // Задаёт центр `box.center` значением нулевой вектор `Vector3.zero`.
        box.center = Vector3.zero;
    }  // Конец метода `EnsureShopHit`.

    // Поворачивает табличку продавца в сторону камеры после обновления кадра.
    void LateUpdate()
    {  // Начало метода `LateUpdate`.
        // Проверяет условие `Nameplate == null || Camera.main == null`.
        if (Nameplate == null || Camera.main == null)
            // Завершает выполнение текущего метода.
            return;
        // Вызывает `Quaternion.LookRotation` с аргументами `Camera.main.transform.position - Nameplate.position` и записывает результат в `Nameplate.rotation`.
        Nameplate.rotation = Quaternion.LookRotation(Camera.main.transform.position - Nameplate.position);
    }  // Конец метода `LateUpdate`.
}  // Конец класса `Seller`.
// Подключает доступ к базовым типам игрового движка.
using UnityEngine;

// Наполняет гараж мебелью, инструментами и декоративными моделями.
public static class GarageSetdress
{  // Начало класса `GarageSetdress`.
    // Собирает прилавок, витрину и точку продавца.
    public static Transform Shop(Transform yard, Vector3 place)
    {  // Начало метода `Shop`.
        // Задаёт `root` значением `new GameObject("Магазин").transform`.
        var root = new GameObject("Магазин").transform;
        // Привязывает к родителю через `root.SetParent` с аргументами `yard, false`.
        root.SetParent(yard, false);
        // Задаёт локальная позиция `root.localPosition` значением `place`.
        root.localPosition = place;
        // Добавляет компонент через `root.gameObject.AddComponent<ShopShovelPreview>`.
        root.gameObject.AddComponent<ShopShovelPreview>();

        // Создаёт экземпляр модели и размещает его в иерархии через `AssetKit.Spawn` с аргументами `"Furniture/kitchenBar", root, Vector3.zero, new Vector3(0f, 180f, 0f), 1.05f` и записывает результат в `counter`.
        var counter = AssetKit.Spawn("Furniture/kitchenBar", root, Vector3.zero, new Vector3(0f, 180f, 0f), 1.05f);
        // Проверяет условие `counter != null`.
        if (counter != null)
            // Вызывает `KenneyKit.FitFootprint` с аргументами `counter, 1.85f, 0.62f, 1.08f`.
            KenneyKit.FitFootprint(counter, 1.85f, 0.62f, 1.08f);
        // Проверяет условие `counter == null`.
        if (counter == null)
        {  // Начало ветки условия.
            // Вызывает `TextureFactory.Lit` с аргументами `Palette.Handle, TextureFactory.Wood()` и записывает результат в `wood`.
            var wood = TextureFactory.Lit(Palette.Handle, TextureFactory.Wood());
            // Создаёт куб с заданными родителем, положением, размерами и материалом через `Box` с аргументами `root, "Прилавок", new Vector3(0f, 0.52f, 0f), new Vector3(2.1f, 1.04f, 0.72f), wood`.
            Box(root, "Прилавок", new Vector3(0f, 0.52f, 0f), new Vector3(2.1f, 1.04f, 0.72f), wood);
            // Создаёт куб с заданными родителем, положением, размерами и материалом через `Box` с аргументами `root, "Столешница", new Vector3(0f, 1.08f, 0.06f), new Vector3(2.2f, 0.08f, 0.86f), wood`.
            Box(root, "Столешница", new Vector3(0f, 1.08f, 0.06f), new Vector3(2.2f, 0.08f, 0.86f), wood);
        }  // Конец ветки условия.

        // Выбирает значение для `counterBounds` по условию `counter` существует.
        Bounds counterBounds = counter != null ? KenneyKit.BoundsOf(counter) : new Bounds(root.TransformPoint(new Vector3(0f, 0.7f, 0f)), new Vector3(2f, 1.1f, 0.8f));

        // Создаёт экземпляр модели и размещает его в иерархии через `AssetKit.Spawn` с аргументами `"Shop/cash-register", root, Vector3.zero, new Vector3(0f, 180f, 0f), 0.26f, false` и записывает результат в `register`.
        var register = AssetKit.Spawn("Shop/cash-register", root, Vector3.zero, new Vector3(0f, 180f, 0f), 0.26f, false);
        // Вызывает `AssetKit.RestOn` с аргументами `register, counterBounds, new Vector3(0.55f, 0f, -0.02f)`.
        AssetKit.RestOn(register, counterBounds, new Vector3(0.55f, 0f, -0.02f));

        // Создаёт экземпляр модели и размещает его в иерархии через `AssetKit.Spawn` с аргументами `"Shop/clothing-table", root, new Vector3(-1.72f, 0f, 1.02f), new Vector3(0f, 90f, 0f), 0.68f` и записывает результат в `table`.
        var table = AssetKit.Spawn("Shop/clothing-table", root, new Vector3(-1.72f, 0f, 1.02f), new Vector3(0f, 90f, 0f), 0.68f);
        // Создаёт экземпляр модели и размещает его в иерархии через `AssetKit.Spawn` с аргументами `"Shop/display-small", root, new Vector3(0.95f, 0f, 0.08f), new Vector3(0f, 200f, 0f), 0.88f`.
        AssetKit.Spawn("Shop/display-small", root, new Vector3(0.95f, 0f, 0.08f), new Vector3(0f, 200f, 0f), 0.88f);
        // Создаёт экземпляр модели и размещает его в иерархии через `AssetKit.Spawn` с аргументами `"Shop/shelf-boxes", root, new Vector3(0.05f, 0f, 0.62f), new Vector3(0f, 180f, 0f), 1.08f`.
        AssetKit.Spawn("Shop/shelf-boxes", root, new Vector3(0.05f, 0f, 0.62f), new Vector3(0f, 180f, 0f), 1.08f);
        // Создаёт экземпляр модели и размещает его в иерархии через `AssetKit.Spawn` с аргументами `"Shop/shelf-end", root, new Vector3(-1.2f, 0f, 0.6f), new Vector3(0f, 180f, 0f), 0.95f`.
        AssetKit.Spawn("Shop/shelf-end", root, new Vector3(-1.2f, 0f, 0.6f), new Vector3(0f, 180f, 0f), 0.95f);
        // Создаёт экземпляр модели и размещает его в иерархии через `AssetKit.Spawn` с аргументами `"Shop/shopping-basket", root, new Vector3(0.95f, 0f, -0.55f), new Vector3(0f, 40f, 0f), 0.42f`.
        AssetKit.Spawn("Shop/shopping-basket", root, new Vector3(0.95f, 0f, -0.55f), new Vector3(0f, 40f, 0f), 0.42f);
        // Создаёт экземпляр модели и размещает его в иерархии через `AssetKit.Spawn` с аргументами `"Furniture/stoolBar", root, new Vector3(0.62f, 0f, -1.12f), new Vector3(0f, 15f, 0f), 0.68f`.
        AssetKit.Spawn("Furniture/stoolBar", root, new Vector3(0.62f, 0f, -1.12f), new Vector3(0f, 15f, 0f), 0.68f);

        // Выставляет образец лопаты нужного уровня через `StandShovel` с аргументами `root, new Vector3(-0.55f, 0f, 0.08f), 12f, 1.05f, 1`.
        StandShovel(root, new Vector3(-0.55f, 0f, 0.08f), 12f, 1.05f, 1);
        // Выставляет образец лопаты нужного уровня через `StandShovel` с аргументами `root, new Vector3(-0.18f, 0f, 0.16f), -8f, 1.12f, 2`.
        StandShovel(root, new Vector3(-0.18f, 0f, 0.16f), -8f, 1.12f, 2);
        // Выставляет образец лопаты нужного уровня через `StandShovel` с аргументами `root, new Vector3(0.22f, 0f, 0.1f), 18f, 1.08f, 3`.
        StandShovel(root, new Vector3(0.22f, 0f, 0.1f), 18f, 1.08f, 3);

        // Вызывает `SellerPlacement.EnsureAnchor` с аргументами `root`.
        SellerPlacement.EnsureAnchor(root);
        // Возвращает `root`.
        return root;
    }  // Конец метода `Shop`.

    // Создаёт кузницу с пнём, наковальней и молотком.
    public static Transform Anvil(Transform yard, Vector3 place)
    {  // Начало метода `Anvil`.
        // Задаёт `root` значением `new GameObject("Улучшения").transform`.
        var root = new GameObject("Улучшения").transform;
        // Привязывает к родителю через `root.SetParent` с аргументами `yard, false`.
        root.SetParent(yard, false);
        // Задаёт локальная позиция `root.localPosition` значением `place`.
        root.localPosition = place;
        // Вызывает `TextureFactory.Lit` с аргументами `Palette.Handle, TextureFactory.Wood()` и записывает результат в `wood`.
        var wood = TextureFactory.Lit(Palette.Handle, TextureFactory.Wood());
        // Загружает ресурс через `Resources.Load<GameObject>` с аргументами `"Nature/stump_squareDetailed"` и записывает результат в `stump`.
        var stump = Resources.Load<GameObject>("Nature/stump_squareDetailed");
        // Инициализирует `stand` значением `null`.
        Transform stand = null;
        // Проверяет условие `stump != null`.
        if (stump != null)
        {  // Начало ветки условия.
            // Создаёт копию объекта через `Object.Instantiate` с аргументами `stump, root` и записывает результат в `go`.
            var go = Object.Instantiate(stump, root);
            // Задаёт имя `go.name` значением `"Пень"`.
            go.name = "Пень";
            // Задаёт локальная позиция `go.transform.localPosition` значением нулевой вектор `Vector3.zero`.
            go.transform.localPosition = Vector3.zero;
            // Собирает компоненты в объекте и его потомках через `go.GetComponentsInChildren<Renderer>` и записывает результат в `renderers`.
            var renderers = go.GetComponentsInChildren<Renderer>();
            // Перебирает индексы начиная с `int i = 0` при условии `i < renderers.Length`.
            for (int i = 0; i < renderers.Length; i++)
                // Задаёт общий материал `renderers[i].sharedMaterial` значением `wood`.
                renderers[i].sharedMaterial = wood;
            // Собирает компоненты в объекте и его потомках через `go.GetComponentsInChildren<Collider>` и записывает результат в `colliders`.
            var colliders = go.GetComponentsInChildren<Collider>();
            // Перебирает индексы начиная с `int i = 0` при условии `i < colliders.Length`.
            for (int i = 0; i < colliders.Length; i++)
                // Планирует удаление объекта через `Object.Destroy` с аргументами `colliders[i]`.
                Object.Destroy(colliders[i]);
            // Вызывает `KenneyKit.FitHeight` с аргументами `go.transform, 0.72f`.
            KenneyKit.FitHeight(go.transform, 0.72f);
            // Задаёт `stand` значением `go.transform`.
            stand = go.transform;
        }  // Конец ветки условия.
        // Обрабатывает случай, когда предыдущее условие ложно.
        else
            // Присваивает `stand` результат `Box(root, "Пень", new Vector3(0f, 0.32f, 0f), new Vector3(0.92f, 0.64f, 0.92f), wood)`.
            stand = Box(root, "Пень", new Vector3(0f, 0.32f, 0f), new Vector3(0.92f, 0.64f, 0.92f), wood);

        // Вызывает `KenneyKit.BoundsOf` с аргументами `stand` и записывает результат в `standBounds`.
        Bounds standBounds = KenneyKit.BoundsOf(stand);
        // Создаёт экземпляр модели и размещает его в иерархии через `PropKit.Spawn` с аргументами `"anvil", root, new Vector3(0f, standBounds.max.y - root.position.y, 0f), new Vector3(-90f, 0f, 0f), 0.7f, false, false` и записывает результат в `model`.
        var model = PropKit.Spawn("anvil", root, new Vector3(0f, standBounds.max.y - root.position.y, 0f), new Vector3(-90f, 0f, 0f), 0.7f, false, false);
        // Проверяет условие `model != null`.
        if (model != null)
            // Вызывает `AssetKit.RestOn` с аргументами `model, standBounds, Vector3.zero`.
            AssetKit.RestOn(model, standBounds, Vector3.zero);
        // Обрабатывает случай, когда предыдущее условие ложно.
        else
        {  // Начало запасной ветки условия.
            // Вызывает `TextureFactory.Lit` с аргументами `Palette.Iron, TextureFactory.Speckled(), 0.38f` и записывает результат в `iron`.
            var iron = TextureFactory.Lit(Palette.Iron, TextureFactory.Speckled(), 0.38f);
            // Создаёт куб с заданными родителем, положением, размерами и материалом через `Box` с аргументами `root, "Тело", new Vector3(0f, 0.7f, 0f), new Vector3(0.9f, 0.32f, 0.42f), iron`.
            Box(root, "Тело", new Vector3(0f, 0.7f, 0f), new Vector3(0.9f, 0.32f, 0.42f), iron);
        }  // Конец запасной ветки условия.

        // Выбирает значение для `anvilBounds` по условию `model` существует.
        Bounds anvilBounds = model != null ? KenneyKit.BoundsOf(model) : new Bounds(root.position + Vector3.up * 0.85f, new Vector3(0.9f, 0.4f, 0.5f));
        // Создаёт экземпляр модели и размещает его в иерархии через `AssetKit.Spawn` с аргументами `"Survival/tool-hammer", root, Vector3.zero, new Vector3(0f, 28f, 90f), 0.28f, false` и записывает результат в `hammer`.
        var hammer = AssetKit.Spawn("Survival/tool-hammer", root, Vector3.zero, new Vector3(0f, 28f, 90f), 0.28f, false);
        // Проверяет условие `hammer != null`.
        if (hammer != null)
            // Вызывает `AssetKit.RestOn` с аргументами `hammer, anvilBounds, new Vector3(0.06f, -0.22f, 0.02f)`.
            AssetKit.RestOn(hammer, anvilBounds, new Vector3(0.06f, -0.22f, 0.02f));
        // Обрабатывает случай, когда предыдущее условие ложно.
        else
        {  // Начало запасной ветки условия.
            // Вызывает `TextureFactory.Lit` с аргументами `Palette.Handle, TextureFactory.Wood()` и записывает результат в `hammerWood`.
            var hammerWood = TextureFactory.Lit(Palette.Handle, TextureFactory.Wood());
            // Вызывает `TextureFactory.Lit` с аргументами `Palette.Iron, TextureFactory.Speckled(), 0.38f` и записывает результат в `head`.
            var head = TextureFactory.Lit(Palette.Iron, TextureFactory.Speckled(), 0.38f);
            // Вычисляет `rest` как `anvilBounds.max.y` плюс `0.015` минус `root.position.y`.
            float rest = anvilBounds.max.y + 0.015f - root.position.y;
            // Создаёт куб с заданными родителем, положением, размерами и материалом через `Box` с аргументами `root, "Ручка", new Vector3(0.05f, rest, 0f), new Vector3(0.52f, 0.045f, 0.045f), hammerWood`.
            Box(root, "Ручка", new Vector3(0.05f, rest, 0f), new Vector3(0.52f, 0.045f, 0.045f), hammerWood);
            // Создаёт куб с заданными родителем, положением, размерами и материалом через `Box` с аргументами `root, "Боёк", new Vector3(0.26f, rest + 0.02f, 0f), new Vector3(0.16f, 0.09f, 0.13f), head`.
            Box(root, "Боёк", new Vector3(0.26f, rest + 0.02f, 0f), new Vector3(0.16f, 0.09f, 0.13f), head);
        }  // Конец запасной ветки условия.

        // Возвращает `root`.
        return root;
    }  // Конец метода `Anvil`.

    // Расставляет мебель, инструменты и светильник в гараже.
    public static void Fill(Transform yard)
    {  // Начало метода `Fill`.
        // Задаёт `x` значением `-5.1f`.
        const float x = -5.1f;
        // Создаёт экземпляр модели и размещает его в иерархии через `AssetKit.Spawn` с аргументами `"Furniture/kitchenCabinet", yard, new Vector3(x - 3.2f, 0f, 4.05f), new Vector3(0f, 90f, 0f), 1.55f`.
        AssetKit.Spawn("Furniture/kitchenCabinet", yard, new Vector3(x - 3.2f, 0f, 4.05f), new Vector3(0f, 90f, 0f), 1.55f);
        // Создаёт экземпляр модели и размещает его в иерархии через `AssetKit.Spawn` с аргументами `"Furniture/kitchenCabinetUpper", yard, new Vector3(x - 3.22f, 1.62f, 4.05f), new Vector3(0f, 90f, 0f), 0.55f`.
        AssetKit.Spawn("Furniture/kitchenCabinetUpper", yard, new Vector3(x - 3.22f, 1.62f, 4.05f), new Vector3(0f, 90f, 0f), 0.55f);
        // Создаёт экземпляр модели и размещает его в иерархии через `AssetKit.Spawn` с аргументами `"Furniture/kitchenCabinetDrawer", yard, new Vector3(x - 3.18f, 0f, 0.85f), new Vector3(0f, 90f, 0f), 0.92f`.
        AssetKit.Spawn("Furniture/kitchenCabinetDrawer", yard, new Vector3(x - 3.18f, 0f, 0.85f), new Vector3(0f, 90f, 0f), 0.92f);
        // Создаёт экземпляр модели и размещает его в иерархии через `AssetKit.Spawn` с аргументами `"Furniture/bookcaseClosed", yard, new Vector3(x - 3.15f, 0f, 3.15f), new Vector3(0f, 90f, 0f), 1.85f`.
        AssetKit.Spawn("Furniture/bookcaseClosed", yard, new Vector3(x - 3.15f, 0f, 3.15f), new Vector3(0f, 90f, 0f), 1.85f);
        // Создаёт экземпляр модели и размещает его в иерархии через `AssetKit.Spawn` с аргументами `"Survival/workbench", yard, new Vector3(x - 1.05f, 0f, 0.58f), new Vector3(0f, 180f, 0f), 0.92f`.
        AssetKit.Spawn("Survival/workbench", yard, new Vector3(x - 1.05f, 0f, 0.58f), new Vector3(0f, 180f, 0f), 0.92f);
        // Создаёт экземпляр модели и размещает его в иерархии через `AssetKit.Spawn` с аргументами `"Furniture/table", yard, new Vector3(x + 1.05f, 0f, 0.7f), new Vector3(0f, 8f, 0f), 0.78f` и записывает результат в `tableGo`.
        var tableGo = AssetKit.Spawn("Furniture/table", yard, new Vector3(x + 1.05f, 0f, 0.7f), new Vector3(0f, 8f, 0f), 0.78f);
        // Проверяет условие `tableGo != null`.
        if (tableGo != null)
            // Вызывает `KenneyKit.FitFootprint` с аргументами `tableGo, 1.65f, 0.72f, 0.8f`.
            KenneyKit.FitFootprint(tableGo, 1.65f, 0.72f, 0.8f);
        // Создаёт экземпляр модели и размещает его в иерархии через `AssetKit.Spawn` с аргументами `"Furniture/chairDesk", yard, new Vector3(x + 1.15f, 0f, 1.15f), new Vector3(0f, 180f, 0f), 0.82f`.
        AssetKit.Spawn("Furniture/chairDesk", yard, new Vector3(x + 1.15f, 0f, 1.15f), new Vector3(0f, 180f, 0f), 0.82f);
        // Создаёт экземпляр модели и размещает его в иерархии через `AssetKit.Spawn` с аргументами `"Furniture/desk", yard, new Vector3(x - 0.15f, 0f, 4.12f), new Vector3(0f, 0f, 0f), 0.78f`.
        AssetKit.Spawn("Furniture/desk", yard, new Vector3(x - 0.15f, 0f, 4.12f), new Vector3(0f, 0f, 0f), 0.78f);
        // Создаёт экземпляр модели и размещает его в иерархии через `AssetKit.Spawn` с аргументами `"Furniture/sideTableDrawers", yard, new Vector3(x + 2.55f, 0f, 4.12f), new Vector3(0f, 0f, 0f), 0.7f`.
        AssetKit.Spawn("Furniture/sideTableDrawers", yard, new Vector3(x + 2.55f, 0f, 4.12f), new Vector3(0f, 0f, 0f), 0.7f);
        // Создаёт экземпляр модели и размещает его в иерархии через `AssetKit.Spawn` с аргументами `"Furniture/coatRackStanding", yard, new Vector3(x + 2.95f, 0f, 0.55f), Vector3.zero, 1.55f`.
        AssetKit.Spawn("Furniture/coatRackStanding", yard, new Vector3(x + 2.95f, 0f, 0.55f), Vector3.zero, 1.55f);
        // Создаёт экземпляр модели и размещает его в иерархии через `AssetKit.Spawn` с аргументами `"Furniture/trashcan", yard, new Vector3(x + 2.55f, 0f, 0.55f), new Vector3(0f, 20f, 0f), 0.48f`.
        AssetKit.Spawn("Furniture/trashcan", yard, new Vector3(x + 2.55f, 0f, 0.55f), new Vector3(0f, 20f, 0f), 0.48f);
        // Создаёт экземпляр модели и размещает его в иерархии через `AssetKit.Spawn` с аргументами `"Survival/barrel", yard, new Vector3(x - 2.95f, 0f, 1.05f), new Vector3(0f, 15f, 0f), 0.78f`.
        AssetKit.Spawn("Survival/barrel", yard, new Vector3(x - 2.95f, 0f, 1.05f), new Vector3(0f, 15f, 0f), 0.78f);
        // Создаёт экземпляр модели и размещает его в иерархии через `AssetKit.Spawn` с аргументами `"Survival/barrel", yard, new Vector3(x - 2.55f, 0f, 1.28f), new Vector3(0f, 40f, 0f), 0.72f`.
        AssetKit.Spawn("Survival/barrel", yard, new Vector3(x - 2.55f, 0f, 1.28f), new Vector3(0f, 40f, 0f), 0.72f);
        // Создаёт экземпляр модели и размещает его в иерархии через `AssetKit.Spawn` с аргументами `"Survival/chest", yard, new Vector3(x - 2.85f, 0f, 3.55f), new Vector3(0f, 90f, 0f), 0.55f`.
        AssetKit.Spawn("Survival/chest", yard, new Vector3(x - 2.85f, 0f, 3.55f), new Vector3(0f, 90f, 0f), 0.55f);
        // Создаёт экземпляр модели и размещает его в иерархии через `AssetKit.Spawn` с аргументами `"Survival/box", yard, new Vector3(x - 2.55f, 0f, 3.95f), new Vector3(0f, 25f, 0f), 0.38f`.
        AssetKit.Spawn("Survival/box", yard, new Vector3(x - 2.55f, 0f, 3.95f), new Vector3(0f, 25f, 0f), 0.38f);
        // Создаёт экземпляр модели и размещает его в иерархии через `AssetKit.Spawn` с аргументами `"Survival/box-large", yard, new Vector3(x - 2.95f, 0f, 0.55f), new Vector3(0f, 10f, 0f), 0.48f`.
        AssetKit.Spawn("Survival/box-large", yard, new Vector3(x - 2.95f, 0f, 0.55f), new Vector3(0f, 10f, 0f), 0.48f);
        // Создаёт экземпляр модели и размещает его в иерархии через `AssetKit.Spawn` с аргументами `"Survival/bucket", yard, new Vector3(x - 2.15f, 0f, 0.48f), Vector3.zero, 0.32f`.
        AssetKit.Spawn("Survival/bucket", yard, new Vector3(x - 2.15f, 0f, 0.48f), Vector3.zero, 0.32f);
        // Создаёт экземпляр модели и размещает его в иерархии через `AssetKit.Spawn` с аргументами `"Furniture/cardboardBoxClosed", yard, new Vector3(x + 2.85f, 0f, 3.55f), new Vector3(0f, 18f, 0f), 0.38f`.
        AssetKit.Spawn("Furniture/cardboardBoxClosed", yard, new Vector3(x + 2.85f, 0f, 3.55f), new Vector3(0f, 18f, 0f), 0.38f);
        // Создаёт экземпляр модели и размещает его в иерархии через `AssetKit.Spawn` с аргументами `"Furniture/cardboardBoxOpen", yard, new Vector3(x + 2.45f, 0f, 3.35f), new Vector3(0f, -12f, 0f), 0.34f`.
        AssetKit.Spawn("Furniture/cardboardBoxOpen", yard, new Vector3(x + 2.45f, 0f, 3.35f), new Vector3(0f, -12f, 0f), 0.34f);
        // Создаёт экземпляр модели и размещает его в иерархии через `AssetKit.Spawn` с аргументами `"Furniture/lampSquareCeiling", yard, new Vector3(x, 2.72f, 2.35f), Vector3.zero, 0.22f, false` и записывает результат в `lamp`.
        var lamp = AssetKit.Spawn("Furniture/lampSquareCeiling", yard, new Vector3(x, 2.72f, 2.35f), Vector3.zero, 0.22f, false);
        // Проверяет условие `lamp != null`.
        if (lamp != null)
        {  // Начало ветки условия.
            // Вызывает `KenneyKit.BoundsOf` с аргументами `lamp` и записывает результат в `bounds`.
            var bounds = KenneyKit.BoundsOf(lamp);
            // Добавляет `Vector3.up * (2.95f - bounds.max.y)` к текущему значению мировая позиция `lamp.position`.
            lamp.position += Vector3.up * (2.95f - bounds.max.y);
        }  // Конец ветки условия.
    }  // Конец метода `Fill`.

    // Выставляет образец лопаты нужного уровня.
    static void StandShovel(Transform parent, Vector3 place, float yaw, float height, int tier)
    {  // Начало метода `StandShovel`.
        // Создаёт экземпляр модели и размещает его в иерархии через `PropKit.Spawn` с аргументами `"shovel", parent, place, new Vector3(0f, yaw, 6f), height, true, true` и записывает результат в `model`.
        var model = PropKit.Spawn("shovel", parent, place, new Vector3(0f, yaw, 6f), height, true, true);
        // Проверяет условие `model == null`.
        if (model == null)
            // Присваивает `model` результат `AssetKit.Spawn("Survival/tool-shovel", parent, place, new Vector3(0f, yaw, 8f), height)`.
            model = AssetKit.Spawn("Survival/tool-shovel", parent, place, new Vector3(0f, yaw, 8f), height);
        // Проверяет условие `model != null`.
        if (model != null)
            // Вызывает `ShovelTint.Apply` с аргументами `model, tier`.
            ShovelTint.Apply(model, tier);
    }  // Конец метода `StandShovel`.

    // Создаёт триггер и связывает его с типом магазина.
    static void Hit(Transform root, ShopKind kind, Vector3 center, Vector3 size)
    {  // Начало метода `Hit`.
        // Добавляет компонент через `root.gameObject.AddComponent<BoxCollider>` и записывает результат в `hit`.
        var hit = root.gameObject.AddComponent<BoxCollider>();
        // Задаёт центр `hit.center` значением `center`.
        hit.center = center;
        // Задаёт размер `hit.size` значением `size`.
        hit.size = size;
        // Устанавливает режим триггера `hit.isTrigger` в `true`.
        hit.isTrigger = true;
        // Задаёт `root.gameObject.AddComponent<ShopCounter>().Kind` значением `kind`.
        root.gameObject.AddComponent<ShopCounter>().Kind = kind;
    }  // Конец метода `Hit`.

    // Создаёт куб с заданными родителем, положением, размерами и материалом.
    static Transform Box(Transform parent, string name, Vector3 position, Vector3 scale, Material material)
    {  // Начало метода `Box`.
        // Создаёт стандартный примитив через `GameObject.CreatePrimitive` с аргументами `PrimitiveType.Cube` и записывает результат в `go`.
        var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
        // Задаёт имя `go.name` значением `name`.
        go.name = name;
        // Привязывает к родителю через `go.transform.SetParent` с аргументами `parent, false`.
        go.transform.SetParent(parent, false);
        // Задаёт локальная позиция `go.transform.localPosition` значением `position`.
        go.transform.localPosition = position;
        // Задаёт локальный масштаб `go.transform.localScale` значением `scale`.
        go.transform.localScale = scale;
        // Задаёт общий материал `go.GetComponent<MeshRenderer>().sharedMaterial` значением `material`.
        go.GetComponent<MeshRenderer>().sharedMaterial = material;
        // Планирует удаление объекта через `Object.Destroy` с аргументами `go.GetComponent<Collider>()`.
        Object.Destroy(go.GetComponent<Collider>());
        // Возвращает `go.transform`.
        return go.transform;
    }  // Конец метода `Box`.
}  // Конец класса `GarageSetdress`.

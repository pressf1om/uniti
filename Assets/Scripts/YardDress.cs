// Подключает доступ к базовым типам игрового движка.
using UnityEngine;

// Создаёт дома, ограждения, дорожки и декоративные детали двора.
public static class YardDress
{  // Начало класса `YardDress`.
    // Собирает дом двора с крышей и окнами.
    public static void House(Transform yard, Material wall, Material roof)
    {  // Начало метода `House`.
        // Создаёт экземпляр модели и размещает его в иерархии через `KenneyKit.Spawn` с аргументами `"building-type-h", yard, new Vector3(-5.1f, 0f, 8.7f), new Vector3(0f, 180f, 0f), 1f` и записывает результат в `house`.
        var house = KenneyKit.Spawn("building-type-h", yard, new Vector3(-5.1f, 0f, 8.7f), new Vector3(0f, 180f, 0f), 1f);
        // Проверяет условие `house == null`.
        if (house == null)
            // Присваивает `house` результат `KenneyKit.Spawn("building-type-a", yard, new Vector3(-5.1f, 0f, 8.7f), new Vector3(0f, 180f, 0f), 1f)`.
            house = KenneyKit.Spawn("building-type-a", yard, new Vector3(-5.1f, 0f, 8.7f), new Vector3(0f, 180f, 0f), 1f);
        // Проверяет условие `house != null`.
        if (house != null)
        {  // Начало ветки условия.
            // Вызывает `KenneyKit.FitFootprint` с аргументами `house, 7.6f, 5.4f, 6.2f`.
            KenneyKit.FitFootprint(house, 7.6f, 5.4f, 6.2f);
            // Вызывает `KenneyKit.Block` с аргументами `house`.
            KenneyKit.Block(house);
            // Завершает выполнение текущего метода.
            return;
        }  // Конец ветки условия.

        // Задаёт `x` значением `-5.1f`.
        const float x = -5.1f;
        // Задаёт `z` значением `8.55`.
        const float z = 8.55f;
        // Создаёт куб с заданными родителем, положением, размерами и материалом через `Box` с аргументами `yard, "Дом", new Vector3(x, 2.15f, z), new Vector3(7.4f, 4.3f, 5.2f), wall`.
        Box(yard, "Дом", new Vector3(x, 2.15f, z), new Vector3(7.4f, 4.3f, 5.2f), wall);
        // Создаёт куб с заданными родителем, положением, размерами и материалом через `Box` с аргументами `yard, "Скат", new Vector3(x, 4.7f, z), new Vector3(7.9f, 0.18f, 3.4f), roof` и записывает результат в `slope`.
        var slope = Box(yard, "Скат", new Vector3(x, 4.7f, z), new Vector3(7.9f, 0.18f, 3.4f), roof);
        // Создаёт поворот из углов Эйлера через `Quaternion.Euler` с аргументами `32f, 0f, 0f` и записывает результат в `slope.localRotation`.
        slope.localRotation = Quaternion.Euler(32f, 0f, 0f);
        // Создаёт куб с заданными родителем, положением, размерами и материалом через `Box` с аргументами `yard, "Скат", new Vector3(x, 4.7f, z), new Vector3(7.9f, 0.18f, 3.4f), roof` и записывает результат в `slope2`.
        var slope2 = Box(yard, "Скат", new Vector3(x, 4.7f, z), new Vector3(7.9f, 0.18f, 3.4f), roof);
        // Создаёт поворот из углов Эйлера через `Quaternion.Euler` с аргументами `-32f, 0f, 0f` и записывает результат в `slope2.localRotation`.
        slope2.localRotation = Quaternion.Euler(-32f, 0f, 0f);
        // Создаёт куб с заданными родителем, положением, размерами и материалом через `Box` с аргументами `yard, "Труба", new Vector3(x + 2.2f, 5.6f, z - 0.8f), new Vector3(0.45f, 1.1f, 0.45f), TextureFactory.Lit(Palette.Brick, TextureFactory.Brick())`.
        Box(yard, "Труба", new Vector3(x + 2.2f, 5.6f, z - 0.8f), new Vector3(0.45f, 1.1f, 0.45f), TextureFactory.Lit(Palette.Brick, TextureFactory.Brick()));
        // Добавляет окно указанного размера через `Window` с аргументами `yard, new Vector3(x + 3.72f, 2.3f, z - 1.2f), new Vector3(0.08f, 1.2f, 1.1f)`.
        Window(yard, new Vector3(x + 3.72f, 2.3f, z - 1.2f), new Vector3(0.08f, 1.2f, 1.1f));
        // Добавляет окно указанного размера через `Window` с аргументами `yard, new Vector3(x + 3.72f, 2.3f, z + 1.2f), new Vector3(0.08f, 1.2f, 1.1f)`.
        Window(yard, new Vector3(x + 3.72f, 2.3f, z + 1.2f), new Vector3(0.08f, 1.2f, 1.1f));
        // Создаёт куб с заданными родителем, положением, размерами и материалом через `Box` с аргументами `yard, "Дверь", new Vector3(x + 3.72f, 1.15f, z), new Vector3(0.1f, 2.2f, 1.05f), TextureFactory.Lit(Palette.Door, TextureFactory.Wood())`.
        Box(yard, "Дверь", new Vector3(x + 3.72f, 1.15f, z), new Vector3(0.1f, 2.2f, 1.05f), TextureFactory.Lit(Palette.Door, TextureFactory.Wood()));
    }  // Конец метода `House`.

    // Ограждает раскоп с учётом прохода.
    public static void PitFence(Transform yard, Material material)
    {  // Начало метода `PitFence`.
        // Вычисляет `far` как `YardMetrics.Width` умножить на `YardMetrics.Cell`.
        float far = YardMetrics.Width * YardMetrics.Cell;
        // Вычисляет `gate0` как `YardMetrics.GateMin` умножить на `YardMetrics.Cell`.
        float gate0 = YardMetrics.GateMin * YardMetrics.Cell;
        // Вычисляет `gate1` как (`YardMetrics.GateMax` плюс `1`) умножить на `YardMetrics.Cell`.
        float gate1 = (YardMetrics.GateMax + 1) * YardMetrics.Cell;
        // Задаёт `rim` значением `-0.18f`.
        const float rim = -0.18f;
        // Распределяет планки вдоль секции забора через `Palings` с аргументами `yard, material, 0f, gate0 - 0.12f, rim, true, 1.35f`.
        Palings(yard, material, 0f, gate0 - 0.12f, rim, true, 1.35f);
        // Распределяет планки вдоль секции забора через `Palings` с аргументами `yard, material, gate1 + 0.12f, far, rim, true, 1.35f`.
        Palings(yard, material, gate1 + 0.12f, far, rim, true, 1.35f);
        // Распределяет планки вдоль секции забора через `Palings` с аргументами `yard, material, 0f, far, far - rim, true, 1.35f`.
        Palings(yard, material, 0f, far, far - rim, true, 1.35f);
        // Распределяет планки вдоль секции забора через `Palings` с аргументами `yard, material, 0f, far, rim, false, 1.35f`.
        Palings(yard, material, 0f, far, rim, false, 1.35f);
        // Распределяет планки вдоль секции забора через `Palings` с аргументами `yard, material, 0f, far, far - rim, false, 1.35f`.
        Palings(yard, material, 0f, far, far - rim, false, 1.35f);

        // Создаёт столб забора через `Post` с аргументами `yard, material, new Vector3(gate0, 1.25f, rim), 2.5f`.
        Post(yard, material, new Vector3(gate0, 1.25f, rim), 2.5f);
        // Создаёт столб забора через `Post` с аргументами `yard, material, new Vector3(gate1, 1.25f, rim), 2.5f`.
        Post(yard, material, new Vector3(gate1, 1.25f, rim), 2.5f);
        // Создаёт куб с заданными родителем, положением, размерами и материалом через `Box` с аргументами `yard, "Перекладина ворот", new Vector3((gate0 + gate1) * 0.5f, 2.52f, rim), new Vector3(gate1 - gate0 + 0.16f, 0.1f, 0.1f), material`.
        Box(yard, "Перекладина ворот", new Vector3((gate0 + gate1) * 0.5f, 2.52f, rim), new Vector3(gate1 - gate0 + 0.16f, 0.1f, 0.1f), material);
    }  // Конец метода `PitFence`.

    // Расставляет декоративные предметы двора.
    public static void Props(Transform yard)
    {  // Начало метода `Props`.
        // Выкладывает дорожку из отдельных плит через `Path` с аргументами `yard`.
        Path(yard);
        // Размещает растение во дворе через `Plant` с аргументами `yard, new Vector3(-7.2f, 0f, -3.6f)`.
        Plant(yard, new Vector3(-7.2f, 0f, -3.6f));
        // Размещает растение во дворе через `Plant` с аргументами `yard, new Vector3(11.6f, 0f, -3.4f)`.
        Plant(yard, new Vector3(11.6f, 0f, -3.4f));
        // Размещает растение во дворе через `Plant` с аргументами `yard, new Vector3(11.7f, 0f, 12.6f)`.
        Plant(yard, new Vector3(11.7f, 0f, 12.6f));
        // Размещает растение во дворе через `Plant` с аргументами `yard, new Vector3(-8.1f, 0f, 12.5f)`.
        Plant(yard, new Vector3(-8.1f, 0f, 12.5f));
        // Размещает дерево во дворе через `Tree` с аргументами `yard, new Vector3(-7.4f, 0f, 6.2f)`.
        Tree(yard, new Vector3(-7.4f, 0f, 6.2f));
        // Размещает дерево во дворе через `Tree` с аргументами `yard, new Vector3(11.3f, 0f, 8.4f)`.
        Tree(yard, new Vector3(11.3f, 0f, 8.4f));
        // Создаёт и размещает почтовый ящик через `Mailbox` с аргументами `yard, new Vector3(1.15f, 0f, -5.7f)`.
        Mailbox(yard, new Vector3(1.15f, 0f, -5.7f));
        // Строит живую изгородь вдоль отрезка через `HedgeRow` с аргументами `yard, new Vector3(-9.25f, 0f, 5.25f), new Vector3(-9.25f, 0f, 10.8f)`.
        HedgeRow(yard, new Vector3(-9.25f, 0f, 5.25f), new Vector3(-9.25f, 0f, 10.8f));
        // Строит живую изгородь вдоль отрезка через `HedgeRow` с аргументами `yard, new Vector3(11.6f, 0f, 1.2f), new Vector3(11.6f, 0f, 11.2f)`.
        HedgeRow(yard, new Vector3(11.6f, 0f, 1.2f), new Vector3(11.6f, 0f, 11.2f));
    }  // Конец метода `Props`.

    // Выкладывает дорожку из отдельных плит.
    static void Path(Transform yard)
    {  // Начало метода `Path`.
        // Вызывает `TextureFactory.Lit` с аргументами `Palette.Sidewalk, TextureFactory.Speckled(), 0.12f` и записывает результат в `stone`.
        var stone = TextureFactory.Lit(Palette.Sidewalk, TextureFactory.Speckled(), 0.12f);
        // Перебирает индексы начиная с `int i = 0` при условии `i < 5`.
        for (int i = 0; i < 5; i++)
            // Создаёт одну плиту дорожки через `Slab` с аргументами `yard, new Vector3(5f + (i % 2 == 0 ? -0.07f : 0.07f), 0.045f, -6.95f + i * 0.42f), stone`.
            Slab(yard, new Vector3(5f + (i % 2 == 0 ? -0.07f : 0.07f), 0.045f, -6.95f + i * 0.42f), stone);
        // Перебирает индексы начиная с `int i = 0` при условии `i < 8`.
        for (int i = 0; i < 8; i++)
            // Создаёт одну плиту дорожки через `Slab` с аргументами `yard, new Vector3(5f + (i % 2 == 0 ? -0.06f : 0.06f), 0.045f, -4.85f + i * 0.48f), stone`.
            Slab(yard, new Vector3(5f + (i % 2 == 0 ? -0.06f : 0.06f), 0.045f, -4.85f + i * 0.48f), stone);
        // Перебирает индексы начиная с `int i = 0` при условии `i < 10`.
        for (int i = 0; i < 10; i++)
            // Создаёт одну плиту дорожки через `Slab` с аргументами `yard, new Vector3(4.8f - i * 0.46f, 0.05f, -1.42f), stone`.
            Slab(yard, new Vector3(4.8f - i * 0.46f, 0.05f, -1.42f), stone);
        // Перебирает индексы начиная с `int i = 0` при условии `i < 2`.
        for (int i = 0; i < 2; i++)
            // Создаёт одну плиту дорожки через `Slab` с аргументами `yard, new Vector3(0.35f + (i % 2 == 0 ? -0.05f : 0.05f), 0.05f, -1.15f + i * 0.3f), stone`.
            Slab(yard, new Vector3(0.35f + (i % 2 == 0 ? -0.05f : 0.05f), 0.05f, -1.15f + i * 0.3f), stone);
    }  // Конец метода `Path`.

    // Создаёт одну плиту дорожки.
    static void Slab(Transform yard, Vector3 position, Material stone)
    {  // Начало метода `Slab`.
        // Вычисляет поворот вокруг вертикальной оси `yaw` как (`position.x` умножить на `37f` плюс `position.z` умножить на `19f`) по модулю `360`.
        float yaw = (position.x * 37f + position.z * 19f) % 360f;
        // Создаёт экземпляр модели и размещает его в иерархии через `KenneyKit.Spawn` с аргументами `"path-stones-short", yard, position, new Vector3(0f, yaw, 0f), 1f` и записывает результат в `model`.
        var model = KenneyKit.Spawn("path-stones-short", yard, position, new Vector3(0f, yaw, 0f), 1f);
        // Проверяет условие `model != null`.
        if (model != null)
        {  // Начало ветки условия.
            // Вызывает `KenneyKit.FitFootprint` с аргументами `model, 0.58f, 0.42f, 0.1f`.
            KenneyKit.FitFootprint(model, 0.58f, 0.42f, 0.1f);
            // Завершает выполнение текущего метода.
            return;
        }  // Конец ветки условия.
        // Создаёт куб с заданными родителем, положением, размерами и материалом через `Box` с аргументами `yard, "Плита", position, new Vector3(0.5f, 0.035f, 0.36f), stone`.
        Box(yard, "Плита", position, new Vector3(0.5f, 0.035f, 0.36f), stone);
    }  // Конец метода `Slab`.

    // Размещает растение во дворе.
    static void Plant(Transform yard, Vector3 place)
    {  // Начало метода `Plant`.
        // Создаёт экземпляр модели и размещает его в иерархии через `KenneyKit.Spawn` с аргументами `"planter", yard, place, Vector3.zero, 1f` и записывает результат в `planter`.
        var planter = KenneyKit.Spawn("planter", yard, place, Vector3.zero, 1f);
        // Проверяет условие `planter != null`.
        if (planter != null)
        {  // Начало ветки условия.
            // Вызывает `KenneyKit.FitHeight` с аргументами `planter, 0.95f`.
            KenneyKit.FitHeight(planter, 0.95f);
            // Вызывает `KenneyKit.BoundsOf` с аргументами `planter` и записывает результат в `bounds`.
            var bounds = KenneyKit.BoundsOf(planter);
            // Добавляет компонент через `planter.gameObject.AddComponent<BoxCollider>` и записывает результат в `box`.
            var box = planter.gameObject.AddComponent<BoxCollider>();
            // Вызывает `planter.InverseTransformPoint` с аргументами `new Vector3(bounds.center.x, bounds.min.y + 0.2f, bounds.center.z)` и записывает результат в `box.center`.
            box.center = planter.InverseTransformPoint(new Vector3(bounds.center.x, bounds.min.y + 0.2f, bounds.center.z));
            // Записывает в `box.size` трёхмерный вектор с параметрами ` 1.15f / Mathf.Max(0.001f, Mathf.Abs(planter.lossyScale.x)), 0.4f / Mathf.Max(0.001f, Mathf.Abs(planter.lossyScale.y)), 1.15f / Mathf.Max(0.001f, Mathf.Abs(pla…`.
            box.size = new Vector3(
                // Передаёт аргумент `1.15f / Mathf.Max(0.001f, Mathf.Abs(planter.lossyScale.x))`.
                1.15f / Mathf.Max(0.001f, Mathf.Abs(planter.lossyScale.x)),
                // Передаёт аргумент `0.4f / Mathf.Max(0.001f, Mathf.Abs(planter.lossyScale.y))`.
                0.4f / Mathf.Max(0.001f, Mathf.Abs(planter.lossyScale.y)),
                // Передаёт аргумент `1.15f / Mathf.Max(0.001f, Mathf.Abs(planter.lossyScale.z))`.
                1.15f / Mathf.Max(0.001f, Mathf.Abs(planter.lossyScale.z)));
            // Завершает выполнение текущего метода.
            return;
        }  // Конец ветки условия.
        // Вызывает `TextureFactory.Lit` с аргументами `Palette.Handle, TextureFactory.Wood()` и записывает результат в `wood`.
        var wood = TextureFactory.Lit(Palette.Handle, TextureFactory.Wood());
        // Вызывает `TextureFactory.Lit` с аргументами `Palette.Leaf, TextureFactory.Leaves()` и записывает результат в `leaf`.
        var leaf = TextureFactory.Lit(Palette.Leaf, TextureFactory.Leaves());
        // Создаёт куб с заданными родителем, положением, размерами и материалом через `Box` с аргументами `yard, "Клумба", place + new Vector3(0f, 0.18f, 0f), new Vector3(0.7f, 0.36f, 0.7f), wood`.
        Box(yard, "Клумба", place + new Vector3(0f, 0.18f, 0f), new Vector3(0.7f, 0.36f, 0.7f), wood);
        // Создаёт куб с заданными родителем, положением, размерами и материалом через `Box` с аргументами `yard, "Куст", place + new Vector3(0f, 0.55f, 0f), new Vector3(0.55f, 0.45f, 0.55f), leaf`.
        Box(yard, "Куст", place + new Vector3(0f, 0.55f, 0f), new Vector3(0.55f, 0.45f, 0.55f), leaf);
    }  // Конец метода `Plant`.

    // Размещает дерево во дворе.
    static void Tree(Transform yard, Vector3 place)
    {  // Начало метода `Tree`.
        // Создаёт экземпляр модели и размещает его в иерархии через `KenneyKit.Spawn` с аргументами `"tree-large", yard, place, Vector3.zero, 1f` и записывает результат в `tree`.
        var tree = KenneyKit.Spawn("tree-large", yard, place, Vector3.zero, 1f);
        // Проверяет условие `tree == null`.
        if (tree == null)
            // Создаёт экземпляр модели и размещает его в иерархии через `KenneyKit.Spawn` с аргументами `"tree-small", yard, place, Vector3.zero, 1f` и записывает результат в `tree`.
            tree = KenneyKit.Spawn("tree-small", yard, place, Vector3.zero, 1f);
        // Проверяет условие `tree != null`.
        if (tree != null)
        {  // Начало ветки условия.
            // Вызывает `KenneyKit.FitHeight` с аргументами `tree, 5.1f`.
            KenneyKit.FitHeight(tree, 5.1f);
            // Вызывает `KenneyKit.TrunkBlock` с аргументами `tree`.
            KenneyKit.TrunkBlock(tree);
            // Завершает выполнение текущего метода.
            return;
        }  // Конец ветки условия.
        // Вызывает `TextureFactory.Lit` с аргументами `Palette.Bark, TextureFactory.Wood()` и записывает результат в `bark`.
        var bark = TextureFactory.Lit(Palette.Bark, TextureFactory.Wood());
        // Вызывает `TextureFactory.Lit` с аргументами `Palette.Leaf, TextureFactory.Leaves()` и записывает результат в `leaf`.
        var leaf = TextureFactory.Lit(Palette.Leaf, TextureFactory.Leaves());
        // Создаёт стандартный примитив через `GameObject.CreatePrimitive` с аргументами `PrimitiveType.Cylinder` и записывает результат в `trunk`.
        var trunk = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        // Задаёт имя `trunk.name` значением `"Ствол"`.
        trunk.name = "Ствол";
        // Привязывает к родителю через `trunk.transform.SetParent` с аргументами `yard, false`.
        trunk.transform.SetParent(yard, false);
        // Задаёт локальная позиция `trunk.transform.localPosition` значением `place` плюс новый экземпляр `Vector3` с аргументами `0f, 1.1f, 0f`.
        trunk.transform.localPosition = place + new Vector3(0f, 1.1f, 0f);
        // Записывает в `trunk.transform.localScale` трёхмерный вектор с параметрами `0.38f, 1.1f, 0.38f`.
        trunk.transform.localScale = new Vector3(0.38f, 1.1f, 0.38f);
        // Задаёт общий материал `trunk.GetComponent<MeshRenderer>().sharedMaterial` значением `bark`.
        trunk.GetComponent<MeshRenderer>().sharedMaterial = bark;
        // Создаёт стандартный примитив через `GameObject.CreatePrimitive` с аргументами `PrimitiveType.Sphere` и записывает результат в `crown`.
        var crown = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        // Задаёт имя `crown.name` значением `"Крона"`.
        crown.name = "Крона";
        // Привязывает к родителю через `crown.transform.SetParent` с аргументами `yard, false`.
        crown.transform.SetParent(yard, false);
        // Задаёт локальная позиция `crown.transform.localPosition` значением `place` плюс новый экземпляр `Vector3` с аргументами `0f, 2.6f, 0f`.
        crown.transform.localPosition = place + new Vector3(0f, 2.6f, 0f);
        // Задаёт локальный масштаб `crown.transform.localScale` значением вектор (1, 1, 1) `Vector3.one` умножить на `2.1`.
        crown.transform.localScale = Vector3.one * 2.1f;
        // Задаёт общий материал `crown.GetComponent<MeshRenderer>().sharedMaterial` значением `leaf`.
        crown.GetComponent<MeshRenderer>().sharedMaterial = leaf;
        // Планирует удаление объекта через `Object.Destroy` с аргументами `crown.GetComponent<Collider>()`.
        Object.Destroy(crown.GetComponent<Collider>());
    }  // Конец метода `Tree`.

    // Создаёт и размещает почтовый ящик.
    static void Mailbox(Transform yard, Vector3 place)
    {  // Начало метода `Mailbox`.
        // Создаёт экземпляр модели и размещает его в иерархии через `PropKit.Spawn` с аргументами `"mailbox", yard, place, new Vector3(0f, 180f, 0f), 1.35f` и записывает результат в `model`.
        var model = PropKit.Spawn("mailbox", yard, place, new Vector3(0f, 180f, 0f), 1.35f);
        // Проверяет условие `model != null`.
        if (model != null)
        {  // Начало ветки условия.
            // Вызывает `KenneyKit.Block` с аргументами `model`.
            KenneyKit.Block(model);
            // Завершает выполнение текущего метода.
            return;
        }  // Конец ветки условия.
        // Вызывает `TextureFactory.Lit` с аргументами `Palette.Handle, TextureFactory.Wood()` и записывает результат в `wood`.
        var wood = TextureFactory.Lit(Palette.Handle, TextureFactory.Wood());
        // Вызывает `TextureFactory.Lit` с аргументами `Palette.CarRed` и записывает результат в `red`.
        var red = TextureFactory.Lit(Palette.CarRed);
        // Создаёт куб с заданными родителем, положением, размерами и материалом через `Box` с аргументами `yard, "Столб ящика", place + new Vector3(0f, 0.55f, 0f), new Vector3(0.08f, 1.1f, 0.08f), wood`.
        Box(yard, "Столб ящика", place + new Vector3(0f, 0.55f, 0f), new Vector3(0.08f, 1.1f, 0.08f), wood);
        // Создаёт куб с заданными родителем, положением, размерами и материалом через `Box` с аргументами `yard, "Ящик", place + new Vector3(0.12f, 1.05f, 0f), new Vector3(0.38f, 0.22f, 0.18f), red`.
        Box(yard, "Ящик", place + new Vector3(0.12f, 1.05f, 0f), new Vector3(0.38f, 0.22f, 0.18f), red);
        // Создаёт куб с заданными родителем, положением, размерами и материалом через `Box` с аргументами `yard, "Флажок", place + new Vector3(0.28f, 1.18f, 0.05f), new Vector3(0.04f, 0.16f, 0.02f), TextureFactory.Lit(Palette.Gold)`.
        Box(yard, "Флажок", place + new Vector3(0.28f, 1.18f, 0.05f), new Vector3(0.04f, 0.16f, 0.02f), TextureFactory.Lit(Palette.Gold));
    }  // Конец метода `Mailbox`.

    // Строит живую изгородь вдоль отрезка.
    static void HedgeRow(Transform yard, Vector3 from, Vector3 to)
    {  // Начало метода `HedgeRow`.
        // Измеряет расстояние между точками через `Vector3.Distance` с аргументами `from, to` и записывает результат в `span`.
        float span = Vector3.Distance(from, to);
        // Выбирает наибольшее значение через `Mathf.Max` с аргументами `4, Mathf.RoundToInt(span / 0.78f)` и записывает результат в `n`.
        int n = Mathf.Max(4, Mathf.RoundToInt(span / 0.78f));
        // Задаёт `names` массивом `{ "plant_bush", "plant_bushDetailed", "plant_bushLarge", "plant_bushSmall" }`.
        string[] names = { "plant_bush", "plant_bushDetailed", "plant_bushLarge", "plant_bushSmall" };
        // Вызывает `TextureFactory.Lit` с аргументами `Palette.Leaf, TextureFactory.Leaves(), 0.12f` и записывает результат в `leaf`.
        var leaf = TextureFactory.Lit(Palette.Leaf, TextureFactory.Leaves(), 0.12f);
        // Перебирает индексы начиная с `int i = 0` при условии `i < n`.
        for (int i = 0; i < n; i++)
        {  // Начало цикла по индексам.
            // Выбирает значение для `t` по условию `n` равно `1`.
            float t = n == 1 ? 0.5f : i / (float)(n - 1);
            // Интерполирует между значениями через `Vector3.Lerp` с аргументами `from, to, t` и записывает результат в `place`.
            Vector3 place = Vector3.Lerp(from, to, t);
            // Добавляет `(i % 2 == 0 ? -0.04f : 0.04f)` к текущему значению `place.x`.
            place.x += (i % 2 == 0 ? -0.04f : 0.04f);
            // Загружает ресурс через `Resources.Load<GameObject>` с аргументами `"Nature/" + names[i % names.Length]` и записывает результат в `prefab`.
            var prefab = Resources.Load<GameObject>("Nature/" + names[i % names.Length]);
            // Проверяет условие `prefab == null`.
            if (prefab == null)
            {  // Начало ветки условия.
                // Создаёт куб с заданными родителем, положением, размерами и материалом через `Box` с аргументами `yard, "Живая изгородь", place + new Vector3(0f, 0.45f, 0f), new Vector3(0.55f, 0.9f, 0.55f), leaf`.
                Box(yard, "Живая изгородь", place + new Vector3(0f, 0.45f, 0f), new Vector3(0.55f, 0.9f, 0.55f), leaf);
                // Пропускает остаток этой итерации и переходит к следующей.
                continue;
            }  // Конец ветки условия.
            // Создаёт копию объекта через `Object.Instantiate` с аргументами `prefab, yard` и записывает результат в `go`.
            var go = Object.Instantiate(prefab, yard);
            // Задаёт имя `go.name` значением `"Куст"`.
            go.name = "Куст";
            // Задаёт мировая позиция `go.transform.position` значением `place`.
            go.transform.position = place;
            // Создаёт поворот из углов Эйлера через `Quaternion.Euler` с аргументами `0f, i * 37f, 0f` и записывает результат в `go.transform.rotation`.
            go.transform.rotation = Quaternion.Euler(0f, i * 37f, 0f);
            // Собирает компоненты в объекте и его потомках через `go.GetComponentsInChildren<Renderer>` и записывает результат в `renderers`.
            var renderers = go.GetComponentsInChildren<Renderer>();
            // Перебирает индексы начиная с `int r = 0` при условии `r < renderers.Length`.
            for (int r = 0; r < renderers.Length; r++)
                // Задаёт общий материал `renderers[r].sharedMaterial` значением `leaf`.
                renderers[r].sharedMaterial = leaf;
            // Собирает компоненты в объекте и его потомках через `go.GetComponentsInChildren<Collider>` и записывает результат в `colliders`.
            var colliders = go.GetComponentsInChildren<Collider>();
            // Перебирает индексы начиная с `int c = 0` при условии `c < colliders.Length`.
            for (int c = 0; c < colliders.Length; c++)
                // Планирует удаление объекта через `Object.Destroy` с аргументами `colliders[c]`.
                Object.Destroy(colliders[c]);
            // Вызывает `KenneyKit.FitHeight` с аргументами `go.transform, 1.05f + (i % 3) * 0.08f`.
            KenneyKit.FitHeight(go.transform, 1.05f + (i % 3) * 0.08f);
            // Вызывает `KenneyKit.TrunkBlock` с аргументами `go.transform, 0.22f`.
            KenneyKit.TrunkBlock(go.transform, 0.22f);
        }  // Конец цикла по индексам.
    }  // Конец метода `HedgeRow`.

    // Создаёт створки калитки в проёме.
    public static void Wicket(Transform yard, Material material, float x0, float x1, float z, float height, float openYaw)
    {  // Начало метода `Wicket`.
        // Берёт модуль числа через `Mathf.Abs` с аргументами `x1 - x0` и записывает результат в `width`.
        float width = Mathf.Abs(x1 - x0);
        // Вычисляет `leaf` как `width` умножить на `0.5` минус `0.02`.
        float leaf = width * 0.5f - 0.02f;
        // Размещает створку относительно точки петли и угла открытия через `Leaf` с аргументами `yard, material, new Vector3(x0, 0f, z), openYaw, leaf, height`.
        Leaf(yard, material, new Vector3(x0, 0f, z), openYaw, leaf, height);
        // Размещает створку относительно точки петли и угла открытия через `Leaf` с аргументами `yard, material, new Vector3(x1, 0f, z), -openYaw, -leaf, height`.
        Leaf(yard, material, new Vector3(x1, 0f, z), -openYaw, -leaf, height);
        // Создаёт столб забора через `Post` с аргументами `yard, material, new Vector3(x0, height * 0.5f, z), height`.
        Post(yard, material, new Vector3(x0, height * 0.5f, z), height);
        // Создаёт столб забора через `Post` с аргументами `yard, material, new Vector3(x1, height * 0.5f, z), height`.
        Post(yard, material, new Vector3(x1, height * 0.5f, z), height);
    }  // Конец метода `Wicket`.

    // Размещает створку относительно точки петли и угла открытия.
    static void Leaf(Transform yard, Material material, Vector3 hinge, float yaw, float length, float height)
    {  // Начало метода `Leaf`.
        // Задаёт `root` значением `new GameObject("Калитка").transform`.
        var root = new GameObject("Калитка").transform;
        // Привязывает к родителю через `root.SetParent` с аргументами `yard, false`.
        root.SetParent(yard, false);
        // Задаёт локальная позиция `root.localPosition` значением `hinge`.
        root.localPosition = hinge;
        // Создаёт поворот из углов Эйлера через `Quaternion.Euler` с аргументами `0f, yaw, 0f` и записывает результат в `root.localRotation`.
        root.localRotation = Quaternion.Euler(0f, yaw, 0f);
        // Вызывает `Mathf.Sign` с аргументами `length` и записывает результат в `dir`.
        float dir = Mathf.Sign(length);
        // Берёт модуль числа через `Mathf.Abs` с аргументами `length` и записывает результат в `span`.
        float span = Mathf.Abs(length);
        // Создаёт куб с заданными родителем, положением, размерами и материалом через `Box` с аргументами `root, "Жердь", new Vector3(dir * span * 0.5f, 0.42f, 0f), new Vector3(span, 0.07f, 0.07f), material`.
        Box(root, "Жердь", new Vector3(dir * span * 0.5f, 0.42f, 0f), new Vector3(span, 0.07f, 0.07f), material);
        // Создаёт куб с заданными родителем, положением, размерами и материалом через `Box` с аргументами `root, "Жердь", new Vector3(dir * span * 0.5f, height - 0.16f, 0f), new Vector3(span, 0.07f, 0.07f), material`.
        Box(root, "Жердь", new Vector3(dir * span * 0.5f, height - 0.16f, 0f), new Vector3(span, 0.07f, 0.07f), material);
        // Перебирает индексы начиная с `float x = 0.08f` при условии `x < span - 0.04f`.
        for (float x = 0.08f; x < span - 0.04f; x += 0.16f)
            // Создаёт куб с заданными родителем, положением, размерами и материалом через `Box` с аргументами `root, "Штакет", new Vector3(dir * x, height * 0.5f + 0.04f, 0f), new Vector3(0.07f, height - 0.08f, 0.05f), material`.
            Box(root, "Штакет", new Vector3(dir * x, height * 0.5f + 0.04f, 0f), new Vector3(0.07f, height - 0.08f, 0.05f), material);
        // Добавляет компонент через `root.gameObject.AddComponent<BoxCollider>` и записывает результат в `block`.
        var block = root.gameObject.AddComponent<BoxCollider>();
        // Записывает в `block.center` трёхмерный вектор с параметрами `dir * span * 0.5f, height * 0.5f, 0f`.
        block.center = new Vector3(dir * span * 0.5f, height * 0.5f, 0f);
        // Записывает в `block.size` трёхмерный вектор с параметрами `span, height, 0.16f`.
        block.size = new Vector3(span, height, 0.16f);
    }  // Конец метода `Leaf`.

    // Распределяет планки вдоль секции забора.
    public static void Palings(Transform yard, Material material, float a0, float a1, float fixedAxis, bool alongX, float height)
    {  // Начало метода `Palings`.
        // Начинает вызов `FenceRun` с аргументами на следующих строках.
        FenceRun(yard, material,
            // Передаёт аргумент `alongX ? new Vector3((a0 + a1) * 0.5f, height * 0.55f, fixedAxis) : new Vector3(fixedAxis, height * 0.55f, (a0 + a1) * 0.5f)`.
            alongX ? new Vector3((a0 + a1) * 0.5f, height * 0.55f, fixedAxis) : new Vector3(fixedAxis, height * 0.55f, (a0 + a1) * 0.5f),
            // Передаёт аргумент `alongX ? new Vector3(Mathf.Abs(a1 - a0) + 0.12f, 0.08f, 0.08f) : new Vector3(0.08f, 0.08f, Mathf.Abs(a1 - a0) + 0.12f)`.
            alongX ? new Vector3(Mathf.Abs(a1 - a0) + 0.12f, 0.08f, 0.08f) : new Vector3(0.08f, 0.08f, Mathf.Abs(a1 - a0) + 0.12f));
        // Перебирает индексы начиная с `float a = Mathf.Min(a0, a1)` при условии `a <= Mathf.Max(a0, a1)`.
        for (float a = Mathf.Min(a0, a1); a <= Mathf.Max(a0, a1); a += 0.28f)
        {  // Начало цикла по индексам.
            // Выбирает значение для `pos` по условию `alongX`.
            var pos = alongX ? new Vector3(a, height * 0.5f + 0.05f, fixedAxis) : new Vector3(fixedAxis, height * 0.5f + 0.05f, a);
            // Выбирает значение для `scale` по условию `alongX`.
            var scale = alongX ? new Vector3(0.1f, height - 0.08f, 0.07f) : new Vector3(0.07f, height - 0.08f, 0.1f);
            // Создаёт прямоугольную секцию ограды через `FenceRun` с аргументами `yard, material, pos, scale`.
            FenceRun(yard, material, pos, scale);
        }  // Конец цикла по индексам.
    }  // Конец метода `Palings`.

    // Создаёт столб забора.
    static void Post(Transform yard, Material material, Vector3 position, float height)
    {  // Начало метода `Post`.
        // Создаёт куб с заданными родителем, положением, размерами и материалом через `Box` с аргументами `yard, "Столб", position, new Vector3(0.16f, height, 0.16f), material`.
        Box(yard, "Столб", position, new Vector3(0.16f, height, 0.16f), material);
    }  // Конец метода `Post`.

    // Добавляет окно указанного размера.
    static void Window(Transform yard, Vector3 position, Vector3 scale)
    {  // Начало метода `Window`.
        // Создаёт куб с заданными родителем, положением, размерами и материалом через `Box` с аргументами `yard, "Рама", position, scale + new Vector3(0.02f, 0.12f, 0.14f), TextureFactory.Lit(Palette.Handle, TextureFactory.Wood())`.
        Box(yard, "Рама", position, scale + new Vector3(0.02f, 0.12f, 0.14f), TextureFactory.Lit(Palette.Handle, TextureFactory.Wood()));
        // Создаёт куб с заданными родителем, положением, размерами и материалом через `Box` с аргументами `yard, "Окно", position + new Vector3(0.04f, 0f, 0f), scale, TextureFactory.Lit(Palette.Window, null, 0.7f)`.
        Box(yard, "Окно", position + new Vector3(0.04f, 0f, 0f), scale, TextureFactory.Lit(Palette.Window, null, 0.7f));
    }  // Конец метода `Window`.

    // Создаёт прямоугольную секцию ограды.
    static void FenceRun(Transform parent, Material material, Vector3 position, Vector3 scale)
    {  // Начало метода `FenceRun`.
        // Создаёт куб с заданными родителем, положением, размерами и материалом через `Box` с аргументами `parent, "Забор", position, scale, material`.
        Box(parent, "Забор", position, scale, material);
    }  // Конец метода `FenceRun`.

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
        // Возвращает `go.transform`.
        return go.transform;
    }  // Конец метода `Box`.
}  // Конец класса `YardDress`.

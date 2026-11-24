// Подключает доступ к базовым типам игрового движка.
using UnityEngine;

// Собирает план двора, гараж, ограждение, лопату и продавца.
public static class YardLayout
{  // Начало класса `YardLayout`.
    // Задаёт `LotMinX` значением `-10f`.
    public const float LotMinX = -10f;
    // Задаёт `LotMaxX` значением `13`.
    public const float LotMaxX = 13f;
    // Задаёт `LotMinZ` значением `-6.2f`.
    public const float LotMinZ = -6.2f;
    // Задаёт `LotMaxZ` значением `14`.
    public const float LotMaxZ = 14f;
    // Задаёт `PitMin` значением `-0.5f`.
    public const float PitMin = -0.5f;
    // Задаёт `PitMax` значением `10.5`.
    public const float PitMax = 10.5f;

    // Создаёт окружение игрового двора и возвращает его корень.
    public static Transform Build(Transform gameRoot)
    {  // Начало метода `Build`.
        // Проверяет условие `gameRoot.Find("Двор") != null`.
        if (gameRoot.Find("Двор") != null)
            // Возвращает результат вызова `gameRoot.Find("Двор")`.
            return gameRoot.Find("Двор");

        // Задаёт `yard` значением `new GameObject("Двор").transform`.
        var yard = new GameObject("Двор").transform;
        // Привязывает к родителю через `yard.SetParent` с аргументами `gameRoot, false`.
        yard.SetParent(gameRoot, false);

        // Вызывает `TextureFactory.Lit` с аргументами `Palette.Yard, TextureFactory.Grass()` и записывает результат в `yardMat`.
        var yardMat = TextureFactory.Lit(Palette.Yard, TextureFactory.Grass());
        // Вызывает `TextureFactory.Lit` с аргументами `Palette.HouseWall, TextureFactory.Speckled()` и записывает результат в `wallMat`.
        var wallMat = TextureFactory.Lit(Palette.HouseWall, TextureFactory.Speckled());
        // Вызывает `TextureFactory.Lit` с аргументами `Palette.HouseRoof, TextureFactory.Wood()` и записывает результат в `roofMat`.
        var roofMat = TextureFactory.Lit(Palette.HouseRoof, TextureFactory.Wood());
        // Вызывает `TextureFactory.Lit` с аргументами `Palette.HouseWall, TextureFactory.Speckled(), 0.2f` и записывает результат в `garageMat`.
        var garageMat = TextureFactory.Lit(Palette.HouseWall, TextureFactory.Speckled(), 0.2f);
        // Вызывает `TextureFactory.Lit` с аргументами `Palette.Fence, TextureFactory.Wood()` и записывает результат в `fenceMat`.
        var fenceMat = TextureFactory.Lit(Palette.Fence, TextureFactory.Wood());

        // Создаёт прямоугольную часть травяного покрытия через `Lawn` с аргументами `yard, yardMat, LotMinX, LotMaxX, LotMinZ, PitMin`.
        Lawn(yard, yardMat, LotMinX, LotMaxX, LotMinZ, PitMin);
        // Создаёт прямоугольную часть травяного покрытия через `Lawn` с аргументами `yard, yardMat, LotMinX, LotMaxX, PitMax, LotMaxZ`.
        Lawn(yard, yardMat, LotMinX, LotMaxX, PitMax, LotMaxZ);
        // Создаёт прямоугольную часть травяного покрытия через `Lawn` с аргументами `yard, yardMat, PitMax, LotMaxX, PitMin, PitMax`.
        Lawn(yard, yardMat, PitMax, LotMaxX, PitMin, PitMax);
        // Создаёт прямоугольную часть травяного покрытия через `Lawn` с аргументами `yard, yardMat, LotMinX, PitMin, PitMin, PitMax`.
        Lawn(yard, yardMat, LotMinX, PitMin, PitMin, PitMax);
        // Добавляет переход между краем раскопа и поверхностью двора через `Collar` с аргументами `yard, PitMin, PitMax`.
        Collar(yard, PitMin, PitMax);

        // Вызывает `YardDress.House` с аргументами `yard, wallMat, roofMat`.
        YardDress.House(yard, wallMat, roofMat);
        // Собирает геометрию гаража и его наполнение через `BuildGarage` с аргументами `yard, garageMat, roofMat`.
        BuildGarage(yard, garageMat, roofMat);
        // Вызывает `YardDress.PitFence` с аргументами `yard, fenceMat`.
        YardDress.PitFence(yard, fenceMat);
        // Строит забор по периметру участка через `BuildLotFence` с аргументами `yard, fenceMat, LotMinX, LotMaxX, LotMinZ, LotMaxZ`.
        BuildLotFence(yard, fenceMat, LotMinX, LotMaxX, LotMinZ, LotMaxZ);
        // Вызывает `YardDress.Props` с аргументами `yard`.
        YardDress.Props(yard);
        // Вызывает `GarageSetdress.Fill` с аргументами `yard`.
        GarageSetdress.Fill(yard);
        // Устанавливает слой объекта и потомков через `SetLayer` с аргументами `yard, 8`.
        SetLayer(yard, 8);

        // Вызывает `GarageSetdress.Shop` с аргументами `yard, new Vector3(-2.15f, 0f, 3.55f)`.
        GarageSetdress.Shop(yard, new Vector3(-2.15f, 0f, 3.55f));
        // Вызывает `GarageSetdress.Anvil` с аргументами `yard, new Vector3(-6.95f, 0f, 2.35f)`.
        GarageSetdress.Anvil(yard, new Vector3(-6.95f, 0f, 2.35f));

        // Ставит подбираемую лопату в начальную точку двора через `CreateShovel` с аргументами `yard`.
        CreateShovel(yard);
        // Добавляет продавца и торговое место через `CreateSeller` с аргументами `yard`.
        CreateSeller(yard);

        // Создаёт окружение игрового двора и возвращает его корень через `StreetScene.Build` с аргументами `yard`.
        StreetScene.Build(yard);
        // Создаёт экземпляр модели и размещает его в иерархии через `FallReset.Spawn` с аргументами `yard`.
        FallReset.Spawn(yard);
        // Проверяет условие `yard.Find("Атмосфера") == null`.
        if (yard.Find("Атмосфера") == null)
        {  // Начало ветки условия.
            // Создаёт объект сцены с параметрами `"Атмосфера"` и сохраняет ссылку в `ambient`.
            var ambient = new GameObject("Атмосфера");
            // Привязывает к родителю через `ambient.transform.SetParent` с аргументами `yard, false`.
            ambient.transform.SetParent(yard, false);
            // Добавляет компонент через `ambient.AddComponent<AmbientBed>`.
            ambient.AddComponent<AmbientBed>();
        }  // Конец ветки условия.

        // Возвращает `yard`.
        return yard;
    }  // Конец метода `Build`.

    // Ставит подбираемую лопату в начальную точку двора.
    public static Transform CreateShovel(Transform yard)
    {  // Начало метода `CreateShovel`.
        // Проверяет условие `yard.Find("Лопата") != null`.
        if (yard.Find("Лопата") != null)
            // Возвращает результат вызова `yard.Find("Лопата")`.
            return yard.Find("Лопата");

        // Задаёт `shovel` значением `new GameObject("Лопата").transform`.
        var shovel = new GameObject("Лопата").transform;
        // Привязывает к родителю через `shovel.SetParent` с аргументами `yard, false`.
        shovel.SetParent(yard, false);
        // Вызывает `YardMetrics.CellCenter` с аргументами `16, 0, 10` и записывает результат в `shovelCell`.
        var shovelCell = YardMetrics.CellCenter(16, 0, 10);
        // Записывает в `shovel.position` трёхмерный вектор с параметрами `shovelCell.x, 0f, shovelCell.z`.
        shovel.position = new Vector3(shovelCell.x, 0f, shovelCell.z);
        // Создаёт экземпляр модели и размещает его в иерархии через `PropKit.Spawn` с аргументами `"shovel", shovel, Vector3.zero, Vector3.zero, 1.15f, false, false` и записывает результат в `shovelModel`.
        var shovelModel = PropKit.Spawn("shovel", shovel, Vector3.zero, Vector3.zero, 1.15f, false, false);
        // Проверяет условие `shovelModel == null`.
        if (shovelModel == null)
        {  // Начало ветки условия.
            // Создаёт куб с заданными родителем, положением, размерами и материалом через `Box` с аргументами `shovel, "Черенок", Vector3.zero, new Vector3(0.07f, 0.92f, 0.07f), Mat(Palette.Handle)`.
            Box(shovel, "Черенок", Vector3.zero, new Vector3(0.07f, 0.92f, 0.07f), Mat(Palette.Handle));
            // Создаёт куб с заданными родителем, положением, размерами и материалом через `Box` с аргументами `shovel, "Полотно", new Vector3(0f, 0.52f, 0.02f), new Vector3(0.26f, 0.32f, 0.04f), Mat(Palette.Blade)`.
            Box(shovel, "Полотно", new Vector3(0f, 0.52f, 0.02f), new Vector3(0.26f, 0.32f, 0.04f), Mat(Palette.Blade));
        }  // Конец ветки условия.
        // Создаёт поворот из углов Эйлера через `Quaternion.Euler` с аргументами `0f, 35f, 90f` и записывает результат в `shovel.rotation`.
        shovel.rotation = Quaternion.Euler(0f, 35f, 90f);
        // Вызывает `KenneyKit.SitOnGround` с аргументами `shovel`.
        KenneyKit.SitOnGround(shovel);
        // Добавляет компонент через `shovel.gameObject.AddComponent<BoxCollider>` и записывает результат в `shovelCollider`.
        var shovelCollider = shovel.gameObject.AddComponent<BoxCollider>();
        // Задаёт центр `shovelCollider.center` значением нулевой вектор `Vector3.zero`.
        shovelCollider.center = Vector3.zero;
        // Записывает в `shovelCollider.size` трёхмерный вектор с параметрами `1.2f, 0.22f, 0.35f`.
        shovelCollider.size = new Vector3(1.2f, 0.22f, 0.35f);
        // Добавляет компонент через `shovel.gameObject.AddComponent<ShovelItem>`.
        shovel.gameObject.AddComponent<ShovelItem>();
        // Возвращает `shovel`.
        return shovel;
    }  // Конец метода `CreateShovel`.

    // Добавляет продавца и торговое место.
    static void CreateSeller(Transform yard)
    {  // Начало метода `CreateSeller`.
        // Проверяет условие `yard.Find("Продавец") != null`.
        if (yard.Find("Продавец") != null)
            // Завершает выполнение текущего метода.
            return;

        // Ищет объект по имени через `yard.Find` с аргументами `"Магазин"` и записывает результат в `shop`.
        var shop = yard.Find("Магазин");
        // Создаёт объект сцены с параметрами `"Продавец"` и сохраняет ссылку в `sellerGo`.
        var sellerGo = new GameObject("Продавец");
        // Привязывает к родителю через `sellerGo.transform.SetParent` с аргументами `shop != null ? shop : yard, false`.
        sellerGo.transform.SetParent(shop != null ? shop : yard, false);
        // Проверяет условие `shop != null`.
        if (shop != null)
            // Вызывает `SellerPlacement.EnsureAnchor` с аргументами `shop`.
            SellerPlacement.EnsureAnchor(shop);
        // Выбирает локальная позиция `sellerGo.transform.localPosition` по условию `shop` существует.
        sellerGo.transform.localPosition = shop != null ? new Vector3(0f, 0f, 0.54f) : new Vector3(-2.05f, 0f, 4.4f);
        // Добавляет компонент через `sellerGo.AddComponent<Seller>`.
        sellerGo.AddComponent<Seller>();
    }  // Конец метода `CreateSeller`.

    // Собирает геометрию гаража и его наполнение.
    static void BuildGarage(Transform yard, Material wall, Material roof)
    {  // Начало метода `BuildGarage`.
        // Задаёт `x` значением `-5.1f`.
        const float x = -5.1f;
        // Вызывает `TextureFactory.Lit` с аргументами `Palette.Concrete, TextureFactory.Speckled(), 0.14f` и записывает результат в `floor`.
        var floor = TextureFactory.Lit(Palette.Concrete, TextureFactory.Speckled(), 0.14f);
        // Создаёт куб с заданными родителем, положением, размерами и материалом через `Box` с аргументами `yard, "Гараж пол", new Vector3(x, 0.05f, 2.42f), new Vector3(7.7f, 0.06f, 4.7f), floor`.
        Box(yard, "Гараж пол", new Vector3(x, 0.05f, 2.42f), new Vector3(7.7f, 0.06f, 4.7f), floor);
        // Создаёт куб с заданными родителем, положением, размерами и материалом через `Box` с аргументами `yard, "Гараж зад", new Vector3(x - 3.55f, 1.55f, 2.35f), new Vector3(0.28f, 3.1f, 4.7f), wall`.
        Box(yard, "Гараж зад", new Vector3(x - 3.55f, 1.55f, 2.35f), new Vector3(0.28f, 3.1f, 4.7f), wall);
        // Создаёт куб с заданными родителем, положением, размерами и материалом через `Box` с аргументами `yard, "Гараж север", new Vector3(x, 1.55f, 4.58f), new Vector3(7.4f, 3.1f, 0.24f), wall`.
        Box(yard, "Гараж север", new Vector3(x, 1.55f, 4.58f), new Vector3(7.4f, 3.1f, 0.24f), wall);
        // Создаёт куб с заданными родителем, положением, размерами и материалом через `Box` с аргументами `yard, "Гараж юг", new Vector3(x, 1.55f, 0.12f), new Vector3(7.4f, 3.1f, 0.24f), wall`.
        Box(yard, "Гараж юг", new Vector3(x, 1.55f, 0.12f), new Vector3(7.4f, 3.1f, 0.24f), wall);
        // Создаёт куб с заданными родителем, положением, размерами и материалом через `Box` с аргументами `yard, "Гараж крыша", new Vector3(x, 3.2f, 2.35f), new Vector3(7.7f, 0.22f, 5f), roof`.
        Box(yard, "Гараж крыша", new Vector3(x, 3.2f, 2.35f), new Vector3(7.7f, 0.22f, 5f), roof);
        // Создаёт куб с заданными родителем, положением, размерами и материалом через `Box` с аргументами `yard, "Окно гаража", new Vector3(x - 1.2f, 1.85f, 0.26f), new Vector3(1.15f, 0.85f, 0.06f), TextureFactory.Lit(Palette.Window, null, 0.7f)`.
        Box(yard, "Окно гаража", new Vector3(x - 1.2f, 1.85f, 0.26f), new Vector3(1.15f, 0.85f, 0.06f), TextureFactory.Lit(Palette.Window, null, 0.7f));
        // Создаёт куб с заданными родителем, положением, размерами и материалом через `Box` с аргументами `yard, "Рама гаража", new Vector3(x - 1.2f, 1.85f, 0.22f), new Vector3(1.32f, 1f, 0.08f), TextureFactory.Lit(Palette.Handle, TextureFactory.Wood())`.
        Box(yard, "Рама гаража", new Vector3(x - 1.2f, 1.85f, 0.22f), new Vector3(1.32f, 1f, 0.08f), TextureFactory.Lit(Palette.Handle, TextureFactory.Wood()));
        // Создаёт куб с заданными родителем, положением, размерами и материалом через `Box` с аргументами `yard, "Балка", new Vector3(x + 3.55f, 3.05f, 2.35f), new Vector3(0.18f, 0.18f, 4.7f), Mat(Palette.Handle)`.
        Box(yard, "Балка", new Vector3(x + 3.55f, 3.05f, 2.35f), new Vector3(0.18f, 0.18f, 4.7f), Mat(Palette.Handle));
        // Создаёт куб с заданными родителем, положением, размерами и материалом через `Box` с аргументами `yard, "Косяк", new Vector3(x + 3.55f, 1.45f, 0.28f), new Vector3(0.16f, 2.9f, 0.16f), Mat(Palette.Handle)`.
        Box(yard, "Косяк", new Vector3(x + 3.55f, 1.45f, 0.28f), new Vector3(0.16f, 2.9f, 0.16f), Mat(Palette.Handle));
        // Создаёт куб с заданными родителем, положением, размерами и материалом через `Box` с аргументами `yard, "Косяк", new Vector3(x + 3.55f, 1.45f, 4.42f), new Vector3(0.16f, 2.9f, 0.16f), Mat(Palette.Handle)`.
        Box(yard, "Косяк", new Vector3(x + 3.55f, 1.45f, 4.42f), new Vector3(0.16f, 2.9f, 0.16f), Mat(Palette.Handle));
        // Создаёт куб с заданными родителем, положением, размерами и материалом через `Box` с аргументами `yard, "Порог", new Vector3(x + 3.55f, 0.06f, 2.35f), new Vector3(0.4f, 0.08f, 4.4f), TextureFactory.Lit(Palette.Concrete, TextureFactory.Speckled(), 0.15f)`.
        Box(yard, "Порог", new Vector3(x + 3.55f, 0.06f, 2.35f), new Vector3(0.4f, 0.08f, 4.4f), TextureFactory.Lit(Palette.Concrete, TextureFactory.Speckled(), 0.15f));
    }  // Конец метода `BuildGarage`.

    // Создаёт прямоугольную часть травяного покрытия.
    static void Lawn(Transform yard, Material material, float x0, float x1, float z0, float z1)
    {  // Начало метода `Lawn`.
        // Вычисляет `w` как `x1` минус `x0`.
        float w = x1 - x0;
        // Вычисляет `d` как `z1` минус `z0`.
        float d = z1 - z0;
        // Проверяет условие `w < 0.2f || d < 0.2f`.
        if (w < 0.2f || d < 0.2f)
            // Завершает выполнение текущего метода.
            return;
        // Вызывает `TextureFactory.Lit` с аргументами `Palette.Yard, TextureFactory.Grass()` и записывает результат в `mat`.
        var mat = TextureFactory.Lit(Palette.Yard, TextureFactory.Grass());
        // Вызывает `mat.SetTextureScale` с аргументами `"_BaseMap", new Vector2(Mathf.Max(1f, w * 0.7f), Mathf.Max(1f, d * 0.7f))`.
        mat.SetTextureScale("_BaseMap", new Vector2(Mathf.Max(1f, w * 0.7f), Mathf.Max(1f, d * 0.7f)));
        // Создаёт куб с заданными родителем, положением, размерами и материалом через `Box` с аргументами `yard, "Газон", new Vector3((x0 + x1) * 0.5f, 0f, (z0 + z1) * 0.5f), new Vector3(w, 0.12f, d), mat`.
        Box(yard, "Газон", new Vector3((x0 + x1) * 0.5f, 0f, (z0 + z1) * 0.5f), new Vector3(w, 0.12f, d), mat);
    }  // Конец метода `Lawn`.

    // Добавляет переход между краем раскопа и поверхностью двора.
    static void Collar(Transform yard, float pitMin, float pitMax)
    {  // Начало метода `Collar`.
        // Вызывает `TextureFactory.Lit` с аргументами `Palette.Dirt, TextureFactory.Speckled(), 0.1f` и записывает результат в `dirt`.
        var dirt = TextureFactory.Lit(Palette.Dirt, TextureFactory.Speckled(), 0.1f);
        // Задаёт `w` значением `0.22`.
        const float w = 0.22f;
        // Задаёт `y` значением `-0.06f`.
        const float y = -0.06f;
        // Создаёт куб с заданными родителем, положением, размерами и материалом через `Box` с аргументами `yard, "Бровка", new Vector3((pitMin + pitMax) * 0.5f, y, pitMin + 0.16f), new Vector3(pitMax - pitMin - 0.08f, 0.04f, w), dirt`.
        Box(yard, "Бровка", new Vector3((pitMin + pitMax) * 0.5f, y, pitMin + 0.16f), new Vector3(pitMax - pitMin - 0.08f, 0.04f, w), dirt);
        // Создаёт куб с заданными родителем, положением, размерами и материалом через `Box` с аргументами `yard, "Бровка", new Vector3((pitMin + pitMax) * 0.5f, y, pitMax - 0.16f), new Vector3(pitMax - pitMin - 0.08f, 0.04f, w), dirt`.
        Box(yard, "Бровка", new Vector3((pitMin + pitMax) * 0.5f, y, pitMax - 0.16f), new Vector3(pitMax - pitMin - 0.08f, 0.04f, w), dirt);
        // Создаёт куб с заданными родителем, положением, размерами и материалом через `Box` с аргументами `yard, "Бровка", new Vector3(pitMin + 0.16f, y, (pitMin + pitMax) * 0.5f), new Vector3(w, 0.04f, pitMax - pitMin - 0.08f), dirt`.
        Box(yard, "Бровка", new Vector3(pitMin + 0.16f, y, (pitMin + pitMax) * 0.5f), new Vector3(w, 0.04f, pitMax - pitMin - 0.08f), dirt);
        // Создаёт куб с заданными родителем, положением, размерами и материалом через `Box` с аргументами `yard, "Бровка", new Vector3(pitMax - 0.16f, y, (pitMin + pitMax) * 0.5f), new Vector3(w, 0.04f, pitMax - pitMin - 0.08f), dirt`.
        Box(yard, "Бровка", new Vector3(pitMax - 0.16f, y, (pitMin + pitMax) * 0.5f), new Vector3(w, 0.04f, pitMax - pitMin - 0.08f), dirt);
    }  // Конец метода `Collar`.

    // Строит забор по периметру участка.
    static void BuildLotFence(Transform yard, Material material, float x0, float x1, float z0, float z1)
    {  // Начало метода `BuildLotFence`.
        // Задаёт `height` значением `2.15`.
        const float height = 2.15f;
        // Задаёт `gateA` значением `4.05`.
        const float gateA = 4.05f;
        // Задаёт `gateB` значением `5.95`.
        const float gateB = 5.95f;
        // Строит ряд планок забора через `Pales` с аргументами `yard, material, x0, gateA, z0, true, height`.
        Pales(yard, material, x0, gateA, z0, true, height);
        // Строит ряд планок забора через `Pales` с аргументами `yard, material, gateB, x1, z0, true, height`.
        Pales(yard, material, gateB, x1, z0, true, height);
        // Строит ряд планок забора через `Pales` с аргументами `yard, material, x0, x1, z1, true, height`.
        Pales(yard, material, x0, x1, z1, true, height);
        // Строит ряд планок забора через `Pales` с аргументами `yard, material, z0, z1, x0, false, height`.
        Pales(yard, material, z0, z1, x0, false, height);
        // Строит ряд планок забора через `Pales` с аргументами `yard, material, z0, z1, x1, false, height`.
        Pales(yard, material, z0, z1, x1, false, height);
        // Вызывает `YardDress.Wicket` с аргументами `yard, material, gateA, gateB, z0, 1.62f, 0f`.
        YardDress.Wicket(yard, material, gateA, gateB, z0, 1.62f, 0f);
    }  // Конец метода `BuildLotFence`.

    // Строит ряд планок забора.
    static void Pales(Transform yard, Material material, float a0, float a1, float fixedAxis, bool alongX, float height)
    {  // Начало метода `Pales`.
        // Начинает вызов `FenceRun` с аргументами на следующих строках.
        FenceRun(yard, material,
            // Передаёт аргумент `alongX ? new Vector3((a0 + a1) * 0.5f, height * 0.38f, fixedAxis) : new Vector3(fixedAxis, height * 0.38f, (a0 + a1) * 0.5f)`.
            alongX ? new Vector3((a0 + a1) * 0.5f, height * 0.38f, fixedAxis) : new Vector3(fixedAxis, height * 0.38f, (a0 + a1) * 0.5f),
            // Передаёт аргумент `alongX ? new Vector3(a1 - a0 + 0.2f, 0.12f, 0.1f) : new Vector3(0.1f, 0.12f, a1 - a0 + 0.2f)`.
            alongX ? new Vector3(a1 - a0 + 0.2f, 0.12f, 0.1f) : new Vector3(0.1f, 0.12f, a1 - a0 + 0.2f));
        // Перебирает индексы начиная с `float a = a0` при условии `a <= a1`.
        for (float a = a0; a <= a1; a += 0.34f)
        {  // Начало цикла по индексам.
            // Выбирает значение для `pos` по условию `alongX`.
            var pos = alongX ? new Vector3(a, height * 0.5f + 0.05f, fixedAxis) : new Vector3(fixedAxis, height * 0.5f + 0.05f, a);
            // Выбирает значение для `scale` по условию `alongX`.
            var scale = alongX ? new Vector3(0.12f, height - 0.08f, 0.08f) : new Vector3(0.08f, height - 0.08f, 0.12f);
            // Создаёт прямоугольную секцию ограды через `FenceRun` с аргументами `yard, material, pos, scale`.
            FenceRun(yard, material, pos, scale);
        }  // Конец цикла по индексам.
    }  // Конец метода `Pales`.

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
        // Ищет компонент через `go.GetComponent<MeshRenderer>` и записывает результат в `renderer`.
        var renderer = go.GetComponent<MeshRenderer>();
        // Задаёт общий материал `renderer.sharedMaterial` значением `material`.
        renderer.sharedMaterial = material;
        // Задаёт режим отбрасывания теней `renderer.shadowCastingMode` значением `UnityEngine.Rendering.ShadowCastingMode.On`.
        renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
        // Устанавливает приём теней `renderer.receiveShadows` в `true`.
        renderer.receiveShadows = true;
        // Возвращает `go.transform`.
        return go.transform;
    }  // Конец метода `Box`.

    // Назначает слой корневому объекту и его потомкам.
    static void SetLayer(Transform root, int layer)
    {  // Начало метода `SetLayer`.
        // Задаёт слой `root.gameObject.layer` значением `layer`.
        root.gameObject.layer = layer;
        // Перебирает индексы начиная с `int i = 0` при условии `i < root.childCount`.
        for (int i = 0; i < root.childCount; i++)
            // Устанавливает слой объекта и потомков через `SetLayer` с аргументами `root.GetChild(i), layer`.
            SetLayer(root.GetChild(i), layer);
    }  // Конец метода `SetLayer`.

    // Создаёт материал с заданным цветом.
    static Material Mat(Color color)
    {  // Начало метода `Mat`.
        // Ищет шейдер по имени через `Shader.Find` с аргументами `"Universal Render Pipeline/Lit"` и записывает результат в `shader`.
        var shader = Shader.Find("Universal Render Pipeline/Lit");
        // Создаёт материал с параметрами `shader` и сохраняет ссылку в `material`.
        var material = new Material(shader);
        // Задаёт цвет `material.color` значением `color`.
        material.color = color;
        // Устанавливает инстансинг материала `material.enableInstancing` в `true`.
        material.enableInstancing = true;
        // Возвращает `material`.
        return material;
    }  // Конец метода `Mat`.
}  // Конец класса `YardLayout`.
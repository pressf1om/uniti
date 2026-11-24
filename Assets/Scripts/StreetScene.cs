// Подключает доступ к базовым типам игрового движка.
using UnityEngine;

// Строит улицу вокруг двора с домами, дорогами и машинами.
public static class StreetScene
{  // Начало класса `StreetScene`.
    // Размещает дорожные сегменты, окружение, светофоры и маршруты.
    public static void Build(Transform parent)
    {  // Начало метода `Build`.
        // Проверяет условие `parent.Find("Город") != null`.
        if (parent.Find("Город") != null)
            // Завершает выполнение текущего метода.
            return;

        // Задаёт `city` значением `new GameObject("Город").transform`.
        var city = new GameObject("Город").transform;
        // Привязывает к родителю через `city.SetParent` с аргументами `parent, false`.
        city.SetParent(parent, false);

        // Вызывает `TextureFactory.Lit` с аргументами `Palette.Asphalt, TextureFactory.Asphalt(), 0.08f` и записывает результат в `asphalt`.
        var asphalt = TextureFactory.Lit(Palette.Asphalt, TextureFactory.Asphalt(), 0.08f);
        // Вызывает `TextureFactory.Lit` с аргументами `Palette.Sidewalk, TextureFactory.Speckled(), 0.12f` и записывает результат в `walk`.
        var walk = TextureFactory.Lit(Palette.Sidewalk, TextureFactory.Speckled(), 0.12f);
        // Вызывает `TextureFactory.Lit` с аргументами `Palette.Brick, TextureFactory.Brick(), 0.2f` и записывает результат в `brick`.
        var brick = TextureFactory.Lit(Palette.Brick, TextureFactory.Brick(), 0.2f);
        // Вызывает `TextureFactory.Lit` с аргументами `Palette.HouseWall, TextureFactory.Speckled(), 0.22f` и записывает результат в `plaster`.
        var plaster = TextureFactory.Lit(Palette.HouseWall, TextureFactory.Speckled(), 0.22f);
        // Вызывает `TextureFactory.Lit` с аргументами `Palette.HouseRoof, TextureFactory.Wood(), 0.18f` и записывает результат в `roof`.
        var roof = TextureFactory.Lit(Palette.HouseRoof, TextureFactory.Wood(), 0.18f);
        // Вызывает `TextureFactory.Lit` с аргументами `Palette.Bark, TextureFactory.Wood(), 0.12f` и записывает результат в `bark`.
        var bark = TextureFactory.Lit(Palette.Bark, TextureFactory.Wood(), 0.12f);
        // Вызывает `TextureFactory.Lit` с аргументами `Palette.Leaf, TextureFactory.Leaves(), 0.1f` и записывает результат в `leaf`.
        var leaf = TextureFactory.Lit(Palette.Leaf, TextureFactory.Leaves(), 0.1f);
        // Вызывает `TextureFactory.Lit` с аргументами `Palette.Window, null, 0.7f` и записывает результат в `glass`.
        var glass = TextureFactory.Lit(Palette.Window, null, 0.7f);

        // Добавляет окружающую землю вне основного двора через `Earth` с аргументами `city`.
        Earth(city);
        // Размещает декоративный примитив на улице через `Prop` с аргументами `city, "Дорога юг", PrimitiveType.Cube, new Vector3(2f, -0.04f, -10.4f), new Vector3(78f, 0.08f, 5.4f), asphalt`.
        Prop(city, "Дорога юг", PrimitiveType.Cube, new Vector3(2f, -0.04f, -10.4f), new Vector3(78f, 0.08f, 5.4f), asphalt);
        // Размещает декоративный примитив на улице через `Prop` с аргументами `city, "Тротуар", PrimitiveType.Cube, new Vector3(2f, 0.02f, -7.4f), new Vector3(78f, 0.06f, 1.5f), walk`.
        Prop(city, "Тротуар", PrimitiveType.Cube, new Vector3(2f, 0.02f, -7.4f), new Vector3(78f, 0.06f, 1.5f), walk);
        // Размещает декоративный примитив на улице через `Prop` с аргументами `city, "Тротуар дальний", PrimitiveType.Cube, new Vector3(2f, 0.02f, -13.4f), new Vector3(78f, 0.06f, 1.5f), walk`.
        Prop(city, "Тротуар дальний", PrimitiveType.Cube, new Vector3(2f, 0.02f, -13.4f), new Vector3(78f, 0.06f, 1.5f), walk);
        // Размещает декоративный примитив на улице через `Prop` с аргументами `city, "Дорога восток", PrimitiveType.Cube, new Vector3(18.5f, -0.04f, 2f), new Vector3(5.2f, 0.08f, 51.2f), asphalt`.
        Prop(city, "Дорога восток", PrimitiveType.Cube, new Vector3(18.5f, -0.04f, 2f), new Vector3(5.2f, 0.08f, 51.2f), asphalt);
        // Размещает декоративный примитив на улице через `Prop` с аргументами `city, "Тротуар восток", PrimitiveType.Cube, new Vector3(15.6f, 0.02f, 2f), new Vector3(1.4f, 0.06f, 51.2f), walk`.
        Prop(city, "Тротуар восток", PrimitiveType.Cube, new Vector3(15.6f, 0.02f, 2f), new Vector3(1.4f, 0.06f, 51.2f), walk);
        // Размещает декоративный примитив на улице через `Prop` с аргументами `city, "Тротуар восток дальний", PrimitiveType.Cube, new Vector3(21.4f, 0.02f, 2f), new Vector3(1.4f, 0.06f, 51.2f), walk`.
        Prop(city, "Тротуар восток дальний", PrimitiveType.Cube, new Vector3(21.4f, 0.02f, 2f), new Vector3(1.4f, 0.06f, 51.2f), walk);

        // Размещает уличный дом, используя импортированную модель или примитивы через `House` с аргументами `city, new Vector3(-18f, 0f, -16.6f), 0f, "building-type-a", plaster, roof, brick, glass`.
        House(city, new Vector3(-18f, 0f, -16.6f), 0f, "building-type-a", plaster, roof, brick, glass);
        // Размещает уличный дом, используя импортированную модель или примитивы через `House` с аргументами `city, new Vector3(-8f, 0f, -17f), 0f, "building-type-c", brick, roof, brick, glass`.
        House(city, new Vector3(-8f, 0f, -17f), 0f, "building-type-c", brick, roof, brick, glass);
        // Размещает уличный дом, используя импортированную модель или примитивы через `House` с аргументами `city, new Vector3(2.5f, 0f, -16.5f), 0f, "building-type-h", plaster, roof, brick, glass`.
        House(city, new Vector3(2.5f, 0f, -16.5f), 0f, "building-type-h", plaster, roof, brick, glass);
        // Размещает уличный дом, используя импортированную модель или примитивы через `House` с аргументами `city, new Vector3(12.5f, 0f, -17.2f), 0f, "building-type-k", brick, roof, brick, glass`.
        House(city, new Vector3(12.5f, 0f, -17.2f), 0f, "building-type-k", brick, roof, brick, glass);
        // Размещает уличный дом, используя импортированную модель или примитивы через `House` с аргументами `city, new Vector3(22f, 0f, -16.6f), 0f, "building-type-m", plaster, roof, brick, glass`.
        House(city, new Vector3(22f, 0f, -16.6f), 0f, "building-type-m", plaster, roof, brick, glass);
        // Размещает уличный дом, используя импортированную модель или примитивы через `House` с аргументами `city, new Vector3(22.5f, 0f, -1f), -90f, "building-type-p", plaster, roof, brick, glass`.
        House(city, new Vector3(22.5f, 0f, -1f), -90f, "building-type-p", plaster, roof, brick, glass);
        // Размещает уличный дом, используя импортированную модель или примитивы через `House` с аргументами `city, new Vector3(23f, 0f, 8.5f), -90f, "building-type-a", brick, roof, brick, glass`.
        House(city, new Vector3(23f, 0f, 8.5f), -90f, "building-type-a", brick, roof, brick, glass);
        // Размещает уличный дом, используя импортированную модель или примитивы через `House` с аргументами `city, new Vector3(21.8f, 0f, 18f), -90f, "building-type-c", plaster, roof, brick, glass`.
        House(city, new Vector3(21.8f, 0f, 18f), -90f, "building-type-c", plaster, roof, brick, glass);
        // Размещает уличный дом, используя импортированную модель или примитивы через `House` с аргументами `city, new Vector3(-18.5f, 0f, 6f), 90f, "building-type-h", brick, roof, brick, glass`.
        House(city, new Vector3(-18.5f, 0f, 6f), 90f, "building-type-h", brick, roof, brick, glass);
        // Размещает уличный дом, используя импортированную модель или примитивы через `House` с аргументами `city, new Vector3(-19f, 0f, 16f), 90f, "building-type-k", plaster, roof, brick, glass`.
        House(city, new Vector3(-19f, 0f, 16f), 90f, "building-type-k", plaster, roof, brick, glass);
        // Размещает уличный дом, используя импортированную модель или примитивы через `House` с аргументами `city, new Vector3(4f, 0f, 20.5f), 180f, "building-type-m", brick, roof, brick, glass`.
        House(city, new Vector3(4f, 0f, 20.5f), 180f, "building-type-m", brick, roof, brick, glass);

        // Размещает дерево вдоль улицы через `Tree` с аргументами `city, new Vector3(-14f, 0f, -8.8f), bark, leaf`.
        Tree(city, new Vector3(-14f, 0f, -8.8f), bark, leaf);
        // Размещает дерево вдоль улицы через `Tree` с аргументами `city, new Vector3(-3.5f, 0f, -8.8f), bark, leaf`.
        Tree(city, new Vector3(-3.5f, 0f, -8.8f), bark, leaf);
        // Размещает дерево вдоль улицы через `Tree` с аргументами `city, new Vector3(8.5f, 0f, -8.8f), bark, leaf`.
        Tree(city, new Vector3(8.5f, 0f, -8.8f), bark, leaf);
        // Размещает дерево вдоль улицы через `Tree` с аргументами `city, new Vector3(16.2f, 0f, 2.8f), bark, leaf`.
        Tree(city, new Vector3(16.2f, 0f, 2.8f), bark, leaf);
        // Размещает дерево вдоль улицы через `Tree` с аргументами `city, new Vector3(16.5f, 0f, 12.5f), bark, leaf`.
        Tree(city, new Vector3(16.5f, 0f, 12.5f), bark, leaf);
        // Размещает дерево вдоль улицы через `Tree` с аргументами `city, new Vector3(-12.8f, 0f, 12.2f), bark, leaf`.
        Tree(city, new Vector3(-12.8f, 0f, 12.2f), bark, leaf);
        // Размещает дерево вдоль улицы через `Tree` с аргументами `city, new Vector3(1.5f, 0f, 16.8f), bark, leaf`.
        Tree(city, new Vector3(1.5f, 0f, 16.8f), bark, leaf);
        // Размещает дерево вдоль улицы через `Tree` с аргументами `city, new Vector3(11f, 0f, 16.4f), bark, leaf`.
        Tree(city, new Vector3(11f, 0f, 16.4f), bark, leaf);
        // Размещает дерево вдоль улицы через `Tree` с аргументами `city, new Vector3(-16f, 0f, -14.2f), bark, leaf`.
        Tree(city, new Vector3(-16f, 0f, -14.2f), bark, leaf);
        // Размещает дерево вдоль улицы через `Tree` с аргументами `city, new Vector3(18f, 0f, -13.8f), bark, leaf`.
        Tree(city, new Vector3(18f, 0f, -13.8f), bark, leaf);
        // Размещает дерево вдоль улицы через `Tree` с аргументами `city, new Vector3(-22f, 0f, -7.4f), bark, leaf`.
        Tree(city, new Vector3(-22f, 0f, -7.4f), bark, leaf);
        // Размещает дерево вдоль улицы через `Tree` с аргументами `city, new Vector3(26f, 0f, 4f), bark, leaf`.
        Tree(city, new Vector3(26f, 0f, 4f), bark, leaf);
        // Размещает дерево вдоль улицы через `Tree` с аргументами `city, new Vector3(-6f, 0f, 18.5f), bark, leaf`.
        Tree(city, new Vector3(-6f, 0f, 18.5f), bark, leaf);

        // Добавляет уличный фонарь через `Lamp` с аргументами `city, new Vector3(-10f, 0f, -7.3f)`.
        Lamp(city, new Vector3(-10f, 0f, -7.3f));
        // Добавляет уличный фонарь через `Lamp` с аргументами `city, new Vector3(1.5f, 0f, -7.3f)`.
        Lamp(city, new Vector3(1.5f, 0f, -7.3f));
        // Добавляет уличный фонарь через `Lamp` с аргументами `city, new Vector3(13f, 0f, -7.3f)`.
        Lamp(city, new Vector3(13f, 0f, -7.3f));
        // Добавляет уличный фонарь через `Lamp` с аргументами `city, new Vector3(16.2f, 0f, 4f)`.
        Lamp(city, new Vector3(16.2f, 0f, 4f));

        // Размещает декоративный примитив на улице через `Prop` с аргументами `city, "Перекрёсток", PrimitiveType.Cube, new Vector3(18.5f, -0.035f, -10.4f), new Vector3(5.4f, 0.09f, 5.6f), asphalt`.
        Prop(city, "Перекрёсток", PrimitiveType.Cube, new Vector3(18.5f, -0.035f, -10.4f), new Vector3(5.4f, 0.09f, 5.6f), asphalt);
        // Ставит сегмент дороги и его основание через `RoadTile` с аргументами `city, "road-crossroad-line", new Vector3(18.5f, 0f, -10.4f), 0f, 5.5f`.
        RoadTile(city, "road-crossroad-line", new Vector3(18.5f, 0f, -10.4f), 0f, 5.5f);
        // Создаёт экземпляр модели и размещает его в иерархии через `TrafficLight.Spawn` с аргументами `city, new Vector3(15.7f, 0f, -13.05f), 0f, true`.
        TrafficLight.Spawn(city, new Vector3(15.7f, 0f, -13.05f), 0f, true);
        // Создаёт экземпляр модели и размещает его в иерархии через `TrafficLight.Spawn` с аргументами `city, new Vector3(21.3f, 0f, -7.75f), 180f, true`.
        TrafficLight.Spawn(city, new Vector3(21.3f, 0f, -7.75f), 180f, true);
        // Создаёт экземпляр модели и размещает его в иерархии через `TrafficLight.Spawn` с аргументами `city, new Vector3(15.7f, 0f, -7.75f), 90f, false`.
        TrafficLight.Spawn(city, new Vector3(15.7f, 0f, -7.75f), 90f, false);
        // Создаёт экземпляр модели и размещает его в иерархии через `TrafficLight.Spawn` с аргументами `city, new Vector3(21.3f, 0f, -13.05f), 270f, false`.
        TrafficLight.Spawn(city, new Vector3(21.3f, 0f, -13.05f), 270f, false);
        // Задаёт начальные элементы массива `loop`.
        var loop = new[]
        {  // Начало списка элементов массива.
            // Создаёт трёхмерный вектор с параметрами `-18f, 0f, -11.55f`.
            new Vector3(-18f, 0f, -11.55f),
            // Создаёт трёхмерный вектор с параметрами `16.1f, 0f, -11.55f`.
            new Vector3(16.1f, 0f, -11.55f),
            // Создаёт трёхмерный вектор с параметрами `19.65f, 0f, -11.55f`.
            new Vector3(19.65f, 0f, -11.55f),
            // Создаёт трёхмерный вектор с параметрами `19.65f, 0f, -8.2f`.
            new Vector3(19.65f, 0f, -8.2f),
            // Создаёт трёхмерный вектор с параметрами `19.65f, 0f, 16f`.
            new Vector3(19.65f, 0f, 16f),
            // Создаёт трёхмерный вектор с параметрами `17.35f, 0f, 16f`.
            new Vector3(17.35f, 0f, 16f),
            // Создаёт трёхмерный вектор с параметрами `17.35f, 0f, -8.2f`.
            new Vector3(17.35f, 0f, -8.2f),
            // Создаёт трёхмерный вектор с параметрами `17.35f, 0f, -9.25f`.
            new Vector3(17.35f, 0f, -9.25f),
            // Создаёт трёхмерный вектор с параметрами `16.1f, 0f, -9.25f`.
            new Vector3(16.1f, 0f, -9.25f),
            // Создаёт трёхмерный вектор с параметрами `-18f, 0f, -9.25f`.
            new Vector3(-18f, 0f, -9.25f)
        };  // Конец списка элементов массива.
        // Создаёт машину с замкнутым маршрутом через заданные точки через `Circuit` с аргументами `city, "sedan", loop, 7.6f, 0f`.
        Circuit(city, "sedan", loop, 7.6f, 0f);
        // Создаёт машину с замкнутым маршрутом через заданные точки через `Circuit` с аргументами `city, "taxi", loop, 7.6f, 0.33f`.
        Circuit(city, "taxi", loop, 7.6f, 0.33f);
        // Создаёт машину с замкнутым маршрутом через заданные точки через `Circuit` с аргументами `city, "hatchback-sports", loop, 7.6f, 0.66f`.
        Circuit(city, "hatchback-sports", loop, 7.6f, 0.66f);
        // Ставит припаркованную машину через `Parked` с аргументами `city, new Vector3(-14.5f, 0f, -13.35f), 90f, "suv"`.
        Parked(city, new Vector3(-14.5f, 0f, -13.35f), 90f, "suv");
        // Ставит припаркованную машину через `Parked` с аргументами `city, new Vector3(4.2f, 0f, -13.35f), 90f, "police"`.
        Parked(city, new Vector3(4.2f, 0f, -13.35f), 90f, "police");
        // Ставит припаркованную машину через `Parked` с аргументами `city, new Vector3(14.85f, 0f, -1.6f), 0f, "delivery"`.
        Parked(city, new Vector3(14.85f, 0f, -1.6f), 0f, "delivery");
    }  // Конец метода `Build`.

    // Ставит сегмент дороги и его основание.
    static void RoadTile(Transform parent, string name, Vector3 place, float yaw, float span)
    {  // Начало метода `RoadTile`.
        // Загружает ресурс через `Resources.Load<GameObject>` с аргументами `"Roads/" + name` и записывает результат в `prefab`.
        var prefab = Resources.Load<GameObject>("Roads/" + name);
        // Проверяет условие `prefab == null`.
        if (prefab == null)
            // Завершает выполнение текущего метода.
            return;
        // Создаёт копию объекта через `Object.Instantiate` с аргументами `prefab, parent` и записывает результат в `go`.
        var go = Object.Instantiate(prefab, parent);
        // Задаёт имя `go.name` значением `name`.
        go.name = name;
        // Задаёт мировая позиция `go.transform.position` значением `place`.
        go.transform.position = place;
        // Создаёт поворот из углов Эйлера через `Quaternion.Euler` с аргументами `0f, yaw, 0f` и записывает результат в `go.transform.rotation`.
        go.transform.rotation = Quaternion.Euler(0f, yaw, 0f);
        // Загружает ресурс через `Resources.Load<Texture2D>` с аргументами `"Roads/colormap"` и записывает результат в `map`.
        var map = Resources.Load<Texture2D>("Roads/colormap");
        // Проверяет условие `map == null`.
        if (map == null)
            // Загружает ресурс через `Resources.Load<Texture2D>` с аргументами `"Kenney/colormap"` и записывает результат в `map`.
            map = Resources.Load<Texture2D>("Kenney/colormap");
        // Проверяет условие `map != null`.
        if (map != null)
        {  // Начало ветки условия.
            // Задаёт режим выборки за границами текстуры `map.wrapMode` значением `TextureWrapMode.Clamp`.
            map.wrapMode = TextureWrapMode.Clamp;
            // Задаёт фильтрация текстуры `map.filterMode` значением `FilterMode.Point`.
            map.filterMode = FilterMode.Point;
        }  // Конец ветки условия.
        // Вызывает `TextureFactory.Lit` с аргументами `Color.white, map, 0.12f` и записывает результат в `skin`.
        var skin = TextureFactory.Lit(Color.white, map, 0.12f);
        // Собирает компоненты в объекте и его потомках через `go.GetComponentsInChildren<Renderer>` и записывает результат в `renderers`.
        var renderers = go.GetComponentsInChildren<Renderer>();
        // Перебирает индексы начиная с `int i = 0` при условии `i < renderers.Length`.
        for (int i = 0; i < renderers.Length; i++)
            // Задаёт общий материал `renderers[i].sharedMaterial` значением `skin`.
            renderers[i].sharedMaterial = skin;
        // Собирает компоненты в объекте и его потомках через `go.GetComponentsInChildren<Collider>` и записывает результат в `colliders`.
        var colliders = go.GetComponentsInChildren<Collider>();
        // Перебирает индексы начиная с `int i = 0` при условии `i < colliders.Length`.
        for (int i = 0; i < colliders.Length; i++)
            // Планирует удаление объекта через `Object.Destroy` с аргументами `colliders[i]`.
            Object.Destroy(colliders[i]);
        // Вызывает `KenneyKit.FitFootprint` с аргументами `go.transform, span, span, 0.35f`.
        KenneyKit.FitFootprint(go.transform, span, span, 0.35f);
        // Записывает в `go.transform.position` трёхмерный вектор с параметрами `place.x, 0.01f, place.z`.
        go.transform.position = new Vector3(place.x, 0.01f, place.z);
    }  // Конец метода `RoadTile`.

    // Добавляет окружающую землю вне основного двора.
    static void Earth(Transform parent)
    {  // Начало метода `Earth`.
        // Вызывает `TextureFactory.Lit` с аргументами `new Color(0.48f, 0.56f, 0.34f), TextureFactory.Grass(), 0.08f` и записывает результат в `earth`.
        var earth = TextureFactory.Lit(new Color(0.48f, 0.56f, 0.34f), TextureFactory.Grass(), 0.08f);
        // Вызывает `earth.SetTextureScale` с аргументами `"_BaseMap", new Vector2(70f, 70f)`.
        earth.SetTextureScale("_BaseMap", new Vector2(70f, 70f));
        // Размещает декоративный примитив на улице через `Prop` с аргументами `parent, "Земля юг", PrimitiveType.Cube, new Vector3(1.5f, -0.16f, -48.1f), new Vector3(200f, 0.24f, 83.8f), earth`.
        Prop(parent, "Земля юг", PrimitiveType.Cube, new Vector3(1.5f, -0.16f, -48.1f), new Vector3(200f, 0.24f, 83.8f), earth);
        // Размещает декоративный примитив на улице через `Prop` с аргументами `parent, "Земля север", PrimitiveType.Cube, new Vector3(1.5f, -0.16f, 52f), new Vector3(200f, 0.24f, 76f), earth`.
        Prop(parent, "Земля север", PrimitiveType.Cube, new Vector3(1.5f, -0.16f, 52f), new Vector3(200f, 0.24f, 76f), earth);
        // Размещает декоративный примитив на улице через `Prop` с аргументами `parent, "Земля запад", PrimitiveType.Cube, new Vector3(-50f, -0.16f, 3.9f), new Vector3(80f, 0.24f, 20.2f), earth`.
        Prop(parent, "Земля запад", PrimitiveType.Cube, new Vector3(-50f, -0.16f, 3.9f), new Vector3(80f, 0.24f, 20.2f), earth);
        // Размещает декоративный примитив на улице через `Prop` с аргументами `parent, "Земля восток", PrimitiveType.Cube, new Vector3(51.5f, -0.16f, 3.9f), new Vector3(77f, 0.24f, 20.2f), earth`.
        Prop(parent, "Земля восток", PrimitiveType.Cube, new Vector3(51.5f, -0.16f, 3.9f), new Vector3(77f, 0.24f, 20.2f), earth);
    }  // Конец метода `Earth`.

    // Размещает уличный дом, используя импортированную модель или примитивы.
    static void House(Transform parent, Vector3 place, float yaw, string kenney, Material wall, Material roof, Material brick, Material glass)
    {  // Начало метода `House`.
        // Создаёт экземпляр модели и размещает его в иерархии через `KenneyKit.Spawn` с аргументами `kenney, parent, place, new Vector3(0f, yaw, 0f), 1f` и записывает результат в `model`.
        var model = KenneyKit.Spawn(kenney, parent, place, new Vector3(0f, yaw, 0f), 1f);
        // Проверяет условие `model != null`.
        if (model != null)
        {  // Начало ветки условия.
            // Вызывает `KenneyKit.FitFootprint` с аргументами `model, 7.4f, 6.2f, 7.5f`.
            KenneyKit.FitFootprint(model, 7.4f, 6.2f, 7.5f);
            // Вызывает `KenneyKit.Block` с аргументами `model`.
            KenneyKit.Block(model);
            // Завершает выполнение текущего метода.
            return;
        }  // Конец ветки условия.

        // Вычисляет `height` как `6.8` плюс (берёт модуль числа через `Mathf.Abs` с аргументами `place.x` по модулю `1.4`).
        float height = 6.8f + (Mathf.Abs(place.x) % 1.4f);
        // Вычисляет `width` как `6.2` плюс (берёт модуль числа через `Mathf.Abs` с аргументами `place.x` по модулю `1.7`).
        float width = 6.2f + (Mathf.Abs(place.x) % 1.7f);
        // Вычисляет `depth` как `5.4` плюс (берёт модуль числа через `Mathf.Abs` с аргументами `place.z` по модулю `1.1`).
        float depth = 5.4f + (Mathf.Abs(place.z) % 1.1f);
        // Задаёт `root` значением `new GameObject("Дом").transform`.
        var root = new GameObject("Дом").transform;
        // Привязывает к родителю через `root.SetParent` с аргументами `parent, false`.
        root.SetParent(parent, false);
        // Задаёт мировая позиция `root.position` значением `place`.
        root.position = place;
        // Создаёт поворот из углов Эйлера через `Quaternion.Euler` с аргументами `0f, yaw, 0f` и записывает результат в `root.rotation`.
        root.rotation = Quaternion.Euler(0f, yaw, 0f);
        // Строит примитив нужного типа с материалом и локальным преобразованием через `Mesh` с аргументами `root, "Стены", PrimitiveType.Cube, new Vector3(0f, height * 0.5f, 0f), new Vector3(width, height, depth), wall`.
        Mesh(root, "Стены", PrimitiveType.Cube, new Vector3(0f, height * 0.5f, 0f), new Vector3(width, height, depth), wall);
        // Строит примитив нужного типа с материалом и локальным преобразованием через `Mesh` с аргументами `root, "Крыша", PrimitiveType.Cube, new Vector3(0f, height + 0.15f, 0.55f), new Vector3(width + 0.5f, 0.16f, depth * 0.72f), roof` и записывает результат в `a`.
        var a = Mesh(root, "Крыша", PrimitiveType.Cube, new Vector3(0f, height + 0.15f, 0.55f), new Vector3(width + 0.5f, 0.16f, depth * 0.72f), roof);
        // Создаёт поворот из углов Эйлера через `Quaternion.Euler` с аргументами `28f, 0f, 0f` и записывает результат в `a.localRotation`.
        a.localRotation = Quaternion.Euler(28f, 0f, 0f);
        // Строит примитив нужного типа с материалом и локальным преобразованием через `Mesh` с аргументами `root, "Крыша", PrimitiveType.Cube, new Vector3(0f, height + 0.15f, -0.55f), new Vector3(width + 0.5f, 0.16f, depth * 0.72f), roof` и записывает результат в `b`.
        var b = Mesh(root, "Крыша", PrimitiveType.Cube, new Vector3(0f, height + 0.15f, -0.55f), new Vector3(width + 0.5f, 0.16f, depth * 0.72f), roof);
        // Создаёт поворот из углов Эйлера через `Quaternion.Euler` с аргументами `-28f, 0f, 0f` и записывает результат в `b.localRotation`.
        b.localRotation = Quaternion.Euler(-28f, 0f, 0f);
        // Строит примитив нужного типа с материалом и локальным преобразованием через `Mesh` с аргументами `root, "Труба", PrimitiveType.Cube, new Vector3(width * 0.28f, height + 1.15f, -0.4f), new Vector3(0.5f, 1.3f, 0.5f), brick`.
        Mesh(root, "Труба", PrimitiveType.Cube, new Vector3(width * 0.28f, height + 1.15f, -0.4f), new Vector3(0.5f, 1.3f, 0.5f), brick);
        // Строит примитив нужного типа с материалом и локальным преобразованием через `Mesh` с аргументами `root, "Дверь", PrimitiveType.Cube, new Vector3(0f, 1.1f, depth * 0.5f + 0.04f), new Vector3(1.05f, 2.2f, 0.08f), TextureFactory.Lit(Palette.Door, TextureFactor…`.
        Mesh(root, "Дверь", PrimitiveType.Cube, new Vector3(0f, 1.1f, depth * 0.5f + 0.04f), new Vector3(1.05f, 2.2f, 0.08f), TextureFactory.Lit(Palette.Door, TextureFactory.Wood()));
        // Строит примитив нужного типа с материалом и локальным преобразованием через `Mesh` с аргументами `root, "Окно", PrimitiveType.Cube, new Vector3(-width * 0.28f, 2.3f, depth * 0.5f + 0.05f), new Vector3(1.15f, 1.15f, 0.06f), glass`.
        Mesh(root, "Окно", PrimitiveType.Cube, new Vector3(-width * 0.28f, 2.3f, depth * 0.5f + 0.05f), new Vector3(1.15f, 1.15f, 0.06f), glass);
        // Строит примитив нужного типа с материалом и локальным преобразованием через `Mesh` с аргументами `root, "Окно", PrimitiveType.Cube, new Vector3(width * 0.28f, 2.3f, depth * 0.5f + 0.05f), new Vector3(1.15f, 1.15f, 0.06f), glass`.
        Mesh(root, "Окно", PrimitiveType.Cube, new Vector3(width * 0.28f, 2.3f, depth * 0.5f + 0.05f), new Vector3(1.15f, 1.15f, 0.06f), glass);
        // Строит примитив нужного типа с материалом и локальным преобразованием через `Mesh` с аргументами `root, "Крыльцо", PrimitiveType.Cube, new Vector3(0f, 0.12f, depth * 0.5f + 0.45f), new Vector3(2.2f, 0.24f, 0.9f), brick`.
        Mesh(root, "Крыльцо", PrimitiveType.Cube, new Vector3(0f, 0.12f, depth * 0.5f + 0.45f), new Vector3(2.2f, 0.24f, 0.9f), brick);
    }  // Конец метода `House`.

    // Ставит припаркованную машину.
    static void Parked(Transform parent, Vector3 place, float yaw, string model)
    {  // Начало метода `Parked`.
        // Создаёт экземпляр модели и размещает его в иерархии через `CarKit.Spawn` с аргументами `model, parent, place, new Vector3(0f, yaw, 0f), 1.28f` и записывает результат в `car`.
        var car = CarKit.Spawn(model, parent, place, new Vector3(0f, yaw, 0f), 1.28f);
        // Проверяет условие `car != null`.
        if (car != null)
        {  // Начало ветки условия.
            // Вызывает `KenneyKit.Block` с аргументами `car`.
            KenneyKit.Block(car);
            // Завершает выполнение текущего метода.
            return;
        }  // Конец ветки условия.
        // Задаёт `root` значением `new GameObject("Стоянка").transform`.
        var root = new GameObject("Стоянка").transform;
        // Привязывает к родителю через `root.SetParent` с аргументами `parent, false`.
        root.SetParent(parent, false);
        // Задаёт мировая позиция `root.position` значением `place`.
        root.position = place;
        // Создаёт поворот из углов Эйлера через `Quaternion.Euler` с аргументами `0f, yaw, 0f` и записывает результат в `root.rotation`.
        root.rotation = Quaternion.Euler(0f, yaw, 0f);
        // Строит примитив нужного типа с материалом и локальным преобразованием через `Mesh` с аргументами `root, "Кузов", PrimitiveType.Cube, new Vector3(0f, 0.55f, 0f), new Vector3(1.7f, 0.55f, 3.4f), TextureFactory.Lit(Palette.CarBlue, null, 0.45f)`.
        Mesh(root, "Кузов", PrimitiveType.Cube, new Vector3(0f, 0.55f, 0f), new Vector3(1.7f, 0.55f, 3.4f), TextureFactory.Lit(Palette.CarBlue, null, 0.45f));
        // Строит примитив нужного типа с материалом и локальным преобразованием через `Mesh` с аргументами `root, "Кабина", PrimitiveType.Cube, new Vector3(0f, 1.05f, -0.15f), new Vector3(1.5f, 0.5f, 1.7f), TextureFactory.Lit(Palette.CarBlue * 0.85f, null, 0.4f)`.
        Mesh(root, "Кабина", PrimitiveType.Cube, new Vector3(0f, 1.05f, -0.15f), new Vector3(1.5f, 0.5f, 1.7f), TextureFactory.Lit(Palette.CarBlue * 0.85f, null, 0.4f));
        // Создаёт колесо для запасной модели автомобиля через `Wheel` с аргументами `root, new Vector3(-0.78f, 0.28f, 1.05f)`.
        Wheel(root, new Vector3(-0.78f, 0.28f, 1.05f));
        // Создаёт колесо для запасной модели автомобиля через `Wheel` с аргументами `root, new Vector3(0.78f, 0.28f, 1.05f)`.
        Wheel(root, new Vector3(0.78f, 0.28f, 1.05f));
        // Создаёт колесо для запасной модели автомобиля через `Wheel` с аргументами `root, new Vector3(-0.78f, 0.28f, -1.05f)`.
        Wheel(root, new Vector3(-0.78f, 0.28f, -1.05f));
        // Создаёт колесо для запасной модели автомобиля через `Wheel` с аргументами `root, new Vector3(0.78f, 0.28f, -1.05f)`.
        Wheel(root, new Vector3(0.78f, 0.28f, -1.05f));
    }  // Конец метода `Parked`.

    // Создаёт машину с замкнутым маршрутом через заданные точки.
    static void Circuit(Transform parent, string model, Vector3[] points, float speed, float offset)
    {  // Начало метода `Circuit`.
        // Проверяет условие `points == null || points.Length < 2`.
        if (points == null || points.Length < 2)
            // Завершает выполнение текущего метода.
            return;
        // Создаёт экземпляр модели и размещает его в иерархии через `CarKit.Spawn` с аргументами `model, parent, points[0], Vector3.zero, 1.32f` и записывает результат в `car`.
        var car = CarKit.Spawn(model, parent, points[0], Vector3.zero, 1.32f);
        // Проверяет условие `car == null`.
        if (car == null)
            // Завершает выполнение текущего метода.
            return;
        // Задаёт `y` значением `car.position.y`.
        float y = car.position.y;
        // Задаёт `path` массивом `new Vector3[points.Length]`.
        var path = new Vector3[points.Length];
        // Перебирает индексы начиная с `int i = 0` при условии `i < points.Length`.
        for (int i = 0; i < points.Length; i++)
            // Записывает в `path[i]` трёхмерный вектор с параметрами `points[i].x, y, points[i].z`.
            path[i] = new Vector3(points[i].x, y, points[i].z);
        // Добавляет компонент через `car.gameObject.AddComponent<CarMover>` и записывает результат в `mover`.
        var mover = car.gameObject.AddComponent<CarMover>();
        // Задаёт `mover.Points` значением `path`.
        mover.Points = path;
        // Задаёт `mover.Speed` значением `speed`.
        mover.Speed = speed;
        // Задаёт `mover.Offset` значением `offset`.
        mover.Offset = offset;
    }  // Конец метода `Circuit`.

    // Создаёт машину с прямым маршрутом между двумя точками.
    static void Drive(Transform parent, string model, Vector3 from, Vector3 to, float speed, float offset)
    {  // Начало метода `Drive`.
        // Создаёт экземпляр модели и размещает его в иерархии через `CarKit.Spawn` с аргументами `model, parent, from, Vector3.zero, 1.32f` и записывает результат в `car`.
        var car = CarKit.Spawn(model, parent, from, Vector3.zero, 1.32f);
        // Проверяет условие `car == null`.
        if (car == null)
        {  // Начало ветки условия.
            // Собирает запасную модель машины и задаёт ей движение через `Car` с аргументами `parent, from, to, Palette.CarRed, speed`.
            Car(parent, from, to, Palette.CarRed, speed);
            // Завершает выполнение текущего метода.
            return;
        }  // Конец ветки условия.
        // Добавляет компонент через `car.gameObject.AddComponent<CarMover>` и записывает результат в `mover`.
        var mover = car.gameObject.AddComponent<CarMover>();
        // Задаёт `y` значением `car.position.y`.
        float y = car.position.y;
        // Записывает в `mover.From` трёхмерный вектор с параметрами `from.x, y, from.z`.
        mover.From = new Vector3(from.x, y, from.z);
        // Записывает в `mover.To` трёхмерный вектор с параметрами `to.x, y, to.z`.
        mover.To = new Vector3(to.x, y, to.z);
        // Задаёт `mover.Speed` значением `speed`.
        mover.Speed = speed;
        // Задаёт `mover.Offset` значением `offset`.
        mover.Offset = offset;
    }  // Конец метода `Drive`.

    // Размещает дерево вдоль улицы.
    static void Tree(Transform parent, Vector3 place, Material bark, Material leaf)
    {  // Начало метода `Tree`.
        // Вычисляет `large` как берёт модуль числа через `Mathf.Abs` с аргументами `place.x` по модулю `2` больше `1`.
        bool large = Mathf.Abs(place.x) % 2f > 1f;
        // Создаёт экземпляр модели и размещает его в иерархии через `KenneyKit.Spawn` с аргументами `large ? "tree-large" : "tree-small", parent, place, Vector3.zero, 1f` и записывает результат в `kenney`.
        var kenney = KenneyKit.Spawn(large ? "tree-large" : "tree-small", parent, place, Vector3.zero, 1f);
        // Проверяет условие `kenney != null`.
        if (kenney != null)
        {  // Начало ветки условия.
            // Вызывает `KenneyKit.FitHeight` с аргументами `kenney, large ? 5.6f : 4.4f`.
            KenneyKit.FitHeight(kenney, large ? 5.6f : 4.4f);
            // Вызывает `KenneyKit.TrunkBlock` с аргументами `kenney`.
            KenneyKit.TrunkBlock(kenney);
            // Завершает выполнение текущего метода.
            return;
        }  // Конец ветки условия.

        // Задаёт `root` значением `new GameObject("Дерево").transform`.
        var root = new GameObject("Дерево").transform;
        // Привязывает к родителю через `root.SetParent` с аргументами `parent, false`.
        root.SetParent(parent, false);
        // Задаёт мировая позиция `root.position` значением `place`.
        root.position = place;
        // Строит примитив нужного типа с материалом и локальным преобразованием через `Mesh` с аргументами `root, "Ствол", PrimitiveType.Cylinder, new Vector3(0f, 1.15f, 0f), new Vector3(0.42f, 1.15f, 0.42f), bark` и записывает результат в `trunk`.
        var trunk = Mesh(root, "Ствол", PrimitiveType.Cylinder, new Vector3(0f, 1.15f, 0f), new Vector3(0.42f, 1.15f, 0.42f), bark);
        // Планирует удаление объекта через `Object.Destroy` с аргументами `trunk.GetComponent<Collider>()`.
        Object.Destroy(trunk.GetComponent<Collider>());
        // Строит примитив нужного типа с материалом и локальным преобразованием через `Mesh` с аргументами `root, "Крона", PrimitiveType.Sphere, new Vector3(0f, 2.7f, 0f), Vector3.one * 2.3f, leaf`.
        Mesh(root, "Крона", PrimitiveType.Sphere, new Vector3(0f, 2.7f, 0f), Vector3.one * 2.3f, leaf);
        // Строит примитив нужного типа с материалом и локальным преобразованием через `Mesh` с аргументами `root, "Крона", PrimitiveType.Sphere, new Vector3(0.55f, 3.15f, -0.2f), Vector3.one * 1.7f, leaf`.
        Mesh(root, "Крона", PrimitiveType.Sphere, new Vector3(0.55f, 3.15f, -0.2f), Vector3.one * 1.7f, leaf);
        // Строит примитив нужного типа с материалом и локальным преобразованием через `Mesh` с аргументами `root, "Крона", PrimitiveType.Sphere, new Vector3(-0.45f, 3.35f, 0.35f), Vector3.one * 1.5f, leaf`.
        Mesh(root, "Крона", PrimitiveType.Sphere, new Vector3(-0.45f, 3.35f, 0.35f), Vector3.one * 1.5f, leaf);
    }  // Конец метода `Tree`.

    // Добавляет уличный фонарь.
    static void Lamp(Transform parent, Vector3 place)
    {  // Начало метода `Lamp`.
        // Задаёт `root` значением `new GameObject("Фонарь").transform`.
        var root = new GameObject("Фонарь").transform;
        // Привязывает к родителю через `root.SetParent` с аргументами `parent, false`.
        root.SetParent(parent, false);
        // Задаёт мировая позиция `root.position` значением `place`.
        root.position = place;
        // Строит примитив нужного типа с материалом и локальным преобразованием через `Mesh` с аргументами `root, "Столб", PrimitiveType.Cylinder, new Vector3(0f, 1.6f, 0f), new Vector3(0.12f, 1.6f, 0.12f), TextureFactory.Lit(Palette.Iron)`.
        Mesh(root, "Столб", PrimitiveType.Cylinder, new Vector3(0f, 1.6f, 0f), new Vector3(0.12f, 1.6f, 0.12f), TextureFactory.Lit(Palette.Iron));
        // Строит примитив нужного типа с материалом и локальным преобразованием через `Mesh` с аргументами `root, "Плафон", PrimitiveType.Cube, new Vector3(0f, 3.25f, 0f), new Vector3(0.35f, 0.28f, 0.35f), TextureFactory.Lit(Palette.Lamp, null, 0.6f)`.
        Mesh(root, "Плафон", PrimitiveType.Cube, new Vector3(0f, 3.25f, 0f), new Vector3(0.35f, 0.28f, 0.35f), TextureFactory.Lit(Palette.Lamp, null, 0.6f));
    }  // Конец метода `Lamp`.

    // Собирает запасную модель машины и задаёт ей движение.
    static void Car(Transform parent, Vector3 from, Vector3 to, Color color, float speed)
    {  // Начало метода `Car`.
        // Задаёт `root` значением `new GameObject("Машина").transform`.
        var root = new GameObject("Машина").transform;
        // Привязывает к родителю через `root.SetParent` с аргументами `parent, false`.
        root.SetParent(parent, false);
        // Задаёт мировая позиция `root.position` значением `from`.
        root.position = from;
        // Строит примитив нужного типа с материалом и локальным преобразованием через `Mesh` с аргументами `root, "Кузов", PrimitiveType.Cube, new Vector3(0f, 0.55f, 0f), new Vector3(1.7f, 0.55f, 3.4f), TextureFactory.Lit(color, null, 0.45f)`.
        Mesh(root, "Кузов", PrimitiveType.Cube, new Vector3(0f, 0.55f, 0f), new Vector3(1.7f, 0.55f, 3.4f), TextureFactory.Lit(color, null, 0.45f));
        // Строит примитив нужного типа с материалом и локальным преобразованием через `Mesh` с аргументами `root, "Кабина", PrimitiveType.Cube, new Vector3(0f, 1.05f, -0.15f), new Vector3(1.5f, 0.5f, 1.7f), TextureFactory.Lit(color * 0.85f, null, 0.4f)`.
        Mesh(root, "Кабина", PrimitiveType.Cube, new Vector3(0f, 1.05f, -0.15f), new Vector3(1.5f, 0.5f, 1.7f), TextureFactory.Lit(color * 0.85f, null, 0.4f));
        // Строит примитив нужного типа с материалом и локальным преобразованием через `Mesh` с аргументами `root, "Стекло", PrimitiveType.Cube, new Vector3(0f, 1.08f, 0.72f), new Vector3(1.35f, 0.38f, 0.08f), TextureFactory.Lit(Palette.Window, null, 0.8f)`.
        Mesh(root, "Стекло", PrimitiveType.Cube, new Vector3(0f, 1.08f, 0.72f), new Vector3(1.35f, 0.38f, 0.08f), TextureFactory.Lit(Palette.Window, null, 0.8f));
        // Создаёт колесо для запасной модели автомобиля через `Wheel` с аргументами `root, new Vector3(-0.78f, 0.28f, 1.05f)`.
        Wheel(root, new Vector3(-0.78f, 0.28f, 1.05f));
        // Создаёт колесо для запасной модели автомобиля через `Wheel` с аргументами `root, new Vector3(0.78f, 0.28f, 1.05f)`.
        Wheel(root, new Vector3(0.78f, 0.28f, 1.05f));
        // Создаёт колесо для запасной модели автомобиля через `Wheel` с аргументами `root, new Vector3(-0.78f, 0.28f, -1.05f)`.
        Wheel(root, new Vector3(-0.78f, 0.28f, -1.05f));
        // Создаёт колесо для запасной модели автомобиля через `Wheel` с аргументами `root, new Vector3(0.78f, 0.28f, -1.05f)`.
        Wheel(root, new Vector3(0.78f, 0.28f, -1.05f));
        // Добавляет компонент через `root.gameObject.AddComponent<CarMover>` и записывает результат в `mover`.
        var mover = root.gameObject.AddComponent<CarMover>();
        // Задаёт `mover.From` значением `from` плюс `Vector3.up` умножить на `0.02`.
        mover.From = from + Vector3.up * 0.02f;
        // Задаёт `mover.To` значением `to` плюс `Vector3.up` умножить на `0.02`.
        mover.To = to + Vector3.up * 0.02f;
        // Задаёт `mover.Speed` значением `speed`.
        mover.Speed = speed;
        // Задаёт `mover.Offset` значением `0`.
        mover.Offset = 0f;
    }  // Конец метода `Car`.

    // Создаёт колесо для запасной модели автомобиля.
    static void Wheel(Transform parent, Vector3 local)
    {  // Начало метода `Wheel`.
        // Строит примитив нужного типа с материалом и локальным преобразованием через `Mesh` с аргументами `parent, "Колесо", PrimitiveType.Cylinder, local, new Vector3(0.42f, 0.12f, 0.42f), TextureFactory.Lit(Palette.Ink)` и записывает результат в `wheel`.
        var wheel = Mesh(parent, "Колесо", PrimitiveType.Cylinder, local, new Vector3(0.42f, 0.12f, 0.42f), TextureFactory.Lit(Palette.Ink));
        // Создаёт поворот из углов Эйлера через `Quaternion.Euler` с аргументами `0f, 0f, 90f` и записывает результат в `wheel.localRotation`.
        wheel.localRotation = Quaternion.Euler(0f, 0f, 90f);
        // Планирует удаление объекта через `Object.Destroy` с аргументами `wheel.GetComponent<Collider>()`.
        Object.Destroy(wheel.GetComponent<Collider>());
    }  // Конец метода `Wheel`.

    // Строит примитив нужного типа с материалом и локальным преобразованием.
    static Transform Mesh(Transform parent, string name, PrimitiveType type, Vector3 local, Vector3 scale, Material material)
    {  // Начало метода `Mesh`.
        // Создаёт стандартный примитив через `GameObject.CreatePrimitive` с аргументами `type` и записывает результат в `go`.
        var go = GameObject.CreatePrimitive(type);
        // Задаёт имя `go.name` значением `name`.
        go.name = name;
        // Привязывает к родителю через `go.transform.SetParent` с аргументами `parent, false`.
        go.transform.SetParent(parent, false);
        // Задаёт локальная позиция `go.transform.localPosition` значением `local`.
        go.transform.localPosition = local;
        // Задаёт локальный масштаб `go.transform.localScale` значением `scale`.
        go.transform.localScale = scale;
        // Ищет компонент через `go.GetComponent<MeshRenderer>` и записывает результат в `renderer`.
        var renderer = go.GetComponent<MeshRenderer>();
        // Задаёт общий материал `renderer.sharedMaterial` значением `material`.
        renderer.sharedMaterial = material;
        // Ищет компонент через `go.GetComponent<Collider>` и записывает результат в `collider`.
        var collider = go.GetComponent<Collider>();
        // Проверяет условие `collider != null`.
        if (collider != null)
            // Планирует удаление объекта через `Object.Destroy` с аргументами `collider`.
            Object.Destroy(collider);
        // Возвращает `go.transform`.
        return go.transform;
    }  // Конец метода `Mesh`.

    // Размещает декоративный примитив на улице.
    static Transform Prop(Transform parent, string name, PrimitiveType type, Vector3 position, Vector3 scale, Material material)
    {  // Начало метода `Prop`.
        // Строит примитив нужного типа с материалом и локальным преобразованием через `Mesh` с аргументами `parent, name, type, Vector3.zero, scale, material` и записывает результат в `t`.
        var t = Mesh(parent, name, type, Vector3.zero, scale, material);
        // Задаёт мировая позиция `t.position` значением `position`.
        t.position = position;
        // Возвращает `t`.
        return t;
    }  // Конец метода `Prop`.
}  // Конец класса `StreetScene`.

// Подключает доступ к базовым типам игрового движка.
using UnityEngine;

// Запускает игровую сцену, создаёт раскоп, игрока, HUD и окружение.
public class YardBootstrap : MonoBehaviour
{  // Начало класса `YardBootstrap`.
    // Подготавливает двор, восстанавливает состояние мира и создаёт игрока.
    public void BuildWorld()
    {  // Начало метода `BuildWorld`.
        // Проверяет условие `transform.Find("Раскоп") != null`.
        if (transform.Find("Раскоп") != null)
            // Завершает выполнение текущего метода.
            return;

        // Задаёт `mainCam` значением главная камера сцены `Camera.main`.
        var mainCam = Camera.main;
        // Проверяет условие `mainCam != null && mainCam.transform.root != transform`.
        if (mainCam != null && mainCam.transform.root != transform)
            // Меняет активность объекта через `mainCam.gameObject.SetActive` с аргументами `false`.
            mainCam.gameObject.SetActive(false);

        // Ищет объект по имени через `transform.Find` с аргументами `"Двор"` и записывает результат в `yard`.
        var yard = transform.Find("Двор");
        // Проверяет условие `yard == null`.
        if (yard == null)
            // Вызывает `YardLayout.Build` с аргументами `transform` и записывает результат в `yard`.
            yard = YardLayout.Build(transform);
        // Вызывает `PropKit.FixShovelsUnder` с аргументами `yard`.
        PropKit.FixShovelsUnder(yard);
        // Вызывает `TrafficLight.RefreshScene` с аргументами `yard`.
        TrafficLight.RefreshScene(yard);
        // Убирает широкий старый триггер продажи у прилавка через `RemoveSellBoothTrigger` с аргументами `yard`.
        RemoveSellBoothTrigger(yard);
        // Ищет объект по имени через `yard.Find` с аргументами `"Магазин"` и записывает результат в `booth`.
        var booth = yard.Find("Магазин");
        // Проверяет условие `booth != null`.
        if (booth != null)
            // Вызывает `SellerPlacement.EnsureAnchor` с аргументами `booth`.
            SellerPlacement.EnsureAnchor(booth);
        // Корректирует коллайдеры вспомогательных точек магазина через `FixShopTriggers` с аргументами `yard`.
        FixShopTriggers(yard);
        // Нормализует вектор через `GaragePhysics.Normalize` с аргументами `yard`.
        GaragePhysics.Normalize(yard);
        // Вызывает `SmithyPhysics.Pin` с аргументами `yard`.
        SmithyPhysics.Pin(yard);
        // Проверяет условие `transform.GetComponent<SmithyPhysicsRefresh>() == null`.
        if (transform.GetComponent<SmithyPhysicsRefresh>() == null)
            // Добавляет компонент через `transform.gameObject.AddComponent<SmithyPhysicsRefresh>`.
            transform.gameObject.AddComponent<SmithyPhysicsRefresh>();
        // Перебирает элементы `yard.GetComponentsInChildren<Transform>(true)` по одному в `shop`.
        foreach (var shop in yard.GetComponentsInChildren<Transform>(true))
        {  // Начало цикла по элементам.
            // Проверяет условие `shop.name != "Магазин"`.
            if (shop.name != "Магазин")
                // Пропускает остаток этой итерации и переходит к следующей.
                continue;
            // Проверяет условие `shop.GetComponent<ShopShovelPreview>() == null`.
            if (shop.GetComponent<ShopShovelPreview>() == null)
                // Добавляет компонент через `shop.gameObject.AddComponent<ShopShovelPreview>`.
                shop.gameObject.AddComponent<ShopShovelPreview>();
            // Вызывает `ShovelTint.PaintShopRow` с аргументами `shop`.
            ShovelTint.PaintShopRow(shop);
        }  // Конец цикла по элементам.

        // Создаёт объект сцены с параметрами `"Раскоп"` и сохраняет ссылку в `siteGo`.
        var siteGo = new GameObject("Раскоп");
        // Привязывает к родителю через `siteGo.transform.SetParent` с аргументами `transform, false`.
        siteGo.transform.SetParent(transform, false);
        // Добавляет компонент через `siteGo.AddComponent<DigSite>` и записывает результат в `site`.
        var site = siteGo.AddComponent<DigSite>();

        // Проверяет условие `transform.Find("Звук") == null`.
        if (transform.Find("Звук") == null)
        {  // Начало ветки условия.
            // Создаёт объект сцены с параметрами `"Звук"` и сохраняет ссылку в `feedback`.
            var feedback = new GameObject("Звук");
            // Привязывает к родителю через `feedback.transform.SetParent` с аргументами `transform, false`.
            feedback.transform.SetParent(transform, false);
            // Добавляет компонент через `feedback.AddComponent<DigFeedback>`.
            feedback.AddComponent<DigFeedback>();
        }  // Конец ветки условия.

        // Выбирает значение для `saveData` по условию `PlaySession.Continue`.
        var saveData = PlaySession.Continue ? GameSave.Read() : null;
        // Проверяет условие `PlaySession.Continue && saveData != null`.
        if (PlaySession.Continue && saveData != null)
        {  // Начало ветки условия.
            // Ищет компонент через `GetComponent<GameLoop>` и записывает результат в `loop`.
            var loop = GetComponent<GameLoop>();
            // Проверяет условие `loop != null`.
            if (loop != null)
                // Вызывает `loop.ApplySaveFlags` с аргументами `saveData.treasureTaken`.
                loop.ApplySaveFlags(saveData.treasureTaken);
            // Вызывает `site.Prime` с аргументами `saveData.dug, saveData.treasureTaken, saveData.looted, saveData.treasureIx, saveData.treasureIz`.
            site.Prime(saveData.dug, saveData.treasureTaken, saveData.looted, saveData.treasureIx, saveData.treasureIz);
        }  // Конец ветки условия.
        // Обрабатывает случай, когда предыдущее условие ложно.
        else
        {  // Начало запасной ветки условия.
            // Вызывает `site.PickNewTreasureCorner`.
            site.PickNewTreasureCorner();
        }  // Конец запасной ветки условия.

        // Вызывает `site.Build`.
        site.Build();

        // Проверяет условие `yard.Find("Лопата") == null`.
        if (yard.Find("Лопата") == null)
            // Вызывает `YardLayout.CreateShovel` с аргументами `yard`.
            YardLayout.CreateShovel(yard);

        // Ищет объект по имени через `yard.Find` с аргументами `"Лопата"` и записывает результат в `shovel`.
        var shovel = yard.Find("Лопата");
        // Проверяет условие `shovel != null`.
        if (shovel != null)
        {  // Начало ветки условия.
            // Выбирает значение для `save` по условию `PlaySession.Continue`.
            var save = PlaySession.Continue ? GameSave.Read() : null;
            // Проверяет условие `save != null && save.hasShovel`.
            if (save != null && save.hasShovel)
                // Планирует удаление объекта через `Destroy` с аргументами `shovel.gameObject`.
                Destroy(shovel.gameObject);
        }  // Конец ветки условия.

        // Ищет компонент через `GetComponent<YardMarkers>` и записывает результат в `markers`.
        var markers = GetComponent<YardMarkers>();
        // Создаёт тело игрока, камеру, модель лопаты и игровой интерфейс через `SpawnPlayer` с аргументами `site, markers` и записывает результат в `player`.
        var player = SpawnPlayer(site, markers);
        // Проверяет условие `PlaySession.Continue`.
        if (PlaySession.Continue)
            // Вызывает `player.ApplyProgress` с аргументами `GameSave.Read()`.
            player.ApplyProgress(GameSave.Read());

        // Ищет компонент в объекте и его потомках через `yard.GetComponentInChildren<Seller>` и записывает результат в `seller`.
        var seller = yard.GetComponentInChildren<Seller>();
        // Проверяет условие `seller != null`.
        if (seller != null)
        {  // Начало ветки условия.
            // Проверяет условие `markers != null`.
            if (markers != null)
                // Вызывает `seller.Begin` с аргументами `markers.SellerNorthPoint, markers.SellerSouthPoint, yard`.
                seller.Begin(markers.SellerNorthPoint, markers.SellerSouthPoint, yard);
            // Обрабатывает случай, когда предыдущее условие ложно.
            else
                // Вызывает `seller.Begin` с аргументами `new Vector3(-2.2f, 0f, 4.35f), new Vector3(-6.5f, 0f, 3.1f), yard`.
                seller.Begin(new Vector3(-2.2f, 0f, 4.35f), new Vector3(-6.5f, 0f, 3.1f), yard);
        }  // Конец ветки условия.

        // Проверяет условие `yard.Find("Атмосфера") == null`.
        if (yard.Find("Атмосфера") == null)
            // Вызывает `AmbientBed.Attach` с аргументами `yard`.
            AmbientBed.Attach(yard);

        // Настраивает туман, окружающее освещение и фон камеры через `TuneLook`.
        TuneLook();
    }  // Конец метода `BuildWorld`.

    // Создаёт тело игрока, камеру, модель лопаты и игровой интерфейс.
    FirstPersonPlayer SpawnPlayer(DigSite site, YardMarkers markers)
    {  // Начало метода `SpawnPlayer`.
        // Выбирает значение для `pos` по условию `markers` существует.
        var pos = markers != null ? markers.PlayerPosition : new Vector3(5f, 0.15f, -3.2f);
        // Создаёт объект сцены с параметрами `"Игрок"` и сохраняет ссылку в `player`.
        var player = new GameObject("Игрок");
        // Задаёт мировая позиция `player.transform.position` значением `pos`.
        player.transform.position = pos;

        // Задаёт `view` значением `new GameObject("View").transform`.
        var view = new GameObject("View").transform;
        // Привязывает к родителю через `view.SetParent` с аргументами `player.transform, false`.
        view.SetParent(player.transform, false);
        // Записывает в `view.localPosition` трёхмерный вектор с параметрами `0f, 1.64f, 0f`.
        view.localPosition = new Vector3(0f, 1.64f, 0f);

        // Задаёт `held` значением `new GameObject("ShovelView").transform`.
        var held = new GameObject("ShovelView").transform;
        // Привязывает к родителю через `held.SetParent` с аргументами `view, false`.
        held.SetParent(view, false);
        // Записывает в `held.localPosition` трёхмерный вектор с параметрами `0.5f, -0.34f, 0.5f`.
        held.localPosition = new Vector3(0.5f, -0.34f, 0.5f);
        // Создаёт поворот из углов Эйлера через `Quaternion.Euler` с аргументами `18f, 74f, 16f` и записывает результат в `held.localRotation`.
        held.localRotation = Quaternion.Euler(18f, 74f, 16f);
        // Создаёт экземпляр модели и размещает его в иерархии через `PropKit.Spawn` с аргументами `"shovel", held, Vector3.zero, new Vector3(180f, 270f, 0f), 0.62f, false, false` и записывает результат в `heldModel`.
        var heldModel = PropKit.Spawn("shovel", held, Vector3.zero, new Vector3(180f, 270f, 0f), 0.62f, false, false);
        // Проверяет условие `heldModel != null`.
        if (heldModel != null)
        {  // Начало ветки условия.
            // Вызывает `KenneyKit.BoundsOf` с аргументами `heldModel` и записывает результат в `bounds`.
            var bounds = KenneyKit.BoundsOf(heldModel);
            // Создаёт трёхмерный вектор с параметрами `bounds.center.x, bounds.min.y, bounds.center.z` и сохраняет ссылку в `grip`.
            Vector3 grip = new Vector3(bounds.center.x, bounds.min.y, bounds.center.z);
            // Добавляет `held.position - grip` к текущему значению мировая позиция `heldModel.position`.
            heldModel.position += held.position - grip;
        }  // Конец ветки условия.
        // Обрабатывает случай, когда предыдущее условие ложно.
        else
        {  // Начало запасной ветки условия.
            // Удаляет коллайдер с переданного объекта через `StripCollider` с аргументами `Box(held, "Черенок", new Vector3(0f, 0.32f, 0f), new Vector3(0.05f, 0.55f, 0.05f), Mat(Palette.Handle)).gameObject`.
            StripCollider(Box(held, "Черенок", new Vector3(0f, 0.32f, 0f), new Vector3(0.05f, 0.55f, 0.05f), Mat(Palette.Handle)).gameObject);
            // Удаляет коллайдер с переданного объекта через `StripCollider` с аргументами `Box(held, "Полотно", new Vector3(0f, 0.68f, 0.04f), new Vector3(0.16f, 0.2f, 0.03f), Mat(Palette.Blade)).gameObject`.
            StripCollider(Box(held, "Полотно", new Vector3(0f, 0.68f, 0.04f), new Vector3(0.16f, 0.2f, 0.03f), Mat(Palette.Blade)).gameObject);
        }  // Конец запасной ветки условия.
        // Устанавливает слой объекта и потомков через `SetLayer` с аргументами `held, 2`.
        SetLayer(held, 2);
        // Меняет активность объекта через `held.gameObject.SetActive` с аргументами `false`.
        held.gameObject.SetActive(false);

        // Создаёт объект сцены с параметрами `"Камера"` и сохраняет ссылку в `camGo`.
        var camGo = new GameObject("Камера");
        // Привязывает к родителю через `camGo.transform.SetParent` с аргументами `view, false`.
        camGo.transform.SetParent(view, false);
        // Добавляет компонент через `camGo.AddComponent<Camera>` и записывает результат в `camera`.
        var camera = camGo.AddComponent<Camera>();
        // Задаёт тег `camera.tag` значением `"MainCamera"`.
        camera.tag = "MainCamera";
        // Задаёт ближняя плоскость отсечения `camera.nearClipPlane` значением `0.08`.
        camera.nearClipPlane = 0.08f;
        // Задаёт угол обзора камеры `camera.fieldOfView` значением `70`.
        camera.fieldOfView = 70f;
        // Добавляет компонент через `camGo.AddComponent<AudioListener>`.
        camGo.AddComponent<AudioListener>();

        // Добавляет компонент через `player.AddComponent<Inventory>`.
        player.AddComponent<Inventory>();
        // Добавляет компонент через `player.AddComponent<FirstPersonPlayer>` и записывает результат в `body`.
        var body = player.AddComponent<FirstPersonPlayer>();
        // Создаёт объект сцены с параметрами `"Худ"` и сохраняет ссылку в `hudGo`.
        var hudGo = new GameObject("Худ");
        // Добавляет компонент через `hudGo.AddComponent<PlayHud>` и записывает результат в `hud`.
        var hud = hudGo.AddComponent<PlayHud>();
        // Вызывает `hud.Build`.
        hud.Build();
        // Вызывает `body.Bind` с аргументами `site, hud`.
        body.Bind(site, hud);
        // Возвращает `body`.
        return body;
    }  // Конец метода `SpawnPlayer`.

    // Убирает широкий старый триггер продажи у прилавка.
    static void RemoveSellBoothTrigger(Transform yard)
    {  // Начало метода `RemoveSellBoothTrigger`.
        // Ищет объект по имени через `yard.Find` с аргументами `"Магазин"` и записывает результат в `booth`.
        var booth = yard.Find("Магазин");
        // Проверяет условие `booth == null`.
        if (booth == null)
            // Завершает выполнение текущего метода.
            return;
        // Собирает компоненты объекта через `booth.GetComponents<ShopCounter>` и записывает результат в `counters`.
        var counters = booth.GetComponents<ShopCounter>();
        // Перебирает индексы начиная с `int i = 0` при условии `i < counters.Length`.
        for (int i = 0; i < counters.Length; i++)
        {  // Начало цикла по индексам.
            // Проверяет условие `counters[i] is SellerShop`.
            if (counters[i] is SellerShop)
                // Пропускает остаток этой итерации и переходит к следующей.
                continue;
            // Проверяет условие `Application.isPlaying`.
            if (Application.isPlaying)
                // Планирует удаление объекта через `Destroy` с аргументами `counters[i]`.
                Destroy(counters[i]);
            // Обрабатывает случай, когда предыдущее условие ложно.
            else
                // Сразу удаляет объект через `DestroyImmediate` с аргументами `counters[i]`.
                DestroyImmediate(counters[i]);
        }  // Конец цикла по индексам.
        // Ищет компонент через `booth.GetComponent<BoxCollider>` и записывает результат в `box`.
        var box = booth.GetComponent<BoxCollider>();
        // Проверяет условие `box != null`.
        if (box != null)
        {  // Начало ветки условия.
            // Проверяет условие `Application.isPlaying`.
            if (Application.isPlaying)
                // Планирует удаление объекта через `Destroy` с аргументами `box`.
                Destroy(box);
            // Обрабатывает случай, когда предыдущее условие ложно.
            else
                // Сразу удаляет объект через `DestroyImmediate` с аргументами `box`.
                DestroyImmediate(box);
        }  // Конец ветки условия.
    }  // Конец метода `RemoveSellBoothTrigger`.

    // Корректирует коллайдеры вспомогательных точек магазина.
    static void FixShopTriggers(Transform yard)
    {  // Начало метода `FixShopTriggers`.
        // Собирает компоненты в объекте и его потомках через `yard.GetComponentsInChildren<ShopCounter>` с аргументами `true` и записывает результат в `counters`.
        var counters = yard.GetComponentsInChildren<ShopCounter>(true);
        // Перебирает индексы начиная с `int i = 0` при условии `i < counters.Length`.
        for (int i = 0; i < counters.Length; i++)
        {  // Начало цикла по индексам.
            // Проверяет условие `counters[i] is SellerShop or AnvilShop`.
            if (counters[i] is SellerShop or AnvilShop)
                // Пропускает остаток этой итерации и переходит к следующей.
                continue;
            // Ищет компонент через `counters[i].GetComponent<BoxCollider>` и записывает результат в `box`.
            var box = counters[i].GetComponent<BoxCollider>();
            // Проверяет условие `box != null`.
            if (box != null)
                // Устанавливает режим триггера `box.isTrigger` в `true`.
                box.isTrigger = true;
        }  // Конец цикла по индексам.
    }  // Конец метода `FixShopTriggers`.

    // Настраивает туман, окружающее освещение и фон камеры.
    static void TuneLook()
    {  // Начало метода `TuneLook`.
        // Устанавливает видимость тумана `RenderSettings.fog` в `true`.
        RenderSettings.fog = true;
        // Задаёт режим тумана `RenderSettings.fogMode` значением `FogMode.Linear`.
        RenderSettings.fogMode = FogMode.Linear;
        // Записывает в `RenderSettings.fogColor` цвет RGBA с параметрами `0.62f, 0.74f, 0.86f`.
        RenderSettings.fogColor = new Color(0.62f, 0.74f, 0.86f);
        // Задаёт начальная дистанция тумана `RenderSettings.fogStartDistance` значением `22`.
        RenderSettings.fogStartDistance = 22f;
        // Задаёт конечная дистанция тумана `RenderSettings.fogEndDistance` значением `78`.
        RenderSettings.fogEndDistance = 78f;
        // Задаёт `RenderSettings.ambientMode` значением `UnityEngine.Rendering.AmbientMode.Trilight`.
        RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Trilight;
        // Записывает в `RenderSettings.ambientSkyColor` цвет RGBA с параметрами `0.55f, 0.66f, 0.82f`.
        RenderSettings.ambientSkyColor = new Color(0.55f, 0.66f, 0.82f);
        // Записывает в `RenderSettings.ambientEquatorColor` цвет RGBA с параметрами `0.62f, 0.58f, 0.48f`.
        RenderSettings.ambientEquatorColor = new Color(0.62f, 0.58f, 0.48f);
        // Записывает в `RenderSettings.ambientGroundColor` цвет RGBA с параметрами `0.28f, 0.32f, 0.22f`.
        RenderSettings.ambientGroundColor = new Color(0.28f, 0.32f, 0.22f);
        // Задаёт `camera` значением главная камера сцены `Camera.main`.
        var camera = Camera.main;
        // Проверяет условие `camera != null`.
        if (camera != null)
        {  // Начало ветки условия.
            // Задаёт режим очистки фона камеры `camera.clearFlags` значением `CameraClearFlags.SolidColor`.
            camera.clearFlags = CameraClearFlags.SolidColor;
            // Записывает в `camera.backgroundColor` цвет RGBA с параметрами `0.55f, 0.72f, 0.9f`.
            camera.backgroundColor = new Color(0.55f, 0.72f, 0.9f);
        }  // Конец ветки условия.
    }  // Конец метода `TuneLook`.

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

    // Удаляет коллайдер с переданного объекта.
    static void StripCollider(GameObject go)
    {  // Начало метода `StripCollider`.
        // Ищет компонент через `go.GetComponent<Collider>` и записывает результат в `collider`.
        var collider = go.GetComponent<Collider>();
        // Проверяет условие `collider != null`.
        if (collider != null)
            // Планирует удаление объекта через `Destroy` с аргументами `collider`.
            Destroy(collider);
    }  // Конец метода `StripCollider`.

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

    // Регистрирует контроллер игры и строит мир при запуске сцены.
    void Awake()
    {  // Начало метода `Awake`.
        // Проверяет условие `!Application.isPlaying`.
        if (!Application.isPlaying)
            // Завершает выполнение текущего метода.
            return;
        // Ищет компонент через `GetComponent<GameLoop>` и записывает результат в `loop`.
        var loop = GetComponent<GameLoop>();
        // Проверяет условие `loop != null`.
        if (loop != null)
            // Вызывает `loop.Touch`.
            loop.Touch();
        // Подготавливает двор, восстанавливает состояние мира и создаёт игрока через `BuildWorld`.
        BuildWorld();
    }  // Конец метода `Awake`.
}  // Конец класса `YardBootstrap`.
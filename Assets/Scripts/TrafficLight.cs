// Подключает доступ к базовым типам игрового движка.
using UnityEngine;

// Создаёт светофоры и синхронизирует сигналы с движением машин.
public class TrafficLight : MonoBehaviour
{  // Начало класса `TrafficLight`.
    // Задаёт `Interval` значением `7`.
    public const float Interval = 7f;
    // Инициализирует `EastWestGreen` значением «да».
    public static bool EastWestGreen = true;
    // Инициализирует `ForEastWest` значением «да».
    public bool ForEastWest = true;
    // Хранит ссылку на компонент отрисовки в `redLamp`.
    Renderer redLamp;
    // Хранит ссылку на компонент отрисовки в `greenLamp`.
    Renderer greenLamp;

    // Собирает модель светофора с красной и зелёной лампами.
    public static void Spawn(Transform parent, Vector3 place, float yaw, bool forEastWest)
    {  // Начало метода `Spawn`.
        // Задаёт `root` значением `new GameObject("Светофор").transform`.
        var root = new GameObject("Светофор").transform;
        // Привязывает к родителю через `root.SetParent` с аргументами `parent, false`.
        root.SetParent(parent, false);
        // Задаёт мировая позиция `root.position` значением `place`.
        root.position = place;
        // Создаёт поворот из углов Эйлера через `Quaternion.Euler` с аргументами `0f, yaw, 0f` и записывает результат в `root.rotation`.
        root.rotation = Quaternion.Euler(0f, yaw, 0f);
        // Загружает ресурс через `Resources.Load<GameObject>` с аргументами `"Roads/traffic-light"` и записывает результат в `model`.
        var model = Resources.Load<GameObject>("Roads/traffic-light");
        // Инициализирует `housing` значением `null`.
        Transform housing = null;
        // Проверяет условие `model != null`.
        if (model != null)
        {  // Начало ветки условия.
            // Создаёт копию объекта через `Object.Instantiate` с аргументами `model, root` и записывает результат в `go`.
            var go = Object.Instantiate(model, root);
            // Задаёт имя `go.name` значением `"traffic-light"`.
            go.name = "traffic-light";
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
            // Вызывает `TextureFactory.Lit` с аргументами `Color.white, map, 0.18f` и записывает результат в `skin`.
            var skin = TextureFactory.Lit(Color.white, map, 0.18f);
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
            // Вызывает `KenneyKit.FitHeight` с аргументами `go.transform, 3.4f, false`.
            KenneyKit.FitHeight(go.transform, 3.4f, false);
            // Вызывает `KenneyKit.SitOnGround` с аргументами `go.transform`.
            KenneyKit.SitOnGround(go.transform);
            // Создаёт поворот из углов Эйлера через `Quaternion.Euler` с аргументами `0f, 180f, 0f` и записывает результат в `go.transform.localRotation`.
            go.transform.localRotation = Quaternion.Euler(0f, 180f, 0f);
            // Задаёт `housing` значением `go.transform`.
            housing = go.transform;
        }  // Конец ветки условия.
        // Обрабатывает случай, когда предыдущее условие ложно.
        else
        {  // Начало запасной ветки условия.
            // Создаёт стандартный примитив через `GameObject.CreatePrimitive` с аргументами `PrimitiveType.Cylinder` и записывает результат в `pole`.
            var pole = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            // Привязывает к родителю через `pole.transform.SetParent` с аргументами `root, false`.
            pole.transform.SetParent(root, false);
            // Записывает в `pole.transform.localPosition` трёхмерный вектор с параметрами `0f, 1.7f, 0f`.
            pole.transform.localPosition = new Vector3(0f, 1.7f, 0f);
            // Записывает в `pole.transform.localScale` трёхмерный вектор с параметрами `0.12f, 1.7f, 0.12f`.
            pole.transform.localScale = new Vector3(0.12f, 1.7f, 0.12f);
            // Вызывает `TextureFactory.Lit` с аргументами `Palette.Iron` и записывает результат в `pole.GetComponent<MeshRenderer>().sharedMaterial`.
            pole.GetComponent<MeshRenderer>().sharedMaterial = TextureFactory.Lit(Palette.Iron);
            // Планирует удаление объекта через `Object.Destroy` с аргументами `pole.GetComponent<Collider>()`.
            Object.Destroy(pole.GetComponent<Collider>());
        }  // Конец запасной ветки условия.

        // Добавляет компонент через `root.gameObject.AddComponent<TrafficLight>` и записывает результат в `light`.
        var light = root.gameObject.AddComponent<TrafficLight>();
        // Задаёт `light.ForEastWest` значением `forEastWest`.
        light.ForEastWest = forEastWest;
        // Выбирает значение для `host` по условию `housing` существует.
        Transform host = housing != null ? housing : root;
        // Создаёт внутреннюю светящуюся часть сигнала через `InnerLamp` с аргументами `host, "Красный", new Vector3(-0.034f, 0.488f, 0f)` и записывает результат в `light.redLamp`.
        light.redLamp = InnerLamp(host, "Красный", new Vector3(-0.034f, 0.488f, 0f));
        // Настраивает материал, способный светиться через `Emissive` с аргументами `new Color(0.35f, 0.28f, 0.05f), 0.15f` и записывает результат в `InnerLamp(host, "Жёлтый", new Vector3(-0.034f, 0.436f, 0f)).sharedMaterial`.
        InnerLamp(host, "Жёлтый", new Vector3(-0.034f, 0.436f, 0f)).sharedMaterial = Emissive(new Color(0.35f, 0.28f, 0.05f), 0.15f);
        // Создаёт внутреннюю светящуюся часть сигнала через `InnerLamp` с аргументами `host, "Зелёный", new Vector3(-0.034f, 0.385f, 0f)` и записывает результат в `light.greenLamp`.
        light.greenLamp = InnerLamp(host, "Зелёный", new Vector3(-0.034f, 0.385f, 0f));
        // Вызывает `KenneyKit.Block` с аргументами `root`.
        KenneyKit.Block(root);
    }  // Конец метода `Spawn`.

    // Создаёт внутреннюю светящуюся часть сигнала.
    static Renderer InnerLamp(Transform host, string name, Vector3 local)
    {  // Начало метода `InnerLamp`.
        // Создаёт стандартный примитив через `GameObject.CreatePrimitive` с аргументами `PrimitiveType.Sphere` и записывает результат в `go`.
        var go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        // Задаёт имя `go.name` значением `name`.
        go.name = name;
        // Привязывает к родителю через `go.transform.SetParent` с аргументами `host, false`.
        go.transform.SetParent(host, false);
        // Задаёт локальная позиция `go.transform.localPosition` значением `local`.
        go.transform.localPosition = local;
        // Задаёт локальный поворот `go.transform.localRotation` значением нулевой поворот `Quaternion.identity`.
        go.transform.localRotation = Quaternion.identity;
        // Задаёт локальный масштаб `go.transform.localScale` значением вектор (1, 1, 1) `Vector3.one` умножить на `0.02`.
        go.transform.localScale = Vector3.one * 0.02f;
        // Планирует удаление объекта через `Object.Destroy` с аргументами `go.GetComponent<Collider>()`.
        Object.Destroy(go.GetComponent<Collider>());
        // Ищет компонент через `go.GetComponent<MeshRenderer>` и записывает результат в `renderer`.
        var renderer = go.GetComponent<MeshRenderer>();
        // Настраивает материал, способный светиться через `Emissive` с аргументами `Color.black, 0.2f` и записывает результат в `renderer.sharedMaterial`.
        renderer.sharedMaterial = Emissive(Color.black, 0.2f);
        // Задаёт режим отбрасывания теней `renderer.shadowCastingMode` значением `UnityEngine.Rendering.ShadowCastingMode.Off`.
        renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        // Возвращает `renderer`.
        return renderer;
    }  // Конец метода `InnerLamp`.

    // Настраивает материал, способный светиться.
    static Material Emissive(Color color, float smoothness)
    {  // Начало метода `Emissive`.
        // Вызывает `TextureFactory.Lit` с аргументами `color, null, smoothness` и записывает результат в `material`.
        var material = TextureFactory.Lit(color, null, smoothness);
        // Включает ключевое слово шейдера через `material.EnableKeyword` с аргументами `"_EMISSION"`.
        material.EnableKeyword("_EMISSION");
        // Задаёт `material.globalIlluminationFlags` значением `MaterialGlobalIlluminationFlags.RealtimeEmissive`.
        material.globalIlluminationFlags = MaterialGlobalIlluminationFlags.RealtimeEmissive;
        // Задаёт цветовой параметр через `material.SetColor` с аргументами `"_EmissionColor", color`.
        material.SetColor("_EmissionColor", color);
        // Возвращает `material`.
        return material;
    }  // Конец метода `Emissive`.

    // Инициализирует компонент при пробуждении объекта Unity.
    void Awake()
    {  // Начало метода `Awake`.
        // Находит рендеры красной и зелёной ламп через `BindLamps`.
        BindLamps();
    }  // Конец метода `Awake`.

    // Находит рендеры красной и зелёной ламп.
    public void BindLamps()
    {  // Начало метода `BindLamps`.
        // Проверяет условие `redLamp != null && greenLamp != null`.
        if (redLamp != null && greenLamp != null)
            // Завершает выполнение текущего метода.
            return;
        // Собирает компоненты в объекте и его потомках через `GetComponentsInChildren<Renderer>` с аргументами `true` и записывает результат в `lamps`.
        var lamps = GetComponentsInChildren<Renderer>(true);
        // Перебирает индексы начиная с `int i = 0` при условии `i < lamps.Length`.
        for (int i = 0; i < lamps.Length; i++)
        {  // Начало цикла по индексам.
            // Задаёт `id` значением `lamps[i].name`.
            string id = lamps[i].name;
            // Проверяет условие `redLamp == null && id == "Красный"`.
            if (redLamp == null && id == "Красный")
                // Задаёт `redLamp` значением элемент `lamps` с индексом `i`.
                redLamp = lamps[i];
            // Если предыдущая проверка не сработала, проверяет `greenLamp == null && id == "Зелёный"`.
            else if (greenLamp == null && id == "Зелёный")
                // Задаёт `greenLamp` значением элемент `lamps` с индексом `i`.
                greenLamp = lamps[i];
        }  // Конец цикла по индексам.
    }  // Конец метода `BindLamps`.

    // Обновляет цвет и свечение ламп в текущей фазе.
    void Update()
    {  // Начало метода `Update`.
        // Проверяет условие `redLamp == null || greenLamp == null`.
        if (redLamp == null || greenLamp == null)
            // Находит рендеры красной и зелёной ламп через `BindLamps`.
            BindLamps();
        // Задаёт `EastWestGreen` значением вызывает `Mathf.PingPong` с аргументами `Time.time, Interval` меньше `Interval` умножить на `0.5`.
        EastWestGreen = Mathf.PingPong(Time.time, Interval) < Interval * 0.5f;
        // Выбирает значение для `green` по условию `ForEastWest`.
        bool green = ForEastWest ? EastWestGreen : !EastWestGreen;
        // Задаёт цвет и яркость конкретной лампы через `SetLamp` с аргументами `redLamp, green ? new Color(0.18f, 0.04f, 0.04f) : new Color(0.95f, 0.12f, 0.08f), green ? 0.15f : 2.6f`.
        SetLamp(redLamp, green ? new Color(0.18f, 0.04f, 0.04f) : new Color(0.95f, 0.12f, 0.08f), green ? 0.15f : 2.6f);
        // Задаёт цвет и яркость конкретной лампы через `SetLamp` с аргументами `greenLamp, green ? new Color(0.12f, 0.95f, 0.18f) : new Color(0.04f, 0.16f, 0.05f), green ? 2.4f : 0.12f`.
        SetLamp(greenLamp, green ? new Color(0.12f, 0.95f, 0.18f) : new Color(0.04f, 0.16f, 0.05f), green ? 2.4f : 0.12f);
    }  // Конец метода `Update`.

    // Задаёт цвет и яркость конкретной лампы.
    static void SetLamp(Renderer lamp, Color color, float emission)
    {  // Начало метода `SetLamp`.
        // Проверяет условие `lamp == null`.
        if (lamp == null)
            // Завершает выполнение текущего метода.
            return;
        // Задаёт цветовой параметр через `lamp.material.SetColor` с аргументами `"_BaseColor", color`.
        lamp.material.SetColor("_BaseColor", color);
        // Задаёт цветовой параметр через `lamp.material.SetColor` с аргументами `"_EmissionColor", color * emission`.
        lamp.material.SetColor("_EmissionColor", color * emission);
    }  // Конец метода `SetLamp`.

    // Проверяет, открыто ли движение в заданном направлении.
    public static bool Allowed(Vector3 dir)
    {  // Начало метода `Allowed`.
        // Вычисляет `eastWest` как берёт модуль числа через `Mathf.Abs` с аргументами `dir.x` не меньше берёт модуль числа через `Mathf.Abs` с аргументами `dir.z`.
        bool eastWest = Mathf.Abs(dir.x) >= Mathf.Abs(dir.z);
        // Выбирает возвращаемое значение по условию `eastWest`.
        return eastWest ? EastWestGreen : !EastWestGreen;
    }  // Конец метода `Allowed`.

    // Привязывает лампы всех светофоров двора после загрузки сцены.
    public static void RefreshScene(Transform yard)
    {  // Начало метода `RefreshScene`.
        // Проверяет условие `yard == null`.
        if (yard == null)
            // Завершает выполнение текущего метода.
            return;
        // Перебирает элементы `yard.GetComponentsInChildren<Transform>(true)` по одному в `transform`.
        foreach (var transform in yard.GetComponentsInChildren<Transform>(true))
        {  // Начало цикла по элементам.
            // Проверяет условие `transform.name != "Светофор"`.
            if (transform.name != "Светофор")
                // Пропускает остаток этой итерации и переходит к следующей.
                continue;
            // Ищет компонент через `transform.GetComponent<TrafficLight>` и записывает результат в `light`.
            var light = transform.GetComponent<TrafficLight>();
            // Проверяет условие `light == null`.
            if (light == null)
            {  // Начало ветки условия.
                // Добавляет компонент через `transform.gameObject.AddComponent<TrafficLight>` и записывает результат в `light`.
                light = transform.gameObject.AddComponent<TrafficLight>();
                // Задаёт поворот вокруг вертикальной оси `yaw` значением `transform.eulerAngles.y`.
                float yaw = transform.eulerAngles.y;
                // Задаёт `light.ForEastWest` значением берёт модуль числа через `Mathf.Abs` с аргументами `Mathf.DeltaAngle(yaw, 0f)` меньше `50` или берёт модуль числа через `Mathf.Abs` с аргументами `Mathf.DeltaAngle(yaw, 180f)` меньше `50`.
                light.ForEastWest = Mathf.Abs(Mathf.DeltaAngle(yaw, 0f)) < 50f
                    // Дополняет проверку условием `Mathf.Abs(Mathf.DeltaAngle(yaw, 180f)) < 50f`.
                    || Mathf.Abs(Mathf.DeltaAngle(yaw, 180f)) < 50f;
            }  // Конец ветки условия.
            // Находит рендеры красной и зелёной ламп через `light.BindLamps`.
            light.BindLamps();
        }  // Конец цикла по элементам.
    }  // Конец метода `RefreshScene`.
}  // Конец класса `TrafficLight`.

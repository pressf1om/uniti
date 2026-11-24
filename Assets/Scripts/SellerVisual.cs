// Подключает доступ к базовым типам игрового движка.
using UnityEngine;

// Загружает модель продавца и при необходимости собирает её из примитивов.
public class SellerVisual : MonoBehaviour
{  // Начало класса `SellerVisual`.
    // Задаёт `ModelHeight` значением `1.62`.
    const float ModelHeight = 1.62f;

    // Хранит ссылку на положение, поворот и масштаб объекта в `swayRoot`.
    Transform swayRoot;
    // Хранит ссылку на положение, поворот и масштаб объекта в `armLeft`.
    Transform armLeft;
    // Хранит ссылку на положение, поворот и масштаб объекта в `armRight`.
    Transform armRight;
    // Хранит вещественное число в `phase`.
    float phase;
    // Хранит поле типа `Animator` в `humanoid`.
    Animator humanoid;
    // Хранит логический флаг в `rigged`.
    bool rigged;

    // Прикрепляет к продавцу видимую модель и подготавливает анимацию.
    public static void Build(Transform host)
    {  // Начало метода `Build`.
        // Проверяет условие `host == null`.
        if (host == null)
            // Завершает выполнение текущего метода.
            return;

        // Убирает исходный примитив, чтобы отобразить новую модель через `ClearPrimitiveMesh` с аргументами `host.gameObject`.
        ClearPrimitiveMesh(host.gameObject);
        // Ищет объект по имени через `host.Find` с аргументами `"Body"` и записывает результат в `existing`.
        var existing = host.Find("Body");
        // Проверяет условие `existing != null`.
        if (existing != null)
        {  // Начало ветки условия.
            // Проверяет условие `existing.Find("Продавец-модель") != null`.
            if (existing.Find("Продавец-модель") != null)
                // Завершает выполнение текущего метода.
                return;
            // Удаляет объект подходящим для режима Unity способом через `DropComponent` с аргументами `existing.gameObject`.
            DropComponent(existing.gameObject);
        }  // Конец ветки условия.

        // Задаёт `body` значением `new GameObject("Body").transform`.
        var body = new GameObject("Body").transform;
        // Привязывает к родителю через `body.SetParent` с аргументами `host, false`.
        body.SetParent(host, false);

        // Перебирает доступные ресурсы модели продавца через `LoadTraderPrefab` и записывает результат в `modelPrefab`.
        var modelPrefab = LoadTraderPrefab();
        // Проверяет условие `modelPrefab != null && TrySpawnModel(body, modelPrefab, host)`.
        if (modelPrefab != null && TrySpawnModel(body, modelPrefab, host))
            // Завершает выполнение текущего метода.
            return;

        // Собирает запасную фигуру продавца из примитивов через `BuildFallback` с аргументами `body, host`.
        BuildFallback(body, host);
    }  // Конец метода `Build`.

    // Перебирает доступные ресурсы модели продавца.
    static GameObject LoadTraderPrefab()
    {  // Начало метода `LoadTraderPrefab`.
        // Загружает ресурс через `Resources.Load<GameObject>` с аргументами `"Characters/ShopkeeperBaked"` и записывает результат в `baked`.
        var baked = Resources.Load<GameObject>("Characters/ShopkeeperBaked");
        // Проверяет условие `baked != null`.
        if (baked != null)
            // Возвращает `baked`.
            return baked;
        // Загружает ресурс через `Resources.Load<GameObject>` с аргументами `"Characters/VendorBaked"` и записывает результат в `baked`.
        baked = Resources.Load<GameObject>("Characters/VendorBaked");
        // Проверяет условие `baked != null`.
        if (baked != null)
            // Возвращает `baked`.
            return baked;

        // Загружает ресурс через `Resources.Load<GameObject>` с аргументами `"Characters/shopkeeper"` и записывает результат в `shopkeeper`.
        var shopkeeper = Resources.Load<GameObject>("Characters/shopkeeper");
        // Проверяет условие `shopkeeper != null && shopkeeper.GetComponentInChildren<Renderer>() != null`.
        if (shopkeeper != null && shopkeeper.GetComponentInChildren<Renderer>() != null)
            // Возвращает `shopkeeper`.
            return shopkeeper;

        // Загружает ресурсы через `Resources.LoadAll<GameObject>` с аргументами `"Characters"` и записывает результат в `all`.
        var all = Resources.LoadAll<GameObject>("Characters");
        // Перебирает индексы начиная с `int i = 0` при условии `i < all.Length`.
        for (int i = 0; i < all.Length; i++)
        {  // Начало цикла по индексам.
            // Вызывает `all[i].name.ToLowerInvariant` и записывает результат в `name`.
            string name = all[i].name.ToLowerInvariant();
            // Проверяет условие `name.Contains("shopkeeper") || name.Contains("shop-worker")`.
            if (name.Contains("shopkeeper") || name.Contains("shop-worker"))
                // Возвращает элемент `all` с индексом `i`.
                return all[i];
        }  // Конец цикла по индексам.

        // Загружает ресурс через `Resources.Load<GameObject>` с аргументами `"Characters/vendor"` и записывает результат в `vendor`.
        var vendor = Resources.Load<GameObject>("Characters/vendor");
        // Проверяет условие `vendor != null`.
        if (vendor != null)
            // Возвращает `vendor`.
            return vendor;

        // Перебирает индексы начиная с `int i = 0` при условии `i < all.Length`.
        for (int i = 0; i < all.Length; i++)
        {  // Начало цикла по индексам.
            // Проверяет условие `all[i].GetComponentInChildren<SkinnedMeshRenderer>() != null`.
            if (all[i].GetComponentInChildren<SkinnedMeshRenderer>() != null)
                // Возвращает элемент `all` с индексом `i`.
                return all[i];
        }  // Конец цикла по индексам.
        // Возвращает `null`, то есть отсутствие результата поиска.
        return null;
    }  // Конец метода `LoadTraderPrefab`.

    // Создаёт импортированную модель и подгоняет её для сцены.
    static bool TrySpawnModel(Transform body, GameObject prefab, Transform host)
    {  // Начало метода `TrySpawnModel`.
        // Создаёт копию объекта через `Object.Instantiate` с аргументами `prefab, body, false` и записывает результат в `model`.
        var model = Object.Instantiate(prefab, body, false);
        // Задаёт имя `model.name` значением `"Продавец-модель"`.
        model.name = "Продавец-модель";
        // Задаёт локальная позиция `model.transform.localPosition` значением нулевой вектор `Vector3.zero`.
        model.transform.localPosition = Vector3.zero;
        // Задаёт локальный поворот `model.transform.localRotation` значением нулевой поворот `Quaternion.identity`.
        model.transform.localRotation = Quaternion.identity;
        // Задаёт локальный масштаб `model.transform.localScale` значением вектор (1, 1, 1) `Vector3.one`.
        model.transform.localScale = Vector3.one;

        // Проверяет условие `model.GetComponentInChildren<Renderer>() == null`.
        if (model.GetComponentInChildren<Renderer>() == null)
        {  // Начало ветки условия.
            // Удаляет объект подходящим для режима Unity способом через `DropComponent` с аргументами `model`.
            DropComponent(model);
            // Возвращает `false`.
            return false;
        }  // Конец ветки условия.

        // Вызывает `KenneyKit.FitHeight` с аргументами `model.transform, ModelHeight, false`.
        KenneyKit.FitHeight(model.transform, ModelHeight, false);
        // Вызывает `KenneyKit.SitOnGround` с аргументами `model.transform, body.position.y`.
        KenneyKit.SitOnGround(model.transform, body.position.y);
        // Убирает коллайдеры из иерархии модели через `StripColliders` с аргументами `model.transform`.
        StripColliders(model.transform);
        // Проверяет условие `!Application.isEditor`.
        if (!Application.isEditor)
            // Обновляет материалы модели для совместимости с URP через `RematerializeUrp` с аргументами `model.transform`.
            RematerializeUrp(model.transform);

        // Ищет компонент через `host.GetComponent<SellerVisual>` и записывает результат в `guide`.
        var guide = host.GetComponent<SellerVisual>();
        // Проверяет условие `guide == null`.
        if (guide == null)
            // Добавляет компонент через `host.gameObject.AddComponent<SellerVisual>` и записывает результат в `guide`.
            guide = host.gameObject.AddComponent<SellerVisual>();
        // Задаёт `guide.swayRoot` значением `body`.
        guide.swayRoot = body;
        // Ищет компонент в объекте и его потомках через `model.GetComponentInChildren<Animator>` и записывает результат в `guide.humanoid`.
        guide.humanoid = model.GetComponentInChildren<Animator>();
        // Проверяет условие `guide.humanoid == null`.
        if (guide.humanoid == null)
            // Ищет компонент через `model.GetComponent<Animator>` и записывает результат в `guide.humanoid`.
            guide.humanoid = model.GetComponent<Animator>();

        // Задаёт `guide.rigged` значением `guide.humanoid` отличается от отсутствующую ссылку (`null`) и `guide.humanoid.avatar` отличается от отсутствующую ссылку (`null`) и `guide.humanoid.isHuman`.
        guide.rigged = guide.humanoid != null && guide.humanoid.avatar != null && guide.humanoid.isHuman;
        // Проверяет условие `guide.rigged`.
        if (guide.rigged)
        {  // Начало ветки условия.
            // Присваивает `guide.armLeft` результат `FindArm(guide.humanoid, HumanBodyBones.LeftUpperArm, "LeftArm", "upperarm_l")`.
            guide.armLeft = FindArm(guide.humanoid, HumanBodyBones.LeftUpperArm, "LeftArm", "upperarm_l");
            // Присваивает `guide.armRight` результат `FindArm(guide.humanoid, HumanBodyBones.RightUpperArm, "RightArm", "upperarm_r")`.
            guide.armRight = FindArm(guide.humanoid, HumanBodyBones.RightUpperArm, "RightArm", "upperarm_r");
        }  // Конец ветки условия.
        // Обрабатывает случай, когда предыдущее условие ложно.
        else
        {  // Начало запасной ветки условия.
            // Ищет дочерний объект по имени через `FindNode` с аргументами `model.transform, "upperarm", "arm"` и записывает результат в `guide.armLeft`.
            guide.armLeft = FindNode(model.transform, "upperarm", "arm");
            // Задаёт `guide.armRight` значением отсутствующую ссылку (`null`).
            guide.armRight = null;
        }  // Конец запасной ветки условия.
        // Возвращает `true`.
        return true;
    }  // Конец метода `TrySpawnModel`.

    // Ищет дочерний объект по имени.
    static Transform FindNode(Transform root, string a, string b)
    {  // Начало метода `FindNode`.
        // Перебирает элементы `root.GetComponentsInChildren<Transform>(true)` по одному в `tr`.
        foreach (var tr in root.GetComponentsInChildren<Transform>(true))
        {  // Начало цикла по элементам.
            // Вызывает `tr.name.ToLowerInvariant` и записывает результат в `n`.
            string n = tr.name.ToLowerInvariant();
            // Проверяет условие `n.Contains(a) || n.Contains(b)`.
            if (n.Contains(a) || n.Contains(b))
                // Возвращает `tr`.
                return tr;
        }  // Конец цикла по элементам.
        // Возвращает `null`, то есть отсутствие результата поиска.
        return null;
    }  // Конец метода `FindNode`.

    // Ищет руку через humanoid-кость или по именам узлов.
    static Transform FindArm(Animator animator, HumanBodyBones bone, string a, string b)
    {  // Начало метода `FindArm`.
        // Проверяет условие `animator != null && animator.avatar != null && animator.isHuman`.
        if (animator != null && animator.avatar != null && animator.isHuman)
        {  // Начало ветки условия.
            // Вызывает `animator.GetBoneTransform` с аргументами `bone` и записывает результат в `t`.
            var t = animator.GetBoneTransform(bone);
            // Проверяет условие `t != null`.
            if (t != null)
                // Возвращает `t`.
                return t;
        }  // Конец ветки условия.
        // Проверяет условие `animator == null`.
        if (animator == null)
            // Возвращает `null`, то есть отсутствие результата поиска.
            return null;
        // Перебирает элементы `animator.GetComponentsInChildren<Transform>(true)` по одному в `tr`.
        foreach (var tr in animator.GetComponentsInChildren<Transform>(true))
        {  // Начало цикла по элементам.
            // Вызывает `tr.name.ToLowerInvariant` и записывает результат в `n`.
            string n = tr.name.ToLowerInvariant();
            // Проверяет условие `n.Contains(a.ToLowerInvariant()) || n.Contains(b)`.
            if (n.Contains(a.ToLowerInvariant()) || n.Contains(b))
                // Возвращает `tr`.
                return tr;
        }  // Конец цикла по элементам.
        // Возвращает `null`, то есть отсутствие результата поиска.
        return null;
    }  // Конец метода `FindArm`.

    // Собирает запасную фигуру продавца из примитивов.
    static void BuildFallback(Transform rig, Transform host)
    {  // Начало метода `BuildFallback`.
        // Создаёт материал с заданным цветом через `Mat` с аргументами `new Color(0.32f, 0.46f, 0.72f)` и записывает результат в `cloth`.
        var cloth = Mat(new Color(0.32f, 0.46f, 0.72f));
        // Создаёт материал с заданным цветом через `Mat` с аргументами `new Color(0.86f, 0.7f, 0.58f)` и записывает результат в `skin`.
        var skin = Mat(new Color(0.86f, 0.7f, 0.58f));

        // Создаёт одну видимую часть запасной модели продавца через `Part` с аргументами `rig, "Туловище", PrimitiveType.Capsule, new Vector3(0f, 1.05f, 0f), new Vector3(0.42f, 0.5f, 0.3f), cloth`.
        Part(rig, "Туловище", PrimitiveType.Capsule, new Vector3(0f, 1.05f, 0f), new Vector3(0.42f, 0.5f, 0.3f), cloth);
        // Создаёт одну видимую часть запасной модели продавца через `Part` с аргументами `rig, "Голова", PrimitiveType.Sphere, new Vector3(0f, 1.52f, 0f), new Vector3(0.34f, 0.34f, 0.34f), skin`.
        Part(rig, "Голова", PrimitiveType.Sphere, new Vector3(0f, 1.52f, 0f), new Vector3(0.34f, 0.34f, 0.34f), skin);

        // Ищет компонент через `host.GetComponent<SellerVisual>` и записывает результат в `guide`.
        var guide = host.GetComponent<SellerVisual>();
        // Проверяет условие `guide == null`.
        if (guide == null)
            // Добавляет компонент через `host.gameObject.AddComponent<SellerVisual>` и записывает результат в `guide`.
            guide = host.gameObject.AddComponent<SellerVisual>();
        // Задаёт `guide.swayRoot` значением `rig`.
        guide.swayRoot = rig;
    }  // Конец метода `BuildFallback`.

    // Обновляет движение частей модели продавца.
    void Update()
    {  // Начало метода `Update`.
        // Добавляет `Time.deltaTime` к текущему значению `phase`.
        phase += Time.deltaTime;
        // Вызывает `Mathf.Sin` с аргументами `phase * 4.8f` и записывает результат в `talk`.
        float talk = Mathf.Sin(phase * 4.8f);
        // Проверяет условие `rigged && armLeft != null`.
        if (rigged && armLeft != null)
            // Создаёт поворот из углов Эйлера через `Quaternion.Euler` с аргументами `18f + talk * 22f, 0f, 0f` и записывает результат в `armLeft.localRotation`.
            armLeft.localRotation = Quaternion.Euler(18f + talk * 22f, 0f, 0f);
        // Проверяет условие `rigged && armRight != null`.
        if (rigged && armRight != null)
            // Создаёт поворот из углов Эйлера через `Quaternion.Euler` с аргументами `14f - talk * 18f, 0f, 0f` и записывает результат в `armRight.localRotation`.
            armRight.localRotation = Quaternion.Euler(14f - talk * 18f, 0f, 0f);
        // Проверяет условие `!rigged && swayRoot != null`.
        if (!rigged && swayRoot != null)
            // Создаёт поворот из углов Эйлера через `Quaternion.Euler` с аргументами `0f, talk * 3.5f, 0f` и записывает результат в `swayRoot.localRotation`.
            swayRoot.localRotation = Quaternion.Euler(0f, talk * 3.5f, 0f);
    }  // Конец метода `Update`.

    // Создаёт одну видимую часть запасной модели продавца.
    static Transform Part(Transform parent, string name, PrimitiveType type, Vector3 pos, Vector3 scale, Material mat)
    {  // Начало метода `Part`.
        // Создаёт стандартный примитив через `GameObject.CreatePrimitive` с аргументами `type` и записывает результат в `go`.
        var go = GameObject.CreatePrimitive(type);
        // Задаёт имя `go.name` значением `name`.
        go.name = name;
        // Привязывает к родителю через `go.transform.SetParent` с аргументами `parent, false`.
        go.transform.SetParent(parent, false);
        // Задаёт локальная позиция `go.transform.localPosition` значением `pos`.
        go.transform.localPosition = pos;
        // Задаёт локальный масштаб `go.transform.localScale` значением `scale`.
        go.transform.localScale = scale;
        // Задаёт общий материал `go.GetComponent<MeshRenderer>().sharedMaterial` значением `mat`.
        go.GetComponent<MeshRenderer>().sharedMaterial = mat;
        // Планирует удаление объекта через `Object.Destroy` с аргументами `go.GetComponent<Collider>()`.
        Object.Destroy(go.GetComponent<Collider>());
        // Возвращает `go.transform`.
        return go.transform;
    }  // Конец метода `Part`.

    // Убирает исходный примитив, чтобы отобразить новую модель.
    static void ClearPrimitiveMesh(GameObject host)
    {  // Начало метода `ClearPrimitiveMesh`.
        // Ищет компонент через `host.GetComponent<MeshFilter>` и записывает результат в `filter`.
        var filter = host.GetComponent<MeshFilter>();
        // Ищет компонент через `host.GetComponent<MeshRenderer>` и записывает результат в `render`.
        var render = host.GetComponent<MeshRenderer>();
        // Ищет компонент через `host.GetComponent<Collider>` и записывает результат в `col`.
        var col = host.GetComponent<Collider>();
        // Проверяет условие `filter != null`.
        if (filter != null)
            // Удаляет объект подходящим для режима Unity способом через `DropComponent` с аргументами `filter`.
            DropComponent(filter);
        // Проверяет условие `render != null`.
        if (render != null)
            // Удаляет объект подходящим для режима Unity способом через `DropComponent` с аргументами `render`.
            DropComponent(render);
        // Проверяет условие `col != null`.
        if (col != null)
            // Удаляет объект подходящим для режима Unity способом через `DropComponent` с аргументами `col`.
            DropComponent(col);
    }  // Конец метода `ClearPrimitiveMesh`.

    // Убирает коллайдеры из иерархии модели.
    static void StripColliders(Transform root)
    {  // Начало метода `StripColliders`.
        // Собирает компоненты в объекте и его потомках через `root.GetComponentsInChildren<Collider>` с аргументами `true` и записывает результат в `cols`.
        var cols = root.GetComponentsInChildren<Collider>(true);
        // Перебирает индексы начиная с `int i = 0` при условии `i < cols.Length`.
        for (int i = 0; i < cols.Length; i++)
            // Удаляет объект подходящим для режима Unity способом через `DropComponent` с аргументами `cols[i]`.
            DropComponent(cols[i]);
    }  // Конец метода `StripColliders`.

    // Обновляет материалы модели для совместимости с URP.
    static void RematerializeUrp(Transform root)
    {  // Начало метода `RematerializeUrp`.
        // Собирает компоненты в объекте и его потомках через `root.GetComponentsInChildren<Renderer>` с аргументами `true` и записывает результат в `renderers`.
        var renderers = root.GetComponentsInChildren<Renderer>(true);
        // Перебирает индексы начиная с `int i = 0` при условии `i < renderers.Length`.
        for (int i = 0; i < renderers.Length; i++)
        {  // Начало цикла по индексам.
            // Задаёт `r` значением элемент `renderers` с индексом `i`.
            var r = renderers[i];
            // Задаёт `src` значением `r.sharedMaterial`.
            var src = r.sharedMaterial;
            // Проверяет условие `src == null || src.shader == null`.
            if (src == null || src.shader == null)
                // Пропускает остаток этой итерации и переходит к следующей.
                continue;
            // Проверяет условие `src.shader.name.Contains("Universal Render Pipeline/Lit")`.
            if (src.shader.name.Contains("Universal Render Pipeline/Lit"))
                // Пропускает остаток этой итерации и переходит к следующей.
                continue;
            // Задаёт `tex` значением `src.mainTexture`.
            var tex = src.mainTexture;
            // Проверяет условие `tex == null && src.HasProperty("_BaseMap")`.
            if (tex == null && src.HasProperty("_BaseMap"))
                // Вызывает `src.GetTexture` с аргументами `"_BaseMap"` и записывает результат в `tex`.
                tex = src.GetTexture("_BaseMap");
            // Выбирает значение для `color` по условию `src.HasProperty("_BaseColor")`.
            var color = src.HasProperty("_BaseColor") ? src.GetColor("_BaseColor") : Color.white;
            // Вызывает `TextureFactory.Lit` с аргументами `color, tex as Texture2D, 0.28f` и записывает результат в `r.material`.
            r.material = TextureFactory.Lit(color, tex as Texture2D, 0.28f);
        }  // Конец цикла по индексам.
    }  // Конец метода `RematerializeUrp`.

    // Удаляет объект подходящим для режима Unity способом.
    static void DropComponent(Object obj)
    {  // Начало метода `DropComponent`.
        // Проверяет условие `Application.isPlaying`.
        if (Application.isPlaying)
            // Планирует удаление объекта через `Object.Destroy` с аргументами `obj`.
            Object.Destroy(obj);
        // Обрабатывает случай, когда предыдущее условие ложно.
        else
            // Сразу удаляет объект через `Object.DestroyImmediate` с аргументами `obj`.
            Object.DestroyImmediate(obj);
    }  // Конец метода `DropComponent`.

    // Создаёт материал с заданным цветом.
    static Material Mat(Color color)
    {  // Начало метода `Mat`.
        // Возвращает результат вызова `TextureFactory.Lit(color, null, 0.22f)`.
        return TextureFactory.Lit(color, null, 0.22f);
    }  // Конец метода `Mat`.
}  // Конец класса `SellerVisual`.
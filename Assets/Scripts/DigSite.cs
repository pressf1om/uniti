// Подключает доступ к базовым типам игрового движка.
using UnityEngine;

// Управляет сеткой раскопа, появлением блоков, находок и клада.
public class DigSite : MonoBehaviour
{  // Начало класса `DigSite`.
    // Объявляет событие `FirstDig` для подписчиков.
    public event System.Action FirstDig;

    // Запоминает, было ли уже первое успешное копание.
    bool dugOnce;
    // Хранит признак уже взятого клада `skipTreasure`.
    bool skipTreasure;
    // Вычисляет индекс клада по X `treasureIx` как `YardMetrics.Width` разделить на `2`.
    int treasureIx = YardMetrics.Width / 2;
    // Вычисляет индекс клада по Z `treasureIz` как `YardMetrics.Width` разделить на `2`.
    int treasureIz = YardMetrics.Width / 2;
    // Хранит ключи удалённых ячеек, которые требуется сохранить.
    readonly System.Collections.Generic.HashSet<int> removed = new System.Collections.Generic.HashSet<int>();
    // Хранит ключи созданных ячеек, чтобы не добавить блок дважды.
    readonly System.Collections.Generic.HashSet<int> spawned = new System.Collections.Generic.HashSet<int>();
    // Хранит ключи ячеек, находки из которых игрок уже подобрал.
    readonly System.Collections.Generic.HashSet<int> looted = new System.Collections.Generic.HashSet<int>();
    // Хранит ссылку на положение, поворот и масштаб объекта в `blocks`.
    Transform blocks;
    // Хранит ссылку на материал поверхности в `grassMat`.
    Material grassMat;
    // Хранит ссылку на материал поверхности в `crossGrassMat`.
    Material crossGrassMat;
    // Хранит ссылку на материал поверхности в `dirtMat`.
    Material dirtMat;
    // Хранит ссылку на материал поверхности в `concreteMat`.
    Material concreteMat;
    // Хранит ссылку на материал поверхности в `stoneMat`.
    Material stoneMat;
    // Хранит ссылку на материал поверхности в `coalMat`.
    Material coalMat;
    // Хранит ссылку на материал поверхности в `copperMat`.
    Material copperMat;
    // Хранит ссылку на материал поверхности в `ironMat`.
    Material ironMat;
    // Хранит ссылку на материал поверхности в `goldMat`.
    Material goldMat;
    // Хранит ссылку на материал поверхности в `treasureMat`.
    Material treasureMat;
    // Хранит ссылку на геометрию модели в `cubeMesh`.
    static Mesh cubeMesh;

    // Даёт доступ к `TreasureIx` через `treasureIx`.
    public int TreasureIx => treasureIx;
    // Даёт доступ к `TreasureIz` через `treasureIz`.
    public int TreasureIz => treasureIz;

    // Перед строительством ямы восстанавливает координаты клада и состояние ячеек.
    public void Prime(int[] keys, bool treasureGone, int[] lootedKeys, int savedIx, int savedIz)
    {  // Начало метода `Prime`.
        // Очищает содержимое через `removed.Clear`.
        removed.Clear();
        // Очищает содержимое через `looted.Clear`.
        looted.Clear();
        // Проверяет условие `keys != null`.
        if (keys != null)
        {  // Начало ветки условия.
            // Перебирает индексы начиная с `int i = 0` при условии `i < keys.Length`.
            for (int i = 0; i < keys.Length; i++)
                // Добавляет элемент через `removed.Add` с аргументами `keys[i]`.
                removed.Add(keys[i]);
        }  // Конец ветки условия.
        // Проверяет условие `lootedKeys != null`.
        if (lootedKeys != null)
        {  // Начало ветки условия.
            // Перебирает индексы начиная с `int i = 0` при условии `i < lootedKeys.Length`.
            for (int i = 0; i < lootedKeys.Length; i++)
                // Добавляет элемент через `looted.Add` с аргументами `lootedKeys[i]`.
                looted.Add(lootedKeys[i]);
        }  // Конец ветки условия.

        // Принимает сохранённое место клада при наличии обоих индексов.
        if (savedIx >= 0 && savedIz >= 0)
        {  // Начало ветки условия.
            // Задаёт индекс клада по X `treasureIx` значением `savedIx`.
            treasureIx = savedIx;
            // Задаёт индекс клада по Z `treasureIz` значением `savedIz`.
            treasureIz = savedIz;
        }  // Конец ветки условия.

        // Запоминает, что клад уже найден в сохранённой игре.
        skipTreasure = treasureGone;
        // Проверяет условие `treasureGone`.
        if (treasureGone)
        {  // Начало ветки условия.
            // Получает ключ ячейки клада на самом глубоком слое.
            int key = Pack(treasureIx, YardMetrics.Layers - 1, treasureIz);
            // Добавляет элемент через `removed.Add` с аргументами `key`.
            removed.Add(key);
            // Добавляет элемент через `looted.Add` с аргументами `key`.
            looted.Add(key);
        }  // Конец ветки условия.
    }  // Конец метода `Prime`.

    // Случайно выбирает один из четырёх углов для клада.
    public void PickNewTreasureCorner()
    {  // Начало метода `PickNewTreasureCorner`.
        // Выбирает одно из четырёх целых значений от 0 до 3.
        int corner = Random.Range(0, 4);
        // Выбирает ветку по значению `corner`.
        switch (corner)
        {  // Начало выбора по значению.
            // Обрабатывает значение `0`.
            case 0:
                // Задаёт индекс клада по X `treasureIx` значением `2`.
                treasureIx = 2;
                // Задаёт индекс клада по Z `treasureIz` значением `2`.
                treasureIz = 2;
                // Выходит из текущего цикла или блока выбора.
                break;
            // Обрабатывает значение `1`.
            case 1:
                // Задаёт индекс клада по X `treasureIx` значением `2`.
                treasureIx = 2;
                // Задаёт индекс клада по Z `treasureIz` значением `YardMetrics.Width` минус `3`.
                treasureIz = YardMetrics.Width - 3;
                // Выходит из текущего цикла или блока выбора.
                break;
            // Обрабатывает значение `2`.
            case 2:
                // Задаёт индекс клада по X `treasureIx` значением `YardMetrics.Width` минус `3`.
                treasureIx = YardMetrics.Width - 3;
                // Задаёт индекс клада по Z `treasureIz` значением `2`.
                treasureIz = 2;
                // Выходит из текущего цикла или блока выбора.
                break;
            // Обрабатывает все остальные значения.
            default:
                // Задаёт индекс клада по X `treasureIx` значением `YardMetrics.Width` минус `3`.
                treasureIx = YardMetrics.Width - 3;
                // Задаёт индекс клада по Z `treasureIz` значением `YardMetrics.Width` минус `3`.
                treasureIz = YardMetrics.Width - 3;
                // Выходит из текущего цикла или блока выбора.
                break;
        }  // Конец выбора по значению.
    }  // Конец метода `PickNewTreasureCorner`.

    // Копирует ключи выкопанных ячеек для сохранения.
    public int[] ExportDug()
    {  // Начало метода `ExportDug`.
        // Задаёт `keys` массивом `new int[removed.Count]`.
        var keys = new int[removed.Count];
        // Копирует элементы в массив через `removed.CopyTo` с аргументами `keys`.
        removed.CopyTo(keys);
        // Возвращает `keys`.
        return keys;
    }  // Конец метода `ExportDug`.

    // Копирует ключи ячеек с уже подобранной добычей.
    public int[] ExportLooted()
    {  // Начало метода `ExportLooted`.
        // Задаёт `keys` массивом `new int[looted.Count]`.
        var keys = new int[looted.Count];
        // Копирует элементы в массив через `looted.CopyTo` с аргументами `keys`.
        looted.CopyTo(keys);
        // Возвращает `keys`.
        return keys;
    }  // Конец метода `ExportLooted`.

    // Преобразует координаты ячейки в целочисленный ключ.
    public static int Pack(int ix, int iy, int iz)
    {  // Начало метода `Pack`.
        // Возвращает результат вызова `YardMetrics.Pack(ix, iy, iz)`.
        return YardMetrics.Pack(ix, iy, iz);
    }  // Конец метода `Pack`.

    // Создаёт травяную поверхность, вскрытые слои и бетонную оболочку раскопа.
    public void Build()
    {  // Начало метода `Build`.
        // Вызывает `TextureFactory.Lit` с аргументами `Palette.Grass, TextureFactory.Grass(), 0.08f` и записывает результат в `grassMat`.
        grassMat = TextureFactory.Lit(Palette.Grass, TextureFactory.Grass(), 0.08f);
        // Вызывает `TextureFactory.Lit` с аргументами `Palette.Cross, TextureFactory.Grass(), 0.08f` и записывает результат в `crossGrassMat`.
        crossGrassMat = TextureFactory.Lit(Palette.Cross, TextureFactory.Grass(), 0.08f);
        // Вызывает `TextureFactory.Lit` с аргументами `Palette.Dirt, TextureFactory.Speckled(), 0.12f` и записывает результат в `dirtMat`.
        dirtMat = TextureFactory.Lit(Palette.Dirt, TextureFactory.Speckled(), 0.12f);
        // Вызывает `TextureFactory.Lit` с аргументами `Palette.Concrete, TextureFactory.Speckled(), 0.18f` и записывает результат в `concreteMat`.
        concreteMat = TextureFactory.Lit(Palette.Concrete, TextureFactory.Speckled(), 0.18f);
        // Создаёт материал с заданным цветом через `Mat` с аргументами `Palette.Stone` и записывает результат в `stoneMat`.
        stoneMat = Mat(Palette.Stone);
        // Создаёт материал с заданным цветом через `Mat` с аргументами `Palette.Coal` и записывает результат в `coalMat`.
        coalMat = Mat(Palette.Coal);
        // Создаёт материал с заданным цветом через `Mat` с аргументами `Palette.Copper` и записывает результат в `copperMat`.
        copperMat = Mat(Palette.Copper);
        // Создаёт материал с заданным цветом через `Mat` с аргументами `Palette.Iron` и записывает результат в `ironMat`.
        ironMat = Mat(Palette.Iron);
        // Создаёт материал с заданным цветом через `Mat` с аргументами `Palette.Gold` и записывает результат в `goldMat`.
        goldMat = Mat(Palette.Gold);
        // Вызывает `TextureFactory.Lit` с аргументами `Palette.Treasure, null, 0.72f` и записывает результат в `treasureMat`.
        treasureMat = TextureFactory.Lit(Palette.Treasure, null, 0.72f);
        // Включает ключевое слово шейдера через `treasureMat.EnableKeyword` с аргументами `"_EMISSION"`.
        treasureMat.EnableKeyword("_EMISSION");
        // Задаёт цветовой параметр через `treasureMat.SetColor` с аргументами `"_EmissionColor", Palette.Treasure * 0.65f`.
        treasureMat.SetColor("_EmissionColor", Palette.Treasure * 0.65f);
        // Один раз получает меш стандартного куба Unity через `EnsureCube`.
        EnsureCube();

        // Задаёт `blocks` значением `new GameObject("Яма").transform`.
        blocks = new GameObject("Яма").transform;
        // Привязывает к родителю через `blocks.SetParent` с аргументами `transform, false`.
        blocks.SetParent(transform, false);

        // Перебирает индексы начиная с `int ix = 0` при условии `ix < YardMetrics.Width`.
        for (int ix = 0; ix < YardMetrics.Width; ix++)
        {  // Начало цикла по индексам.
            // Перебирает индексы начиная с `int iz = 0` при условии `iz < YardMetrics.Width`.
            for (int iz = 0; iz < YardMetrics.Width; iz++)
                // Создаёт новый блок ячейки с мешем, материалом и коллайдером через `AddBlock` с аргументами `BlockKind.Grass, ResourceKind.None, ix, 0, iz`.
                AddBlock(BlockKind.Grass, ResourceKind.None, ix, 0, iz);
        }  // Конец цикла по индексам.

        // Проверяет условие `removed.Count > 0`.
        if (removed.Count > 0)
        {  // Начало ветки условия.
            // Задаёт `keys` массивом `new int[removed.Count]`.
            var keys = new int[removed.Count];
            // Копирует элементы в массив через `removed.CopyTo` с аргументами `keys`.
            removed.CopyTo(keys);
            // Перебирает индексы начиная с `int i = 0` при условии `i < keys.Length`.
            for (int i = 0; i < keys.Length; i++)
            {  // Начало цикла по индексам.
                // Вызывает `YardMetrics.Unpack` с аргументами `keys[i], out int ix, out int iy, out int iz`.
                YardMetrics.Unpack(keys[i], out int ix, out int iy, out int iz);
                // Пробует открыть грунт по шести сторонам удалённого блока через `Reveal` с аргументами `ix, iy, iz`.
                Reveal(ix, iy, iz);
            }  // Конец цикла по индексам.
        }  // Конец ветки условия.

        // Строит четыре бетонные стенки и дно раскопа через `BuildShell`.
        BuildShell();
        // Восстанавливает выкопанные предметы, которые игрок ещё не подобрал через `RestoreDrops`.
        RestoreDrops();
    }  // Конец метода `Build`.

    // Удаляет подходящие блоки в радиусе удара и возвращает их количество.
    public int Dig(Vector3 point, float radius)
    {  // Начало метода `Dig`.
        // Находит коллайдеры, пересекающие сферу удара лопаты.
        var hits = Physics.OverlapSphere(point, radius);
        // Подготавливает список блоков для удаления после отбора.
        var doomed = new System.Collections.Generic.List<DigBlock>();
        // Перебирает индексы начиная с `int i = 0` при условии `i < hits.Length`.
        for (int i = 0; i < hits.Length; i++)
        {  // Начало цикла по индексам.
            // Ищет компонент через `hits[i].GetComponent<DigBlock>` и записывает результат в `block`.
            var block = hits[i].GetComponent<DigBlock>();
            // Пропускает попадание по постороннему объекту или бетону.
            if (block == null || block.Kind == BlockKind.Concrete)
                // Пропускает остаток этой итерации и переходит к следующей.
                continue;
            // Проверяет расстояние до центра блока, поскольку коллайдер может лишь касаться сферы.
            if (Vector3.Distance(block.transform.position, point) > radius)
                // Пропускает остаток этой итерации и переходит к следующей.
                continue;
            // Добавляет элемент через `doomed.Add` с аргументами `block`.
            doomed.Add(block);
        }  // Конец цикла по индексам.

        // Перебирает индексы начиная с `int i = 0` при условии `i < doomed.Count`.
        for (int i = 0; i < doomed.Count; i++)
        {  // Начало цикла по индексам.
            // Задаёт `block` значением элемент `doomed` с индексом `i`.
            var block = doomed[i];
            // Записывает удаление ячейки под её координатным ключом.
            removed.Add(Pack(block.Ix, block.Iy, block.Iz));
            // Создаёт ещё не открытые блоки на гранях удаляемой ячейки.
            Reveal(block.Ix, block.Iy, block.Iz);
            // Выбрасывает находку, если она была в блоке, и удаляет сам блок.
            block.Release(this);
        }  // Конец цикла по индексам.

        // Проверяет, удалось ли ударом удалить хотя бы один блок.
        if (doomed.Count > 0)
        {  // Начало ветки условия.
            // Воспроизводит эффект копания через `DigFeedback.Burst` с аргументами `point`.
            DigFeedback.Burst(point);
            // Помечает прогресс к отложенному сохранению через `GameSave.MarkDirty`.
            GameSave.MarkDirty();
        }  // Конец ветки условия.

        // Проверяет, был ли этот удар первым успешным за текущий запуск раскопа.
        if (doomed.Count > 0 && !dugOnce)
        {  // Начало ветки условия.
            // Устанавливает признак первого успешного копания `dugOnce` в `true`.
            dugOnce = true;
            // Уведомляет подписчиков события первого удара, если они есть.
            FirstDig?.Invoke();
        }  // Конец ветки условия.

        // Возвращает число удалённых блоков.
        return doomed.Count;
    }  // Конец метода `Dig`.

    // Отмечает ячейку как собранную, чтобы находка не появилась повторно.
    public void RegisterLooted(int cellKey)
    {  // Начало метода `RegisterLooted`.
        // Проверяет условие `cellKey < 0`.
        if (cellKey < 0)
            // Завершает выполнение текущего метода.
            return;
        // Проверяет условие `!looted.Add(cellKey)`.
        if (!looted.Add(cellKey))
            // Завершает выполнение текущего метода.
            return;
        // Помечает прогресс к отложенному сохранению через `GameSave.MarkDirty`.
        GameSave.MarkDirty();
    }  // Конец метода `RegisterLooted`.

    // Создаёт физический подбираемый предмет из найденного ресурса.
    public void SpawnBuried(ResourceKind kind, Vector3 position, int cellKey)
    {  // Начало метода `SpawnBuried`.
        // Проверяет условие `looted.Contains(cellKey)`.
        if (looted.Contains(cellKey))
            // Прерывает создание находки, отмеченной как подобранная.
            return;

        // Создаёт объект сцены с параметрами `ResourceInfo.Title(kind)` и сохраняет ссылку в `go`.
        var go = new GameObject(ResourceInfo.Title(kind));
        // Привязывает к родителю через `go.transform.SetParent` с аргументами `blocks, true`.
        go.transform.SetParent(blocks, true);
        // Задаёт мировая позиция `go.transform.position` значением `position`.
        go.transform.position = position;
        // Выбирает значение для `size` по условию `kind` равно клад.
        float size = kind == ResourceKind.Treasure ? 0.52f : YardMetrics.Cell * 0.9f;
        // Создаёт экземпляр модели и размещает его в иерархии через `RockKit.Spawn` с аргументами `go.transform, position, kind, size` и записывает результат в `rock`.
        var rock = RockKit.Spawn(go.transform, position, kind, size);
        // Проверяет условие `rock == null`.
        if (rock == null)
        {  // Начало ветки условия.
            // Задаёт локальный масштаб `go.transform.localScale` значением вектор (1, 1, 1) `Vector3.one` умножить на `size`.
            go.transform.localScale = Vector3.one * size;
            // Добавляет компонент через `go.AddComponent<MeshFilter>` и записывает результат в `filter`.
            var filter = go.AddComponent<MeshFilter>();
            // Задаёт общий меш `filter.sharedMesh` значением `cubeMesh`.
            filter.sharedMesh = cubeMesh;
            // Добавляет компонент через `go.AddComponent<MeshRenderer>` и записывает результат в `renderer`.
            var renderer = go.AddComponent<MeshRenderer>();
            // Подбирает материал для вида ресурса через `MaterialFor` с аргументами `kind` и записывает результат в `renderer.sharedMaterial`.
            renderer.sharedMaterial = MaterialFor(kind);
            // Задаёт режим отбрасывания теней `renderer.shadowCastingMode` значением `UnityEngine.Rendering.ShadowCastingMode.Off`.
            renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        }  // Конец ветки условия.
        // Добавляет компонент через `go.AddComponent<BoxCollider>` и записывает результат в `box`.
        var box = go.AddComponent<BoxCollider>();
        // Задаёт размер `box.size` значением вектор (1, 1, 1) `Vector3.one` умножить на выбирает наибольшее значение через `Mathf.Max` с аргументами `0.12f, size`.
        box.size = Vector3.one * Mathf.Max(0.12f, size);

        // Добавляет компонент через `go.AddComponent<Rigidbody>` и записывает результат в `body`.
        var body = go.AddComponent<Rigidbody>();
        // Задаёт масса `body.mass` значением `0.4`.
        body.mass = 0.4f;
        // Добавляет компонент через `go.AddComponent<ResourcePickup>` и записывает результат в `pickup`.
        var pickup = go.AddComponent<ResourcePickup>();
        // Задаёт `pickup.Kind` значением `kind`.
        pickup.Kind = kind;
        // Задаёт `pickup.CellKey` значением `cellKey`.
        pickup.CellKey = cellKey;
        // Задаёт `pickup.Site` значением текущий экземпляр (`this`).
        pickup.Site = this;
    }  // Конец метода `SpawnBuried`.

    // Восстанавливает выкопанные предметы, которые игрок ещё не подобрал.
    void RestoreDrops()
    {  // Начало метода `RestoreDrops`.
        // Проверяет условие `removed.Count == 0`.
        if (removed.Count == 0)
            // Завершает выполнение текущего метода.
            return;
        // Задаёт `keys` массивом `new int[removed.Count]`.
        var keys = new int[removed.Count];
        // Копирует элементы в массив через `removed.CopyTo` с аргументами `keys`.
        removed.CopyTo(keys);
        // Перебирает индексы начиная с `int i = 0` при условии `i < keys.Length`.
        for (int i = 0; i < keys.Length; i++)
        {  // Начало цикла по индексам.
            // Задаёт `key` значением элемент `keys` с индексом `i`.
            int key = keys[i];
            // Проверяет условие `looted.Contains(key)`.
            if (looted.Contains(key))
                // Пропускает остаток этой итерации и переходит к следующей.
                continue;
            // Вызывает `YardMetrics.Unpack` с аргументами `key, out int ix, out int iy, out int iz`.
            YardMetrics.Unpack(key, out int ix, out int iy, out int iz);
            // Проверяет условие `iy <= 0 || iy >= YardMetrics.Layers`.
            if (iy <= 0 || iy >= YardMetrics.Layers)
                // Пропускает остаток этой итерации и переходит к следующей.
                continue;
            // По глубине и координатам определяет содержимое ячейки через `Roll` с аргументами `ix, iy, iz` и записывает результат в `kind`.
            var kind = Roll(ix, iy, iz);
            // Проверяет условие `kind == ResourceKind.None`.
            if (kind == ResourceKind.None)
                // Пропускает остаток этой итерации и переходит к следующей.
                continue;
            // Проверяет условие `kind == ResourceKind.Treasure && skipTreasure`.
            if (kind == ResourceKind.Treasure && skipTreasure)
                // Пропускает остаток этой итерации и переходит к следующей.
                continue;
            // Создаёт физический подбираемый предмет из найденного ресурса через `SpawnBuried` с аргументами `kind, YardMetrics.CellCenter(ix, iy, iz), key`.
            SpawnBuried(kind, YardMetrics.CellCenter(ix, iy, iz), key);
        }  // Конец цикла по индексам.
    }  // Конец метода `RestoreDrops`.

    // Пробует открыть грунт по шести сторонам удалённого блока.
    void Reveal(int ix, int iy, int iz)
    {  // Начало метода `Reveal`.
        // Проверяет границы ямы и создаёт грунтовую ячейку через `TryDirt` с аргументами `ix + 1, iy, iz`.
        TryDirt(ix + 1, iy, iz);
        // Проверяет границы ямы и создаёт грунтовую ячейку через `TryDirt` с аргументами `ix - 1, iy, iz`.
        TryDirt(ix - 1, iy, iz);
        // Проверяет границы ямы и создаёт грунтовую ячейку через `TryDirt` с аргументами `ix, iy + 1, iz`.
        TryDirt(ix, iy + 1, iz);
        // Проверяет границы ямы и создаёт грунтовую ячейку через `TryDirt` с аргументами `ix, iy - 1, iz`.
        TryDirt(ix, iy - 1, iz);
        // Проверяет границы ямы и создаёт грунтовую ячейку через `TryDirt` с аргументами `ix, iy, iz + 1`.
        TryDirt(ix, iy, iz + 1);
        // Проверяет границы ямы и создаёт грунтовую ячейку через `TryDirt` с аргументами `ix, iy, iz - 1`.
        TryDirt(ix, iy, iz - 1);
    }  // Конец метода `Reveal`.

    // Пробует открыть ещё одну грунтовую ячейку после удаления соседнего блока.
    void TryDirt(int ix, int iy, int iz)
    {  // Начало метода `TryDirt`.
        // Ограничивает генерацию подземными слоями от первого до последнего.
        if (iy < 1 || iy >= YardMetrics.Layers)
            // Завершает выполнение текущего метода.
            return;
        // Проверяет границы раскопа по горизонтальным осям.
        if (ix < 0 || iz < 0 || ix >= YardMetrics.Width || iz >= YardMetrics.Width)
            // Завершает выполнение текущего метода.
            return;
        // Создаёт новый блок ячейки с мешем, материалом и коллайдером через `AddBlock` с аргументами `BlockKind.Dirt, Roll(ix, iy, iz), ix, iy, iz`.
        AddBlock(BlockKind.Dirt, Roll(ix, iy, iz), ix, iy, iz);
    }  // Конец метода `TryDirt`.

    // Строит четыре бетонные стенки и дно раскопа.
    void BuildShell()
    {  // Начало метода `BuildShell`.
        // Задаёт `c` значением `YardMetrics.Cell`.
        float c = YardMetrics.Cell;
        // Вычисляет `pit` как `YardMetrics.Width` умножить на `c`.
        float pit = YardMetrics.Width * c;
        // Вычисляет `thick` как `c` умножить на `1.08`.
        float thick = c * 1.08f;
        // Вычисляет `height` как (`YardMetrics.Layers` плюс `1`) умножить на `c`.
        float height = (YardMetrics.Layers + 1) * c;
        // Вычисляет `mid` как `pit` умножить на `0.5`.
        float mid = pit * 0.5f;
        // Вычисляет `y` как `-height` умножить на `0.5`.
        float y = -height * 0.5f;
        // Создаёт бетонный прямоугольный элемент заданных размеров через `ShellWall` с аргументами `new Vector3(-c * 0.5f, y, mid - c * 0.5f), new Vector3(thick, height, pit + 2f * c)`.
        ShellWall(new Vector3(-c * 0.5f, y, mid - c * 0.5f), new Vector3(thick, height, pit + 2f * c));
        // Создаёт бетонный прямоугольный элемент заданных размеров через `ShellWall` с аргументами `new Vector3(pit + c * 0.5f, y, mid - c * 0.5f), new Vector3(thick, height, pit + 2f * c)`.
        ShellWall(new Vector3(pit + c * 0.5f, y, mid - c * 0.5f), new Vector3(thick, height, pit + 2f * c));
        // Создаёт бетонный прямоугольный элемент заданных размеров через `ShellWall` с аргументами `new Vector3(mid, y, -c * 0.5f), new Vector3(pit, height, thick)`.
        ShellWall(new Vector3(mid, y, -c * 0.5f), new Vector3(pit, height, thick));
        // Создаёт бетонный прямоугольный элемент заданных размеров через `ShellWall` с аргументами `new Vector3(mid, y, pit + c * 0.5f), new Vector3(pit, height, thick)`.
        ShellWall(new Vector3(mid, y, pit + c * 0.5f), new Vector3(pit, height, thick));
        // Создаёт бетонный прямоугольный элемент заданных размеров через `ShellWall` с аргументами `new Vector3(mid, -YardMetrics.Layers * c - c * 0.5f, mid), new Vector3(pit + 2f * c, thick, pit + 2f * c)`.
        ShellWall(new Vector3(mid, -YardMetrics.Layers * c - c * 0.5f, mid), new Vector3(pit + 2f * c, thick, pit + 2f * c));
    }  // Конец метода `BuildShell`.

    // Создаёт бетонный прямоугольный элемент заданных размеров.
    void ShellWall(Vector3 position, Vector3 scale)
    {  // Начало метода `ShellWall`.
        // Создаёт стандартный примитив через `GameObject.CreatePrimitive` с аргументами `PrimitiveType.Cube` и записывает результат в `go`.
        var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
        // Задаёт имя `go.name` значением `"Бетон"`.
        go.name = "Бетон";
        // Привязывает к родителю через `go.transform.SetParent` с аргументами `blocks, false`.
        go.transform.SetParent(blocks, false);
        // Задаёт мировая позиция `go.transform.position` значением `position`.
        go.transform.position = position;
        // Задаёт локальный масштаб `go.transform.localScale` значением `scale`.
        go.transform.localScale = scale;
        // Ищет компонент через `go.GetComponent<MeshRenderer>` и записывает результат в `renderer`.
        var renderer = go.GetComponent<MeshRenderer>();
        // Задаёт общий материал `renderer.sharedMaterial` значением `concreteMat`.
        renderer.sharedMaterial = concreteMat;
        // Задаёт режим отбрасывания теней `renderer.shadowCastingMode` значением `UnityEngine.Rendering.ShadowCastingMode.Off`.
        renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        // Устанавливает приём теней `renderer.receiveShadows` в `false`.
        renderer.receiveShadows = false;
    }  // Конец метода `ShellWall`.

    // Создаёт новый блок ячейки с мешем, материалом и коллайдером.
    void AddBlock(BlockKind kind, ResourceKind buried, int ix, int iy, int iz)
    {  // Начало метода `AddBlock`.
        // Преобразует координаты ячейки в целочисленный ключ через `Pack` с аргументами `ix, iy, iz` и записывает результат в `key`.
        int key = Pack(ix, iy, iz);
        // Пропускает выкопанные ячейки и те, которые уже были созданы.
        if (removed.Contains(key) || !spawned.Add(key))
            // Завершает выполнение текущего метода.
            return;

        // Создаёт объект сцены с параметрами `kind == BlockKind.Concrete ? "Бетон" : kind == BlockKind.Grass ? "Трава" : "Земля"` и сохраняет ссылку в `go`.
        var go = new GameObject(kind == BlockKind.Concrete ? "Бетон" : kind == BlockKind.Grass ? "Трава" : "Земля");
        // Привязывает к родителю через `go.transform.SetParent` с аргументами `blocks, false`.
        go.transform.SetParent(blocks, false);
        // Вызывает `YardMetrics.CellCenter` с аргументами `ix, iy, iz` и записывает результат в `go.transform.position`.
        go.transform.position = YardMetrics.CellCenter(ix, iy, iz);
        // Вычисляет `size` как `YardMetrics.Cell` умножить на (`1.012`, если `kind` равно `BlockKind.Concrete`, иначе `1.004`).
        float size = YardMetrics.Cell * (kind == BlockKind.Concrete ? 1.012f : 1.004f);
        // Задаёт локальный масштаб `go.transform.localScale` значением вектор (1, 1, 1) `Vector3.one` умножить на `size`.
        go.transform.localScale = Vector3.one * size;
        // Добавляет компонент через `go.AddComponent<MeshFilter>` и записывает результат в `filter`.
        var filter = go.AddComponent<MeshFilter>();
        // Задаёт общий меш `filter.sharedMesh` значением `cubeMesh`.
        filter.sharedMesh = cubeMesh;
        // Добавляет компонент через `go.AddComponent<MeshRenderer>` и записывает результат в `renderer`.
        var renderer = go.AddComponent<MeshRenderer>();
        // Выбирает общий материал `renderer.sharedMaterial` по условию `kind` равно трава.
        renderer.sharedMaterial = kind == BlockKind.Grass
            // При истинном условии выбирает `crossGrassMat`.
            ? (YardMetrics.IsCrossGrass(ix, iy, iz) ? crossGrassMat : grassMat)
            // При ложном условии выбирает `concreteMat`.
            : kind == BlockKind.Dirt ? dirtMat : concreteMat;
        // Задаёт режим отбрасывания теней `renderer.shadowCastingMode` значением `UnityEngine.Rendering.ShadowCastingMode.Off`.
        renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        // Устанавливает приём теней `renderer.receiveShadows` в `false`.
        renderer.receiveShadows = false;
        // Добавляет компонент через `go.AddComponent<BoxCollider>`.
        go.AddComponent<BoxCollider>();
        // Добавляет компонент через `go.AddComponent<DigBlock>` и записывает результат в `block`.
        var block = go.AddComponent<DigBlock>();
        // Задаёт `block.Kind` значением `kind`.
        block.Kind = kind;
        // Задаёт `block.Buried` значением `buried`.
        block.Buried = buried;
        // Задаёт `block.Ix` значением индекс ячейки по X `ix`.
        block.Ix = ix;
        // Задаёт `block.Iy` значением индекс слоя по глубине `iy`.
        block.Iy = iy;
        // Задаёт `block.Iz` значением индекс ячейки по Z `iz`.
        block.Iz = iz;
    }  // Конец метода `AddBlock`.

    // По глубине и координатам определяет содержимое ячейки.
    ResourceKind Roll(int ix, int iy, int iz)
    {  // Начало метода `Roll`.
        // Проверяет, совпала ли ячейка с точным местом клада на дне.
        if (iy == YardMetrics.Layers - 1 && ix == treasureIx && iz == treasureIz)
            // Назначает клад этой ячейке независимо от таблицы обычной добычи.
            return ResourceKind.Treasure;

        // Получает воспроизводимое значение от 0 до 99 из координат ячейки.
        int roll = Hash(ix, iy, iz) % 100;
        // Проверяет условие `iy <= 6`.
        if (iy <= 6)
            // Назначает камень при значении от 0 до 11 на глубине до шестого слоя.
            return roll < 12 ? ResourceKind.Stone : ResourceKind.None;
        // Проверяет условие `iy <= 12`.
        if (iy <= 12)
        {  // Начало ветки условия.
            // На слоях 7–12 назначает камень при значении от 0 до 7.
            if (roll < 8) return ResourceKind.Stone;
            // На тех же слоях назначает уголь при значении от 8 до 15.
            if (roll < 16) return ResourceKind.Coal;
            // Возвращает отсутствие ресурса.
            return ResourceKind.None;
        }  // Конец ветки условия.
        // Проверяет условие `iy <= 18`.
        if (iy <= 18)
        {  // Начало ветки условия.
            // На слоях 13–18 назначает уголь при значении от 0 до 5.
            if (roll < 6) return ResourceKind.Coal;
            // На тех же слоях назначает медь при значении от 6 до 11.
            if (roll < 12) return ResourceKind.Copper;
            // Возвращает отсутствие ресурса.
            return ResourceKind.None;
        }  // Конец ветки условия.
        // Проверяет условие `iy <= 24`.
        if (iy <= 24)
        {  // Начало ветки условия.
            // На слоях 19–24 назначает медь при значении от 0 до 4.
            if (roll < 5) return ResourceKind.Copper;
            // На тех же слоях назначает железо при значении от 5 до 9.
            if (roll < 10) return ResourceKind.Iron;
            // Возвращает отсутствие ресурса.
            return ResourceKind.None;
        }  // Конец ветки условия.
        // Проверяет условие `iy <= 28`.
        if (iy <= 28)
        {  // Начало ветки условия.
            // На слоях 25–28 назначает железо при значении от 0 до 3.
            if (roll < 4) return ResourceKind.Iron;
            // На тех же слоях назначает золото при значении от 4 до 6.
            if (roll < 7) return ResourceKind.Gold;
            // Возвращает отсутствие ресурса.
            return ResourceKind.None;
        }  // Конец ветки условия.

        // На оставшихся глубоких слоях назначает золото при значении от 0 до 3.
        return roll < 4 ? ResourceKind.Gold : ResourceKind.None;
    }  // Конец метода `Roll`.

    // Вычисляет воспроизводимое неотрицательное число из трёх координат.
    static int Hash(int x, int y, int z)
    {  // Начало метода `Hash`.
        // Разрешает переполнение целых чисел в расчёте хеша.
        unchecked
        {  // Начало блока расчётов с допустимым переполнением.
            // Смешивает индексы ячейки, умножая каждую координату на свою константу.
            int h = x * 73856093 ^ y * 19349663 ^ z * 83492791;
            // Дополнительно перемешивает биты числа сдвигом и операцией XOR.
            h = (h << 13) ^ h;
            // Обнуляет знаковый бит, чтобы результат хеша был неотрицательным.
            return h & 0x7fffffff;
        }  // Конец блока расчётов с допустимым переполнением.
    }  // Конец метода `Hash`.

    // Подбирает материал для вида ресурса.
    Material MaterialFor(ResourceKind kind)
    {  // Начало метода `MaterialFor`.
        // Выбирает ветку по значению `kind`.
        switch (kind)
        {  // Начало выбора по значению.
            // Обрабатывает значение камень. Возвращает `stoneMat`.
            case ResourceKind.Stone: return stoneMat;
            // Обрабатывает значение уголь. Возвращает `coalMat`.
            case ResourceKind.Coal: return coalMat;
            // Обрабатывает значение медь. Возвращает `copperMat`.
            case ResourceKind.Copper: return copperMat;
            // Обрабатывает значение железо. Возвращает `ironMat`.
            case ResourceKind.Iron: return ironMat;
            // Обрабатывает значение золото. Возвращает `goldMat`.
            case ResourceKind.Gold: return goldMat;
            // Обрабатывает значение клад. Возвращает `treasureMat`.
            case ResourceKind.Treasure: return treasureMat;
            // Обрабатывает все остальные значения. Возвращает `dirtMat`.
            default: return dirtMat;
        }  // Конец выбора по значению.
    }  // Конец метода `MaterialFor`.

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

    // Один раз получает меш стандартного куба Unity.
    static void EnsureCube()
    {  // Начало метода `EnsureCube`.
        // Проверяет условие `cubeMesh != null`.
        if (cubeMesh != null)
            // Завершает выполнение текущего метода.
            return;
        // Создаёт стандартный примитив через `GameObject.CreatePrimitive` с аргументами `PrimitiveType.Cube` и записывает результат в `temp`.
        var temp = GameObject.CreatePrimitive(PrimitiveType.Cube);
        // Задаёт `cubeMesh` значением `temp.GetComponent<MeshFilter>().sharedMesh`.
        cubeMesh = temp.GetComponent<MeshFilter>().sharedMesh;
        // Проверяет условие `Application.isPlaying`.
        if (Application.isPlaying)
            // Планирует удаление объекта через `Destroy` с аргументами `temp`.
            Destroy(temp);
        // Обрабатывает случай, когда предыдущее условие ложно.
        else
            // Сразу удаляет объект через `DestroyImmediate` с аргументами `temp`.
            DestroyImmediate(temp);
    }  // Конец метода `EnsureCube`.
}  // Конец класса `DigSite`.
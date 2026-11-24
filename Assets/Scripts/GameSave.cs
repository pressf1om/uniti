// Подключает доступ к основным типам .NET.
using System;
// Подключает доступ к чтению и записи файлов.
using System.IO;
// Подключает доступ к базовым типам игрового движка.
using UnityEngine;

// Передаёт в игровую сцену признак продолжения сохранённой игры.
public static class PlaySession
{  // Начало класса `PlaySession`.
    // Хранит логический флаг в `Continue`.
    public static bool Continue;
}  // Конец класса `PlaySession`.

// Разрешает сериализацию этого типа.
[Serializable]
// Описывает поля локального JSON-сохранения.
public class GameSaveData
{  // Начало класса `GameSaveData`.
    // Хранит баланс монет `money`.
    public int money;
    // Задаёт вместимость рюкзака `capacity` значением `5`.
    public int capacity = 5;
    // Хранит уровень лопаты `shovelTier`.
    public int shovelTier;
    // Хранит признак наличия лопаты `hasShovel`.
    public bool hasShovel;
    // Хранит старый признак покупки двойного прыжка `doubleJumpBoots`.
    public bool doubleJumpBoots;
    // Хранит уровень дополнительных прыжков `jumpBootsLevel`.
    public int jumpBootsLevel;
    // Хранит количество камня `stone`.
    public int stone;
    // Хранит количество угля `coal`.
    public int coal;
    // Хранит количество меди `copper`.
    public int copper;
    // Хранит количество железа `iron`.
    public int iron;
    // Хранит количество золота `gold`.
    public int gold;
    // Задаёт ключи выкопанных ячеек `dug` массивом `new int[0]`.
    public int[] dug = new int[0];
    // Задаёт ключи ячеек с подобранными находками `looted` массивом `new int[0]`.
    public int[] looted = new int[0];
    // Хранит признак взятого клада `treasureTaken`.
    public bool treasureTaken;
    // Задаёт индекс клада по X `treasureIx` значением `-1`.
    public int treasureIx = -1;
    // Задаёт индекс клада по Z `treasureIz` значением `-1`.
    public int treasureIz = -1;
    // Задаёт позиция игрока по X `px` значением `5`.
    public float px = 5f;
    // Задаёт позиция игрока по Y `py` значением `0.2`.
    public float py = 0.2f;
    // Задаёт позиция игрока по Z `pz` значением `-3.2f`.
    public float pz = -3.2f;
    // Хранит поворот вокруг вертикальной оси `yaw`.
    public float yaw;
}  // Конец класса `GameSaveData`.

// Читает и записывает локальное сохранение и управляет частотой автозаписи.
public static class GameSave
{  // Начало класса `GameSave`.
    // Объявляет свойство путь к файлу сохранения `FilePath` и правила доступа к нему.
    static string FilePath
    {  // Начало свойства и его методов доступа.
        // Строит путь к treasure_hunting.json внутри каталога постоянных данных приложения.
        get { return Path.Combine(Application.persistentDataPath, "treasure_hunting.json"); }
    }  // Конец свойства и его методов доступа.

    // Объявляет свойство `Exists` и правила доступа к нему.
    public static bool Exists
    {  // Начало свойства и его методов доступа.
        // Описывает чтение свойства. Возвращает результат вызова `File.Exists(FilePath)`.
        get { return File.Exists(FilePath); }
    }  // Конец свойства и его методов доступа.

    // Загружает данные из JSON, если файл существует.
    public static GameSaveData Read()
    {  // Начало метода `Read`.
        // Проверяет условие `!Exists`.
        if (!Exists)
            // Возвращает `null`, то есть отсутствие результата поиска.
            return null;
        // Возвращает результат вызова `JsonUtility.FromJson<GameSaveData>(File.ReadAllText(FilePath))`.
        return JsonUtility.FromJson<GameSaveData>(File.ReadAllText(FilePath));
    }  // Конец метода `Read`.

    // Удаляет локальное сохранение, если оно существует.
    public static void Delete()
    {  // Начало метода `Delete`.
        // Проверяет условие `Exists`.
        if (Exists)
            // Удаляет файл через `File.Delete` с аргументами `FilePath`.
            File.Delete(FilePath);
    }  // Конец метода `Delete`.

    // Хранит признак несохранённых изменений `dirty`.
    static bool dirty;
    // Хранит ближайшее время следующей записи `nextFlush`.
    static float nextFlush;

    // Помечает прогресс для отложенной записи.
    public static void MarkDirty()
    {  // Начало метода `MarkDirty`.
        // Устанавливает признак несохранённых изменений `dirty` в `true`.
        dirty = true;
    }  // Конец метода `MarkDirty`.

    // Записывает накопленные изменения с ограничением частоты.
    public static void FlushPending()
    {  // Начало метода `FlushPending`.
        // Откладывает запись, если нет изменений или ещё не истёк интервал.
        if (!dirty || Time.unscaledTime < nextFlush)
            // Завершает выполнение текущего метода.
            return;
        // Устанавливает признак несохранённых изменений `dirty` в `false`.
        dirty = false;
        // Назначает следующую допустимую отложенную запись через 0,4 секунды реального времени.
        nextFlush = Time.unscaledTime + 0.4f;
        // Сохраняет текущий игровой прогресс через `WriteCurrent`.
        WriteCurrent();
    }  // Конец метода `FlushPending`.

    // Находит игрока и раскоп на текущей сцене и сохраняет их состояние.
    public static void WriteCurrent()
    {  // Начало метода `WriteCurrent`.
        // Устанавливает признак несохранённых изменений `dirty` в `false`.
        dirty = false;
        // Ищет любой объект типа через `UnityEngine.Object.FindAnyObjectByType<FirstPersonPlayer>` и записывает результат в `player`.
        var player = UnityEngine.Object.FindAnyObjectByType<FirstPersonPlayer>();
        // Ищет любой объект типа через `UnityEngine.Object.FindAnyObjectByType<DigSite>` и записывает результат в `site`.
        var site = UnityEngine.Object.FindAnyObjectByType<DigSite>();
        // Проверяет условие `player == null || site == null`.
        if (player == null || site == null)
            // Завершает выполнение текущего метода.
            return;
        // Формирует данные сохранения из переданных компонентов через `Write` с аргументами `player, site`.
        Write(player, site);
    }  // Конец метода `WriteCurrent`.

    // Формирует данные сохранения из переданных компонентов.
    public static void Write(FirstPersonPlayer player, DigSite site)
    {  // Начало метода `Write`.
        // Собирает состояние игрока, параметры клада и ключи изменённых ячеек.
        var data = player.Export(site);
        // Преобразует данные в JSON и записывает их на диск через `WriteData` с аргументами `data`.
        WriteData(data);
    }  // Конец метода `Write`.

    // Преобразует данные в JSON и записывает их на диск.
    public static void WriteData(GameSaveData data)
    {  // Начало метода `WriteData`.
        // Проверяет условие `data == null`.
        if (data == null)
            // Завершает выполнение текущего метода.
            return;
        // Преобразует снимок прогресса в JSON и целиком записывает файл.
        File.WriteAllText(FilePath, JsonUtility.ToJson(data));
    }  // Конец метода `WriteData`.

    // Добавляет монеты в текущую игру или непосредственно в файл сохранения.
    public static bool AddMoney(int amount)
    {  // Начало метода `AddMoney`.
        // Проверяет условие `amount <= 0`.
        if (amount <= 0)
            // Возвращает `false`.
            return false;
        // Ищет любой объект типа через `UnityEngine.Object.FindAnyObjectByType<FirstPersonPlayer>` и записывает результат в `player`.
        var player = UnityEngine.Object.FindAnyObjectByType<FirstPersonPlayer>();
        // Проверяет условие `player != null`.
        if (player != null)
        {  // Начало ветки условия.
            // Добавляет `amount` к текущему значению `player.Bag.Money`.
            player.Bag.Money += amount;
            // Сохраняет пополненный баланс активной партии.
            WriteCurrent();
            // Возвращает `true`.
            return true;
        }  // Конец ветки условия.

        // Проверяет условие `!Exists`.
        if (!Exists)
            // Возвращает `false`.
            return false;
        // Загружает данные из JSON, если файл существует через `Read` и записывает результат в `data`.
        var data = Read();
        // Проверяет условие `data == null`.
        if (data == null)
            // Возвращает `false`.
            return false;
        // Начисляет `amount` монет.
        data.money += amount;
        // Преобразует данные в JSON и записывает их на диск через `WriteData` с аргументами `data`.
        WriteData(data);
        // Возвращает `true`.
        return true;
    }  // Конец метода `AddMoney`.
}  // Конец класса `GameSave`.

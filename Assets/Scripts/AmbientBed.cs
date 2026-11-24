// Подключает доступ к базовым типам игрового движка.
using UnityEngine;

// Управляет фоновыми звуками двора и меняет подложку плавным переходом.
public class AmbientBed : MonoBehaviour
{  // Начало класса `AmbientBed`.
    // Хранит поле типа `AudioSource[]` в `music`.
    AudioSource[] music;
    // Хранит массив звуковых клипов в `beds`.
    AudioClip[] beds;
    // Хранит целое число в `index`.
    int index;
    // Хранит целое число в `playing`.
    int playing;
    // Хранит вещественное число в `nextSwitch`.
    float nextSwitch;
    // Хранит вещественное число в `fade`.
    float fade;

    // Создаёт дочерний объект с источниками фонового звука.
    public static void Attach(Transform parent)
    {  // Начало метода `Attach`.
        // Создаёт объект сцены с параметрами `"Атмосфера"` и сохраняет ссылку в `go`.
        var go = new GameObject("Атмосфера");
        // Привязывает к родителю через `go.transform.SetParent` с аргументами `parent, false`.
        go.transform.SetParent(parent, false);
        // Добавляет компонент через `go.AddComponent<AmbientBed>`.
        go.AddComponent<AmbientBed>();
    }  // Конец метода `Attach`.

    // Загружает звуки окружения и настраивает источники.
    void Awake()
    {  // Начало метода `Awake`.
        // Присваивает `beds` результат `Collect( Resources.Load<AudioClip>("music_explore"), Resources.Load<AudioClip>("music_crystal"), Resources.Load<AudioClip>("music_soft"))`.
        beds = Collect(
            // Передаёт аргумент `Resources.Load<AudioClip>("music_explore")`.
            Resources.Load<AudioClip>("music_explore"),
            // Передаёт аргумент `Resources.Load<AudioClip>("music_crystal")`.
            Resources.Load<AudioClip>("music_crystal"),
            // Передаёт аргумент `Resources.Load<AudioClip>("music_soft")`.
            Resources.Load<AudioClip>("music_soft"));
        // Проверяет условие `beds == null || beds.Length == 0`.
        if (beds == null || beds.Length == 0)
        {  // Начало ветки условия.
            // Отбрасывает отсутствующие звуковые клипы через `Collect` с аргументами ` Resources.Load<AudioClip>("music_a"), Resources.Load<AudioClip>("music_b")` и записывает результат в `beds`.
            beds = Collect(
                // Передаёт аргумент `Resources.Load<AudioClip>("music_a")`.
                Resources.Load<AudioClip>("music_a"),
                // Передаёт аргумент `Resources.Load<AudioClip>("music_b")`.
                Resources.Load<AudioClip>("music_b"));
        }  // Конец ветки условия.
        // Задаёт `music` значением массив `new[] { Source(0f, true), Source(0f, true) }`.
        music = new[] { Source(0f, true), Source(0f, true) };
        // Загружает ресурс через `Resources.Load<AudioClip>` с аргументами `"birds_real"` и записывает результат в `birdClip`.
        var birdClip = Resources.Load<AudioClip>("birds_real");
        // Проверяет условие `birdClip == null`.
        if (birdClip == null)
            // Загружает ресурс через `Resources.Load<AudioClip>` с аргументами `"birds"` и записывает результат в `birdClip`.
            birdClip = Resources.Load<AudioClip>("birds");
        // Создаёт источник заданного звука в точке двора через `PlaceSource` с аргументами `"Птицы", new Vector3(4f, 3.2f, 8f), birdClip, 0.1f, 0.32f` и записывает результат в `birds`.
        var birds = PlaceSource("Птицы", new Vector3(4f, 3.2f, 8f), birdClip, 0.1f, 0.32f);
        // Создаёт источник заданного звука в точке двора через `PlaceSource` с аргументами `"Машины", new Vector3(2f, 1.4f, -10.2f), Resources.Load<AudioClip>("traffic"), 0.14f, 0.55f` и записывает результат в `cars`.
        var cars = PlaceSource("Машины", new Vector3(2f, 1.4f, -10.2f), Resources.Load<AudioClip>("traffic"), 0.14f, 0.55f);
        // Проверяет условие `birds != null`. Запускает воспроизведение через `birds.Play`, если условие выполнено.
        if (birds != null) birds.Play();
        // Проверяет условие `cars != null`. Запускает воспроизведение через `cars.Play`, если условие выполнено.
        if (cars != null) cars.Play();
        // Включает очередную фоновую дорожку и задаёт режим перехода через `PlayBed` с аргументами `0, true`.
        PlayBed(0, true);
        // Применяет громкость звука через `GameSettings.Apply`.
        GameSettings.Apply();
    }  // Конец метода `Awake`.

    // Переключает фоновый клип и плавно меняет громкость источников.
    void Update()
    {  // Начало метода `Update`.
        // Проверяет условие `fade < 1f`.
        if (fade < 1f)
        {  // Начало ветки условия.
            // Приближает значение к цели через `Mathf.MoveTowards` с аргументами `fade, 1f, Time.unscaledDeltaTime / 4f` и записывает результат в `fade`.
            fade = Mathf.MoveTowards(fade, 1f, Time.unscaledDeltaTime / 4f);
            // Вычисляет `other` как `1` минус `playing`.
            int other = 1 - playing;
            // Задаёт громкость `music[playing].volume` значением `0.28` умножить на `fade`.
            music[playing].volume = 0.28f * fade;
            // Задаёт громкость `music[other].volume` значением `0.28` умножить на (`1` минус `fade`).
            music[other].volume = 0.28f * (1f - fade);
            // Проверяет условие `fade >= 1f`.
            if (fade >= 1f)
                // Останавливает воспроизведение через `music[other].Stop`.
                music[other].Stop();
        }  // Конец ветки условия.

        // Проверяет условие `Time.unscaledTime >= nextSwitch`.
        if (Time.unscaledTime >= nextSwitch)
            // Включает очередную фоновую дорожку и задаёт режим перехода через `PlayBed` с аргументами `index + 1, false`.
            PlayBed(index + 1, false);
    }  // Конец метода `Update`.

    // Включает очередную фоновую дорожку и задаёт режим перехода.
    void PlayBed(int next, bool instant)
    {  // Начало метода `PlayBed`.
        // Проверяет условие `beds == null || beds.Length == 0`.
        if (beds == null || beds.Length == 0)
            // Завершает выполнение текущего метода.
            return;
        // Задаёт `index` значением ((`next % beds.Length`) плюс `beds.Length`) по модулю `beds.Length`.
        index = ((next % beds.Length) + beds.Length) % beds.Length;
        // Задаёт `clip` значением элемент `beds` с индексом `index`.
        var clip = beds[index];
        // Проверяет условие `clip == null`.
        if (clip == null)
            // Завершает выполнение текущего метода.
            return;
        // Выбирает значение для `other` по условию `instant`.
        int other = instant ? playing : 1 - playing;
        // Задаёт звуковой клип `music[other].clip` значением `clip`.
        music[other].clip = clip;
        // Устанавливает зацикливание `music[other].loop` в `true`.
        music[other].loop = true;
        // Выбирает громкость `music[other].volume` по условию `instant`.
        music[other].volume = instant ? 0.28f : 0f;
        // Запускает воспроизведение через `music[other].Play`.
        music[other].Play();
        // Задаёт `playing` значением `other`.
        playing = other;
        // Выбирает `fade` по условию `instant`.
        fade = instant ? 1f : 0f;
        // Выбирает наибольшее значение через `Mathf.Max` с аргументами `clip.length - 4f, 110f` и записывает результат в `hold`.
        float hold = Mathf.Max(clip.length - 4f, 110f);
        // Задаёт `nextSwitch` значением время без учёта паузы `Time.unscaledTime` плюс `hold`.
        nextSwitch = Time.unscaledTime + hold;
    }  // Конец метода `PlayBed`.

    // Отбрасывает отсутствующие звуковые клипы.
    static AudioClip[] Collect(params AudioClip[] clips)
    {  // Начало метода `Collect`.
        // Задаёт `count` значением `0`.
        int count = 0;
        // Перебирает индексы начиная с `int i = 0` при условии `i < clips.Length`.
        for (int i = 0; i < clips.Length; i++)
        {  // Начало цикла по индексам.
            // Проверяет условие `clips[i] != null`.
            if (clips[i] != null)
                // Увеличивает `count` на единицу.
                count++;
        }  // Конец цикла по индексам.
        // Задаёт `result` массивом `new AudioClip[count]`.
        var result = new AudioClip[count];
        // Задаёт `w` значением `0`.
        int w = 0;
        // Перебирает индексы начиная с `int i = 0` при условии `i < clips.Length`.
        for (int i = 0; i < clips.Length; i++)
        {  // Начало цикла по индексам.
            // Проверяет условие `clips[i] != null`.
            if (clips[i] != null)
                // Задаёт `result[w++]` значением элемент `clips` с индексом `i`.
                result[w++] = clips[i];
        }  // Конец цикла по индексам.
        // Возвращает `result`.
        return result;
    }  // Конец метода `Collect`.

    // Создаёт источник заданного звука в точке двора.
    AudioSource PlaceSource(string name, Vector3 local, AudioClip clip, float volume, float spatial)
    {  // Начало метода `PlaceSource`.
        // Проверяет условие `clip == null`.
        if (clip == null)
            // Возвращает `null`, то есть отсутствие результата поиска.
            return null;
        // Создаёт объект сцены с параметрами `name` и сохраняет ссылку в `go`.
        var go = new GameObject(name);
        // Привязывает к родителю через `go.transform.SetParent` с аргументами `transform, false`.
        go.transform.SetParent(transform, false);
        // Задаёт локальная позиция `go.transform.localPosition` значением `local`.
        go.transform.localPosition = local;
        // Добавляет компонент через `go.AddComponent<AudioSource>` и записывает результат в `source`.
        var source = go.AddComponent<AudioSource>();
        // Задаёт звуковой клип `source.clip` значением `clip`.
        source.clip = clip;
        // Устанавливает зацикливание `source.loop` в `true`.
        source.loop = true;
        // Устанавливает автозапуск звука `source.playOnAwake` в `false`.
        source.playOnAwake = false;
        // Задаёт громкость `source.volume` значением `volume`.
        source.volume = volume;
        // Задаёт доля пространственного звука `source.spatialBlend` значением `spatial`.
        source.spatialBlend = spatial;
        // Задаёт минимальная дистанция звука `source.minDistance` значением `6`.
        source.minDistance = 6f;
        // Задаёт максимальная дистанция звука `source.maxDistance` значением `48`.
        source.maxDistance = 48f;
        // Возвращает `source`.
        return source;
    }  // Конец метода `PlaceSource`.

    // Создаёт источник с заданной громкостью и режимом повторения.
    AudioSource Source(float volume, bool loop)
    {  // Начало метода `Source`.
        // Добавляет компонент через `gameObject.AddComponent<AudioSource>` и записывает результат в `source`.
        var source = gameObject.AddComponent<AudioSource>();
        // Устанавливает автозапуск звука `source.playOnAwake` в `false`.
        source.playOnAwake = false;
        // Задаёт зацикливание `source.loop` значением `loop`.
        source.loop = loop;
        // Задаёт громкость `source.volume` значением `volume`.
        source.volume = volume;
        // Задаёт доля пространственного звука `source.spatialBlend` значением `0`.
        source.spatialBlend = 0f;
        // Возвращает `source`.
        return source;
    }  // Конец метода `Source`.
}  // Конец класса `AmbientBed`.

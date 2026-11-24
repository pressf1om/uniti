// Подключает доступ к базовым типам игрового движка.
using UnityEngine;

// Воспроизводит звук и частицы раскопок и звук получения монет.
public class DigFeedback : MonoBehaviour
{  // Начало класса `DigFeedback`.
    // Объявляет свойство `Instance` и правила доступа к нему.
    public static DigFeedback Instance { get; private set; }

    // Хранит ссылку на источник звука в `source`.
    AudioSource source;
    // Хранит ссылку на звуковой клип в `digClip`.
    AudioClip digClip;
    // Хранит ссылку на звуковой клип в `coinClip`.
    AudioClip coinClip;

    // Запоминает компонент звуковых эффектов и подготавливает клипы.
    void Awake()
    {  // Начало метода `Awake`.
        // Задаёт `Instance` значением текущий экземпляр (`this`).
        Instance = this;
        // Добавляет компонент через `gameObject.AddComponent<AudioSource>` и записывает результат в `source`.
        source = gameObject.AddComponent<AudioSource>();
        // Устанавливает автозапуск звука `source.playOnAwake` в `false`.
        source.playOnAwake = false;
        // Задаёт доля пространственного звука `source.spatialBlend` значением `0`.
        source.spatialBlend = 0f;
        // Загружает ресурс через `Resources.Load<AudioClip>` с аргументами `"dig"` и записывает результат в `digClip`.
        digClip = Resources.Load<AudioClip>("dig");
        // Загружает ресурс через `Resources.Load<AudioClip>` с аргументами `"coin"` и записывает результат в `coinClip`.
        coinClip = Resources.Load<AudioClip>("coin");
        // Применяет громкость звука через `GameSettings.Apply`.
        GameSettings.Apply();
    }  // Конец метода `Awake`.

    // Запускает эффект удара лопатой в указанной точке.
    public static void Burst(Vector3 point)
    {  // Начало метода `Burst`.
        // Воспроизводит эффект копания через `Burst` с аргументами `point, 0.8f`.
        Burst(point, 0.8f);
    }  // Конец метода `Burst`.

    // Запускает эффект удара лопатой в указанной точке.
    public static void Burst(Vector3 point, float life)
    {  // Начало метода `Burst`.
        // Создаёт объект сцены с параметрами `"Пыль"` и сохраняет ссылку в `go`.
        var go = new GameObject("Пыль");
        // Меняет активность объекта через `go.SetActive` с аргументами `false`.
        go.SetActive(false);
        // Задаёт мировая позиция `go.transform.position` значением `point` плюс `Vector3.up` умножить на `0.12`.
        go.transform.position = point + Vector3.up * 0.12f;
        // Добавляет компонент через `go.AddComponent<ParticleSystem>` и записывает результат в `particles`.
        var particles = go.AddComponent<ParticleSystem>();
        // Задаёт `main` значением `particles.main`.
        var main = particles.main;
        // Устанавливает автозапуск звука `main.playOnAwake` в `false`.
        main.playOnAwake = false;
        // Задаёт `main.duration` значением `0.35`.
        main.duration = 0.35f;
        // Устанавливает зацикливание `main.loop` в `false`.
        main.loop = false;
        // Задаёт `main.startLifetime` значением `life`.
        main.startLifetime = life;
        // Задаёт `main.startSpeed` значением `0.9`.
        main.startSpeed = 0.9f;
        // Задаёт `main.startSize` значением `0.2`.
        main.startSize = 0.2f;
        // Задаёт `dust` значением `Palette.Dirt`.
        var dust = Palette.Dirt;
        // Записывает в `main.startColor` цвет RGBA с параметрами `dust.r, dust.g, dust.b, 0.7f`.
        main.startColor = new Color(dust.r, dust.g, dust.b, 0.7f);
        // Задаёт `main.maxParticles` значением `18`.
        main.maxParticles = 18;
        // Задаёт `main.simulationSpace` значением `ParticleSystemSimulationSpace.World`.
        main.simulationSpace = ParticleSystemSimulationSpace.World;
        // Задаёт `emission` значением `particles.emission`.
        var emission = particles.emission;
        // Задаёт `emission.rateOverTime` значением `0`.
        emission.rateOverTime = 0f;
        // Вызывает `emission.SetBursts` с аргументами `new[] { new ParticleSystem.Burst(0f, 14) }`.
        emission.SetBursts(new[] { new ParticleSystem.Burst(0f, 14) });
        // Задаёт `shape` значением `particles.shape`.
        var shape = particles.shape;
        // Задаёт `shape.shapeType` значением `ParticleSystemShapeType.Hemisphere`.
        shape.shapeType = ParticleSystemShapeType.Hemisphere;
        // Задаёт радиус `shape.radius` значением `0.3`.
        shape.radius = 0.3f;
        // Задаёт `size` значением `particles.sizeOverLifetime`.
        var size = particles.sizeOverLifetime;
        // Устанавливает активность компонента `size.enabled` в `true`.
        size.enabled = true;
        // Записывает в `size.size` диапазон свойства частиц с параметрами `1f, AnimationCurve.Linear(0f, 1f, 1f, 0.7f)`.
        size.size = new ParticleSystem.MinMaxCurve(1f, AnimationCurve.Linear(0f, 1f, 1f, 0.7f));
        // Ищет компонент через `go.GetComponent<ParticleSystemRenderer>` и записывает результат в `renderer`.
        var renderer = go.GetComponent<ParticleSystemRenderer>();
        // Ищет шейдер по имени через `Shader.Find` с аргументами `"Universal Render Pipeline/Particles/Unlit"` и записывает результат в `shader`.
        var shader = Shader.Find("Universal Render Pipeline/Particles/Unlit");
        // Проверяет условие `shader != null`.
        if (shader != null)
        {  // Начало ветки условия.
            // Создаёт материал с параметрами `shader` и сохраняет ссылку в `material`.
            var material = new Material(shader);
            // Задаёт текстуру через `material.SetTexture` с аргументами `"_BaseMap", SoftDot()`.
            material.SetTexture("_BaseMap", SoftDot());
            // Задаёт цветовой параметр через `material.SetColor` с аргументами `"_BaseColor", Color.white`.
            material.SetColor("_BaseColor", Color.white);
            // Задаёт числовой параметр через `material.SetFloat` с аргументами `"_Surface", 1f`.
            material.SetFloat("_Surface", 1f);
            // Задаёт числовой параметр через `material.SetFloat` с аргументами `"_Blend", 0f`.
            material.SetFloat("_Blend", 0f);
            // Задаёт целочисленный параметр через `material.SetInt` с аргументами `"_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha`.
            material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            // Задаёт целочисленный параметр через `material.SetInt` с аргументами `"_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha`.
            material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            // Задаёт целочисленный параметр через `material.SetInt` с аргументами `"_ZWrite", 0`.
            material.SetInt("_ZWrite", 0);
            // Включает ключевое слово шейдера через `material.EnableKeyword` с аргументами `"_SURFACE_TYPE_TRANSPARENT"`.
            material.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");
            // Задаёт `material.renderQueue` значением `3000`.
            material.renderQueue = 3000;
            // Задаёт материал `renderer.material` значением `material`.
            renderer.material = material;
        }  // Конец ветки условия.
        // Меняет активность объекта через `go.SetActive` с аргументами `true`.
        go.SetActive(true);
        // Запускает воспроизведение через `particles.Play`.
        particles.Play();
        // Планирует удаление объекта через `Object.Destroy` с аргументами `go, life + 0.4f`.
        Object.Destroy(go, life + 0.4f);
        // Проверяет условие `Instance != null`.
        if (Instance != null)
            // Запускает воспроизведение через `Instance.Play` с аргументами `Instance.digClip`.
            Instance.Play(Instance.digClip);
    }  // Конец метода `Burst`.

    // Хранит ссылку на двумерную текстуру в `softDot`.
    static Texture2D softDot;

    // Генерирует текстуру мягкой точки для частиц пыли.
    static Texture2D SoftDot()
    {  // Начало метода `SoftDot`.
        // Проверяет условие `softDot != null`.
        if (softDot != null)
            // Возвращает `softDot`.
            return softDot;
        // Задаёт `n` значением `32`.
        const int n = 32;
        // Записывает в `softDot` двумерную текстуру с параметрами `n, n, TextureFormat.RGBA32, false`.
        softDot = new Texture2D(n, n, TextureFormat.RGBA32, false);
        // Задаёт `pixels` массивом `new Color[n * n]`.
        var pixels = new Color[n * n];
        // Вычисляет `center` как (`n` минус `1`) умножить на `0.5`.
        var center = (n - 1) * 0.5f;
        // Перебирает индексы начиная с `int y = 0` при условии `y < n`.
        for (int y = 0; y < n; y++)
        {  // Начало цикла по индексам.
            // Перебирает индексы начиная с `int x = 0` при условии `x < n`.
            for (int x = 0; x < n; x++)
            {  // Начало цикла по индексам.
                // Вычисляет `d` как измеряет расстояние между точками через `Vector2.Distance` с аргументами `new Vector2(x, y), new Vector2(center, center)` разделить на `center`.
                float d = Vector2.Distance(new Vector2(x, y), new Vector2(center, center)) / center;
                // Ограничивает значение диапазоном от 0 до 1 через `Mathf.Clamp01` с аргументами `1f - d` и записывает результат в `a`.
                float a = Mathf.Clamp01(1f - d);
                // Умножает `a` на `a`.
                a *= a;
                // Записывает в `pixels[y * n + x]` цвет RGBA с параметрами `1f, 1f, 1f, a`.
                pixels[y * n + x] = new Color(1f, 1f, 1f, a);
            }  // Конец цикла по индексам.
        }  // Конец цикла по индексам.
        // Записывает цвета пикселей через `softDot.SetPixels` с аргументами `pixels`.
        softDot.SetPixels(pixels);
        // Вызывает `softDot.Apply`.
        softDot.Apply();
        // Возвращает `softDot`.
        return softDot;
    }  // Конец метода `SoftDot`.

    // Воспроизводит звук монет.
    public static void Coin()
    {  // Начало метода `Coin`.
        // Проверяет условие `Instance != null`.
        if (Instance != null)
            // Запускает воспроизведение через `Instance.Play` с аргументами `Instance.coinClip`.
            Instance.Play(Instance.coinClip);
    }  // Конец метода `Coin`.

    // Воспроизводит переданный одноразовый звуковой эффект.
    void Play(AudioClip clip)
    {  // Начало метода `Play`.
        // Проверяет условие `clip != null && source != null`.
        if (clip != null && source != null)
            // Проигрывает звук один раз через `source.PlayOneShot` с аргументами `clip`.
            source.PlayOneShot(clip);
    }  // Конец метода `Play`.
}  // Конец класса `DigFeedback`.

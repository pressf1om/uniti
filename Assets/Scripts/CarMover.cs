// Подключает доступ к базовым типам игрового движка.
using UnityEngine;

// Двигает машину по замкнутому маршруту с учётом других машин и светофоров.
public class CarMover : MonoBehaviour
{  // Начало класса `CarMover`.
    // Хранит массив трёхмерных векторов в `Points`.
    public Vector3[] Points;
    // Задаёт `Speed` значением `7`.
    public float Speed = 7f;
    // Хранит вещественное число в `Offset`.
    public float Offset;
    // Хранит трёхмерный вектор в `From`.
    public Vector3 From;
    // Хранит трёхмерный вектор в `To`.
    public Vector3 To;

    // Создаёт трёхмерный вектор с параметрами `18.5f, 0f, -10.4f` и сохраняет ссылку в `Crossing`.
    static readonly Vector3 Crossing = new Vector3(18.5f, 0f, -10.4f);
    // Задаёт `StopFar` значением `7.2`.
    const float StopFar = 7.2f;
    // Задаёт `StopNear` значением `3.6`.
    const float StopNear = 3.6f;

    // Хранит вещественное число в `along`.
    float along;
    // Хранит вещественное число в `length`.
    float length;
    // Хранит поле типа `float[]` в `spans`.
    float[] spans;
    // Хранит поле типа `CarMover[]` в `pack`.
    static CarMover[] pack;
    // Хранит вещественное число в `packAt`.
    static float packAt;

    // Подготавливает длины отрезков и начальную позицию машины.
    void Start()
    {  // Начало метода `Start`.
        // Проверяет условие `Points == null || Points.Length < 2`.
        if (Points == null || Points.Length < 2)
            // Задаёт `Points` значением массив `new[] { From, To }`.
            Points = new[] { From, To };
        // Рассчитывает длины отдельных сегментов маршрута через `BuildSpans`.
        BuildSpans();
        // Задаёт `along` значением `Offset` умножить на `length`.
        along = Offset * length;
    }  // Конец метода `Start`.

    // Передвигает машину за кадр, когда путь разрешён.
    void Update()
    {  // Начало метода `Update`.
        // Проверяет условие `Points == null || Points.Length < 2 || length < 0.1f`.
        if (Points == null || Points.Length < 2 || length < 0.1f)
            // Завершает выполнение текущего метода.
            return;
        // Находит позицию и направление движения по пройденному расстоянию через `Sample` с аргументами `along, out Vector3 pos, out Vector3 dir`.
        Sample(along, out Vector3 pos, out Vector3 dir);
        // Проверяет условие `!ShouldHold(pos, dir) && !Blocked()`.
        if (!ShouldHold(pos, dir) && !Blocked())
            // Возвращает позицию внутри повторяющегося периода через `Mathf.Repeat` с аргументами `along + Speed * Time.deltaTime, length` и записывает результат в `along`.
            along = Mathf.Repeat(along + Speed * Time.deltaTime, length);
        // Находит позицию и направление движения по пройденному расстоянию через `Sample` с аргументами `along, out pos, out dir`.
        Sample(along, out pos, out dir);
        // Задаёт мировая позиция `transform.position` значением `pos`.
        transform.position = pos;
        // Проверяет условие `dir.sqrMagnitude > 0.01f`.
        if (dir.sqrMagnitude > 0.01f)
            // Вызывает `Quaternion.LookRotation` с аргументами `dir` и записывает результат в `transform.rotation`.
            transform.rotation = Quaternion.LookRotation(dir);
    }  // Конец метода `Update`.

    // Рассчитывает длины отдельных сегментов маршрута.
    void BuildSpans()
    {  // Начало метода `BuildSpans`.
        // Задаёт `spans` значением массив `new float[Points.Length]`.
        spans = new float[Points.Length];
        // Задаёт `length` значением `0`.
        length = 0f;
        // Перебирает индексы начиная с `int i = 0` при условии `i < Points.Length`.
        for (int i = 0; i < Points.Length; i++)
        {  // Начало цикла по индексам.
            // Задаёт `a` значением элемент `Points` с индексом `i`.
            Vector3 a = Points[i];
            // Задаёт `b` значением элемент `Points` с индексом `(i + 1) % Points.Length`.
            Vector3 b = Points[(i + 1) % Points.Length];
            // Измеряет расстояние между точками через `Vector3.Distance` с аргументами `a, b` и записывает результат в `spans[i]`.
            spans[i] = Vector3.Distance(a, b);
            // Добавляет `spans[i]` к текущему значению `length`.
            length += spans[i];
        }  // Конец цикла по индексам.
    }  // Конец метода `BuildSpans`.

    // Находит позицию и направление движения по пройденному расстоянию.
    void Sample(float d, out Vector3 pos, out Vector3 dir)
    {  // Начало метода `Sample`.
        // Возвращает позицию внутри повторяющегося периода через `Mathf.Repeat` с аргументами `d, length` и записывает результат в `rest`.
        float rest = Mathf.Repeat(d, length);
        // Перебирает индексы начиная с `int i = 0` при условии `i < Points.Length`.
        for (int i = 0; i < Points.Length; i++)
        {  // Начало цикла по индексам.
            // Проверяет условие `rest > spans[i] && i < Points.Length - 1`.
            if (rest > spans[i] && i < Points.Length - 1)
            {  // Начало ветки условия.
                // Уменьшает `rest` на `spans[i]`.
                rest -= spans[i];
                // Пропускает остаток этой итерации и переходит к следующей.
                continue;
            }  // Конец ветки условия.
            // Задаёт `a` значением элемент `Points` с индексом `i`.
            Vector3 a = Points[i];
            // Задаёт `b` значением элемент `Points` с индексом `(i + 1) % Points.Length`.
            Vector3 b = Points[(i + 1) % Points.Length];
            // Выбирает значение для `t` по условию элемент `spans` с индексом `i` меньше `0.001`.
            float t = spans[i] < 0.001f ? 0f : rest / spans[i];
            // Интерполирует между значениями через `Vector3.Lerp` с аргументами `a, b, t` и записывает результат в `pos`.
            pos = Vector3.Lerp(a, b, t);
            // Задаёт `dir` значением `b` минус `a`.
            dir = b - a;
            // Завершает выполнение текущего метода.
            return;
        }  // Конец цикла по индексам.
        // Задаёт `pos` значением элемент `Points` с индексом `0`.
        pos = Points[0];
        // Задаёт `dir` значением элемент `Points` с индексом `1` минус элемент `Points` с индексом `0`.
        dir = Points[1] - Points[0];
    }  // Конец метода `Sample`.

    // Проверяет расстояние по маршруту до других машин перед текущей.
    bool Blocked()
    {  // Начало метода `Blocked`.
        // Проверяет условие `Time.time > packAt + 0.25f || pack == null`.
        if (Time.time > packAt + 0.25f || pack == null)
        {  // Начало ветки условия.
            // Собирает объекты типа через `Object.FindObjectsByType<CarMover>` с аргументами `FindObjectsSortMode.None` и записывает результат в `pack`.
            pack = Object.FindObjectsByType<CarMover>(FindObjectsSortMode.None);
            // Задаёт `packAt` значением игровое время `Time.time`.
            packAt = Time.time;
        }  // Конец ветки условия.
        // Перебирает индексы начиная с `int i = 0` при условии `i < pack.Length`.
        for (int i = 0; i < pack.Length; i++)
        {  // Начало цикла по индексам.
            // Задаёт `other` значением элемент `pack` с индексом `i`.
            var other = pack[i];
            // Проверяет условие `other == this || other.length < 0.1f`.
            if (other == this || other.length < 0.1f)
                // Пропускает остаток этой итерации и переходит к следующей.
                continue;
            // Возвращает позицию внутри повторяющегося периода через `Mathf.Repeat` с аргументами `other.along - along, length` и записывает результат в `ahead`.
            float ahead = Mathf.Repeat(other.along - along, length);
            // Проверяет условие `ahead > 0.4f && ahead < 5.5f`.
            if (ahead > 0.4f && ahead < 5.5f)
                // Возвращает `true`.
                return true;
        }  // Конец цикла по индексам.
        // Возвращает `false`.
        return false;
    }  // Конец метода `Blocked`.

    // Проверяет, нужно ли остановиться перед красным сигналом.
    static bool ShouldHold(Vector3 pos, Vector3 dir)
    {  // Начало метода `ShouldHold`.
        // Проверяет условие `dir.sqrMagnitude < 0.01f`.
        if (dir.sqrMagnitude < 0.01f)
            // Возвращает `false`.
            return false;
        // Задаёт `flat` значением `dir`.
        Vector3 flat = dir;
        // Задаёт `flat.y` значением `0`.
        flat.y = 0f;
        // Вычисляет `to` как `Crossing` минус `pos`.
        Vector3 to = Crossing - pos;
        // Задаёт `to.y` значением `0`.
        to.y = 0f;
        // Задаёт `dist` значением `to.magnitude`.
        float dist = to.magnitude;
        // Проверяет условие `dist > StopFar || dist < StopNear`.
        if (dist > StopFar || dist < StopNear)
            // Возвращает `false`.
            return false;
        // Проверяет условие `Vector3.Dot(flat.normalized, to.normalized) < 0.55f`.
        if (Vector3.Dot(flat.normalized, to.normalized) < 0.55f)
            // Возвращает `false`.
            return false;
        // Возвращает не выполняется `TrafficLight.Allowed(flat)`.
        return !TrafficLight.Allowed(flat);
    }  // Конец метода `ShouldHold`.
}  // Конец класса `CarMover`.

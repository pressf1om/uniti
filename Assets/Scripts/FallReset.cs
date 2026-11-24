// Подключает доступ к базовым типам игрового движка.
using UnityEngine;

// Возвращает игрока в безопасную точку при падении под сцену.
public class FallReset : MonoBehaviour
{  // Начало класса `FallReset`.
    // Добавляет под сцену триггер обнаружения падения.
    public static void Spawn(Transform parent)
    {  // Начало метода `Spawn`.
        // Создаёт стандартный примитив через `GameObject.CreatePrimitive` с аргументами `PrimitiveType.Cube` и записывает результат в `go`.
        var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
        // Задаёт имя `go.name` значением `"Возврат"`.
        go.name = "Возврат";
        // Привязывает к родителю через `go.transform.SetParent` с аргументами `parent, false`.
        go.transform.SetParent(parent, false);
        // Записывает в `go.transform.position` трёхмерный вектор с параметрами `2f, -14f, 2f`.
        go.transform.position = new Vector3(2f, -14f, 2f);
        // Записывает в `go.transform.localScale` трёхмерный вектор с параметрами `220f, 2f, 220f`.
        go.transform.localScale = new Vector3(220f, 2f, 220f);
        // Планирует удаление объекта через `Object.Destroy` с аргументами `go.GetComponent<MeshRenderer>()`.
        Object.Destroy(go.GetComponent<MeshRenderer>());
        // Ищет компонент через `go.GetComponent<BoxCollider>` и записывает результат в `box`.
        var box = go.GetComponent<BoxCollider>();
        // Устанавливает режим триггера `box.isTrigger` в `true`.
        box.isTrigger = true;
        // Добавляет компонент через `go.AddComponent<Rigidbody>` и записывает результат в `body`.
        var body = go.AddComponent<Rigidbody>();
        // Устанавливает кинематический режим `body.isKinematic` в `true`.
        body.isKinematic = true;
        // Устанавливает действие гравитации `body.useGravity` в `false`.
        body.useGravity = false;
        // Добавляет компонент через `go.AddComponent<FallReset>`.
        go.AddComponent<FallReset>();
    }  // Конец метода `Spawn`.

    // Возвращает персонажа из зоны падения.
    void OnTriggerEnter(Collider other)
    {  // Начало метода `OnTriggerEnter`.
        // Ищет компонент в объекте и его родителях через `other.GetComponentInParent<FirstPersonPlayer>` и записывает результат в `player`.
        var player = other.GetComponentInParent<FirstPersonPlayer>();
        // Проверяет условие `player != null`.
        if (player != null)
            // Вызывает `player.Respawn`.
            player.Respawn();
    }  // Конец метода `OnTriggerEnter`.
}  // Конец класса `FallReset`.

// Подключает доступ к базовым типам игрового движка.
using UnityEngine;

// Хранит разметку стартовой позиции игрока и мест продавца.
public class YardMarkers : MonoBehaviour
{  // Начало класса `YardMarkers`.
    // Просит Unity сохранять поле и показывать его в инспекторе.
    [SerializeField] Transform playerSpawn;
    // Просит Unity сохранять поле и показывать его в инспекторе.
    [SerializeField] Transform sellerNorth;
    // Просит Unity сохранять поле и показывать его в инспекторе.
    [SerializeField] Transform sellerSouth;

    // Даёт доступ к `PlayerPosition` через `playerSpawn != null ? playerSpawn.position : new Vector3(5f, 0.15f, -3.2f)`.
    public Vector3 PlayerPosition =>
        // Использует значение `playerSpawn != null ? playerSpawn.position : new Vector3(5f, 0.15f, -3.2f)`.
        playerSpawn != null ? playerSpawn.position : new Vector3(5f, 0.15f, -3.2f);

    // Даёт доступ к `SellerNorthPoint` через `sellerNorth != null ? sellerNorth.position : new Vector3(-2.2f, 0f, 4.35f)`.
    public Vector3 SellerNorthPoint =>
        // Использует значение `sellerNorth != null ? sellerNorth.position : new Vector3(-2.2f, 0f, 4.35f)`.
        sellerNorth != null ? sellerNorth.position : new Vector3(-2.2f, 0f, 4.35f);

    // Даёт доступ к `SellerSouthPoint` через `sellerSouth != null ? sellerSouth.position : new Vector3(-6.5f, 0f, 3.1f)`.
    public Vector3 SellerSouthPoint =>
        // Использует значение `sellerSouth != null ? sellerSouth.position : new Vector3(-6.5f, 0f, 3.1f)`.
        sellerSouth != null ? sellerSouth.position : new Vector3(-6.5f, 0f, 3.1f);

    // Соединяет компонент с объектами-разметкой сцены.
    public void Wire(Transform spawn, Transform north, Transform south)
    {  // Начало метода `Wire`.
        // Задаёт `playerSpawn` значением `spawn`.
        playerSpawn = spawn;
        // Задаёт `sellerNorth` значением `north`.
        sellerNorth = north;
        // Задаёт `sellerSouth` значением `south`.
        sellerSouth = south;
    }  // Конец метода `Wire`.
}  // Конец класса `YardMarkers`.
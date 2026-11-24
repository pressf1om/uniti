// Подключает доступ к базовым типам игрового движка.
using UnityEngine;
// Подключает доступ к вводу с клавиатуры, мыши и других устройств.
using UnityEngine.InputSystem;
// Подключает доступ к загрузке и запросу сцен.
using UnityEngine.SceneManagement;

// Просит Unity автоматически добавить обязательный компонент `CharacterController`.
[RequireComponent(typeof(CharacterController))]
// Просит Unity автоматически добавить обязательный компонент `Inventory`.
[RequireComponent(typeof(Inventory))]
// Обрабатывает движение, обзор, копание, взаимодействие, торговлю и паузу.
public class FirstPersonPlayer : MonoBehaviour
{  // Начало класса `FirstPersonPlayer`.
    // Задаёт скорость ходьбы `MoveSpeed` значением `4.5`.
    public float MoveSpeed = 4.5f;
    // Задаёт высота прыжка `JumpHeight` значением `1.25`.
    public float JumpHeight = 1.25f;
    // Задаёт чувствительность мыши `LookSensitivity` значением `0.1`.
    public float LookSensitivity = 0.1f;

    // Задаёт ускорение падения `Gravity` значением `-20f`.
    const float Gravity = -20f;

    // Хранит ссылку на физический контроллер персонажа в `body`.
    CharacterController body;
    // Хранит рюкзак игрока `bag`.
    Inventory bag;
    // Хранит ссылку на раскоп в `site`.
    DigSite site;
    // Хранит ссылку на положение, поворот и масштаб объекта в `view`.
    Transform view;
    // Хранит модель лопаты в руках `shovelView`.
    Transform shovelView;
    // Хранит игровой интерфейс `hud`.
    PlayHud hud;
    // Хранит угол вертикального наклона камеры `pitch`.
    float pitch;
    // Хранит вертикальная скорость `verticalVelocity`.
    float verticalVelocity;
    // Хранит вещественное число в `bob`.
    float bob;
    // Хранит признак наличия лопаты `hasShovel`.
    bool hasShovel;
    // Хранит уровень лопаты `shovelTier`.
    int shovelTier;
    // Хранит уровень дополнительных прыжков `jumpBootsLevel`.
    int jumpBootsLevel;
    // Хранит счётчик использованных прыжков в воздухе `airJumpsLeft`.
    int airJumpsLeft;
    // Хранит тип открытого магазина `openShop`.
    ShopKind? openShop;
    // Задаёт `hint` значением `""`.
    string hint = "";
    // Хранит время окончания показа подсказки `hintUntil`.
    float hintUntil;

    // Хранит признак паузы `paused`.
    bool paused;
    // Хранит признак записи победы в этом сеансе `savedWin`.
    bool savedWin;

    // Даёт доступ к `HasShovel` через `hasShovel`.
    public bool HasShovel => hasShovel;
    // Даёт доступ к `Bag` через `bag`.
    public Inventory Bag => bag;
    // Даёт доступ к `Tier` через `shovelTier`.
    public int Tier => shovelTier;
    // Даёт доступ к `DigRadius` через `ShovelTiers.Radius[shovelTier]`.
    public float DigRadius => ShovelTiers.Radius[shovelTier];
    // Даёт доступ к `JumpBootsLevel` через `jumpBootsLevel`.
    public int JumpBootsLevel => jumpBootsLevel;
    // Даёт доступ к `DoubleJumpBoots` через `jumpBootsLevel >= 1`.
    public bool DoubleJumpBoots => jumpBootsLevel >= 1;

    // Передаёт контроллеру ссылки на раскоп и игровой интерфейс.
    public void Bind(DigSite digSite, PlayHud playHud)
    {  // Начало метода `Bind`.
        // Задаёт `site` значением участок раскопа `digSite`.
        site = digSite;
        // Задаёт игровой интерфейс `hud` значением `playHud`.
        hud = playHud;
    }  // Конец метода `Bind`.

    // Находит компоненты игрока и настраивает форму его физического тела.
    void Awake()
    {  // Начало метода `Awake`.
        // Ищет компонент через `GetComponent<CharacterController>` и записывает результат в `body`.
        body = GetComponent<CharacterController>();
        // Ищет компонент через `GetComponent<Inventory>` и записывает результат в `bag`.
        bag = GetComponent<Inventory>();
        // Ищет объект по имени через `transform.Find` с аргументами `"View"` и записывает результат в `view`.
        view = transform.Find("View");
        // Ищет объект по имени через `view.Find` с аргументами `"ShovelView"` и записывает результат в `shovelView`.
        shovelView = view.Find("ShovelView");
        // Задаёт высота `body.height` значением `1.8`.
        body.height = 1.8f;
        // Задаёт радиус `body.radius` значением `0.35`.
        body.radius = 0.35f;
        // Записывает в `body.center` трёхмерный вектор с параметрами `0f, 0.9f, 0f`.
        body.center = new Vector3(0f, 0.9f, 0f);
        // Задаёт высота доступной ступени `body.stepOffset` значением `0.4`.
        body.stepOffset = 0.4f;
    }  // Конец метода `Awake`.

    // Применяет сохранённые настройки и захватывает курсор в начале игры.
    void Start()
    {  // Начало метода `Start`.
        // Задаёт чувствительность мыши `LookSensitivity` значением `GameSettings.Sensitivity`.
        LookSensitivity = GameSettings.Sensitivity;
        // Применяет громкость звука через `GameSettings.Apply`.
        GameSettings.Apply();
        // Проверяет условие `GameLoop.Instance == null || GameLoop.Instance.State == RunState.Running`.
        if (GameLoop.Instance == null || GameLoop.Instance.State == RunState.Running)
            // Захватывает курсор и скрывает его для управления видом через `LockCursor`.
            LockCursor();
    }  // Конец метода `Start`.

    // Восстанавливает рюкзак, улучшения, лопату, позицию и поворот игрока.
    public void ApplyProgress(GameSaveData data)
    {  // Начало метода `ApplyProgress`.
        // Проверяет условие `data == null`.
        if (data == null)
            // Завершает выполнение текущего метода.
            return;
        // Выбирает наибольшее значение через `Mathf.Max` с аргументами `5, data.capacity` и записывает результат в `bag.Capacity`.
        bag.Capacity = Mathf.Max(5, data.capacity);
        // Задаёт `bag.Money` значением `data.money`.
        bag.Money = data.money;
        // Вызывает `bag.Fill` с аргументами `data.stone, data.coal, data.copper, data.iron, data.gold`.
        bag.Fill(data.stone, data.coal, data.copper, data.iron, data.gold);
        // Ограничивает загруженный уровень лопаты диапазоном от базового до максимального.
        shovelTier = Mathf.Clamp(data.shovelTier, 0, ShovelTiers.UpgradeCost.Length);
        // Ограничивает уровень дополнительных прыжков пределами, заданными в PlayerUpgrades.
        jumpBootsLevel = Mathf.Clamp(data.jumpBootsLevel, 0, PlayerUpgrades.MaxJumpBootsLevel);
        // Поддерживает старые файлы сохранений, в которых был только флаг двойного прыжка.
        if (jumpBootsLevel == 0 && data.doubleJumpBoots)
            // Задаёт уровень дополнительных прыжков `jumpBootsLevel` значением `1`.
            jumpBootsLevel = 1;
        // Проверяет условие `data.hasShovel`.
        if (data.hasShovel)
            // Отмечает найденную лопату и показывает её модель в руках через `GiveShovel`.
            GiveShovel();
        // Временно отключает CharacterController перед переносом персонажа.
        body.enabled = false;
        // Записывает в `transform.position` трёхмерный вектор с параметрами `data.px, data.py, data.pz`.
        transform.position = new Vector3(data.px, data.py, data.pz);
        // Создаёт поворот из углов Эйлера через `Quaternion.Euler` с аргументами `0f, data.yaw, 0f` и записывает результат в `transform.rotation`.
        transform.rotation = Quaternion.Euler(0f, data.yaw, 0f);
        // Возвращает физику персонажа после установки позиции и угла.
        body.enabled = true;
    }  // Конец метода `ApplyProgress`.

    // Собирает прогресс игрока и раскопа в объект для записи в JSON.
    public GameSaveData Export(DigSite site)
    {  // Начало метода `Export`.
        // Возвращает новый объект `GameSaveData` с полями, перечисленными ниже.
        return new GameSaveData
        {  // Начало списка значений полей и свойств.
            // Задаёт баланс монет `money` значением `bag.Money`.
            money = bag.Money,
            // Задаёт вместимость рюкзака `capacity` значением `bag.Capacity`.
            capacity = bag.Capacity,
            // Задаёт уровень лопаты `shovelTier` значением уровень лопаты `shovelTier`.
            shovelTier = shovelTier,
            // Задаёт признак наличия лопаты `hasShovel` значением признак наличия лопаты `hasShovel`.
            hasShovel = hasShovel,
            // Задаёт старый признак покупки двойного прыжка `doubleJumpBoots` значением уровень дополнительных прыжков `jumpBootsLevel` не меньше `1`.
            doubleJumpBoots = jumpBootsLevel >= 1,
            // Задаёт уровень дополнительных прыжков `jumpBootsLevel` значением уровень дополнительных прыжков `jumpBootsLevel`.
            jumpBootsLevel = jumpBootsLevel,
            // Вызывает `bag.CountOf` с аргументами `ResourceKind.Stone` и записывает результат в `stone`.
            stone = bag.CountOf(ResourceKind.Stone),
            // Вызывает `bag.CountOf` с аргументами `ResourceKind.Coal` и записывает результат в `coal`.
            coal = bag.CountOf(ResourceKind.Coal),
            // Вызывает `bag.CountOf` с аргументами `ResourceKind.Copper` и записывает результат в `copper`.
            copper = bag.CountOf(ResourceKind.Copper),
            // Вызывает `bag.CountOf` с аргументами `ResourceKind.Iron` и записывает результат в `iron`.
            iron = bag.CountOf(ResourceKind.Iron),
            // Вызывает `bag.CountOf` с аргументами `ResourceKind.Gold` и записывает результат в `gold`.
            gold = bag.CountOf(ResourceKind.Gold),
            // Вызывает `site.ExportDug` и записывает результат в `dug`.
            dug = site.ExportDug(),
            // Вызывает `site.ExportLooted` и записывает результат в `looted`.
            looted = site.ExportLooted(),
            // Задаёт признак взятого клада `treasureTaken` значением контроллер партии `GameLoop.Instance` отличается от отсутствующую ссылку (`null`) и `GameLoop.Instance.TreasureClaimed`.
            treasureTaken = GameLoop.Instance != null && GameLoop.Instance.TreasureClaimed,
            // Задаёт индекс клада по X `treasureIx` значением `site.TreasureIx`.
            treasureIx = site.TreasureIx,
            // Задаёт индекс клада по Z `treasureIz` значением `site.TreasureIz`.
            treasureIz = site.TreasureIz,
            // Задаёт позиция игрока по X `px` значением `transform.position.x`.
            px = transform.position.x,
            // Задаёт позиция игрока по Y `py` значением `transform.position.y`.
            py = transform.position.y,
            // Задаёт позиция игрока по Z `pz` значением `transform.position.z`.
            pz = transform.position.z,
            // Задаёт поворот вокруг вертикальной оси `yaw` значением `transform.eulerAngles.y`.
            yaw = transform.eulerAngles.y
        };  // Конец списка значений полей и свойств.
    }  // Конец метода `Export`.

    // За кадр проверяет состояние игры и передаёт управление нужному режиму.
    void Update()
    {  // Начало метода `Update`.
        // Задаёт `keyboard` значением текущая клавиатура `Keyboard.current`.
        var keyboard = Keyboard.current;
        // Задаёт `mouse` значением текущая мышь `Mouse.current`.
        var mouse = Mouse.current;
        // Проверяет условие `keyboard == null || mouse == null`.
        if (keyboard == null || mouse == null)
            // Завершает выполнение текущего метода.
            return;

        // Проверяет, пора ли записать накопленные изменения.
        GameSave.FlushPending();

        // Приостанавливает управление, пока игрок подтверждает найденный клад.
        if (GameLoop.Instance != null && GameLoop.Instance.AwaitingTreasureAck)
        {  // Начало ветки условия.
            // Проверяет, была ли победа уже записана для этого запуска.
            if (!savedWin)
            {  // Начало ветки условия.
                // Устанавливает признак записи победы в этом сеансе `savedWin` в `true`.
                savedWin = true;
                // Сохраняет текущий игровой прогресс через `GameSave.WriteCurrent`.
                GameSave.WriteCurrent();
            }  // Конец ветки условия.
            // Завершает выполнение текущего метода.
            return;
        }  // Конец ветки условия.

        // Проверяет условие `keyboard.escapeKey.wasPressedThisFrame`.
        if (keyboard.escapeKey.wasPressedThisFrame)
            // Обрабатывает переключение паузы с учётом открытых окон через `TogglePause`.
            TogglePause();

        // Проверяет условие `keyboard.mKey.wasPressedThisFrame && !openShop.HasValue`.
        if (keyboard.mKey.wasPressedThisFrame && !openShop.HasValue)
            // Обрабатывает переключение паузы с учётом открытых окон через `TogglePause`.
            TogglePause();

        // Проверяет условие `paused`.
        if (paused)
        {  // Начало ветки условия.
            // Обновляет HUD с пустой подсказкой взаимодействия.
            PushHud("");
            // Проверяет условие `hud != null`.
            if (hud != null)
                // Вызывает `hud.SetPaused` с аргументами `true`.
                hud.SetPaused(true);
            // Завершает выполнение текущего метода.
            return;
        }  // Конец ветки условия.
        // Проверяет условие `hud != null`.
        if (hud != null)
            // Вызывает `hud.SetPaused` с аргументами `false`.
            hud.SetPaused(false);

        // Проверяет условие `openShop.HasValue`.
        if (openShop.HasValue)
        {  // Начало ветки условия.
            // Распознаёт клавиши покупки улучшений в кузнице через `Trade` с аргументами `keyboard`.
            Trade(keyboard);
            // Обновляет подсказку, рюкзак и видимость окна торговли через `PushHud` с аргументами `""`.
            PushHud("");
            // Завершает выполнение текущего метода.
            return;
        }  // Конец ветки условия.

        // Проверяет условие `Cursor.lockState != CursorLockMode.Locked`.
        if (Cursor.lockState != CursorLockMode.Locked)
        {  // Начало ветки условия.
            // Проверяет условие `mouse.leftButton.wasPressedThisFrame`.
            if (mouse.leftButton.wasPressedThisFrame)
                // Захватывает курсор и скрывает его для управления видом через `LockCursor`.
                LockCursor();
            // Завершает выполнение текущего метода.
            return;
        }  // Конец ветки условия.

        // Поворачивает тело и камеру по перемещению мыши через `Look` с аргументами `mouse`.
        Look(mouse);
        // Двигает персонажа по WASD и рассчитывает обычный и воздушные прыжки через `Move` с аргументами `keyboard`.
        Move(keyboard);
        // Проверяет условие `transform.position.y < -11f`.
        if (transform.position.y < -11f)
            // Переносит игрока в безопасную точку двора через `Respawn`.
            Respawn();
        // Покачивает камеру во время движения по земле через `Bob`.
        Bob();
        // Разбирает нажатия для удара лопатой, торговли и подбора предметов через `Act` с аргументами `keyboard, mouse`.
        Act(keyboard, mouse);
        // Обновляет подсказку, рюкзак и видимость окна торговли через `PushHud` с аргументами `NearestPrompt()`.
        PushHud(NearestPrompt());
    }  // Конец метода `Update`.

    // Обновляет подсказку, рюкзак и видимость окна торговли.
    void PushHud(string prompt)
    {  // Начало метода `PushHud`.
        // Проверяет условие `hud == null`.
        if (hud == null)
            // Завершает выполнение текущего метода.
            return;
        // Выбирает значение для `line` по условию игровое время `Time.time` меньше время окончания показа подсказки `hintUntil`.
        string line = Time.time < hintUntil ? hint : prompt;
        // Обновляет данные через `hud.Refresh` с аргументами `bag, hasShovel, shovelTier, line, ""`.
        hud.Refresh(bag, hasShovel, shovelTier, line, "");
        // Передаёт открытую торговую панель и текущего игрока интерфейсу.
        hud.PresentShop(openShop, this);
    }  // Конец метода `PushHud`.

    // Отмечает найденную лопату и показывает её модель в руках.
    public void GiveShovel()
    {  // Начало метода `GiveShovel`.
        // Устанавливает признак наличия лопаты `hasShovel` в `true`.
        hasShovel = true;
        // Проверяет условие `shovelView != null`.
        if (shovelView != null)
            // Меняет активность объекта через `shovelView.gameObject.SetActive` с аргументами `true`.
            shovelView.gameObject.SetActive(true);
        // Вызывает `ShovelTint.Apply` с аргументами `shovelView, shovelTier`.
        ShovelTint.Apply(shovelView, shovelTier);
        // Показывает временную подсказку через `ShowHint` с аргументами `"Лопата в руках"`.
        ShowHint("Лопата в руках");
    }  // Конец метода `GiveShovel`.

    // Переносит игрока в безопасную точку двора.
    public void Respawn()
    {  // Начало метода `Respawn`.
        // Устанавливает активность компонента `body.enabled` в `false`.
        body.enabled = false;
        // Записывает в `transform.position` трёхмерный вектор с параметрами `5f, 0.2f, -3.2f`.
        transform.position = new Vector3(5f, 0.2f, -3.2f);
        // Задаёт вертикальная скорость `verticalVelocity` значением `-2f`.
        verticalVelocity = -2f;
        // Устанавливает активность компонента `body.enabled` в `true`.
        body.enabled = true;
    }  // Конец метода `Respawn`.

    // Поворачивает тело и камеру по перемещению мыши.
    void Look(Mouse mouse)
    {  // Начало метода `Look`.
        // Считывает значение устройства ввода через `mouse.delta.ReadValue` и записывает результат в `delta`.
        Vector2 delta = mouse.delta.ReadValue();
        // Поворачивает тело игрока горизонтально по смещению мыши.
        transform.Rotate(0f, delta.x * LookSensitivity, 0f);
        // Ограничивает наклон взгляда 85 градусами вверх и вниз.
        pitch = Mathf.Clamp(pitch - delta.y * LookSensitivity, -85f, 85f);
        // Создаёт поворот из углов Эйлера через `Quaternion.Euler` с аргументами `pitch, 0f, 0f` и записывает результат в `view.localRotation`.
        view.localRotation = Quaternion.Euler(pitch, 0f, 0f);
    }  // Конец метода `Look`.

    // Двигает персонажа по WASD и рассчитывает обычный и воздушные прыжки.
    void Move(Keyboard keyboard)
    {  // Начало метода `Move`.
        // Проверяет условие `!body.enabled`.
        if (!body.enabled)
            // Завершает выполнение текущего метода.
            return;
        // Задаёт `x` значением `0`.
        float x = 0f;
        // Задаёт `z` значением `0`.
        float z = 0f;
        // Читает удержание A как движение влево.
        if (keyboard.aKey.isPressed) x -= 1f;
        // Читает удержание D как движение вправо.
        if (keyboard.dKey.isPressed) x += 1f;
        // Читает удержание S как движение назад.
        if (keyboard.sKey.isPressed) z -= 1f;
        // Читает удержание W как движение вперёд.
        if (keyboard.wKey.isPressed) z += 1f;
        // Складывает смещения относительно текущего поворота персонажа.
        Vector3 wish = transform.right * x + transform.forward * z;
        // Проверяет условие `wish.sqrMagnitude > 1f`.
        if (wish.sqrMagnitude > 1f)
            // Нормализует диагональное направление, чтобы ходьба по диагонали не ускорялась.
            wish.Normalize();

        // Проверяет условие `body.isGrounded`.
        if (body.isGrounded)
        {  // Начало ветки условия.
            // Восстанавливает возможность использовать все купленные прыжки при приземлении.
            airJumpsLeft = 0;
            // Проверяет условие `verticalVelocity < 0f`.
            if (verticalVelocity < 0f)
                // Задаёт вертикальная скорость `verticalVelocity` значением `-2f`.
                verticalVelocity = -2f;
        }  // Конец ветки условия.

        // Проверяет условие `keyboard.spaceKey.wasPressedThisFrame`.
        if (keyboard.spaceKey.wasPressedThisFrame)
        {  // Начало ветки условия.
            // Проверяет условие `body.isGrounded`.
            if (body.isGrounded)
                // Вычисляет начальную вертикальную скорость для заданной высоты прыжка.
                verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);
            // Проверяет, остался ли доступный дополнительный прыжок в воздухе.
            else if (jumpBootsLevel > 0 && airJumpsLeft < jumpBootsLevel)
            {  // Начало ветки условия.
                // Учитывает один использованный воздушный прыжок.
                airJumpsLeft++;
                // Вычисляет квадратный корень через `Mathf.Sqrt` с аргументами `JumpHeight * -2f * Gravity` и записывает результат в `verticalVelocity`.
                verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);
            }  // Конец ветки условия.
        }  // Конец ветки условия.

        // Прибавляет к вертикальной скорости изменение от гравитации за кадр.
        verticalVelocity += Gravity * Time.deltaTime;
        // Вычисляет `velocity` как `wish` умножить на скорость ходьбы `MoveSpeed`.
        Vector3 velocity = wish * MoveSpeed;
        // Задаёт `velocity.y` значением вертикальная скорость `verticalVelocity`.
        velocity.y = verticalVelocity;
        // Передаёт CharacterController смещение игрока за один кадр.
        body.Move(velocity * Time.deltaTime);
    }  // Конец метода `Move`.

    // Покачивает камеру во время движения по земле.
    void Bob()
    {  // Начало метода `Bob`.
        // Задаёт `flat` значением `body.velocity`.
        Vector3 flat = body.velocity;
        // Задаёт `flat.y` значением `0`.
        flat.y = 0f;
        // Задаёт `speed` значением `flat.magnitude`.
        float speed = flat.magnitude;
        // Проверяет условие `body.isGrounded && speed > 0.2f`.
        if (body.isGrounded && speed > 0.2f)
            // Добавляет `Time.deltaTime * speed * 1.8f` к текущему значению `bob`.
            bob += Time.deltaTime * speed * 1.8f;
        // Вычисляет вертикальное покачивание камеры как синус, усиленный скоростью ходьбы.
        float offset = body.isGrounded ? Mathf.Sin(bob) * 0.035f * Mathf.Clamp01(speed / MoveSpeed) : 0f;
        // Записывает в `view.localPosition` трёхмерный вектор с параметрами `0f, 1.64f + offset, 0f`.
        view.localPosition = new Vector3(0f, 1.64f + offset, 0f);
    }  // Конец метода `Bob`.

    // Разбирает нажатия для удара лопатой, торговли и подбора предметов.
    void Act(Keyboard keyboard, Mouse mouse)
    {  // Начало метода `Act`.
        // Проверяет условие `mouse.leftButton.wasPressedThisFrame`.
        if (mouse.leftButton.wasPressedThisFrame)
        {  // Начало ветки условия.
            // Задаёт `counter` значением `Aimed()` как `ShopCounter` с `null` при несовпадении типа.
            var counter = Aimed() as ShopCounter;
            // Проверяет условие `counter != null`.
            if (counter != null)
            {  // Начало ветки условия.
                // Открывает нужный магазин и освобождает курсор через `OpenShop` с аргументами `counter.Kind`.
                OpenShop(counter.Kind);
                // Завершает выполнение текущего метода.
                return;
            }  // Конец ветки условия.
            // Ищет лучом доступный блок перед игроком и ударяет по нему через `TryDig`.
            TryDig();
        }  // Конец ветки условия.

        // Проверяет условие `keyboard.eKey.wasPressedThisFrame`.
        if (keyboard.eKey.wasPressedThisFrame)
            // Подбирает предмет или открывает магазин, с которым взаимодействует игрок через `TryUse`.
            TryUse();
    }  // Конец метода `Act`.

    // Ищет лучом доступный блок перед игроком и ударяет по нему.
    void TryDig()
    {  // Начало метода `TryDig`.
        // Проверяет условие `!hasShovel`.
        if (!hasShovel)
        {  // Начало ветки условия.
            // Показывает временную подсказку через `ShowHint` с аргументами `"Нужна лопата"`.
            ShowHint("Нужна лопата");
            // Завершает выполнение текущего метода.
            return;
        }  // Конец ветки условия.

        // Посылает из камеры луч на дальность лопаты, игнорируя триггеры.
        var hits = Physics.RaycastAll(view.position, view.forward, YardMetrics.DigReach, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore);
        // Сортирует все пересечения луча от ближайшего к дальнему.
        System.Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance));
        // Перебирает индексы начиная с `int i = 0` при условии `i < hits.Length`.
        for (int i = 0; i < hits.Length; i++)
        {  // Начало цикла по индексам.
            // Берёт коллайдер очередного попадания луча.
            var collider = hits[i].collider;
            // Определяет, относится ли коллайдер к уже выкопанной находке.
            if (collider.GetComponentInParent<ResourcePickup>() != null)
                // Пропускает остаток этой итерации и переходит к следующей.
                continue;
            // Определяет, относится ли коллайдер к самому игроку.
            if (collider.GetComponentInParent<FirstPersonPlayer>() != null)
                // Пропускает остаток этой итерации и переходит к следующей.
                continue;

            // Ищет компонент через `collider.GetComponent<DigBlock>` и записывает результат в `block`.
            var block = collider.GetComponent<DigBlock>();
            // Проверяет условие `block == null`.
            if (block == null)
                // Останавливает копание, если первый подходящий коллайдер принадлежит постороннему объекту.
                return;

            // Проверяет условие `block.Kind == BlockKind.Concrete`.
            if (block.Kind == BlockKind.Concrete)
                // Останавливает копание на бетонном блоке.
                return;

            // Копает вокруг центра блока с радиусом текущего уровня лопаты.
            site.Dig(block.transform.position, DigRadius);
            // Завершает выполнение текущего метода.
            return;
        }  // Конец цикла по индексам.
    }  // Конец метода `TryDig`.

    // Подбирает предмет или открывает магазин, с которым взаимодействует игрок.
    void TryUse()
    {  // Начало метода `TryUse`.
        // Задаёт `item` значением первый непустой результат из `Aimed()` и `NearbyTool()`.
        var item = Aimed() ?? NearbyTool();
        // Для лопаты выполняет отдельную ветку взаимодействия.
        if (item is ShovelItem shovel)
        {  // Начало ветки условия.
            // Вызывает `shovel.Take` с аргументами `this`.
            shovel.Take(this);
            // Сохраняет текущий игровой прогресс через `GameSave.WriteCurrent`.
            GameSave.WriteCurrent();
            // Завершает выполнение текущего метода.
            return;
        }  // Конец ветки условия.

        // Для выкопанного предмета проверяет возможность подбора.
        if (item is ResourcePickup pickup)
        {  // Начало ветки условия.
            // Проверяет, удалось ли положить находку в рюкзак или засчитать клад.
            if (!pickup.TryTake(bag))
            {  // Начало ветки условия.
                // Показывает временную подсказку через `ShowHint` с аргументами `"Рюкзак полон"`.
                ShowHint("Рюкзак полон");
                // Завершает выполнение текущего метода.
                return;
            }  // Конец ветки условия.
            // Проигрывает звук монет через `DigFeedback.Coin`.
            DigFeedback.Coin();
            // Сохраняет текущий игровой прогресс через `GameSave.WriteCurrent`.
            GameSave.WriteCurrent();
            // Завершает выполнение текущего метода.
            return;
        }  // Конец ветки условия.

        // Проверяет условие `item is ShopCounter counter`.
        if (item is ShopCounter counter)
            // Открывает нужный магазин и освобождает курсор через `OpenShop` с аргументами `counter.Kind`.
            OpenShop(counter.Kind);
    }  // Конец метода `TryUse`.

    // Открывает нужный магазин и освобождает курсор.
    public void OpenShop(ShopKind kind)
    {  // Начало метода `OpenShop`.
        // Задаёт тип открытого магазина `openShop` значением `kind`.
        openShop = kind;
        // Освобождает курсор для работы с интерфейсом.
        Cursor.lockState = CursorLockMode.None;
        // Показывает курсор.
        Cursor.visible = true;
        // Показывает временную подсказку через `ShowHint` с аргументами `""`.
        ShowHint("");
    }  // Конец метода `OpenShop`.

    // Закрывает магазин и возвращает управление мышью.
    public void CloseShop()
    {  // Начало метода `CloseShop`.
        // Задаёт тип открытого магазина `openShop` значением отсутствующую ссылку (`null`).
        openShop = null;
        // Проверяет условие `!paused`.
        if (!paused)
            // Захватывает курсор и скрывает его для управления видом через `LockCursor`.
            LockCursor();
    }  // Конец метода `CloseShop`.

    // Обрабатывает переключение паузы с учётом открытых окон.
    void TogglePause()
    {  // Начало метода `TogglePause`.
        // Проверяет условие `openShop.HasValue`.
        if (openShop.HasValue)
        {  // Начало ветки условия.
            // Закрывает магазин и возвращает управление мышью через `CloseShop`.
            CloseShop();
            // Завершает выполнение текущего метода.
            return;
        }  // Конец ветки условия.
        // Проверяет условие `paused && hud != null && hud.PauseSettingsOpen`.
        if (paused && hud != null && hud.PauseSettingsOpen)
        {  // Начало ветки условия.
            // Вызывает `hud.ClosePauseSettings`.
            hud.ClosePauseSettings();
            // Завершает выполнение текущего метода.
            return;
        }  // Конец ветки условия.
        // Проверяет условие `paused`.
        if (paused)
            // Продолжает игру после паузы и возвращает захват курсора через `Resume`.
            Resume();
        // Обрабатывает случай, когда предыдущее условие ложно.
        else
            // Сохраняет прогресс, останавливает игровое время и показывает паузу через `EnterPause`.
            EnterPause();
    }  // Конец метода `TogglePause`.

    // Сохраняет прогресс, останавливает игровое время и показывает паузу.
    void EnterPause()
    {  // Начало метода `EnterPause`.
        // Устанавливает признак паузы `paused` в `true`.
        paused = true;
        // Останавливает ход игрового времени.
        Time.timeScale = 0f;
        // Освобождает курсор для работы с интерфейсом.
        Cursor.lockState = CursorLockMode.None;
        // Показывает курсор.
        Cursor.visible = true;
        // Проверяет условие `hud != null`.
        if (hud != null)
            // Вызывает `hud.SetPaused` с аргументами `true`.
            hud.SetPaused(true);
        // Обновляет подсказку, рюкзак и видимость окна торговли через `PushHud` с аргументами `""`.
        PushHud("");
    }  // Конец метода `EnterPause`.

    // Продолжает игру после паузы и возвращает захват курсора.
    public void Resume()
    {  // Начало метода `Resume`.
        // Устанавливает признак паузы `paused` в `false`.
        paused = false;
        // Возвращает нормальную скорость игрового времени.
        Time.timeScale = 1f;
        // Проверяет условие `hud != null`.
        if (hud != null)
        {  // Начало ветки условия.
            // Вызывает `hud.SetPaused` с аргументами `false`.
            hud.SetPaused(false);
            // Вызывает `hud.ClosePauseSettings`.
            hud.ClosePauseSettings();
        }  // Конец ветки условия.
        // Захватывает курсор и скрывает его для управления видом через `LockCursor`.
        LockCursor();
    }  // Конец метода `Resume`.

    // Записывает прогресс и загружает главное меню.
    public void LeaveToMenu()
    {  // Начало метода `LeaveToMenu`.
        // Возвращает нормальную скорость игрового времени.
        Time.timeScale = 1f;
        // Сохраняет текущий игровой прогресс через `GameSave.WriteCurrent`.
        GameSave.WriteCurrent();
        // Загружает сцену через `SceneManager.LoadScene` с аргументами `"MainMenu"`.
        SceneManager.LoadScene("MainMenu");
    }  // Конец метода `LeaveToMenu`.

    // Подбирает подсказку для предмета перед игроком или рядом с ним.
    string NearestPrompt()
    {  // Начало метода `NearestPrompt`.
        // Задаёт `item` значением первый непустой результат из `Aimed()` и `NearbyTool()`.
        var item = Aimed() ?? NearbyTool();
        // Проверяет условие `item is ShovelItem`.
        if (item is ShovelItem)
            // Возвращает `"E — взять лопату"`.
            return "E — взять лопату";
        // Проверяет условие `item is ResourcePickup pickup`.
        if (item is ResourcePickup pickup)
            // Возвращает `"E — взять "` плюс вызывает `ResourceInfo.Title` с аргументами `pickup.Kind`.
            return "E — взять " + ResourceInfo.Title(pickup.Kind);
        // Проверяет условие `item is SellerShop`.
        if (item is SellerShop)
            // Возвращает `"E — магазин"`.
            return "E — магазин";
        // Проверяет условие `item is AnvilShop`.
        if (item is AnvilShop)
            // Возвращает `"E — улучшить"`.
            return "E — улучшить";
        // Проверяет условие `item is ShopCounter counter`.
        if (item is ShopCounter counter)
            // Выбирает возвращаемое значение по условию `counter.Kind` равно улучшения.
            return counter.Kind == ShopKind.Upgrade ? "E — улучшить" : "";
        // Возвращает `""`.
        return "";
    }  // Конец метода `NearestPrompt`.

    // Сортирует попадания луча и выбирает доступный предмет под прицелом.
    Component Aimed()
    {  // Начало метода `Aimed`.
        // Собирает все попадания физического луча через `Physics.RaycastAll` с аргументами `view.position, view.forward, 2.8f, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Collide` и записывает результат в `hits`.
        var hits = Physics.RaycastAll(view.position, view.forward, 2.8f, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Collide);
        // Сортирует элементы через `System.Array.Sort` с аргументами `hits, (a, b) => a.distance.CompareTo(b.distance)`.
        System.Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance));
        // Перебирает индексы начиная с `int i = 0` при условии `i < hits.Length`.
        for (int i = 0; i < hits.Length; i++)
        {  // Начало цикла по индексам.
            // Задаёт `collider` значением `hits[i].collider`.
            var collider = hits[i].collider;
            // Проверяет условие `collider.GetComponentInParent<FirstPersonPlayer>() != null`.
            if (collider.GetComponentInParent<FirstPersonPlayer>() != null)
                // Пропускает остаток этой итерации и переходит к следующей.
                continue;
            // Ищет компонент в объекте и его родителях через `collider.GetComponentInParent<ResourcePickup>` и записывает результат в `pickup`.
            var pickup = collider.GetComponentInParent<ResourcePickup>();
            // Проверяет условие `pickup != null`.
            if (pickup != null)
                // Возвращает `pickup`.
                return pickup;
            // Ищет компонент в объекте и его родителях через `collider.GetComponentInParent<ShovelItem>` и записывает результат в `shovel`.
            var shovel = collider.GetComponentInParent<ShovelItem>();
            // Проверяет условие `shovel != null`.
            if (shovel != null)
                // Возвращает `shovel`.
                return shovel;
            // Ищет компонент в объекте и его родителях через `collider.GetComponentInParent<SellerShop>` и записывает результат в `seller`.
            var seller = collider.GetComponentInParent<SellerShop>();
            // Проверяет условие `seller != null`.
            if (seller != null)
                // Возвращает `seller`.
                return seller;
            // Ищет компонент в объекте и его родителях через `collider.GetComponentInParent<AnvilShop>` и записывает результат в `anvil`.
            var anvil = collider.GetComponentInParent<AnvilShop>();
            // Проверяет условие `anvil != null`.
            if (anvil != null)
                // Возвращает `anvil`.
                return anvil;
            // Ищет компонент в объекте и его родителях через `collider.GetComponentInParent<ShopCounter>` и записывает результат в `shop`.
            var shop = collider.GetComponentInParent<ShopCounter>();
            // Проверяет условие `shop != null`.
            if (shop != null)
                // Возвращает `shop`.
                return shop;
            // Проверяет условие `collider.GetComponent<DigBlock>() != null`.
            if (collider.GetComponent<DigBlock>() != null)
                // Возвращает `null`, то есть отсутствие результата поиска.
                return null;
        }  // Конец цикла по индексам.
        // Возвращает `null`, то есть отсутствие результата поиска.
        return null;
    }  // Конец метода `Aimed`.

    // Ищет ближайший инструмент или магазин в передней части поля зрения.
    Component NearbyTool()
    {  // Начало метода `NearbyTool`.
        // Собирает коллайдеры в сфере через `Physics.OverlapSphere` с аргументами `view.position, 2.2f` и записывает результат в `hits`.
        var hits = Physics.OverlapSphere(view.position, 2.2f);
        // Инициализирует `best` значением `null`.
        Component best = null;
        // Задаёт `bestDistance` значением `99`.
        float bestDistance = 99f;
        // Перебирает индексы начиная с `int i = 0` при условии `i < hits.Length`.
        for (int i = 0; i < hits.Length; i++)
        {  // Начало цикла по индексам.
            // Ищет компонент в объекте и его родителях через `hits[i].GetComponentInParent<ShovelItem>` и записывает результат в `item`.
            Component item = hits[i].GetComponentInParent<ShovelItem>();
            // Проверяет условие `item == null`.
            if (item == null)
                // Ищет компонент в объекте и его родителях через `hits[i].GetComponentInParent<SellerShop>` и записывает результат в `item`.
                item = hits[i].GetComponentInParent<SellerShop>();
            // Проверяет условие `item == null`.
            if (item == null)
                // Ищет компонент в объекте и его родителях через `hits[i].GetComponentInParent<AnvilShop>` и записывает результат в `item`.
                item = hits[i].GetComponentInParent<AnvilShop>();
            // Проверяет условие `item == null`.
            if (item == null)
                // Ищет компонент в объекте и его родителях через `hits[i].GetComponentInParent<ShopCounter>` и записывает результат в `item`.
                item = hits[i].GetComponentInParent<ShopCounter>();
            // Проверяет условие `item == null`.
            if (item == null)
                // Пропускает остаток этой итерации и переходит к следующей.
                continue;
            // Вычисляет `to` как `item.transform.position` минус `view.position`.
            Vector3 to = item.transform.position - view.position;
            // Задаёт `distance` значением `to.magnitude`.
            float distance = to.magnitude;
            // Проверяет условие `distance > 2.2f`.
            if (distance > 2.2f)
                // Пропускает остаток этой итерации и переходит к следующей.
                continue;
            // Отбрасывает кандидата, если он слишком далеко в стороне или позади игрока.
            if (Vector3.Dot(view.forward, to.normalized) < 0.15f)
                // Пропускает остаток этой итерации и переходит к следующей.
                continue;
            // Проверяет условие `distance < bestDistance`.
            if (distance < bestDistance)
            {  // Начало ветки условия.
                // Задаёт `bestDistance` значением `distance`.
                bestDistance = distance;
                // Задаёт `best` значением `item`.
                best = item;
            }  // Конец ветки условия.
        }  // Конец цикла по индексам.
        // Возвращает `best`.
        return best;
    }  // Конец метода `NearbyTool`.

    // Распознаёт клавиши покупки улучшений в кузнице.
    void Trade(Keyboard keyboard)
    {  // Начало метода `Trade`.
        // Проверяет условие `openShop == ShopKind.Sell`.
        if (openShop == ShopKind.Sell)
            // Завершает выполнение текущего метода.
            return;
        // Проверяет условие `keyboard.digit1Key.wasPressedThisFrame`.
        if (keyboard.digit1Key.wasPressedThisFrame)
            // Оплачивает следующий уровень лопаты и меняет её внешний вид через `BuyShovel`.
            BuyShovel();
        // Проверяет условие `keyboard.digit2Key.wasPressedThisFrame`.
        if (keyboard.digit2Key.wasPressedThisFrame)
            // Покупает ещё одно место в рюкзаке через `BuySlot`.
            BuySlot();
        // Проверяет условие `keyboard.digit3Key.wasPressedThisFrame`.
        if (keyboard.digit3Key.wasPressedThisFrame)
            // Покупает следующий уровень дополнительных прыжков через `BuyDoubleJumpBoots`.
            BuyDoubleJumpBoots();
    }  // Конец метода `Trade`.

    // Покупает следующий уровень дополнительных прыжков.
    public void BuyDoubleJumpBoots()
    {  // Начало метода `BuyDoubleJumpBoots`.
        // Проверяет условие `jumpBootsLevel >= PlayerUpgrades.MaxJumpBootsLevel`.
        if (jumpBootsLevel >= PlayerUpgrades.MaxJumpBootsLevel)
        {  // Начало ветки условия.
            // Показывает временную подсказку через `ShowHint` с аргументами `"Прыжки уже на максимуме"`.
            ShowHint("Прыжки уже на максимуме");
            // Завершает выполнение текущего метода.
            return;
        }  // Конец ветки условия.
        // Вычисляет `next` как уровень дополнительных прыжков `jumpBootsLevel` плюс `1`.
        int next = jumpBootsLevel + 1;
        // Задаёт `cost` значением элемент `PlayerUpgrades.JumpBootsCost` с индексом `next`.
        int cost = PlayerUpgrades.JumpBootsCost[next];
        // Проверяет условие `bag.Money < cost`.
        if (bag.Money < cost)
        {  // Начало ветки условия.
            // Показывает временную подсказку через `ShowHint` с аргументами `"Не хватает денег"`.
            ShowHint("Не хватает денег");
            // Завершает выполнение текущего метода.
            return;
        }  // Конец ветки условия.
        // Списывает `cost` монет.
        bag.Money -= cost;
        // Задаёт уровень дополнительных прыжков `jumpBootsLevel` значением `next`.
        jumpBootsLevel = next;
        // Проверяет условие `next == 1`.
        if (next == 1)
            // Показывает временную подсказку через `ShowHint` с аргументами `"Теперь можно прыгнуть второй раз в воздухе"`.
            ShowHint("Теперь можно прыгнуть второй раз в воздухе");
        // Если предыдущая проверка не сработала, проверяет `next == 2`.
        else if (next == 2)
            // Показывает временную подсказку через `ShowHint` с аргументами `"Тройной прыжок: три раза в воздухе"`.
            ShowHint("Тройной прыжок: три раза в воздухе");
        // Обрабатывает случай, когда предыдущее условие ложно.
        else
            // Показывает временную подсказку через `ShowHint` с аргументами `"Четверной прыжок: четыре раза в воздухе"`.
            ShowHint("Четверной прыжок: четыре раза в воздухе");
        // Сохраняет текущий игровой прогресс через `GameSave.WriteCurrent`.
        GameSave.WriteCurrent();
    }  // Конец метода `BuyDoubleJumpBoots`.

    // Обменивает всё содержимое рюкзака на монеты.
    public void SellAll()
    {  // Начало метода `SellAll`.
        // Вызывает `bag.TakeAll` и записывает результат в `value`.
        int value = bag.TakeAll();
        // Проверяет условие `value <= 0`.
        if (value <= 0)
        {  // Начало ветки условия.
            // Показывает временную подсказку через `ShowHint` с аргументами `"Рюкзак пуст"`.
            ShowHint("Рюкзак пуст");
            // Завершает выполнение текущего метода.
            return;
        }  // Конец ветки условия.
        // Начисляет `value` монет.
        bag.Money += value;
        // Проигрывает звук монет через `DigFeedback.Coin`.
        DigFeedback.Coin();
        // Сохраняет текущий игровой прогресс через `GameSave.WriteCurrent`.
        GameSave.WriteCurrent();
    }  // Конец метода `SellAll`.

    // Продаёт один предмет или все предметы указанного вида.
    public void Sell(ResourceKind kind, bool all)
    {  // Начало метода `Sell`.
        // Вызывает `bag.CountOf` с аргументами `kind` и записывает результат в `have`.
        int have = bag.CountOf(kind);
        // Проверяет условие `have == 0`.
        if (have == 0)
        {  // Начало ветки условия.
            // Показывает временную подсказку через `ShowHint` с аргументами `"Этого нет в рюкзаке"`.
            ShowHint("Этого нет в рюкзаке");
            // Завершает выполнение текущего метода.
            return;
        }  // Конец ветки условия.
        // Вызывает `bag.Take` с аргументами `kind, all ? have : 1` и записывает результат в `sold`.
        int sold = bag.Take(kind, all ? have : 1);
        // Начисляет `sold * ResourceInfo.Price(kind)` монет.
        bag.Money += sold * ResourceInfo.Price(kind);
        // Проигрывает звук монет через `DigFeedback.Coin`.
        DigFeedback.Coin();
        // Сохраняет текущий игровой прогресс через `GameSave.WriteCurrent`.
        GameSave.WriteCurrent();
    }  // Конец метода `Sell`.

    // Оплачивает следующий уровень лопаты и меняет её внешний вид.
    public void BuyShovel()
    {  // Начало метода `BuyShovel`.
        // Проверяет условие `!hasShovel`.
        if (!hasShovel)
        {  // Начало ветки условия.
            // Показывает временную подсказку через `ShowHint` с аргументами `"Сначала найди лопату"`.
            ShowHint("Сначала найди лопату");
            // Завершает выполнение текущего метода.
            return;
        }  // Конец ветки условия.
        // Проверяет условие `shovelTier >= ShovelTiers.UpgradeCost.Length`.
        if (shovelTier >= ShovelTiers.UpgradeCost.Length)
        {  // Начало ветки условия.
            // Показывает временную подсказку через `ShowHint` с аргументами `"Лопата уже лучшая"`.
            ShowHint("Лопата уже лучшая");
            // Завершает выполнение текущего метода.
            return;
        }  // Конец ветки условия.
        // Задаёт `cost` значением элемент `ShovelTiers.UpgradeCost` с индексом `shovelTier`.
        int cost = ShovelTiers.UpgradeCost[shovelTier];
        // Проверяет условие `bag.Money < cost`.
        if (bag.Money < cost)
        {  // Начало ветки условия.
            // Показывает временную подсказку через `ShowHint` с аргументами `"Не хватает денег"`.
            ShowHint("Не хватает денег");
            // Завершает выполнение текущего метода.
            return;
        }  // Конец ветки условия.
        // Списывает `cost` монет.
        bag.Money -= cost;
        // Увеличивает уровень лопаты `shovelTier` на единицу.
        shovelTier++;
        // Вызывает `ShovelTint.Apply` с аргументами `shovelView, shovelTier`.
        ShovelTint.Apply(shovelView, shovelTier);
        // Сохраняет текущий игровой прогресс через `GameSave.WriteCurrent`.
        GameSave.WriteCurrent();
    }  // Конец метода `BuyShovel`.

    // Покупает ещё одно место в рюкзаке.
    public void BuySlot()
    {  // Начало метода `BuySlot`.
        // Проверяет условие `bag.Capacity >= ShovelTiers.MaxSlots`.
        if (bag.Capacity >= ShovelTiers.MaxSlots)
        {  // Начало ветки условия.
            // Показывает временную подсказку через `ShowHint` с аргументами `"Рюкзак уже полный по слотам"`.
            ShowHint("Рюкзак уже полный по слотам");
            // Завершает выполнение текущего метода.
            return;
        }  // Конец ветки условия.
        // Вызывает `ShovelTiers.NextSlotCost` с аргументами `bag.Capacity` и записывает результат в `cost`.
        int cost = ShovelTiers.NextSlotCost(bag.Capacity);
        // Проверяет условие `bag.Money < cost`.
        if (bag.Money < cost)
        {  // Начало ветки условия.
            // Показывает временную подсказку через `ShowHint` с аргументами `"Не хватает денег"`.
            ShowHint("Не хватает денег");
            // Завершает выполнение текущего метода.
            return;
        }  // Конец ветки условия.
        // Списывает `cost` монет.
        bag.Money -= cost;
        // Увеличивает `bag.Capacity` на единицу.
        bag.Capacity++;
        // Сохраняет текущий игровой прогресс через `GameSave.WriteCurrent`.
        GameSave.WriteCurrent();
    }  // Конец метода `BuySlot`.

    // Формирует текстовое представление цен и действий магазина.
    string ShopText()
    {  // Начало метода `ShopText`.
        // Проверяет условие `openShop == ShopKind.Sell`.
        if (openShop == ShopKind.Sell)
        {  // Начало ветки условия.
            // Возвращает `"Продажа. Esc — выйти\n\n" + Line(ResourceKind.Stone, "1", "6")` плюс `Line(ResourceKind.Coal, "2", "7")` плюс формирует строку о количестве и цене отдельного ресурса через `Line` с аргументами `ResourceKind.Copper, "3", "8"` плюс формирует строку о количестве и цене отдельного ресурса через `Line` с аргументами `ResourceKind.Iron, "4", "9"` плюс формирует строку о количестве и цене отдельного ресурса через `Line` с аргументами `ResourceKind.Gold, "5", "0"`.
            return "Продажа. Esc — выйти\n\n"
                // Добавляет к формируемому значению `Line(ResourceKind.Stone, "1", "6")`.
                + Line(ResourceKind.Stone, "1", "6")
                // Добавляет к формируемому значению `Line(ResourceKind.Coal, "2", "7")`.
                + Line(ResourceKind.Coal, "2", "7")
                // Добавляет к формируемому значению `Line(ResourceKind.Copper, "3", "8")`.
                + Line(ResourceKind.Copper, "3", "8")
                // Добавляет к формируемому значению `Line(ResourceKind.Iron, "4", "9")`.
                + Line(ResourceKind.Iron, "4", "9")
                // Добавляет к формируемому значению `Line(ResourceKind.Gold, "5", "0")`.
                + Line(ResourceKind.Gold, "5", "0");
        }  // Конец ветки условия.

        // Выбирает значение для `shovelLine` по условию уровень лопаты `shovelTier` не меньше `ShovelTiers.UpgradeCost.Length`.
        string shovelLine = shovelTier >= ShovelTiers.UpgradeCost.Length
            // При истинном условии выбирает `"1 — лопата " + ShovelTiers.LevelLabel(shovelTier) + ", улучшений нет\n"`.
            ? "1 — лопата " + ShovelTiers.LevelLabel(shovelTier) + ", улучшений нет\n"
            // При ложном условии выбирает `"1 — " + ShovelTiers.LevelLabel(shovelTier + 1) + " за " + ShovelTiers.UpgradeCost[shovelTier] + "\n"`.
            : "1 — " + ShovelTiers.LevelLabel(shovelTier + 1) + " за " + ShovelTiers.UpgradeCost[shovelTier] + "\n";
        // Выбирает значение для `slotLine` по условию `bag.Capacity` не меньше `ShovelTiers.MaxSlots`.
        string slotLine = bag.Capacity >= ShovelTiers.MaxSlots
            // При истинном условии выбирает `"2 — слотов " + bag.Capacity + ", это потолок"`.
            ? "2 — слотов " + bag.Capacity + ", это потолок"
            // При ложном условии выбирает `"2 — слот рюкзака " + bag.Capacity + " → " + (bag.Capacity + 1) + " за " + ShovelTiers.NextSlotCost(bag.Capacity)`.
            : "2 — слот рюкзака " + bag.Capacity + " → " + (bag.Capacity + 1) + " за " + ShovelTiers.NextSlotCost(bag.Capacity);
        // Объявляет локальную переменную `bootLine` типа `string`.
        string bootLine;
        // Проверяет условие `jumpBootsLevel >= PlayerUpgrades.MaxJumpBootsLevel`.
        if (jumpBootsLevel >= PlayerUpgrades.MaxJumpBootsLevel)
            // Задаёт `bootLine` значением `"3 — прыжки на максимуме ("` плюс уровень дополнительных прыжков `jumpBootsLevel` плюс `" в воздухе)"`.
            bootLine = "3 — прыжки на максимуме (" + jumpBootsLevel + " в воздухе)";
        // Обрабатывает случай, когда предыдущее условие ложно.
        else
        {  // Начало запасной ветки условия.
            // Вычисляет `next` как уровень дополнительных прыжков `jumpBootsLevel` плюс `1`.
            int next = jumpBootsLevel + 1;
            // Задаёт `bootLine` значением `"3 — "` плюс `PlayerUpgrades.JumpBootsCaption(next).ToLower()` плюс `" за "` плюс элемент `PlayerUpgrades.JumpBootsCost` с индексом `next` плюс `" монет"`.
            bootLine = "3 — " + PlayerUpgrades.JumpBootsCaption(next).ToLower() + " за "
                // Добавляет к формируемому значению `PlayerUpgrades.JumpBootsCost[next] + " монет"`.
                + PlayerUpgrades.JumpBootsCost[next] + " монет";
        }  // Конец запасной ветки условия.
        // Возвращает `"Кузница. Деньги: " + bag.Money + ". Esc — выйти\n\n"` плюс `shovelLine` плюс `slotLine` плюс `"\n"` плюс `bootLine`.
        return "Кузница. Деньги: " + bag.Money + ". Esc — выйти\n\n" + shovelLine + slotLine + "\n" + bootLine;
    }  // Конец метода `ShopText`.

    // Формирует строку о количестве и цене отдельного ресурса.
    string Line(ResourceKind kind, string oneKey, string allKey)
    {  // Начало метода `Line`.
        // Возвращает `oneKey + " один, " + allKey + " все " + ResourceInfo.Title(kind) + " × "` плюс `bag.CountOf(kind)` плюс `" по "` плюс вызывает `ResourceInfo.Price` с аргументами `kind` плюс `"\n"`.
        return oneKey + " один, " + allKey + " все  "
            // Добавляет к формируемому значению `ResourceInfo.Title(kind) + " × " + bag.CountOf(kind)`.
            + ResourceInfo.Title(kind) + " × " + bag.CountOf(kind)
            // Добавляет к формируемому значению `"  по " + ResourceInfo.Price(kind) + "\n"`.
            + "  по " + ResourceInfo.Price(kind) + "\n";
    }  // Конец метода `Line`.

    // Проверяет код монет и выводит результат игроку.
    public bool TryRedeemCode(string raw)
    {  // Начало метода `TryRedeemCode`.
        // Проверяет условие `!GameSettings.TryRedeemCoinCode(raw, out var message)`.
        if (!GameSettings.TryRedeemCoinCode(raw, out var message))
        {  // Начало ветки условия.
            // Проверяет условие `message != null`.
            if (message != null)
            {  // Начало ветки условия.
                // Показывает временную подсказку через `ShowHint` с аргументами `message`.
                ShowHint(message);
                // Обновляет подсказку, рюкзак и видимость окна торговли через `PushHud` с аргументами `NearestPrompt()`.
                PushHud(NearestPrompt());
            }  // Конец ветки условия.

            // Возвращает `false`.
            return false;
        }  // Конец ветки условия.

        // Показывает временную подсказку через `ShowHint` с аргументами `message`.
        ShowHint(message);
        // Обновляет подсказку, рюкзак и видимость окна торговли через `PushHud` с аргументами `NearestPrompt()`.
        PushHud(NearestPrompt());
        // Возвращает `true`.
        return true;
    }  // Конец метода `TryRedeemCode`.

    // Запоминает временное сообщение и срок его показа.
    void ShowHint(string text)
    {  // Начало метода `ShowHint`.
        // Задаёт `hint` значением `text`.
        hint = text;
        // Задаёт время окончания показа подсказки `hintUntil` значением игровое время `Time.time` плюс `1.6`.
        hintUntil = Time.time + 1.6f;
    }  // Конец метода `ShowHint`.

    // Возвращает захват курсора при возобновлении игры.
    public void RelockForPlay()
    {  // Начало метода `RelockForPlay`.
        // Захватывает курсор и скрывает его для управления видом через `LockCursor`.
        LockCursor();
    }  // Конец метода `RelockForPlay`.

    // Захватывает курсор и скрывает его для управления видом.
    static void LockCursor()
    {  // Начало метода `LockCursor`.
        // Захватывает курсор для обзора мышью.
        Cursor.lockState = CursorLockMode.Locked;
        // Скрывает курсор.
        Cursor.visible = false;
    }  // Конец метода `LockCursor`.
}  // Конец класса `FirstPersonPlayer`.

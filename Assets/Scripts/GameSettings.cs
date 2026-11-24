// Подключает доступ к базовым типам игрового движка.
using UnityEngine;

// Хранит громкость и чувствительность мыши и проверяет код на монеты.
public static class GameSettings
{  // Начало класса `GameSettings`.
    // Задаёт ключ громкости в PlayerPrefs `VolumeKey` значением `"th_volume"`.
    const string VolumeKey = "th_volume";
    // Задаёт ключ чувствительности в PlayerPrefs `SensitivityKey` значением `"th_sensitivity"`.
    const string SensitivityKey = "th_sensitivity";

    // Задаёт код начисления монет `CoinCheatCode` значением `"999999"`.
    public const string CoinCheatCode = "999999";
    // Задаёт размер награды за код `CoinCheatReward` значением `10000`.
    public const int CoinCheatReward = 10000;

    // Объявляет свойство `Volume` и правила доступа к нему.
    public static float Volume
    {  // Начало свойства и его методов доступа.
        // Описывает чтение свойства. Возвращает результат вызова `PlayerPrefs.GetFloat(VolumeKey, 0.8f)`.
        get { return PlayerPrefs.GetFloat(VolumeKey, 0.8f); }
        // Описывает присваивание свойства через входное значение `value`.
        set
        {  // Начало блока записи свойства.
            // Задаёт числовой параметр через `PlayerPrefs.SetFloat` с аргументами `VolumeKey, Mathf.Clamp01(value)`.
            PlayerPrefs.SetFloat(VolumeKey, Mathf.Clamp01(value));
            // Применяет текущую громкость к звуку Unity через `Apply`.
            Apply();
        }  // Конец блока записи свойства.
    }  // Конец свойства и его методов доступа.

    // Объявляет свойство `Sensitivity` и правила доступа к нему.
    public static float Sensitivity
    {  // Начало свойства и его методов доступа.
        // Описывает чтение свойства. Возвращает результат вызова `PlayerPrefs.GetFloat(SensitivityKey, 0.1f)`.
        get { return PlayerPrefs.GetFloat(SensitivityKey, 0.1f); }
        // Описывает запись свойства. Задаёт числовой параметр через `PlayerPrefs.SetFloat` с аргументами `SensitivityKey, Mathf.Clamp(value, 0.03f, 0.35f)`.
        set { PlayerPrefs.SetFloat(SensitivityKey, Mathf.Clamp(value, 0.03f, 0.35f)); }
    }  // Конец свойства и его методов доступа.

    // Применяет текущую громкость к звуку Unity.
    public static void Apply()
    {  // Начало метода `Apply`.
        // Задаёт громкость `AudioListener.volume` значением `Volume`.
        AudioListener.volume = Volume;
    }  // Конец метода `Apply`.

    // Проверяет введённый код и начисляет монеты в имеющуюся игру.
    public static bool TryRedeemCoinCode(string raw, out string message)
    {  // Начало метода `TryRedeemCoinCode`.
        // Задаёт `message` значением отсутствующую ссылку (`null`).
        message = null;
        // Проверяет условие `string.IsNullOrWhiteSpace(raw)`.
        if (string.IsNullOrWhiteSpace(raw))
            // Возвращает `false`.
            return false;
        // Проверяет условие `raw.Trim() != CoinCheatCode`.
        if (raw.Trim() != CoinCheatCode)
        {  // Начало ветки условия.
            // Задаёт `message` значением `"Неверный код"`.
            message = "Неверный код";
            // Возвращает `false`.
            return false;
        }  // Конец ветки условия.

        // Проверяет условие `!GameSave.AddMoney(CoinCheatReward)`.
        if (!GameSave.AddMoney(CoinCheatReward))
        {  // Начало ветки условия.
            // Задаёт `message` значением `"Сначала начни или продолжи игру"`.
            message = "Сначала начни или продолжи игру";
            // Возвращает `false`.
            return false;
        }  // Конец ветки условия.

        // Задаёт `message` значением `"Получено "` плюс размер награды за код `CoinCheatReward` плюс `" монет"`.
        message = "Получено " + CoinCheatReward + " монет";
        // Возвращает `true`.
        return true;
    }  // Конец метода `TryRedeemCoinCode`.
}  // Конец класса `GameSettings`.

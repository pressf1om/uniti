// Подключает доступ к типам асинхронных задач.
using System.Threading.Tasks;

// Шаблон Unity Gaming Services: в сборке двора не тянем Remote Config в player.
// Предоставляет асинхронный признак включения LiveOps.
public static class LiveOps
{  // Начало класса `LiveOps`.
    // Задаёт `EnabledKey` значением `"liveops_enabled"`.
    public const string EnabledKey = "liveops_enabled";

    // Возвращает готовую задачу со значением флага LiveOps.
    public static Task<bool> IsEnabledAsync()
    {  // Начало метода `IsEnabledAsync`.
        // Возвращает уже завершённую задачу со значением true.
        return Task.FromResult(true);
    }  // Конец метода `IsEnabledAsync`.
}  // Конец класса `LiveOps`.
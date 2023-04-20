using ScheduleSender.Bot;
using ScheduleSender.Types;
using ScheduleSender.Utils;

namespace ScheduleSender;

public static class Program
{
    public static void Main() => MainAsync().GetAwaiter().GetResult();

    public static async Task MainAsync()
    {
        await TelegramBot.Start();
    }
}

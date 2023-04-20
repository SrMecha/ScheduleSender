using ScheduleSender.Bot;

namespace ScheduleSender;

public static class Program
{
    public static void Main() => MainAsync().GetAwaiter().GetResult();

    public static async Task MainAsync()
    {
        await TelegramBot.Start();
    }
}

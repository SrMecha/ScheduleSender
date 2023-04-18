using ScheduleSender.Bot;
using ScheduleSender.Types;
using ScheduleSender.Utils;

namespace ScheduleSender;

public static class Program
{
    public static void Main() => TelegramBot.Start().GetAwaiter().GetResult();
}

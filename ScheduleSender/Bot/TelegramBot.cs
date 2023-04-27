using ScheduleSender.Extensions;
using ScheduleSender.Types;
using ScheduleSender.Utils;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.ReplyMarkups;

namespace ScheduleSender.Bot;

public static class TelegramBot
{
    public static readonly ITelegramBotClient Client = new TelegramBotClient(EnvReader.GetEnviroment("TelegramBotToken")!);
    public static readonly string ChannelId = EnvReader.GetEnviroment("ChannelId")!;
    private static GroupSchedule? _lastSchedule = null;
    private static bool _isSchedulePosted = false;

    public static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        if (update.Type == Telegram.Bot.Types.Enums.UpdateType.Message)
        {
            var message = update.Message!;
            switch (message.Text)
            {
                case "ping":
                    await botClient.SendTextMessageAsync(message.Chat, "Бот работает", cancellationToken: cancellationToken);
                    break;
                case "schedule":
                    await SendSchedule(_lastSchedule ?? new GroupSchedule(), chatId: message.Chat);
                    break;
                case "load":
                    await SendSchedule(await ScheduleLoader.LoadSchedule(), chatId: message.Chat);
                    break;
            }
        }
    }

    public static async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(exception));
    }

    public static void StartScheduleSend()
    {
        _ = Task.Run(async () =>
        {
            while (await new PeriodicTimer(TimeSpan.FromMinutes(5)).WaitForNextTickAsync())
            {
                if (_lastSchedule is null)
                {
                    _lastSchedule = await ScheduleLoader.LoadSchedule();
                    _isSchedulePosted = false;
                }
                if (!_isSchedulePosted)
                {
                    await SendSchedule(_lastSchedule);
                    _isSchedulePosted = true;
                }
            }
        });
    }

    public static async Task SendSchedule(GroupSchedule schedule, ChatId? chatId = null)
    {
        using (var stream = ImageCreator.Create(schedule).ToStream())
        {
            var button = new InlineKeyboardMarkup(InlineKeyboardButton.WithUrl(
                schedule.Name,
                schedule.URL
                ));
            await Client.SendPhotoAsync(
                    chatId ?? ChannelId,
                    new InputOnlineFile(stream),
                    replyMarkup: button
                    );
            stream.Close();
        }
    }

    public static void StartReceiving()
    {
        var cts = new CancellationTokenSource();
        var cancellationToken = cts.Token;
        var receiverOptions = new ReceiverOptions
        {
            AllowedUpdates = { },// receive all update types
        };
        Client.StartReceiving(
            HandleUpdateAsync,
            HandleErrorAsync,
            receiverOptions,
            cancellationToken);
    }

    public static async Task Start()
    {
        Console.WriteLine($"{Client.GetMeAsync().Result.FirstName} запущен.");
        StartReceiving();
        StartScheduleSend();
        await Task.Delay(Timeout.Infinite);
    }
}

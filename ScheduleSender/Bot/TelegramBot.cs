using ScheduleSender.Extensions;
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

    public static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        if (update.Type == Telegram.Bot.Types.Enums.UpdateType.Message)
        {
            await SendSchedule(update.Message!.Chat);
        }
    }

    public static async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(exception));
        throw exception;
    }

    public static void StartScheduleSend()
    {
        _ = Task.Run(async () =>
        {
            var timer = new PeriodicTimer(TimeSpan.FromMinutes(5));

            while (await timer.WaitForNextTickAsync())
            {
                if (await ScheduleLoader.IsNewSchedulePosted())
                    await SendSchedule();
            }
        });
    }

    public static async Task SendSchedule(ChatId? chatId = null)
    {
        var schedule = await ScheduleLoader.GetSchedule();
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

    public static async Task Start()
    {
        Console.WriteLine($"{Client.GetMeAsync().Result.FirstName} запущен.");

        var cts = new CancellationTokenSource();
        var cancellationToken = cts.Token;
        var receiverOptions = new ReceiverOptions
        {
            AllowedUpdates = { }, // receive all update types
        };
        Client.StartReceiving(
            HandleUpdateAsync,
            HandleErrorAsync,
            receiverOptions,
            cancellationToken
        );
        StartScheduleSend();
        Console.ReadLine();
        await Task.Delay(Timeout.Infinite);
    }
}

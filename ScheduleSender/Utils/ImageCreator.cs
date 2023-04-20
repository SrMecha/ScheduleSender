using ScheduleSender.Data;
using ScheduleSender.Extensions;
using ScheduleSender.Types;
using SkiaSharp;

namespace ScheduleSender.Utils;

public static class ImageCreator
{
    private static readonly DirectoryInfo _imagesDirectory = new(Path.GetFullPath($"../../../Images", AppDomain.CurrentDomain.BaseDirectory));
    private static readonly SKPaint _paint = new()
    { 
        TextSize = 45,
        Color = SKColor.Parse("#000000")
    };

    public static SKBitmap Create(GroupSchedule schedule)
    {
        var imageDirectoryPath = GetRandomImagePath();
        var config = ConfigReader.GetConfig($"{imageDirectoryPath}/config.json");
        var bitmap = SKBitmap.Decode($"{imageDirectoryPath}/schedule.png");
        DrawLessons(schedule, config, bitmap);
        DrawDate(schedule, config, bitmap);
        return bitmap;
    }
    public static string GetRandomImagePath()
    {
        return _imagesDirectory
            .GetDirectories()[new Random().Next(0, _imagesDirectory.GetDirectories().Length - 1)]
            .FullName;
    }
    private static void DrawLessons(GroupSchedule schedule, ScheduleDrawingConfig config, SKBitmap bitmap)
    {
        using (var canvas = new SKCanvas(bitmap))
        {
            for (int i = 0; i < schedule.Lessons.Count; i++)
            {
            canvas.DrawTextInBox(schedule.Lessons[i].Name,
                config.Lessons[i].Name.ToRectangle(),
                _paint);
            canvas.DrawTextInBox(schedule.Lessons[i].Office,
                config.Lessons[i].Office.ToRectangle(),
                _paint);
            }
        }
    }
    private static void DrawDate(GroupSchedule schedule, ScheduleDrawingConfig config, SKBitmap bitmap)
    {
        using (var canvas = new SKCanvas(bitmap))
        {
            if (config.Date is null)
                return;
            canvas.DrawTextInBox(schedule.Date,
                config.Date.ToRectangle(),
                _paint);
        }
    }
}

using ScheduleSender.Data;
using ScheduleSender.Types;
using System.Drawing;
using System.Drawing.Imaging;

namespace ScheduleSender.Utils;

public static class ImageCreator
{
    private static readonly DirectoryInfo _imagesDirectory = new(Path.GetFullPath($"../../../Images"));
    private static readonly StringFormat _textFormat = new(StringFormatFlags.LineLimit)
    {
        LineAlignment = StringAlignment.Center,
        Alignment = StringAlignment.Center
    };

    public static Image Create(GroupSchedule schedule)
    {
        var imageDirectoryPath = GetRandomImagePath();
        var image = Image.FromFile($"{imageDirectoryPath}/schedule.png");
        var config = ConfigReader.GetConfig($"{imageDirectoryPath}/config.json");
        Graphics graphic = Graphics.FromImage(image);
        DrawLessons(schedule, config, graphic);
        DrawDate(schedule, config, graphic);
        return image;
    }
    public static string GetRandomImagePath()
    {
        return _imagesDirectory
            .GetDirectories()[new Random().Next(0, _imagesDirectory.GetDirectories().Length - 1)]
            .FullName;
    }
    private static void DrawLessons(GroupSchedule schedule, ScheduleDrawingConfig config, Graphics graphic)
    {
        for (int i = 0; i < schedule.Lessons.Count; i++)
        {
            graphic.DrawString(schedule.Lessons[i].Name,
                new Font("Arial", 27, FontStyle.Bold),
                new SolidBrush(Color.Black), config.Lessons[i].Name.ToRectangle(),
                _textFormat);
            graphic.DrawString(schedule.Lessons[i].Office,
                new Font("Arial", 27, FontStyle.Bold),
                new SolidBrush(Color.Black), config.Lessons[i].Office.ToRectangle(),
                _textFormat);
        }
    }
    private static void DrawDate(GroupSchedule schedule, ScheduleDrawingConfig config, Graphics graphic)
    {
        if (config.Date is null)
            return;
        graphic.DrawString(schedule.Date,
            new Font("Arial", 27, FontStyle.Bold),
            new SolidBrush(Color.Black), config.Date.ToRectangle(),
            _textFormat);
    }
}

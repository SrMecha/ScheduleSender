using AngleSharp.Dom;
using ScheduleSender.Data;
using ScheduleSender.Types;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ScheduleSender.Utils;

public static class WebExcelReader
{
    public static GroupSchedule? FindGroup(IDocument document, string name)
    {
        var row = 0;
        var column = 0;
        var lowerName = name.ToLower();
        var isFinded = false;
        var table = document.GetElementsByClassName("waffle")[0];
        var scheduleElements = table.GetElementsByTagName("tbody")[0];
        foreach (var rowElements in scheduleElements.GetElementsByTagName("tr"))
        {
            foreach (var columnElement in rowElements.GetElementsByTagName("td"))
            {
                if (columnElement.TextContent != string.Empty)
                    if (columnElement.TextContent.ToLower() == lowerName)
                    {
                        isFinded = true;
                    }
                if (isFinded)
                    break;
                column++;
            }
            if (isFinded)
                break;
            column = 0;
            row++;
        }
        if (!isFinded)
            return null;
        var isLessonsStarted = false;
        var date = document.GetElementsByClassName("name")[0].TextContent.Split(":").Last();
        var result = new GroupScheduleData()
        {
            Name = $"Расписание на {date}",
            Date = date,
            URL = document.BaseUri
        };
        column++;
        while (true)
        {
            string lessonName;
            row++;
            try
            {
                lessonName = scheduleElements.GetElementsByTagName("tr")[row].GetElementsByTagName("td")[column].TextContent;
            }
            catch
            {
                continue;
            }
            if (lessonName == string.Empty)
            {
                if (isLessonsStarted)
                    break;
                result.Lessons.Add(new LessonData());
            }
            else
            {
                isLessonsStarted = true;
                result.Lessons.Add(new LessonData()
                {
                    Name = lessonName,
                    Office = scheduleElements.GetElementsByTagName("tr")[row].GetElementsByTagName("td")[column + 1].TextContent
                });
            }
        }
        return new GroupSchedule(result);
    }
}

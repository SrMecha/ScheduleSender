﻿using ScheduleSender.Data;

namespace ScheduleSender.Types;

public class GroupSchedule
{
    public string Name { get; set; }
    public string Date { get; init; }
    public List<Lesson> Lessons { get; init; }
    public string URL { get; init; }

    public GroupSchedule(GroupScheduleData data)
    {
        Name = data.Name;
        Date = data.Date;
        Lessons = ConfigureLessons(data.Lessons);
        URL = data.URL;
    }

    public GroupSchedule()
    {
        var data = new GroupScheduleData();
        Name = data.Name;
        Date = data.Date;
        Lessons = ConfigureLessons(data.Lessons);
        URL = data.URL;
    }

    private List<Lesson> ConfigureLessons(List<LessonData> lessonsData)
    {
        var result = new List<Lesson>();
        foreach (var data in lessonsData)
            result.Add(new Lesson(data));
        return result;
    }
}

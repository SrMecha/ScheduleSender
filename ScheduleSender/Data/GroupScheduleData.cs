namespace ScheduleSender.Data;

public class GroupScheduleData
{
    public string Date { get; set; } = "404";
    public List<LessonData> Lessons { get; set; } = new();
    public string URL { get; set; } = string.Empty;
}

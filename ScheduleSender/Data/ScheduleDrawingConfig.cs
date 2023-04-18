namespace ScheduleSender.Data;

public class ScheduleDrawingConfig
{
    public TextDrawingData? Date { get; init; }
    public List<LessonDrawingConfig> Lessons { get; init; } = new();

}

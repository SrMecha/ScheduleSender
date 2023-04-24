namespace ScheduleSender.Data;

public class ScheduleDrawingConfig
{
    public int TextSize { get; init; } = 5;
    public TextDrawingData? Date { get; init; }
    public List<LessonDrawingConfig> Lessons { get; init; } = new();

}

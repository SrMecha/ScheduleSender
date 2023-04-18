using ScheduleSender.Data;

namespace ScheduleSender.Types;

public class Lesson
{
    public string Name { get; init; }
    public string Office { get; init; }

    public Lesson(LessonData data)
    {
        Name = data.Name;
        Office = data.Office;
    }
}

using Newtonsoft.Json;
using ScheduleSender.Data;

namespace ScheduleSender.Utils;

public static class ConfigReader
{
    public static ScheduleDrawingConfig GetConfig(string path)
    {
        return JsonConvert.DeserializeObject<ScheduleDrawingConfig>(new StreamReader(path).ReadToEnd())!;
    }
}

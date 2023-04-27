using ScheduleSender.Types;

namespace ScheduleSender.Utils;

public static class ScheduleLoader
{
    public static async Task<GroupSchedule> LoadSchedule()
    {
        var scheduleURLs = SiteParser.ParseScheduleURLs(await SiteParser.OpenSiteAsync());
        foreach (var url in scheduleURLs)
        {
            var schedule = WebExcelReader.FindGroup(await SiteParser.OpenSiteAsync(url), "СБ-130");
            if (schedule is not null)
                return schedule;
        }
        return new GroupSchedule();
    }
}

using ScheduleSender.Types;

namespace ScheduleSender.Utils;

public static class ScheduleLoader
{
    private static GroupSchedule? _lastSchedule = null;
    public static async Task<GroupSchedule> LoadSchedule()
    {
        var scheduleURLs = SiteParser.ParseScheduleURLs(await SiteParser.OpenSiteAsync());
        GroupSchedule? groupSchedule = null;
        foreach (var url in scheduleURLs)
        {
            var schedule = WebExcelReader.FindGroup(await SiteParser.OpenSiteAsync(url), "СБ-130");
            if (schedule is not null)
            {
                _lastSchedule = schedule;
                groupSchedule = schedule;
            }
        }
        return groupSchedule ?? new GroupSchedule(new());
    }

    public static async Task<bool> IsNewSchedulePosted()
    {
        return _lastSchedule is null || !SiteParser.ParseScheduleURLs(await SiteParser.OpenSiteAsync()).Contains(_lastSchedule.URL);
    } 

    public static async Task<GroupSchedule> GetSchedule()
    {
        _lastSchedule ??= await LoadSchedule();
        return _lastSchedule;
    }

}

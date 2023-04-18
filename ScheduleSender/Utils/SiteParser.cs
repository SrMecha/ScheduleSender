using AngleSharp;
using AngleSharp.Dom;

namespace ScheduleSender.Utils;

public static class SiteParser
{
    public const string SiteURL = "https://vgke.by/raspisanie-zanjatij";

    public static async Task<IDocument> OpenSiteAsync(string url = SiteURL)
    {
        return await BrowsingContext.New(Configuration.Default.WithDefaultLoader()).OpenAsync(url);
    }

    public static List<string> ParseScheduleURLs(IDocument document)
    {
        var result = new List<string>();
        var ElementsWithSchedule = document
            .GetElementsByClassName("entry-content")[0]
            .GetElementsByTagName("p");
        foreach (var element in ElementsWithSchedule)
        {
            result.Add(element.FirstElementChild!.GetAttribute("src")!.Split("?")[0]);
        }
        return result;
    }
}

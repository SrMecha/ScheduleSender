using SkiaSharp;
using System.Text.RegularExpressions;

namespace ScheduleSender.Extensions;

public static class SKCanvasExtension
{
    public static void DrawTextInBox(this SKCanvas canvas, string text, SKRect rect, SKPaint paint)
    {
        var wordLines = new List<List<string>>() { new() };
        var spaceWidth = paint.MeasureText(" ");
        var lineWidth = 0f;
        var fixedText = Regex.Replace(text.Trim(), @"\s+", "\n");
        foreach (var line in fixedText.Split("\n"))
        {
            foreach (var word in line.Split(" "))
            {
                var wordWidth = paint.MeasureText(word);
                if (wordWidth > rect.Right - rect.Left - lineWidth)
                {
                    wordLines.Add(new());
                    lineWidth = 0;
                }
                lineWidth += wordLines[^1].Count < 1 ? wordWidth : wordWidth + spaceWidth;
                wordLines[^1].Add(word);
            }
            wordLines.Add(new());
        }
        var lineY = (rect.Bottom - rect.Top - paint.TextSize * wordLines.Count) / 2 + paint.FontSpacing / 2 + rect.Top;
        foreach (var lineWords in wordLines)
        {
            var line = string.Join(" ", lineWords);
            lineWidth = paint.MeasureText(line);
            canvas.DrawText(line, rect.Left + (rect.Right - rect.Left - lineWidth) / 2, lineY, paint);
            lineY += paint.FontSpacing;
        }
    }
}

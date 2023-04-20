using SkiaSharp;

namespace ScheduleSender.Extensions;

public static class BitmapExtension
{
    public static Stream ToStream(this SKBitmap bitmap)
    {
        return SKImage.FromPixels(bitmap.PeekPixels()).Encode().AsStream();
    }
}

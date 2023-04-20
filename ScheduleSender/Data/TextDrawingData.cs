using SkiaSharp;

namespace ScheduleSender.Data;

public class TextDrawingData
{
    public int X { get; init; }
    public int Y { get; init; }
    public int Width { get; init; }
    public int Height { get; init; }

    public SKRect ToRectangle()
    {
        return new SKRect(X, Y, X + Width, Y + Height);
    }
}

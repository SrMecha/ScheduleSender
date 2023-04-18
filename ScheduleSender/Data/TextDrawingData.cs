using System.Drawing;

namespace ScheduleSender.Data;

public class TextDrawingData
{
    public int X { get; init; }
    public int Y { get; init; }
    public int Width { get; init; }
    public int Height { get; init; }

    public RectangleF ToRectangle()
    {
        return new RectangleF(X, Y, Width, Height);
    }
}

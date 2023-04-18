using System.Drawing;
using System.Drawing.Imaging;

namespace ScheduleSender.Extensions;

public static class ImageExtension
{
    public static Stream ToStream(this Image image)
    {
        var stream = new MemoryStream();
        image.Save(stream, ImageFormat.Jpeg);
        stream.Position = 0;
        return stream;
    }
}

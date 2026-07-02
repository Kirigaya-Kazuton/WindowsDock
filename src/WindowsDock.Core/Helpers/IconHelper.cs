using System.Drawing;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace WindowsDock.Core;

public static class IconHelper
{
    public static ImageSource? GetIcon(string filename)
    {
        try
        {
            if (!File.Exists(filename)) return null;

            using var extractedIcon = System.Drawing.Icon.ExtractAssociatedIcon(filename);
            if (extractedIcon == null) return null;

            using var bmp = extractedIcon.ToBitmap();
            var hIcon = bmp.GetHicon();
            var img = Imaging.CreateBitmapSourceFromHIcon(
                hIcon,
                new System.Windows.Int32Rect(0, 0, 32, 32),
                BitmapSizeOptions.FromEmptyOptions());
            return img;
        }
        catch
        {
            return null;
        }
    }
}

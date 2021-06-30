using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Windows.Media.Imaging;

namespace ApoUI
{
    /// <summary>
    /// Converts Bitmap to BitmapImage so it can be shown in WPF UI
    /// </summary>
    public class BitmapToBitmapImage : BaseConverter<BitmapToBitmapImage>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return null;
            var bitmap = (System.Drawing.Bitmap)value;
            //var bitmapClone = bitmap.Clone(new Rectangle(0, 0, bitmap.Width, bitmap.Height), bitmap.PixelFormat);
            MemoryStream ms = new MemoryStream();
            //var bitmap = (System.Drawing.Bitmap)value;
            bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            ms.Seek(0, SeekOrigin.Begin);
            image.StreamSource = ms;            
            image.EndInit();
            return image;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

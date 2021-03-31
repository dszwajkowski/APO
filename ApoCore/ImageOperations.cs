using System.Drawing;

namespace ApoCore
{
    /// <summary>
    /// Extension methods and helpers
    /// </summary>
    public static class ImageOperations
    {
        /// <summary>
        /// Converts colorfull image to grayscale
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public static Bitmap ToGrayScale(this Bitmap image)
        {
            for (int x = 0; x < image.Width; x++)
            {
                for (int y = 0; y < image.Height; y++)
                {
                    // TODO - make it based on LUT table to avoid using GetPixel every time
                    Color color = image.GetPixel(x, y);
                    int avg = (color.R + color.G + color.B) / 3;
                    Color newcolor = Color.FromArgb(avg, avg, avg);
                    image.SetPixel(x, y, newcolor);
                }
            }
            return image;
        }

        /// <summary>
        /// Create image negation
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public static Bitmap Negation(this Bitmap image)
        {
            var max = image.GetMaxIntensity();
            for (int x = 0; x < image.Width; x++)
            {
                for (int y = 0; y < image.Height; y++)
                {
                    // TODO - make it based on LUT table to avoid using GetPixel every time
                    Color color = image.GetPixel(x, y);
                    Color newcolor = Color.FromArgb(max.maxR - color.R, max.maxG - color.G, max.maxB - color.B);
                    image.SetPixel(x, y, newcolor);
                }
            }
            return image;
        }

        public static Bitmap Thresholding(this Bitmap image, int threshold)
        {
            image = image.ToGrayScale();
            for (int x = 0; x < image.Width; x++)
            {
                for (int y = 0; y < image.Height; y++)
                {
                    // TODO - make it based on LUT table to avoid using GetPixel every time
                    Color color = image.GetPixel(x, y);
                    int r = 0;
                    if (color.R > threshold)
                        r = 255;
                    Color newcolor = Color.FromArgb(r, r, r);
                    image.SetPixel(x, y, newcolor);
                }
            }
            return image;
        }

        /// <summary>
        /// Get max intensities for each channel (R, G, B)
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public static (int maxR, int maxG, int maxB) GetMaxIntensity(this Bitmap image)
        {
            int maxR = 0;
            int maxG = 0;
            int maxB = 0;
            for (int x = 0; x < image.Width; x++)
            {
                for (int y = 0; y < image.Height; y++)
                {
                    // TODO - make it based on LUT table to avoid using GetPixel
                    Color color = image.GetPixel(x, y);
                    if (color.R > maxR)
                        maxR = color.R;
                    if (color.G > maxG)
                        maxG = color.G;
                    if (color.B > maxB)
                        maxB = color.B;
                }
            }
            return (maxR, maxG, maxB);
        }


        //public static int MaxIntensityR(this Bitmap image)
        //{
        //    int max = 0;
        //    for (int x = 0; x < image.Width; x++)
        //    {
        //        for (int y = 0; y < image.Height; y++)
        //        {
        //            if (image.GetPixel(x,y).R > max)
        //            {
        //                max = image.GetPixel(x, y).R;
        //            }
        //        }
        //    }
        //    return max;
        //}

        //public static int MaxIntensityG(this Bitmap image)
        //{
        //    int max = 0;
        //    for (int x = 0; x < image.Width; x++)
        //    {
        //        for (int y = 0; y < image.Height; y++)
        //        {
        //            if (image.GetPixel(x, y).G > max)
        //            {
        //                max = image.GetPixel(x, y).G;
        //            }
        //        }
        //    }
        //    return max;
        //}

        //public static int MaxIntensityB(this Bitmap image)
        //{
        //    int max = 0;
        //    for (int x = 0; x < image.Width; x++)
        //    {
        //        for (int y = 0; y < image.Height; y++)
        //        {
        //            if (image.GetPixel(x, y).B > max)
        //            {
        //                max = image.GetPixel(x, y).B;
        //            }
        //        }
        //    }
        //    return max;
        //}

        //public static BitmapSource BitmapToImageSource(Bitmap image)
        //{
        //    using (MemoryStream memory = new MemoryStream())
        //    {
        //        bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
        //        memory.Position = 0;
        //        BitmapSource bitmapimage = new BitmapSource();
        //        bitmapimage.BeginInit();
        //        bitmapimage.StreamSource = memory;
        //        bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
        //        bitmapimage.EndInit();

        //        return bitmapimage;
        //    }
        //}
    }
}

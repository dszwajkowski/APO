using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;

namespace ApoCore
{
    /// <summary>
    /// Extension methods for ImageModel
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
                    Color color = image.GetPixel(x, y);
                    int avg = (color.R + color.G + color.B) / 3;
                    Color newcolor = Color.FromArgb(avg, avg, avg);
                    image.SetPixel(x, y, newcolor);
                }
            }
            return image;
        }

        /// <summary>
        /// Converts colorfull image to grayscale using Lookup Table
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public static Bitmap ToGrayScaleLUT(this ImageModel image)
        {
            for (int x = 0; x < image.Image.Width; x++)
            {
                for (int y = 0; y < image.Image.Height; y++)
                {
                    int avg = (image.LutR[x,y] + image.LutG[x, y] + image.LutB[x, y]) / 3;
                    image.LutR[x, y] = avg;
                    image.LutG[x, y] = avg;
                    image.LutB[x, y] = avg;
                    Color newcolor = Color.FromArgb(avg, avg, avg);
                    image.Image.SetPixel(x, y, newcolor);
                }
            }
            return image.Image;
        }

        /// <summary>
        /// Creates negation of a image
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public static Bitmap Negation(this ImageModel image)
        {
            int maxR = 0;
            int maxG = 0;
            int maxB = 0;
            foreach (var item in image.HistogramData)
            {
                switch (item.Channel)
                {
                    case ChannelModel.Red:
                        maxR = item.Max;
                        break;
                    case ChannelModel.Green:
                        maxG = item.Max;
                        break;
                    case ChannelModel.Blue:
                        maxB = item.Max;
                        break;
                    default:
                        break;
                }
            }
            for (int x = 0; x < image.Image.Width; x++)
            {
                for (int y = 0; y < image.Image.Height; y++)
                {
                    image.LutR[x, y] = maxR - image.LutR[x, y];
                    image.LutG[x, y] = maxG - image.LutG[x, y];
                    image.LutB[x, y] = maxB - image.LutB[x, y];
                    Color newcolor = Color.FromArgb(image.LutR[x, y], image.LutG[x, y], image.LutB[x, y]);
                    image.Image.SetPixel(x, y, newcolor);
                }
            }
            return image.Image;
        }

        public static Bitmap Thresholding(this ImageModel image, int threshold)
        {
            //image.Image = image.Image.ToGrayScale();
            for (int x = 0; x < image.Image.Width; x++)
            {
                for (int y = 0; y < image.Image.Height; y++)
                {
                    int r = 0;
                    if (image.LutR[x, y] > threshold)
                        r = 255;
                    image.LutR[x, y] = r;
                    image.LutG[x, y] = r;
                    image.LutB[x, y] = r;
                    Color newcolor = Color.FromArgb(r, r, r);
                    image.Image.SetPixel(x, y, newcolor);
                }
            }
            return image.Image;
        }

        public static Bitmap ThresholdingKeepGrayLevels(this ImageModel image, int threshold, int thresholdmax)
        {
            //image.Image = image.Image.ToGrayScale();
            for (int x = 0; x < image.Image.Width; x++)
            {
                for (int y = 0; y < image.Image.Height; y++)
                {
                    if (image.LutR[x, y] <= threshold || image.LutR[x, y] >= thresholdmax)
                    {
                        Color newcolor = Color.FromArgb(0, 0, 0);
                        image.Image.SetPixel(x, y, newcolor); 
                    }
                    
                }
            }
            return image.Image;
        }

        public static Bitmap Posterize(this ImageModel image, int numberofbins = 8)
        {
            List<int> bins = new List<int>();
            for (int i = 0; i < numberofbins; i++)
            {
                bins.Add((int)Math.Ceiling((255.0 / numberofbins) * i));
            }

            for (int x = 0; x < image.Image.Width; x++)
            {
                for (int y = 0; y < image.Image.Height; y++)
                {
                    int currentpixel = image.LutR[x, y];
                    for (int i = 0; i < bins.Count; i++)
                    {
                        if (currentpixel > bins[i])
                        {
                            image.LutR[x, y] = bins[i];
                            image.Image.SetPixel(x, y, Color.FromArgb(bins[i], bins[i], bins[i]));
                        }
                        if (currentpixel > bins.Last())
                            image.Image.SetPixel(x, y, Color.FromArgb(255, 255, 255));
                    }                  
                }
            }
            return image.Image;
        }

        public static Bitmap HistogramStretch(this ImageModel image, int min = 0, int max = 255)
        {
            int maxR = 0;
            int minR = 0;
            //int maxG = 0;
            //int maxB = 0;
            foreach (var item in image.HistogramData)
            {
                switch (item.Channel)
                {
                    case ChannelModel.Red:
                        maxR = item.Max;
                        minR = item.Min;
                        break;
                    //case ChannelModel.Green:
                    //    maxG = item.Max;
                    //    break;
                    //case ChannelModel.Blue:
                    //    maxB = item.Max;
                    //    break;
                    default:
                        break;
                }
            }
            for (int x = 0; x < image.Image.Width; x++)
            {
                for (int y = 0; y < image.Image.Height; y++)
                {
                    image.LutR[x, y] = ((image.LutR[x,y] - minR)*max)/(maxR-minR);
                    Color newcolor = Color.FromArgb(image.LutR[x, y], image.LutR[x, y], image.LutR[x, y]);
                    image.Image.SetPixel(x, y, newcolor);
                }
            }
            return image.Image;
        }

        public static Bitmap HistogramStretchLinear(this ImageModel image, int min = 0, int max = 255)
        {
            int maxR = 0;
            int minR = 0;
            //int maxG = 0;
            //int maxB = 0;
            foreach (var item in image.HistogramData)
            {
                switch (item.Channel)
                {
                    case ChannelModel.Red:
                        maxR = item.Max;
                        minR = item.Min;
                        break;
                    //case ChannelModel.Green:
                    //    maxG = item.Max;
                    //    break;
                    //case ChannelModel.Blue:
                    //    maxB = item.Max;
                    //    break;
                    default:
                        break;
                }
            }
            for (int x = 0; x < image.Image.Width; x++)
            {
                for (int y = 0; y < image.Image.Height; y++)
                {
                    if (image.LutR[x, y] < minR)
                        image.LutR[x, y] = min;
                    if (image.LutR[x, y] >= minR && image.LutR[x, y] <= maxR)
                        image.LutR[x, y] = ((image.LutR[x, y] - minR) * max) / (maxR - minR);
                    else
                        image.LutR[x, y] = max;

                    Color newcolor = Color.FromArgb(image.LutR[x, y], image.LutR[x, y], image.LutR[x, y]);
                    image.Image.SetPixel(x, y, newcolor);
                }
            }
            return image.Image;
        }

        public static Bitmap Blur(this ImageModel image, BorderType border = BorderType.Default)
        {
            Image<Bgr, Byte> myImage = image.Image.ToImage<Bgr, Byte>();
            CvInvoke.Blur(myImage, myImage, new Size(5,5), new Point(-1,-1), border);
            image.Image = myImage.ToBitmap<Bgr, byte>();
            return image.Image;
        }

        public static Bitmap GaussianBlur(this ImageModel image, BorderType border = BorderType.Default)
        {
            Image<Bgr, Byte> myImage = image.Image.ToImage<Bgr, Byte>();
            CvInvoke.GaussianBlur(myImage, myImage, new Size(5,5), 0, 0, border);
            image.Image = myImage.ToBitmap<Bgr, byte>();
            return image.Image;
        }


        /// <summary>
        /// Get max intensities for each channel (R, G, B)
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        //public static (int maxR, int maxG, int maxB) GetMaxIntensity(this Bitmap image)
        //{
        //    int maxR = 0;
        //    int maxG = 0;
        //    int maxB = 0;
        //    for (int x = 0; x < image.Width; x++)
        //    {
        //        for (int y = 0; y < image.Height; y++)
        //        {
        //            Color color = image.GetPixel(x, y);
        //            if (color.R > maxR)
        //                maxR = color.R;
        //            if (color.G > maxG)
        //                maxG = color.G;
        //            if (color.B > maxB)
        //                maxB = color.B;
        //        }
        //    }
        //    return (maxR, maxG, maxB);
        //}

        //public static Bitmap Negation(this Bitmap image)
        //{
        //    var max = image.GetMaxIntensity();
        //    for (int x = 0; x < image.Width; x++)
        //    {
        //        for (int y = 0; y < image.Height; y++)
        //        {                
        //            Color color = image.GetPixel(x, y);
        //            Color newcolor = Color.FromArgb(max.maxR - color.R, max.maxG - color.G, max.maxB - color.B);
        //            image.SetPixel(x, y, newcolor);
        //        }
        //    }
        //    return image;
        //}

        //public static Bitmap Thresholding2(this Bitmap image, int threshold)
        //{
        //    image = image.ToGrayScale();
        //    for (int x = 0; x < image.Width; x++)
        //    {
        //        for (int y = 0; y < image.Height; y++)
        //        {        
        //            Color color = image.GetPixel(x, y);
        //            int r = 0;
        //            if (color.R > threshold)
        //                r = 255;
        //            Color newcolor = Color.FromArgb(r, r, r);
        //            image.SetPixel(x, y, newcolor);
        //        }
        //    }
        //    return image;
        //}
    }
}

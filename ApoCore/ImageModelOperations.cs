using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace ApoCore
{
    /// <summary>
    /// Operations for ImageModel
    /// </summary>
    public static class ImageModelOperations
    {
        /// <summary>
        /// Converts colorfull image to grayscale
        /// </summary>
        /// <param name="imageModel"></param>
        /// <returns></returns>
        public static Bitmap ToGrayScale(this ImageModel imageModel)
        {
            for (int x = 0; x < imageModel.Image.Width; x++)
            {
                for (int y = 0; y < imageModel.Image.Height; y++)
                {
                    int avg = (imageModel.LutR[x,y] + imageModel.LutG[x, y] + imageModel.LutB[x, y]) / 3;
                    imageModel.LutR[x, y] = avg;
                    imageModel.LutG[x, y] = avg;
                    imageModel.LutB[x, y] = avg;
                    Color newcolor = Color.FromArgb(avg, avg, avg);
                    imageModel.Image.SetPixel(x, y, newcolor);
                }
            }
            return imageModel.Image;
        }

        /// <summary>
        /// Creates negation of a image
        /// </summary>
        /// <param name="imageModel"></param>
        /// <returns></returns>
        public static ImageModel Negation(ImageModel imageModel)
        {
            int maxR = 0;
            int maxG = 0;
            int maxB = 0;
            foreach (var item in imageModel.HistogramData)
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
                    case ChannelModel.RGB:
                        maxR = maxG = maxB = item.Max;
                        break;
                    default:
                        break;
                }
            }
            for (int x = 0; x < imageModel.Image.Width; x++)
            {
                for (int y = 0; y < imageModel.Image.Height; y++)
                {
                    imageModel.LutR[x, y] = maxR - imageModel.LutR[x, y];
                    imageModel.LutG[x, y] = maxG - imageModel.LutG[x, y];
                    imageModel.LutB[x, y] = maxB - imageModel.LutB[x, y];
                    Color newcolor = Color.FromArgb(imageModel.LutR[x, y], 
                        imageModel.LutG[x, y], imageModel.LutB[x, y]);
                    imageModel.Image.SetPixel(x, y, newcolor);
                }
            }
            return imageModel;
        }

        public static Bitmap Thresholding(ImageModel image, int threshold, bool keepGrayLevels = false, int thresholdMax = 0)
        {
            if (keepGrayLevels == true && threshold >= thresholdMax) throw new ArgumentException("threshold must be smaller than threshlodMax");
            if (!keepGrayLevels)
            {
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
            }
            else
            {
                for (int x = 0; x < image.Image.Width; x++)
                {
                    for (int y = 0; y < image.Image.Height; y++)
                    {
                        if (image.LutR[x, y] <= threshold || image.LutR[x, y] >= thresholdMax)
                        {
                            Color newcolor = Color.FromArgb(0, 0, 0);
                            image.Image.SetPixel(x, y, newcolor);
                        }

                    }
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

        public static ImageModel Posterize(ImageModel image, int numberofbins = 8)
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
            return image;
        }

        public static ImageModel HistogramStretch(ImageModel imageModel, int min = 0, int max = 255)
        {
            int maxR = 0;
            int minR = 0;
            foreach (var item in imageModel.HistogramData)
            {
                switch (item.Channel)
                {
                    case ChannelModel.RGB:
                        maxR = item.Max;
                        minR = item.Min;
                        break;
                    default:
                        break;
                }
            }
            for (int x = 0; x < imageModel.Image.Width; x++)
            {
                for (int y = 0; y < imageModel.Image.Height; y++)
                {
                    imageModel.LutR[x, y] = ((imageModel.LutR[x,y] - minR)*max)/(maxR-minR);
                    Color newcolor = Color.FromArgb(imageModel.LutR[x, y], imageModel.LutR[x, y], imageModel.LutR[x, y]);
                    imageModel.Image.SetPixel(x, y, newcolor);
                }
            }
            return imageModel;
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
    }
}

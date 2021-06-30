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

        /// <summary>
        /// Performs thresholding operation on image
        /// </summary>
        /// <param name="imageModel"></param>
        /// <param name="threshold"></param>
        /// <param name="keepGrayLevels"></param>
        /// <param name="thresholdMax"></param>
        /// <returns></returns>
        public static ImageModel Thresholding(ImageModel imageModel, int threshold, bool keepGrayLevels = false, int thresholdMax = 0)
        {
            ImageModel imageModelCopy = imageModel.DeepClone();
            //if (keepGrayLevels == true && threshold >= thresholdMax) throw new ArgumentException("threshold must be smaller than threshlodMax");
            if (!keepGrayLevels)
            {
                for (int x = 0; x < imageModelCopy.Image.Width; x++)
                {
                    for (int y = 0; y < imageModelCopy.Image.Height; y++)
                    {
                        int r = 0;
                        if (imageModelCopy.LutR[x, y] > threshold)
                            r = 255;
                        imageModelCopy.LutR[x, y] = r;
                        imageModelCopy.LutG[x, y] = r;
                        imageModelCopy.LutB[x, y] = r;
                        Color newcolor = Color.FromArgb(r, r, r);
                        imageModelCopy.Image.SetPixel(x, y, newcolor);
                    }
                }
            }
            else
            {
                for (int x = 0; x < imageModelCopy.Image.Width; x++)
                {
                    for (int y = 0; y < imageModelCopy.Image.Height; y++)
                    {
                        if (imageModelCopy.LutR[x, y] <= threshold || imageModelCopy.LutR[x, y] >= thresholdMax)
                        {
                            Color newcolor = Color.FromArgb(0, 0, 0);
                            imageModelCopy.Image.SetPixel(x, y, newcolor);
                        }

                    }
                }
            }
            return imageModelCopy;
        }

        /// <summary>
        /// Performs posterize operation on image
        /// </summary>
        /// <param name="imageModel"></param>
        /// <param name="numberofbins"></param>
        /// <returns></returns>
        public static ImageModel Posterize(ImageModel imageModel, int numberofbins = 8)
        {
            ImageModel imageModelCopy = imageModel.DeepClone();
            List<int> bins = new List<int>();
            for (int i = 0; i < numberofbins; i++)
            {
                bins.Add((int)Math.Ceiling((255.0 / numberofbins) * i));
            }

            for (int x = 0; x < imageModelCopy.Image.Width; x++)
            {
                for (int y = 0; y < imageModelCopy.Image.Height; y++)
                {
                    int currentpixel = imageModelCopy.LutR[x, y];
                    for (int i = 0; i < bins.Count; i++)
                    {
                        if (currentpixel > bins[i])
                        {
                            imageModelCopy.LutR[x, y] = bins[i];
                            imageModelCopy.Image.SetPixel(x, y, Color.FromArgb(bins[i], bins[i], bins[i]));
                        }
                        if (currentpixel > bins.Last())
                            imageModelCopy.Image.SetPixel(x, y, Color.FromArgb(255, 255, 255));
                    }                  
                }
            }
            return imageModelCopy;
        }

        /// <summary>
        /// Stretches histogram of an image
        /// </summary>
        /// <param name="imageModel"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static ImageModel HistogramStretch(ImageModel imageModel, int min = 0, int max = 255)
        {
            ImageModel imageModelCopy = imageModel.DeepClone();
            int maxR = 0;
            int minR = 0;
            foreach (var item in imageModelCopy.HistogramData)
            {
                switch (item.Channel)
                {
                    case ChannelModel.RGB:
                        maxR = item.Max;
                        minR = item.Min;
                        break;
                    case ChannelModel.Red:
                        maxR = item.Max;
                        minR = item.Min;
                        break;
                    default:
                        break;
                }
            }
            for (int x = 0; x < imageModelCopy.Image.Width; x++)
            {
                for (int y = 0; y < imageModelCopy.Image.Height; y++)
                {
                    imageModelCopy.LutR[x, y] = ((imageModelCopy.LutR[x,y] - minR)*max)/(maxR-minR);
                    Color newcolor = Color.FromArgb(imageModelCopy.LutR[x, y], imageModelCopy.LutR[x, y], imageModelCopy.LutR[x, y]);
                    imageModelCopy.Image.SetPixel(x, y, newcolor);
                }
            }
            return imageModelCopy;
        }

        /// <summary>
        /// Stretches histogram of an image (linear)
        /// </summary>
        /// <param name="imageModel"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static ImageModel HistogramStretchLinear(ImageModel imageModel, int min = 0, int max = 255)
        {
            ImageModel imageModelCopy = imageModel.DeepClone();
            int maxR = 0;
            int minR = 0;
            //int maxG = 0;
            //int maxB = 0;
            foreach (var item in imageModelCopy.HistogramData)
            {
                switch (item.Channel)
                {
                    case ChannelModel.RGB:
                        maxR = item.Max;
                        minR = item.Min;
                        break;
                    case ChannelModel.Red:
                        maxR = item.Max;
                        minR = item.Min;
                        break;
                    default:
                        break;
                }
            }
            for (int x = 0; x < imageModelCopy.Image.Width; x++)
            {
                for (int y = 0; y < imageModelCopy.Image.Height; y++)
                {
                    if (imageModelCopy.LutR[x, y] >= minR && imageModelCopy.LutR[x, y] <= maxR)
                        imageModelCopy.LutR[x, y] = ((imageModelCopy.LutR[x, y] - minR) * max) / (maxR - minR);
                    if (imageModelCopy.LutR[x, y] < min)
                        imageModelCopy.LutR[x, y] = min;                
                    if (imageModelCopy.LutR[x, y] > max)
                        imageModelCopy.LutR[x, y] = max;

                    Color newcolor = Color.FromArgb(imageModelCopy.LutR[x, y], imageModelCopy.LutR[x, y], imageModelCopy.LutR[x, y]);
                    imageModelCopy.Image.SetPixel(x, y, newcolor);
                }
            }
            return imageModelCopy;
        }

        /// <summary>
        /// Performs equalization operation on image
        /// </summary>
        /// <param name="imageModel"></param>
        /// <returns></returns>
        public static ImageModel Equalization(ImageModel imageModel)
        {
            ImageModel imageModelCopy = imageModel.DeepClone();
            int r = 0;
            double h = 0.0;
            double[] left = new double[256];
            double[] right = new double[256];
            int[] newValue = new int[256];

            var histogramData = imageModelCopy.HistogramData[0].PlotData;
            double avg = histogramData.Average();
            for (int i = 0; i < 256; ++i)
            {
                left[i] = r;
                h += histogramData[i];
                while (h > avg)
                {
                    h -= avg;
                    if (r < 255) r++;
                }
                right[i] = r;
                newValue[i] = (int)((left[i] + right[i]) / 2.0);
            }

            for (int x = 0; x < imageModelCopy.Image.Width; x++)
            {
                for (int y = 0; y < imageModelCopy.Image.Height; y++)
                {
                    int color = imageModel.LutR[x, y];
                    if (left[color] == right[color])
                    {
                        Color newcolor = Color.FromArgb(255, (int)left[color], (int)left[color], (int)left[color]);
                        imageModelCopy.Image.SetPixel(x, y, newcolor);
                    }
                    else
                    {
                        Color newcolor = Color.FromArgb(255, (int)newValue[color], (int)newValue[color], (int)newValue[color]);
                        imageModelCopy.Image.SetPixel(x, y, newcolor);
                    }
                }
            }
            return imageModelCopy;
        }

        /// <summary>
        /// Allow histogram stretching/narrowing and performing gamma operation
        /// </summary>
        /// <param name="imageModel"></param>
        /// <param name="gamma"></param>
        /// <param name="inputMin"></param>
        /// <param name="inputMax"></param>
        /// <param name="outputMin"></param>
        /// <param name="outputMax"></param>
        /// <param name="channel"></param>
        /// <returns></returns>
        public static ImageModel ContrastEnhancement(ImageModel imageModel, double gamma, int inputMin = 0, int inputMax = 255, 
            int outputMin = 0, int outputMax = 255, ChannelModel channel = ChannelModel.RGB)
        {
            if (gamma > 10 || gamma < 0.10) throw new ArgumentOutOfRangeException("gamma should be greater than or equal to 0.10 and less than or equal to 10.");
            if (inputMin > 254 || inputMin < 0) throw new ArgumentOutOfRangeException("inputMin should be greater than or equal to 0 and less than or equal to 254");
            if (inputMax > 255 || inputMax < 1) throw new ArgumentOutOfRangeException("inputMax should be greater than or equal to 0 and less than or equal to 254");
            if (outputMin > 254 || outputMin < 0) throw new ArgumentOutOfRangeException("outputMin should be greater than or equal to 1 and less than or equal to 255");
            if (outputMax > 255 || outputMax < 1) throw new ArgumentOutOfRangeException("outputMax should be greater than or equal to 1 and less than or equal to 255");

            if (gamma != 1 || inputMin != 0 || inputMax != 255 || outputMin != 0 || outputMax != 255)
            {
                ImageModel imageModelCopy = imageModel.DeepClone();
                for (int x = 0; x < imageModelCopy.Image.Width; x++)
                {
                    for (int y = 0; y < imageModelCopy.Image.Height; y++)
                    {
                        int oldMin = inputMin + 1;
                        int oldMax = inputMax - 1;
                        if (channel == ChannelModel.Red || channel == ChannelModel.RGB)
                        {
                            var currentPixel = imageModelCopy.LutR[x, y];
                            if (currentPixel <= inputMin)
                                imageModelCopy.LutR[x, y] = outputMin;
                            else if (currentPixel >= inputMax)
                                imageModelCopy.LutR[x, y] = outputMax;
                            else
                            {
                                // linear stretch other values
                                imageModelCopy.LutR[x, y] = (((currentPixel - oldMin) * (outputMax - outputMin)) / (oldMax - oldMin)) + outputMin;
                            }
                            // gamma
                            imageModelCopy.LutR[x, y] = (int)Math.Round(255.0 * Math.Exp(Math.Log(imageModelCopy.LutR[x, y] / 255.0) / gamma));
                        }
                        if (channel == ChannelModel.Green || channel == ChannelModel.RGB)
                        {
                            var currentPixel = imageModelCopy.LutG[x, y];
                            if (currentPixel <= inputMin)
                                imageModelCopy.LutG[x, y] = outputMin;
                            else if (currentPixel >= inputMax)
                                imageModelCopy.LutG[x, y] = outputMax;
                            else
                            {
                                // linear stretch other values
                                imageModelCopy.LutG[x, y] = (((currentPixel - oldMin) * (outputMax - outputMin)) / (oldMax - oldMin)) + outputMin;
                            }
                            // gamma
                            imageModelCopy.LutG[x, y] = (int)Math.Round(255.0 * Math.Exp(Math.Log(imageModelCopy.LutG[x, y] / 255.0) / gamma));
                        }
                        if (channel == ChannelModel.Blue || channel == ChannelModel.RGB)
                        {
                            var currentPixel = imageModelCopy.LutB[x, y];
                            if (currentPixel <= inputMin)
                                imageModelCopy.LutB[x, y] = outputMin;
                            else if (currentPixel >= inputMax)
                                imageModelCopy.LutB[x, y] = outputMax;
                            else
                            {
                                // linear stretch other values
                                imageModelCopy.LutB[x, y] = (((currentPixel - oldMin) * (outputMax - outputMin)) / (oldMax - oldMin)) + outputMin;
                            }
                            // gama
                            imageModelCopy.LutB[x, y] = (int)Math.Round(255.0 * Math.Exp(Math.Log(imageModelCopy.LutB[x, y] / 255.0) / gamma));
                        }
                        imageModelCopy.Image.SetPixel(x, y, Color.FromArgb(imageModelCopy.LutR[x, y], imageModelCopy.LutG[x, y], imageModelCopy.LutB[x, y]));
                    }
                }
                return imageModelCopy;
            }
            return imageModel;
        }
    }
}

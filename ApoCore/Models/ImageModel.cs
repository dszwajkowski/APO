using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace ApoCore
{
    /// <summary>
    /// Data model for image
    /// </summary>
    [Serializable]
    public class ImageModel
    {
        #region Public properties 

        // bitmap
        public Bitmap Image { get; set; } 
        // path of the image
        public string ImagePath { get; private set; }
        // list containg histogram data for each channel
        public List<HistogramModel> HistogramData { get; private set; } = new List<HistogramModel>();
        // array of each pixel on red channel. If image is grayscale servers as LUT for each channel
        public int[,] LutR { get; private set; }
        // array of each pixel on green channel
        public int[,] LutG { get; private set; }
        // array of each pixel on blue channel
        public int[,] LutB { get; private set; }
        public bool IsColorful { get; set; } = false;

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="imagePath"></param>
        public ImageModel(string imagePath)
        {
            Bitmap image = new(imagePath);
            // this creates Bitmap with PixelFormat.Format32bppArgb, regardless of original PixelFormat
            this.Image = new Bitmap(image);
            image.Dispose();
            this.ImagePath = imagePath;
            GenerateLUT();
            if (IsColorful)
            {
                HistogramData.Add(new HistogramModel(LutR, Image.Width, Image.Height, ChannelModel.Red));
                HistogramData.Add(new HistogramModel(LutG, Image.Width, Image.Height, ChannelModel.Green));
                HistogramData.Add(new HistogramModel(LutB, Image.Width, Image.Height, ChannelModel.Blue));
            }
            else
            {
                HistogramData.Add(new HistogramModel(LutR, Image.Width, Image.Height, ChannelModel.RGB));
            }           
        }

        #endregion

        #region Helpers

        /// <summary>
        /// Generates Lookup tables and checks if image is colorful or grayscale
        /// </summary>
        public void GenerateLUT()
        {
            LutR = new int[(Image.Width), (Image.Height)];
            LutG = new int[(Image.Width), (Image.Height)];
            LutB = new int[(Image.Width), (Image.Height)];
            for (int x = 0; x < Image.Width; x++)
            {
                for (int y = 0; y < Image.Height; y++)
                {
                    Color C = Image.GetPixel(x, y);
                    LutR[x, y] = C.R;
                    LutG[x, y] = C.G;
                    LutB[x, y] = C.B;
                    if (LutR[x, y] != LutG[x, y] || LutR[x, y] != LutB[x, y] || LutG[x, y] != LutB[x, y]) IsColorful = true;
                }
            }
        }

        /// <summary>
        /// Updates histogram data
        /// </summary>
        public void UpdateHistogramData()
        {
            GenerateLUT();
            HistogramData.Clear();
            if (IsColorful)
            {
                HistogramData.Add(new HistogramModel(LutR, Image.Width, Image.Height, ChannelModel.Red));
                HistogramData.Add(new HistogramModel(LutG, Image.Width, Image.Height, ChannelModel.Green));
                HistogramData.Add(new HistogramModel(LutB, Image.Width, Image.Height, ChannelModel.Blue));
            }
            else
            {
                HistogramData.Add(new HistogramModel(LutR, Image.Width, Image.Height, ChannelModel.RGB));
            }
        }

        /// <summary>
        /// Returns lowest and highest intensity from <paramref name="channel"/> in <see cref="HistogramData"/>
        /// </summary>
        /// <param name="channel"></param>
        /// <returns></returns>
        public (int min, int max) MinMax(ChannelModel channel)
        {
            int min = -1;
            int max = -1;
            if (HistogramData.Count > 1)
            {
                foreach (var item in HistogramData)
                {
                    if (item.Channel == channel)
                    {
                        min = item.Min;
                        max = item.Max;
                    }
                }
            }
            else if (HistogramData.Count == 1)
            {
                min = HistogramData[0].Min;
                max = HistogramData[0].Max;
            }          
            return (min, max);
        }

        #endregion
    }
}

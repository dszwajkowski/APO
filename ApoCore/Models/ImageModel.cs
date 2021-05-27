using System.Collections.Generic;
using System.Drawing;

namespace ApoCore
{
    public class ImageModel
    {
        #region Public properties 

        public Bitmap Image { get; set; } 
        public string ImagePath { get; private set; }       
        public List<HistogramModel> HistogramData { get; private set; } = new List<HistogramModel>();
        public int[,] LutR { get; private set; }
        public int[,] LutG { get; private set; }
        public int[,] LutB { get; private set; }
        public bool IsColorful { get; set; } = false;

        #endregion

        #region Constructor

        public ImageModel(string imagePath)
        {
            //photo-1537704858610-057120889e0d
            this.Image = new(imagePath);
            //using (FileStream fs = new FileStream(imagepath, FileMode.Open))
            //{
            //    this.Image = (Bitmap)Bitmap.FromStream(fs);
            //    fs.Close();
            //}
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

        #endregion
    }
}

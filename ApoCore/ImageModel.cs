//using Emgu.CV;
//using Emgu.CV.Structure;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace ApoCore
{
    public class ImageModel
    {
        #region Public properties 

        public Bitmap Image { get; set; } 
        //public Image<Bgr,byte> EmguImage { get; private set; }
        //public DirectBitmap Image { get; set; }
        public string ImagePath { get; private set; }
        public List<HistogramModel> HistogramData { get; private set; } = new List<HistogramModel>();
        public int[,] LutR { get; private set; }
        public int[,] LutG { get; private set; }
        public int[,] LutB { get; private set; }

        #endregion

        #region Constructors

        public ImageModel(string imagepath)
        {
            //this.Image = new(imagepath);
            using (FileStream fs = new FileStream(imagepath, FileMode.Open))
            {
                this.Image = (Bitmap)Bitmap.FromStream(fs);
                fs.Close();
            }
            this.ImagePath = imagepath;
            GenerateLUT();
            HistogramData.Add(new HistogramModel(LutR, Image.Width, Image.Height, ChannelModel.Red));
            HistogramData.Add(new HistogramModel(LutG, Image.Width, Image.Height, ChannelModel.Green));
            HistogramData.Add(new HistogramModel(LutB, Image.Width, Image.Height, ChannelModel.Blue));
        }

        public ImageModel(Bitmap image)
        {
            this.Image = new(image);
            this.ImagePath = "none";
            GenerateLUT();
            HistogramData.Add(new HistogramModel(LutR, Image.Width, Image.Height, ChannelModel.Red));
            HistogramData.Add(new HistogramModel(LutG, Image.Width, Image.Height, ChannelModel.Green));
            HistogramData.Add(new HistogramModel(LutB, Image.Width, Image.Height, ChannelModel.Blue));
        }

        #endregion

        #region Helpers

        /// <summary>
        /// Generates Lookup tables
        /// </summary>
        public void GenerateLUT()
        {
            //int MaxR = 0;
            //int MaxG = 0;
            //int MaxB = 0;
            //Image = new DirectBitmap(Image.Width, Image.Height);
            //using (var g = Graphics.FromImage(Image.Bitmap))
            //{
            //    g.DrawImage(Image2, 0, 0);
            //}
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
                }
            }
        }

        /// <summary>
        /// Updates histogram data
        /// </summary>
        public void UpdateHistogramData()
        {
            HistogramData.Clear();
            HistogramData.Add(new HistogramModel(LutR, Image.Width, Image.Height, ChannelModel.Red));
            HistogramData.Add(new HistogramModel(LutG, Image.Width, Image.Height, ChannelModel.Green));
            HistogramData.Add(new HistogramModel(LutB, Image.Width, Image.Height, ChannelModel.Blue));
        }

        #endregion
    }
}

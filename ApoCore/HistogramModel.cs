using System.Collections.Generic;
using System.Drawing;

namespace ApoCore
{
    public class HistogramModel
    {
        #region Public properties
        public Bitmap Image { get; set; }
        public SortedDictionary<int, int> ChannelRGB = new SortedDictionary<int, int>();
        //public SortedDictionary<int, int> ChannelR { get; private set; } = new SortedDictionary<int, int>();
        //public SortedDictionary<int, int> ChannelG { get; private set; } = new SortedDictionary<int, int>();
        //public SortedDictionary<int, int> ChannelB { get; private set; } = new SortedDictionary<int, int>();
        #endregion

        #region Constructor
        public HistogramModel(Bitmap image)
        {
            this.Image = image;
            GetHistogramData();
        }
        #endregion

        #region Helpers
        public void GetHistogramData()
        {
            ChannelRGB.Clear();
            //Bitmap grayscaleimage = Image.ToGrayScale();
            Bitmap grayscaleimage = Image;
            for (int x = 0; x < Image.Width; x++)
            {
                for (int y = 0; y < Image.Height; y++)
                {
                    // TODO - make it faster
                    Color C = grayscaleimage.GetPixel(x, y);

                    if (ChannelRGB.ContainsKey(C.R))
                        ChannelRGB[C.R] = ChannelRGB[C.R] + 1;
                    else
                        ChannelRGB.Add(C.R, 1);

                    //if (ChannelR.ContainsKey(C.R))
                    //    ChannelR[C.R] = ChannelR[C.R] + 1;
                    //else
                    //    ChannelR.Add(C.R, 1);

                    //if (ChannelG.ContainsKey(C.G))
                    //    ChannelG[C.G] = ChannelG[C.G] + 1;
                    //else
                    //    ChannelG.Add(C.G, 1);

                    //if (ChannelB.ContainsKey(C.B))
                    //    ChannelB[C.B] = ChannelB[C.B] + 1;
                    //else
                    //    ChannelB.Add(C.B, 1);
                }
            }
        }
        #endregion
    }
}

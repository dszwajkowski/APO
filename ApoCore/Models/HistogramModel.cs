using System;

namespace ApoCore
{
    /// <summary>
    /// Data model for histogram
    /// </summary>
    [Serializable]
    public class HistogramModel
    {
        #region Public properties

        // image channel
        public ChannelModel Channel { get; private set; }
        // occurrences of each intensity/color
        public int[] PlotData { get; private set; } = new int[256];
        // highest intensity/color
        public int Max { get; private set; } = 0;
        // lowest intensity/color
        public int Min { get; private set; } = 255;

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public HistogramModel()
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="lut"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="channel"></param>
        public HistogramModel(int[,] lut, int width, int height, ChannelModel channel)
        {
            Channel = channel;
            GenerateHistogramData(width, height, lut);
        }

        #endregion

        #region Helpers

        /// <summary>
        /// Counts occurence of each color/intensity, maximum and minimum value
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="lut"></param>
        public void GenerateHistogramData(int width, int height, int[,] lut)
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (lut[x, y] > Max) Max = lut[x, y];
                    if (lut[x, y] < Min) Min = lut[x, y];
                    PlotData[lut[x,y]]++;
                }
            }
        }

        /// <summary>
        /// Returns channel as string
        /// </summary>
        public override string ToString()
        {
            return Channel.ToString();
        }

        #endregion
    }
}

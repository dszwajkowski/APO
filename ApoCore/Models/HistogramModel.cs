namespace ApoCore
{
    public class HistogramModel
    {
        #region Public properties

        public ChannelModel Channel { get; private set; }
        public int[] PlotData { get; private set; } = new int[256];
        public int Max { get; private set; } = 0;
        public int Min { get; private set; } = 255;

        #endregion

        #region Constructors

        /// <summary>
        /// Empty constructor
        /// </summary>
        public HistogramModel()
        {

        }

        public HistogramModel(int[,] lut, int width, int height, ChannelModel channel)
        {
            Channel = channel;
            GenerateHistogramData(width, height, lut);
        }

        #endregion

        #region Helpers

        /// <summary>
        /// Counts occurence of each color/intensity and maximum value
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

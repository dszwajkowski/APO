using ApoCore;
using System.Collections.ObjectModel;

namespace ApoUI
{
    
    /// <summary>
    /// View model for histogram table dialog
    /// </summary>
    public class HistogramTableViewModel
    {
        /// <summary>
        /// Source for DataGrid containing intensity/color value and it's occurence amount
        /// </summary>
        public class HistogramData
        {
            public int Value { get; private set; }
            public int Occurences { get; private set; }
            
            public HistogramData(int value, int occurences)
            {
                this.Value = value;
                this.Occurences = occurences;
            }
        }
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="histogramModel"></param>
        public HistogramTableViewModel(HistogramModel histogramModel)
        {
            int test = 0;
            for (int i = 0; i < histogramModel.PlotData.Length; i++)
            {
                if (histogramModel.PlotData[i] > 0) HistogramDataList.Add(new HistogramData(i, histogramModel.PlotData[i]));
                //if (i >= 28 && i <= 154) test += histogramModel.PlotData[i];
                if (i >= 183) test += histogramModel.PlotData[i];
            }
        }

        #endregion

        #region Public Properties   

        // list of all histogram data values on currently selected channel
        public ObservableCollection<HistogramData> HistogramDataList { get; set; } = new ObservableCollection<HistogramData>();

        #endregion
    }
}

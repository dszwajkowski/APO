using ApoCore;
using System;
using System.Linq;

namespace ApoUI
{
    /// <summary>
    /// View model for contrast enhancement dialog
    /// </summary>
    public class ContrastEnhancementViewModel : BaseViewModel
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="window"></param>
        public ContrastEnhancementViewModel(ImageViewModel window)
        {
            this.Parent = window;
            this.Series = Parent.Series;
            this.ImageModelCopy = Parent.ImageModel.DeepClone();
            SelectedChannel = Parent.ImageModel.HistogramData.FirstOrDefault();
        }

        #endregion

        #region Public Properties   

        // viewmodel that opened dialog that uses this viewmodel
        public ImageViewModel Parent { get; private set; }

        // input value min
        public int InputClippingMin
        {
            get => _InputClippingMin;
            set
            {
                if (_InputClippingMin == value || value > 254 || value < 0) return;
                if (!(value >= InputClippingMax)) _InputClippingMin = value;
                OnPropertyChanged();
                ContrastEnhancement();
            }
        }
        // input value max
        public int InputClippingMax
        {
            get => _InputClippingMax;
            set
            {
                if (_InputClippingMax == value || value > 255 || value < 1) return;
                if (!(value <= InputClippingMin)) _InputClippingMax = value;
                OnPropertyChanged();
                ContrastEnhancement();
            }
        }
        // output value min
        public int OutputCompressionMin
        {
            get => _OutputCompressionMin;
            set
            {
                if (_OutputCompressionMin == value || value > 254 || value < 0) return;
                if (!(value >= OutputCompressionMax)) _OutputCompressionMin = value;
                OnPropertyChanged();
                ContrastEnhancement();
            }
        }
        // outmput value max
        public int OutputCompressionMax
        {
            get => _OutputCompressionMax;
            set
            {
                if (_OutputCompressionMax == value || value > 255 || value < 1) return;
                if (!(value <= OutputCompressionMin)) _OutputCompressionMax = value;
                OnPropertyChanged();
                ContrastEnhancement();
            }
        }
        public double Gamma
        {
            get => _Gamma;
            set
            {
                if (_InputClippingMin == value || value > 10 || value < 0.10) return;
                _Gamma = Math.Round(value, 2);
                OnPropertyChanged();
                ContrastEnhancement();
            }
        }
        // currently selected channel
        public HistogramModel SelectedChannel
        {
            get => _SelectedChannel;
            set
            {
                if (_SelectedChannel == value) return;
                Parent.SelectedChannel = value;
                _SelectedChannel = Parent.SelectedChannel;
                PlotHistogram();
                OnPropertyChanged();
            }
        }
        // deepcopy of an ImageModel, used on for displaying second histogram
        public ImageModel ImageModelCopy { get; private set; }
        // histogram series
        public int[] Series
        {
            get => _Series;
            set
            {
                if (_Series == value) return;
                _Series = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Private fields

        private int _InputClippingMin = 0;
        private int _InputClippingMax = 255;
        private int _OutputCompressionMin = 0;
        private int _OutputCompressionMax = 255;
        private double _Gamma = 1;
        private HistogramModel _SelectedChannel;
        private int[] _Series;

        #endregion

        #region Methods

        /// <summary>
        /// Performs contrast enhancement operation
        /// </summary>
        private void ContrastEnhancement()
        {
            Parent.ImageModel = Parent.BackupModel;
            Parent.Image = Parent.BackupModel.Image;
            Parent.ImageModel = ImageModelOperations.ContrastEnhancement(Parent.ImageModel, Gamma, (byte)InputClippingMin, (byte)InputClippingMax,
                (byte)OutputCompressionMin, (byte)OutputCompressionMax, SelectedChannel.Channel);
            Parent.Image = Parent.ImageModel.Image;
            PlotHistogram();
        }

        /// <summary>
        /// Plots data depending on <see cref="SelectedChannel"/>
        /// </summary>
        private void PlotHistogram()
        {
            foreach (var item in ImageModelCopy.HistogramData)
            {
                if (item.Channel == SelectedChannel?.Channel)
                {
                    Series = item.PlotData;
                    Parent.SelectChannel(SelectedChannel.Channel);
                }
            }
        }

        #endregion
    }
}
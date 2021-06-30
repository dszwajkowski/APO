using ApoCore;
using Emgu.CV.CvEnum;
using System.Collections.ObjectModel;

namespace ApoUI
{
    /// <summary>
    /// View model for adaptive thresholding dialog
    /// </summary>
    public class AdaptiveThresholdingOperationViewModel : BaseViewModel
    {
        #region Constructor

        public AdaptiveThresholdingOperationViewModel(ImageViewModel window)
        {
            this.Parent = window;
            ThresholdTypeList = new ObservableCollection<AdaptiveThresholdType>()
            {
                AdaptiveThresholdType.MeanC,
                AdaptiveThresholdType.GaussianC,
            };
            Thresholding();
        }

        #endregion

        #region Public Properties   
        // Viewmodel that opened dialog that uses this viewmodel
        public ImageViewModel Parent;

        // thresholding value
        public int Threshold
        {
            get => threshold;
            set
            {
                if (threshold == value || value < 3 || value > 51 || value % 2 == 0) return;
                threshold = value;
                Thresholding();
                OnPropertyChanged();
            }
        }

        // Selected threshold type
        public AdaptiveThresholdType ThresholdType
        {
            get => _ThresholdType;
            set
            {
                if (_ThresholdType == value) return;
                _ThresholdType = value;
                Thresholding();
            }
        }
        // List of all supported threshold types
        public ObservableCollection<AdaptiveThresholdType> ThresholdTypeList { get; set; }
        #endregion

        #region Private fields

        private int threshold = 21;
        private AdaptiveThresholdType _ThresholdType = AdaptiveThresholdType.MeanC;

        #endregion

        #region Methods

        private void Thresholding()
        {
            Parent.Image = Parent.backupimage;
            Parent.Image = EmguOperations.AdaptiveThresholding(Parent.Image, 255, Threshold, ThresholdType);
        }

        #endregion
    }
}

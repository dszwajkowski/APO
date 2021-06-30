using ApoCore;

namespace ApoUI
{
    /// <summary>
    /// View model for thresholding dialog
    /// </summary>
    public class ThresholdingDialogViewModel : BaseViewModel
    {
        #region Constructor

        public ThresholdingDialogViewModel(ImageViewModel window)
        {
            this.Parent = window;
            Thresholding();
        }

        #endregion

        #region Public Properties   

        // viewmodel that opened dialog that uses this viewmodel
        public ImageViewModel Parent;

        // threshold min
        public int Threshold
        {
            get => _Threshold;
            set
            {
                if (_Threshold == value || value > 254 || value < 0) return;
                if (!(value >= ThresholdMax)) _Threshold = value;
                OnPropertyChanged();
                Thresholding();
            }
        }
        // threshold max
        public int ThresholdMax
        {
            get => _ThresholdMax;
            set
            {
                if (_ThresholdMax == value || value > 255 || value < 1) return;
                if (!(value <= Threshold)) _ThresholdMax = value;
                OnPropertyChanged();
                Thresholding();
            }
        }
        public bool KeepGrayLevels
        {
            get => _KeepGrayLevels;
            set
            {
                if (_KeepGrayLevels == value) return;
                if (ThresholdMax <= Threshold) ThresholdMax = Threshold++;
                _KeepGrayLevels = value;
                OnPropertyChanged();
                Thresholding();
            }
        }

        #endregion

        #region Private fields

        private int _Threshold = 127;
        private int _ThresholdMax = 255;
        private bool _KeepGrayLevels = false;

        #endregion

        #region Methods

        /// <summary>
        /// Performs thresholding operation
        /// </summary>
        private void Thresholding()
        {
            Parent.ImageModel = Parent.BackupModel;
            Parent.Image = Parent.BackupModel.Image;
            Parent.ImageModel = ImageModelOperations.Thresholding(Parent.ImageModel, Threshold, KeepGrayLevels, ThresholdMax);
            Parent.Image = Parent.ImageModel.Image;
        }

        #endregion
    }
}
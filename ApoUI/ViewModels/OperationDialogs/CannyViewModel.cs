using ApoCore;
using Emgu.CV.CvEnum;
using System.Collections.ObjectModel;

namespace ApoUI
{
    /// <summary>
    /// View model for canny edge detection dialog
    /// </summary>
    public class CannyViewModel : BaseViewModel
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="window"></param>
        public CannyViewModel(ImageViewModel window)
        {
            this.Parent = window;
            BorderTypeList = new ObservableCollection<BorderType>()
            {
                BorderType.Isolated,
                BorderType.Reflect,
                BorderType.Replicate,
            };
            Canny();
        }

        #endregion

        #region Public Properties   
        
        // viewmodel that opened dialog that uses this viewmodel
        public ImageViewModel Parent;
        // threshold value
        public int Threshold
        {
            get => _Threshold;
            set
            {
                if (_Threshold == value || value > 254 || value < 0) return;
                if (!(value >= ThresholdMax)) _Threshold = value;
                OnPropertyChanged();
                Canny();                
            }
        }
        // maximum threshold value
        public int ThresholdMax
        {
            get => _ThresholdMax;
            set
            {
                if (_ThresholdMax == value || value > 255 || value < 1) return;
                if (!(value <= Threshold)) _ThresholdMax = value;
                OnPropertyChanged();
                Canny();               
            }
        }
        // currently selected border
        public BorderType BorderType
        {
            get => _BorderType;
            set
            {
                if (_BorderType == value) return;
                _BorderType = value;
                OnPropertyChanged();
                Canny();
            }
        }
        // list of all supported borders
        public ObservableCollection<BorderType> BorderTypeList { get; set; }
              
        #endregion

        #region Private fields

        private int _Threshold = 100;
        private int _ThresholdMax = 200;
        private BorderType _BorderType = BorderType.Isolated;

        #endregion

        #region Methods
        private void Canny()
        {
            Parent.Image = Parent.backupimage;
            Parent.Image = EmguOperations.Canny(Parent.Image, Threshold, ThresholdMax, BorderType);
        }

        #endregion
    }
}

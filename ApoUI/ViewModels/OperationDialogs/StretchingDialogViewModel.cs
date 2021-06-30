using ApoCore;

namespace ApoUI
{
    /// <summary>
    /// View model for stretching dialog
    /// </summary>
    public class StretchingDialogViewModel : BaseViewModel
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="window"></param>
        public StretchingDialogViewModel(ImageViewModel window)
        {
            this.Parent = window;
            Stretching();
        }

        #endregion

        #region Public Properties   

        // viewmodel that opened dialog that uses this viewmodel
        public ImageViewModel Parent;
        // min
        public int P1
        {
            get => _P1;
            set
            {
                if (_P1 == value || value > 254 || value < 0) return;
                if (!(value >= P2)) _P1 = value;
                OnPropertyChanged();
                Stretching();                
            }
        }
        // max
        public int P2
        {
            get => _P2;
            set
            {
                if (_P2 == value || value > 255 || value < 1) return;
                if (!(value <= P1)) _P2 = value;
                OnPropertyChanged();
                Stretching();               
            }
        }

        #endregion

        #region Private fields

        private int _P1 = 127;
        private int _P2 = 255;

        #endregion

        #region Methods

        /// <summary>
        /// Performs stretching operation
        /// </summary>
        private void Stretching()
        {
            Parent.ImageModel = Parent.BackupModel;
            Parent.Image = Parent.BackupModel.Image;
            Parent.ImageModel = ImageModelOperations.HistogramStretchLinear(Parent.ImageModel, P1, P2);
            Parent.Image = Parent.ImageModel.Image;
        }

        #endregion
    }
}

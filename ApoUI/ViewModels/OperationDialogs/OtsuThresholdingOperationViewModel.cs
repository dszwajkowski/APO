using ApoCore;

namespace ApoUI
{
    /// <summary>
    /// View model for otsu thresholding dialog
    /// </summary>
    public class OtsuThresholdingOperationViewModel
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="window"></param>
        public OtsuThresholdingOperationViewModel(ImageViewModel window)
        {
            this.Parent = window;
            Thresholding();
        }

        #endregion

        #region Public Properties   

        // viewmodel that opened dialog that uses this viewmodel
        public ImageViewModel Parent;
        public bool UseGaussianBlur
        {
            get => _UseGaussianBlur;
            set
            {
                if (_UseGaussianBlur == value) return;
                _UseGaussianBlur = value;
                Thresholding();
            }
        }

        #endregion

        #region Private fields

        private bool _UseGaussianBlur = false;

        #endregion

        #region Methods

        private void Thresholding()
        {
            Parent.Image = Parent.backupimage;
            Parent.Image = EmguOperations.OtsuThresholding(Parent.Image, 127, UseGaussianBlur);
        }

        #endregion
    }
}

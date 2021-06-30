using ApoCore;
using System.Windows.Input;

namespace ApoUI
{
    /// <summary>
    /// View model for medianblur dialog
    /// </summary>
    public class MedianBlurOperationViewModel : BaseViewModel
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="window"></param>
        public MedianBlurOperationViewModel(ImageViewModel window)
        {
            this.Parent = window;
            MedianBlur();
        }

        #endregion

        #region Public Properties   

        // viewmodel that opened dialog that uses this viewmodel
        public ImageViewModel Parent;
        public int Size
        {
            get => _Size;
            set
            {
                if (_Size == value || value < 1 || value % 2 == 0) return;
                _Size = value;
                MedianBlur();
                OnPropertyChanged();
            }
        }

        #endregion

        #region Private fields

        private int _Size = 1;

        #endregion

        #region Commands

        /// <summary>
        /// Increases window size (for medianblur) by 2
        /// </summary>
        public ICommand IncreaseSizeCommand => new RelayCommand(() => Size += 2);
        /// <summary>
        /// Decreases window size (for medianblur) by 2
        /// </summary>
        public ICommand DecreaseSizeCommand => new RelayCommand(() => Size -= 2);

        #endregion

        #region Methods

        /// <summary>
        /// Performs median blur operation
        /// </summary>
        private void MedianBlur()
        {
            Parent.Image = Parent.backupimage;
            Parent.Image = EmguOperations.MedianBlur(Parent.Image, Size);
        }

        #endregion
    }
}

using ApoCore;
using System.Windows.Input;

namespace ApoUI
{
    /// <summary>
    /// View model for posterize operation dialog
    /// </summary>
    public class PosterizeOperationViewModel : BaseViewModel
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="window"></param>
        public PosterizeOperationViewModel(ImageViewModel window)
        {
            this.Parent = window;
            Posterize();
        }

        #endregion

        #region Public Properties   

        // viewmodel that opened dialog that uses this viewmodel
        public ImageViewModel Parent;
        // number of bins
        public int NumberOfBins
        {
            get => _NumberOfBins;
            set
            {
                if (_NumberOfBins == value || value > 10 || value < 0) return;
                _NumberOfBins = value;
                Posterize();
                OnPropertyChanged();
            }
        }

        #endregion

        #region Private fields

        private int _NumberOfBins = 8;

        #endregion

        #region Commands
        
        /// <summary>
        /// Increases number of bins by 1
        /// </summary>
        public ICommand IncreaseBinsCommand => new RelayCommand(() => NumberOfBins++);
        /// <summary>
        /// Decreases number of bins by 1
        /// </summary>
        public ICommand DecreaseBinsCommand => new RelayCommand(() => NumberOfBins--);

        #endregion

        #region Methods

        /// <summary>
        /// Performs posterize operation
        /// </summary>
        private void Posterize()
        {
            Parent.ImageModel = Parent.BackupModel;
            Parent.Image = Parent.BackupModel.Image;
            Parent.ImageModel = ImageModelOperations.Posterize(Parent.ImageModel, NumberOfBins);
            Parent.Image = Parent.ImageModel.Image;
        }

        #endregion
    }
}

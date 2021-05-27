using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;

namespace ApoUI
{
    /// <summary>
    /// View Model for custom window
    /// </summary>
    public class MainPageViewModel : BaseViewModel
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="window"></param>
        public MainPageViewModel()
        {
            Page = new ImagePage();
        }

        #endregion

        #region Public properties

        public Page Page
        {
            get => _Page;
            set
            {
                if (_Page == value) return;
                _Page = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<ImageViewModel> ImageList = new ObservableCollection<ImageViewModel>();

        public ImageViewModel SelectedImage
        {
            get => _SelectedImage;
            set
            {
                if (_SelectedImage == value) return;
                _SelectedImage = value;
                Page.DataContext = SelectedImage;
                OnPropertyChanged();
            }
        }

        public bool HistogramVisible
        {
            get => _HistogramVisible;
            set
            {
                if (_HistogramVisible == value) return;
                _HistogramVisible = value;
                OnPropertyChanged();
            }
        }
        public bool IsImageLoaded
        {
            get => _IsImageLoaded;
            set
            {
                if (_IsImageLoaded == value) return;
                _IsImageLoaded = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Private fields

        private Page _Page;
        private ImageViewModel _SelectedImage = new ImageViewModel();
        private bool _HistogramVisible = false;
        private bool _IsImageLoaded = false;
        #endregion

        #region Commands

        public ICommand GetImagePathCommand => new RelayCommand(GetImagePath);
        public ICommand CloseImageCommand => new RelayCommand(CloseImage);
        public ICommand SaveImageCommand => new RelayCommand(SaveImage);
        public ICommand SaveImageAsCommand => new RelayCommand(SaveImageAs);
        public ICommand SaveAllImagesCommand => new RelayCommand(SaveAllImages);
        public ICommand DuplicateImageCommand => new RelayCommand(DuplicateImage);
        public ICommand ToggleHistogramCommand => new RelayCommand(ToggleHistogram);

        #endregion

        #region Command methods

        /// <summary>
        /// Opens file dialog
        /// </summary>
        private void GetImagePath()
        {
            OpenFileDialog openFile = new();
            openFile.Filter = "Image files (*.jpg;*jpeg;*.png;*.bmp)|*.jpg;*.jpeg;*.png;*.bmp";
            if (openFile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                var newImageViewModel = new ImageViewModel(openFile.FileName);
                ImageList.Add(newImageViewModel);
                SelectedImage = newImageViewModel;
                IsImageLoaded = true;
            }
        }

        /// <summary>
        /// Closes currently opened image
        /// </summary>
        private void CloseImage()
        {
            ImageList.Remove(SelectedImage);
            if (ImageList.Count > 0) SelectedImage = ImageList.Last();
            else
            {
                SelectedImage = null;
                IsImageLoaded = false;
                HistogramVisible = false;
            }
        }

        /// <summary>
        /// Save changes to currently selected image
        /// </summary>
        private void SaveImage()
        {
            if (System.IO.File.Exists(SelectedImage.ImagePath))
                System.IO.File.Delete(SelectedImage.ImagePath);
            SelectedImage.Image.Save(SelectedImage.ImagePath);
        }

        /// <summary>
        /// Saves currently selected image as
        /// </summary>
        private void SaveImageAs()
        {
            SaveFileDialog saveFile = new();
            saveFile.Filter = "JPG (*.jpg)|*.jpg|PNG (*.png)|*.png|BMP (*.bmp)|*.bmp";
            if (saveFile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                SelectedImage.Image.Save(saveFile.FileName);
            }
        }

        /// <summary>
        /// Saves changes to all currently opened images
        /// </summary>
        private void SaveAllImages()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Duplicates selected image
        /// </summary>
        private void DuplicateImage()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Toggles histogram visibility 
        /// </summary>
        private void ToggleHistogram()
        {
            HistogramVisible ^= true;
        }

        #endregion
    }
}

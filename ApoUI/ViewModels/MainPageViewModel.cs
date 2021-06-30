using ApoCore;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
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
            DialogGuid = Guid.NewGuid().ToString();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="page"></param>
        /// <param name="viewModel"></param>
        public MainPageViewModel(Page page, ImageViewModel viewModel)
        {
            Page = page;
            ImageList.Add(viewModel);
            SelectedImage = viewModel;
            IsImageLoaded = true;
            DialogGuid = Guid.NewGuid().ToString();
            SelectedImage.DialogGuid = DialogGuid;
        }

        #endregion

        #region Public properties

        // currently displayed page
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
        // list of all opened images (tabs)
        public ObservableCollection<ImageViewModel> ImageList { get; set; } = new ObservableCollection<ImageViewModel>();

        // image from cyrrently selected tab
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
        public ImageViewModel SecondImage
        {
            get => _SecondImage;
            set
            {
                if (_SecondImage == value) return;
                _SecondImage = value;                
                OnPropertyChanged();
            }
        }

        // controls visibility of histogram control
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
        // id of DialogHost, allows controling dialogs in multiple windows
        public string DialogGuid { get; private set; }

        #endregion

        #region Private fields

        private Page _Page;
        private ImageViewModel _SelectedImage = null;
        private ImageViewModel _SecondImage = null;
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
        public ICommand OpenImageNewWindowCommand => new RelayCommand(OpenImageNewWindow);
        public ICommand ArithmeticCommand => new RelayCommand(Arithmetic);
        public ICommand ToggleHistogramCommand => new RelayCommand(ToggleHistogram);
        public ICommand ShowInfoCommand => new RelayCommand(ShowInfo);

        #endregion

        #region Command methods

        /// <summary>
        /// Opens file dialog
        /// </summary>
        private void GetImagePath()
        {
            OpenFileDialog openFile = new();
            openFile.Filter = "Image files (*.jpg;*jpeg;*.png;*.bmp;*tif)|*.jpg;*.jpeg;*.png;*.bmp;*tif";
            if (openFile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                var newImageViewModel = new ImageViewModel(openFile.FileName, DialogGuid);
                ImageList.Add(newImageViewModel);
                OnPropertyChanged(nameof(ImageList));
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
            if (SelectedImage.ImagePath == null)
            {
                SaveImageAs();
                return;
            }
            if (System.IO.File.Exists(SelectedImage.ImagePath)) System.IO.File.Delete(SelectedImage.ImagePath);
            SelectedImage.Image.Save(SelectedImage.ImagePath);
        }

        /// <summary>
        /// Saves currently selected image as
        /// </summary>
        private void SaveImageAs()
        {
            SaveFileDialog saveFile = new();
            saveFile.Filter = "JPG (*.jpg)|*.jpg|PNG (*.png)|*.png|BMP (*.bmp)|*.bmp|TIFF (*.tif)|*.tif";
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
        /// Opens currently selected image in new window
        /// </summary>
        private void OpenImageNewWindow()
        {
            if (SelectedImage != null)
            {
                var view = new CustomWindow(new MainPage(new ImagePage(), SelectedImage));
                CloseImage();
                view.Show();
            }
        }

        /// <summary>
        /// Duplicates selected image
        /// </summary>
        private void DuplicateImage()
        {
            if (SelectedImage != null)
            {
                int i = 1;
                string newName = $"{SelectedImage.ImageName}_{i}";
                for (int image = 0; image < ImageList.Count; image++)
                {
                    if (ImageList[image].ImageName == newName)
                    {
                        i++;
                        newName = $"{SelectedImage.ImageName}_{i}";
                        image = 0;
                    }
                }
                var copyModel = SelectedImage.ImageModel.DeepClone();
                var copy = new ImageViewModel(copyModel, newName, DialogGuid);
                ImageList.Add(copy);
                SelectedImage = copy;
            }
        }

        /// <summary>
        /// Opens dialog for arithmetic operations
        /// </summary>
        private async void Arithmetic()
        {
            if (ImageList.Count > 1)
            {
                while (SelectedImage == null)
                {
                    foreach (var item in ImageList)
                    {
                        if (item != SelectedImage) SecondImage = item.DeepClone();
                    }
                }
                SelectedImage.backupimage = SelectedImage.Image;
                var view = new ArithmeticDialog()
                {
                    DataContext = new ArithmeticOperationViewModel(SelectedImage, ImageList)
                };
                await DialogHost.Show(view, DialogGuid, ClosingEventHandler);
            }
            else
            {
                var view = new MessageDialog()
                {
                    DataContext = new MessageDialogViewModel("At least two images have to be open in the same window.")
                };
                await DialogHost.Show(view, DialogGuid);
            }
        }

        /// <summary>
        /// Toggles histogram visibility 
        /// </summary>
        private void ToggleHistogram()
        {
            HistogramVisible ^= true;
        }

        /// <summary>
        /// Shows information about application
        /// </summary>
        private async void ShowInfo()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Aplikacja zbiorcza z ćwiczeń laboratoryjnych i projektu" + Environment.NewLine)
            .Append("Tytuł projektu: Udoskonalenie oprogramowania przygotowanego" + Environment.NewLine +
            "na zajęciach przez wykonanie narzędzie do rozciągania i" + Environment.NewLine +
            "zawężania histogramu z jednoczesnym zastosowaniem funkcji gamma" + Environment.NewLine)
            .Append("Autor: Dawid Szwajkowski" + Environment.NewLine)
            .Append("Prowadzący: dr hab. Anna Korzyńska, mgr inż. Łukasz Roszkowiak " + Environment.NewLine)
            .Append("Algorytmy Przetwarzania Obrazów 2021" + Environment.NewLine)
            .Append("WIT grupa ID: ID06IO1");
            var view = new MessageDialog()
            {
                DataContext = new MessageDialogViewModel(sb.ToString())
            };
            await DialogHost.Show(view, DialogGuid);
        }

        #endregion

        #region Helpers

        /// <summary>
        /// Intercepts dialog closing event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        private void ClosingEventHandler(object sender, DialogClosingEventArgs eventArgs)
        {
            if (eventArgs.Parameter is bool parameter && parameter == true) return;
            else SelectedImage.Image = SelectedImage.backupimage;
        }

        #endregion
    }
}

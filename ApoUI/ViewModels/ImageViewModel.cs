using ApoCore;
using static ApoCore.EmguOperations;
using static ApoCore.ImageOperations;
using static ApoCore.ImageModel;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Input;
using MaterialDesignThemes.Wpf;

namespace ApoUI
{
    /// <summary>
    /// View model for image items
    /// </summary>
    public class ImageViewModel : BaseViewModel
    {
        #region Constructors

        /// <summary>
        /// Default Constructor
        /// </summary>
        public ImageViewModel()
        {
            //CurrentSideMenuContent = SideMenuModel.Histogram;
            SideMenuVisible = false;
        }

        public ImageViewModel(string imagePath)
        {
            ImagePath = imagePath;
            ImageModel = new ImageModel(ImagePath);
            Image = ImageModel.Image;
            SelectedChannel = ImageModel.HistogramData[0];             
        }

        #endregion

        #region Public Properties   

        public ImageModel ImageModel
        {
            get => imagemodel;
            set
            {
                if (imagemodel == value) return;
                imagemodel = value;
                OnPropertyChanged();
            }
        }
        public Bitmap Image
        {
            get => image;
            set
            {
                image = value;
                ImageModel.Image = image;
                UpdateHistogram();
                OnPropertyChanged();
            }
        }
        public string ImagePath
        {
            get => imagepath;
            set
            {
                if (imagepath == value) return;
                imagepath = value; ;
                OnPropertyChanged();
            }
        }
        
        public bool SideMenuVisible
        {
            get => sidemenuvisible;
            set
            {
                if (sidemenuvisible == value) return;
                sidemenuvisible = value;
                OnPropertyChanged();
            }
        }
        public HistogramModel SelectedChannel
        {
            get => _SelectedChannel;
            set
            {
                if (_SelectedChannel == value) return;
                _SelectedChannel = value;
                PlotHistogram(_SelectedChannel);
                OnPropertyChanged();
            }
        }
        public List<string> Labels = new List<string>();
        public int[] Series
        {
            get => series;
            set
            {
                if (series == value) return;
                series = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Private fields

        private ImageModel imagemodel;
        private Bitmap image;
        private Bitmap backupimage;
        private string imagepath;
               
        private HistogramModel _SelectedChannel;
        private bool sidemenuvisible = false;
        // Side menu control related properties, not yet implemented
        //private SideMenuModel currentsidemenucontent;

        private int[] series;

        #endregion

        #region Commands

        public ICommand ConvertToGrayscaleCommand => new RelayCommand(ConvertToGrayscale);
        public ICommand NegationCommand => new RelayCommand(Negation);
        public ICommand ThresholdingCommand => new RelayCommand(Thresholding);
        public ICommand PosterizeCommand => new RelayCommand(Posterize);
        public ICommand EqualizeCommand => new RelayCommand(Equalize);
        public ICommand StretchCommand => new RelayCommand(Stretch);
        
        public ICommand BlurCommand => new RelayCommand(Blur);
        public ICommand GaussianBlurCommand => new RelayCommand(GaussianBlur);
        public ICommand SobelCommand => new RelayCommand(Sobel);
        public ICommand LaplacianCommand => new RelayCommand(Laplacian);
        public ICommand CannyCommand => new RelayCommand(Canny);
        public ICommand SharpeningCommand => new RelayCommand(Sharpening);

        public ICommand AddCommand => new RelayCommand(Add);
        public ICommand SubtractCommand => new RelayCommand(Subtract);
        public ICommand BlendCommand => new RelayCommand(Blend);
        public ICommand AndCommand => new RelayCommand(And);
        public ICommand OrCommand => new RelayCommand(Or);
        public ICommand NotCommand => new RelayCommand(Not);
        public ICommand XorCommand => new RelayCommand(XorOperation);

        public ICommand MorphologyCommand => new RelayCommand(Morphology);
        public ICommand FiltrationTwoCommand => new RelayCommand(FiltrationTwo);
        public ICommand SkeletonCommand => new RelayCommand(Skeleton);

        public ICommand OtsuThresholdingCommand => new RelayCommand(OtsuThresholding);
        public ICommand AdaptiveThresholdingCommand => new RelayCommand(AdaptiveThresholding);

        #endregion

        #region Command methods - image operations

        /// <summary>
        /// Converts colorfull image to grayscale
        /// </summary>
        private void ConvertToGrayscale()
        {
            if (ImageModel.IsColorful == false) return;
            else
            {
                ImageModel.ToGrayScale();
                ImageModel.IsColorful = false;               
                Image = ImageModel.Image;
                SelectedChannel = ImageModel.HistogramData[0];
            }           
        }

        /// <summary>
        /// Performs negation operation
        /// </summary>
        private void Negation()
        {
            ImageModel = ImageModelOperations.Negation(ImageModel);
            Image = ImageModel.Image;
        }

        /// <summary>
        /// Performs thresholding operation
        /// </summary>
        private async void Thresholding()
        {
            backupimage = Image;
            var view = new ThresholdingDialog()
            {
                DataContext = new ThresholdingOperationViewModel(this)
            };
            var result = await DialogHost.Show(view, "RootDialog", ClosingEventHandler);
        }

        /// <summary>
        /// Performs posterize operation
        /// </summary>
        private void Posterize()
        {
            ImageModel = ImageModelOperations.Posterize(ImageModel);
            Image = ImageModel.Image;
        }

        private void Equalize()
        {
            MessageBox.Show("Not implemented yet :(");
        }

        /// <summary>
        /// Performs histogram stretching operation
        /// </summary>
        private void Stretch()
        {
            ImageModel = ImageModelOperations.HistogramStretch(ImageModel);
            Image = ImageModel.Image;
        }

        /// <summary>
        /// Performs blur operation
        /// </summary>
        private async void Blur()
        {
            backupimage = Image;
            var view = new BorderDialog()
            {
                DataContext = new BorderTypeViewModel(this, BorderTypeViewModel.BorderTypeOperations.Blur)
            };
            var result = await DialogHost.Show(view, "RootDialog", ClosingEventHandler);
        }

        /// <summary>
        /// Performs gaussian blur operation
        /// </summary>
        private async void GaussianBlur()
        {
            backupimage = Image;
            var view = new BorderDialog()
            {
                DataContext = new BorderTypeViewModel(this, BorderTypeViewModel.BorderTypeOperations.GaussianBlur)
            };
            var result = await DialogHost.Show(view, "RootDialog", ClosingEventHandler);
        }

        private async void Sobel()
        {
            backupimage = Image;
            var view = new BorderDialog()
            {
                DataContext = new BorderTypeViewModel(this, BorderTypeViewModel.BorderTypeOperations.Sobel)
            };
            var result = await DialogHost.Show(view, "RootDialog", ClosingEventHandler);
        }

        private async void Laplacian()
        {
            backupimage = Image;
            var view = new BorderDialog()
            {
                DataContext = new BorderTypeViewModel(this, BorderTypeViewModel.BorderTypeOperations.Laplacian)
            };
            var result = await DialogHost.Show(view, "RootDialog", ClosingEventHandler);
        }

        private void Canny()
        {
            ImageModel.Image = EmguOperations.Canny(ImageModel.Image);
            Image = ImageModel.Image;
        }

        private void Sharpening()
        {
            ImageModel.Image = LinearSharpening(ImageModel.Image);
            Image = ImageModel.Image;
        }

        private void Add()
        {
            OpenFileDialog openFile = new();
            openFile.Filter = "Image files (*.jpg;*jpeg;*.png;*.bmp)|*.jpg;*.jpeg;*.png;*.bmp";
            if (openFile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Bitmap secondImage = new(openFile.FileName);
                //ImageModel.Image = ImageModel.Image.ArithmeticOperations(ArithmeticOperationType.Add, secondImage);
                ImageModel.Image = ArithmeticOperations(ImageModel.Image, ArithmeticOperationType.Add, secondImage);
            }
            Image = ImageModel.Image;
        }

        private void Subtract()
        {
            OpenFileDialog openFile = new();
            openFile.Filter = "Image files (*.jpg;*jpeg;*.png;*.bmp)|*.jpg;*.jpeg;*.png;*.bmp";
            if (openFile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Bitmap secondImage = new(openFile.FileName);
                //ImageModel.Image = ImageModel.Image.ArithmeticOperations(ArithmeticOperationType.Subtract, secondImage);
                ImageModel.Image = ArithmeticOperations(ImageModel.Image, ArithmeticOperationType.Subtract, secondImage);
            }
            Image = ImageModel.Image;
        }

        private void Blend()
        {
            OpenFileDialog openFile = new();
            openFile.Filter = "Image files (*.jpg;*jpeg;*.png;*.bmp)|*.jpg;*.jpeg;*.png;*.bmp";
            if (openFile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Bitmap secondImage = new(openFile.FileName);
                //ImageModel.Image = ImageModel.Image.ArithmeticOperations(ArithmeticOperationType.Blend, secondImage);
                ImageModel.Image = ArithmeticOperations(ImageModel.Image, ArithmeticOperationType.Blend, secondImage);
            }
            Image = ImageModel.Image;
        }

        private void And()
        {
            OpenFileDialog openFile = new();
            openFile.Filter = "Image files (*.jpg;*jpeg;*.png;*.bmp)|*.jpg;*.jpeg;*.png;*.bmp";
            if (openFile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Bitmap secondImage = new(openFile.FileName);
                //ImageModel.Image = ImageModel.Image.ArithmeticOperations(ArithmeticOperationType.And, secondImage);
                ImageModel.Image = ArithmeticOperations(ImageModel.Image, ArithmeticOperationType.And, secondImage);
            }
            Image = ImageModel.Image;
            //OnPropertyChanged(nameof(Image));
        }

        private void Or()
        {
            OpenFileDialog openFile = new();
            openFile.Filter = "Image files (*.jpg;*jpeg;*.png;*.bmp)|*.jpg;*.jpeg;*.png;*.bmp";
            if (openFile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Bitmap secondImage = new(openFile.FileName);
                //ImageModel.Image = ImageModel.Image.ArithmeticOperations(ArithmeticOperationType.Or, secondImage);
                ImageModel.Image = ArithmeticOperations(ImageModel.Image, ArithmeticOperationType.Or, secondImage);
            }
            Image = ImageModel.Image;
        }

        private void Not()
        {
            //ImageModel.Image = ImageModel.Image.ArithmeticOperations(operationType: ArithmeticOperationType.Not);
            ImageModel.Image = ArithmeticOperations(ImageModel.Image, ArithmeticOperationType.Not);
            Image = ImageModel.Image;
        }

        private void XorOperation()
        {
            //var view = new PointOperationsDialog()
            //{
            //    DataContext = new PointOperationsViewModel(ImageModel)
            //};
            //view.Show();
            OpenFileDialog openFile = new();
            openFile.Filter = "Image files (*.jpg;*jpeg;*.png;*.bmp)|*.jpg;*.jpeg;*.png;*.bmp";
            if (openFile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Bitmap secondImage = new(openFile.FileName);
                //ImageModel.Image = ImageModel.Image.ArithmeticOperations(ArithmeticOperationType.Xor, secondImage);
                ImageModel.Image = ArithmeticOperations(ImageModel.Image, ArithmeticOperationType.Xor, secondImage);
            }
            Image = ImageModel.Image;
        }

        private async void Morphology()
        {
            backupimage = Image;
            var view = new MorphologyDialog()
            {
                DataContext = new MorphologyOperationViewModel(this)
            };
            var result = await DialogHost.Show(view, "RootDialog", ClosingEventHandler);            
        }

        private void FiltrationTwo()
        {
            Image = FiltrationTwoStep(Image);
        }

        private async void Skeleton()
        {
            backupimage = Image;
            var view = new SkeletonDialog()
            {
                DataContext = new SkeletonOperationViewModel(this)
            };
            var result = await DialogHost.Show(view, "RootDialog", ClosingEventHandler);
        }

        private void OtsuThresholding()
        {
            ImageModel.Image = EmguOperations.OtsuThresholding(ImageModel.Image, 126);
            Image = ImageModel.Image;
        }

        private void AdaptiveThresholding()
        {
            ImageModel.Image = EmguOperations.AdaptiveThresholding(ImageModel.Image);
            Image = ImageModel.Image;
            //ImageModel.Image = EmguOperations.Test(ImageModel.Image);
            //Image = ImageModel.Image;
        }

        #endregion

        #region Command methods - UI related

        /// <summary>
        /// Puts histogram data on the chart
        /// </summary>
        private void PlotHistogram(HistogramModel selecteditem)
        {
            for (int i = 0; i <= 255; i++)
            {
                Labels.Add(i.ToString());
            }
            Series = selecteditem.PlotData;
        }

        /// <summary>
        /// Updates histogram after performing operation on image
        /// </summary>
        private void UpdateHistogram()
        {
            ImageModel.UpdateHistogramData();
            foreach (var item in ImageModel.HistogramData)
            {
                if (item.Channel == SelectedChannel?.Channel) SelectedChannel = item;
            }
        }

        private void ClosingEventHandler(object sender, DialogClosingEventArgs eventArgs)
        {
            if (eventArgs.Parameter is bool parameter &&
                parameter == true) return;
            else Image = backupimage;
        }

        #endregion
    }
}


// TODO: poprawic zapis, karty/konroler okien
// https://web.csulb.edu/~pnguyen/cecs475/pdf/closingwindowmvvm.pdf
using ApoCore;
using static ApoCore.EmguOperations;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Input;
using MaterialDesignThemes.Wpf;
using System.IO;

namespace ApoUI
{
    /// <summary>
    /// View model for image items
    /// </summary>
    public class ImageViewModel : BaseViewModel
    {
        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="imagePath"></param>
        /// <param name="dialogGuid"></param>
        public ImageViewModel(string imagePath, string dialogGuid)
        {
            ImagePath = imagePath;
            ImageModel = new ImageModel(ImagePath);
            Image = ImageModel.Image;
            SelectedChannel = ImageModel.HistogramData[0];
            DialogGuid = dialogGuid;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="imageModel"></param>
        /// <param name="name"></param>
        /// <param name="dialogGuid"></param>
        public ImageViewModel(ImageModel imageModel, string name, string dialogGuid)
        {
            ImageModel = imageModel;
            Image = ImageModel.Image;
            ImagePath = null;
            ImageName = name;
            SelectedChannel = ImageModel.HistogramData[0];
            DialogGuid = dialogGuid;
        }

        #endregion

        #region Public Properties   

        // image model
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
        // back of ImageModel, used before performing operation on ImageModel
        public ImageModel BackupModel;
        // image
        public Bitmap Image
        {
            get => image;
            set
            {
                image = value;
                ImageModel.Image = image;
                UpdateHistogram();
                OnPropertyChanged();
                //OnPropertyChanged(nameof(ImageModel));
            }
        }
        // backup of an image, used before performing operation on Image
        public Bitmap backupimage;
        // path of an image
        public string ImagePath
        {
            get => imagepath;
            set
            {
                if (imagepath == value) return;
                imagepath = value;
                ImageName = Path.GetFileNameWithoutExtension(ImagePath);
                OnPropertyChanged();
            }
        }
        // name of an image
        public string ImageName
        {
            get => imagename;
            set
            {
                if (imagename == value) return;
                imagename = value;
                OnPropertyChanged();
            }
        }
        // currently selected channel of an image
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
        // labels for X axis of chart
        public List<string> Labels { get; private set; } = new List<string>();
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
        // id of DialogHost, allows controling dialogs in multiple windows
        public string DialogGuid { get; set; }

        #endregion

        #region Private fields

        private ImageModel imagemodel;
        private Bitmap image;
        private string imagepath;
        private string imagename;               
        private HistogramModel _SelectedChannel;        
        private int[] series;

        #endregion

        #region Commands

        public ICommand ShowHistogramDataCommand => new RelayCommand(ShowHistogramData);
        public ICommand ConvertToGrayscaleCommand => new RelayCommand(ConvertToGrayscale);
        public ICommand NegationCommand => new RelayCommand(Negation);
        public ICommand ThresholdingCommand => new RelayCommand(Thresholding);
        public ICommand PosterizeCommand => new RelayCommand(Posterize);
        public ICommand EqualizeCommand => new RelayCommand(Equalize);
        public ICommand StretchCommand => new RelayCommand(Stretch);
        public ICommand StretchCustomRangeCommand => new RelayCommand(StretchCustomRange);

        public ICommand BlurCommand => new RelayCommand(Blur);
        public ICommand GaussianBlurCommand => new RelayCommand(GaussianBlur);
        public ICommand SobelCommand => new RelayCommand(Sobel);
        public ICommand LaplacianCommand => new RelayCommand(Laplacian);
        public ICommand CannyCommand => new RelayCommand(Canny);
        public ICommand SharpeningCommand => new RelayCommand(Sharpening);
        public ICommand SharpeningPrewittCommand => new RelayCommand(SharpeningPrewitt);
        public ICommand SharpeningCustomCommand => new RelayCommand(SharpeningCustom);
        public ICommand MedianFilterCommand => new RelayCommand(MedianFilter);
        public ICommand NotCommand => new RelayCommand(Not);

        public ICommand MorphologyCommand => new RelayCommand(Morphology);
        public ICommand FiltrationOneStepCommand => new RelayCommand(FiltrationOneStep);
        public ICommand FiltrationTwoStepCommand => new RelayCommand(FiltrationTwoStep);
        public ICommand SkeletonCommand => new RelayCommand(Skeleton);

        public ICommand OtsuThresholdingCommand => new RelayCommand(OtsuThresholding);
        public ICommand AdaptiveThresholdingCommand => new RelayCommand(AdaptiveThresholding);
        public ICommand WatershedCommand => new RelayCommand(Watershed);

        public ICommand FeatureVectorCommand => new RelayCommand(FeatureVector);
        public ICommand SvmCommand => new RelayCommand(Svm);

        public ICommand ContrastEnhancementCommand => new RelayCommand(ContrastEnhancement);

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
                try
                {
                    ImageModel.ToGrayScale();
                    ImageModel.IsColorful = false;
                    Image = ImageModel.Image;
                    SelectedChannel = ImageModel.HistogramData[0];
                }
                catch (System.Exception)
                {
                    ShowMessage("Image not supported for this operation.");
                }                
            }           
        }

        /// <summary>
        /// Performs negation operation
        /// </summary>
        private void Negation()
        {
            try
            {
                ImageModel = ImageModelOperations.Negation(ImageModel);
                Image = ImageModel.Image;
            }
            catch (System.Exception)
            {
                ShowMessage("Image not supported for this operation.");
            }
        }

        /// <summary>
        /// Opens <see cref="HistogramTableDialog"/>
        /// </summary>
        public async void ShowHistogramData()
        {
            try
            {
                if (SelectedChannel != null)
                {
                    var view = new HistogramTableDialog()
                    {
                        DataContext = new HistogramTableViewModel(SelectedChannel)
                    };
                    await DialogHost.Show(view, DialogGuid);
                }
            }
            catch (System.InvalidOperationException) { }
        }

        /// <summary>
        /// Opens <see cref="ThresholdingDialog"/>
        /// </summary>
        private async void Thresholding()
        {
            try
            {
                backupimage = Image;
                BackupModel = ImageModel;
                var view = new ThresholdingDialog()
                {
                    DataContext = new ThresholdingDialogViewModel(this)
                };
                var result = await DialogHost.Show(view, DialogGuid, ClosingEventHandlerImageModel);
            }
            catch (System.Exception)
            {
                ShowMessage("Image not supported for this operation.");
            }           
        }

        /// <summary>
        /// Opens <see cref="PosterizeDialog"/>
        /// </summary>
        private async void Posterize()
        {
            try
            {
                backupimage = Image;
                BackupModel = ImageModel;
                var view = new PosterizeDialog()
                {
                    DataContext = new PosterizeOperationViewModel(this)
                };
                var result = await DialogHost.Show(view, DialogGuid, ClosingEventHandlerImageModel);
            }
            catch (System.Exception)
            {
                ShowMessage("Image not supported for this operation.");
            }           
        }

        /// <summary>
        /// Performs equalize operation
        /// </summary>
        private void Equalize()
        {
            try
            {
                ImageModel = ImageModelOperations.Equalization(ImageModel);
                Image = ImageModel.Image;
            }
            catch (System.Exception)
            {
                ShowMessage("Image not supported for this operation.");
            }
        }

        /// <summary>
        /// Performs histogram stretching operation
        /// </summary>
        private void Stretch()
        {
            try
            {
                ImageModel = ImageModelOperations.HistogramStretch(ImageModel);
                Image = ImageModel.Image;
            }
            catch (System.Exception)
            {
                ShowMessage("Image not supported for this operation.");
            }            
        }

        /// <summary>
        /// Opens <see cref="StretchingDialog"/>
        /// </summary>
        private async void StretchCustomRange()
        {
            try
            {
                backupimage = Image;
                BackupModel = ImageModel;
                var view = new StretchingDialog()
                {
                    DataContext = new StretchingDialogViewModel(this)
                };
                var result = await DialogHost.Show(view, DialogGuid, ClosingEventHandlerImageModel);
            }
            catch (System.Exception)
            {
                ShowMessage("Image not supported for this operation.");
            }
        }

        /// <summary>
        /// Opens <see cref="BorderDialog"/> with <see cref="BorderTypeViewModel.BorderTypeOperations.Blur"/> parameter
        /// </summary>
        private async void Blur()
        {
            try
            {
                backupimage = Image;
                var view = new BorderDialog()
                {
                    DataContext = new BorderTypeViewModel(this, BorderTypeViewModel.BorderTypeOperations.Blur)
                };
                var result = await DialogHost.Show(view, DialogGuid, ClosingEventHandler);
            }
            catch (System.Exception)
            {
                ShowMessage("Image not supported for this operation.");
            }
        }

        /// <summary>
        /// Opens <see cref="BorderDialog"/> with <see cref="BorderTypeViewModel.BorderTypeOperations.GaussianBlur"/> parameter
        /// </summary>
        private async void GaussianBlur()
        {
            try
            {
                backupimage = Image;
                var view = new BorderDialog()
                {
                    DataContext = new BorderTypeViewModel(this, BorderTypeViewModel.BorderTypeOperations.GaussianBlur)
                };
                var result = await DialogHost.Show(view, DialogGuid, ClosingEventHandler);
            }
            catch (System.Exception)
            {
                ShowMessage("Image not supported for this operation.");
            }
        }

        /// <summary>
        /// Opens <see cref="BorderDialog"/> with <see cref="BorderTypeViewModel.BorderTypeOperations.Sobel"/> parameter
        /// </summary>
        private async void Sobel()
        {
            try
            {
                backupimage = Image;
                var view = new BorderDialog()
                {
                    DataContext = new BorderTypeViewModel(this, BorderTypeViewModel.BorderTypeOperations.Sobel)
                };
                var result = await DialogHost.Show(view, DialogGuid, ClosingEventHandler);
            }
            catch (System.Exception)
            {
                ShowMessage("Image not supported for this operation.");
            }
        }

        /// <summary>
        /// Opens <see cref="BorderDialog"/> with <see cref="BorderTypeViewModel.BorderTypeOperations.Laplacian"/> parameter
        /// </summary>
        private async void Laplacian()
        {
            try
            {
                backupimage = Image;
                var view = new BorderDialog()
                {
                    DataContext = new BorderTypeViewModel(this, BorderTypeViewModel.BorderTypeOperations.Laplacian)
                };
                var result = await DialogHost.Show(view, DialogGuid, ClosingEventHandler);
            }
            catch (System.Exception)
            {
                ShowMessage("Image not supported for this operation.");
            }           
        }

        /// <summary>
        /// Opens <see cref="CannyDialog"/>
        /// </summary>
        private async void Canny()
        {
            try
            {
                backupimage = Image;
                var view = new CannyDialog()
                {
                    DataContext = new CannyViewModel(this)
                };
                var result = await DialogHost.Show(view, DialogGuid, ClosingEventHandler);
            }
            catch (System.Exception)
            {
                ShowMessage("Image not supported for this operation.");
            }           
        }

        /// <summary>
        /// Opens <see cref="UniversalLinearOperationDialog"/> with <see cref="UniversalLinearOperationViewModel.LinearOperationMasks.Laplacian"/> parameter
        /// </summary>
        private async void Sharpening()
        {
            //ImageModel.Image = LinearSharpening(ImageModel.Image);
            //Image = ImageModel.Image;
            try
            {
                backupimage = Image;
                var view = new UniversalLinearOperationDialog()
                {
                    DataContext = new UniversalLinearOperationViewModel(this, UniversalLinearOperationViewModel.LinearOperationMasks.Laplacian)
                };
                var result = await DialogHost.Show(view, DialogGuid, ClosingEventHandler);
            }
            catch (System.Exception)
            {
                ShowMessage("Image not supported for this operation.");
            }            
        }

        /// <summary>
        /// Opens <see cref="UniversalLinearOperationDialog"/> with <see cref="UniversalLinearOperationViewModel.LinearOperationMasks.Prewitt"/> parameter
        /// </summary>
        private async void SharpeningPrewitt()
        {
            try
            {
                backupimage = Image;
                var view = new UniversalLinearOperationDialog()
                {
                    DataContext = new UniversalLinearOperationViewModel(this, UniversalLinearOperationViewModel.LinearOperationMasks.Prewitt)
                };
                var result = await DialogHost.Show(view, DialogGuid, ClosingEventHandler);
            }
            catch (System.Exception)
            {
                ShowMessage("Image not supported for this operation.");
            }           
        }

        /// <summary>
        /// Opens <see cref="UniversalLinearOperationDialog"/> with <see cref="UniversalLinearOperationViewModel.LinearOperationMasks.Custom"/> parameter
        /// </summary>
        private async void SharpeningCustom()
        {
            try
            {
                backupimage = Image;
                var view = new UniversalLinearOperationDialog()
                {
                    DataContext = new UniversalLinearOperationViewModel(this, UniversalLinearOperationViewModel.LinearOperationMasks.Custom)
                };
                var result = await DialogHost.Show(view, DialogGuid, ClosingEventHandler);
            }
            catch (System.Exception)
            {
                ShowMessage("Image not supported for this operation.");
            }            
        }

        /// <summary>
        /// Opens <see cref="MedianBlurDialog"/>
        /// </summary>
        private async void MedianFilter()
        {
            try
            {
                backupimage = Image;
                var view = new MedianBlurDialog()
                {
                    DataContext = new MedianBlurOperationViewModel(this)
                };
                var result = await DialogHost.Show(view, DialogGuid, ClosingEventHandler);
            }
            catch (System.Exception)
            {
                ShowMessage("Image not supported for this operation.");
            }           
        }

        /// <summary>
        /// Performs NOT operation on image
        /// </summary>
        private void Not()
        {
            try
            {
                ImageModel.Image = ArithmeticOperations(ImageModel.Image, ArithmeticOperationType.Not);
                Image = ImageModel.Image;
            }
            catch (System.Exception)
            {
                ShowMessage("Image not supported for this operation.");
            }           
        }

        /// <summary>
        /// Opens <see cref="MorphologyDialog"/>
        /// </summary>
        private async void Morphology()
        {
            try
            {
                backupimage = Image;
                var view = new MorphologyDialog()
                {
                    DataContext = new MorphologyOperationViewModel(this)
                };
                var result = await DialogHost.Show(view, DialogGuid, ClosingEventHandler);
            }
            catch (System.Exception)
            {
                ShowMessage("Image not supported for this operation.");
            }                       
        }

        /// <summary>
        /// Opens <see cref="OneStepFiltrationDialog"/>
        /// </summary>
        private async void FiltrationOneStep()
        {
            try
            {
                backupimage = Image;
                var view = new OneStepFiltrationDialog()
                {
                    DataContext = new OneStepFiltrationViewModel(this)
                };
                var result = await DialogHost.Show(view, DialogGuid, ClosingEventHandler);
            }
            catch (System.Exception)
            {
                ShowMessage("Image not supported for this operation.");
            }
        }

        /// <summary>
        /// Opens <see cref="TwoStepFiltrationDialog"/>
        /// </summary>
        private async void FiltrationTwoStep()
        {
            try
            {
                backupimage = Image;
                var view = new TwoStepFiltrationDialog()
                {
                    DataContext = new TwoStepFiltrationViewModel(this)
                };
                var result = await DialogHost.Show(view, DialogGuid, ClosingEventHandler);
            }
            catch (System.Exception)
            {
                ShowMessage("Image not supported for this operation.");
            }          
        }

        /// <summary>
        /// Opens <see cref="SkeletonDialog"/>
        /// </summary>
        private async void Skeleton()
        {
            try
            {
                backupimage = Image;
                var view = new SkeletonDialog()
                {
                    DataContext = new SkeletonOperationViewModel(this)
                };
                var result = await DialogHost.Show(view, DialogGuid, ClosingEventHandler);
            }
            catch (System.Exception)
            {
                ShowMessage("Image not supported for this operation.");
            }            
        }

        /// <summary>
        /// Opens <see cref="OtsuThresholdingDialog"/>
        /// </summary>
        private async void OtsuThresholding()
        {
            try
            {
                backupimage = Image;
                var view = new OtsuThresholdingDialog()
                {
                    DataContext = new OtsuThresholdingOperationViewModel(this)
                };
                var result = await DialogHost.Show(view, DialogGuid, ClosingEventHandler);
            }
            catch (System.Exception)
            {
                ShowMessage("Image not supported for this operation.");
            }           
        }

        /// <summary>
        /// Opens <see cref="AdaptiveThresholdingDialog"/>
        /// </summary>
        private async void AdaptiveThresholding()
        {
            try
            {
                backupimage = Image;
                var view = new AdaptiveThresholdingDialog()
                {
                    DataContext = new AdaptiveThresholdingOperationViewModel(this)
                };
                var result = await DialogHost.Show(view, DialogGuid, ClosingEventHandler);
            }
            catch (System.Exception)
            {
                ShowMessage("Image not supported for this operation.");
            }           
        }

        /// <summary>
        /// Performs watershed segmentation on image
        /// </summary>
        private void Watershed()
        {
            try
            {
                Image = EmguOperations.Watershed(Image);
            }
            catch (System.Exception)
            {
                ShowMessage("Image not supported for this operation.");
            }
        }

        /// <summary>
        /// Opens <see cref="FeatureVectorDialog"/>
        /// </summary>
        private async void FeatureVector()
        {
            var view = new FeatureVectorDialog()
            {
                DataContext = new FeatureVectorViewModel(Image)
            };
            await DialogHost.Show(view, DialogGuid);
        }

        private void Svm()
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Opens <see cref="ContrastEnhancementDialog"/>
        /// </summary>
        private async void ContrastEnhancement()
        {
            try
            {
                backupimage = Image;
                BackupModel = ImageModel;
                var view = new ContrastEnhancementDialog()
                {
                    DataContext = new ContrastEnhancementViewModel(this)
                };
                var result = await DialogHost.Show(view, DialogGuid, ClosingEventHandler);
            }
            catch (System.Exception)
            {
                //ShowMessage("Image not supported for this operation.");
            }
        }

        #endregion

        #region Helpers

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
            OnPropertyChanged(nameof(Series));
        }

        /// <summary>
        /// Puts histogram data on the chart
        /// </summary>
        private void PlotHistogram(HistogramModel selecteditem)
        {
            for (int i = 0; i <= 255; i++)
            {
                Labels.Add(i.ToString());
            }
            Series = selecteditem?.PlotData;
        }

        /// <summary>
        /// Intercepts closing dialog event. If user clicked cancel restores image from <see cref="backupimage"/>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        private void ClosingEventHandler(object sender, DialogClosingEventArgs eventArgs)
        {
            if (eventArgs.Parameter is bool parameter &&
                parameter == true) return;
            else Image = backupimage;
        }

        /// <summary>
        /// Opens <see cref="MessageDialog"/> with custom text
        /// </summary>
        /// <param name="message"></param>
        private async void ShowMessage(string message)
        {
            var view = new MessageDialog()
            {
                DataContext = new MessageDialogViewModel(message)
            };
            await DialogHost.Show(view, DialogGuid);

        }

        /// <summary>
        /// Intercepts closing dialog event. If user clicked cancel restores image from <see cref="BackupModel"/>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eventArgs"></param>
        private void ClosingEventHandlerImageModel(object sender, DialogClosingEventArgs eventArgs)
        {
            if (eventArgs.Parameter is bool parameter && parameter == true) return;
            else
            {
                ImageModel = BackupModel;
                Image = BackupModel.Image;
            }
        }

        #endregion

        /// <summary>
        /// Allows changing <see cref="SelectedChannel"/> for channel == <paramref name="channel"/>
        /// </summary>
        /// <param name="channel"></param>
        public void SelectChannel(ChannelModel channel)
        {
            foreach (var item in ImageModel.HistogramData)
            {
                if (item.Channel == channel) SelectedChannel = item;
            }
            OnPropertyChanged(nameof(Series));
        }

        /// <summary>
        /// ToString returning name of an image
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return ImageName;
        }
    }
}
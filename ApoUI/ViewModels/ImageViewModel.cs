using ApoCore;
using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace ApoUI.ViewModels
{
    /// <summary>
    /// View model for image items
    /// </summary>
    public class ImageViewModel : BaseViewModel
    {
        #region Constructor
        /// <summary>
        /// Default Constructor
        /// </summary>
        public ImageViewModel()
        {
            SideMenuVisible = false;
            //CurrentSideMenuContent = SideMenuModel.Histogram;
            IsImageLoaded = false;

            #region Commands
            GetImagePathCommand = new ExecuteCommand(GetImagePath);
            CloseImageCommand = new ExecuteCommand(CloseImage);
            SaveImageCommand = new ExecuteCommand(SaveImage);
            SaveAllImagesCommand = new ExecuteCommand(SaveAllImages);
            DuplicateImageCommand = new ExecuteCommand(DuplicateImage);

            OpenHistogramCommand = new ExecuteCommand(OpenHistogram);
            GenerateHistogramCommand = new ExecuteCommand(GenerateHistogram);
            CloseSideMenuCommand = new ExecuteCommand(CloseSideMenu);

            ConvertToGrayscaleCommand = new ExecuteCommand(ConvertToGrayscale);
            NegationCommand = new ExecuteCommand(Negation);
            ThresholdingCommand = new ExecuteCommand(Thresholding);
            #endregion
        }
        #endregion

        #region Public Properties            
        public ImageModel Image { get; set; }
        public BitmapImage ImageTest
        {
            get => imagetest;
            set
            {
                if (imagetest == value)
                    return;
                imagetest = value;
                OnPropertyChanged();
            }
        }
        //public Bitmap ImageTest
        //{
        //    get => imagetest;
        //    set
        //    {
        //        if (imagetest == value)
        //            return;
        //        imagetest = value;
        //        OnPropertyChanged();
        //    }
        //}
        public string ImagePath
        {
            get => imagepath;
            set
            {
                if (imagepath == value)
                    return;
                imagepath = value;
                //PropertyChanged(this, new PropertyChangedEventArgs(nameof(ImagePath)));
                OnPropertyChanged();
            }
        }
        public bool IsImageLoaded
        {
            get => isimageloaded;
            set
            {
                if (isimageloaded == value)
                    return;
                isimageloaded = value;
                OnPropertyChanged();
            }
        }

        public bool SideMenuVisible
        {
            get => sidemenuvisible;
            set
            {
                if (sidemenuvisible == value)
                    return;
                sidemenuvisible = value;
                OnPropertyChanged();
            }
        }

        // Side menu control related properties, not yet implemented

        //public SideMenuModel CurrentSideMenuContent
        //{
        //    get => currentsidemenucontent;
        //    set
        //    {
        //        if (currentsidemenucontent == value)
        //            return;
        //        currentsidemenucontent = value;
        //        OnPropertyChanged();
        //    }
        //}

        public SeriesCollection Series
        {
            get => series;
            set
            {
                if (series == value)
                    return;
                series = value;
                OnPropertyChanged();
            }
        }
        public List<string> Labels
        {
            get => labels;
            set
            {
                if (labels == value)
                    return;
                labels = value;
                OnPropertyChanged();
            }
        }
        public Func<double, string> Formatter
        {
            get => formatter;
            set
            {
                if (formatter == value)
                    return;
                formatter = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Private properties
        private string imagepath;
        private bool isimageloaded;
        private BitmapImage imagetest;
        //private Bitmap imagetest;

        // Side menu control related properties, not yet implemented
        //private SideMenuModel currentsidemenucontent;

        private bool sidemenuvisible;
        private SeriesCollection series;
        private List<string> labels;
        private Func<double, string> formatter;
        #endregion

        #region Public Commands
        public ICommand GetImagePathCommand { get; set; }
        public ICommand CloseImageCommand { get; set; }
        public ICommand SaveImageCommand { get; set; }
        public ICommand SaveAllImagesCommand { get; set; }
        public ICommand DuplicateImageCommand { get; set; }

        public ICommand OpenHistogramCommand { get; set; }
        public ICommand GenerateHistogramCommand { get; set; }
        public ICommand CloseSideMenuCommand { get; set; }
        public ICommand ConvertToGrayscaleCommand { get; set; }
        public ICommand NegationCommand { get; set; }
        public ICommand ThresholdingCommand { get; set; }
        #endregion

        #region Functions
        /// <summary>
        /// Open file dialog
        /// </summary>
        private void GetImagePath()
        {
            OpenFileDialog openFile = new();
            openFile.Filter = "Image files (*.jpg;*jpeg;*.png;*.bmp)|*.jpg;*.jpeg;*.png;*.bmp";
            string openedFileName;
            if (openFile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                openedFileName = openFile.FileName;
                this.ImagePath = openedFileName;
                this.Image = new ImageModel(ImagePath);
                Bitmap bitmap = new(this.ImagePath);
                this.ImageTest = new BitmapImage(new Uri(this.ImagePath));
                IsImageLoaded = true;
                GenerateHistogram();
            }
        }

        /// <summary>
        /// Closes image
        /// </summary>
        private void CloseImage()
        {
            this.ImagePath = "";
            this.ImageTest = null;
            SideMenuVisible = false;
            IsImageLoaded = false;
        }

        private void SaveImage()
        {
            // TODO: SaveImage implementation
            //Image.Image.Save(imagepath);
            MessageBox.Show("Not implemented yet :(");
        }

        private void SaveAllImages()
        {
            // TODO: SaveAllImages implementation
            MessageBox.Show("Not implemented yet :(");
        }

        private void DuplicateImage()
        {
            // TODO: DuplicateImage implementation
            MessageBox.Show("Not implemented yet :(");
        }

        /// <summary>
        /// Opens side panel 
        /// </summary>
        private void OpenHistogram()
        {
            //TODO: mutiple side panel tabs, command to open each tab

            //CurrentSideMenuContent = SideMenuModel.Histogram;
            SideMenuVisible ^= true;
        }

        /// <summary>
        /// Closes side menu
        /// </summary>
        private void CloseSideMenu()
        {
            SideMenuVisible = false;
        }

        /// <summary>
        /// Puts histogram data on the chart
        /// </summary>
        private void GenerateHistogram()
        {
            // TODO: When image is loaded histogram data are calculated twice, fix this
            Image.HistogramData.GetHistogramData();
            Series = new SeriesCollection();
            Labels = new List<string>();
            ChartValues<int> cv = new ChartValues<int>();
            foreach (var item in Image.HistogramData.ChannelRGB)
            {
                cv.Add(item.Value);
                Labels.Add(item.Key.ToString());
            }
            ColumnSeries cs = new ColumnSeries()
            {
                Values = cv
            };
            cs.ColumnPadding = 0;
            Series.Add(cs);
            Formatter = value => value.ToString("N");
        }

        public BitmapImage Convert(Bitmap src)
        {
            // TODO: Let viewmodel use Bitmap, create converter to BitmapImage for the View
            MemoryStream ms = new MemoryStream();
            ((System.Drawing.Bitmap)src).Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            BitmapImage image = new BitmapImage();
            image.BeginInit();
            ms.Seek(0, SeekOrigin.Begin);
            image.StreamSource = ms;
            image.EndInit();
            return image;
        }

        #region Image operations

        /// <summary>
        /// Converts colorfull image to grayscale
        /// </summary>
        private void ConvertToGrayscale()
        {
            this.ImageTest = Convert(this.Image.Image.ToGrayScale());
            GenerateHistogram();
        }

        /// <summary>
        /// Performs negation operation on the image
        /// </summary>
        private void Negation()
        {
            this.ImageTest = Convert(this.Image.Image.Negation());
            GenerateHistogram();
        }

        /// <summary>
        /// Performs thresholding operation on the image
        /// </summary>
        private void Thresholding()
        {
            // TODO: Let user choose threshold value
            this.ImageTest = Convert(this.Image.Image.Thresholding(127));
            GenerateHistogram();
        }
        #endregion

        #endregion
    }
}

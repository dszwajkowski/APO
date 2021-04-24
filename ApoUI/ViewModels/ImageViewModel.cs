using ApoCore;
using ApoUI.Views;
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
            //CurrentSideMenuContent = SideMenuModel.Histogram;
        }

        #endregion

        #region Public Properties   
        
        public ImageModel ImageModel 
        {
            get => imagemodel;         
            set
            {
                if (imagemodel == value)
                    return;
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
                UpdateHistogram();
                OnPropertyChanged();
            }
        }
        public string ImagePath
        {
            get => imagepath;
            set
            {
                if (imagepath == value)
                    return;
                imagepath = value;;
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
        public HistogramModel SelectedItem
        {
            get => selecteditem;
            set
            {
                if (selecteditem == value)
                    return;
                selecteditem = value;
                PlotHistogram(selecteditem);
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

        public List<string> Labels = new List<string>();
        public int[] Series
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

        public int Threshold
        {
            get => threshold;
            set
            {
                if (threshold == value)
                    return;
                threshold = value;
                Thresholding();
                OnPropertyChanged();
            }
        }
        public int ThresholdMax
        {
            get => thresholdmax;
            set
            {
                if (thresholdmax == value)
                    return;
                thresholdmax = value;
                Thresholding();
                OnPropertyChanged();
            }
        }
        public bool KeepGrayLevels
        {
            get => keepgraylevels;
            set
            {
                if (keepgraylevels == value)
                    return;
                keepgraylevels = value;
                OnPropertyChanged();
            }
        }
        #endregion

        #region Private properties

        private ImageModel imagemodel;
        private Bitmap image;
        private Bitmap backupimage;
        private string imagepath;
        private bool isimageloaded = false;
               
        private HistogramModel selecteditem;
        private bool sidemenuvisible = false;
        // Side menu control related properties, not yet implemented
        //private SideMenuModel currentsidemenucontent;

        private int[] series;
        private int threshold = 127;
        private int thresholdmax = 255;
        private bool keepgraylevels = false;
        #endregion

        #region Public Commands
        public ICommand GetImagePathCommand => new RelayCommand(GetImagePath);
        public ICommand CloseImageCommand => new RelayCommand(CloseImage);
        public ICommand SaveImageCommand => new RelayCommand(SaveImage);
        public ICommand SaveImageAsCommand => new RelayCommand(SaveImageAs);
        public ICommand SaveAllImagesCommand => new RelayCommand(SaveAllImages);
        public ICommand DuplicateImageCommand => new RelayCommand(DuplicateImage);

        public ICommand OpenHistogramCommand => new RelayCommand(OpenHistogram);
        public ICommand CloseSideMenuCommand => new RelayCommand(CloseSideMenu);
        public ICommand ConvertToGrayscaleCommand => new RelayCommand(ConvertToGrayscale);
        public ICommand NegationCommand => new RelayCommand(Negation);
        public ICommand ThresholdingCommand => new RelayCommand(Thresholding);
        public ICommand PosterizeCommand => new RelayCommand(Posterize);
        public ICommand EqualizeCommand => new RelayCommand(Equalize);
        public ICommand StretchCommand => new RelayCommand(Stretch);
        public ICommand BlurCommand => new RelayCommand(Blur);
        public ICommand GaussianBlurCommand => new RelayCommand(GaussianBlur);

        public ICommand TestCommand => new RelayCommand(Test);
        #endregion
              
        #region Command methods - image operations

        /// <summary>
        /// Converts colorfull image to grayscale
        /// </summary>
        private void ConvertToGrayscale()
        {
            ImageModel.ToGrayScaleLUT();
            Image = ImageModel.Image;
        }

        /// <summary>
        /// Performs negation operation on the image
        /// </summary>
        private void Negation()
        {
            ImageModel.Negation();
            Image = ImageModel.Image;
        }

        /// <summary>
        /// Performs thresholding operation on the image
        /// </summary>
        private void Thresholding()
        {
            Image = backupimage;
            //if (KeepGrayLevels)
            //{
            //    ImageModel.ThresholdingKeepGrayLevels(Threshold, ThresholdMax);
            //    Image = ImageModel.Image;
            //}
            //else
            //{
            //    ImageModel.Thresholding(Threshold);
            //    Image = ImageModel.Image;
            //}
            ImageModel.Thresholding(127);
            Image = ImageModel.Image;
        }

        private void Posterize()
        {
            ImageModel.Posterize();
            Image = ImageModel.Image;
        }

        private void Equalize()
        {
            MessageBox.Show("Not implemented yet :(");
        }

        private void Stretch()
        {
            ImageModel.HistogramStretch();
            Image = ImageModel.Image;
        }

        private void Blur()
        {
            ImageModel.Blur();
            Image = ImageModel.Image;
        }

        private void GaussianBlur()
        {
            ImageModel.GaussianBlur();
            Image = ImageModel.Image;
        }

        private void Test()
        {
            //var view = new ThresholdingDialog()
            //{ 
            //    DataContext = this 
            //};
            //view.Show();
        }

        #endregion

        #region Command methods - UI

        /// <summary>
        /// Opens open file dialog
        /// </summary>
        private void GetImagePath()
        {
            OpenFileDialog openFile = new();
            openFile.Filter = "Image files (*.jpg;*jpeg;*.png;*.bmp)|*.jpg;*.jpeg;*.png;*.bmp";
            if (openFile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                ImageModel = new ImageModel(openFile.FileName);             
                Image = ImageModel.Image;
                //var db = new DirectBitmap(ImageModel.Image.Width, ImageModel.Image.Height);
                //using (var g = Graphics.FromImage(ImageModel.Image))
                //{
                //    g.DrawImage(this.Image, 0, 0);
                //    g.DrawImage(backupimage, 0, 0);
                //}
                backupimage = Image;
                IsImageLoaded = true;
            }
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
            Series = selecteditem.PlotData;
        }

        /// <summary>
        /// Closes image
        /// </summary>
        private void CloseImage()
        {
            ImageModel.Image.Dispose();
            this.Image.Dispose();
            ImageModel.Image = null;
            this.Image = null;
            SideMenuVisible = false;
            IsImageLoaded = false;
        }

        private void SaveImage()
        {
            if (System.IO.File.Exists(ImageModel.ImagePath))
                System.IO.File.Delete(ImageModel.ImagePath);
            Image.Save(ImageModel.ImagePath);
        }

        private void SaveImageAs()
        {
            SaveFileDialog saveFile = new();           
            saveFile.Filter = "JPG (*.jpg)|*.jpg|PNG (*.png)|*.png|BMP (*.bmp)|*.bmp";
            if (saveFile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Image.Save(saveFile.FileName);
            }
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
        /// Updates histogram after performing operation on image
        /// </summary>
        private void UpdateHistogram()
        {
            ImageModel.UpdateHistogramData();
            foreach (var item in ImageModel.HistogramData)
            {
                if (item.Channel == SelectedItem.Channel)
                    SelectedItem = item;
            }
        }

        #endregion
    }
}

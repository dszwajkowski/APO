using ApoCore;
using Emgu.CV.CvEnum;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Windows.Input;

namespace ApoUI
{
    /// <summary>
    /// View model for thresholding dialog
    /// </summary>
    public class ThresholdingOperationViewModel
    {
        #region Constructor
        public ThresholdingOperationViewModel(ImageViewModel window)
        {
            this.Parent = window;
            ImageModel = Parent.ImageModel;
            backupimage = Parent.Image;
        }

        #endregion

        #region Public Properties   

        public ImageViewModel Parent;
        public Bitmap Image
        {
            get => image;
            set
            {
                image = value;
                Parent.Image = Image;
            }
        }
        public ImageModel ImageModel
        {
            get => imagemodel;
            set
            {
                if (imagemodel == value)
                    return;
                imagemodel = value;
                Parent.Image = Image;
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
                Thresholding();
            }
        }
        #endregion

        #region Private properties

        private Bitmap image;
        private Bitmap backupimage;
        private ImageModel imagemodel;

        private int threshold = 127;
        private int thresholdmax = 255;
        private bool keepgraylevels = false;

        #endregion


        public ICommand ThresholdingCommand => new RelayCommand(Thresholding);
        public ICommand CancelCommand => new RelayCommand(Cancel);

        private void Thresholding()
        {
            Image = backupimage;
            Image = ImageOperations.Thresholding(ImageModel, Threshold);
        }

        private void Cancel()
        {
            Parent.Image = backupimage;
        }
    }
}

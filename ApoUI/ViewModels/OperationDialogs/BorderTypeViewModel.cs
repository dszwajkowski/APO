using ApoCore;
using Emgu.CV.CvEnum;
using System;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Windows.Input;

namespace ApoUI
{
    
    /// <summary>
    /// View model for operations with border type parameter
    /// </summary>
    public class BorderTypeViewModel
    {
        /// <summary>
        /// Operation types
        /// </summary>
        public enum BorderTypeOperations
        {
            Blur,
            GaussianBlur,
            Sobel,
            Laplacian,
        }

        #region Constructor

        /// <summary>
        /// Construcotr
        /// </summary>
        /// <param name="window"></param>
        /// <param name="borderTypeOperation"></param>
        public BorderTypeViewModel(ImageViewModel window, BorderTypeOperations borderTypeOperation)
        {
            this.Parent = window;
            _UneditedImage = Parent.Image;
            switch (borderTypeOperation)
            {
                case BorderTypeOperations.Blur:
                    _Action = new Action(Blur);
                    break;
                case BorderTypeOperations.GaussianBlur:
                    _Action = new Action(GaussianBlur);
                    break;
                case BorderTypeOperations.Sobel:
                    _Action = new Action(Sobel);
                    break;
                case BorderTypeOperations.Laplacian:
                    _Action = new Action(Laplacian);
                    break;
                default:
                    break;
            }
            OperationCommand = new RelayCommand(_Action);
            BorderTypeList = new ObservableCollection<BorderType>()
            {
                BorderType.Isolated,
                BorderType.Reflect,
                BorderType.Replicate,
            };
            OperationCommand.Execute(_Action);
        }

        #endregion

        #region Public Properties   

        // viewmodel that opened dialog that uses this viewmodel
        public ImageViewModel Parent;

        // Currently selected border type
        public BorderType BorderType
        {
            get => _BorderType;
            set
            {
                if (_BorderType == value)
                    return;
                _BorderType = value;
                OperationCommand.Execute(_Action);
            }
        }
        // List of all border types
        public ObservableCollection<BorderType> BorderTypeList { get; set; }

        #endregion

        #region Private fields

        // Operation to perform
        private Action _Action;
        private BorderType _BorderType = BorderType.Isolated;
        private Bitmap _UneditedImage;

        #endregion

        #region Commands

        // Operation to perform
        public ICommand OperationCommand;

        #endregion

        #region Methods

        /// <summary>
        /// Performs blur operation
        /// </summary>
        private void Blur()
        {
            Parent.Image = _UneditedImage;
            Parent.Image = EmguOperations.Blur(Parent.Image, BorderType);
        }

        /// <summary>
        /// Performs gaussian blur operation
        /// </summary>
        private void GaussianBlur()
        {
            Parent.Image = _UneditedImage;
            Parent.Image = EmguOperations.GaussianBlur(Parent.Image, BorderType);
        }

        /// <summary>
        /// Performs edge detection operation using Sobel
        /// </summary>
        private void Sobel()
        {
            Parent.Image = _UneditedImage;
            Parent.Image = EmguOperations.Sobel(Parent.Image, BorderType);
        }

        /// <summary>
        /// Performs edge detection operation using Laplacian
        /// </summary>
        private void Laplacian()
        {
            Parent.Image = _UneditedImage;
            Parent.Image = EmguOperations.Laplacian(Parent.Image, BorderType);
        }

        #endregion

    }
}

using ApoCore;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using static ApoCore.EmguOperations;

namespace ApoUI
{
    
    /// <summary>
    /// View model for arithmetic operation dialog
    /// </summary>
    public class ArithmeticOperationViewModel : BaseViewModel
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="firstImage"></param>
        /// <param name="imageList"></param>
        public ArithmeticOperationViewModel(ImageViewModel firstImage, ObservableCollection<ImageViewModel> imageList)
        {
            this.Parent = firstImage;
            _UneditedImage = Parent.Image;
            foreach (var item in imageList)
            { 
                if (item != firstImage) ImageList.Add(item);
            }
            SecondImage = ImageList.FirstOrDefault();
            OperationsList = new ObservableCollection<ArithmeticOperationType>()
            {
                ArithmeticOperationType.Add,
                ArithmeticOperationType.And,
                ArithmeticOperationType.Or,
                ArithmeticOperationType.Subtract,
                ArithmeticOperationType.Xor,
                ArithmeticOperationType.Blend,
            };            
        }

        #endregion

        #region Public Properties   
        // viewmodel that opened dialog that uses this viewmodel
        public ImageViewModel Parent;
        // second image used in arithmetic operation
        public ImageViewModel SecondImage
        {
            get => _SecondImage;
            set
            {
                if (_SecondImage == value) return;
                _SecondImage = value;
                OnPropertyChanged();
                ArithmeticOperation();
            }
        }
        // list of all currently opened images
        public ObservableCollection<ImageViewModel> ImageList { get; set; } = new ObservableCollection<ImageViewModel>();
        // selected operation
        public ArithmeticOperationType SelectedOperation
        {
            get => _SelectedOperation;
            set
            {
                if (_SelectedOperation == value) return;
                _SelectedOperation = value;
                ArithmeticOperation();
            }
        }
        // list of all supported operations
        public ObservableCollection<ArithmeticOperationType> OperationsList { get; set; }

        #endregion

        #region Private fields

        private ImageViewModel _SecondImage;
        private ArithmeticOperationType _SelectedOperation = ArithmeticOperationType.Add;
        private Bitmap _UneditedImage;

        #endregion

        #region Methods

        private void ArithmeticOperation()
        {           
            Parent.Image = _UneditedImage;
            Parent.Image = EmguOperations.ArithmeticOperations(Parent.Image, SelectedOperation, SecondImage.Image);
        }

        #endregion

    }
}

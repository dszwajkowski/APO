using ApoCore;
using Emgu.CV.CvEnum;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Windows.Input;

namespace ApoUI
{
    /// <summary>
    /// View model for image items
    /// </summary>
    public class SkeletonOperationViewModel : BaseViewModel
    {
        #region Constructor

        public SkeletonOperationViewModel(ImageViewModel window)
        {
            this.Parent = window;
            backupimage = Parent.Image;

            ElementShapeList = new ObservableCollection<ElementShape>()
            {
                ElementShape.Cross,
                ElementShape.Rectangle,
            };
            BorderTypeList = new ObservableCollection<BorderType>()
            {
                BorderType.Isolated,
                BorderType.Reflect,
                BorderType.Replicate,
            };
            IterationsList = new ObservableCollection<int>()
            {
                1, 2, 3, 4, 5, 6, 7, 8, 
            };
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

        public ElementShape ElementShape
        {
            get => elementshape;
            set
            {
                if (elementshape == value)
                    return;
                elementshape = value;
                Skeleton();
            }
        }
        public ObservableCollection<ElementShape> ElementShapeList { get; set; } 

        public BorderType BorderType
        {
            get => bordertype;
            set
            {
                if (bordertype == value)
                    return;
                bordertype = value;
                Skeleton();
            }
        }
        public ObservableCollection<BorderType> BorderTypeList { get; set; }

        public int Iterations
        {
            get => iterations;
            set
            {
                if (iterations == value)
                    return;
                iterations = value;
                Skeleton();
            }
        }
        public ObservableCollection<int> IterationsList { get; set; } 

        #endregion

        #region Private properties

        private Bitmap image;
        private Bitmap backupimage;

        private int iterations = 3;
        private ElementShape elementshape = ElementShape.Cross;
        private BorderType bordertype = BorderType.Isolated;

        #endregion


        public ICommand SkeletonCommand => new RelayCommand(Skeleton);
        public ICommand CancelCommand => new RelayCommand(Cancel);

        private void Skeleton()
        {
            Image = backupimage;
            Image = EmguOperations.Skeleton(Image, ElementShape, Iterations, BorderType);
        }

        private void Cancel()
        {
            Parent.Image = backupimage;
        }
    }
}

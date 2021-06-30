using ApoCore;
using Emgu.CV.CvEnum;
using System.Collections.ObjectModel;

namespace ApoUI
{
    /// <summary>
    /// View model for skeleton operation dialog
    /// </summary>
    public class SkeletonOperationViewModel : BaseViewModel
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="window"></param>
        public SkeletonOperationViewModel(ImageViewModel window)
        {
            this.Parent = window;

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
            Skeleton();
        }

        #endregion

        #region Public Properties   

        // viewmodel that opened dialog that uses this viewmodel
        public ImageViewModel Parent;

        // currently selected kernel shape
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
        // list of all supported kernel shapes
        public ObservableCollection<ElementShape> ElementShapeList { get; set; } 
        // currently selected border
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
        // list of all supported border types
        public ObservableCollection<BorderType> BorderTypeList { get; set; }
        // currently selected iteration amount
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
        // list of all possible iteration amounts
        public ObservableCollection<int> IterationsList { get; set; } 

        #endregion

        #region Private properties

        private int iterations = 3;
        private ElementShape elementshape = ElementShape.Cross;
        private BorderType bordertype = BorderType.Isolated;

        #endregion

        #region Methods

        /// <summary>
        /// Performs skeleton operation
        /// </summary>
        private void Skeleton()
        {
            Parent.Image = Parent.backupimage;
            Parent.Image = EmguOperations.Skeleton(Parent.Image, ElementShape, Iterations, BorderType);
        }

        #endregion
    }
}

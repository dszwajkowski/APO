using ApoCore;
using Emgu.CV.CvEnum;
using System.Collections.ObjectModel;

namespace ApoUI
{
    /// <summary>
    /// View model for morphology operation dialog
    /// </summary>
    public class MorphologyOperationViewModel
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="window"></param>
        public MorphologyOperationViewModel(ImageViewModel window)
        {
            this.Parent = window;

            MorphOpList = new ObservableCollection<MorphOp>()
            {
                MorphOp.Erode,
                MorphOp.Dilate,
                MorphOp.Open,
                MorphOp.Close,
            };
            ElementShapeList = new ObservableCollection<ElementShape>()
            {
                ElementShape.Cross,
                ElementShape.Rectangle,
            };
            //KernelShapeList = new ObservableCollection<KernelShape>()
            //{
            //    KernelShape.Rectangle,
            //    KernelShape.Cross,
            //    KernelShape.Square,
            //    KernelShape.Rhombus,
            //};
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
            Morphology();
        }

        #endregion

        #region Public Properties   

        // viewmodel that opened dialog that uses this viewmodel
        public ImageViewModel Parent;

        // currently selected operation
        public MorphOp MorphOp
        {
            get => morphop;
            set
            {
                if (morphop == value)
                    return;
                morphop = value;
                Morphology();
            }
        }
        // list of all supported operations
        public ObservableCollection<MorphOp> MorphOpList { get; set; }
        // currently selected kernel shape
        public ElementShape ElementShape
        {
            get => elementshape;
            set
            {
                if (elementshape == value)
                    return;
                elementshape = value;
                Morphology();
            }
        }
        // list of all suported kernel shapes
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
                Morphology();
            }
        }
        // list of all supported border types
        public ObservableCollection<BorderType> BorderTypeList { get; set; }

        // currently selected amount of iterations
        public int Iterations
        {
            get => iterations;
            set
            {
                if (iterations == value)
                    return;
                iterations = value;
                Morphology();
            }
        }
        // list of supported iteration amounts
        public ObservableCollection<int> IterationsList { get; set; } 

        #endregion

        #region Private fields

        private int iterations = 3;
        private MorphOp morphop = MorphOp.Erode;
        private ElementShape elementshape = ElementShape.Cross;
        private BorderType bordertype = BorderType.Isolated;

        #endregion

        #region Methods

        private void Morphology()
        {
            Parent.Image = Parent.backupimage;
            Parent.Image = EmguOperations.MorphologyOperations(Parent.Image, MorphOp, ElementShape, Iterations, BorderType);
        }

        #endregion
    }
}

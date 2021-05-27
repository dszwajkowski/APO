using ApoCore;
using Emgu.CV.CvEnum;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Windows.Input;

namespace ApoUI
{
    /// <summary>
    /// View model for morphology operation dialog
    /// </summary>
    public class MorphologyOperationViewModel
    {
        #region Constructor
        public MorphologyOperationViewModel(ImageViewModel window)
        {
            this.Parent = window;
            _UneditedImage = Parent.Image;

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
        public ObservableCollection<MorphOp> MorphOpList { get; set; }

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
        public ObservableCollection<ElementShape> ElementShapeList { get; set; } 

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
        public ObservableCollection<BorderType> BorderTypeList { get; set; }

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
        public ObservableCollection<int> IterationsList { get; set; } 

        #endregion

        #region Private fields

        private Bitmap _UneditedImage;
        private int iterations = 3;
        private MorphOp morphop = MorphOp.Erode;
        private ElementShape elementshape = ElementShape.Cross;
        private BorderType bordertype = BorderType.Isolated;

        #endregion

        public ICommand MorphologyCommand => new RelayCommand(Morphology);

        private void Morphology()
        {
            Parent.Image = _UneditedImage;
            Parent.Image = EmguOperations.MorphologyOperations(Parent.Image, MorphOp, ElementShape, Iterations, BorderType);
        }
    }
}

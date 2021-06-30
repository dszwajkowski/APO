using ApoCore;
using Emgu.CV;
using Emgu.CV.Util;
using System;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Text;

namespace ApoUI
{
    /// <summary>
    /// View model for feature vector dialog
    /// </summary>
    public class FeatureVectorViewModel : BaseViewModel
    {
        /// <summary>
        /// Adds index for each contour that is displayed on combobox instead of class name
        /// </summary>
        public class VectorOfPointWithToString
        {
            public VectorOfPoint VectorOfPoint { get; set; }
            private int index = 0;
            public VectorOfPointWithToString(VectorOfPoint vop, int index)
            {
                VectorOfPoint = vop;
                this.index = index;
            }

            public override string ToString()
            {
                return index.ToString();
            }
        }

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="image"></param>
        public FeatureVectorViewModel(Bitmap image)
        {
            _Image = image;
            _FeatureVector = EmguOperations.FeatureVector(image);
            //RetrTypeList = new ObservableCollection<RetrType>()
            //{
            //    RetrType.Ccomp,
            //    RetrType.Tree,
            //    RetrType.External,
            //    RetrType.Floodfill,
            //    RetrType.List
            //};
            //ApproxMethodList = new ObservableCollection<ChainApproxMethod>()
            //{
            //    ChainApproxMethod.ChainApproxNone,
            //    ChainApproxMethod.ChainApproxSimple,
            //    ChainApproxMethod.ChainApproxTc89Kcos,
            //    ChainApproxMethod.ChainApproxTc89L1,
            //    ChainApproxMethod.ChainCode,
            //    ChainApproxMethod.LinkRuns
            //};
            for (int i = 0; i < _FeatureVector.contours.Size; i++)
            {
                ContoursList.Add(new VectorOfPointWithToString(_FeatureVector.contours[i], i));
            }
            _Contour = ContoursList[1];
            CalculateVector();
            BuildMomentsString();
        }

        #endregion

        #region Public Properties   

        //public RetrType RetrType
        //{
        //    get => _RetrType;
        //    set
        //    {
        //        if (_RetrType == value) return;
        //        _RetrType = value;
        //        OnPropertyChanged();
        //        CalculateVector();
        //    }
        //}
        //public ObservableCollection<RetrType> RetrTypeList { get; set; }

        //public ChainApproxMethod ApproxMethod
        //{
        //    get => _ApproxMethod;
        //    set
        //    {
        //        if (_ApproxMethod == value) return;
        //        _ApproxMethod = value;
        //        OnPropertyChanged();
        //        CalculateVector();
        //    }
        //}
        //public ObservableCollection<ChainApproxMethod> ApproxMethodList { get; set; }

        // currently selected contour
        public VectorOfPointWithToString Contour
        {
            get => _Contour;
            set
            {
                if (_Contour == value) return;
                _Contour = value;
                OnPropertyChanged();
                CalculateVector();
            }
        }
        // list of all contours found on image
        public ObservableCollection<VectorOfPointWithToString> ContoursList { get; set; } = new ObservableCollection<VectorOfPointWithToString>();

        // string with moments
        public string Moments
        {
            get => _Moments;
            set
            {
                if (_Moments == value) return;
                _Moments = value;
                OnPropertyChanged();
            }         
        }
        // area of selected contour
        public double Area
        {
            get => _Area;
            set
            {
                if (_Area == value) return;
                _Area = value;
                OnPropertyChanged();
            }
        }
        // perimeter of selected contour
        public double Perimeter
        {
            get => _Perimeter;
            set
            {
                if (_Perimeter == value) return;
                _Perimeter = value;
                OnPropertyChanged();
            }
        }
        // aspect ratio of selected contour
        public double AspectRatio
        {
            get => _AspectRatio;
            set
            {
                if (_AspectRatio == value) return;
                _AspectRatio = value;
                OnPropertyChanged();
            }
        }
        // extend of selected contour
        public double Extend
        {
            get => _Extend;
            set
            {
                if (_Extend == value) return;
                _Extend = value;
                OnPropertyChanged();
            }
        }
        // solidity of selected contour
        public double Solidity
        {
            get => _Solidity;
            set
            {
                if (_Solidity == value) return;
                _Solidity = value;
                OnPropertyChanged();
            }
        }
        // equivalent diameter of selected contour
        public double EquivalentDiameter
        {
            get => _EquivalentDiameter;
            set
            {
                if (_EquivalentDiameter == value) return;
                _EquivalentDiameter = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Private fields

        private Bitmap _Image;
        private (Moments moments, VectorOfVectorOfPoint contours) _FeatureVector;
        //private RetrType _RetrType = RetrType.Ccomp;
        //private ChainApproxMethod _ApproxMethod = ChainApproxMethod.ChainApproxNone;
        private VectorOfPointWithToString _Contour;
        private string _Moments;
        private double _Area;
        private double _Perimeter;
        private double _AspectRatio;
        private double _Extend;
        private double _Solidity;
        private double _EquivalentDiameter;

        #endregion

        #region Methods
        
        /// <summary>
        /// Calculates feature vector
        /// </summary>
        private void CalculateVector()
        {
            // calculate area and perimeter
            Area = CvInvoke.ContourArea(Contour.VectorOfPoint);
            Perimeter = CvInvoke.ArcLength(Contour.VectorOfPoint, true);
            // calculate aspect ratio
            Rectangle rectangle = CvInvoke.BoundingRectangle(Contour.VectorOfPoint);
            AspectRatio = (double)rectangle.Width / rectangle.Height;
            // calculate extend
            double rectangleArea = rectangle.Width * rectangle.Height;
            Extend = Area / rectangleArea;
            // calculate solidity
            VectorOfPoint hull = new VectorOfPoint();
            CvInvoke.ConvexHull(Contour.VectorOfPoint, hull);
            double hull_area = CvInvoke.ContourArea(hull);
            Solidity = Area / hull_area;
            // calculate equivalent diameter
            EquivalentDiameter = Math.Sqrt(4 * Area / Math.PI);
        }

        /// <summary>
        /// Creates string containing moments
        /// </summary>
        private void BuildMomentsString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("M(0,0): ").Append(_FeatureVector.moments.M00).Append(Environment.NewLine).Append("M(0,1): ").Append(_FeatureVector.moments.M01)
                .Append(Environment.NewLine).Append("M(1,0): ").Append(_FeatureVector.moments.M10).Append(Environment.NewLine).Append("M(1,1): ")
                .Append(_FeatureVector.moments.M11);
            Moments = sb.ToString();
        }

        #endregion
    }
}

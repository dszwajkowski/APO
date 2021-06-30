using ApoCore;
using Emgu.CV.CvEnum;
using System;
using System.Collections.ObjectModel;
using System.Drawing;

namespace ApoUI
{
    /// <summary>
    /// View model for linear sharpening operation dialog
    /// </summary>
    public class UniversalLinearOperationViewModel
    {
        /// <summary>
        /// Mask types
        /// </summary>
        public enum LinearOperationMasks
        {
            Laplacian,
            Prewitt,
            Custom
        }

        public class LinearOperationMask : BaseViewModel
        {
            #region Constructors
            public LinearOperationMask(int index1, int index2, int index3, int index4, int index5, int index6,
                int index7, int index8, int index9)
            {
                i1 = index1;
                i2 = index2;
                i3 = index3;
                i4 = index4;
                i5 = index5;
                i6 = index6;
                i7 = index7;
                i8 = index8;
                i9 = index9;
            }
            public LinearOperationMask(UniversalLinearOperationViewModel viewModel, int index1, int index2, int index3, int index4, int index5, int index6, 
                int index7, int index8, int index9)
            {
                this.Parent = viewModel;
                i1 = index1;
                i2 = index2;
                i3 = index3;
                i4 = index4;
                i5 = index5;
                i6 = index6;
                i7 = index7;
                i8 = index8;
                i9 = index9;
            }

            #endregion

            #region Public properties

            public UniversalLinearOperationViewModel Parent;
            public int i1
            {
                get => _i1;
                set
                {
                    if (_i1 == value) return;
                    _i1 = value;
                    OnPropertyChanged();
                    if (Parent != null) Parent.LinearOperation();
                }
            }
            public int i2
            {
                get => _i2;
                set
                {
                    if (_i2 == value) return;
                    _i2 = value;
                    OnPropertyChanged();
                    if (Parent != null) Parent.LinearOperation();
                }
            }
            public int i3
            {
                get => _i3;
                set
                {
                    if (_i3 == value) return;
                    _i3 = value;
                    OnPropertyChanged();
                    if (Parent != null) Parent.LinearOperation();
                }
            }
            public int i4
            {
                get => _i4;
                set
                {
                    if (_i4 == value) return;
                    _i4 = value;
                    OnPropertyChanged();
                    if (Parent != null) Parent.LinearOperation();
                }
            }
            public int i5
            {
                get => _i5;
                set
                {
                    if (_i5 == value) return;
                    _i5 = value;
                    OnPropertyChanged();
                    if (Parent != null) Parent.LinearOperation();
                }
            }
            public int i6
            {
                get => _i6;
                set
                {
                    if (_i6 == value) return;
                    _i6 = value;
                    OnPropertyChanged();
                    if (Parent != null) Parent.LinearOperation();
                }
            }
            public int i7
            {
                get => _i7;
                set
                {
                    if (_i7 == value) return;
                    _i7 = value;
                    OnPropertyChanged();
                    if (Parent != null) Parent.LinearOperation();
                }
            }
            public int i8
            {
                get => _i8;
                set
                {
                    if (_i8 == value) return;
                    _i8 = value;
                    OnPropertyChanged();
                    if (Parent != null) Parent.LinearOperation();
                }
            }
            public int i9
            {
                get => _i9;
                set
                {
                    if (_i9 == value) return;
                    _i9 = value;
                    OnPropertyChanged();
                    if (Parent != null) Parent.LinearOperation();
                }
            }

            #endregion

            #region Private fields

            private int _i1, _i2, _i3, _i4, _i5, _i6, _i7, _i8, _i9 = 0;

            #endregion

            #region Methods

            public override string ToString()
            {
                return $"{i1}\t{i2}\t{i3}{Environment.NewLine}{i4}\t{i5}\t{i6}{Environment.NewLine}{i7}\t{i8}\t{i9}";
            }

            #endregion
        }

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="window"></param>
        /// <param name="linearOperationMask"></param>
        public UniversalLinearOperationViewModel(ImageViewModel window, LinearOperationMasks linearOperationMask)
        {
            this.Parent = window;
            _UneditedImage = Parent.Image;
            switch (linearOperationMask)
            {
                case LinearOperationMasks.Laplacian:
                    MaskList = new ObservableCollection<LinearOperationMask>()
                    {
                        new LinearOperationMask(0, -1, 0, -1, 5, -1, 0, -1, 0),
                        new LinearOperationMask(-1, -1, -1, -1, 9, -1, -1, -1, -1),
                        new LinearOperationMask(1, -2, 1, -2, 5, -2, 1, -2, 1)
                    };
                    Mask = MaskList[0];
                    break;
                case LinearOperationMasks.Prewitt:
                    MaskList = new ObservableCollection<LinearOperationMask>()
                    {
                        // N
                        new LinearOperationMask(1, 1, 1, 1, -2, 1, -1, -1, -1),
                        // E
                        new LinearOperationMask(-1, 1, 1, -1, -2, 1, -1, 1, 1),
                        // S
                        new LinearOperationMask(-1, -1, -1, 1, -2, 1, 1, 1, 1),
                        // W 
                        new LinearOperationMask(1, 1, -1, 1, -2, -1, 1, 1, -1),
                        // NW
                        new LinearOperationMask(1, 1, 0, 1, -2, -1, 0, -1, -1),
                        // NE
                        new LinearOperationMask(0, 1, 1, -1, -2, 1, -1, -1, 0),
                        // SW
                        new LinearOperationMask(0, -1, -1, 1, -2, -1, 1, 1, 0),
                        // SE
                        new LinearOperationMask(-1, -1, 0, -1, -2, 1, 0, 1, 1),
                    };
                    Mask = MaskList[0];
                    break;
                case LinearOperationMasks.Custom:
                    // random mask
                    Mask = new LinearOperationMask(this, 0, -1, 0, -1, 5, -1, 0, -1, 0);
                    //Mask = new LinearOperationMask(0, 0, 0, 0, 0, 0, 0, 0, 0);
                    CustomMaskInputVisible = true;
                    break;
                default:
                    break;
            }
            BorderTypeList = new ObservableCollection<BorderType>()
            {
                BorderType.Isolated,
                BorderType.Reflect,
                BorderType.Replicate,
            };
            LinearOperation();
        }

        #endregion

        #region Public Properties   

        // viewmodel that opened dialog that uses this viewmodel
        public ImageViewModel Parent;

        // currently selected border
        public BorderType BorderType
        {
            get => _BorderType;
            set
            {
                if (_BorderType == value) return;
                _BorderType = value;
                LinearOperation();
            }
        }
        // list of all supported border types
        public ObservableCollection<BorderType> BorderTypeList { get; set; }

        // currently selected mask
        public LinearOperationMask Mask
        {
            get => _Mask;
            set
            {
                if (_Mask == value) return;
                _Mask = value;
                LinearOperation();
            }
        }
        // list of all maska
        public ObservableCollection<LinearOperationMask> MaskList { get; set; }
        // controls visibility of text fields for custom mask
        public bool CustomMaskInputVisible
        {
            get => _CustomMaskInputVisible;
            set
            {
                if (_CustomMaskInputVisible == value) return;
                _CustomMaskInputVisible = value;
            }
        }

        #endregion

        #region Private fields

        private BorderType _BorderType = BorderType.Isolated;
        private Bitmap _UneditedImage;
        private LinearOperationMask _Mask;
        private bool _CustomMaskInputVisible = false;

        #endregion

        #region Methods

        /// <summary>
        /// Performs linear sharpening operation
        /// </summary>
        private void LinearOperation()
        {
            if (Mask != null)
            {
                Parent.Image = _UneditedImage;
                float[,] mask = {
                { Mask.i1, Mask.i2, Mask.i3 },
                { Mask.i4, Mask.i5, Mask.i6 },
                { Mask.i7, Mask.i8, Mask.i9 } };
                Parent.Image = EmguOperations.LinearSharpening(Parent.Image, mask, BorderType);
            }
        }

        #endregion
    }
}

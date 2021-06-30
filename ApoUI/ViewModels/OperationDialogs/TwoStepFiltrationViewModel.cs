using ApoCore;
using Emgu.CV.CvEnum;
using System.Collections.ObjectModel;

namespace ApoUI
{
    /// <summary>
    /// View model for two step filtration dialog
    /// </summary>
    public class TwoStepFiltrationViewModel : BaseViewModel
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="window"></param>
        public TwoStepFiltrationViewModel(ImageViewModel window)
        {
            this.Parent = window;

            BorderTypeList = new ObservableCollection<BorderType>()
            {
                BorderType.Isolated,
                BorderType.Reflect,
                BorderType.Replicate,
            };
            TwoStepFiltration();
        }

        #endregion

        #region Public Properties   

        // viewmodel that opened dialog that uses this viewmodel
        public ImageViewModel Parent;

        // values of first 3x3 mask
        public int FirstMask1
        {
            get => _FirstMask1;
            set
            {
                if (_FirstMask1 == value) return;
                _FirstMask1 = value;
                OnPropertyChanged();
                TwoStepFiltration();
            }
        }
        public int FirstMask2
        {
            get => _FirstMask2;
            set
            {
                if (_FirstMask2 == value) return;
                _FirstMask2 = value;
                OnPropertyChanged();
                TwoStepFiltration();
            }
        }
        public int FirstMask3
        {
            get => _FirstMask3;
            set
            {
                if (_FirstMask3 == value) return;
                _FirstMask3 = value;
                OnPropertyChanged();
                TwoStepFiltration();
            }
        }
        public int FirstMask4
        {
            get => _FirstMask4;
            set
            {
                if (_FirstMask4 == value) return;
                _FirstMask4 = value;
                OnPropertyChanged();
                TwoStepFiltration();
            }
        }
        public int FirstMask5
        {
            get => _FirstMask5;
            set
            {
                if (_FirstMask5 == value) return;
                _FirstMask5 = value;
                OnPropertyChanged();
                TwoStepFiltration();
            }
        }
        public int FirstMask6
        {
            get => _FirstMask6;
            set
            {
                if (_FirstMask6 == value) return;
                _FirstMask6 = value;
                OnPropertyChanged();
                TwoStepFiltration();
            }
        }
        public int FirstMask7
        {
            get => _FirstMask7;
            set
            {
                if (_FirstMask7 == value) return;
                _FirstMask7 = value;
                OnPropertyChanged();
                TwoStepFiltration();
            }
        }
        public int FirstMask8
        {
            get => _FirstMask8;
            set
            {
                if (_FirstMask8 == value) return;
                _FirstMask8 = value;
                OnPropertyChanged();
                TwoStepFiltration();
            }
        }
        public int FirstMask9
        {
            get => _FirstMask9;
            set
            {
                if (_FirstMask9 == value) return;
                _FirstMask9 = value;
                OnPropertyChanged();
                TwoStepFiltration();
            }
        }
        // values of second 3x3 mask
        public int SecondMask1
        {
            get => _SecondMask1;
            set
            {
                if (_SecondMask1 == value) return;
                _SecondMask1 = value;
                OnPropertyChanged();
                TwoStepFiltration();
            }
        }
        public int SecondMask2
        {
            get => _SecondMask2;
            set
            {
                if (_SecondMask2 == value) return;
                _SecondMask2 = value;
                OnPropertyChanged();
                TwoStepFiltration();
            }
        }
        public int SecondMask3
        {
            get => _SecondMask3;
            set
            {
                if (_SecondMask3 == value) return;
                _SecondMask3 = value;
                OnPropertyChanged();
                TwoStepFiltration();
            }
        }
        public int SecondMask4
        {
            get => _SecondMask4;
            set
            {
                if (_SecondMask4 == value) return;
                _SecondMask4 = value;
                OnPropertyChanged();
                TwoStepFiltration();
            }
        }
        public int SecondMask5
        {
            get => _SecondMask5;
            set
            {
                if (_SecondMask5 == value) return;
                _SecondMask5 = value;
                OnPropertyChanged();
                TwoStepFiltration();
            }
        }
        public int SecondMask6
        {
            get => _SecondMask6;
            set
            {
                if (_SecondMask6 == value) return;
                _SecondMask6 = value;
                OnPropertyChanged();
                TwoStepFiltration();
            }
        }
        public int SecondMask7
        {
            get => _SecondMask7;
            set
            {
                if (_SecondMask7 == value) return;
                _SecondMask7 = value;
                OnPropertyChanged();
                TwoStepFiltration();
            }
        }
        public int SecondMask8
        {
            get => _SecondMask8;
            set
            {
                if (_SecondMask8 == value) return;
                _SecondMask8 = value;
                OnPropertyChanged();
                TwoStepFiltration();
            }
        }
        public int SecondMask9
        {
            get => _SecondMask9;
            set
            {
                if (_SecondMask9 == value) return;
                _SecondMask9 = value;
                OnPropertyChanged();
                TwoStepFiltration();
            }
        }

        // currently selected border
        public BorderType BorderType
        {
            get => _BorderType;
            set
            {
                if (_BorderType == value) return;
                _BorderType = value;
                TwoStepFiltration();
            }
        }
        // list of all supported borders
        public ObservableCollection<BorderType> BorderTypeList { get; set; }

        #endregion

        #region Private fields

        private BorderType _BorderType = BorderType.Isolated;
        private int _FirstMask1 = 1;
        private int _FirstMask2 = 1;
        private int _FirstMask3 = 1;
        private int _FirstMask4 = 1;
        private int _FirstMask5 = 1;
        private int _FirstMask6 = 1;
        private int _FirstMask7 = 1;
        private int _FirstMask8 = 1;
        private int _FirstMask9 = 1;
        private int _SecondMask1 = 1;
        private int _SecondMask2 = -2;
        private int _SecondMask3 = 1;
        private int _SecondMask4 = -2;
        private int _SecondMask5 = 4;
        private int _SecondMask6 = -2;
        private int _SecondMask7 = 1;
        private int _SecondMask8 = -2;
        private int _SecondMask9 = 1;

        #endregion

        #region Methods

        /// <summary>
        /// Performs two step filtration
        /// </summary>
        private void TwoStepFiltration()
        {
            Parent.Image = Parent.backupimage;
            float[,] m1 = {
            { FirstMask1, FirstMask2, FirstMask3 },
            { FirstMask4, FirstMask5, FirstMask6 },
            { FirstMask7, FirstMask8, FirstMask9 } };
            float[,] m2 = {
            { SecondMask1, SecondMask2, SecondMask3 },
            { SecondMask4, SecondMask5, SecondMask6 },
            { SecondMask7, SecondMask8, SecondMask9 } };
            Parent.Image = EmguOperations.FiltrationTwoStep(Parent.Image, m1, m2, BorderType);
        }

        #endregion
    }
}

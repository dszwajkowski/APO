using ApoCore;
using Emgu.CV.CvEnum;
using System.Collections.ObjectModel;

namespace ApoUI
{
    /// <summary>
    /// View model for one step filtration dialog
    /// </summary>
    public class OneStepFiltrationViewModel : BaseViewModel
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="window"></param>
        public OneStepFiltrationViewModel(ImageViewModel window)
        {
            this.Parent = window;

            BorderTypeList = new ObservableCollection<BorderType>()
            {
                BorderType.Isolated,
                BorderType.Reflect,
                BorderType.Replicate,
            };
            OneStepFiltration();
        }

        #endregion

        #region Public Properties   

        // viewmodel that opened dialog that uses this viewmodel
        public ImageViewModel Parent;
        
        // values for 5x5 mask
        public int Mask1
        {
            get => _Mask1;
            set
            {
                if (_Mask1 == value) return;
                _Mask1 = value;
                OnPropertyChanged();
                OneStepFiltration();
            }
        }
        public int Mask2
        {
            get => _Mask2;
            set
            {
                if (_Mask2 == value) return;
                _Mask2 = value;
                OnPropertyChanged();
                OneStepFiltration();
            }
        }
        public int Mask3
        {
            get => _Mask3;
            set
            {
                if (_Mask3 == value) return;
                _Mask3 = value;
                OnPropertyChanged();
                OneStepFiltration();
            }
        }
        public int Mask4
        {
            get => _Mask4;
            set
            {
                if (_Mask4 == value) return;
                _Mask4 = value;
                OnPropertyChanged();
                OneStepFiltration();
            }
        }
        public int Mask5
        {
            get => _Mask5;
            set
            {
                if (_Mask5 == value) return;
                _Mask5 = value;
                OnPropertyChanged();
                OneStepFiltration();
            }
        }
        public int Mask6
        {
            get => _Mask6;
            set
            {
                if (_Mask6 == value) return;
                _Mask6 = value;
                OnPropertyChanged();
                OneStepFiltration();
            }
        }
        public int Mask7
        {
            get => _Mask7;
            set
            {
                if (_Mask7 == value) return;
                _Mask7 = value;
                OnPropertyChanged();
                OneStepFiltration();
            }
        }
        public int Mask8
        {
            get => _Mask8;
            set
            {
                if (_Mask8 == value) return;
                _Mask8 = value;
                OnPropertyChanged();
                OneStepFiltration();
            }
        }
        public int Mask9
        {
            get => _Mask9;
            set
            {
                if (_Mask9 == value) return;
                _Mask9 = value;
                OnPropertyChanged();
                OneStepFiltration();
            }
        }
        public int Mask10
        {
            get => _Mask10;
            set
            {
                if (_Mask10 == value) return;
                _Mask10 = value;
                OnPropertyChanged();
                OneStepFiltration();
            }
        }
        public int Mask11
        {
            get => _Mask11;
            set
            {
                if (_Mask11 == value) return;
                _Mask11 = value;
                OnPropertyChanged();
                OneStepFiltration();
            }
        }
        public int Mask12
        {
            get => _Mask12;
            set
            {
                if (_Mask12 == value) return;
                _Mask12 = value;
                OnPropertyChanged();
                OneStepFiltration();
            }
        }
        public int Mask13
        {
            get => _Mask13;
            set
            {
                if (_Mask13 == value) return;
                _Mask13 = value;
                OnPropertyChanged();
                OneStepFiltration();
            }
        }
        public int Mask14
        {
            get => _Mask14;
            set
            {
                if (_Mask14 == value) return;
                _Mask14 = value;
                OnPropertyChanged();
                OneStepFiltration();
            }
        }
        public int Mask15
        {
            get => _Mask15;
            set
            {
                if (_Mask15 == value) return;
                _Mask15 = value;
                OnPropertyChanged();
                OneStepFiltration();
            }
        }
        public int Mask16
        {
            get => _Mask16;
            set
            {
                if (_Mask16 == value) return;
                _Mask16 = value;
                OnPropertyChanged();
                OneStepFiltration();
            }
        }
        public int Mask17
        {
            get => _Mask17;
            set
            {
                if (_Mask17 == value) return;
                _Mask17 = value;
                OnPropertyChanged();
                OneStepFiltration();
            }
        }
        public int Mask18
        {
            get => _Mask18;
            set
            {
                if (_Mask18 == value) return;
                _Mask18 = value;
                OnPropertyChanged();
                OneStepFiltration();
            }
        }
        public int Mask19
        {
            get => _Mask19;
            set
            {
                if (_Mask19 == value) return;
                _Mask19 = value;
                OnPropertyChanged();
                OneStepFiltration();
            }
        }
        public int Mask20
        {
            get => _Mask20;
            set
            {
                if (_Mask20 == value) return;
                _Mask20 = value;
                OnPropertyChanged();
                OneStepFiltration();
            }
        }
        public int Mask21
        {
            get => _Mask21;
            set
            {
                if (_Mask21 == value) return;
                _Mask21 = value;
                OnPropertyChanged();
                OneStepFiltration();
            }
        }
        public int Mask22
        {
            get => _Mask22;
            set
            {
                if (_Mask22 == value) return;
                _Mask22 = value;
                OnPropertyChanged();
                OneStepFiltration();
            }
        }
        public int Mask23
        {
            get => _Mask23;
            set
            {
                if (_Mask23 == value) return;
                _Mask23 = value;
                OnPropertyChanged();
                OneStepFiltration();
            }
        }
        public int Mask24
        {
            get => _Mask24;
            set
            {
                if (_Mask24 == value) return;
                _Mask24 = value;
                OnPropertyChanged();
                OneStepFiltration();
            }
        }
        public int Mask25
        {
            get => _Mask25;
            set
            {
                if (_Mask25 == value) return;
                _Mask25 = value;
                OnPropertyChanged();
                OneStepFiltration();
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
                OneStepFiltration();
            }
        }
        // list of all supported border types
        public ObservableCollection<BorderType> BorderTypeList { get; set; }

        #endregion

        #region Private fields

        private BorderType _BorderType = BorderType.Isolated;
        private int _Mask1 = 1;
        private int _Mask2 = -1;
        private int _Mask3 = 0;
        private int _Mask4 = -1;
        private int _Mask5 = 1;
        private int _Mask6 = -1;
        private int _Mask7 = 1;
        private int _Mask8 = 0;
        private int _Mask9 = 1;
        private int _Mask10 = -1;
        private int _Mask11 = 0;
        private int _Mask12 = 0;
        private int _Mask13 = 0;
        private int _Mask14 = 0;
        private int _Mask15 = 0;
        private int _Mask16 = -1;
        private int _Mask17 = 1;
        private int _Mask18 = 0;
        private int _Mask19 = 1;
        private int _Mask20 = -1;
        private int _Mask21 = 1;
        private int _Mask22 = -1;
        private int _Mask23 = 0;
        private int _Mask24 = -1;
        private int _Mask25 = 1;

        #endregion

        #region Methods

        /// <summary>
        /// Perorms one stepp filtration
        /// </summary>
        private void OneStepFiltration()
        {
            Parent.Image = Parent.backupimage;
            float[,] m = {
            { Mask1, Mask2, Mask3, Mask4, Mask5 },
            { Mask6, Mask7, Mask8, Mask9, Mask10 },
            { Mask11, Mask12, Mask13, Mask14, Mask15 },
            { Mask16, Mask17, Mask18, Mask19, Mask20 },
            { Mask21, Mask22, Mask23, Mask24, Mask25 } };
            Parent.Image = EmguOperations.FiltrationOneStep(Parent.Image, m, BorderType);
        }

        #endregion
    }
}

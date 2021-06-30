using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ApoUI
{
    /// <summary>
    /// View Model for custom window
    /// </summary>
    public class CustomWindowViewModel : BaseViewModel
    {
        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="window"></param>
        public CustomWindowViewModel(Window window)
        {
            Window = window;
            Page = new MainPage();
            MinimizeCommand = new RelayCommand(() => Window.WindowState = WindowState.Minimized);
            MaximizeCommand = new RelayCommand(() => Window.WindowState ^= WindowState.Maximized);
            CloseCommand = new RelayCommand(() => Window.Close());
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="window"></param>
        /// <param name="page"></param>
        public CustomWindowViewModel(Window window, Page page)
        {
            Window = window;
            Page = page;
            MinimizeCommand = new RelayCommand(() => Window.WindowState = WindowState.Minimized);
            MaximizeCommand = new RelayCommand(() => Window.WindowState ^= WindowState.Maximized);
            CloseCommand = new RelayCommand(() => Window.Close());
        }

        #endregion

        #region Public properties

        // currently displayed page
        public Page Page
        {
            get => page;
            set
            {
                if (page == value)
                    return;
                page = value;
                OnPropertyChanged();
            }
        }
        // window that uses this viewmodel
        public Window Window
        {
            get => window;
            set
            {
                if (window == value)
                    return;
                window = value;
                OnPropertyChanged();
            }
        }

        // Size of resize border
        public int ResizeBorder { get; set; } = 6;

        // Thickness of resize border
        public Thickness ResizeBorderThickness { get { return new Thickness(ResizeBorder); } }
        // Height of title bar
        public int TitleHeight { get; set; } = 40;

        #endregion

        #region Private member

        private Window window;
        private Page page;

        #endregion

        #region Commands

        public ICommand MinimizeCommand { get; set; }
        public ICommand MaximizeCommand { get; set; }
        public ICommand CloseCommand { get; set; }

        #endregion
    }
}

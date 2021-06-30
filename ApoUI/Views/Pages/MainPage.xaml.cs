using System.Windows.Controls;

namespace ApoUI
{
    /// <summary>
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
            this.DataContext = new MainPageViewModel();
        }
        public MainPage(Page page, ImageViewModel viewModel)
        {
            InitializeComponent();
            this.DataContext = new MainPageViewModel(page, viewModel);
        }
    }
}

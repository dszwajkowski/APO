using System.Windows;
using System.Windows.Controls;

namespace ApoUI
{
    /// <summary>
    /// Interaction logic for CustomWindow.xaml
    /// </summary>
    public partial class CustomWindow : Window
    {
        public CustomWindow()
        {
            InitializeComponent();
            this.DataContext = new CustomWindowViewModel(this);
        }

        public CustomWindow(Page page)
        {
            InitializeComponent();
            this.DataContext = new CustomWindowViewModel(this, page);
        }
    }
}

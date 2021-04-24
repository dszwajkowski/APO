using ApoUI.ViewModels;
using System.Windows;

namespace ApoUI.Views
{
    /// <summary>
    /// Interaction logic for ImageView.xaml
    /// </summary>
    public partial class ImageView : Window
    {
        public ImageView()
        {
            InitializeComponent();
            this.DataContext = new ImageViewModel();
            //this.DataContext = new WindowViewModel(this);
        }
    }
}

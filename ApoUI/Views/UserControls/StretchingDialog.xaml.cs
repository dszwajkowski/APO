using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ApoUI
{
    /// <summary>
    /// Interaction logic for StretchingDialog.xaml
    /// </summary>
    public partial class StretchingDialog : UserControl
    {
        public StretchingDialog()
        {
            InitializeComponent();
        }

        private void Slider_PreviewMouseUp_P1(object sender, MouseButtonEventArgs e)
        {
            if (this.DataContext != null)
            {
                ((dynamic)this.DataContext).P1 = (int)((Slider)sender).Value;
            }
        }

        private void Slider_PreviewMouseUp_P2(object sender, MouseButtonEventArgs e)
        {
            if (this.DataContext != null)
            {
                ((dynamic)this.DataContext).P2 = (int)((Slider)sender).Value;
            }
        }
    }
}

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
    /// Interaction logic for ThresholdingDialog.xaml
    /// </summary>
    public partial class AdaptiveThresholdingDialog : UserControl
    {
        public AdaptiveThresholdingDialog()
        {
            InitializeComponent();
        }

        private void Slider_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (this.DataContext != null)
            {
                ((dynamic)this.DataContext).Threshold = (int)((Slider)sender).Value;
            }
        }
    }
}

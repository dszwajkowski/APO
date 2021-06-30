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
    /// Interaction logic for ContrastEnhancementDialog.xaml
    /// </summary>
    public partial class ContrastEnhancementDialog : UserControl
    {
        public ContrastEnhancementDialog()
        {
            InitializeComponent();
        }

        private void Slider_PreviewMouseUp_InputClippingMin(object sender, MouseButtonEventArgs e)
        {
            if (this.DataContext != null)
            {
                ((dynamic)this.DataContext).InputClippingMin = (int)((Slider)sender).Value;
            }
        }

        private void Slider_PreviewMouseUp_InputClippingMax(object sender, MouseButtonEventArgs e)
        {
            if (this.DataContext != null)
            {
                ((dynamic)this.DataContext).InputClippingMax = (int)((Slider)sender).Value;
            }
        }
        private void Slider_PreviewMouseUp_OutputCompressionMin(object sender, MouseButtonEventArgs e)
        {
            if (this.DataContext != null)
            {
                ((dynamic)this.DataContext).OutputCompressionMin = (int)((Slider)sender).Value;
            }
        }

        private void Slider_PreviewMouseUp_OutputCompressionMax(object sender, MouseButtonEventArgs e)
        {
            if (this.DataContext != null)
            {
                ((dynamic)this.DataContext).OutputCompressionMax = (int)((Slider)sender).Value;
            }
        }

        private void Slider_PreviewMouseUp_Gamma(object sender, MouseButtonEventArgs e)
        {
            if (this.DataContext != null)
            {
                ((dynamic)this.DataContext).Gamma = (double)((Slider)sender).Value;
            }
        }
    }
}

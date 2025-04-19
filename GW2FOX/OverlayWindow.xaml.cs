using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace GW2FOX
{
    public partial class OverlayWindow : Window
    {

        public OverlayWindow()
        {
            InitializeComponent();
        }

        private void Icon_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}

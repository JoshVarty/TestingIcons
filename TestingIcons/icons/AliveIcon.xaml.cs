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

namespace TestingIcons.icons
{
    /// <summary>
    /// Interaction logic for AliveIcon.xaml
    /// </summary>
    public partial class AliveIcon : UserControl
    {
        public enum IconStates
        {
            Idle,
            Processing,
            Success,
            Error,
            Warning,
        }

        public AliveIcon()
        {
            InitializeComponent();
            //Our default state
            VisualStateManager.GoToState(this, nameof(IconStates.Idle), true);
        }

        private void IdleButton_Click(object sender, RoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, nameof(IconStates.Idle), true);
        }

        private void ProcessingButton_Click(object sender, RoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, nameof(IconStates.Processing), true);
        }

        private void SuccessButton_Click(object sender, RoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, nameof(IconStates.Success), true);
        }

        private void ErrorButton_Click(object sender, RoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, nameof(IconStates.Error), true);
        }

        private void WarningButton_Click(object sender, RoutedEventArgs e)
        {
            VisualStateManager.GoToState(this, "Warning", true);
        }
    }
}

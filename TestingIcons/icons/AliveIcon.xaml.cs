using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
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
        private enum AliveState
        {
            Idle,
            Processing,
            Success,
            Error,
            Warning,
            Stop
        }

        /// <summary>
        /// The current logical state represents the underlying state of the icon.
        /// Typically, this is one-to-one with the visual state of the icon. However,
        /// the icon may also enter a Stop state when the user hovers over the icon.
        /// </summary>
        private AliveState _currentLogicalState;
        private AliveState CurrentLogicalState
        {
            get
            {
                return _currentLogicalState;
            }
            set
            {
                _currentLogicalState = value;
                VisualStateManager.GoToState(this, value.ToString(), true);
            }
        }

        private Timer aTimer;
        public AliveIcon()
        {
            InitializeComponent();
            CurrentLogicalState = AliveState.Idle;
            aTimer = new Timer(3000);
            aTimer.Elapsed += timerElapsed;
            aTimer.Enabled = true;


        }

        private void timerElapsed(object sender, ElapsedEventArgs e)
        {
            var currentVisualState = iconStateGroup.CurrentState.Name;
            System.Diagnostics.Debug.WriteLine(currentVisualState);
        }

        public void GoToIdle()
        {
            CurrentLogicalState = AliveState.Idle;
        }

        public void GoToProcessing()
        {
            CurrentLogicalState = AliveState.Processing;
        }

        public void GoToSuccess()
        {
            CurrentLogicalState = AliveState.Success;
        }

        public void GoToWarning()
        {
            CurrentLogicalState = AliveState.Warning;
        }

        public void GoToError()
        {
            CurrentLogicalState = AliveState.Error;
        }

        private void aliveIcon_MouseEnter(object sender, MouseEventArgs e)
        {
            var currentVisualState = iconStateGroup.CurrentState.Name;
            switch (currentVisualState)
            {
                case nameof(AliveState.Success):
                case nameof(AliveState.Error):
                case nameof(AliveState.Warning):
                    VisualStateManager.GoToState(this, nameof(AliveState.Stop), true);
                    break;
                case nameof(AliveState.Processing):
                case nameof(AliveState.Idle):
                case nameof(AliveState.Stop):
                default:
                    break;
            }
        }

        private void aliveIcon_MouseLeave(object sender, MouseEventArgs e)
        {
            var currentVisualState = iconStateGroup.CurrentState.Name;
            switch (currentVisualState)
            {
                //Return to current logical state 
                case nameof(AliveState.Stop):
                    VisualStateManager.GoToState(this, CurrentLogicalState.ToString(), true);
                    break;
                case nameof(AliveState.Processing):
                case nameof(AliveState.Success):
                case nameof(AliveState.Error):
                case nameof(AliveState.Warning):
                case nameof(AliveState.Idle):
                    break;
                default:
                    break;
            }
        }

        private void aliveIcon_Clicked(object sender, MouseButtonEventArgs e)
        {
            cycleStates();

            return;
            var currentVisualState = iconStateGroup.CurrentState.Name;
            switch (currentVisualState)
            {
                case nameof(AliveState.Idle):
                    launchAliveWindow();
                    break;
                case nameof(AliveState.Stop):
                    stopAlive();
                    break;
                case nameof(AliveState.Warning):
                case nameof(AliveState.Processing):
                case nameof(AliveState.Success):
                case nameof(AliveState.Error):
                default:
                    break;
            }
        }

        int index = 0;
        private void cycleStates()
        {
            if (index == 0)
            {
                VisualStateManager.GoToState(this, nameof(AliveState.Processing), true);
            }
            else if (index == 1)
            {
                VisualStateManager.GoToState(this, nameof(AliveState.Success), true);
            }
            else if (index == 2)
            {
                VisualStateManager.GoToState(this, nameof(AliveState.Processing), true);
            }
            else if (index == 3)
            {
                VisualStateManager.GoToState(this, nameof(AliveState.Error), true);
            }
            else if (index == 4)
            {
                VisualStateManager.GoToState(this, nameof(AliveState.Processing), true);
            }
            else if (index == 5)
            {
                VisualStateManager.GoToState(this, nameof(AliveState.Warning), true);
            }


            index++;

        }

        private void launchAliveWindow()
        {
            //TODO: @AmadeusW please insert any code here to launch the window.
        }

        private void stopAlive()
        {
  
        }
    }
}

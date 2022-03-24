using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading;
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
using System.Windows.Threading;
using ControlzEx.Theming;
using MahApps.Metro.Controls;

namespace SimpleTimer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public TimeOnly Time { get; set; }
        public TimeOnly EndTime { get; set; }
        public TimeOnly SpentTime { get; set; }

        public DispatcherTimer MainTimer
        {
            get; set;
        }

        public MainWindow()
        {
            InitializeComponent();
            MainTimer = new DispatcherTimer();
            MainTimer.Interval = TimeSpan.FromSeconds(1);
            MainTimer.Tick += (sender, args) => SpendTime();

            ThemeManager.Current.ThemeSyncMode = ThemeSyncMode.SyncAll;
            ThemeManager.Current.SyncTheme();
        }

        private void CtrlTimeButton_OnClick(object sender, RoutedEventArgs e)
        {

            if (SpentTime.ToTimeSpan().TotalSeconds == 0)
            {
                CtrlTimeButton.IsChecked = false;
                return;
            }

            EndTime = TimeOnly.FromDateTime(DateTime.Now);
            EndTime =  EndTime.Add(Time.ToTimeSpan());
            if (CtrlTimeButton.IsChecked == true)
            {
                MainTimer.Start();
                TimeAfterEndLabel.Visibility = Visibility.Visible;
            }
            else
            {
                MainTimer.Stop();
                TimeAfterEndLabel.Visibility = Visibility.Hidden;
            }

        }


        void SpendTime()
        {
            SpentTime = SpentTime.Add(-TimeSpan.FromSeconds(1));
            EndTime = TimeOnly.FromDateTime(DateTime.Now).Add(SpentTime.ToTimeSpan());
            RemainTimeLabel.Content = SpentTime;
            Debug.Write((SpentTime.ToTimeSpan().TotalSeconds / Time.ToTimeSpan().TotalSeconds) * 360); 
            if (SpentTime.ToTimeSpan().TotalSeconds == 0)
            {
                CtrlTimeButton.IsChecked = false;
                SystemSounds.Beep.Play();
                MainTimer.Stop();
            }
        }


        private void RemainTimeLabel_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if(CtrlTimeButton.IsChecked == false) 
                MTimeSelector.Visibility = Visibility.Visible;

        }


        private void MTimeSelector_OnConfirmClick(object? sender, EventArgs e)
        {
            int h = MTimeSelector.Hour.Value;
            int m = MTimeSelector.Minute.Value;
            int s = MTimeSelector.Second.Value;
            Time = new TimeOnly(h, m, s);
            SpentTime = new TimeOnly(h, m, s);
            RemainTimeLabel.Content = Time.ToString("HH:mm:ss");
            MTimeSelector.Visibility = Visibility.Hidden;
        }

        private void CtrlRestartButton_OnClick(object sender, RoutedEventArgs e)
        {
            MainTimer.Stop();
            CtrlTimeButton.IsChecked = false;
            SpentTime = new TimeOnly(Time.Hour, Time.Minute, Time.Second);
            RemainTimeLabel.Content = SpentTime;
        }
    }
}

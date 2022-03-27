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
using System.Windows.Media.Animation;
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
        public TimeOnly SelectedTime
        { get; set; }
        public TimeOnly EndTime { get; set; }
        public static TimeOnly SpentTime { get; set; }
        public static double FromAngle { get; set; } = 360;

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
            EndTime =  EndTime.Add(SelectedTime.ToTimeSpan());
            SyncArcValue();

            if (CtrlTimeButton.IsChecked == true)
            {
                MainTimer.Start();
                TimeAfterEndLabel.Content = EndTime;
                TimeAfterEndLabel.Visibility = Visibility.Visible; 
            }
            else
            {
                MainTimer.Stop();
                TimeAfterEndLabel.Visibility = Visibility.Collapsed;
            }

        }

        void SyncArcValue()
        {
            Arc.EndAngle = (SpentTime.ToTimeSpan().TotalSeconds / SelectedTime.ToTimeSpan().TotalSeconds) * 360;
        }

        void SpendTime()
        {
            SpentTime = SpentTime.Add(-TimeSpan.FromSeconds(1));
            Debug.WriteLine(DateTime.Now);
            EndTime = TimeOnly.FromDateTime(DateTime.Now).Add(SpentTime.ToTimeSpan());
            RemainTimeLabel.Content = SpentTime;
            SyncArcValue();
            if (SpentTime.ToTimeSpan().TotalSeconds == 0)
            {
                CtrlTimeButton.IsChecked = false;
                SystemSounds.Beep.Play();
                MainTimer.Stop();
                Arc.Visibility = Visibility.Collapsed;
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
            SelectedTime = new TimeOnly(h, m, s);
            SpentTime = new TimeOnly(h, m, s);
            SyncArcValue();
            RemainTimeLabel.Content = SelectedTime.ToString("HH:mm:ss");
            MTimeSelector.Visibility = Visibility.Hidden;
            Arc.Visibility = Visibility.Visible;
        }

        private void CtrlRestartButton_OnClick(object sender, RoutedEventArgs e)
        {
            MainTimer.Stop();
            CtrlTimeButton.IsChecked = false;
            SpentTime = new TimeOnly(SelectedTime.Hour, SelectedTime.Minute, SelectedTime.Second);
            Arc.EndAngle = (SpentTime.ToTimeSpan().TotalSeconds / SelectedTime.ToTimeSpan().TotalSeconds) * 360;
            RemainTimeLabel.Content = SpentTime;
            Arc.Visibility = Visibility.Visible;
        }

        private void MTimeSelector_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if(MTimeSelector.Visibility == Visibility.Visible)
            {
                var temp = new DispatcherTimer();
                temp.Interval = TimeSpan.FromTicks(100);
                temp.Tick  += (args, evt) =>
                {
                    for(double i = 0.1; i <= 1; i += 0.01)
                        MTimeSelector.BgBorder.Opacity = i;

                    Debug.Print(MTimeSelector.BgBorder.Opacity+"");
                };

                temp.Start();
            }
        }
    }
}

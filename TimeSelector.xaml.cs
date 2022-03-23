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
using ControlzEx.Theming;

namespace SimpleTimer
{
    /// <summary>
    /// TimeSelector.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class TimeSelector : UserControl
    {
        public TimeSelector()
        {
            InitializeComponent();
            ThemeManager.Current.ThemeSyncMode = ThemeSyncMode.SyncAll;
            ThemeManager.Current.SyncTheme();
            BgBorder.Background = SystemParameters.WindowGlassBrush;

        }

        private void BackgroundRect_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Visibility = Visibility.Hidden;
        }

        private void CtrlConfirmButton_OnClick(object sender, RoutedEventArgs e)
        {
            
        }

        private void CtrlCancelButton_OnClick(object sender, RoutedEventArgs e)
        {
            Visibility = Visibility.Hidden;
        }
    }
}

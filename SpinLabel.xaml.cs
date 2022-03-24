using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
using SimpleTimer.Annotations;

namespace SimpleTimer
{
    /// <summary>
    /// SpinLabel.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class SpinLabel : UserControl, INotifyPropertyChanged
    {
        [Category("MaxNumber"), Description("Max Amount of spin")]
        public int Max
        {
            get;
            set;
        }


        public SpinLabel TossTarget
        {
            get;
            set;
        }

        private int _value;
        public int Value
        {
            get => _value;
            set
            {
                _value = value;
                OnPropertyChanged();
            }
        }

        public SpinLabel()
        {
            InitializeComponent();
            DataContext = this;
        }

        enum Incrementer
        {
            Minus = -1,Plus = 1
        }

        void addValue(Incrementer d)
        {
            if (d == Incrementer.Plus)
            {
                if (Value + 1 > Max)
                {
                    Value = 0;
                    if (TossTarget != null)
                        TossTarget.addValue(d);
                }
                else
                    Value++;
            }
            else if (d == Incrementer.Minus)
            {
                if (Value - 1 < 0)
                {
                    Value = Max;
                    if (TossTarget != null)
                        TossTarget.addValue(d);
                }
                else
                    Value--;
            }
        }


        private void CtrlUpButton_OnClick(object sender, RoutedEventArgs e)
        {
            addValue(Incrementer.Plus);
        }

        private void CtrlDownButton_OnClick(object sender, RoutedEventArgs e)
        {
            addValue(Incrementer.Minus);
        }


        public event PropertyChangedEventHandler? PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChangedEventHandler? handler = this.PropertyChanged;
            if (handler != null)
            {
                var e = new PropertyChangedEventArgs(propertyName);
                handler(this, e);
            }
        }
    }
}

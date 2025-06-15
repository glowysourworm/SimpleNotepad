using System.Windows;
using System.Windows.Controls;

namespace SimpleNotepad.View
{
    public partial class BoolPropertyView : UserControl
    {
        public static readonly DependencyProperty BoolNameProperty =
            DependencyProperty.Register("BoolName", typeof(string), typeof(BoolPropertyView));

        public static readonly DependencyProperty BoolValueProperty =
            DependencyProperty.Register("BoolValue", typeof(bool), typeof(BoolPropertyView));

        public string BoolName
        {
            get { return (string)GetValue(BoolNameProperty); }
            set { SetValue(BoolNameProperty, value); }
        }

        public bool BoolValue
        {
            get { return (bool)GetValue(BoolValueProperty); }
            set { SetValue(BoolValueProperty, value); }
        }

        public BoolPropertyView()
        {
            InitializeComponent();
        }
    }
}

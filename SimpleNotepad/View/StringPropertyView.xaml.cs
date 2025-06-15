using System.Windows;
using System.Windows.Controls;

namespace SimpleNotepad.View
{
    public partial class StringPropertyView : UserControl
    {
        public static readonly DependencyProperty StringNameProperty =
            DependencyProperty.Register("StringName", typeof(string), typeof(StringPropertyView));

        public static readonly DependencyProperty StringValueProperty =
            DependencyProperty.Register("StringValue", typeof(string), typeof(StringPropertyView));

        public string StringName
        {
            get { return (string)GetValue(StringNameProperty); }
            set { SetValue(StringNameProperty, value); }
        }
        public string StringValue
        {
            get { return (string)GetValue(StringValueProperty); }
            set { SetValue(StringValueProperty, value); }
        }

        public StringPropertyView()
        {
            InitializeComponent();
        }
    }
}

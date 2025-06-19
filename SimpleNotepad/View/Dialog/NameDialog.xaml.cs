using System.Windows;

namespace SimpleNotepad.View.Dialog
{
    public partial class NameDialog : Window
    {
        public static readonly DependencyProperty DialogNameResultProperty =
            DependencyProperty.Register("DialogNameResult", typeof(string), typeof(NameDialog));

        public static readonly DependencyProperty DialogLabelProperty =
            DependencyProperty.Register("DialogLabel", typeof(string), typeof(NameDialog));

        public static readonly DependencyProperty DialogTitleProperty =
            DependencyProperty.Register("DialogTitle", typeof(string), typeof(NameDialog));

        public string DialogNameResult
        {
            get { return (string)GetValue(DialogNameResultProperty); }
            set { SetValue(DialogNameResultProperty, value); }
        }
        public string DialogLabel
        {
            get { return (string)GetValue(DialogLabelProperty); }
            set { SetValue(DialogLabelProperty, value); }
        }
        public string DialogTitle
        {
            get { return (string)GetValue(DialogTitleProperty); }
            set { SetValue(DialogTitleProperty, value); }
        }

        public NameDialog()
        {
            InitializeComponent();

            this.DataContext = this;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void ResultTB_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            this.OkButton.IsEnabled = !string.IsNullOrWhiteSpace(this.ResultTB.Text);
        }
    }
}

using System.Windows;

using AvalonDock.Themes;

using SimpleNotepad.ViewModel;

namespace SimpleNotepad
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            this.DataContext = new MainViewModel();
        }
    }
}
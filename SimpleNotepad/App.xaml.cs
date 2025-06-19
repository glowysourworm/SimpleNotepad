using System.Windows;

namespace SimpleNotepad
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            InitializeResources();
        }


        void InitializeResources()
        {
            var resourceUri0 = new Uri("pack://application:,,,/SimpleNotepad;Component/Resources/ControlThemes.xaml");

            var resourceDictionary0 = new ResourceDictionary();

            resourceDictionary0.Source = resourceUri0;

            this.Resources.MergedDictionaries.Add(resourceDictionary0);
        }
    }

}

using System.Collections.ObjectModel;

using SimpleNotepad.Model;

using SimpleWpf.Extensions;

namespace SimpleNotepad.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        ObservableCollection<DockingManagerItemViewModel> _dockingManagerItemsSource;

        public ObservableCollection<DockingManagerItemViewModel> DockingManagerItemsSource
        {
            get { return _dockingManagerItemsSource; }
            set { this.RaiseAndSetIfChanged(ref _dockingManagerItemsSource, value); }
        }



        public MainViewModel()
        {
            // Docking Manager Binding:  It appears that the items are based on a single collection. So, what probably
            //                           has to happen is that you create all the proper template selectors, and even
            //                           attached behaviors to intercept how to build the UI. Each of these will bind
            //                           to the proper view model type.

            this.DockingManagerItemsSource = new ObservableCollection<DockingManagerItemViewModel>()
            {
                new DocumentViewModel("", true)
                {
                    FileName = "(new file)",
                    IsDirty = true,
                    Contents = "Document Contents"
                },
                new SyntaxTemplateMainViewModel()
                {
                    Templates = new ObservableCollection<SyntaxTemplateViewModel>()
                    {
                        new SyntaxTemplateViewModel()
                        {
                            Name = "My Template",
                            TemplateBody = "My Template Body with one parameter {name}",
                            Parameters = new ObservableCollection<SyntaxTemplateParameterViewModel>()
                            {
                                new SyntaxTemplateParameterViewModel()
                                {
                                    Name = "name"
                                }
                            }
                        }
                    }
                }
            };
        }
    }
}

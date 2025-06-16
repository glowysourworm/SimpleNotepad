using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Forms;

using SimpleWpf.Extensions;
using SimpleWpf.Extensions.Command;
using SimpleWpf.Extensions.Event;

namespace SimpleNotepad.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        public event SimpleEventHandler<DocumentViewModel, SyntaxTemplateViewModel> PlaySyntaxTemplateEvent;

        ObservableCollection<DockingManagerItemViewModel> _dockingManagerItemsSource;

        SyntaxTemplateViewModel _selectedSyntaxTemplate;

        SimpleCommand _openCommand;
        SimpleCommand _saveCommand;
        SimpleCommand _saveAsCommand;
        SimpleCommand _closeCommand;

        SimpleCommand _playCommand;
        SimpleCommand _playRestCommand;

        public ObservableCollection<DockingManagerItemViewModel> DockingManagerItemsSource
        {
            get { return _dockingManagerItemsSource; }
            set { this.RaiseAndSetIfChanged(ref _dockingManagerItemsSource, value); }
        }
        public ObservableCollection<SyntaxTemplateViewModel> SyntaxTemplates
        {
            get
            {
                return (this.DockingManagerItemsSource
                             .FirstOrDefault(x => x.GetType() == typeof(SyntaxTemplateMainViewModel)) as SyntaxTemplateMainViewModel)?.Templates;
            }
        }

        public SimpleCommand PlayCommand
        {
            get { return _playCommand; }
            set { this.RaiseAndSetIfChanged(ref _playCommand, value); }
        }
        public SimpleCommand PlayRestCommand
        {
            get { return _playRestCommand; }
            set { this.RaiseAndSetIfChanged(ref _playRestCommand, value); }
        }

        public SimpleCommand OpenCommand
        {
            get { return _openCommand; }
            set { this.RaiseAndSetIfChanged(ref _openCommand, value); }
        }
        public SimpleCommand SaveCommand
        {
            get { return _saveCommand; }
            set { this.RaiseAndSetIfChanged(ref _saveCommand, value); }
        }
        public SimpleCommand SaveAsCommand
        {
            get { return _saveAsCommand; }
            set { this.RaiseAndSetIfChanged(ref _saveAsCommand, value); }
        }
        public SimpleCommand CloseCommand
        {
            get { return _closeCommand; }
            set { this.RaiseAndSetIfChanged(ref _closeCommand, value); }
        }

        public SyntaxTemplateViewModel SelectedSyntaxTemplate
        {
            get { return _selectedSyntaxTemplate; }
            set { this.RaiseAndSetIfChanged(ref _selectedSyntaxTemplate, value); }
        }

        public MainViewModel()
        {
            // Docking Manager Binding:  It appears that the items are based on a single collection. So, what probably
            //                           has to happen is that you create all the proper template selectors, and even
            //                           attached behaviors to intercept how to build the UI. Each of these will bind
            //                           to the proper view model type.

            var defaultDocument = new DocumentViewModel("", true)
            {
                FileName = "(new file)",
                IsDirty = true,
                Contents = "Document Contents"
            };
            var syntaxTemplateMain = new SyntaxTemplateMainViewModel()
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
                                Parameter = "name"
                            }
                        }
                    }
                }
            };

            this.DockingManagerItemsSource = new ObservableCollection<DockingManagerItemViewModel>()
            {
                defaultDocument, syntaxTemplateMain
            };

            this.OpenCommand = new SimpleCommand(() =>
            {
                var dialog = new OpenFileDialog();

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    foreach (var file in dialog.FileNames)
                    {
                        var text = File.ReadAllText(file);
                        var newDocument = new DocumentViewModel(file, false)
                        {
                            Contents = text,
                            IsSelected = true
                        };

                        this.DockingManagerItemsSource.Add(newDocument);
                    }
                }
            });

            this.PlayCommand = new SimpleCommand(() =>
            {
                var currentDocument = this.DockingManagerItemsSource.FirstOrDefault(x => x.IsSelected) as DocumentViewModel;

                if (this.SelectedSyntaxTemplate != null &&
                    currentDocument != null && 
                    this.PlaySyntaxTemplateEvent != null)
                    this.PlaySyntaxTemplateEvent(currentDocument, this.SelectedSyntaxTemplate);
            });
        }
    }
}

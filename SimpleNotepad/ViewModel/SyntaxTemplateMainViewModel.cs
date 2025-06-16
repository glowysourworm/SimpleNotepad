using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

using SimpleWpf.Extensions.Command;

namespace SimpleNotepad.ViewModel
{
    public class SyntaxTemplateMainViewModel : DockingManagerItemViewModel
    {
        SimpleCommand _addTemplateCommand;

        ObservableCollection<SyntaxTemplateViewModel> _templates;

        public ObservableCollection<SyntaxTemplateViewModel> Templates
        {
            get { return _templates; }
            set { this.RaiseAndSetIfChanged(ref _templates, value); }
        }

        [JsonIgnore]
        public SimpleCommand AddTemplateCommand
        {
            get { return _addTemplateCommand; }
            set { this.RaiseAndSetIfChanged(ref _addTemplateCommand, value); }
        }

        public SyntaxTemplateMainViewModel()
        {
            this.Header = "New Template";                 
            this.Templates = new ObservableCollection<SyntaxTemplateViewModel>();

            this.AddTemplateCommand = new SimpleCommand(() =>
            {
                this.Templates.Add(new SyntaxTemplateViewModel()
                {
                    Name = "New Template",
                    Parameters = new ObservableCollection<SyntaxTemplateParameterViewModel>()
                    {
                        new SyntaxTemplateParameterViewModel() { Parameter = "name"},
                        new SyntaxTemplateParameterViewModel() { Parameter = "names"},
                    },
                    TemplateBody = "Place your parameter {name} or {names} here and {name} it!"
                });
            });
        }
    }
}

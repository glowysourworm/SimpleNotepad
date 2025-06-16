using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

using SimpleWpf.Extensions;
using SimpleWpf.Extensions.Command;

namespace SimpleNotepad.ViewModel
{
    public class SyntaxTemplateViewModel : ViewModelBase
    {
        string _name;
        string _templateBody;
        ObservableCollection<SyntaxTemplateParameterViewModel> _parameters;

        SimpleCommand _addParameterCommand;
        SimpleCommand<SyntaxTemplateParameterViewModel> _removeParameterCommand;

        public string Name
        {
            get { return _name; }
            set { this.RaiseAndSetIfChanged(ref _name, value); }
        }
        public string TemplateBody
        {
            get { return _templateBody; }
            set { this.RaiseAndSetIfChanged(ref _templateBody, value); }
        }
        public ObservableCollection<SyntaxTemplateParameterViewModel> Parameters
        {
            get { return _parameters; }
            set { this.RaiseAndSetIfChanged(ref _parameters, value); }
        }

        [JsonIgnore]
        public SimpleCommand AddParameterCommand
        {
            get { return _addParameterCommand; }
            set { this.RaiseAndSetIfChanged(ref _addParameterCommand, value); }
        }

        [JsonIgnore]
        public SimpleCommand<SyntaxTemplateParameterViewModel> RemoveParameterCommand
        {
            get { return _removeParameterCommand; }
            set { this.RaiseAndSetIfChanged(ref _removeParameterCommand, value); }
        }

        public SyntaxTemplateViewModel()
        {
            this.Name = string.Empty;
            this.TemplateBody = string.Empty;
            this.Parameters = new ObservableCollection<SyntaxTemplateParameterViewModel>();

            this.AddParameterCommand = new SimpleCommand(() =>
            {
                this.Parameters.Add(new SyntaxTemplateParameterViewModel()
                {
                    Parameter = "parameter"
                });
            });
            this.RemoveParameterCommand = new SimpleCommand<SyntaxTemplateParameterViewModel>(parameter =>
            {
                this.Parameters.Remove(parameter);
            });
        }
    }
}

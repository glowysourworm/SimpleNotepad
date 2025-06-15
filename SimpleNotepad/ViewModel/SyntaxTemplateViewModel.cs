using System.Collections.ObjectModel;

using SimpleWpf.Extensions;

namespace SimpleNotepad.ViewModel
{
    public class SyntaxTemplateViewModel : ViewModelBase
    {
        string _name;
        string _templateBody;
        ObservableCollection<SyntaxTemplateParameterViewModel> _parameters;

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

        public SyntaxTemplateViewModel()
        {
            this.Name = string.Empty;
            this.TemplateBody = string.Empty;
            this.Parameters = new ObservableCollection<SyntaxTemplateParameterViewModel>();
        }
    }
}

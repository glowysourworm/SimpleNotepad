using SimpleWpf.Extensions;

namespace SimpleNotepad.ViewModel
{
    public class SyntaxTemplateParameterViewModel : ViewModelBase
    {
        string _name;

        public string Name
        {
            get { return _name; }
            set { this.RaiseAndSetIfChanged(ref _name, value); }
        }

        public SyntaxTemplateParameterViewModel()
        {
            this.Name = string.Empty;
        }
    }
}

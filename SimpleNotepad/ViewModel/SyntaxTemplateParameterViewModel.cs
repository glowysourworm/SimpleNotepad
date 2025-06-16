using SimpleWpf.Extensions;

namespace SimpleNotepad.ViewModel
{
    public class SyntaxTemplateParameterViewModel : ViewModelBase
    {
        string _parameter;

        public string Parameter
        {
            get { return _parameter; }
            set { this.RaiseAndSetIfChanged(ref _parameter, value); }
        }

        public SyntaxTemplateParameterViewModel()
        {
            this.Parameter = string.Empty;
        }
    }
}

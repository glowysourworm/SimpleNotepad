using SimpleWpf.Extensions;

namespace SimpleNotepad.Model
{
    public class CPlusPlusViewModel : ViewModelBase
    {
        CodeClass _class;

        public CodeClass Class
        {
            get { return _class; }
            set { this.RaiseAndSetIfChanged(ref _class, value); }
        }

        public CPlusPlusViewModel()
        {
            this.Class = new CodeClass();
        }
    }
}

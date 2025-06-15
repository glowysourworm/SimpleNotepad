using System.Collections.ObjectModel;

using SimpleWpf.Extensions;

namespace SimpleNotepad.Model
{
    public class CodeFunction : ViewModelBase
    {
        string _name;
        string _prefix;
        CodeArgument _return;
        CodeModifier _modifier;
        CodeAccess _access;
        ObservableCollection<CodeArgument> _arguments;

        public string Prefix
        {
            get { return _prefix; }
            set { this.RaiseAndSetIfChanged(ref _prefix, value); }
        }
        public string Name
        {
            get { return _name; }
            set { this.RaiseAndSetIfChanged(ref _name, value); }
        }
        public CodeModifier Modifier
        {
            get { return _modifier; }
            set { this.RaiseAndSetIfChanged(ref _modifier, value); }
        }
        public CodeArgument Return
        {
            get { return _return; }
            set { this.RaiseAndSetIfChanged(ref _return, value); }
        }
        public CodeAccess Access
        {
            get { return _access; }
            set { this.RaiseAndSetIfChanged(ref _access, value); }
        }
        public ObservableCollection<CodeArgument> Arguments
        {
            get { return _arguments; }
            set { this.RaiseAndSetIfChanged(ref _arguments, value); }
        }

        public CodeFunction()
        {
            this.Name = "";
            this.Prefix = "";
            this.Modifier = CodeModifier.None;
            this.Access = CodeAccess.Public;
            this.Return = new CodeArgument();
            this.Arguments = new ObservableCollection<CodeArgument>();
        }
    }
}

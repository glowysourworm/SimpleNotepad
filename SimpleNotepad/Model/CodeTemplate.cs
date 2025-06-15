using SimpleWpf.Extensions;

namespace SimpleNotepad.Model
{
    public class CodeTemplate : ViewModelBase
    {
        string _constraint;
        string _variable;

        CodeTemplateType _type;

        public string Constraint
        {
            get { return _constraint; }
            set { this.RaiseAndSetIfChanged(ref _constraint, value); }
        }
        public string Variable
        {
            get { return _variable; }
            set { this.RaiseAndSetIfChanged(ref _variable, value); }
        }
        public CodeTemplateType Type
        {
            get { return _type; }
            set { this.RaiseAndSetIfChanged(ref _type, value); }
        }

        public CodeTemplate()
        {
            this.Constraint = "";
            this.Variable = "";
            this.Type = CodeTemplateType.Class;
        }

        public override string ToString()
        {
            return string.Format("template<{0}, {1}>", this.Constraint, this.Variable);
        }
    }
}

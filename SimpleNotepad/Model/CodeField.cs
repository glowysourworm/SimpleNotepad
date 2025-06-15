using SimpleNotepad.Component;

using SimpleWpf.Extensions;

namespace SimpleNotepad.Model
{
    public class CodeField : ViewModelBase
    {
        string _type;
        string _name;

        CodeModifier _modifier;
        CodeFieldType _fieldType;
        CodeAccess _access;

        string _boundedArrayLengthVariable;

        public string Type
        {
            get { return _type; }
            set { this.RaiseAndSetIfChanged(ref _type, value); }
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
        public CodeFieldType FieldType
        {
            get { return _fieldType; }
            set { this.RaiseAndSetIfChanged(ref _fieldType, value); }
        }
        public CodeAccess Access
        {
            get { return _access; }
            set { this.RaiseAndSetIfChanged(ref _access, value); }
        }
        public string BoundedArrayLengthVariable
        {
            get { return _boundedArrayLengthVariable; }
            set { this.RaiseAndSetIfChanged(ref _boundedArrayLengthVariable, value); }
        }

        public CodeField()
        {
            this.Name = "";
            this.Type = "";
            this.Modifier = CodeModifier.None;
            this.FieldType = CodeFieldType.Value;
            this.Access = CodeAccess.Private;
            this.BoundedArrayLengthVariable = "";
        }

        public override string ToString()
        {
            return CPlusPlusCodeGenerator.GenerateField(this);
        }
    }
}

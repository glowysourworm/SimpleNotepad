using SimpleNotepad.Component;

namespace SimpleNotepad.Model
{
    public class CodeArgument : CodeField
    {
        CodeArgumentType _argumentType;

        public CodeArgumentType ArgumentType
        {
            get { return _argumentType; }
            set { this.RaiseAndSetIfChanged(ref _argumentType, value); }
        }
        public CodeArgumentType DefaultArgumentType
        {
            get
            {
                switch (this.FieldType)
                {
                    case CodeFieldType.BoundedArray:
                        return CodeArgumentType.BoundedArray;
                    case CodeFieldType.UnboundedArray:
                        return CodeArgumentType.UnboundedArray;
                    case CodeFieldType.Pointer:
                        return CodeArgumentType.Pointer;
                    case CodeFieldType.Value:
                        return CodeArgumentType.Reference;
                    default:
                        throw new Exception("Unhandled CodeFieldType:  CodeArgument.cs");
                }
            }
        }
        public CodeArgumentType DefaultReturnType
        {
            get
            {
                switch (this.FieldType)
                {
                    case CodeFieldType.BoundedArray:
                        return CodeArgumentType.BoundedArray;
                    case CodeFieldType.UnboundedArray:
                        return CodeArgumentType.UnboundedArray;
                    case CodeFieldType.Pointer:
                        return CodeArgumentType.Pointer;
                    case CodeFieldType.Value:
                        return CodeArgumentType.Value;
                    default:
                        throw new Exception("Unhandled CodeFieldType:  CodeArgument.cs");
                }
            }
        }

        public CodeArgument()
        {
            this.ArgumentType = CodeArgumentType.Value;
        }

        public override string ToString()
        {
            return CPlusPlusCodeGenerator.GenerateFieldAsReturn(this);
        }
    }
}

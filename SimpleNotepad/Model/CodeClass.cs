using System.Collections.ObjectModel;
using System.Windows.Input;

using SimpleNotepad.Component;

using SimpleWpf.Extensions;
using SimpleWpf.Extensions.Command;

namespace SimpleNotepad.Model
{
    public class CodeClass : ViewModelBase
    {
        string _name;
        string _namespace;

        bool _isContainer;

        ObservableCollection<CodeTemplate> _containerTemplates;
        ObservableCollection<CodeField> _fields;
        ObservableCollection<CodeFunction> _functions;
        ObservableCollection<CodeFunction> _defaultFunctions;

        ICommand _addFieldCommand;
        ICommand _addFunctionCommand;
        ICommand _addTemplateCommand;
        ICommand _createDefaultFunctionsCommand;

        public string Name
        {
            get { return _name; }
            set { this.RaiseAndSetIfChanged(ref _name, value); }
        }
        public string Namespace
        {
            get { return _namespace; }
            set { this.RaiseAndSetIfChanged(ref _namespace, value); }
        }
        public bool IsContainer
        {
            get { return _isContainer; }
            set { this.RaiseAndSetIfChanged(ref _isContainer, value); }
        }

        public ObservableCollection<CodeTemplate> ContainerTemplates
        {
            get { return _containerTemplates; }
            set { this.RaiseAndSetIfChanged(ref _containerTemplates, value); }
        }
        public ObservableCollection<CodeField> Fields
        {
            get { return _fields; }
            set { this.RaiseAndSetIfChanged(ref _fields, value); }
        }
        public ObservableCollection<CodeFunction> Functions
        {
            get { return _functions; }
            set { this.RaiseAndSetIfChanged(ref _functions, value); }
        }
        public ObservableCollection<CodeFunction> DefaultFunctions
        {
            get { return _defaultFunctions; }
            set { this.RaiseAndSetIfChanged(ref _defaultFunctions, value); }
        }

        public ICommand AddFieldCommand
        {
            get { return _addFieldCommand; }
            set { this.RaiseAndSetIfChanged(ref _addFieldCommand, value); }
        }
        public ICommand AddFunctionCommand
        {
            get { return _addFunctionCommand; }
            set { this.RaiseAndSetIfChanged(ref _addFunctionCommand, value); }
        }
        public ICommand AddTemplateCommand
        {
            get { return _addTemplateCommand; }
            set { this.RaiseAndSetIfChanged(ref _addTemplateCommand, value); }
        }
        public ICommand CreateDefaultFunctionsCommand
        {
            get { return _createDefaultFunctionsCommand; }
            set { this.RaiseAndSetIfChanged(ref _createDefaultFunctionsCommand, value); }
        }

        public void CreateDefaultFunctions()
        {
            // Default Ctor
            this.DefaultFunctions.Add(new CodeFunction()
            {
                Name = this.Name,
                Return = null,
                Access = CodeAccess.Public
            });

            // Copy Ctor
            this.DefaultFunctions.Add(new CodeFunction()
            {
                Name = this.Name,
                Return = null,
                Access = CodeAccess.Public,
                Arguments = new ObservableCollection<CodeArgument>()
                {
                    new CodeArgument()
                    {
                        ArgumentType = CodeArgumentType.Reference,
                        Modifier = CodeModifier.Const,
                        Name = "copy",
                        Type = this.Name
                    }
                }
            });

            // Destructor
            this.DefaultFunctions.Add(new CodeFunction()
            {
                Name = this.Name,
                Return = null,
                Access = CodeAccess.Public,
                Prefix = "~"
            });

            // Public Getters
            foreach (var field in _fields.Where(x => x.Access == CodeAccess.Private))
            {
                this.DefaultFunctions.Add(new CodeFunction()
                {
                    Name = "get" + field.Name,
                    Access = CodeAccess.Public,
                    Return = CPlusPlusCodeGenerator.CreateDefaultReturnArgument(field)
                });
            }
        }

        public CodeClass()
        {
            this.Name = "";
            this.Namespace = "";
            this.IsContainer = false;

            this.ContainerTemplates = new ObservableCollection<CodeTemplate>();
            this.Fields = new ObservableCollection<CodeField>();
            this.Functions = new ObservableCollection<CodeFunction>();
            this.DefaultFunctions = new ObservableCollection<CodeFunction>();

            this.AddFieldCommand = new SimpleCommand(() =>
            {
                this.Fields.Add(new CodeField());
            });
            this.AddFunctionCommand = new SimpleCommand(() =>
            {
                this.Functions.Add(new CodeFunction());
            });
            this.AddTemplateCommand = new SimpleCommand(() =>
            {
                this.ContainerTemplates.Add(new CodeTemplate());
            });
            this.CreateDefaultFunctionsCommand = new SimpleCommand(() =>
            {
                this.CreateDefaultFunctions();
            });
        }
    }
}

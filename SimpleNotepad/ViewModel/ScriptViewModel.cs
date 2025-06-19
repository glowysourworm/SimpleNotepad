using System.Collections.ObjectModel;
using System.Reflection;
using System.Text.Json.Serialization;

using Microsoft.CodeAnalysis;

using SimpleNotepad.Component;

using SimpleWpf.Extensions;
using SimpleWpf.Extensions.Command;
using SimpleWpf.SimpleCollections.Collection;

namespace SimpleNotepad.ViewModel
{
    public class ScriptViewModel : ViewModelBase
    {
        string _code;
        string _name;
        bool _isCompiled;
        ObservableCollection<string> _publicMethods;
        Assembly _compiledAssembly;
        SimpleDictionary<string, MethodInfo> _publicMethodDict;
        SimpleCommand _compileCommand;

        public string Code
        {
            get { return _code; }
            set { this.RaiseAndSetIfChanged(ref _code, value); }
        }
        public string Name
        {
            get { return _name; }
            set { this.RaiseAndSetIfChanged(ref _name, value); }
        }

        [JsonIgnore]
        public bool IsCompiled
        {
            get { return _isCompiled; }
            set { this.RaiseAndSetIfChanged(ref _isCompiled, value); }
        }

        [JsonIgnore]
        public ObservableCollection<string> PublicMethods
        {
            get { return _publicMethods; }
            set { this.RaiseAndSetIfChanged(ref _publicMethods, value); }
        }

        [JsonIgnore]
        public SimpleCommand CompileCommand
        {
            get { return _compileCommand; }
            set { this.RaiseAndSetIfChanged(ref _compileCommand, value); }
        }

        public void Compile(out IEnumerable<Diagnostic> diagnostics, out string errorMessage)
        {
            IEnumerable<MethodInfo> publicMethods = null;

            _compiledAssembly = CSharpCompiler.Compile(_code, out publicMethods, out diagnostics, out errorMessage);

            this.IsCompiled = publicMethods != null;

            this.PublicMethods.Clear();
            _publicMethodDict.Clear();

            if (publicMethods != null)
            {
                foreach (var methodInfo in publicMethods)
                {
                    this.PublicMethods.Add(methodInfo.Name);
                    _publicMethodDict.Add(methodInfo.Name, methodInfo);
                }
            }
            else
            {
                _compiledAssembly = null;
            }
        }

        public string Execute(string methodName, string inputString, out string errorMessage)
        {
            if (!this.IsCompiled)
                throw new Exception("Must compile the script before trying to execute:  ScriptViewModel.cs");

            if (!_publicMethodDict.ContainsKey(methodName))
                throw new ArgumentException("Public method not found for script:  " + this.Name);

            errorMessage = string.Empty;

            try
            {
                // Create an instance of the script's compiled class
                var instance = _compiledAssembly.CreateInstance(_publicMethodDict[methodName].DeclaringType.FullName);

                // Invoke the public method requested by the user
                return (string)_publicMethodDict[methodName].Invoke(instance, new object[] { inputString });
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return inputString;
            }
        }

        public ScriptViewModel()
        {
            this.Code = string.Empty;
            this.Name = string.Empty;
            this.IsCompiled = false;
            this.PublicMethods = new ObservableCollection<string>();

            this.CompileCommand = new SimpleCommand(() =>
            {
                IEnumerable<Diagnostic> diagnostics = null;
                string errorMessage = string.Empty;

                Compile(out diagnostics, out errorMessage);
            });

            _compiledAssembly = null;
            _publicMethodDict = new SimpleDictionary<string, MethodInfo>();
        }
    }
}

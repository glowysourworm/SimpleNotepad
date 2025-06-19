using System.Reflection;
using System.Windows.Controls;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

using SimpleNotepad.Component;
using SimpleNotepad.ViewModel;

using SimpleWpf.Extensions;
using SimpleWpf.Extensions.Collection;

namespace SimpleNotepad.View
{
    public partial class ScriptMainView : UserControl
    {
        public ScriptMainView()
        {
            InitializeComponent();
        }

        private void CompileButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var scriptViewModel = this.ScriptLB.SelectedItem as ScriptViewModel;

            if (scriptViewModel != null)
            {
                IEnumerable<MethodInfo> publicMethods;
                IEnumerable<Diagnostic> diagnostics;
                string errorMessage = string.Empty;

                var assembly = CSharpCompiler.Compile(scriptViewModel.Code, out publicMethods, out diagnostics, out errorMessage);
                var anyCompilationErrors = diagnostics.Any(x => x.Severity == DiagnosticSeverity.Error);

                if (!string.IsNullOrEmpty(errorMessage))
                {
                    this.OutputLB.Items.Add("Error compiling C# source:  " + errorMessage);
                    return;
                }

                this.OutputLB.Items.Clear();

                if (anyCompilationErrors)
                {
                    foreach (Diagnostic diagnostic in diagnostics.Where(x => x.Severity == DiagnosticSeverity.Error))
                    {
                        this.OutputLB.Items.Add(diagnostic.ToString());
                    }
                }
                else
                {
                    var type = assembly.GetType("SimpleNotepadUserMethods.UserMethods");
                    var instance = assembly.CreateInstance("SimpleNotepadUserMethods.UserMethods");
                    var methods = type.GetMethods();

                    this.OutputLB.Items.Add("The following public methods have been created:  " + methods.Join(", ", x => x.Name));
                }
            }
        }
    }
}

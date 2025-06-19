using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Input;

using EMA.ExtendedWPFVisualTreeHelper;

using SimpleNotepad.View;
using SimpleNotepad.ViewModel;

using SimpleWpf.Extensions.Collection;

using static System.Net.Mime.MediaTypeNames;

namespace SimpleNotepad
{
    public partial class MainWindow : Window
    {
        public const string CONFIG_FILE = ".SimpleNotepad";

        public MainWindow()
        {
            InitializeComponent();

            // Manage view model on docking changed events (want access to the active "tab")
            this.DockingManager.ActiveContentChanged += DockingManager_ActiveContentChanged;

            this.Loaded += MainWindow_Loaded;
            this.Closing += MainWindow_Closing;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                var configuration = System.IO.Path.Combine(Environment.CurrentDirectory, CONFIG_FILE);
                var json = File.ReadAllText(configuration);
                var viewModel = System.Text.Json.JsonSerializer.Deserialize<MainViewModel>(json, new System.Text.Json.JsonSerializerOptions()
                {
                    AllowTrailingCommas = false,
                    WriteIndented = true
                });

                if (viewModel == null ||
                   !viewModel.IsValid())
                    viewModel = MainViewModel.CreateDefault();

                viewModel.StopMacroEvent += OnStopMacroEvent;
                viewModel.RecordMacroEvent += OnRecordMacroEvent;
                viewModel.PlayMacroEvent += OnPlayMacroEvent;
                viewModel.PlayRestMacroEvent += OnPlayRestMacroEvent;
                viewModel.PlaySyntaxTemplateEvent += OnPlaySyntaxTemplateEvent;
                viewModel.PlayRestSyntaxTemplateEvent += OnPlayRestSyntaxTemplateEvent;
                viewModel.PlayScriptEvent += OnPlayScriptEvent;
                viewModel.PlayRestScriptEvent += OnPlayRestScriptEvent;

                this.DataContext = viewModel;
            }
            catch (Exception ex)
            {
                // TODO: LOG

                if (this.DataContext == null)
                {
                    var viewModel = MainViewModel.CreateDefault();

                    viewModel.StopMacroEvent += OnStopMacroEvent;
                    viewModel.RecordMacroEvent += OnRecordMacroEvent;
                    viewModel.PlayMacroEvent += OnPlayMacroEvent;
                    viewModel.PlayRestMacroEvent += OnPlayRestMacroEvent;
                    viewModel.PlaySyntaxTemplateEvent += OnPlaySyntaxTemplateEvent;
                    viewModel.PlayRestSyntaxTemplateEvent += OnPlayRestSyntaxTemplateEvent;
                    viewModel.PlayScriptEvent += OnPlayScriptEvent;
                    viewModel.PlayRestScriptEvent += OnPlayRestScriptEvent;

                    this.DataContext = viewModel;
                }
            }
        }

        private void MainWindow_Closing(object? sender, CancelEventArgs e)
        {
            try
            {
                var viewModel = this.DataContext as MainViewModel;
                var json = System.Text.Json.JsonSerializer.Serialize<MainViewModel>(viewModel, new System.Text.Json.JsonSerializerOptions()
                {
                    AllowTrailingCommas = false,
                    WriteIndented = true
                });
                var configuration = System.IO.Path.Combine(Environment.CurrentDirectory, CONFIG_FILE);

                File.WriteAllText(configuration, json);
            }
            catch (Exception ex)
            {
                // TODO: LOG
            }
        }

        private void DockingManager_ActiveContentChanged(object? sender, EventArgs e)
        {
            // Stop Macro Recording
            var viewModel = this.DataContext as MainViewModel;

            viewModel?.CancelRecording();
        }

        private void OnRecordMacroEvent(DocumentViewModel item1)
        {
            // Enter any UI changes
        }

        private void OnStopMacroEvent(DocumentViewModel viewModel)
        {
            // Enter any UI changes
        }

        private void OnPlayMacroEvent(DocumentViewModel sender, MacroViewModel macro)
        {
            var view = this.DockingManager
                           .FindAllChildren<DocumentView>()
                           .FirstOrDefault(x => x.DataContext == sender);

            if (view != null)
            {
                // Times To Run:  One Time
                foreach (var keyStroke in macro.KeyStrokes)
                {
                    //view.Editor.TextArea.RaiseEvent(new System.Windows.Input.KeyEventArgs(InputManager.Current.PrimaryKeyboardDevice,
                    //                                                             PresentationSource.FromDependencyObject(view.Editor.TextArea),
                    //                                                             (int)DateTime.UtcNow.Ticks,
                    //                                                             keyStroke.Key)
                    //{
                    //    RoutedEvent = UIElement.PreviewKeyDownEvent,
                    //    Source = view.Editor.TextArea,
                    //    Handled = false,
                    //});
                }
            }
        }

        private void OnPlayRestMacroEvent(DocumentViewModel sender, MacroViewModel macro)
        {

        }

        private void OnPlaySyntaxTemplateEvent(DocumentViewModel sender, SyntaxTemplateViewModel template)
        {
            ProcessForCurrentLine(sender, (inputText) =>
            {
                return SubstituteCurrentLine(inputText, template);
            });
        }

        private void OnPlayRestSyntaxTemplateEvent(DocumentViewModel sender, SyntaxTemplateViewModel template)
        {
            ProcessUntilEndOfFile(sender, (inputText) =>
            {
                return SubstituteCurrentLine(inputText, template);
            });
        }

        private void OnPlayRestScriptEvent(DocumentViewModel sender, ScriptViewModel script, string scriptMethod)
        {
            ProcessUntilEndOfFile(sender, (inputText) =>
            {
                var errorMessage = string.Empty;
                var outputText = script.Execute(scriptMethod, inputText, out errorMessage);

                return outputText;
            });
        }

        private void OnPlayScriptEvent(DocumentViewModel sender, ScriptViewModel script, string scriptMethod)
        {
            ProcessForCurrentLine(sender, (inputText) =>
            {
                var errorMessage = string.Empty;
                var outputText = script.Execute(scriptMethod, inputText, out errorMessage);

                return outputText;
            });
        }

        private void ProcessUntilEndOfFile(DocumentViewModel sender, Func<string, string> lineSubstitutionCallback)
        {
            var view = this.DockingManager
                           .FindAllChildren<DocumentView>()
                           .FirstOrDefault(x => x.DataContext == sender);

            if (view != null)
            {
                // Start BeginUpdate (holds binding / undo updates until we're finished)
                view.Editor.Document.BeginUpdate();

                var changes = new List<string>();

                foreach (var line in view.Editor.Document.Lines)
                {
                    // Get line of text
                    var text = view.Editor.Document.GetText(line.Offset, line.Length);

                    // Process line substitution
                    var outputText = lineSubstitutionCallback(text);

                    // Keep changes (these may be multi-line)
                    changes.Add(outputText);
                }

                view.Editor.Document.Text = changes.Join("\n", x => x);

                // End Update -> Apply Bindings / Undo
                view.Editor.Document.EndUpdate();
            }
        }

        private void ProcessForCurrentLine(DocumentViewModel sender, Func<string, string> lineSubstitutionCallback)
        {
            var view = this.DockingManager
                           .FindAllChildren<DocumentView>()
                           .FirstOrDefault(x => x.DataContext == sender);

            if (view != null)
            {
                // Start BeginUpdate (holds binding / undo updates until we're finished)
                view.Editor.Document.BeginUpdate();

                // Get current line from the caret offset
                var currentLine = view.Editor.Document.GetLineByOffset(view.Editor.CaretOffset);

                // Get line of text
                var text = view.Editor.Document.GetText(currentLine.Offset, currentLine.Length);

                // Process line substitution
                var outputText = lineSubstitutionCallback(text);

                // Replace current line text
                view.Editor.Document.Replace(currentLine.Offset, currentLine.Length, outputText);

                // End Update -> Apply Bindings / Undo
                view.Editor.Document.EndUpdate();
            }
        }

        private string SubstituteCurrentLine(string text, SyntaxTemplateViewModel template)
        {
            // Split the current line of text and use as input parameters of the template
            var parameterInputs = text.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            // TODO: Notify user that parameters don't match
            if (parameterInputs.Length != template.Parameters.Count)
            {
                return text;
            }

            var outputText = text;

            // Replace each parameter instance
            for (int index = 0; index < template.Parameters.Count; index++)
            {
                // Parameter name (by index)
                var parameterName = template.Parameters[index].Parameter;

                // Run Parameter
                if (index == 0)
                    outputText = template.TemplateBody.Replace("{" + parameterName + "}", parameterInputs[index]);

                else
                    outputText = outputText.Replace("{" + parameterName + "}", parameterInputs[index]);
            }

            return outputText;
        }
    }
}
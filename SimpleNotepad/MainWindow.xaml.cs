using System.IO;
using System.Windows;
using System.Xml;

using AvalonDock.Themes;

using EMA.ExtendedWPFVisualTreeHelper;

using SimpleNotepad.View;
using SimpleNotepad.ViewModel;

namespace SimpleNotepad
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var viewModel = new MainViewModel();

            viewModel.PlaySyntaxTemplateEvent += OnPlaySyntaxTemplateEvent;

            this.DataContext = viewModel;
        }

        private void OnPlaySyntaxTemplateEvent(DocumentViewModel sender, SyntaxTemplateViewModel template)
        {
            var view = this.DockingManager
                           .FindAllChildren<DocumentView>()
                           .FirstOrDefault(x => x.DataContext == sender);

            if (view != null)
            {
                foreach (var line in view.Editor.Document.Lines)
                {
                    // Start BeginUpdate (holds binding / undo updates until we're finished)
                    //view.Editor.Document.BeginUpdate();

                    // Get line of text
                    var text = view.Editor.Document.GetText(line.Offset, line.Length);

                    // Split the current line of text and use as input parameters of the template
                    var parameterInputs = text.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

                    if (parameterInputs.Length != template.Parameters.Count)
                        throw new Exception("Parameter count mismatch. Cannot apply template. Please be sure that the number of parameters per line matches the template.");

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

                    view.Editor.Document.Replace(line.Offset, line.Length, outputText);

                    // End Update -> Apply Bindings / Undo
                    //view.Editor.Document.EndUpdate();
                }
            }
        }
    }
}
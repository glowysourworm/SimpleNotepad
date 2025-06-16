using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Shapes;

using EMA.ExtendedWPFVisualTreeHelper;

using SimpleNotepad.View;
using SimpleNotepad.ViewModel;

using SimpleWpf.Extensions.Collection;

namespace SimpleNotepad
{
    public partial class MainWindow : Window
    {
        public const string CONFIG_FILE = ".SimpleNotepad";

        public MainWindow()
        {
            InitializeComponent();

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

                viewModel.PlaySyntaxTemplateEvent += OnPlaySyntaxTemplateEvent;

                this.DataContext = viewModel;
            }
            catch (Exception ex)
            {
                // TODO: LOG

                if (this.DataContext == null)
                    this.DataContext = MainViewModel.CreateDefault();
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

        private void OnPlaySyntaxTemplateEvent(DocumentViewModel sender, SyntaxTemplateViewModel template)
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

                    // Split the current line of text and use as input parameters of the template
                    var parameterInputs = text.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

                    // TODO: Notify user that parameters don't match
                    if (parameterInputs.Length != template.Parameters.Count)
                    {
                        changes.Add(text);
                        continue;
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

                    changes.Add(outputText);
                }

                view.Editor.Document.Text = changes.Join("\n", x => x);

                // End Update -> Apply Bindings / Undo
                view.Editor.Document.EndUpdate();
            }
        }
    }
}
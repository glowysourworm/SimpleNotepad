using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

using AvalonDock.Controls;

using ICSharpCode.AvalonEdit.Editing;

using SimpleNotepad.View.AvalonEditExtension;
using SimpleNotepad.ViewModel;

using SimpleWpf.Extensions;

namespace SimpleNotepad.View
{
    public partial class DocumentView : UserControl
    {
        // NOTE*** The TextEditor must not set the text property more than once (it's out of sync with the undo stack).
        //         So, watch for binding issues.
        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register("Source", typeof(string), typeof(DocumentView), new PropertyMetadata(OnSourceChanged));

        public static readonly DependencyProperty IsChangedProperty =
            DependencyProperty.Register("IsChanged", typeof(bool), typeof(DocumentView));

        public static readonly DependencyProperty IsRecordingProperty =
            DependencyProperty.Register("IsRecording", typeof(bool), typeof(DocumentView));

        public string Source
        {
            get { return (string)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }
        public bool IsChanged
        {
            get { return (bool)GetValue(IsChangedProperty); }
            set { SetValue(IsChangedProperty, value); }
        }
        public bool IsRecording
        {
            get { return (bool)GetValue(IsRecordingProperty); }
            set { SetValue(IsRecordingProperty, value); }
        }

        public DocumentView()
        {
            InitializeComponent();

            this.Editor.SyntaxHighlighting = HL.Manager.ThemedHighlightingManager.Instance.GetDefinition("C#");
            this.Editor
                .TextArea
                .TextView
                .BackgroundRenderers
                .Add(new HighlightCurrentLineBackgroundRenderer(this.Editor, System.Windows.Media.Color.FromArgb(20, 0, 100, 250)));

            this.DataContextChanged += DocumentView_DataContextChanged;
        }

        private void DocumentView_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var viewModel = e.NewValue as DocumentViewModel;

            if (viewModel != null)
            {
                this.Editor.Text = viewModel.Contents;
            }
        }

        private void UpdateFromSource()
        {
            if (this.Editor.Text != this.Source)
                this.Editor.Text = this.Source;
        }

        // Editor binding -> Their Data
        private void Editor_TextChanged(object sender, EventArgs e)
        {
            var viewModel = this.DataContext as DocumentViewModel;

            if (viewModel != null &&
                this.Source != null &&
                this.Source != this.Editor.Text)
            {
                viewModel.IsDirty = true;
                this.IsChanged = true;

                // Binding could take care of this part
                this.Source = this.Editor.Text;
            }
            else
            {
                this.Source = this.Editor.Text;
            }
        }

        // Macro Recording -> Log Keystroke
        private void Editor_KeyDown(object sender, KeyEventArgs e)
        {
            var viewModel = this.DataContext as DocumentViewModel;

            if (viewModel != null &&
                viewModel.IsRecordingKeystrokes &&
                !e.IsRepeat)
            {
                if (!e.Key.IsModifier())
                    viewModel.AddMacroKeystroke(e);
            }
        }

        // Source binding -> Our Control
        private static void OnSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var view = d as DocumentView;
            var source = e.NewValue as string;

            if (view != null)
            {
                view.UpdateFromSource();
            }
        }
    }
}

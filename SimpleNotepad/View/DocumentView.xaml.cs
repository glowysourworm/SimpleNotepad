using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace SimpleNotepad.View
{
    public partial class DocumentView : UserControl
    {
        // NOTE*** The TextEditor must not set the text property more than once (it's out of sync with the undo stack).
        //         So, watch for binding issues.
        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register("Source", typeof(string), typeof(DocumentView), new PropertyMetadata(OnSourceChanged));

        public string Source
        {
            get { return (string)GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        public DocumentView()
        {
            InitializeComponent();

            this.Editor.SyntaxHighlighting = HL.Manager.ThemedHighlightingManager.Instance.GetDefinition("C#");
        }

        private void UpdateFromSource()
        {
            if (this.Editor.Text != this.Source)
                this.Editor.Text = this.Source;
        }

        // Editor binding -> Their Data
        private void Editor_TextChanged(object sender, EventArgs e)
        {
            this.Source = this.Editor.Text;
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

using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

using AvalonDock.Controls;

using SimpleNotepad.ViewModel;

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

        public DocumentView()
        {
            InitializeComponent();

            this.Editor.SyntaxHighlighting = HL.Manager.ThemedHighlightingManager.Instance.GetDefinition("C#");

            this.DataContextChanged += DocumentView_DataContextChanged;
        }

        private void DocumentView_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var viewModel = e.NewValue as DocumentViewModel;

            if (viewModel != null)
                this.Editor.Text = viewModel.Contents;
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

                // Binding could take care of this part
                this.Source = this.Editor.Text;
            }
            else
            {
                this.Source = this.Editor.Text;
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

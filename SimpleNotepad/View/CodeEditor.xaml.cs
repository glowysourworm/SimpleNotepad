using System.Windows.Controls;
using System.Windows.Documents;

namespace SimpleNotepad.View
{
    public partial class CodeEditor : UserControl
    {
        public CodeEditor()
        {
            InitializeComponent();

            this.CodeRTB.Document.PageWidth = 3000;
        }
        private void CodeRTB_TextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {

        }

        private void CodeRTB_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            // Special key combinations
            if (e.Key == System.Windows.Input.Key.W && e.KeyboardDevice.Modifiers == System.Windows.Input.ModifierKeys.Control)
            {
                // Get text run from beginning of line to the end of line
                var startPointer = this.CodeRTB.CaretPosition.GetLineStartPosition(0);
                var endPointer = this.CodeRTB.CaretPosition.GetNextContextPosition(LogicalDirection.Forward);

                // BACKWARDS WORKING ONLY!!! (???)
                var length = endPointer.GetTextRunLength(LogicalDirection.Backward);
                var lineOfText = endPointer.GetTextInRun(LogicalDirection.Backward);

                // Substitute Line (remove white space)
                lineOfText = lineOfText.Replace("//", "#").Replace("NULL", "0").Replace("\t", "").Replace("  ", "").Replace("&", "");
                //lineOfText = lineOfText.Replace("//", "#").Replace(",", "");

                // Delete line
                endPointer.DeleteTextInRun(-1 * length);

                // Insert line under caret
                this.CodeRTB.CaretPosition.GetLineStartPosition(0).InsertTextInRun(lineOfText);

                // TODO: FIX THIS ONE FOR THE LAST LINE!
                if (this.CodeRTB.CaretPosition.GetNextContextPosition(LogicalDirection.Forward) == null)
                    this.CodeRTB.CaretPosition = this.CodeRTB.CaretPosition.GetLineStartPosition(0);

                else
                    this.CodeRTB.CaretPosition = this.CodeRTB.CaretPosition.GetLineStartPosition(1);

                e.Handled = true;
            }
        }
    }
}

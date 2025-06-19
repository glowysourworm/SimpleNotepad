using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

using ICSharpCode.AvalonEdit.Editing;

namespace SimpleNotepad.View.AvalonEditExtension
{
    public class TextAreaStackedInputAutomationHandler : TextAreaStackedInputHandler
    {
        public void SetPlayback()
        {

        }
        public void ClearPlayback()
        {

        }

        public TextAreaStackedInputAutomationHandler(TextArea textArea) : base(textArea)
        {
        }

        public override void OnPreviewKeyDown(KeyEventArgs e)
        {
            
        }

        public override void OnPreviewKeyUp(KeyEventArgs e)
        {
            
        }

        public override string? ToString()
        {
            return base.ToString();
        }
    }
}

using System.Windows.Controls;

namespace SimpleNotepad.View
{
    public partial class CPlusPlusView : UserControl
    {
        public CPlusPlusView()
        {
            InitializeComponent();

            this.ClassView.CodeGeneratedEvent += OnCodeGenerated;
        }

        private void OnCodeGenerated(string hppCode)
        {
            //this.ClassEditor.TextSource = hppCode;
        }
    }
}

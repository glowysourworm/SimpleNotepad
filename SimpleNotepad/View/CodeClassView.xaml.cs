using System.Windows;
using System.Windows.Controls;

using SimpleNotepad.Component;
using SimpleNotepad.Model;

using SimpleWpf.Extensions.Event;

namespace SimpleNotepad.View
{
    public partial class CodeClassView : UserControl
    {
        public SimpleEventHandler<string> CodeGeneratedEvent;

        public CodeClassView()
        {
            InitializeComponent();
        }

        private void CreateCPPButton_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = this.DataContext as CodeClass;

            if (viewModel != null)
            {
                var hpp = CPlusPlusCodeGenerator.CreateClass(viewModel);

                if (this.CodeGeneratedEvent != null)
                    this.CodeGeneratedEvent(hpp);
            }
        }
    }
}

using System.Collections.ObjectModel;

namespace SimpleNotepad.ViewModel
{
    public class SyntaxTemplateMainViewModel : DockingManagerItemViewModel
    {
        ObservableCollection<SyntaxTemplateViewModel> _templates;

        public ObservableCollection<SyntaxTemplateViewModel> Templates
        {
            get { return _templates; }
            set { this.RaiseAndSetIfChanged(ref _templates, value); }
        }

        public SyntaxTemplateMainViewModel() : base("Syntax Templates")
        {
            this.Templates = new ObservableCollection<SyntaxTemplateViewModel>();
        }
    }
}

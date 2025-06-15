using System.Collections.ObjectModel;

namespace SimpleNotepad.Model
{
    public class CodeContainerField : CodeField
    {
        ObservableCollection<string> _templates;

        public ObservableCollection<string> Templates
        {
            get { return _templates; }
            set { this.RaiseAndSetIfChanged(ref _templates, value); }
        }

        public CodeContainerField()
        {
            this.Templates = new ObservableCollection<string>();
        }
    }
}

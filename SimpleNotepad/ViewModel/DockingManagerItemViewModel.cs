using SimpleWpf.Extensions;

namespace SimpleNotepad.ViewModel
{
    public class DockingManagerItemViewModel : ViewModelBase
    {
        string _header;

        public string Header
        {
            get { return _header; }
        }

        public DockingManagerItemViewModel(string header)
        {
            _header = header;
        }
    }
}

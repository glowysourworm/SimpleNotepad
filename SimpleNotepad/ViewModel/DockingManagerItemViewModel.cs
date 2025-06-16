using SimpleWpf.Extensions;

namespace SimpleNotepad.ViewModel
{
    public class DockingManagerItemViewModel : ViewModelBase
    {
        string _header;
        bool _isDirty;
        bool _isSelected;

        public string Header
        {
            get { return _isDirty ? _header + "*" : _header; }
        }
        public bool IsDirty
        {
            get { return _isDirty; }
            set { this.RaiseAndSetIfChanged(ref _isDirty, value); OnPropertyChanged("Header"); }
        }
        public bool IsSelected
        {
            get { return _isSelected; }
            set { this.RaiseAndSetIfChanged(ref _isSelected, value); }
        }
        public DockingManagerItemViewModel(string header)
        {
            _header = header;
        }
    }
}

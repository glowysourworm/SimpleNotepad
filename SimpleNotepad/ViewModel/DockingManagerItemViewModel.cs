using System.Text.Json.Serialization;

using SimpleWpf.Extensions;

namespace SimpleNotepad.ViewModel
{    
    [JsonDerivedType(typeof(DockingManagerItemViewModel), typeDiscriminator: "base")]
    [JsonDerivedType(typeof(ScriptMainViewModel), typeDiscriminator: "Scripts")]
    [JsonDerivedType(typeof(DocumentViewModel), typeDiscriminator: "FileName")]
    [JsonDerivedType(typeof(SyntaxTemplateMainViewModel), typeDiscriminator: "Templates")]
    public class DockingManagerItemViewModel : ViewModelBase
    {
        string _header;
        bool _isDirty;
        bool _isSelected;

        public string Header
        {
            get { return _isDirty ? _header + "*" : _header; }
            set { _header = value?.Replace("*", "") ?? string.Empty; }
        }
        [JsonIgnore]
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
        public DockingManagerItemViewModel()
        {
            this.Header = string.Empty;
        }
    }
}

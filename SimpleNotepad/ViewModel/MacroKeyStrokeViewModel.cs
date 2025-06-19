using System.Windows.Input;

using SimpleWpf.Extensions;

namespace SimpleNotepad.ViewModel
{
    public class MacroKeyStrokeViewModel : ViewModelBase
    {
        Key _key;
        ModifierKeys _modifiers;

        public Key Key
        {
            get { return _key; }
            set { this.RaiseAndSetIfChanged(ref _key, value); }
        }
        public ModifierKeys Modifiers
        {
            get { return _modifiers; }
            set { this.RaiseAndSetIfChanged(ref _modifiers, value); }
        }

        public MacroKeyStrokeViewModel()
        {
        }
    }
}

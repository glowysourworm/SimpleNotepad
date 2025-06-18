using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SimpleWpf.Extensions;

namespace SimpleNotepad.ViewModel
{
    public class MacroViewModel : ViewModelBase
    {
        string _name;
        ObservableCollection<MacroKeyStrokeViewModel> _keyStrokes;

        public string Name
        {
            get { return _name; }
            set { this.RaiseAndSetIfChanged(ref _name, value); }
        }
        public ObservableCollection<MacroKeyStrokeViewModel> KeyStrokes
        {
            get { return _keyStrokes; }
            set { this.RaiseAndSetIfChanged(ref _keyStrokes, value); }
        }

        public MacroViewModel()
        {
            this.Name = string.Empty;
            this.KeyStrokes = new ObservableCollection<MacroKeyStrokeViewModel>();
        }
    }
}

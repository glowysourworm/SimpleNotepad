using SimpleNotepad.Model;

using SimpleWpf.Extensions;

namespace SimpleNotepad.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        CPlusPlusViewModel _cplusPlus;

        public CPlusPlusViewModel CPlusPlus
        {
            get { return _cplusPlus; }
            set { this.RaiseAndSetIfChanged(ref _cplusPlus, value); }
        }

        public MainViewModel()
        {
            this.CPlusPlus = new CPlusPlusViewModel();
        }
    }
}

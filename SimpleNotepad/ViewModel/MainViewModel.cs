using System.Collections.ObjectModel;

using SimpleNotepad.Model;

using SimpleWpf.Extensions;

namespace SimpleNotepad.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        CPlusPlusViewModel _cplusPlus;

        ObservableCollection<DocumentViewModel> _documents;

        public CPlusPlusViewModel CPlusPlus
        {
            get { return _cplusPlus; }
            set { this.RaiseAndSetIfChanged(ref _cplusPlus, value); }
        }
        public ObservableCollection<DocumentViewModel> Documents
        {
            get { return _documents; }
            set { this.RaiseAndSetIfChanged(ref _documents, value); }
        }

        public MainViewModel()
        {
            this.CPlusPlus = new CPlusPlusViewModel();
            this.Documents = new ObservableCollection<DocumentViewModel>()
            {
                new DocumentViewModel()
                {
                    Title = "New File",
                    Contents = "Document Contents"
                }
            };
        }
    }
}

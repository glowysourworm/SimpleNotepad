using SimpleWpf.Extensions;
using SimpleWpf.Extensions.Command;

namespace SimpleNotepad.ViewModel
{
    public class DocumentViewModel : DockingManagerItemViewModel
    {
        string _fileName;
        string _contents;
        bool _isDirty;
        SimpleCommand _closeCommand;

        public string FileName
        {
            get { return _fileName; }
            set { this.RaiseAndSetIfChanged(ref _fileName, value); }
        }
        public string Contents
        {
            get { return _contents; }
            set { this.RaiseAndSetIfChanged(ref _contents, value); }
        }
        public bool IsDirty
        {
            get { return _isDirty; }
            set { this.RaiseAndSetIfChanged(ref _isDirty, value); }
        }
        public SimpleCommand CloseCommand
        {
            get { return _closeCommand; }
            set { this.RaiseAndSetIfChanged(ref _closeCommand, value); }
        }


        public DocumentViewModel(string fileName, bool isNew) : base(isNew ? "New File" : fileName)
        {
            this.FileName = string.Empty;
            this.Contents = string.Empty;
            this.IsDirty = true;
            this.CloseCommand = new SimpleCommand(() =>
            {

            });
        }
    }
}

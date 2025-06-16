using System.Text.Json.Serialization;

using SimpleWpf.Extensions.Command;

namespace SimpleNotepad.ViewModel
{
    public class DocumentViewModel : DockingManagerItemViewModel
    {
        string _fileName;
        string _contents;
        SimpleCommand _closeCommand;

        public string FileName
        {
            get { return _fileName; }
            set { this.RaiseAndSetIfChanged(ref _fileName, value); }
        }
        [JsonIgnore]
        public string Contents
        {
            get { return _contents; }
            set { this.RaiseAndSetIfChanged(ref _contents, value); }
        }

        [JsonIgnore]
        public SimpleCommand CloseCommand
        {
            get { return _closeCommand; }
            set { this.RaiseAndSetIfChanged(ref _closeCommand, value); }
        }


        public DocumentViewModel()
        {
            this.Header = "New Document";
            this.IsDirty = true;
            this.FileName = string.Empty;
            this.Contents = string.Empty;
            this.IsSelected = false;
            this.CloseCommand = new SimpleCommand(() =>
            {

            });
        }
    }
}

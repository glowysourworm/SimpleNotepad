using System.Collections.ObjectModel;
using System.Text.Json.Serialization;
using System.Windows.Input;

using SimpleWpf.Extensions.Command;

namespace SimpleNotepad.ViewModel
{
    public class DocumentViewModel : DockingManagerItemViewModel
    {
        bool _isRecordingKeystrokes;
        string _fileName;
        string _contents;
        SimpleCommand _closeCommand;

        List<MacroKeyStrokeViewModel> _macroKeystrokeLog;

        public string FileName
        {
            get { return _fileName; }
            set { this.RaiseAndSetIfChanged(ref _fileName, value); }
        }
        [JsonIgnore]
        public bool IsRecordingKeystrokes
        {
            get { return _isRecordingKeystrokes; }
            set { this.RaiseAndSetIfChanged(ref _isRecordingKeystrokes, value); }
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

        public void AddMacroKeystroke(KeyEventArgs args)
        {
            _macroKeystrokeLog.Add(new MacroKeyStrokeViewModel()
            {
                Key = args.Key,
                Modifiers = args.KeyboardDevice.Modifiers
            });
        }

        public void ClearMacroKeystrokes()
        {
            if (this.IsRecordingKeystrokes)
                throw new Exception("Trying to modify the macro buffer while recording keystrokes");

            _macroKeystrokeLog.Clear();
        }

        public IEnumerable<MacroKeyStrokeViewModel> GetMacroKeystrokes()
        {
            return _macroKeystrokeLog;
        }

        public DocumentViewModel()
        {
            this.Header = "New Document";
            this.IsDirty = true;
            this.IsRecordingKeystrokes = false;
            this.FileName = string.Empty;
            this.Contents = string.Empty;
            this.IsSelected = false;
            this.CloseCommand = new SimpleCommand(() =>
            {

            });

            _macroKeystrokeLog = new List<MacroKeyStrokeViewModel>();
        }
    }
}

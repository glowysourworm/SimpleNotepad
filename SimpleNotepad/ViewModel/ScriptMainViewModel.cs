using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

using SimpleWpf.Extensions.Command;

namespace SimpleNotepad.ViewModel
{
    public class ScriptMainViewModel : DockingManagerItemViewModel
    {
        ObservableCollection<ScriptViewModel> _scripts;

        SimpleCommand _addScriptCommand;

        public ObservableCollection<ScriptViewModel> Scripts
        {
            get { return _scripts; }
            set { this.RaiseAndSetIfChanged(ref _scripts, value); }
        }

        [JsonIgnore]
        public SimpleCommand AddScriptCommand
        {
            get { return _addScriptCommand; }
            set { this.RaiseAndSetIfChanged(ref _addScriptCommand, value); }
        }

        public ScriptMainViewModel()
        {
            this.Scripts = new ObservableCollection<ScriptViewModel>();
            this.Header = "Scripts";

            this.AddScriptCommand = new SimpleCommand(() =>
            {
                this.Scripts.Add(new ScriptViewModel()
                {
                    Name = "New Script",
                    Code = "Code Body C#"
                });
            });
        }
    }
}

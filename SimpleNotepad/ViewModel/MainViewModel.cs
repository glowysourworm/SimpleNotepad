using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Text.Json.Serialization;
using System.Windows.Forms;

using SimpleNotepad.View.Dialog;

using SimpleWpf.Extensions;
using SimpleWpf.Extensions.Command;
using SimpleWpf.Extensions.Event;

namespace SimpleNotepad.ViewModel
{
    public enum PlayMode
    {
        SyntaxTemplate,
        Macro,
        Script
    }

    public class MainViewModel : ViewModelBase
    {
        public event SimpleEventHandler<DocumentViewModel> RecordMacroEvent;
        public event SimpleEventHandler<DocumentViewModel> StopMacroEvent;
        public event SimpleEventHandler<DocumentViewModel, MacroViewModel> PlayRestMacroEvent;
        public event SimpleEventHandler<DocumentViewModel, MacroViewModel> PlayMacroEvent;
        public event SimpleEventHandler<DocumentViewModel, SyntaxTemplateViewModel> PlayRestSyntaxTemplateEvent;
        public event SimpleEventHandler<DocumentViewModel, SyntaxTemplateViewModel> PlaySyntaxTemplateEvent;
        public event SimpleEventHandler<DocumentViewModel, ScriptViewModel, string> PlayScriptEvent;                // sender, script, public method name
        public event SimpleEventHandler<DocumentViewModel, ScriptViewModel, string> PlayRestScriptEvent;

        ObservableCollection<DockingManagerItemViewModel> _dockingManagerItemsSource;
        ObservableCollection<MacroViewModel> _macros;

        MacroViewModel _selectedMacro;
        SyntaxTemplateViewModel _selectedSyntaxTemplate;
        ScriptViewModel _selectedScript;
        string _selectedScriptMethod;

        SimpleCommand _openCommand;
        SimpleCommand _saveCommand;
        SimpleCommand _saveAsCommand;
        SimpleCommand _saveTemplatesCommand;
        SimpleCommand _closeCommand;

        SimpleCommand _recordCommand;
        SimpleCommand _stopCommand;
        SimpleCommand _playCommand;
        SimpleCommand _playRestCommand;

        PlayMode _mode;
        bool _isRecording;
        bool _isPlayable;

        public ObservableCollection<DockingManagerItemViewModel> DockingManagerItemsSource
        {
            get { return _dockingManagerItemsSource; }
            set { this.RaiseAndSetIfChanged(ref _dockingManagerItemsSource, value); }
        }
        public ObservableCollection<MacroViewModel> Macros
        {
            get { return _macros; }
            set { this.RaiseAndSetIfChanged(ref _macros, value); }
        }
        public PlayMode Mode
        {
            get { return _mode; }
            set { this.RaiseAndSetIfChanged(ref _mode, value); SetPlayableFlags(); }
        }

        [JsonIgnore]
        public bool IsRecording
        {
            get { return _isRecording; }
            set { this.RaiseAndSetIfChanged(ref _isRecording, value); }
        }
        [JsonIgnore]
        public bool IsPlayable
        {
            get { return _isPlayable; }
            set { this.RaiseAndSetIfChanged(ref _isPlayable, value); }
        }

        [JsonIgnore]
        public ObservableCollection<SyntaxTemplateViewModel> SyntaxTemplates
        {
            get
            {
                return (this.DockingManagerItemsSource
                             .FirstOrDefault(x => x.GetType() == typeof(SyntaxTemplateMainViewModel)) as SyntaxTemplateMainViewModel)?.Templates;
            }
        }
        [JsonIgnore]
        public ObservableCollection<ScriptViewModel> Scripts
        {
            get
            {
                return (this.DockingManagerItemsSource
                             .FirstOrDefault(x => x.GetType() == typeof(ScriptMainViewModel)) as ScriptMainViewModel)?.Scripts;
            }
        }

        [JsonIgnore]
        public SimpleCommand RecordCommand
        {
            get { return _recordCommand; }
            set { this.RaiseAndSetIfChanged(ref _recordCommand, value); }
        }
        [JsonIgnore]
        public SimpleCommand StopCommand
        {
            get { return _stopCommand; }
            set { this.RaiseAndSetIfChanged(ref _stopCommand, value); }
        }
        [JsonIgnore]
        public SimpleCommand PlayCommand
        {
            get { return _playCommand; }
            set { this.RaiseAndSetIfChanged(ref _playCommand, value); }
        }
        [JsonIgnore]
        public SimpleCommand PlayRestCommand
        {
            get { return _playRestCommand; }
            set { this.RaiseAndSetIfChanged(ref _playRestCommand, value); }
        }

        [JsonIgnore]
        public SimpleCommand OpenCommand
        {
            get { return _openCommand; }
            set { this.RaiseAndSetIfChanged(ref _openCommand, value); }
        }
        [JsonIgnore]
        public SimpleCommand SaveCommand
        {
            get { return _saveCommand; }
            set { this.RaiseAndSetIfChanged(ref _saveCommand, value); }
        }
        [JsonIgnore]
        public SimpleCommand SaveAsCommand
        {
            get { return _saveAsCommand; }
            set { this.RaiseAndSetIfChanged(ref _saveAsCommand, value); }
        }
        [JsonIgnore]
        public SimpleCommand SaveTemplatesCommand
        {
            get { return _saveTemplatesCommand; }
            set { this.RaiseAndSetIfChanged(ref _saveTemplatesCommand, value); }
        }
        [JsonIgnore]
        public SimpleCommand CloseCommand
        {
            get { return _closeCommand; }
            set { this.RaiseAndSetIfChanged(ref _closeCommand, value); }
        }

        [JsonIgnore]
        public SyntaxTemplateViewModel SelectedSyntaxTemplate
        {
            get { return _selectedSyntaxTemplate; }
            set { this.RaiseAndSetIfChanged(ref _selectedSyntaxTemplate, value); SetPlayableFlags(); }
        }
        [JsonIgnore]
        public MacroViewModel SelectedMacro
        {
            get { return _selectedMacro; }
            set { this.RaiseAndSetIfChanged(ref _selectedMacro, value); SetPlayableFlags(); }
        }
        [JsonIgnore]
        public ScriptViewModel SelectedScript
        {
            get { return _selectedScript; }
            set { this.RaiseAndSetIfChanged(ref _selectedScript, value); SetPlayableFlags(); }
        }
        [JsonIgnore]
        public string SelectedScriptMethod
        {
            get { return _selectedScriptMethod; }
            set { this.RaiseAndSetIfChanged(ref _selectedScriptMethod, value); SetPlayableFlags(); }
        }


        public MainViewModel()
        {
            // Docking Manager Binding:  It appears that the items are based on a single collection. So, what probably
            //                           has to happen is that you create all the proper template selectors, and even
            //                           attached behaviors to intercept how to build the UI. Each of these will bind
            //                           to the proper view model type.

            var defaultDocument = new DocumentViewModel()
            {
                Header = "New Document",
                FileName = "(new file)",
                IsDirty = true,
                Contents = "Document Contents"
            };
            var syntaxTemplateMain = new SyntaxTemplateMainViewModel()
            {
                Header = "Syntax Templates",
                Templates = new ObservableCollection<SyntaxTemplateViewModel>()
                {
                    new SyntaxTemplateViewModel()
                    {
                        Name = "My Template",
                        TemplateBody = "My Template Body with one parameter {name}",
                        Parameters = new ObservableCollection<SyntaxTemplateParameterViewModel>()
                        {
                            new SyntaxTemplateParameterViewModel()
                            {
                                Parameter = "name"
                            }
                        }
                    }
                }
            };
            var scriptMain = new ScriptMainViewModel();

            this.DockingManagerItemsSource = new ObservableCollection<DockingManagerItemViewModel>();
            this.DockingManagerItemsSource.Add(defaultDocument);
            this.DockingManagerItemsSource.Add(syntaxTemplateMain);
            this.DockingManagerItemsSource.Add(scriptMain);

            this.Mode = PlayMode.Macro;
            this.IsRecording = false;
            this.IsPlayable = false;
            this.Macros = new ObservableCollection<MacroViewModel>();

            this.OpenCommand = new SimpleCommand(() =>
            {
                var dialog = new OpenFileDialog();

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    foreach (var file in dialog.FileNames)
                    {
                        var text = File.ReadAllText(file);
                        var newDocument = new DocumentViewModel()
                        {
                            Header = file,
                            FileName = file,
                            IsDirty = false,
                            Contents = text,
                            IsSelected = true
                        };

                        this.DockingManagerItemsSource.Add(newDocument);
                    }
                }
            });

            this.SaveCommand = new SimpleCommand(() =>
            {

            });

            this.SaveAsCommand = new SimpleCommand(() =>
            {

            });

            this.SaveTemplatesCommand = new SimpleCommand(() =>
            {

            });

            this.RecordCommand = new SimpleCommand(() =>
            {
                ProcessRecord();
            });

            this.StopCommand = new SimpleCommand(() =>
            {
                ProcessStop();
            });
            this.PlayCommand = new SimpleCommand(() =>
            {
                switch (this.Mode)
                {
                    case PlayMode.SyntaxTemplate:
                        ProcessPlaySyntaxTemplate();
                        break;
                    case PlayMode.Macro:
                        ProcessPlayMacro();
                        break;
                    case PlayMode.Script:
                        ProcessPlayScript();
                        break;
                    default:
                        break;
                }
            });

            this.PlayRestCommand = new SimpleCommand(() =>
            {
                switch (this.Mode)
                {
                    case PlayMode.SyntaxTemplate:
                        ProcessPlayRestSyntaxTemplate();
                        break;
                    case PlayMode.Macro:
                        ProcessPlayRestMacro();
                        break;
                    case PlayMode.Script:
                        ProcessPlayRestScript();
                        break;
                    default:
                        break;
                }
            });
        }

        private void SetPlayableFlags()
        {
            this.IsPlayable = this.SelectedSyntaxTemplate != null && this.Mode == PlayMode.SyntaxTemplate ||
                              this.SelectedMacro != null && this.Mode == PlayMode.Macro ||
                              this.SelectedScript != null && this.SelectedScriptMethod != null && this.Mode == PlayMode.Script;
        }

        public void CancelRecording()
        {
            foreach (var item in this.DockingManagerItemsSource)
            {
                if (item is DocumentViewModel)
                {
                    (item as DocumentViewModel).IsRecordingKeystrokes = false;
                    (item as DocumentViewModel).ClearMacroKeystrokes();
                }
            }
            this.IsRecording = false;
        }
        public bool IsValid()
        {
            return this.DockingManagerItemsSource != null &&
                   this.DockingManagerItemsSource.Count(x => x.GetType() == typeof(SyntaxTemplateMainViewModel)) == 1;
        }

        private void ProcessStop()
        {
            var currentDocument = this.DockingManagerItemsSource.FirstOrDefault(x => x.IsSelected) as DocumentViewModel;

            if (currentDocument != null &&
                this.StopMacroEvent != null)
            {
                this.StopMacroEvent(currentDocument);
                this.IsRecording = false;

                // Set view model for not-recording
                currentDocument.IsRecordingKeystrokes = false;

                var nameDialog = new NameDialog();
                nameDialog.DialogTitle = "New Macro";
                nameDialog.DialogLabel = "Macro Name";

                if (nameDialog.ShowDialog() == true)
                {
                    // Get keystrokes and create macro
                    var keyStrokes = currentDocument.GetMacroKeystrokes();
                    var macro = new MacroViewModel()
                    {
                        Name = nameDialog.DialogNameResult,
                        KeyStrokes = new ObservableCollection<MacroKeyStrokeViewModel>(keyStrokes)
                    };
                    this.Macros.Add(macro);

                    // Clear out the macro keystroke buffer
                    currentDocument.ClearMacroKeystrokes();
                }
            }
        }
        private void ProcessRecord()
        {
            var currentDocument = this.DockingManagerItemsSource.FirstOrDefault(x => x.IsSelected) as DocumentViewModel;

            if (currentDocument != null &&
                this.RecordMacroEvent != null)
            {
                this.RecordMacroEvent(currentDocument);
                this.IsRecording = true;

                // Set view model for recording
                currentDocument.IsRecordingKeystrokes = true;
            }
        }

        private void ProcessPlayMacro()
        {
            var currentDocument = this.DockingManagerItemsSource.FirstOrDefault(x => x.IsSelected) as DocumentViewModel;

            if (this.SelectedMacro != null &&
                currentDocument != null &&
                this.PlayMacroEvent != null)
                this.PlayMacroEvent(currentDocument, this.SelectedMacro);
        }
        private void ProcessPlayRestMacro()
        {
            var currentDocument = this.DockingManagerItemsSource.FirstOrDefault(x => x.IsSelected) as DocumentViewModel;

            if (this.SelectedMacro != null &&
                currentDocument != null &&
                this.PlayRestMacroEvent != null)
                this.PlayRestMacroEvent(currentDocument, this.SelectedMacro);
        }
        private void ProcessPlaySyntaxTemplate()
        {
            var currentDocument = this.DockingManagerItemsSource.FirstOrDefault(x => x.IsSelected) as DocumentViewModel;

            if (this.SelectedSyntaxTemplate != null &&
                currentDocument != null &&
                this.PlaySyntaxTemplateEvent != null)
                this.PlaySyntaxTemplateEvent(currentDocument, this.SelectedSyntaxTemplate);
        }
        private void ProcessPlayRestSyntaxTemplate()
        {
            var currentDocument = this.DockingManagerItemsSource.FirstOrDefault(x => x.IsSelected) as DocumentViewModel;

            if (this.SelectedSyntaxTemplate != null &&
                currentDocument != null &&
                this.PlayRestSyntaxTemplateEvent != null)
                this.PlayRestSyntaxTemplateEvent(currentDocument, this.SelectedSyntaxTemplate);
        }
        private void ProcessPlayScript()
        {
            var currentDocument = this.DockingManagerItemsSource.FirstOrDefault(x => x.IsSelected) as DocumentViewModel;

            if (this.SelectedScript != null &&
                currentDocument != null &&
                this.PlayScriptEvent != null)
                this.PlayScriptEvent(currentDocument, this.SelectedScript, this.SelectedScriptMethod);
        }
        private void ProcessPlayRestScript()
        {
            var currentDocument = this.DockingManagerItemsSource.FirstOrDefault(x => x.IsSelected) as DocumentViewModel;

            if (this.SelectedScript != null &&
                currentDocument != null &&
                this.PlayRestScriptEvent != null)
                this.PlayRestScriptEvent(currentDocument, this.SelectedScript, this.SelectedScriptMethod);
        }

        public static MainViewModel CreateDefault()
        {
            return new MainViewModel();
        }

        public static MainViewModel Create(IEnumerable<DockingManagerItemViewModel> items)
        {
            var viewModel = new MainViewModel();
            SyntaxTemplateMainViewModel syntaxViewModel = null;

            viewModel.DockingManagerItemsSource.Clear();

            foreach (var item in items)
            {
                if (item.GetType() == typeof(SyntaxTemplateMainViewModel))
                {
                    if (syntaxViewModel != null)
                        throw new Exception("Syntax templates defined more than once. Cannot create main view model");

                    else
                        syntaxViewModel = (SyntaxTemplateMainViewModel)item;
                }

                viewModel.DockingManagerItemsSource.Add(item);
            }

            return viewModel;
        }
    }
}

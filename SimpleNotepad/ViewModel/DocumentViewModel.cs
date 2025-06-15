using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SimpleWpf.Extensions;

namespace SimpleNotepad.ViewModel
{
    public class DocumentViewModel : ViewModelBase
    {
        string _title;
        string _fileName;
        string _contents;

        public string Title
        {
            get { return _title; }
            set { this.RaiseAndSetIfChanged(ref _title, value); }
        }
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

        public DocumentViewModel()
        {
            this.Title = string.Empty;
            this.FileName = string.Empty;
            this.Contents = string.Empty;
        }
    }
}

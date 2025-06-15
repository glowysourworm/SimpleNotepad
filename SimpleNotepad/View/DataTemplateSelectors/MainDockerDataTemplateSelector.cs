using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

using AvalonDock;
using AvalonDock.Layout;

using EMA.ExtendedWPFVisualTreeHelper;

using SimpleNotepad.ViewModel;

namespace SimpleNotepad.View.DataTemplateSelectors
{
    public class MainDockerDataTemplateSelector : DataTemplateSelector
    {
        // These data templates are built in the XAML. The placeholders here are to bind to
        // the template.

        public DataTemplate DocumentTemplateDefault { get; set; }
        public DataTemplate SyntaxTemplatesMain { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (this.DocumentTemplateDefault == null ||
                this.SyntaxTemplatesMain == null)
                throw new Exception("Main Docker data templates are not initialized appropriately. Please set these in the XAML first.");

            if (item is SyntaxTemplateMainViewModel)
                return this.SyntaxTemplatesMain;

            else 
                return this.DocumentTemplateDefault;
        }
    }
}

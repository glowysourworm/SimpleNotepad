using System.Windows;
using System.Windows.Media;

using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Rendering;

namespace SimpleNotepad.View.AvalonEditExtension
{
    public class HighlightCurrentLineBackgroundRenderer : IBackgroundRenderer
    {
        private TextEditor _editor;
        private Brush _highlightBrush;

        public HighlightCurrentLineBackgroundRenderer(TextEditor editor, Color highlightColor)
        {
            _editor = editor;
            _highlightBrush = new SolidColorBrush(highlightColor);
        }

        public KnownLayer Layer
        {
            get { return KnownLayer.Selection; }
        }

        public void Draw(TextView textView, DrawingContext drawingContext)
        {
            if (_editor.Document == null)
                return;

            textView.EnsureVisualLines();

            var currentLine = _editor.Document.GetLineByOffset(_editor.CaretOffset);

            foreach (var rect in BackgroundGeometryBuilder.GetRectsForSegment(textView, currentLine, true))
            {
                drawingContext.DrawRectangle(_highlightBrush, null, new Rect(rect.Location, new Size(_editor.RenderSize.Width, rect.Height)));
            }
        }
    }
}

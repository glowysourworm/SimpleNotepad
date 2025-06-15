﻿using System.Globalization;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.TextFormatting;

namespace SimpleNotepad.Controls
{
    public class SimpleTextEditor : Control
    {
        public static readonly DependencyProperty TextSourceProperty =
            DependencyProperty.Register("TextSource", typeof(string), typeof(SimpleTextEditor), new PropertyMetadata(OnTextSourceChanged));

        public static readonly DependencyProperty FocusedBackgroundProperty =
            DependencyProperty.Register("FocusedBackground", typeof(Brush), typeof(SimpleTextEditor), new PropertyMetadata(Brushes.White));

        public static readonly DependencyProperty FocusedBorderBrushProperty =
            DependencyProperty.Register("FocusedBorderBrush", typeof(Brush), typeof(SimpleTextEditor), new PropertyMetadata(Brushes.DodgerBlue));

        public static readonly DependencyProperty FocusedCaretBrushProperty =
            DependencyProperty.Register("FocusedCaretBrush", typeof(Brush), typeof(SimpleTextEditor), new PropertyMetadata(Brushes.Gray));

        public static readonly DependencyProperty CaretBrushProperty =
            DependencyProperty.Register("CaretBrush", typeof(Brush), typeof(SimpleTextEditor), new PropertyMetadata(Brushes.LightGray));

        public string TextSource
        {
            get { return (string)GetValue(TextSourceProperty); }
            set { SetValue(TextSourceProperty, value); }
        }
        public Brush FocusedBackground
        {
            get { return (Brush)GetValue(FocusedBackgroundProperty); }
            set { SetValue(FocusedBackgroundProperty, value); }
        }
        public Brush FocusedBorderBrush
        {
            get { return (Brush)GetValue(FocusedBorderBrushProperty); }
            set { SetValue(FocusedBorderBrushProperty, value); }
        }
        public Brush FocusedCaretBrush
        {
            get { return (Brush)GetValue(FocusedCaretBrushProperty); }
            set { SetValue(FocusedCaretBrushProperty, value); }
        }
        public Brush CaretBrush
        {
            get { return (Brush)GetValue(CaretBrushProperty); }
            set { SetValue(CaretBrushProperty, value); }
        }

        SimpleTextEditorCore _core;        

        public SimpleTextEditor()
        {
            this.Cursor = Cursors.IBeam;

            this.Loaded += SimpleTextEditor_Loaded;
        }

        private void SimpleTextEditor_Loaded(object sender, RoutedEventArgs e)
        {
            _core = new SimpleTextEditorCore(this.FontFamily, this.FontSize, this.Foreground, Brushes.Transparent, TextWrapping.Wrap);

            SetText("");
        }

        protected void SetText(string text)
        {
            if (_core == null)
                return;

            _core.SetText(text);

            InvalidateMeasure();
            InvalidateVisual();
        }

        protected override Size MeasureOverride(Size constraint)
        {
            if (_core == null)
                return constraint;

            if (constraint.Width == 0 ||
                double.IsNaN(constraint.Width) ||
                constraint.Height == 0 ||
                double.IsNaN(constraint.Height))
                return constraint;

            else
            {
                // Add Control Padding
                constraint.Width -= (this.Padding.Left + this.Padding.Right);
                constraint.Height -= (this.Padding.Top + this.Padding.Bottom);

                var desiredSize = _core.MeasureText(constraint);

                return desiredSize;
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            if (e.Key == Key.Back)
            {
                _core.Backspace();
                e.Handled = true;

                InvalidateMeasure();
                InvalidateVisual();
            }
        }
        protected override void OnTextInput(TextCompositionEventArgs e)
        {
            base.OnTextInput(e);   

            _core.AppendText(e.Text);

            InvalidateMeasure();
            InvalidateVisual();
        }

        protected override void OnPreviewTextInput(TextCompositionEventArgs e)
        {
            base.OnPreviewTextInput(e);
        }

        
        protected override void OnLostFocus(RoutedEventArgs e)
        {
            base.OnLostFocus(e);

            // Does not invalidate by default
            InvalidateVisual();
        }
        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);

            if (this.IsFocused)
                return;

            // Does not focus by default
            this.Focus();

            InvalidateVisual();
        }

        private static void OnTextSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var editor = d as SimpleTextEditor;

            editor?.SetText(e.NewValue as string ?? "");
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            if (_core == null)
                return;

            Brush background = this.Background;
            Brush border = this.BorderBrush;
            Brush caret = this.CaretBrush;

            // Limited Focus Visual Style
            if (this.IsFocused || this.IsKeyboardFocused)
            {
                background = this.FocusedBackground;
                border = this.FocusedBorderBrush;
                caret = this.FocusedCaretBrush;
            }

            // Border / Background (Outer Padded Area + Control Area)
            drawingContext.DrawRectangle(background, new Pen(border, this.BorderThickness.Top), new Rect(this.RenderSize));

            var caretBounds = _core.GetCaretRenderBounds();
            var constraint = new Size(this.RenderSize.Width - this.Padding.Left - this.Padding.Right, 
                                      this.RenderSize.Height - this.Padding.Top - this.Padding.Bottom);
            var position = new Point(0, this.Padding.Top);

            foreach (var textLine in _core.GetTextLines())
            {
                //var text = new FormattedText(this.TextSource, CultureInfo.CurrentUICulture, FlowDirection.LeftToRight, _typeface, this.FontSize, this.Foreground);

                position.X = textLine.Start + this.Padding.Left;
                position.Y += textLine.TextHeight;
                textLine.Draw(drawingContext, position, InvertAxes.None);

                //drawingContext.DrawText(text, new Point(this.Padding.Left, this.Padding.Top));
            }

            if (caretBounds.Height > 0)
            {
                caretBounds.X += this.Padding.Left + 2; // KLUDGE
                caretBounds.Y += this.Padding.Top + 2;

                drawingContext.DrawRectangle(caret, null, caretBounds);
            }

            /*
            Brush background = this.Background;
            Brush border = this.BorderBrush;
            Brush caret = this.CaretBrush;

            // Limited Focus Visual Style
            if (this.IsFocused || this.IsKeyboardFocused)
            {
                background = this.FocusedBackground;
                border = this.FocusedBorderBrush;
                caret = this.FocusedCaretBrush;
            }

            // Border / Background (Outer Padded Area + Control Area)
            drawingContext.DrawRectangle(background, new Pen(border, this.BorderThickness.Top), new Rect(this.RenderSize));

            // Control Area
            var textArea = new Rect(this.Padding.Left,
                                    this.Padding.Top,
                                    this.RenderSize.Width - this.Padding.Left - this.Padding.Right,
                                    this.RenderSize.Height - this.Padding.Top - this.Padding.Bottom);

            var textRendering = _core.GetRendering(textArea.Size);          // Used for the measurement
            var caretBounds = _core.GetCaretRenderBounds();
            var caretRenderBounds = new Rect(caretBounds.X + textArea.X,
                                             caretBounds.Y + textArea.Y + 4,            // KLUDGE
                                             caretBounds.Width, caretBounds.Height);

            // Text Bitmap (if TextStore has non-null / empty text)
            if (textRendering != null)
            {
                drawingContext.DrawImage(textRendering, new Rect(this.Padding.Left, this.Padding.Top, textRendering.Width, textRendering.Height));
            }

            // Caret (pre-measured)
            if (caretRenderBounds.Height > 0)
                drawingContext.DrawRectangle(caret, null, caretRenderBounds);
            */
        }
    }
}

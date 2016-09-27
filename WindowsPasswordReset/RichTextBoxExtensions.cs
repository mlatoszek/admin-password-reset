using System;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace WindowsPasswordReset
{
    public static class RichTextBoxExtensions
    {
        public static void AppendText(this RichTextBox box, string text, Brush brush)
        {
            box.Dispatcher.BeginInvoke(new Action(() =>
            {
                BrushConverter bc = new BrushConverter();
                TextRange tr = new TextRange(box.Document.ContentEnd, box.Document.ContentEnd);
                tr.Text = text;
                try
                {
                    tr.ApplyPropertyValue(TextElement.ForegroundProperty, brush);
                    box.ScrollToEnd();
                }
                catch (FormatException) { }
            }));            
        }
    }
}
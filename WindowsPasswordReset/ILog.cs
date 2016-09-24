using System;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace WindowsPasswordReset
{
    public interface ILog
    {
        void Info(string message);
        void Error(string message);
        void Success(string message);
    }

    public class RtbLogger : ILog
    {
        private RichTextBox rtbConsole;

        public RtbLogger(RichTextBox rtbConsole)
        {
            this.rtbConsole = rtbConsole;
        }

        public void Error(string message)
        {
            rtbConsole.AppendText(message + Environment.NewLine, Brushes.Red);
        }

        public void Info(string message)
        {            
            rtbConsole.AppendText(message + Environment.NewLine, Brushes.Black);
        }

        public void Success(string message)
        {
            rtbConsole.AppendText(message + Environment.NewLine, Brushes.Green);
        }
    }

    public static class RichTextBoxExtensions
    {
        public static void AppendText(this RichTextBox box, string text, Brush brush)
        {
            BrushConverter bc = new BrushConverter();
            TextRange tr = new TextRange(box.Document.ContentEnd, box.Document.ContentEnd);
            tr.Text = text;
            try
            {
                tr.ApplyPropertyValue(TextElement.ForegroundProperty, brush);
            }
            catch (FormatException) { }
        }
    }

}

using System;
using System.Windows.Controls;
using System.Windows.Media;
using WindowsPasswordReset.Logic;

namespace WindowsPasswordReset
{
    public class RtbLogger : ILog
    {
        private readonly RichTextBox rtbConsole;

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
}
using System;
using WindowsPasswordReset.Logic;

namespace WindowsPasswortReset.Console
{
    public class ConsoleLogger : ILog
    {        

        public void Error(string message)
        {
            System.Console.ForegroundColor = ConsoleColor.Red;
            System.Console.WriteLine(message);
            System.Console.ForegroundColor = ConsoleColor.White;
        }

        public void Info(string message)
        {
            System.Console.ForegroundColor = ConsoleColor.White;
            System.Console.WriteLine(message);
        }

        public void Success(string message)
        {
            System.Console.ForegroundColor = ConsoleColor.Green;
            System.Console.WriteLine(message);
            System.Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
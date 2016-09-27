namespace WindowsPasswordReset.Logic
{
    public interface ILog
    {
        void Info(string message);
        void Error(string message);
        void Success(string message);
    }
}

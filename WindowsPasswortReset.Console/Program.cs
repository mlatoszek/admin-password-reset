using System;
using WindowsPasswordReset.Logic;

namespace WindowsPasswortReset.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var logger = new ConsoleLogger();
            var profileConnection = new ProfileConnection(logger);

            logger.Info("Current user " + profileConnection.GetCurrentUserName() + " has admin rights: " + profileConnection.IsCurrentUserAdmin());
            logger.Info("Trying to reset administrators password");

            try
            {
                // profileConnection.ListGroups();
                if (!profileConnection.HasUser(ProfileConnection.AdminUserName))
                {
                    profileConnection.CreateNewUser(ProfileConnection.AdminUserName, ProfileConnection.AdminPassword);
                }
                profileConnection.UnlockUser(ProfileConnection.AdminUserName);

                profileConnection.ResetUserPassword(ProfileConnection.AdminUserName, ProfileConnection.AdminPassword);
                logger.Success("Password reset succedded");
            }
            catch (Exception exc)
            {
                logger.Error("Cannot reset administrators password: " + exc.Message);
                System.Environment.Exit(-1);
            }
        }
    }
}

using System;
using System.Windows;
using System.Windows.Controls;

namespace WindowsPasswordReset
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ProfileConnection profileConnection;
        private ILog logger;

        private const string AdminPassword = "1qazXSW@3edc";

        public MainWindow()
        {
            InitializeComponent();

            logger = new RtbLogger(rtbConsole);
            profileConnection = new ProfileConnection(logger);

            InitBindings();
        }

        private void InitBindings()
        {
            var profileInfo = new ProfileInformation(profileConnection);
            spUserInfo.DataContext = profileInfo;
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            Button bResetPassword = (Button)sender;

            System.Threading.ThreadPool.QueueUserWorkItem((state) => {
                Dispatcher.Invoke(new Action(() => {
                    bResetPassword.IsEnabled = false;
                    pbProgress.IsIndeterminate = true;                    
                }));
                logger.Info("Trying to reset administrators password");

                try
                {
                    profileConnection.ListGroups();
                    if (!profileConnection.HasUser(ProfileConnection.AdminUserName))
                    {
                        profileConnection.CreateNewUser(ProfileConnection.AdminUserName, AdminPassword);
                    }
                    profileConnection.UnlockUser(ProfileConnection.AdminUserName);

                    profileConnection.ResetUserPassword(ProfileConnection.AdminUserName, AdminPassword);
                    logger.Success("Password reset succedded");
                }
                catch (Exception exc)
                {
                    logger.Error("Cannot reset administrators password: " + exc.Message);
                }
                Dispatcher.Invoke(new Action(() => {
                    bResetPassword.IsEnabled = true;
                    pbProgress.IsIndeterminate = false;
                }));
            });            
        }

        private void ResetPassword(object state)
        {
            throw new NotImplementedException();
        }        
    }

    public class ProfileInformation
    {
        public string Username { get; set; }
        public bool IsAdmin { get; set; }

        public ProfileInformation(ProfileConnection profileConnection)
        {
            Username = profileConnection.GetCurrentUserName();
            IsAdmin = profileConnection.IsCurrentUserAdmin();
        }

    }
}

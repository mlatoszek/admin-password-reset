using System;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Security.Principal;

namespace WindowsPasswordReset.Logic
{
    public class ProfileConnection : IDisposable
    {
        private ILog logger;
        PrincipalContext context;

        public const string AdminUserName = "TestUser";
        public const string AdminPassword = "1qazXSW@3edc";

        public ProfileConnection(ILog logger)
        {
            this.logger = logger;
            context = new PrincipalContext(ContextType.Machine, Environment.MachineName);
        }

        public string GetCurrentUserName()
        {
            logger.Info("Getting username");
            return Environment.UserName;
        }

        public bool IsCurrentUserAdmin()
        {
            logger.Info("Checking administration rights");
            return new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator);
        }

        public bool HasUser(string username)
        {
            logger.Info("Checking for "+username+" account");
            UserPrincipal user = new UserPrincipal(context);
            user.Name = username;
            PrincipalSearcher ps = new PrincipalSearcher();
            ps.QueryFilter = user;

            PrincipalSearchResult<Principal> result = ps.FindAll();
            if (result.Any())
            {
                logger.Info(username + " account found");
            }
            else
            {
                logger.Info(username + " account not found");
            }

            return result.Any();
        }

        public void UnlockUser(string username)
        {
            logger.Info("Unlocking user " + username);

            UserPrincipal usr = UserPrincipal.FindByIdentity(context, username);
            if (usr == null)
            {
                throw new ArgumentException("Username " + username + " not found!");
            }
           
            if (!usr.IsAccountLockedOut())
            {
                logger.Info("User " + username + " is not locked out. Unlocking anyway");
            }

            usr.Enabled = true;
            usr.UnlockAccount();
            usr.Save();
            logger.Info("User " + username + " is unlocked");
        }

        public void ResetUserPassword(string username, string password)
        {
            logger.Info("Reseting password for user " + username);
            UserPrincipal usr = UserPrincipal.FindByIdentity(context, username);
            if (usr == null)
            {
                throw new ArgumentException("Username " + username + " not found!");
            }

            usr.SetPassword(password);
        }

        public void ListGroups()
        {
            GroupPrincipal groupSearch = new GroupPrincipal(context);
            PrincipalSearcher searcher = new PrincipalSearcher(groupSearch);
            var groups = searcher.FindAll();
            foreach (var grp in groups)
            {
                Console.WriteLine(grp.Name);
            }
        }

        public void CreateNewUser(string username, string password)
        {
            var administratorGroups = new string[] { "Administrators", "Administratorzy" };
            GroupPrincipal adminGroup = null;

            foreach (var adminGroupName in administratorGroups)
            {
                try
                {
                    logger.Info("Checking if " + adminGroupName + " exist");
                    adminGroup = GroupPrincipal.FindByIdentity(context, adminGroupName);
                    if (adminGroup != null)
                    {
                        // check if admin group is working:)
                        adminGroup.GetMembers();
                    }
                    logger.Info("Group " + adminGroupName + " found");
                }
                catch(Exception exc)
                {
                    logger.Info("Group " + adminGroupName + " not found");
                    adminGroup = null;
                }

                if (adminGroup != null) break;

            }

            UserPrincipal usr = new UserPrincipal(context);
            usr.Name = username;
            usr.SetPassword(password);
            usr.Save();

            adminGroup.Members.Add(usr);
            adminGroup.Save();
        }


        public void Dispose()
        {
            if (context != null)
            {
                context.Dispose();
            }
        }
    }
}

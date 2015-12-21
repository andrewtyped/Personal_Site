using DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;

namespace PersonalSite.Models
{
    public class UserMemberProvider : MembershipProvider
    {
        #region Unused MembershipProvider Nonsense
        public override string ApplicationName
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            throw new NotImplementedException();
        }

        public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            throw new NotImplementedException();
        }

        public override bool EnablePasswordReset
        {
            get { throw new NotImplementedException(); }
        }

        public override bool EnablePasswordRetrieval
        {
            get { throw new NotImplementedException(); }
        }

        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override int GetNumberOfUsersOnline()
        {
            throw new NotImplementedException();
        }

        public override string GetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            throw new NotImplementedException();
        }

        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            throw new NotImplementedException();
        }

        public override string GetUserNameByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public override int MaxInvalidPasswordAttempts
        {
            get { throw new NotImplementedException(); }
        }

        public override int MinRequiredNonAlphanumericCharacters
        {
            get { throw new NotImplementedException(); }
        }

        public override int MinRequiredPasswordLength
        {
            get { throw new NotImplementedException(); }
        }

        public override int PasswordAttemptWindow
        {
            get { throw new NotImplementedException(); }
        }

        public override MembershipPasswordFormat PasswordFormat
        {
            get { throw new NotImplementedException(); }
        }

        public override string PasswordStrengthRegularExpression
        {
            get { throw new NotImplementedException(); }
        }

        public override bool RequiresQuestionAndAnswer
        {
            get { throw new NotImplementedException(); }
        }

        public override bool RequiresUniqueEmail
        {
            get { throw new NotImplementedException(); }
        }

        public override string ResetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        public override bool UnlockUser(string userName)
        {
            throw new NotImplementedException();
        }

        public override void UpdateUser(MembershipUser user)
        {
            throw new NotImplementedException();
        }

        public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
        {
            throw new NotImplementedException();
        }
        #endregion 

        public User user
        {
            get;
            set;
        }

        private IUserDataAccess dataAccess;

        public UserMemberProvider()
            : this(new SqlUserDataAccess())
        {
        }

        public UserMemberProvider(IUserDataAccess dataAccess) :base()
        {
            this.dataAccess = dataAccess;
        }

        public User CreateUser(string userName, string password, string email)
        {
            var role = ChooseNewUserRole();

            string encryptedPassword = EncryptPassword(userName, password);

            User user = dataAccess.CreateUser(userName, encryptedPassword, email, role);

            return user;
        }

        private string ChooseNewUserRole()
        {
            int userCount = int.MaxValue;
            string newUserRole = "member";

            try
            {
                userCount = dataAccess.GetUserCount();
            }
            catch
            {
                userCount = int.MaxValue;
            }

            if (userCount == 0)
            {
                newUserRole = "admin";
            }
            else
            {
                newUserRole = "member";
            }

            return newUserRole;
        }
        
        //Making this method public for purposes of testability
        public string EncryptPassword(string userName, string password)
        {
            var saltedPwd = GetSaltedPwd(userName.ToUpper(),password);

            byte[] pwdBytes = Encoding.Default.GetBytes(saltedPwd);
            byte[] hashBytes = System.Security.Cryptography.SHA256Managed.Create().ComputeHash(pwdBytes);

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < hashBytes.Length; i++)
            {
                sb.Append(hashBytes[i].ToString("X2"));
            }

            return sb.ToString();
        }

        private string GetSaltedPwd(string userName, string password)
        {
            return string.Concat(userName[0], userName[4], password);
        }

        public override bool ValidateUser(string userName, string password)
        {
            if (string.IsNullOrEmpty(password.Trim()))
            {
                return false;
            }

            string hash = EncryptPassword(userName,password);

            User user = dataAccess.GetUserByName(userName);

            if (user == null)
            {
                return false;
            }

            if (user.Password == hash)
            {
                return true;
            }

            return false;
        }
    }
}
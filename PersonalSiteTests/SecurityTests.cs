using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Moq;
using DataAccess.Interfaces;
using PersonalSite.Models;
using System.Data.SqlClient;
using DataAccess;
using System.Data;

namespace PersonalSiteTests
{
    [TestFixture]
    public class SecurityDataTests
    {
        private Mock<IDataAccess> dataAccess;
        private Mock<IUserDataAccess> userDataAccess;

        private Role testRoleAdmin;
        private Role testRoleUser;

        private List<IDataParameter> userParms;

        [TestFixtureSetUp]
        public void SetUpTestObjects()
        {
            SetUpTestRoles();

            userParms = new List<IDataParameter>();
            dataAccess = new Mock<IDataAccess>();
            userDataAccess = new Mock<IUserDataAccess>();
        }

        private void SetUpTestRoles()
        {
            testRoleAdmin = new Role("admin");
            testRoleUser = new Role("user");
        }

        [Test]
        public void ShouldBeAbleToGetUserByName()
        {
            var userDataAccess = new SqlUserDataAccess(dataAccess.Object);

            string userName = "andrew";
            SetUpuspGetUserByName(userName);           

            User user = userDataAccess.GetUserByName(userName);

            Assert.IsTrue(user.UserId == 1);
            Assert.IsTrue(user.UserName == "andrew");
            Assert.IsTrue(user.Email == "test@test");
            Assert.IsTrue(user.Password == "12345");
            Assert.IsTrue(user.Role.RoleName == "admin");

            userName = "bob";
            SetUpuspGetUserByName(userName);
            user = userDataAccess.GetUserByName(userName);

            Assert.IsTrue(user.UserId == 2);
            Assert.IsTrue(user.UserName == "bob");
            Assert.IsTrue(user.Email == "test@test");
            Assert.IsTrue(user.Password == "12345");
            Assert.IsTrue(user.Role.RoleName == "user");

            //This user is undefined, should get the FAIL USER for now
            userName = "snidely";

            SetUpuspGetUserByName(userName);
            user = userDataAccess.GetUserByName(userName);

            Assert.IsTrue(user.UserId == 0);
            Assert.IsTrue(user.UserName == "FAIL");
        }

        private void SetUpuspGetUserByName(string userName)
        {
            dataAccess.Setup(m => m.ExecProcWithReturnData("usp_GetUserByName", It.IsAny<IEnumerable<IDataParameter>>()))
                .Returns(GetUserForMockuspGetUserCall(userName));
        }

        private IDataResult GetUserForMockuspGetUserCall(string userName)
        {
            DataTable table = new DataTable();
            table.Columns.Add(new DataColumn("Id", typeof(int)));
            table.Columns.Add(new DataColumn("Name", typeof(string)));
            table.Columns.Add(new DataColumn("Email", typeof(string)));
            table.Columns.Add(new DataColumn("Password", typeof(string)));
            table.Columns.Add(new DataColumn("Role", typeof(string)));

            if (userName == "andrew")
            {
                table.Rows.Add(1, "andrew", "test@test", "12345", "admin");
                return new DataResult(true, "", table);
            }
            else if (userName == "bob")
            {
                table.Rows.Add(2, "bob", "test@test", "12345", "user");
                return new DataResult(true, "", table);
            }

            return new DataResult(false, "", table);
        }

        [Test]
        public void ShouldBeAbleToGetUserCount()
        {
            var userDataAccess = new SqlUserDataAccess(dataAccess.Object);

            dataAccess.Setup(m => m.ExecScalar("usp_GetUserCount"))
                .Returns(new ScalarResult(true,"",0));

            int userCount = userDataAccess.GetUserCount();

            Assert.IsTrue(userCount == 0);
        }

        [Test]
        public void ShouldBeAbleToCreateUser()
        {
            var userDataAccess = new SqlUserDataAccess(dataAccess.Object);

            dataAccess.Setup(m => m.ExecScalar("usp_GetUserCount"))
                .Returns(new ScalarResult(true,"",0));

            var procResult = new DataTable();
            Func<string,Type,DataColumn> addColumn = procResult.Columns.Add;
            addColumn("Id", typeof(int));
            procResult.Rows.Add(1);

            dataAccess.Setup(m => m.ExecProcWithReturnData("usp_CreateUser", It.IsAny<IEnumerable<IDataParameter>>()))
                .Returns(new DataResult(true,"",procResult));

            User admin = userDataAccess.CreateUser("andrew", "12345", "test@test", "admin");

            Assert.AreEqual(admin.UserId, 1);
            Assert.AreEqual(admin.Role.RoleName, "admin");
        }

        [Test]
        public void ShouldBeAbleToEncryptPassword()
        {
            var provider = new UserMemberProvider(userDataAccess.Object);

            //Run through a few known test vectors of SHA 256 
            string username = "m...e";
            string password = "ssage digest";
            string encryptedPassword = provider.EncryptPassword(username, password);

            Assert.IsTrue(string.Equals(encryptedPassword, "f7846f55cf23e14eebeab5b4e1550cad5b509e3348fbc4efa3a1413d393cb650"
                , StringComparison.OrdinalIgnoreCase));

            username = "F...o";
            password = "r this sample, this 63-byte string will be used as input data";
            encryptedPassword = provider.EncryptPassword(username, password);

            Assert.IsTrue(string.Equals(encryptedPassword, "f08a78cbbaee082b052ae0708f32fa1e50c5c421aa772ba5dbb406a2ea6be342"
                , StringComparison.OrdinalIgnoreCase));

            username = "T...h";
            password = "is is exactly 64 bytes long, not counting the terminating byte";
            encryptedPassword = provider.EncryptPassword(username, password);

            Assert.IsTrue(string.Equals(encryptedPassword, "ab64eff7e88e2e46165e29f2bce41826bd4c7b3552f6b382a9e7d3af47c245f8"
                , StringComparison.OrdinalIgnoreCase));

            //Not a known test vector, but comforting to know all these special chars work
            username = @"!...@";
            password = @"#$%%^&*()+=~`?><,./[]{}\|";
            encryptedPassword = provider.EncryptPassword(username, password);

            Assert.IsTrue(string.Equals(encryptedPassword, "300609615f69e44968f8503b60eec274ad7bf58dae7237efb053b22d6d0cef46"
                , StringComparison.OrdinalIgnoreCase));
        }

        [Test]
        public void ShouldBeAbleToCreateAdminFromMembershipProvider()
        {
            var provider = new UserMemberProvider(userDataAccess.Object);

            userDataAccess.Setup(m => m.CreateUser("andrew", It.IsAny<string>(), "test@test", "admin"))
                .Returns(new User(1, "m...e", "test@test", "12345",new Role("admin")));

            dataAccess.Setup(m => m.ExecScalar("usp_GetUserCount"))
                .Returns(new ScalarResult(true, "", 0));

            User user = provider.CreateUser("andrew", "12345", "test@test");

            Assert.AreEqual(user.Role.RoleName, "admin");
        }

        [Test]
        public void ShouldBeAbleToCreateMemberFromMembershipProvider()
        {
            var provider = new UserMemberProvider(userDataAccess.Object);

            userDataAccess.Setup(m => m.CreateUser("andrew", It.IsAny<string>(), "test@test", "admin"))
                .Returns(new User(1, "m...e", "test@test", "12345", new Role("member")));

            dataAccess.Setup(m => m.ExecScalar("usp_GetUserCount"))
                .Returns(new ScalarResult(true, "", 1));

            User user = provider.CreateUser("andrew", "12345", "test@test");

            Assert.AreEqual(user.Role.RoleName, "member");
        }

    }
}

using Tarqeem.CA.Domain.Entities.User;
using Tarqeem.CA.Tests.Setup.Setups;

namespace Tarqeem.CA.Test.Infrastructure.Identity
{
    public class UserManagerTest : TestIdentitySetup
    {
        [Fact]
        public async Task Duplicate_User_Names_Not_Allowed()
        {
            var user = new User()
            {
                UserName = "Test", Email = "Test@example.com"
            };

            var duplicateUser = new User()
            {
                UserName = "Test",
                Email = "Test@example.com"
            };

            // ReSharper disable once RedundantBaseQualifier
            await base.TestAppUserManager.CreateUser(user);

            // ReSharper disable once RedundantBaseQualifier
            var duplicateCreateUserResult = await base.TestAppUserManager.CreateUser(duplicateUser);

            Assert.False(duplicateCreateUserResult.Succeeded);
        }
    }
}
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Tarqeem.CA.Domain.Entities.Organization;
using Tarqeem.CA.Domain.Entities.User;

namespace Tarqeem.CA.Application.Contracts.Identity;

public interface IAppUserManager
{
    Task<IdentityResult> CreateUser(User user);
    Task<bool> IsExistUser(string userName);
    Task<IdentityResult> VerifyUserCode(User user, string code);
    Task<string> GenerateOtpCode(User user);
    Task<SignInResult> AdminLogin(User user, string password);
    Task<User> GetByUserName(string userName);
    Task<User> GetUserByIdAsync(int userId);
    Task<List<User>> GetAllUsersAsync();
    Task<IdentityResult> CreateUserWithPasswordAsync(User user, string password);
    Task<IdentityResult> AddUserToRoleAsync(User user, Role role);
    Task<IdentityResult> AddClaimToUser(User user, Claim claim);
    Task<IList<Claim>> GetClaimsByUser(User user);
    Task<IdentityResult> SetUserClaims(User user, IEnumerable<Claim> claim);

    Task<IdentityResult> IncrementAccessFailedCountAsync(User user);
    Task<bool> IsUserLockedOutAsync(User user);
    Task ResetUserLockoutAsync(User user);
    Task UpdateUserAsync(User user);
    Task<IdentityResult> UpdatePasswordAsync(User user, string password);

    Task UpdateSecurityStampAsync(User user);
    Task<IEnumerable<Room>> GetUserRooms(int userId);
}
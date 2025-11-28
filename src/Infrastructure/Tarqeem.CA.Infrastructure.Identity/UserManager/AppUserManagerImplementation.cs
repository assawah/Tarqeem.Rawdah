using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Tarqeem.CA.Application.Contracts.Identity;
using Tarqeem.CA.Domain.Entities.Organization;
using Tarqeem.CA.Domain.Entities.User;
using Tarqeem.CA.Infrastructure.Identity.Identity.Dtos;
using Tarqeem.CA.Infrastructure.Identity.Identity.Manager;

namespace Tarqeem.CA.Infrastructure.Identity.UserManager;

public class AppUserManagerImplementation(AppUserManager userManager) : IAppUserManager
{
    public Task<IdentityResult> CreateUser(User user)
    {
        return userManager.CreateAsync(user);
    }

    public Task<bool> IsExistUser(string userName)
    {
        return userManager.Users.AnyAsync(c => c.UserName.Equals(userName));
    }

    public async Task<IdentityResult> VerifyUserCode(User user, string code)
    {
        var confirmationResult = await userManager.VerifyUserTokenAsync(
            user,
            CustomIdentityConstants.OtpPasswordLessLoginProvider,
            CustomIdentityConstants.OtpPasswordLessLoginPurpose,
            code
        );

        if (confirmationResult)
            await userManager.UpdateSecurityStampAsync(user);

        return confirmationResult
            ? IdentityResult.Success
            : IdentityResult.Failed(new IdentityError() { Description = "Incorrect Code" });
    }

    public Task<string> GenerateOtpCode(User user)
    {
        return userManager.GenerateUserTokenAsync(
            user,
            CustomIdentityConstants.OtpPasswordLessLoginProvider,
            CustomIdentityConstants.OtpPasswordLessLoginPurpose
        );
    }

    public async Task<SignInResult> AdminLogin(User user, string password)
    {
        return await userManager.CheckPasswordAsync(user, password)
            ? SignInResult.Success
            : SignInResult.Failed;
    }

    public Task<User> GetByUserName(string userName)
    {
        return userManager.FindByNameAsync(userName);
    }

    public async Task<User> GetUserByIdAsync(int userId)
    {
        return await userManager.FindByIdAsync(userId.ToString());
    }

    public async Task<IEnumerable<Room>> GetUserRooms(int userId)
    {
        var u = await userManager.Users.Include(u => u.Rooms).FirstOrDefaultAsync(u => u.Id.Equals(userId));
        return u.Rooms.ToList();
    }

    public async Task<List<User>> GetAllUsersAsync()
    {
        return await userManager.Users.AsNoTracking().ToListAsync();
    }

    public async Task<IdentityResult> CreateUserWithPasswordAsync(User user, string password)
    {
        return await userManager.CreateAsync(user, password);
    }

    public async Task<IdentityResult> AddUserToRoleAsync(User user, Role role)
    {
        ArgumentNullException.ThrowIfNull(role, nameof(role));
        ArgumentNullException.ThrowIfNull(role.Name, nameof(role.Name));

        return await userManager.AddToRoleAsync(user, role.Name);
    }

    public async Task<IdentityResult> AddClaimToUser(User user, Claim claim)
    {
        ArgumentNullException.ThrowIfNull(claim, nameof(claim));
        ArgumentNullException.ThrowIfNull(claim.Type, nameof(claim.Value));
        return await userManager.AddClaimAsync(user, claim);
    }

    public async Task<IList<Claim>> GetClaimsByUser(User user)
    {
        return await userManager.GetClaimsAsync(user);
    }

    public async Task<IdentityResult> SetUserClaims(User user, IEnumerable<Claim> claim)
    {
        var currentClaims = await userManager.GetClaimsAsync(user);
        await userManager.RemoveClaimsAsync(user, currentClaims);
        return await userManager.AddClaimsAsync(user, claim);
    }

    public async Task<IdentityResult> IncrementAccessFailedCountAsync(User user)
    {
        return await userManager.AccessFailedAsync(user);
    }

    public async Task<bool> IsUserLockedOutAsync(User user)
    {
        var lockoutEndDate = await userManager.GetLockoutEndDateAsync(user);

        return (lockoutEndDate.HasValue && lockoutEndDate.Value > DateTimeOffset.Now);
    }

    public async Task ResetUserLockoutAsync(User user)
    {
        await userManager.SetLockoutEndDateAsync(user, null);
        await userManager.ResetAccessFailedCountAsync(user);
    }

    public async Task UpdateUserAsync(User user)
    {
        await userManager.UpdateAsync(user);
    }

    public async Task UpdateSecurityStampAsync(User user)
    {
        await userManager.UpdateSecurityStampAsync(user);
    }

    async Task<IdentityResult> IAppUserManager.UpdatePasswordAsync(User user, string password)
    {
        var code = await userManager.GeneratePasswordResetTokenAsync(user);
        return await userManager.ResetPasswordAsync(user, code, password);
    }
}
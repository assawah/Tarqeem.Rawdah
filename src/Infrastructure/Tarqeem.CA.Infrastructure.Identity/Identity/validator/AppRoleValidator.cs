using Tarqeem.CA.Domain.Entities.User;
using Microsoft.AspNetCore.Identity;

namespace Tarqeem.CA.Infrastructure.Identity.Identity.validator;

public class AppRoleValidator:RoleValidator<Role>
{
    public AppRoleValidator(IdentityErrorDescriber errors):base(errors)
    {
            
    }

    // ReSharper disable once RedundantOverriddenMember
    public override Task<IdentityResult> ValidateAsync(RoleManager<Role> manager, Role role)
    {
        return base.ValidateAsync(manager, role);
    }
}
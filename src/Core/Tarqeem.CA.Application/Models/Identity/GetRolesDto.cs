using Tarqeem.CA.Domain.Entities.User;
using Tarqeem.CA.Application.Profiles;

namespace Tarqeem.CA.Application.Models.Identity;

public class GetRolesDto:ICreateMapper<Role>
{
    public string Id { get; set; }
    public string Name { get; set; }
}
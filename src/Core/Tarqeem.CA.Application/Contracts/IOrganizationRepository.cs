using Tarqeem.CA.Domain.Entities.Organization;

namespace Tarqeem.CA.Application.Contracts;

public interface IOrganizationRepository
{
    public Task<Organization> GetOrganizationById(int organizationId);
    public Task<Organization> GetOrganizationByIdIncludeRoomsIncludeTeachers(int organizationId);
}
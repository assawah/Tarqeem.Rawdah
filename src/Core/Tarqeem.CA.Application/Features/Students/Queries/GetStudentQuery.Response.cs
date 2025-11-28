using Tarqeem.CA.Application.Profiles;
using Tarqeem.CA.Domain.Entities.Organization;

namespace Tarqeem.CA.Application.Features.Students.Queries;

public record GetStudentQueryResponse(IEnumerable<GetStudentQueryMapped> Students);

public record GetStudentQueryMapped(
    string FullName,
    string PhoneNumber,
    string Note,
    string Address,
    int Age,
    int Id,
    byte[] ProfilePicture
) : ICreateMapper<Student>;

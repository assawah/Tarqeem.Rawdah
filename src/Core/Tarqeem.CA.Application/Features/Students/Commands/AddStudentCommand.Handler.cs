using AutoMapper;
using Mediator;
using Tarqeem.CA.Application.Contracts;
using Tarqeem.CA.Application.Contracts.Persistence;
using Tarqeem.CA.Application.Models.Common;
using Tarqeem.CA.Domain.Entities.Organization;

namespace Tarqeem.CA.Application.Features.Students.Commands;

public class AddStudentCommandHandler(
    IStudentRepository studentRepository,
    IOrganizationRepository organizationRepository,
    IMapper mapper,
    IUnitOfWork work)
    : IRequestHandler<AddStudentCommand, OperationResult<int>>
{
    public async ValueTask<OperationResult<int>> Handle(AddStudentCommand request,
        CancellationToken cancellationToken)
    {
        // make sure org exists
        if (organizationRepository.GetOrganizationById(request.OrganizationId) == null)
            return OperationResult<int>.FailureResult(RawdahErrors.OrganizationNotFound);
        var student = mapper.Map<Student>(request);

        var allowedTypes = new[] { ".jpg", ".jpeg", ".png", ".gif" };
        var fileExtension = Path.GetExtension(request.File.FileName).ToLowerInvariant();
        if (!allowedTypes.Contains(fileExtension))
            return OperationResult<int>.FailureResult(RawdahErrors.FormatNotSupported);


        using var memoryStream = new MemoryStream();
        await request.File.CopyToAsync(memoryStream, cancellationToken);
        student.ProfilePicture = memoryStream.ToArray();

        var res = await studentRepository.AddStudent(student);
        await work.CommitAsync();
        return OperationResult<int>.SuccessResult(res.Entity.Id);
    }
}
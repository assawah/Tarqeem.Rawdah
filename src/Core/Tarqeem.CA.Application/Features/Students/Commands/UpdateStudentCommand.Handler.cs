#nullable enable
using Mediator;
using Tarqeem.CA.Application.Contracts;
using Tarqeem.CA.Application.Contracts.Persistence;
using Tarqeem.CA.Application.Models.Common;

namespace Tarqeem.CA.Application.Features.Students.Commands;

public class UpdateStudentCommandHandler(IStudentRepository students, IUnitOfWork unitOfWork)
    : IRequestHandler<UpdateStudentCommand, OperationResult<bool>>
{
    public async ValueTask<OperationResult<bool>> Handle(
        UpdateStudentCommand request,
        CancellationToken cancellationToken
    )
    {
        var student = await students.GetStudentByIdAsync(request.Id);

        if (student == null)
        {
            return OperationResult<bool>.FailureResult(RawdahErrors.UserNotFound);
        }

        if (request.FullName != null)
            student.FullName = request.FullName;
        if (request.PhoneNumber != null)
            student.PhoneNumber = request.PhoneNumber;
        if (request.Age.HasValue)
            student.Age = request.Age.Value;
        if (request.Note != null)
            student.Note = request.Note;
        if (request.File != null)
        {
            using var memoryStream = new MemoryStream();
            await request.File.CopyToAsync(memoryStream, cancellationToken);
            student.ProfilePicture = memoryStream.ToArray();
        }

        await unitOfWork.CommitAsync();
        return OperationResult<bool>.SuccessResult(true);
    }
}

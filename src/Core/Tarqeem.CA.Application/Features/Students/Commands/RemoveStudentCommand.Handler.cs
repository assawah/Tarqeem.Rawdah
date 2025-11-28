using Mediator;
using Tarqeem.CA.Application.Contracts;
using Tarqeem.CA.Application.Models.Common;

namespace Tarqeem.CA.Application.Features.Students.Commands;

public class RemoveStudentCommandHandler(IStudentRepository studentRepository)
    : IRequestHandler<RemoveStudentCommand, OperationResult<bool>>
{
    public async ValueTask<OperationResult<bool>> Handle(
        RemoveStudentCommand request,
        CancellationToken cancellationToken
    )
    {
        var student = await studentRepository.GetStudentByIdAsync(request.StudentId);
        if (student == null)
        {
            return OperationResult<bool>.FailureResult(RawdahErrors.UserNotFound);
        }

        await studentRepository.Remove(student.Id);
        return OperationResult<bool>.SuccessResult(true);
    }
}

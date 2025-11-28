using FluentValidation;
using Mediator;
using Tarqeem.CA.Application.Contracts;
using Tarqeem.CA.Application.Contracts.Identity;
using Tarqeem.CA.Application.Contracts.Persistence;
using Tarqeem.CA.Application.Models.Common;
using Tarqeem.CA.Domain.Entities.Organization;
using Tarqeem.CA.SharedKernel.ValidationBase;
using Tarqeem.CA.SharedKernel.ValidationBase.Contracts;

namespace Tarqeem.CA.Application.Features.Rooms.Commands;

public record UpdateRoomUsersCommand(int RoomId, List<int> TeacherIds)
    : IRequest<OperationResult<bool>>,
        IValidatableModel<UpdateRoomUsersCommand>
{
    public IValidator<UpdateRoomUsersCommand> ValidateApplicationModel(
        ApplicationBaseValidationModelProvider<UpdateRoomUsersCommand> validator
    )
    {
        validator.RuleFor(x => x.RoomId).NotEmpty();
        validator.RuleFor(x => x.TeacherIds).NotNull();
        return validator;
    }
}

public record UpdateRoomStudentsCommand(int RoomId, List<int> StudentIds)
    : IRequest<OperationResult<bool>>,
        IValidatableModel<UpdateRoomStudentsCommand>
{
    public IValidator<UpdateRoomStudentsCommand> ValidateApplicationModel(
        ApplicationBaseValidationModelProvider<UpdateRoomStudentsCommand> validator
    )
    {
        validator.RuleFor(x => x.RoomId).NotEmpty();
        validator.RuleFor(x => x.StudentIds).NotNull();
        return validator;
    }
}

public class UpdateRoomStudentsCommandHandler(
    IRoomRepository roomRepo,
    IStudentRepository studentRepo,
    IUnitOfWork unitOfWork
) : IRequestHandler<UpdateRoomStudentsCommand, OperationResult<bool>>
{
    public async ValueTask<OperationResult<bool>> Handle(
        UpdateRoomStudentsCommand request,
        CancellationToken cancellationToken
    )
    {
        var r = await roomRepo.GetRoomWithStudentsAndTeachers(request.RoomId);
        if (r == null)
        {
            return OperationResult<bool>.FailureResult(RawdahErrors.RoomNotFound);
        }
        var nusers = new List<Student>();
        foreach (var s in request.StudentIds)
        {
            var ns = await studentRepo.GetStudentByIdAsync(s);
            if (ns == null)
            {
                return OperationResult<bool>.FailureResult(RawdahErrors.StudentNotFound);
            }
            nusers.Add(ns);
        }
        r.Students = nusers;
        await unitOfWork.CommitAsync();
        return OperationResult<bool>.SuccessResult(true);
    }
}

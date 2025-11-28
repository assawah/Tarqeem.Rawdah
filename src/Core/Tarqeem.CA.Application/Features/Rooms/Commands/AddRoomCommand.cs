using System.Text.Json.Serialization;
using FluentValidation;
using Mediator;
using Tarqeem.CA.Application.Models.Common;
using Tarqeem.CA.Application.Profiles;
using Tarqeem.CA.Domain.Entities.Organization;
using Tarqeem.CA.SharedKernel.ValidationBase;
using Tarqeem.CA.SharedKernel.ValidationBase.Contracts;

namespace Tarqeem.CA.Application.Features.Rooms.Commands;

public record AddRoomCommand
    : IRequest<OperationResult<AddRoomCommandResponse>>, IValidatableModel<AddRoomCommand>, ICreateMapper<Room>
{
    public IValidator<AddRoomCommand> ValidateApplicationModel(
        ApplicationBaseValidationModelProvider<AddRoomCommand> validator)
    {
        validator.RuleFor(x => x.Name).NotEmpty();
        return validator;
    }

    public string Name { get; set; }
    [JsonIgnore] public int OrganizationId { get; set; }
}
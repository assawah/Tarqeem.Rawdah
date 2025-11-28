#nullable enable
using System.Text.Json.Serialization;
using FluentValidation;
using Mediator;
using Microsoft.AspNetCore.Http;
using Tarqeem.CA.Application.Models.Common;
using Tarqeem.CA.Application.Profiles;
using Tarqeem.CA.Domain.Entities.Organization;
using Tarqeem.CA.SharedKernel.ValidationBase;
using Tarqeem.CA.SharedKernel.ValidationBase.Contracts;

namespace Tarqeem.CA.Application.Features.Students.Commands;

public record UpdateStudentCommand(
    int Id,
    string? FullName,
    string? PhoneNumber,
    int? Age,
    string? Note,
    IFormFile? File
) : IRequest<OperationResult<bool>>, IValidatableModel<UpdateStudentCommand>, ICreateMapper<Student>
{
    public IValidator<UpdateStudentCommand> ValidateApplicationModel(
        ApplicationBaseValidationModelProvider<UpdateStudentCommand> validator
    )
    {
        validator.RuleFor(x => x.Id).NotEqual(0);
        validator.RuleFor(x => x.FullName).NotEmpty().When(x => x.FullName != null);
        validator.RuleFor(x => x.PhoneNumber).NotEmpty().When(x => x.PhoneNumber != null);
        validator.RuleFor(x => x.Age).NotEqual(0).When(x => x.Age != null);
        validator.RuleFor(f => f.File).NotNull().When(f => f.File != null);
#pragma warning disable CS8602 // Dereference of a possibly null reference.
        // validator.RuleFor(f => f.File.Length).LessThan(5 * 1024 * 1024).When(f => f.File != null);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
        return validator;
    }
}

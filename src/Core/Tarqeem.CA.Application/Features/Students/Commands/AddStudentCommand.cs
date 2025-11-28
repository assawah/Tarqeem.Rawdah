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

public record AddStudentCommand
    : IRequest<OperationResult<int>>,
        IValidatableModel<AddStudentCommand>,
        ICreateMapper<Student>
{
    public string FullName { get; set; }
    public string PhoneNumber { get; set; }
    public string Address { get; set; }
    public int Age { get; set; }

    public string Note { get; set; }
    public IFormFile File { get; set; }

    [JsonIgnore]
    public int OrganizationId { get; set; }

    public IValidator<AddStudentCommand> ValidateApplicationModel(
        ApplicationBaseValidationModelProvider<AddStudentCommand> validator
    )
    {
        validator.RuleFor(x => x.FullName).NotEmpty();
        validator.RuleFor(x => x.PhoneNumber).NotEmpty();
        validator.RuleFor(x => x.Age).NotEqual(0);
        validator.RuleFor(f => f.File.Length).LessThan(5 * 1024 * 1024);
        return validator;
    }
}

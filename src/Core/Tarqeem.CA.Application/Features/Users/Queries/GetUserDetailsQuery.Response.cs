using System.Text.Json.Serialization;
using Tarqeem.CA.Application.Profiles;
using Tarqeem.CA.Domain.Entities.User;

namespace Tarqeem.CA.Application.Features.Users.Queries;

public record GetUserDetailsQueryResponse : ICreateMapper<User>
{
    
    [JsonPropertyName("userId")]
    public int Id { get; set; }
    public string UserName { get; set; }
    public string Name { get; set; }
    public string FamilyName { get; set; }
    public int Age { get; set; }
    public string Specialization { get; set; }
    public string Qualification { get; set; }
};
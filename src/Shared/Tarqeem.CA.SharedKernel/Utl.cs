using Microsoft.OpenApi.Models;

namespace Tarqeem.CA.SharedKernel;

public static class Utl
{

    public static (string Name, int Value) GetEnumInfo<TEnum>(TEnum enumMember) where TEnum : Enum
    {
        string name = enumMember.ToString(); // Get the name of the enum member
        int value = Convert.ToInt32(enumMember); // Get the value of the enum member
        return (name, value);
    }

    public static OpenApiOperation WriteOpenApiErrors<TEnum>(OpenApiOperation op, params TEnum[] errors)
    {
        foreach (var error in errors)
        {
            op.Responses["R" + Convert.ToInt32(error)] = new OpenApiResponse
                { Description = error.ToString() };
        }

        return op;
    }
}
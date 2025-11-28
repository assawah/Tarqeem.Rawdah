using Tarqeem.CA.Application.Models.Common;
using Microsoft.AspNetCore.Mvc;

namespace Tarqeem.CA.WebFramework.WebExtensions
{
    public static class ActionResultExtensions
    {
        public static IActionResult ToActionResult<TModel>(this OperationResult<TModel> result)
        {
            ArgumentNullException.ThrowIfNull(result, nameof(OperationResult<TModel>));

            if (result.IsSuccess) return result.Result is bool ? new OkResult() : new OkObjectResult(result.Result);

            if (result.IsNotFound) return new NotFoundResult();

            return new BadRequestObjectResult("Invalid Parameters. Please try again");
        }
    }
}

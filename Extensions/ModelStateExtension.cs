using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Blog.Extensions;

public static class ModelStateExtension
{
    public static List<string> GetErrors(this ModelStateDictionary modelState)
    {
        var result = new List<string>();
        foreach (var value in modelState.Values)
            result.AddRange(value.Errors.Select(err => err.ErrorMessage));

        return result;
    }
}

using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Identity;

namespace Adly.Application.Extensions;

public static class ApplicationIdentityExtensions
{
    public static List<KeyValuePair<string, string>> ConvertToKetKeyValuePair(
        [NotNull] this IEnumerable<IdentityError> errors)
    {
        return errors.Select(error => new KeyValuePair<string, string>("GeneralError", error.Description))
            .ToList();
    }

}
using System.ComponentModel.DataAnnotations;

namespace Adly.Application.Extensions;

public static class ApplicationStringExtensions
{
    public static bool IsEmail(this string email)
    {
        var emailValidation = new EmailAddressAttribute();
        return emailValidation.IsValid(email);
    }
}
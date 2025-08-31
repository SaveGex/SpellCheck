using SpellCheck.Enums;
using System.Text.RegularExpressions;

namespace SpellCheck.Services;

internal static class Determiner
{

    public static LoginTypes DetermineLoginType(string input)
    {
        // Regex for email
        string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        // Regex for phone number (basic example, adjust for your region)
        string phonePattern = @"^\+?[1-9]\d{1,14}$";

        if (Regex.IsMatch(input, emailPattern))
        {
            return LoginTypes.Email;
        }
        else if (Regex.IsMatch(input, phonePattern))
        {
            return LoginTypes.PhoneNumber;
        }
        else
        {
            return LoginTypes.Indefined;
        }
    }

}

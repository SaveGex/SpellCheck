namespace SpellCheck.Services.Converters;

public static class Base64Converter
{
    public static string StringToBase64(string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return string.Empty;
        }
        byte[] bytes = System.Text.Encoding.UTF8.GetBytes(input);
        return Convert.ToBase64String(bytes);
    }
}

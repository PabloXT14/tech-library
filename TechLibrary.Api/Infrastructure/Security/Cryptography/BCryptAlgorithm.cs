namespace TechLibrary.Api.Infrastructure.Security.Cryptography;

public class BCryptAlgorithm
{
    public static string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    public static bool Verify(string password, string hash)
    {
        return BCrypt.Net.BCrypt.Verify(password, hash);
    }
}
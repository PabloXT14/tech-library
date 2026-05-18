using TechLibrary.Api.Domain.Entities;

namespace TechLibrary.Api.Infrastructure.Security.Cryptography;

public class BCryptAlgorithm
{
    public static string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    public static bool Verify(string password, User user)
    {
        return BCrypt.Net.BCrypt.Verify(password, user.Password);
    }
}
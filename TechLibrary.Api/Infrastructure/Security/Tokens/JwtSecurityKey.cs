using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace TechLibrary.Api.Infrastructure.Security.Tokens;

public static class JwtSecurityKey
{
    public static SymmetricSecurityKey Generate(string signingKey)
    {
        var symmetricKey = Encoding.UTF8.GetBytes(signingKey);
        
        return new SymmetricSecurityKey(symmetricKey);
    } 
}
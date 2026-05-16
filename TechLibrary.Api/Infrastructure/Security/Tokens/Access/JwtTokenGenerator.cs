using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using TechLibrary.Api.Domain.Entities;

namespace TechLibrary.Api.Infrastructure.Security.Tokens.Access;

public class JwtTokenGenerator
{
    public string Generate(User user)
    {
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
        };
        
        // Descrevendo/Configurando nosso token, definindo data de expiração, algoritmo de criptografação e claims (payload) do token
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Expires = DateTime.UtcNow.AddHours(1),
            Subject = new ClaimsIdentity(claims),
            SigningCredentials = new SigningCredentials(SecurityKey(), SecurityAlgorithms.HmacSha256Signature)
        };

        // Criando o token a partir da descrição/configuração feita acima e convertendo ele para string
        var tokenHandler = new JwtSecurityTokenHandler();

        var securityToken = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(securityToken);
    }

    private SymmetricSecurityKey SecurityKey()
    {
        var signingKey = "uvoKT4tfbS6Ix8rTKJt23hfqlrhT3zTr"; // OBS: em produção salve essa key como variável de ambiente (mínimo de 32 caracteres)
        
        var symmetricKey = Encoding.UTF8.GetBytes(signingKey);
        
        return new SymmetricSecurityKey(symmetricKey);
    }
}
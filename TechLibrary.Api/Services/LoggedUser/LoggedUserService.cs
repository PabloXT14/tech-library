using System.IdentityModel.Tokens.Jwt;
using TechLibrary.Api.Domain.Entities;
using TechLibrary.Api.Infrastructure.DataAccess;

namespace TechLibrary.Api.Services.LoggedUser;

public class LoggedUserService
{
    private readonly HttpContext _httpContext;
    
    public LoggedUserService(HttpContext httpContext)
    {
        _httpContext = httpContext;
    }

    public User GetLoggedUser(TechLibraryDbContext dbContext)
    {
        
        // token format: "Bearer your-token"
        var authentication = _httpContext.Request.Headers.Authorization.ToString();
        
        // Remove Bearer word from auth token
        var token = authentication["Bearer ".Length..].Trim();

        var tokenHandler = new JwtSecurityTokenHandler();
        
        var jwtSecurityToken = tokenHandler.ReadJwtToken(token);
        
        var identifier = jwtSecurityToken.Claims.First(claim => claim.Type == JwtRegisteredClaimNames.Sub).Value;
            
        var userId = Guid.Parse(identifier);
            
        // Obs: estamos usando First direto pois para chegar nessa parte da lógica do negócio o usuário precisa necessariamente estar autenticado
        var user = dbContext.Users.First(user => user.Id == userId);
            
        return user;
    }
}
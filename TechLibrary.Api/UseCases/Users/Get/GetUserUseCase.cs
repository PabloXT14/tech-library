using TechLibrary.Api.Infrastructure.DataAccess;
using TechLibrary.Api.Services.LoggedUser;
using TechLibrary.Communication.Responses;

namespace TechLibrary.Api.UseCases.Users.Get;

public class GetUserUseCase
{
    private readonly LoggedUserService _loggedUserService;
    
    public GetUserUseCase(LoggedUserService loggedUserService)
    {
        _loggedUserService = loggedUserService;
    }
    
    public ResponseUserJson Execute()
    {
        var dbContext = new TechLibraryDbContext();
        
        var loggedUser = _loggedUserService.GetLoggedUser(dbContext);

        return new ResponseUserJson
        {
            Id = loggedUser.Id,
            Name = loggedUser.Name,
            Email = loggedUser.Email
        };
    }
}
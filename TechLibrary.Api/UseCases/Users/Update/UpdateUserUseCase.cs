using TechLibrary.Api.Domain.Entities;
using TechLibrary.Api.Infrastructure.DataAccess;
using TechLibrary.Api.Infrastructure.Security.Cryptography;
using TechLibrary.Api.Services.LoggedUser;
using TechLibrary.Communication.Requests;
using TechLibrary.Exception;

namespace TechLibrary.Api.UseCases.Users.Update;

public class UpdateUserUseCase
{
    private readonly LoggedUserService _loggedUserService;

    public UpdateUserUseCase(LoggedUserService loggedUserService)
    {
        _loggedUserService = loggedUserService;
    }

    public void Execute(RequestUpdateUserJson request)
    {
        var dbContext = new TechLibraryDbContext();
        
        var user = _loggedUserService.GetLoggedUser(dbContext);

        Validate(request, user);
        
        if (request.Name is not null)
            user.Name = request.Name;
        
        if (request.Email is not null)
            user.Email = request.Email;
        
        if (request.NewPassword is not null)
            user.Password = BCryptAlgorithm.HashPassword(request.NewPassword);
        
        dbContext.Users.Update(user);
        
        dbContext.SaveChanges();
    }

    private void Validate(RequestUpdateUserJson request, User user)
    {
        if (request.NewPassword == null)
            return;
        
        if (request.OldPassword == null)
        {
            throw new ErrorOnValidationException(["A senha antiga é obrigatória quando for definida uma nova senha."]);
        }

        if (request.NewPassword.Length < 6)
        {
            throw new ErrorOnValidationException(["A nova senha deve conter no mínimo 6 caracteres."]);
        }
        
        var isOldPasswordCorrect = BCryptAlgorithm.Verify(request.OldPassword, user);

        if (!isOldPasswordCorrect)
        {
            throw new ErrorOnValidationException(["A senha antiga está incorreta."]);
        }
    }
}
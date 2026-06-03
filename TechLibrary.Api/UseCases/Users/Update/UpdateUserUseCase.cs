using FluentValidation.Results;
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
        var validator = new UpdateUserValidator();
        
        var validationResult = validator.Validate(request);

        var isOldPasswordCorrect = request.OldPassword != null && BCryptAlgorithm.Verify(request.OldPassword, user);

        if (request?.OldPassword?.Length > 0 && !isOldPasswordCorrect)
        {
            validationResult.Errors.Add(new ValidationFailure("OldPassword", "A senha antiga está incorreta."));
        }
        
        if (!validationResult.IsValid)
        {
            var errorMessages = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
            
            throw new ErrorOnValidationException(errorMessages);
        }
    }
}
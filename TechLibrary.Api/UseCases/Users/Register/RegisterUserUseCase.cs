using TechLibrary.Api.Domain.Entities;
using TechLibrary.Api.Infrastructure.DataAccess;
using TechLibrary.Api.Infrastructure.Security.Cryptography;
using TechLibrary.Communication.Requests;
using TechLibrary.Communication.Responses;
using TechLibrary.Exception;

namespace TechLibrary.Api.UseCases.Users.Register;

public class RegisterUserUseCase
{
    public ResponseRegisteredUserJson Execute(RequestUserJson request)
    {
        Validate(request);
        
        var encryptedPassword = BCryptAlgorithm.HashPassword(request.Password);
        
        var entity = new User
        {
            Name = request.Name,
            Email = request.Email,
            Password = encryptedPassword
        };

        var dbContext = new TechLibraryDbContext();

        dbContext.Users.Add(entity);

        dbContext.SaveChanges();

        return new ResponseRegisteredUserJson
        {
            Name = entity.Name,
            AccessToken = Guid.NewGuid().ToString() // Gerar um token de acesso fictício
        };
    }

    private void Validate(RequestUserJson request)
    {
        var validator = new RegisterUserValidator();
        var validationResult = validator.Validate(request);

        if (!validationResult.IsValid)
        {
            var errorMessages = validationResult.Errors.Select(e => e.ErrorMessage).ToList();

            throw new ErrorOnValidationException(errorMessages);
        }
    }
}
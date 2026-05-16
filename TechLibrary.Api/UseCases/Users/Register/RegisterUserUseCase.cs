using FluentValidation.Results;
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
        var dbContext = new TechLibraryDbContext();
        
        Validate(request, dbContext);
        
        var encryptedPassword = BCryptAlgorithm.HashPassword(request.Password);
        
        var entity = new User
        {
            Name = request.Name,
            Email = request.Email,
            Password = encryptedPassword
        };

        dbContext.Users.Add(entity);

        dbContext.SaveChanges();

        return new ResponseRegisteredUserJson
        {
            Name = entity.Name,
            AccessToken = Guid.NewGuid().ToString() // Gerar um token de acesso fictício
        };
    }

    private void Validate(RequestUserJson request, TechLibraryDbContext dbContext)
    {
        var validator = new RegisterUserValidator();
        var validationResult = validator.Validate(request);

        var emailAlreadyExists = dbContext.Users.Any(user => user.Email.Equals(request.Email));

        if (emailAlreadyExists)
        {
            validationResult.Errors.Add(new ValidationFailure("Email", "Email já está em uso."));
        }
        
        if (!validationResult.IsValid)
        {
            var errorMessages = validationResult.Errors.Select(e => e.ErrorMessage).ToList();

            throw new ErrorOnValidationException(errorMessages);
        }
    }
}
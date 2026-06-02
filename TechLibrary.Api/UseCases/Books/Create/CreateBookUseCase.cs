using FluentValidation.Results;
using TechLibrary.Api.Domain.Entities;
using TechLibrary.Api.Infrastructure.DataAccess;
using TechLibrary.Communication.Requests;
using TechLibrary.Communication.Responses;
using TechLibrary.Exception;

namespace TechLibrary.Api.UseCases.Books.Create;

public class CreateBookUseCase
{
    public ResponseCreateBookJson Execute(RequestCreateBookJson request)
    {
        var dbContext = new TechLibraryDbContext();
        
        Validate(request, dbContext);
        
        var book = new Book
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            Author = request.Author,
            Amount = request.Amount,
        };

        dbContext.Books.Add(book);
        dbContext.SaveChanges();

        var response = new ResponseCreateBookJson
        {
            BookId = book.Id,
        };

        return response;
    }

    private void Validate(RequestCreateBookJson request, TechLibraryDbContext dbContext)
    {
        var validator = new CreateBookValidator();

        var validationResult = validator.Validate(request);
        
        var bookAlreadyExists = dbContext.Books.Any(book => 
            book.Title.ToLower().Equals(request.Title.ToLower()) && 
            book.Author.ToLower().Equals(request.Author.ToLower())
        );

        if (bookAlreadyExists)
        {
            validationResult.Errors.Add(
                new ValidationFailure("Título", "O livro já foi registrado.")
            );
        }

        if (!validationResult.IsValid)
        {
            var errorMessages = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
            
            throw new ErrorOnValidationException(errorMessages);
        }
            
    }
}
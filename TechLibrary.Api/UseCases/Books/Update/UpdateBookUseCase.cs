using TechLibrary.Api.Infrastructure.DataAccess;
using TechLibrary.Communication.Requests;
using TechLibrary.Exception;

namespace TechLibrary.Api.UseCases.Books.Update;

public class UpdateBookUseCase
{
    public void Execute(RequestUpdateBookJson request, Guid bookId)
    {
        var dbContext = new TechLibraryDbContext();
        
        var book = dbContext.Books.FirstOrDefault(book => book.Id == bookId);

        if (book is null)
        {
            throw new NotFoundException("O livro não foi  encontrado.");
        }
        
        Validate(request);

        if (request.Title is not null)
        {
            book.Title = request.Title;
        }

        if (request.Author is not null)
        {
            book.Author = request.Author;
        }

        if (request.Amount is not null)
        {
            book.Amount = (int)request.Amount;
        }
        
        dbContext.Books.Update(book);
        
        dbContext.SaveChanges();
    }

    private void Validate(RequestUpdateBookJson request)
    {
        var validator = new UpdateBookValidator();
        
        var validationResult = validator.Validate(request);

        if (!validationResult.IsValid)
        {
            var errorMessages = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
            
            throw new ErrorOnValidationException(errorMessages);
        }
    }
}
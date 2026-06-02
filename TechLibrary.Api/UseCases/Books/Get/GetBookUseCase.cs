using TechLibrary.Api.Infrastructure.DataAccess;
using TechLibrary.Communication.Responses;
using TechLibrary.Exception;

namespace TechLibrary.Api.UseCases.Books.Get;

public class GetBookUseCase
{
    public ResponseBookJson Execute(Guid bookId)
    {
        var dbContext = new TechLibraryDbContext();
        
        var book = dbContext.Books.FirstOrDefault(book => book.Id.Equals(bookId));

        if (book is null)
        {
            throw new NotFoundException("Livro não encontrado.");
        }

        return new ResponseBookJson
        {
            Id = book.Id,
            Title = book.Title,
            Author = book.Author,
            Amount = book.Amount
        };
    }
}
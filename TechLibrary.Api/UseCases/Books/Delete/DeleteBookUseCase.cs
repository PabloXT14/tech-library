using TechLibrary.Api.Infrastructure.DataAccess;
using TechLibrary.Exception;

namespace TechLibrary.Api.UseCases.Books.Delete;

public class DeleteBookUseCase
{
    public void Execute(Guid bookId)
    {
        var dbContext = new TechLibraryDbContext();
        
        var book = dbContext.Books.FirstOrDefault(book => book.Id == bookId);

        if (book is null)
        {
            throw new NotFoundException("O livro informado não foi encontrado.");
        }
        
        dbContext.Books.Remove(book);
        
        dbContext.SaveChanges();
    }
}
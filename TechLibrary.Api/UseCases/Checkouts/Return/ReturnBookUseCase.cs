using TechLibrary.Api.Domain.Entities;
using TechLibrary.Api.Infrastructure.DataAccess;
using TechLibrary.Api.Services.LoggedUser;
using TechLibrary.Exception;

namespace TechLibrary.Api.UseCases.Checkouts.Return;

public class ReturnBookUseCase
{
    private readonly LoggedUserService _loggedUserServer;
    
    public ReturnBookUseCase(LoggedUserService loggedUserServer)
    {
        _loggedUserServer = loggedUserServer;
    }
    
    public void Execute(Guid bookId)
    {
        var dbContext = new TechLibraryDbContext();

        var user = _loggedUserServer.GetLoggedUser(dbContext);
        
        Validate(dbContext, bookId, user);
        
        var checkout = dbContext.Checkouts.First(checkout => 
            checkout.BookId == bookId &&
            checkout.UserId == user.Id &&
            checkout.ReturnedDate == null
        );
        
        checkout.ReturnedDate = DateTime.UtcNow;
        
        dbContext.Checkouts.Update(checkout);
        
        dbContext.SaveChanges();
    }

    private void Validate(TechLibraryDbContext dbContext, Guid bookId, User loggedUser)
    {
        var bookExists = dbContext.Books.Any(book => book.Id == bookId);
        
        if  (!bookExists)
            throw new NotFoundException("Livro não encontrado.");
        
        var checkout = dbContext.Checkouts.FirstOrDefault(checkout => 
            checkout.BookId == bookId &&
            checkout.UserId == loggedUser.Id
        );
        
        if (checkout is null)
            throw new NotFoundException("Empréstimo do livro não encontrado para o usuário logado.");
        
        var bookWasAlreadyReturned = checkout.ReturnedDate != null;
        
        if (bookWasAlreadyReturned)
            throw new ConflictException("Livro já foi devolvido.");
    }
}
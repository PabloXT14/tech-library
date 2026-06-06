using TechLibrary.Api.Domain.Entities;
using TechLibrary.Api.Infrastructure.DataAccess;
using TechLibrary.Api.Services.LoggedUser;
using TechLibrary.Exception;

namespace TechLibrary.Api.UseCases.Checkouts.Register;

public class RegisterBookCheckoutUseCase
{
    private const int MAX_LOAN_DAYS = 7;
    
    private readonly LoggedUserService _loggedUserService;

    public RegisterBookCheckoutUseCase(LoggedUserService loggedUserService)
    {
        _loggedUserService = loggedUserService;
    }
    
    public void Execute(Guid bookId)
    {
        var dbContext = new TechLibraryDbContext();
        
        var user =  _loggedUserService.GetLoggedUser(dbContext);
        
        Validate(dbContext, bookId, user);

        var entity = new Checkout
        {
            BookId = bookId,
            UserId = user.Id,
            ExpectedReturnDate = DateTime.UtcNow.AddDays(MAX_LOAN_DAYS)
        };
        
        dbContext.Checkouts.Add(entity);
        
        dbContext.SaveChanges();
    }

    private void Validate(TechLibraryDbContext dbContext, Guid bookId, User user)
    {
        var book = dbContext.Books.FirstOrDefault(book => book.Id == bookId);
        
        if (book is null)
            throw new NotFoundException("Livro não encontrado.");
        
        var amountBookNotReturned = dbContext
            .Checkouts
            .Count(checkout => checkout.BookId == bookId && checkout.ReturnedDate == null);
        
        if (amountBookNotReturned >= book.Amount)
            throw new ConflictException("Não há exemplares disponíveis para empréstimo.");
        
        var useAlreadyReservedBook = dbContext
            .Checkouts
            .Any(checkout => checkout.BookId == bookId && checkout.UserId == user.Id && checkout.ReturnedDate == null);
        
        if  (useAlreadyReservedBook)
            throw new ConflictException("Usuário já possui um exemplar reservado deste livro.");
    }
}
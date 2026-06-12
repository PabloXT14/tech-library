using TechLibrary.Api.Infrastructure.DataAccess;
using TechLibrary.Api.Services.LoggedUser;
using TechLibrary.Exception;

namespace TechLibrary.Api.UseCases.Checkouts.Extend;

public class ExtendCheckoutUseCase
{
  private const int MAX_LOAN_DAYS = 7;
  
  private readonly LoggedUserService _loggedUserService;

  public ExtendCheckoutUseCase(LoggedUserService  loggedUserService)
  {
    _loggedUserService = loggedUserService;
  }
  
  public void Execute(Guid checkoutId)
  {
    var dbContext = new TechLibraryDbContext();
    
    var user =  _loggedUserService.GetLoggedUser(dbContext);

    var checkout = dbContext.Checkouts.FirstOrDefault(checkout => checkout.Id == checkoutId && checkout.UserId ==  user.Id);

    if (checkout is null)
    {
      throw new NotFoundException("Empréstimo do livro não encontrado para o usuário logado.");
    }
    
    var isCheckoutAlreadyReturned = checkout.ReturnedDate != null;

    if (isCheckoutAlreadyReturned)
    {
      throw new ConflictException("O livro deste empréstimo já foi devolvido. Não é possível estender um empréstimo de livro que já foi finalizado.");
    }
    
    var isCheckoutOverdue = checkout.ExpectedReturnDate < DateTime.UtcNow;
    
    if (isCheckoutOverdue)
    {
      throw new ConflictException("O livro deste empréstimo está atrasado. Não é possível estender um empréstimo de livro que já está atrasado.");
    }
    
    checkout.ExpectedReturnDate = checkout.ExpectedReturnDate.AddDays(MAX_LOAN_DAYS);
    
    dbContext.Checkouts.Update(checkout);
    
    dbContext.SaveChanges();
  }
}
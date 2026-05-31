using TechLibrary.Api.Infrastructure.DataAccess;
using TechLibrary.Communication.Responses;
using TechLibrary.Exception;

namespace TechLibrary.Api.UseCases.Users.Delete;

public class DeleteUserUseCase
{
    public void Execute(Guid userId)
    {
        var dbContext = new TechLibraryDbContext();
        
        var user = dbContext.Users.FirstOrDefault(u => u.Id == userId);

        if (user == null)
        {
            throw new NotFoundException("Usuário não encontrado.");
        }
        
        var isUserActive = user.IsActive;

        if (!isUserActive)
        {
            throw new NotActiveException();
        }

        var userHasCheckoutsPending = dbContext.Checkouts.Any(c => c.UserId == userId && c.ReturnedDate == null);
        
        if (userHasCheckoutsPending)
        {
            throw new ConflictException("O usuário possui empréstimos pendentes e não pode ser excluído.");
        }
        
        user.IsActive = false;    
        
        dbContext.Users.Update(user);
        dbContext.SaveChanges();
    }
}
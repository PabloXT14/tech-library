using TechLibrary.Api.Infrastructure.DataAccess;
using TechLibrary.Exception;

namespace TechLibrary.Api.UseCases.Users.Activate;

public class ActivateUserUseCase
{
    public void Execute(Guid userId)
    {
        var dbContext = new TechLibraryDbContext();
        
        var user = dbContext.Users.FirstOrDefault(user => user.Id == userId);
        
        if (user is null)
            throw new NotFoundException("Usuário não encontrado.");

        user.IsActive = true;
        
        dbContext.Users.Update(user);
        
        dbContext.SaveChanges();
    }
}
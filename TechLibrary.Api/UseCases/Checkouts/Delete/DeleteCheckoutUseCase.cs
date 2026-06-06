using TechLibrary.Api.Infrastructure.DataAccess;
using TechLibrary.Exception;

namespace TechLibrary.Api.UseCases.Checkouts.Delete;

public class DeleteCheckoutUseCase
{
    public void Execute(Guid checkoutId)
    {
        var dbContext = new TechLibraryDbContext();

        var checkout = dbContext.Checkouts.FirstOrDefault(checkout => checkout.Id == checkoutId);

        if (checkout is null)
            throw new NotFoundException("Empréstimo de livro não encontrado.");

        var checkoutBookWasAlreadyReturned = checkout.ReturnedDate != null;

        if (!checkoutBookWasAlreadyReturned)
        {
            throw new ConflictException("O livro deste empréstimo ainda não foi devolvido. Não é possível excluir um empréstimo de livro que ainda não foi finalizado.");
        }

        dbContext.Checkouts.Remove(checkout);

        dbContext.SaveChanges();
    }
}
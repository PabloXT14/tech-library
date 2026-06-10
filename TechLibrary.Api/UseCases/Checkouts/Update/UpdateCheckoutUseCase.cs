using TechLibrary.Api.Domain.Entities;
using TechLibrary.Api.Infrastructure.DataAccess;
using TechLibrary.Api.Services.LoggedUser;
using TechLibrary.Communication.Requests;
using TechLibrary.Exception;

namespace TechLibrary.Api.UseCases.Checkouts.Update;

public class UpdateCheckoutUseCase
{
    
    private readonly LoggedUserService _loggedUserServer;
    
    public UpdateCheckoutUseCase(LoggedUserService loggedUserServer)
    {
        _loggedUserServer = loggedUserServer;
    }
    
    public void Execute(Guid checkoutId, RequestUpdateCheckoutJson request)
    {
        var dbContext = new TechLibraryDbContext();
        
        var user = _loggedUserServer.GetLoggedUser(dbContext);
        
        var checkout = dbContext.Checkouts.FirstOrDefault(checkout => checkout.Id == checkoutId && checkout.UserId == user.Id);
        
        Validate(checkout, request);
        
        if (request.ExpectedReturnDate != null)
        {
            checkout!.ExpectedReturnDate = (DateTime)request.ExpectedReturnDate;
        }

        if (request.ReturnedDate != null)
        {
            checkout!.ReturnedDate = (DateTime)request.ReturnedDate;
        }
        
        dbContext.Checkouts.Update(checkout!);
        
        dbContext.SaveChanges();
    }

    private void Validate(Checkout? checkout, RequestUpdateCheckoutJson request)
    {
        if (checkout == null)
        {
            throw new NotFoundException("Empréstimo do livro não encontrado para o usuário logado.");
        }
        
        var validator = new UpdateCheckoutValidator(checkout);
        
        var validationResult = validator.Validate(request);

        if (!validationResult.IsValid)
        {
            var errorMessages =  validationResult.Errors.Select(e => e.ErrorMessage).ToList();
            
            throw new ErrorOnValidationException(errorMessages);
        }
    }
}
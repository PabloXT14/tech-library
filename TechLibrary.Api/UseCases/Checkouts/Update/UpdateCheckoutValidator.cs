using FluentValidation;
using TechLibrary.Api.Domain.Entities;
using TechLibrary.Api.Infrastructure.DataAccess;
using TechLibrary.Communication.Requests;

namespace TechLibrary.Api.UseCases.Checkouts.Update;

public class UpdateCheckoutValidator : AbstractValidator<RequestUpdateCheckoutJson>
{
    public  UpdateCheckoutValidator(Checkout checkout)
    {
        // Garante que ao menos um campo foi enviado
        RuleFor(request => request)
            .Must(HaveAtLeastOneField)
            .WithMessage("Pelo menos um campo deve ser enviado para atualização.")
            .WithName("Request");
        
        When(request => request.ExpectedReturnDate is not null, () =>
        {
            RuleFor(request => request.ExpectedReturnDate)
                .NotEmpty()
                .WithMessage("A data de retorno esperada não pode ser vazia.")
                .GreaterThan(DateTime.Now)
                .WithMessage("A data de retorno esperada deve ser uma data futura.")
                .GreaterThan(checkout.CheckoutDate)
                .WithMessage("A data de retorno esperada deve ser posterior à data de checkout.");
        });
        
        When(request => request.ReturnedDate is not null, () =>
        {
            RuleFor(request => request.ReturnedDate)
                .NotEmpty()
                .WithMessage("A data de retorno não pode ser vazia.")
                .GreaterThan(checkout.CheckoutDate)
                .WithMessage("A data de retorno deve ser posterior à data de checkout.");
        });
    }

    private bool HaveAtLeastOneField(RequestUpdateCheckoutJson request)
    {
        return request.ExpectedReturnDate is not null
               || request.ReturnedDate is not null;
    }
}
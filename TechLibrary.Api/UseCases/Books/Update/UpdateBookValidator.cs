using FluentValidation;
using TechLibrary.Communication.Requests;

namespace TechLibrary.Api.UseCases.Books.Update;

public class UpdateBookValidator : AbstractValidator<RequestUpdateBookJson>
{
    public UpdateBookValidator()
    {
        // Garante que ao menos um campo foi enviado
        RuleFor(request => request)
            .Must(HaveAtLeastOneField)
            .WithMessage("Pelo menos um campo deve ser enviado para atualização.")
            .WithName("Request");

        When(request => request.Title is not null, () =>
        {
            RuleFor(request => request.Title)
                .NotEmpty()
                .WithMessage("O título não pode ser vazio.")
                .MaximumLength(200)
                .WithMessage("O título deve conter no máximo 200 caracteres.");
        });
        
        When(request => request.Author is not null, () =>
        {
            RuleFor(request => request.Author)
                .NotEmpty()
                .WithMessage("O nome do autor não pode ser vazio.")
                .MaximumLength(100)
                .WithMessage("O autor deve conter no máximo 100 caracteres.");
        });
        
        When(request => request.Amount is not null, () =>
        {
            RuleFor(request => request.Amount)
                .GreaterThan(0)
                .WithMessage("A quantidade deve ser maior que zero.");
        });
    }

    private bool HaveAtLeastOneField(RequestUpdateBookJson request)
    {
        return request.Title is not null
               || request.Author is not null
               || request.Amount is not null;
    }
}
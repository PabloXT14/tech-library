using FluentValidation;
using TechLibrary.Communication.Requests;

namespace TechLibrary.Api.UseCases.Books.Create;

public class CreateBookValidator : AbstractValidator<RequestCreateBookJson>
{
    public CreateBookValidator()
    {
        RuleFor(request => request.Title)
            .NotEmpty()
            .WithMessage("O título é obrigatório.")
            .MaximumLength(200)
            .WithMessage("O título deve conter no máximo 200 caracteres.");
        
        RuleFor(request => request.Author)
            .NotEmpty()
            .WithMessage("O autor é obrigatório.")
            .MaximumLength(100)
            .WithMessage("O autor deve conter no máximo 100 caracteres.");

        RuleFor(request => request.Amount)
            .GreaterThan(0)
            .WithMessage("A quantidade deve ser maior que zero.");
    }
}
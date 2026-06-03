using FluentValidation;
using TechLibrary.Communication.Requests;

namespace TechLibrary.Api.UseCases.Users.Update;

public class UpdateUserValidator : AbstractValidator<RequestUpdateUserJson>
{
    public UpdateUserValidator()
    {
        RuleFor(request => request)
            .Must(HaveAtLeastOneField)
            .WithMessage("Pelo menos um campo deve ser enviado para atualização.")
            .WithName("Request");

        When(request => request.Name is not null, () =>
        {
            RuleFor(request => request.Name)
                .NotEmpty()
                .WithMessage("O nome não pode ser vazio.")
                .MaximumLength(100)
                .WithMessage("O nome deve conter no máximo 100 caracteres.");
        });
        
        When(request => request.Email is not null, () =>
        {
            RuleFor(request => request.Email)
                .EmailAddress()
                .WithMessage("O email não é válido.");
        });
        
        When(request => request.NewPassword is not null, () =>
        {
            RuleFor(request => request.NewPassword)
                .MinimumLength(6)
                .WithMessage("A nova senha deve conter no mínimo 6 caracteres.");
            
            RuleFor(request => request.OldPassword)
                .NotEmpty()
                .WithMessage("A senha antiga é obrigatória quando for definida uma nova senha.");
        });
    }
    
    private bool HaveAtLeastOneField(RequestUpdateUserJson request)
    {
        return request.Name is not null
               || request.Email is not null
               || request.NewPassword is not null
               || request.OldPassword is not null;
    }
}
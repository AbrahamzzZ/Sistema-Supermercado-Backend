using Domain.Models;
using FluentValidation;
using Utilities.Shared;

namespace Infrastructure.Services.Validators
{
    public class CategoriaValidator : AbstractValidator<Categorium>
    {
        public CategoriaValidator()
        {
            RuleFor(c => c.Codigo).NotEmpty().WithMessage(Mensajes.MESSAGE_EMPTY).When(c => c.Id_Categoria == 0);
            RuleFor(c => c.Nombre_Categoria).NotEmpty().WithMessage(Mensajes.MESSAGE_EMPTY).Matches("^[a-zA-ZáéíóúÁÉÍÓÚñÑ ]+$").WithMessage(Mensajes.MESSAGE_ONLY_LETTERS);
            RuleFor(c => c.Nombre_Categoria).MinimumLength(3).WithMessage(Mensajes.MESSAGE_MIN_LENGTH);
        }
    }
}

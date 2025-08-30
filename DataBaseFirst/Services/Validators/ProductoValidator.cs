using DataBaseFirst.Models;
using FluentValidation;
using Utilities.Shared;

namespace DataBaseFirst.Services.Validators
{
    public class ProductoValidator : AbstractValidator<Producto>
    {
        public ProductoValidator()
        {
            RuleFor(p => p.Codigo).NotEmpty().WithMessage(Mensajes.MESSAGE_EMPTY).When(p => p.Id_Producto == 0);
            RuleFor(p => p.Nombre_Producto).NotEmpty().WithMessage(Mensajes.MESSAGE_EMPTY).Matches("^[a-zA-ZáéíóúÁÉÍÓÚñÑ ]+$").WithMessage(Mensajes.MESSAGE_ONLY_LETTERS);
            RuleFor(p => p.Pais_Origen).NotEmpty().WithMessage(Mensajes.MESSAGE_EMPTY).Matches("^[a-zA-ZáéíóúÁÉÍÓÚñÑ ]+$").WithMessage(Mensajes.MESSAGE_ONLY_LETTERS);
            RuleFor(p => p.Descripcion).NotEmpty().WithMessage(Mensajes.MESSAGE_EMPTY);
            RuleFor(p => p.Nombre_Producto).MinimumLength(3).WithMessage(Mensajes.MESSAGE_MIN_LENGTH);
            RuleFor(p => p.Descripcion).MinimumLength(3).WithMessage(Mensajes.MESSAGE_MIN_LENGTH);
            RuleFor(p => p.Descripcion).MinimumLength(3).WithMessage(Mensajes.MESSAGE_MIN_LENGTH);
        }
    }
}

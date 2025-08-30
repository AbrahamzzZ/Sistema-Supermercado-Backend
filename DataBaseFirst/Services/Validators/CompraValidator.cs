using DataBaseFirst.Models.Dto;
using FluentValidation;
using Utilities.Shared;

namespace DataBaseFirst.Services.Validators
{
    public class CompraValidator : AbstractValidator<Compras>
    {
        public CompraValidator()
        {
            RuleFor(c => c.Tipo_Documento).NotEmpty().WithMessage(Mensajes.MESSAGE_EMPTY).Matches("^[a-zA-ZáéíóúÁÉÍÓÚñÑ ]+$").WithMessage(Mensajes.MESSAGE_ONLY_LETTERS);
            RuleFor(c => c.Monto_Total).GreaterThan(0).WithMessage("El monto total debe ser mayor que 0");
            RuleFor(c => c.Numero_Documento).NotEmpty().WithMessage(Mensajes.MESSAGE_EMPTY);
            RuleFor(c => c.Detalles).NotEmpty().WithMessage(Mensajes.MESSAGE_DETAILS_EMPTY);
        }
    }
}

using DataBaseFirst.Models.Dto;
using FluentValidation;
using Utilities.Shared;

namespace Infrastructure.Services.Validators
{
    public class VentaValidator : AbstractValidator<Ventas>
    {
        public VentaValidator()
        {
            RuleFor(v => v.Tipo_Documento).NotEmpty().WithMessage(Mensajes.MESSAGE_EMPTY).Matches("^[a-zA-ZáéíóúÁÉÍÓÚñÑ ]+$").WithMessage(Mensajes.MESSAGE_ONLY_LETTERS);
            RuleFor(v => v.Monto_Total).GreaterThan(0).WithMessage("El monto total debe ser mayor que 0");
            RuleFor(v => v.Monto_Pago).GreaterThan(0).WithMessage("El monto pago debe ser mayor que 0");
            RuleFor(v => v.Numero_Documento).NotEmpty().WithMessage(Mensajes.MESSAGE_EMPTY);
            RuleFor(v => v.Detalles).NotEmpty().WithMessage(Mensajes.MESSAGE_DETAILS_EMPTY);
        }
    }
}

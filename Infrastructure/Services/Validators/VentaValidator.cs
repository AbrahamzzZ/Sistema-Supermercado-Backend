using Domain.Models.Dto.Response.Venta;
using FluentValidation;
using Utilities.Shared;

namespace Infrastructure.Services.Validators
{
    public class VentaValidator : AbstractValidator<Ventas>
    {
        public VentaValidator()
        {
            RuleFor(v => v.Id_Sucursal).GreaterThan(0).WithMessage("Debe seleccionar una sucursal.");
            RuleFor(v => v.Id_Cliente).GreaterThan(0).WithMessage("Debe seleccionar un cliente.");
            RuleFor(v => v.Tipo_Documento).NotEmpty().WithMessage(Mensajes.MESSAGE_EMPTY).Matches("^[a-zA-ZáéíóúÁÉÍÓÚñÑ ]+$").WithMessage(Mensajes.MESSAGE_ONLY_LETTERS);
            RuleFor(v => v.Monto_Pago).GreaterThan(0).WithMessage("El monto pago debe ser mayor que 0");
            RuleFor(v => v.Detalles).NotEmpty().WithMessage(Mensajes.MESSAGE_DETAILS_EMPTY);
            RuleForEach(v => v.Detalles).ChildRules(detalle =>
            {
                detalle.RuleFor(d => d.Id_Producto).GreaterThan(0).WithMessage("Cada línea debe indicar un producto.");
                detalle.RuleFor(d => d.Cantidad).GreaterThan(0).WithMessage("La cantidad de cada producto debe ser mayor que 0.");
            });
        }
    }
}

using Domain.Models.Dto.Response.Compra;
using FluentValidation;
using Utilities.Shared;

namespace Infrastructure.Services.Validators
{
    public class CompraValidator : AbstractValidator<Compras>
    {
        public CompraValidator()
        {
            RuleFor(c => c.Id_Sucursal).GreaterThan(0).WithMessage("Debe seleccionar una sucursal.");
            RuleFor(c => c.Id_Proveedor).GreaterThan(0).WithMessage("Debe seleccionar un proveedor.");
            RuleFor(c => c.Id_Transportista).GreaterThan(0).WithMessage("Debe seleccionar un transportista.");
            RuleFor(c => c.Tipo_Documento).NotEmpty().WithMessage(Mensajes.MESSAGE_EMPTY).Matches("^[a-zA-ZáéíóúÁÉÍÓÚñÑ ]+$").WithMessage(Mensajes.MESSAGE_ONLY_LETTERS);
            RuleFor(c => c.Monto_Total).GreaterThan(0).WithMessage("El monto total debe ser mayor que 0");
            RuleFor(c => c.Numero_Documento).NotEmpty().WithMessage(Mensajes.MESSAGE_EMPTY);
            RuleFor(c => c.Detalles).NotEmpty().WithMessage(Mensajes.MESSAGE_DETAILS_EMPTY);
            RuleForEach(c => c.Detalles).ChildRules(detalle =>
            {
                detalle.RuleFor(d => d.Id_Producto).GreaterThan(0).WithMessage("Cada línea debe indicar un producto.");
                detalle.RuleFor(d => d.Cantidad).GreaterThan(0).WithMessage("La cantidad de cada producto debe ser mayor que 0.");
                detalle.RuleFor(d => d.Precio_Compra).GreaterThan(0).WithMessage("El precio de compra debe ser mayor que 0.");
                detalle.RuleFor(d => d.Precio_Venta).GreaterThan(0).WithMessage("El precio de venta debe ser mayor que 0.");
                detalle.RuleFor(d => d.SubTotal)
                    .Must((d, subtotal) => Math.Round(d.Precio_Compra * d.Cantidad, 2) == Math.Round(subtotal, 2))
                    .WithMessage("El subtotal de cada línea debe ser precio de compra × cantidad.");
            });
        }
    }
}

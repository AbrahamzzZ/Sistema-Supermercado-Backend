using DataBaseFirst.Models;
using FluentValidation;
using Utilities.Shared;

namespace Infrastructure.Services.Validators
{
    public class OfertaValidator : AbstractValidator<Ofertum>
    {
        public OfertaValidator()
        {
            RuleFor(o => o.Codigo).NotEmpty().WithMessage(Mensajes.MESSAGE_EMPTY).When(o => o.Id_Oferta == 0);
            RuleFor(o => o.Nombre_Oferta).NotEmpty().WithMessage(Mensajes.MESSAGE_EMPTY);
            RuleFor(o => o.Descripcion).NotEmpty().WithMessage(Mensajes.MESSAGE_EMPTY);
            RuleFor(o => o.Descuento).InclusiveBetween(0, 100).WithMessage("El descuento debe estar entre 0 y 100");
            RuleFor(o => o.Fecha_Fin).Must(fecha => fecha > DateOnly.FromDateTime(DateTime.Now)).WithMessage("La fecha de fin debe ser una fecha futura");
        }
    }
}

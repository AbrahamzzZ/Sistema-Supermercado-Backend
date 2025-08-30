using DataBaseFirst.Models;
using FluentValidation;
using Utilities.Shared;

namespace DataBaseFirst.Services.Validators
{
    public class SucursalValidator : AbstractValidator<Sucursal>
    {
        public SucursalValidator()
        {
            RuleFor(s => s.Codigo).NotEmpty().WithMessage(Mensajes.MESSAGE_EMPTY).When(s => s.Id_Sucursal == 0);
            RuleFor(s => s.Nombre_Sucursal).NotEmpty().WithMessage(Mensajes.MESSAGE_EMPTY);
            RuleFor(s => s.Direccion_Sucursal).NotEmpty().WithMessage(Mensajes.MESSAGE_EMPTY);
            RuleFor(s => s.Ciudad_Sucursal).NotEmpty().WithMessage(Mensajes.MESSAGE_EMPTY).Matches("^[a-zA-ZáéíóúÁÉÍÓÚñÑ ]+$").WithMessage(Mensajes.MESSAGE_ONLY_LETTERS);
            RuleFor(s => s.Latitud).InclusiveBetween(-90, 90).WithMessage("Ingrese una latitud válida (ej: -2.224610).");
            RuleFor(s => s.Longitud).InclusiveBetween(-180, 180).WithMessage("Ingrese una longitud válida (ej: -79.897900).");
        }
    }
}

using Domain.Models;
using FluentValidation;
using Utilities.Shared;

namespace Infrastructure.Services.Validators
{
    public class ProveedorValidator : AbstractValidator<Proveedor>
    {
        public ProveedorValidator()
        {
            RuleFor(p => p.Codigo).NotEmpty().WithMessage(Mensajes.MESSAGE_EMPTY).When(p => p.Id_Proveedor == 0);
            RuleFor(p => p.Nombres).NotEmpty().WithMessage(Mensajes.MESSAGE_EMPTY).Matches("^[a-zA-ZáéíóúÁÉÍÓÚñÑ ]+$").WithMessage(Mensajes.MESSAGE_ONLY_LETTERS).MinimumLength(3).WithMessage(Mensajes.MESSAGE_MIN_LENGTH);
            RuleFor(p => p.Apellidos).NotEmpty().WithMessage(Mensajes.MESSAGE_EMPTY).Matches("^[a-zA-ZáéíóúÁÉÍÓÚñÑ ]+$").WithMessage(Mensajes.MESSAGE_ONLY_LETTERS).MinimumLength(3).WithMessage(Mensajes.MESSAGE_MIN_LENGTH);
            RuleFor(p => p.Correo_Electronico).NotEmpty().WithMessage(Mensajes.MESSAGE_EMPTY).EmailAddress().WithMessage(Mensajes.MESSAGE_EMAIL);
            RuleFor(p => p.Cedula).NotEmpty().WithMessage(Mensajes.MESSAGE_EMPTY).Matches(@"^\d{10}$").WithMessage(Mensajes.MESSAGE_CEDULA);
            RuleFor(p => p.Telefono).NotEmpty().WithMessage(Mensajes.MESSAGE_EMPTY).Matches(@"^\d{10}$").WithMessage(Mensajes.MESSAGE_PHONE);
        }
    }
}

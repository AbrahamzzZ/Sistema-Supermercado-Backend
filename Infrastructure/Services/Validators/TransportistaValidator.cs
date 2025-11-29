using Domain.Models;
using FluentValidation;
using Utilities.Shared;

namespace Infrastructure.Services.Validators
{
    public class TransportistaValidator : AbstractValidator<Transportistum>
    {
        public TransportistaValidator()
        {
            RuleFor(t => t.Codigo).NotEmpty().WithMessage(Mensajes.MESSAGE_EMPTY).When(t => t.Id_Transportista == 0);
            RuleFor(t => t.Nombres).NotEmpty().WithMessage(Mensajes.MESSAGE_EMPTY).Matches("^[a-zA-ZáéíóúÁÉÍÓÚñÑ ]+$").WithMessage(Mensajes.MESSAGE_ONLY_LETTERS);
            RuleFor(t => t.Apellidos).NotEmpty().WithMessage(Mensajes.MESSAGE_EMPTY).Matches("^[a-zA-ZáéíóúÁÉÍÓÚñÑ ]+$").WithMessage(Mensajes.MESSAGE_ONLY_LETTERS);
            RuleFor(t => t.Correo_Electronico).NotEmpty().WithMessage(Mensajes.MESSAGE_EMPTY).EmailAddress().WithMessage(Mensajes.MESSAGE_EMAIL);
            RuleFor(t => t.Cedula).NotEmpty().WithMessage(Mensajes.MESSAGE_EMPTY).Matches(@"^\d{10}$").WithMessage(Mensajes.MESSAGE_CEDULA);
            RuleFor(t => t.Telefono).NotEmpty().WithMessage(Mensajes.MESSAGE_EMPTY).Matches(@"^\d{10}$").WithMessage(Mensajes.MESSAGE_PHONE);
            RuleFor(t => t.Nombres).MinimumLength(3).WithMessage(Mensajes.MESSAGE_MIN_LENGTH);
            RuleFor(t => t.Apellidos).MinimumLength(3).WithMessage(Mensajes.MESSAGE_MIN_LENGTH);
        }
    }
}

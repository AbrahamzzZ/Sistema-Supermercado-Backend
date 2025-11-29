using Domain.Models;
using FluentValidation;
using Utilities.Shared;

namespace Infrastructure.Services.Validators
{
    public class ClienteValidator : AbstractValidator<Cliente>
    {
        public ClienteValidator()
        {
            RuleFor(c => c.Codigo).NotEmpty().WithMessage(Mensajes.MESSAGE_EMPTY).When(c => c.Id_Cliente == 0);
            RuleFor(c => c.Nombres).NotEmpty().WithMessage(Mensajes.MESSAGE_EMPTY).Matches("^[a-zA-ZáéíóúÁÉÍÓÚñÑ ]+$").WithMessage(Mensajes.MESSAGE_ONLY_LETTERS);
            RuleFor(c => c.Apellidos).NotEmpty().WithMessage(Mensajes.MESSAGE_EMPTY).Matches("^[a-zA-ZáéíóúÁÉÍÓÚñÑ ]+$").WithMessage(Mensajes.MESSAGE_ONLY_LETTERS);
            RuleFor(c => c.Correo_Electronico).NotEmpty().WithMessage(Mensajes.MESSAGE_EMPTY).EmailAddress().WithMessage(Mensajes.MESSAGE_EMAIL);
            RuleFor(c => c.Cedula).NotEmpty().WithMessage(Mensajes.MESSAGE_EMPTY).Matches(@"^\d{10}$").WithMessage(Mensajes.MESSAGE_CEDULA);
            RuleFor(c => c.Telefono).NotEmpty().WithMessage(Mensajes.MESSAGE_EMPTY).Matches(@"^\d{10}$").WithMessage(Mensajes.MESSAGE_PHONE);
            RuleFor(c => c.Nombres).MinimumLength(3).WithMessage(Mensajes.MESSAGE_MIN_LENGTH);
            RuleFor(c => c.Apellidos).MinimumLength(3).WithMessage(Mensajes.MESSAGE_MIN_LENGTH);
        }
    }
}

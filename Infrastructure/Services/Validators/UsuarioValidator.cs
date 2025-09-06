using DataBaseFirst.Models;
using FluentValidation;
using Utilities.Shared;

namespace Infrastructure.Services.Validators
{
    public class UsuarioValidator : AbstractValidator<Usuario>
    {
        public UsuarioValidator() 
        {
            RuleFor(c => c.Codigo).NotEmpty().WithMessage(Mensajes.MESSAGE_EMPTY).When(u => u.Id_Usuario == 0);
            RuleFor(u => u.Nombre_Completo).NotEmpty().WithMessage(Mensajes.MESSAGE_EMPTY).Matches("^[a-zA-ZáéíóúÁÉÍÓÚñÑ ]+$").WithMessage(Mensajes.MESSAGE_ONLY_LETTERS).MinimumLength(3).WithMessage(Mensajes.MESSAGE_MIN_LENGTH);
            RuleFor(u => u.Clave).NotEmpty().WithMessage(Mensajes.MESSAGE_EMPTY).MinimumLength(10).WithMessage(Mensajes.MESSAGE_MIN_LENGTH).Matches(@"[A-Za-z]").WithMessage("La contraseña debe contener al menos una letra.").Matches(@"\d").WithMessage("La contraseña debe contener al menos un número.").Matches(@"[\W]").WithMessage("La contraseña debe contener al menos un carácter especial.").When(u => u.Id_Usuario == 0 || !string.IsNullOrWhiteSpace(u.Clave));
            RuleFor(u => u.Correo_Electronico).NotEmpty().WithMessage(Mensajes.MESSAGE_EMPTY).EmailAddress().WithMessage(Mensajes.MESSAGE_EMAIL);
        }
    }
}

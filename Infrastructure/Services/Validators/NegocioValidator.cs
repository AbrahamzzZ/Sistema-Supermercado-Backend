using Domain.Models;
using FluentValidation;
using Utilities.Shared;

namespace Infrastructure.Services.Validators
{
    public class NegocioValidator : AbstractValidator<Negocio>
    {
        public NegocioValidator()
        {
            RuleFor(n => n.Nombre).NotEmpty().WithMessage(Mensajes.MESSAGE_EMPTY).MaximumLength(50).WithMessage(Mensajes.MESSAGE_MIN_LENGTH);
            RuleFor(n => n.Direccion).NotEmpty().WithMessage(Mensajes.MESSAGE_EMPTY).MaximumLength(200).WithMessage(Mensajes.MESSAGE_MIN_LENGTH);
            RuleFor(n => n.Ruc).NotEmpty().WithMessage(Mensajes.MESSAGE_EMPTY).Matches(@"^\d{13}$").WithMessage("El RUC deben contener exactamente 13 dígitos numéricos");
            RuleFor(n => n.Telefono).NotEmpty().WithMessage(Mensajes.MESSAGE_EMPTY).Matches(@"^\d{10}$").WithMessage(Mensajes.MESSAGE_PHONE);
            RuleFor(n => n.Correo_Electronico).NotEmpty().WithMessage(Mensajes.MESSAGE_EMPTY).EmailAddress().WithMessage(Mensajes.MESSAGE_EMAIL);
        }
    }
}

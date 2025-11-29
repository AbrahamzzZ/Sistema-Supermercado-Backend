using Domain.Models;
using Domain.Models.Dto;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Infrastructure.Helpers
{
    public class Token
    {
        private readonly IConfiguration _configuration;

        public Token(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerarToken(UsuarioRol usuario, List<Menu> permisos)
        {
            var jwtSettings = _configuration.GetSection("Jwt");
            var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]);

            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, usuario.Id_Usuario.ToString()),
                new(ClaimTypes.Name, usuario.Nombre_Completo ?? ""),
                new(ClaimTypes.Email, usuario.Correo_Electronico ?? ""),
                new(ClaimTypes.Role, usuario.Nombre_Rol ?? "Usuario"),
                new("codigo", usuario.Codigo ?? "")
            };

            foreach (var menu in permisos)
            {
                claims.Add(new Claim("permiso", menu.UrlMenu ?? ""));
            }

            var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(jwtSettings.GetValue<int>("DurationInMinutes")),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

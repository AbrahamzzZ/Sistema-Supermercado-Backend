using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models.Dto
{
    public class UsuarioRol
    {
        public int Id_Usuario { get; set; }
        public string? Codigo { get; set; }
        public string? Nombre_Completo { get; set; }
        public string? Correo_Electronico { get; set; }
        public int? Id_Rol { get; set; }
        public string? Nombre_Rol { get; set; }
        public bool? Estado { get; set; }
        [NotMapped]
        public int TotalCount { get; set; }
        public DateTime? Fecha_Creacion { get; set; }
    }
}

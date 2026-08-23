namespace Domain.Models.Dto.Response.Cliente
{
    public class ClienteResponse
    {
        public int Id_Cliente { get; set; }
        public string? Codigo { get; set; }
        public string? Nombres { get; set; }
        public string? Apellidos { get; set; }
        public string? Cedula { get; set; }
        public string? Telefono { get; set; }
        public string? Correo_Electronico { get; set; }
        public DateTime? Fecha_Creacion { get; set; }
    }
}

namespace Domain.Models.Dto.Response.Transportista
{
    public class TransportistaResponse
    {
        public int Id_Transportista { get; set; }
        public string? Codigo { get; set; }
        public string? Nombres { get; set; }
        public string? Apellidos { get; set; }
        public string? Cedula { get; set; }
        public string? Telefono { get; set; }
        public string? Correo_Electronico { get; set; }
        public byte[]? Foto { get; set; }
        public bool? Estado { get; set; }
        public DateTime? Fecha_Creacion { get; set; }
    }
}

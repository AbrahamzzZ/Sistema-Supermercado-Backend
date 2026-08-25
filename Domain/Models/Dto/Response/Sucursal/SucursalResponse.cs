namespace Domain.Models.Dto.Response.Sucursal
{
    public class SucursalResponse
    {
        public int Id_Sucursal { get; set; }

        public string? Codigo { get; set; }

        public int? Id_Negocio { get; set; }

        public string? Nombre_Sucursal { get; set; }

        public string? Direccion_Sucursal { get; set; }

        public double? Latitud { get; set; }

        public double? Longitud { get; set; }

        public string? Ciudad_Sucursal { get; set; }

        public bool? Estado { get; set; }
    }
}

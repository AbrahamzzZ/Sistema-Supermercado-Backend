namespace Domain.Models.Dto.Compra
{
    public class CompraRespuesta
    {
        public int Id_Compra { get; set; }
        public int Id_Usuario { get; set; }
        public string? Codigo_Usuario { get; set; } 
        public string? Nombre_Completo { get; set; }
        public int Id_Sucursal { get; set; }
        public string? Codigo { get; set; }
        public string? Nombre_Sucursal { get; set; }
        public string? Direccion_Sucursal { get; set; }
        public string? Tipo_Documento { get; set; } 
        public string? Numero_Documento { get; set; }
        public int Id_Proveedor { get; set; }
        public string? Codigo_Proveedor { get; set; } 
        public string? Nombres_Proveedor { get; set; } 
        public string? Apellidos_Proveedor { get; set; }
        public string? Cedula_Proveedor { get; set; }
        public int Id_Transportista { get; set; }
        public string? Codigo_Transportista { get; set; } 
        public string? Nombres_Transportista { get; set; }
        public string? Apellidos_Transportista { get; set; } 
        public string? Cedula_Transportista { get; set; } 
        public decimal Monto_Total { get; set; }
        public string? Fecha_Compra { get; set; } 
    }
}

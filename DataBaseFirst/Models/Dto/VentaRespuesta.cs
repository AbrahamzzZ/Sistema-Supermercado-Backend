namespace DataBaseFirst.Models.Dto
{
    public class VentaRespuesta
    {
        public int Id_Venta { get; set; }
        public int Id_Usuario { get; set; }
        public string? Codigo_Usuario { get; set; }
        public string? Nombre_Completo { get; set; }
        public int Id_Sucursal { get; set; }
        public string? Codigo { get; set; }
        public string? Nombre_Sucursal { get; set; }
        public string? Direccion_Sucursal { get; set; }
        public string? Tipo_Documento { get; set; }
        public string? Numero_Documento { get; set; }
        public int Id_Cliente { get; set; }
        public string? Codigo_Cliente { get; set; }
        public string? Nombres_Cliente { get; set; }
        public string? Apellidos_Cliente { get; set; }
        public string? Cedula_Cliente { get; set; }
        public decimal Monto_Pago { get; set; }
        public decimal Monto_Cambio { get; set; }
        public decimal Monto_Total { get; set; }
        public decimal Descuento { get; set; }
        public string? Fecha_Venta { get; set; }
    }
}

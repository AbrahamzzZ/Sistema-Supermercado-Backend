using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models;

public partial class Log
{
    public int Id_Log { get; set; }

    public string? Codigo_Error { get; set; }

    public string? Mensaje_Error { get; set; }

    public string? Detalle_Error { get; set; }

    public int? Id_Usuario { get; set; }

    public DateTime? Fecha { get; set; }

    public string? Endpoint { get; set; }

    public string? Metodo { get; set; }

    public string? Nivel { get; set; }

    [NotMapped]
    public int TotalCount { get; set; }
}

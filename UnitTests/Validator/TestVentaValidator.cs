using Domain.Models.Dto.Response.Venta;
using Infrastructure.Services.Validators;

namespace UnitTests.Validator;

[TestClass]
public class TestVentaValidator
{
    private readonly VentaValidator _validator = new();

    private static Ventas VentaValida() => new()
    {
        Id_Sucursal = 1,
        Id_Cliente = 1,
        Tipo_Documento = "Boleta",
        Monto_Pago = 20,
        Detalles = new List<DetalleVentas> { new DetalleVentas { Id_Producto = 1, Cantidad = 2 } }
    };

    [TestMethod]
    public void VentaValida_NoDebeTenerErrores()
    {
        var result = _validator.Validate(VentaValida());

        Assert.IsTrue(result.IsValid);
    }

    [DataTestMethod]
    [DataRow(0)]
    [DataRow(-5)]
    public void CantidadMenorOIgualACero_DebeSerRechazada(int cantidad)
    {
        var venta = VentaValida();
        venta.Detalles![0].Cantidad = cantidad;

        var result = _validator.Validate(venta);

        Assert.IsFalse(result.IsValid);
        Assert.IsTrue(result.Errors.Any(e => e.ErrorMessage == "La cantidad de cada producto debe ser mayor que 0."));
    }

    [TestMethod]
    public void SinSucursalNiCliente_DebeSerRechazada()
    {
        var venta = VentaValida();
        venta.Id_Sucursal = 0;
        venta.Id_Cliente = 0;

        var result = _validator.Validate(venta);

        Assert.IsTrue(result.Errors.Any(e => e.ErrorMessage == "Debe seleccionar una sucursal."));
        Assert.IsTrue(result.Errors.Any(e => e.ErrorMessage == "Debe seleccionar un cliente."));
    }
}

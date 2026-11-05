using Domain.Models.Dto.Response.Compra;
using Infrastructure.Services.Validators;

namespace UnitTests.Validator;

[TestClass]
public class TestCompraValidator
{
    private readonly CompraValidator _validator = new();

    private static Compras CompraValida() => new()
    {
        Id_Sucursal = 1,
        Id_Proveedor = 1,
        Id_Transportista = 1,
        Tipo_Documento = "Factura",
        Numero_Documento = "00001",
        Monto_Total = 20,
        Detalles = new List<DetalleCompras> { new DetalleCompras { Id_Producto = 1, Cantidad = 4, Precio_Compra = 5, Precio_Venta = 7, SubTotal = 20 } }
    };

    [TestMethod]
    public void CompraValida_NoDebeTenerErrores()
    {
        Assert.IsTrue(_validator.Validate(CompraValida()).IsValid);
    }

    [TestMethod]
    public void CantidadNegativa_DebeSerRechazada()
    {
        var compra = CompraValida();
        compra.Detalles![0].Cantidad = -4;
        compra.Detalles[0].SubTotal = -20;

        var result = _validator.Validate(compra);

        Assert.IsTrue(result.Errors.Any(e => e.ErrorMessage == "La cantidad de cada producto debe ser mayor que 0."));
    }

    [TestMethod]
    public void PrecioDeVentaEnCero_DebeSerRechazado()
    {
        var compra = CompraValida();
        compra.Detalles![0].Precio_Venta = 0;

        var result = _validator.Validate(compra);

        Assert.IsTrue(result.Errors.Any(e => e.ErrorMessage == "El precio de venta debe ser mayor que 0."));
    }

    [TestMethod]
    public void SubtotalQueNoCoincide_DebeSerRechazado()
    {
        var compra = CompraValida();
        compra.Detalles![0].SubTotal = 1;

        var result = _validator.Validate(compra);

        Assert.IsTrue(result.Errors.Any(e => e.ErrorMessage == "El subtotal de cada línea debe ser precio de compra × cantidad."));
    }
}

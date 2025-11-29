using Domain.Models.Dto.Venta;
using Infrastructure.Repository.InterfacesRepository;
using Infrastructure.Services;
using Moq;
using Utilities.Shared;

namespace UnitTests.Services;

[TestClass]
public class TestVentaService
{
    private Mock<IVentaRepository> _mockRepository;
    private VentaService _service;

    [TestInitialize]
    public void Setup()
    {
        _mockRepository = new Mock<IVentaRepository>();
        //_service = new VentaService(_mockRepository.Object);
    }

    [TestMethod]
    public async Task ObtenerNumeroDocumentoAsync_DebeRetornarNumero()
    { 
        var numeroEsperado = "DOC-001";
        _mockRepository.Setup(r => r.ObtenerNumeroDocumentoAsync()).ReturnsAsync(numeroEsperado);

        var result = await _service.ObtenerNumeroDocumentoAsync();

        Assert.IsTrue(result.IsSuccess);
        Assert.AreEqual(numeroEsperado, result.Data);
        Assert.AreEqual("Número de documento generado correctamente.", result.Message);
    }

    [TestMethod]
    public async Task ObtenerVentaAsync_SiNoExisteDebeRetornarError()
    {
        _mockRepository.Setup(r => r.ObtenerVentaAsync("DOC-002")).ReturnsAsync((VentaRespuesta)null);

        var result = await _service.ObtenerVentaAsync("DOC-002");

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual(Mensajes.MESSAGE_QUERY_EMPTY, result.Message);
    }

    [TestMethod]
    public async Task ObtenerVentaAsync_SiExisteDebeRetornarVenta()
    {
        var venta = new VentaRespuesta { Id_Venta = 1, Numero_Documento = "DOC-003" };
        _mockRepository.Setup(r => r.ObtenerVentaAsync("DOC-003")).ReturnsAsync(venta);

        var result = await _service.ObtenerVentaAsync("DOC-003");

        Assert.IsTrue(result.IsSuccess);
        Assert.AreEqual(venta, result.Data);
    }

    [TestMethod]
    public async Task RegistrarVentaAsync_SiDtoEsNuloDebeRetornarError()
    {
        var result = await _service.RegistrarVentaAsync(null);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual(Mensajes.MESSAGE_NULL, result.Message);
    }

    [TestMethod]
    public async Task RegistrarVentaAsync_SiCamposInvalidosDebeRetornarError()
    {
        var venta = new Ventas { Id_Usuario = 0,  Id_Cliente = 0, Tipo_Documento = "", Numero_Documento = "",  Monto_Total = 0, Monto_Pago = 0, Detalles = null };

        var result = await _service.RegistrarVentaAsync(venta);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual(Mensajes.MESSAGE_EMPTY, result.Message);
    }

    [TestMethod]
    public async Task RegistrarVentaAsync_SiTipoDocumentoInvalidoDebeRetornarError()
    {
        var venta = new Ventas { Id_Usuario = 1, Id_Cliente = 1, Tipo_Documento = "DOC123", Numero_Documento = "001", Monto_Total = 100, Monto_Pago = 100, Monto_Cambio = 0, Detalles = new List<DetalleVentas> { new DetalleVentas() } };

        var result = await _service.RegistrarVentaAsync(venta);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual("El tipo de documento solo pueden contener letras.", result.Message);
    }

    [TestMethod]
    public async Task RegistrarVentaAsync_SiVentaValidaDebeRegistrar()
    {
        var venta = new Ventas { Id_Usuario = 1, Id_Cliente = 1, Tipo_Documento = "Factura", Numero_Documento = "001", Monto_Total = 100, Monto_Pago = 150, Monto_Cambio = 50, Detalles = new List<DetalleVentas> { new DetalleVentas() } };

        _mockRepository.Setup(r => r.RegistrarVentaAsync(venta)).ReturnsAsync(true);

        var result = await _service.RegistrarVentaAsync(venta);

        Assert.IsTrue(result.IsSuccess);
        Assert.AreEqual(Mensajes.MESSAGE_REGISTER, result.Message);
    }
}

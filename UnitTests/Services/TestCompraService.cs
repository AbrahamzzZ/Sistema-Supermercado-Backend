using DataBaseFirst.Models;
using DataBaseFirst.Models.Dto;
using DataBaseFirst.Services;
using Infrastructure.Repository.InterfacesRepository;
using Moq;
using Utilities.Shared;

namespace UnitTests.Services;

[TestClass]
public class TestCompraService
{
    private Mock<ICompraRepository> _mockRepository;
    private CompraService _service;

    [TestInitialize]
    public void Setup()
    {
        _mockRepository = new Mock<ICompraRepository>();
        //_service = new CompraService(_mockRepository.Object);
    }

    [TestMethod]
    public async Task ObtenerNumeroDocumentoAsync_DebeRetornarNumero()
    {
        var numeroEsperado = "COMP-001";
        _mockRepository.Setup(r => r.ObtenerNumeroDocumentoAsync()).ReturnsAsync(numeroEsperado);

        var result = await _service.ObtenerNumeroDocumentoAsync();

        Assert.IsTrue(result.IsSuccess);
        Assert.AreEqual(numeroEsperado, result.Data);
        Assert.AreEqual("Número de documento generado correctamente.", result.Message);
    }

    [TestMethod]
    public async Task ObtenerCompraAsync_SiNoExisteDebeRetornarError()
    {
        _mockRepository.Setup(r => r.ObtenerCompraAsync("COMP-002")).ReturnsAsync((CompraRespuesta)null);

        var result = await _service.ObtenerCompraAsync("COMP-002");

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual(Mensajes.MESSAGE_QUERY_EMPTY, result.Message);
    }

    [TestMethod]
    public async Task ObtenerCompraAsync_SiExisteDebeRetornarCompra()
    {
        var compra = new CompraRespuesta { Id_Compra = 1, Numero_Documento = "COMP-003" };
        _mockRepository.Setup(r => r.ObtenerCompraAsync("COMP-003")).ReturnsAsync(compra);

        var result = await _service.ObtenerCompraAsync("COMP-003");

        Assert.IsTrue(result.IsSuccess);
        Assert.AreEqual(compra, result.Data);
    }

    [TestMethod]
    public async Task RegistrarCompraAsync_SiDtoEsNuloDebeRetornarError()
    {
        var result = await _service.RegistrarCompraAsync(null);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual(Mensajes.MESSAGE_NULL, result.Message);
    }

    [TestMethod]
    public async Task RegistrarCompraAsync_SiCamposInvalidosDebeRetornarError()
    {
        var compra = new Compras { Id_Usuario = 0, Id_Proveedor = 0, Id_Transportista = 0, Tipo_Documento = "", Numero_Documento = "", Monto_Total = 0, Detalles = null };

        var result = await _service.RegistrarCompraAsync(compra);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual(Mensajes.MESSAGE_EMPTY, result.Message);
    }

    [TestMethod]
    public async Task RegistrarCompraAsync_SiTipoDocumentoInvalidoDebeRetornarError()
    {
        var compra = new Compras { Id_Usuario = 1, Id_Proveedor = 1, Id_Transportista = 1, Tipo_Documento = "COMP123", Numero_Documento = "001", Monto_Total = 100, Detalles = new List<DetalleCompras> { new DetalleCompras() } };

        var result = await _service.RegistrarCompraAsync(compra);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual("El tipo de documento solo pueden contener letras.", result.Message);
    }

    [TestMethod]
    public async Task RegistrarCompraAsync_SiCompraValidaDebeRegistrar()
    {
        var compra = new Compras { Id_Usuario = 1, Id_Proveedor = 1, Id_Transportista = 1, Tipo_Documento = "Factura", Numero_Documento = "001", Monto_Total = 500, Detalles = new List<DetalleCompras> { new DetalleCompras() } };

        _mockRepository.Setup(r => r.RegistrarCompraAsync(compra)).ReturnsAsync(true);

        var result = await _service.RegistrarCompraAsync(compra);

        Assert.IsTrue(result.IsSuccess);
        Assert.AreEqual(Mensajes.MESSAGE_REGISTER, result.Message);
    }
}

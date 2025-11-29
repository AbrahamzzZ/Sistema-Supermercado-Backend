using Domain.Models;
using Domain.Models.Dto.Negocio;
using Infrastructure.Repository.InterfacesRepository;
using Infrastructure.Services;
using Moq;
using Utilities.Shared;

namespace UnitTests.Services;

[TestClass]
public class TestNegocioService
{
    private Mock<INegocioRepository> _mockRepository;
    private NegocioService _service;

    [TestInitialize]
    public void Setup()
    {
        _mockRepository = new Mock<INegocioRepository>();
        //_service = new NegocioService(_mockRepository.Object);
    }

    [TestMethod]
    public async Task ObtenerNegocioAsync_SiNoExisteDebeRetornarError()
    {
        _mockRepository.Setup(r => r.ObtenerNegocioAsync(1)).ReturnsAsync((Negocio)null);

        var result = await _service.ObtenerNegocioAsync(1);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual(Mensajes.MESSAGE_QUERY_NOT_FOUND, result.Message);
    }

    [TestMethod]
    public async Task ObtenerNegocioAsync_SiExisteDebeRetornarDatos()
    {
        var negocio = new Negocio { Id_Negocio = 1, Nombre = "MiNegocio" };
        _mockRepository.Setup(r => r.ObtenerNegocioAsync(1)).ReturnsAsync(negocio);

        var result = await _service.ObtenerNegocioAsync(1);

        Assert.IsTrue(result.IsSuccess);
        Assert.AreEqual(negocio, result.Data);
    }

    [TestMethod]
    public async Task EditarNegocioAsync_SiNegocioEsNuloDebeRetornarError()
    {
        var result = await _service.EditarNegocioAsync(null);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual(Mensajes.MESSAGE_NULL, result.Message);
    }

    [TestMethod]
    public async Task EditarNegocioAsync_SiCamposVaciosDebeRetornarError()
    {
        var negocio = new Negocio { Id_Negocio = 1, Nombre = "", Telefono = "", Ruc = "", Direccion = "", Correo_Electronico = "" };

        var result = await _service.EditarNegocioAsync(negocio);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual(Mensajes.MESSAGE_EMPTY, result.Message);
    }

    [TestMethod]
    public async Task EditarNegocioAsync_SiNoExisteEnBDDebeRetornarError()
    {
        var negocio = new Negocio { Id_Negocio = 1, Nombre = "Test", Telefono = "1234567890", Ruc = "1234567890123", Direccion = "Dir", Correo_Electronico = "correo@test.com" };

        _mockRepository.Setup(r => r.ObtenerNegocioAsync(negocio.Id_Negocio)).ReturnsAsync((Negocio)null);

        var result = await _service.EditarNegocioAsync(negocio);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual(Mensajes.MESSAGE_QUERY_NOT_FOUND, result.Message);
    }

    [TestMethod]
    public async Task EditarNegocioAsync_TelefonoInvalidoDebeRetornarError()
    {
        var negocio = new Negocio { Id_Negocio = 1, Nombre = "Test", Telefono = "1234", Ruc = "1234567890123", Direccion = "Dir", Correo_Electronico = "correo@test.com" };

        _mockRepository.Setup(r => r.ObtenerNegocioAsync(negocio.Id_Negocio)).ReturnsAsync(negocio);

        var result = await _service.EditarNegocioAsync(negocio);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual("El teléfono deben contener exactamente 10 dígitos numéricos", result.Message);
    }

    [TestMethod]
    public async Task EditarNegocioAsync_RucInvalidoDebeRetornarError()
    {
        var negocio = new Negocio { Id_Negocio = 1, Nombre = "Test", Telefono = "1234567890", Ruc = "1234", Direccion = "Dir", Correo_Electronico = "correo@test.com" };

        _mockRepository.Setup(r => r.ObtenerNegocioAsync(negocio.Id_Negocio)).ReturnsAsync(negocio);

        var result = await _service.EditarNegocioAsync(negocio);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual("El RUC deben contener exactamente 13 dígitos numéricos", result.Message);
    }

    [TestMethod]
    public async Task EditarNegocioAsync_CorreoInvalidoDebeRetornarError()
    {
        var negocio = new Negocio { Id_Negocio = 1, Nombre = "Test", Telefono = "1234567890", Ruc = "1234567890123", Direccion = "Dir", Correo_Electronico = "correo_invalido" };

        _mockRepository.Setup(r => r.ObtenerNegocioAsync(negocio.Id_Negocio)).ReturnsAsync(negocio);

        var result = await _service.EditarNegocioAsync(negocio);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual("El correo electrónico no tiene un formato válido", result.Message);
    }


    [TestMethod]
    public async Task EditarNegocioAsync_SiValidoDebeActualizar()
    {
        var negocio = new Negocio { Id_Negocio = 1, Nombre = "Test", Telefono = "1234567890", Ruc = "1234567890123", Direccion = "Dir", Correo_Electronico = "correo@test.com" };

        _mockRepository.Setup(r => r.ObtenerNegocioAsync(negocio.Id_Negocio)).ReturnsAsync(negocio);
        _mockRepository.Setup(r => r.EditarNegocioAsync(negocio)).ReturnsAsync(1);

        var result = await _service.EditarNegocioAsync(negocio);

        Assert.IsTrue(result.IsSuccess);
        Assert.AreEqual(Mensajes.MESSAGE_UPDATE, result.Message);
    }

    [TestMethod]
    public async Task ObtenerProductoMasComprado_SiNullDebeRetornarError()
    {
        _mockRepository.Setup(r => r.ObtenerProductoMasComprado()).ReturnsAsync((List<ProductoMasComprado>)null);

        var result = await _service.ObtenerProductoMasComprado();

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual(Mensajes.MESSAGE_QUERY_EMPTY, result.Message);
    }

    [TestMethod]
    public async Task ObtenerProductoMasVendido_SiListaVaciaDebeRetornarError()
    {
        _mockRepository.Setup(r => r.ObtenerProductoMasVendido()).ReturnsAsync(new List<ProductoMasVendido>());

        var result = await _service.ObtenerProductoMasVendido();

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual(Mensajes.MESSAGE_QUERY_EMPTY, result.Message);
    }

    [TestMethod]
    public async Task ObtenerTopClientes_SiListaVaciaDebeRetornarError()
    {
        _mockRepository.Setup(r => r.ObtenerTopClientes()).ReturnsAsync(new List<TopCliente>());

        var result = await _service.ObtenerTopClientes();

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual(Mensajes.MESSAGE_QUERY_EMPTY, result.Message);
    }

    [TestMethod]
    public async Task ObtenerTopProveedores_SiListaVaciaDebeRetornarError()
    {
        _mockRepository.Setup(r => r.ObtenerTopProveedores()).ReturnsAsync(new List<TopProveedor>());

        var result = await _service.ObtenerTopProveedores();

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual(Mensajes.MESSAGE_QUERY_EMPTY, result.Message);
    }

    [TestMethod]
    public async Task ObtenerViajesTransportista_SiListaVaciaDebeRetornarError()
    {
        _mockRepository.Setup(r => r.ObtenerViajesTransportista()).ReturnsAsync(new List<ViajesTransportista>());

        var result = await _service.ObtenerViajesTransportista();

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual(Mensajes.MESSAGE_QUERY_EMPTY, result.Message);
    }

    [TestMethod]
    public async Task ObtenerEmpleadosProductivos_SiListaVaciaDebeRetornarError()
    {
        _mockRepository.Setup(r => r.ObtenerEmpleadosProductivos()).ReturnsAsync(new List<EmpleadoProductivo>());

        var result = await _service.ObtenerEmpleadosProductivos();

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual(Mensajes.MESSAGE_QUERY_EMPTY, result.Message);
    }
}

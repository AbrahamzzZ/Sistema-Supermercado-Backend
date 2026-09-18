using Domain.Models;
using Domain.Models.Dto.Response.Negocio;
using FluentValidation;
using FluentValidation.Results;
using Infrastructure.Repository.InterfacesRepository;
using Infrastructure.Repository.InterfacesBusiness;
using Infrastructure.Repository.InterfacesServices;
using Infrastructure.Services;
using Moq;
using Utilities.IA;
using Utilities.Shared;

namespace UnitTests.Services;

[TestClass]
public class TestNegocioService
{
    private Mock<INegocioRepository> _mockRepository;
    private Mock<IProductoRepository> _mockProductoRepository;
    private Mock<ICategoriaRepository> _mockCategoriaRepository;
    private Mock<IValidator<Negocio>> _mockValidator;
    private Mock<ICurrentUser> _mockCurrentUser;
    private Mock<IAuditoriaService> _mockAuditoria;
    private NegocioService _service;

    [TestInitialize]
    public void Setup()
    {
        _mockRepository = new Mock<INegocioRepository>();
        _mockProductoRepository = new Mock<IProductoRepository>();
        _mockCategoriaRepository = new Mock<ICategoriaRepository>();
        _mockValidator = new Mock<IValidator<Negocio>>();
        _mockCurrentUser = new Mock<ICurrentUser>();
        _mockCurrentUser.Setup(user => user.GetUserId()).Returns(1);
        _mockAuditoria = new Mock<IAuditoriaService>();
        _service = new NegocioService(
            _mockRepository.Object,
            _mockProductoRepository.Object,
            _mockCategoriaRepository.Object,
            _mockCurrentUser.Object,
            _mockValidator.Object,
            new OllamaClient(new HttpClient()),
            _mockAuditoria.Object);
    }

    [TestMethod]
    public async Task ObtenerNegocioAsync_SiNoExisteDebeRetornarNoEncontrado()
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
        _mockValidator.Setup(v => v.ValidateAsync(It.IsAny<Negocio>(), default)).ReturnsAsync(new ValidationResult(new List<ValidationFailure>{ new ValidationFailure("Nombre", Mensajes.MESSAGE_EMPTY) }));
        var result = await _service.EditarNegocioAsync(negocio);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual(Mensajes.MESSAGE_EMPTY, result.Message);
    }

    [TestMethod]
    public async Task EditarNegocioAsync_SiNoExisteEnBaseDeDatosDebeRetornarError()
    {
        var negocio = new Negocio { Id_Negocio = 1, Nombre = "Test", Telefono = "1234567890", Ruc = "1234567890123", Direccion = "Dir", Correo_Electronico = "correo@test.com" };
        _mockValidator.Setup(v => v.ValidateAsync(It.IsAny<Negocio>(), default)).ReturnsAsync(new ValidationResult());
        _mockRepository.Setup(r => r.ObtenerNegocioAsync(negocio.Id_Negocio)).ReturnsAsync((Negocio)null);
        var result = await _service.EditarNegocioAsync(negocio);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual(Mensajes.MESSAGE_QUERY_NOT_FOUND, result.Message);
    }

    [TestMethod]
    public async Task EditarNegocioAsync_TelefonoInvalidoDebeRetornarError()
    {
        var negocio = new Negocio { Id_Negocio = 1, Nombre = "Test", Telefono = "1234", Ruc = "1234567890123", Direccion = "Dir", Correo_Electronico = "correo@test.com" };
        _mockValidator.Setup(v => v.ValidateAsync(It.IsAny<Negocio>(), default)).ReturnsAsync(new ValidationResult(new List<ValidationFailure> { new ValidationFailure("Tel�fono", "El tel�fono deben contener exactamente 10 d�gitos num�ricos") }));
        _mockRepository.Setup(r => r.ObtenerNegocioAsync(negocio.Id_Negocio)).ReturnsAsync(negocio);
        var result = await _service.EditarNegocioAsync(negocio);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual("El tel�fono deben contener exactamente 10 d�gitos num�ricos", result.Message);
    }

    [TestMethod]
    public async Task EditarNegocioAsync_RucInvalidoDebeRetornarError()
    {
        var negocio = new Negocio { Id_Negocio = 1, Nombre = "Test", Telefono = "1234567890", Ruc = "1234", Direccion = "Dir", Correo_Electronico = "correo@test.com" };
        _mockValidator.Setup(v => v.ValidateAsync(It.IsAny<Negocio>(), default)).ReturnsAsync(new ValidationResult(new List<ValidationFailure> { new ValidationFailure("Ruc", "El RUC deben contener exactamente 13 d�gitos num�ricos") }));
        _mockRepository.Setup(r => r.ObtenerNegocioAsync(negocio.Id_Negocio)).ReturnsAsync(negocio);
        var result = await _service.EditarNegocioAsync(negocio);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual("El RUC deben contener exactamente 13 d�gitos num�ricos", result.Message);
    }

    [TestMethod]
    public async Task EditarNegocioAsync_CorreoInvalidoDebeRetornarError()
    {
        var negocio = new Negocio { Id_Negocio = 1, Nombre = "Test", Telefono = "1234567890", Ruc = "1234567890123", Direccion = "Dir", Correo_Electronico = "correo_invalido" };
        _mockValidator.Setup(v => v.ValidateAsync(It.IsAny<Negocio>(), default)).ReturnsAsync(new ValidationResult(new List<ValidationFailure>{ new ValidationFailure("Correo_Electronico", "El correo electr�nico no tiene un formato v�lido") }));
        _mockRepository.Setup(r => r.ObtenerNegocioAsync(negocio.Id_Negocio)).ReturnsAsync(negocio);
        var result = await _service.EditarNegocioAsync(negocio);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual("El correo electr�nico no tiene un formato v�lido", result.Message);
    }


    [TestMethod]
    public async Task EditarNegocioAsync_SiValidoDebeActualizar()
    {
        var negocio = new Negocio { Id_Negocio = 1, Nombre = "Test", Telefono = "1234567890", Ruc = "1234567890123", Direccion = "Dir", Correo_Electronico = "correo@test.com" };
        _mockValidator.Setup(v => v.ValidateAsync(It.IsAny<Negocio>(), default)).ReturnsAsync(new ValidationResult());
        _mockRepository.Setup(r => r.ObtenerNegocioAsync(negocio.Id_Negocio)).ReturnsAsync(negocio);
        _mockRepository.Setup(r => r.EditarNegocioAsync(negocio, 1)).ReturnsAsync(1);
        var result = await _service.EditarNegocioAsync(negocio);

        Assert.IsTrue(result.IsSuccess);
        Assert.AreEqual(Mensajes.MESSAGE_UPDATE, result.Message);
    }

    [TestMethod]
    public async Task ObtenerProductoMasComprado_SiNoHayDatosDebeRetornarError()
    {
        _mockRepository.Setup(r => r.ObtenerProductoMasComprado()).ReturnsAsync((List<ProductoMasCompradoResponse>)null);

        var result = await _service.ObtenerProductoMasComprado();

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual(Mensajes.MESSAGE_QUERY_EMPTY, result.Message);
    }

    [TestMethod]
    public async Task ObtenerProductoMasVendido_SiListaVaciaDebeRetornarError()
    {
        _mockRepository.Setup(r => r.ObtenerProductoMasVendido()).ReturnsAsync(new List<ProductoMasVendidoResponse>());

        var result = await _service.ObtenerProductoMasVendido();

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual(Mensajes.MESSAGE_QUERY_EMPTY, result.Message);
    }

    [TestMethod]
    public async Task ObtenerTopClientes_SiListaVaciaDebeRetornarError()
    {
        _mockRepository.Setup(r => r.ObtenerTopClientes()).ReturnsAsync(new List<TopClienteResponse>());

        var result = await _service.ObtenerTopClientes();

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual(Mensajes.MESSAGE_QUERY_EMPTY, result.Message);
    }

    [TestMethod]
    public async Task ObtenerTopProveedores_SiListaVaciaDebeRetornarError()
    {
        _mockRepository.Setup(r => r.ObtenerTopProveedores()).ReturnsAsync(new List<TopProveedorResponse>());

        var result = await _service.ObtenerTopProveedores();

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual(Mensajes.MESSAGE_QUERY_EMPTY, result.Message);
    }

    [TestMethod]
    public async Task ObtenerViajesTransportista_SiListaVaciaDebeRetornarError()
    {
        _mockRepository.Setup(r => r.ObtenerViajesTransportista()).ReturnsAsync(new List<ViajesTransportistaResponse>());

        var result = await _service.ObtenerViajesTransportista();

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual(Mensajes.MESSAGE_QUERY_EMPTY, result.Message);
    }

    [TestMethod]
    public async Task ObtenerEmpleadosProductivos_SiListaVaciaDebeRetornarError()
    {
        _mockRepository.Setup(r => r.ObtenerEmpleadosProductivos()).ReturnsAsync(new List<EmpleadoProductivoResponse>());

        var result = await _service.ObtenerEmpleadosProductivos();

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual(Mensajes.MESSAGE_QUERY_EMPTY, result.Message);
    }

    [TestMethod]
    public async Task ObtenerNegocio_DebePropagarExcepcion_YRegistrarError()
    {
        var excepcion = new InvalidOperationException("Error de repositorio");
        _mockRepository.Setup(r => r.ObtenerNegocioAsync(1)).ThrowsAsync(excepcion);

        await Assert.ThrowsExceptionAsync<InvalidOperationException>(() => _service.ObtenerNegocioAsync(1));

        _mockAuditoria.Verify(a => a.RegistrarErrorAsync("Obtener Negocio", excepcion), Times.Once);
    }
}

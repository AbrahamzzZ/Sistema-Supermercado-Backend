using Domain.Models;
using Domain.Models.Dto.Response.Transportista;
using FluentValidation;
using FluentValidation.Results;
using Infrastructure.Repository.InterfacesRepository;
using Infrastructure.Repository.InterfacesBusiness;
using Infrastructure.Repository.InterfacesServices;
using Infrastructure.Services;
using Moq;
using Utilities.Shared;

namespace UnitTests.Services;

[TestClass]
public class TestTransportistaService
{
    private Mock<ITransportistaRepository> _mockRepository;
    private Mock<IValidator<Transportistum>> _mockValidator;
    private Mock<ICurrentUser> _mockCurrentUser;
    private Mock<IAuditoriaService> _mockAuditoria;
    private TransportistaService _service;

    [TestInitialize]
    public void Setup()
    {
        _mockRepository = new Mock<ITransportistaRepository>();
        _mockValidator = new Mock<IValidator<Transportistum>>();
        _mockCurrentUser = new Mock<ICurrentUser>();
        _mockCurrentUser.Setup(user => user.GetUserId()).Returns(1);
        _mockAuditoria = new Mock<IAuditoriaService>();
        _service = new TransportistaService(
            _mockRepository.Object,
            _mockValidator.Object,
            _mockCurrentUser.Object,
            _mockAuditoria.Object);
    }

    [TestMethod]
    public async Task RegistrarTransportista_DeberiaFallar_SiTransportistaEsNulo()
    {
        var result = await _service.RegistrarTransportistaAsync(null);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual(Mensajes.MESSAGE_NULL, result.Message);
    }

    [TestMethod]
    public async Task RegistrarTransportista_DeberiaFallar_SiCamposObligatoriosVacios()
    {
        var transportista = new Transportistum { Codigo = "", Nombres = "", Apellidos = "", Cedula = "", Telefono = "", Correo_Electronico = "" };
        _mockValidator.Setup(v => v.ValidateAsync(It.IsAny<Transportistum>(), default)).ReturnsAsync(new ValidationResult(new List<ValidationFailure> { new ValidationFailure("Codigo", Mensajes.MESSAGE_EMPTY) }));
        var result = await _service.RegistrarTransportistaAsync(transportista);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual(Mensajes.MESSAGE_EMPTY, result.Message);
    }

    [TestMethod]
    public async Task RegistrarTransportista_DeberiaFallar_SiNombresInvalidos()
    {
        var transportista = new Transportistum { Codigo = "CLI01", Nombres = "Juan123", Apellidos = "Perez", Cedula = "1234567890", Telefono = "0987654321", Correo_Electronico = "test@mail.com" };
        _mockValidator.Setup(v => v.ValidateAsync(It.IsAny<Transportistum>(), default)).ReturnsAsync(new ValidationResult(new List<ValidationFailure> { new ValidationFailure("Nombres Apellidos", "Los nombres y apellidos solo puede contener letras y espacios") }));
        var result = await _service.RegistrarTransportistaAsync(transportista);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual("Los nombres y apellidos solo puede contener letras y espacios", result.Message);
    }

    [TestMethod]
    public async Task RegistrarTransportista_DeberiaFallar_SiCedulaOTelefonoInvalidos()
    {
        var transportista = new Transportistum { Codigo = "CLI01", Nombres = "Juan", Apellidos = "Perez", Cedula = "123", Telefono = "098", Correo_Electronico = "test@mail.com" };
        _mockValidator.Setup(v => v.ValidateAsync(It.IsAny<Transportistum>(), default)).ReturnsAsync(new ValidationResult(new List<ValidationFailure> { new ValidationFailure("Cedula", "La c�dula y el tel�fono deben contener exactamente 10 d�gitos num�ricos") }));
        var result = await _service.RegistrarTransportistaAsync(transportista);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual("La c�dula y el tel�fono deben contener exactamente 10 d�gitos num�ricos", result.Message);
    }

    [TestMethod]
    public async Task RegistrarTransportista_DeberiaFallar_SiCorreoInvalido()
    {
        var transportista = new Transportistum { Codigo = "CLI01", Nombres = "Juan", Apellidos = "Perez", Cedula = "1234567890", Telefono = "0987654321", Correo_Electronico = "correo_invalido" };
        _mockValidator.Setup(v => v.ValidateAsync(It.IsAny<Transportistum>(), default)).ReturnsAsync(new ValidationResult(new List<ValidationFailure> { new ValidationFailure("Correo Electronico", "El correo electr�nico no tiene un formato v�lido") }));
        var result = await _service.RegistrarTransportistaAsync(transportista);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual("El correo electr�nico no tiene un formato v�lido", result.Message);
    }

    [TestMethod]
    public async Task RegistrarTransportista_DeberiaFallar_SiCodigoDuplicado()
    {
        var transportista = new Transportistum { Codigo = "CLI01", Nombres = "Juan", Apellidos = "Perez", Cedula = "1234567890", Telefono = "0987654321", Correo_Electronico = "test@mail.com" };
        _mockValidator.Setup(v => v.ValidateAsync(It.IsAny<Transportistum>(), default)).ReturnsAsync(new ValidationResult(new List<ValidationFailure> { new ValidationFailure("Codigo", Mensajes.MESSAGE_CODE_EXITS) }));
        _mockRepository.Setup(r => r.ListarTransportistasAsync()).ReturnsAsync(new List<TransportistaResponse> { new TransportistaResponse { Codigo = "CLI01" } });
        var result = await _service.RegistrarTransportistaAsync(transportista);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual(Mensajes.MESSAGE_CODE_EXITS, result.Message);
    }

    [TestMethod]
    public async Task EditarTransportista_DeberiaFallar_SiTransportistaEsNulo()
    {
        var result = await _service.EditarTransportistaAsync(null);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual(Mensajes.MESSAGE_NULL, result.Message);
    }

    [TestMethod]
    public async Task EditarTransportista_DeberiaFallar_SiCamposObligatoriosVacios()
    {
        var transportista = new Transportistum { Nombres = "", Apellidos = "", Cedula = "", Telefono = "", Correo_Electronico = "" };
        _mockValidator.Setup(v => v.ValidateAsync(It.IsAny<Transportistum>(), default)).ReturnsAsync(new ValidationResult(new List<ValidationFailure> { new ValidationFailure("Codigo", Mensajes.MESSAGE_EMPTY) }));
        var result = await _service.EditarTransportistaAsync(transportista);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual(Mensajes.MESSAGE_EMPTY, result.Message);
    }

    [TestMethod]
    public async Task EditarTransportista_DeberiaFallar_SiTransportistaNoExiste()
    {
        var transportista = new Transportistum { Id_Transportista = 1, Nombres = "Juan", Apellidos = "Perez", Cedula = "1234567890", Telefono = "0987654321", Correo_Electronico = "test@mail.com" };
        _mockValidator.Setup(v => v.ValidateAsync(It.IsAny<Transportistum>(), default)).ReturnsAsync(new ValidationResult());
        _mockRepository.Setup(r => r.ObtenerTransportistaAsync(1)).ReturnsAsync((TransportistaResponse)null);
        var result = await _service.EditarTransportistaAsync(transportista);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual(Mensajes.MESSAGE_QUERY_NOT_FOUND, result.Message);
    }

    [TestMethod]
    public async Task EditarTransportista_DeberiaFallar_SiNombresInvalidos()
    {
        var transportista = new Transportistum { Id_Transportista = 1, Nombres = "Juan123", Apellidos = "Perez", Cedula = "1234567890", Telefono = "0987654321", Correo_Electronico = "test@mail.com" };
        _mockValidator.Setup(v => v.ValidateAsync(It.IsAny<Transportistum>(), default)).ReturnsAsync(new ValidationResult(new List<ValidationFailure> { new ValidationFailure("Nombres Apellidos", "Los nombres y apellidos solo puede contener letras y espacios") }));
        _mockRepository.Setup(r => r.ObtenerTransportistaAsync(1)).ReturnsAsync(new TransportistaResponse());
        var result = await _service.EditarTransportistaAsync(transportista);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual("Los nombres y apellidos solo puede contener letras y espacios", result.Message);
    }

    [TestMethod]
    public async Task EditarTransportista_DeberiaFallar_SiCedulaOTelefonoInvalidos()
    {
        var cliente = new Transportistum { Id_Transportista = 1, Nombres = "Juan", Apellidos = "Perez", Cedula = "123", Telefono = "098", Correo_Electronico = "test@mail.com" };
        _mockValidator.Setup(v => v.ValidateAsync(It.IsAny<Transportistum>(), default)).ReturnsAsync(new ValidationResult(new List<ValidationFailure> { new ValidationFailure("Cedula", "La c�dula y el tel�fono deben contener exactamente 10 d�gitos num�ricos") }));
        _mockRepository.Setup(r => r.ObtenerTransportistaAsync(1)).ReturnsAsync(new TransportistaResponse());
        var result = await _service.EditarTransportistaAsync(cliente);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual("La c�dula y el tel�fono deben contener exactamente 10 d�gitos num�ricos", result.Message);
    }

    [TestMethod]
    public async Task EditarTransportista_DeberiaFallar_SiCorreoInvalido()
    {
        var cliente = new Transportistum { Id_Transportista = 1, Nombres = "Juan", Apellidos = "Perez", Cedula = "1234567890", Telefono = "0987654321", Correo_Electronico = "correo_invalido" };
        _mockValidator.Setup(v => v.ValidateAsync(It.IsAny<Transportistum>(), default)).ReturnsAsync(new ValidationResult(new List<ValidationFailure> { new ValidationFailure("Correo Electronico", "El correo electr�nico no tiene un formato v�lido") }));
        _mockRepository.Setup(r => r.ObtenerTransportistaAsync(1)).ReturnsAsync(new TransportistaResponse());
        var result = await _service.EditarTransportistaAsync(cliente);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual("El correo electr�nico no tiene un formato v�lido", result.Message);
    }

    [TestMethod]
    public async Task EditarTransportista_DeberiaFallar_SiCedulaOTelefonoDuplicados()
    {
        var cliente = new Transportistum { Id_Transportista = 1, Nombres = "Juan", Apellidos = "Perez", Cedula = "1234567890", Telefono = "0987654321", Correo_Electronico = "test@mail.com" };
        _mockValidator.Setup(v => v.ValidateAsync(It.IsAny<Transportistum>(), default)).ReturnsAsync(new ValidationResult());
        _mockRepository.Setup(r => r.ObtenerTransportistaAsync(1)).ReturnsAsync(new TransportistaResponse());
        _mockRepository.Setup(r => r.ListarTransportistasAsync()).ReturnsAsync(new List<TransportistaResponse>{ new TransportistaResponse { Id_Transportista = 2, Cedula = "1234567890" } });
        var result = await _service.EditarTransportistaAsync(cliente);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual(Mensajes.MESSAGE_CEDULA_EXITS, result.Message);
    }

    [TestMethod]
    public async Task EditarTransportista_DeberiaSerExitoso()
    {
        var transportista = new Transportistum { Id_Transportista = 1, Nombres = "Juan", Apellidos = "Perez", Cedula = "1234567890", Telefono = "0987654321", Correo_Electronico = "test@mail.com" };
        _mockValidator.Setup(v => v.ValidateAsync(It.IsAny<Transportistum>(), default)).ReturnsAsync(new ValidationResult());
        _mockRepository.Setup(r => r.ObtenerTransportistaAsync(1)).ReturnsAsync(new TransportistaResponse());
        _mockRepository.Setup(r => r.ListarTransportistasAsync()).ReturnsAsync(new List<TransportistaResponse>());
        _mockRepository.Setup(r => r.EditarTransportistaAsync(transportista, 1)).ReturnsAsync(1);

        var result = await _service.EditarTransportistaAsync(transportista);

        Assert.IsTrue(result.IsSuccess);
        Assert.AreEqual(Mensajes.MESSAGE_UPDATE, result.Message);
    }

    [TestMethod]
    public async Task EliminarTransportista_DeberiaFallar_SiClienteNoExiste()
    {
        _mockRepository.Setup(r => r.ObtenerTransportistaAsync(99)).ReturnsAsync((TransportistaResponse)null);

        var result = await _service.EliminarTransportistaAsync(99);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual(Mensajes.MESSAGE_QUERY_NOT_FOUND, result.Message);
    }

    [TestMethod]
    public async Task EliminarTransportista_DeberiaSerExitoso()
    {
        _mockRepository.Setup(r => r.ObtenerTransportistaAsync(1)).ReturnsAsync(new TransportistaResponse());
        _mockRepository.Setup(r => r.EliminarTransportistaAsync(1)).ReturnsAsync(1);

        var result = await _service.EliminarTransportistaAsync(1);

        Assert.IsTrue(result.IsSuccess);
        Assert.AreEqual(Mensajes.MESSAGE_DELETE, result.Message);
    }

    [TestMethod]
    public async Task EliminarTransportista_DeberiaFallar_SiOperacionDevuelveCero()
    {
        _mockRepository.Setup(r => r.ObtenerTransportistaAsync(1)).ReturnsAsync(new TransportistaResponse());
        _mockRepository.Setup(r => r.EliminarTransportistaAsync(1)).ReturnsAsync(0);

        var result = await _service.EliminarTransportistaAsync(1);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual(Mensajes.MESSAGE_DELETE_FAILLED, result.Message);
    }

    [TestMethod]
    public async Task ListarTransportistas_DebePropagarExcepcion_YRegistrarError()
    {
        var excepcion = new InvalidOperationException("Error de repositorio");
        _mockRepository.Setup(r => r.ListarTransportistasAsync()).ThrowsAsync(excepcion);

        await Assert.ThrowsExceptionAsync<InvalidOperationException>(() => _service.ListarTransportistasAsync());

        _mockAuditoria.Verify(a => a.RegistrarErrorAsync("Listar Transportistas", excepcion), Times.Once);
    }
}

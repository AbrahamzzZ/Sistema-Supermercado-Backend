using Domain.Models;
using Domain.Models.Dto.Response.Cliente;
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
public class TestClienteService
{
    private Mock<IClienteRepository> _mockRepository;
    private Mock<IValidator<Cliente>> _mockValidator;
    private Mock<ICurrentUser> _mockCurrentUser;
    private Mock<IAuditoriaService> _mockAuditoria;
    private ClienteService _service;

    [TestInitialize]
    public void Setup()
    {
        _mockRepository = new Mock<IClienteRepository>();
        _mockValidator = new Mock<IValidator<Cliente>>();
        _mockCurrentUser = new Mock<ICurrentUser>();
        _mockCurrentUser.Setup(user => user.GetUserId()).Returns(1);
        _mockAuditoria = new Mock<IAuditoriaService>();
        _service = new ClienteService(
            _mockRepository.Object,
            _mockValidator.Object,
            _mockCurrentUser.Object,
            _mockAuditoria.Object);
    }

    [TestMethod]
    public async Task RegistrarCliente_DeberiaFallar_SiClienteEsNulo()
    {
        var result = await _service.RegistrarClienteAsync(null);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual(Mensajes.MESSAGE_NULL, result.Message);
    }

    [TestMethod]
    public async Task RegistrarCliente_DeberiaFallar_SiCamposObligatoriosVacios()
    {
        var cliente = new Cliente { Codigo = "", Nombres = "", Apellidos = "", Cedula = "", Telefono = "", Correo_Electronico = "" };
        _mockValidator.Setup(v => v.ValidateAsync(It.IsAny<Cliente>(), default)).ReturnsAsync(new ValidationResult(new List<ValidationFailure>{ new ValidationFailure("Codigo", Mensajes.MESSAGE_EMPTY) }));
        var result = await _service.RegistrarClienteAsync(cliente);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual(Mensajes.MESSAGE_EMPTY, result.Message);
    }

    [TestMethod]
    public async Task RegistrarCliente_DeberiaFallar_SiNombresInvalidos()
    {
        var cliente = new Cliente { Codigo = "CLI01", Nombres = "Juan123", Apellidos = "Perez", Cedula = "1234567890", Telefono = "0987654321", Correo_Electronico = "test@mail.com" };
        _mockValidator.Setup(v => v.ValidateAsync(It.IsAny<Cliente>(), default)).ReturnsAsync(new ValidationResult(new List<ValidationFailure> { new ValidationFailure("Nombres Apellidos", "Los nombres y apellidos solo puede contener letras y espacios") }));
        var result = await _service.RegistrarClienteAsync(cliente);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual("Los nombres y apellidos solo puede contener letras y espacios", result.Message);
    }

    [TestMethod]
    public async Task RegistrarCliente_DeberiaFallar_SiCedulaOTelefonoInvalidos()
    {
        var cliente = new Cliente { Codigo = "CLI01", Nombres = "Juan", Apellidos = "Perez", Cedula = "123", Telefono = "098", Correo_Electronico = "test@mail.com" };
        _mockValidator.Setup(v => v.ValidateAsync(It.IsAny<Cliente>(), default)).ReturnsAsync(new ValidationResult(new List<ValidationFailure>{ new ValidationFailure("Cedula", "La c�dula y el tel�fono deben contener exactamente 10 d�gitos num�ricos") }));
        var result = await _service.RegistrarClienteAsync(cliente);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual("La c�dula y el tel�fono deben contener exactamente 10 d�gitos num�ricos", result.Message);
    }

    [TestMethod]
    public async Task RegistrarCliente_DeberiaFallar_SiCorreoInvalido()
    {
        var cliente = new Cliente { Codigo = "CLI01", Nombres = "Juan", Apellidos = "Perez", Cedula = "1234567890", Telefono = "0987654321", Correo_Electronico = "correo_invalido" };
        _mockValidator.Setup(v => v.ValidateAsync(It.IsAny<Cliente>(), default)).ReturnsAsync(new ValidationResult(new List<ValidationFailure> { new ValidationFailure("Correo Electronico", "El correo electr�nico no tiene un formato v�lido") }));
        var result = await _service.RegistrarClienteAsync(cliente);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual("El correo electr�nico no tiene un formato v�lido", result.Message);
    }

    [TestMethod]
    public async Task RegistrarCliente_DeberiaFallar_SiCodigoDuplicado()
    {
        var cliente = new Cliente { Codigo = "CLI01", Nombres = "Juan", Apellidos = "Perez", Cedula = "1234567890", Telefono = "0987654321", Correo_Electronico = "test@mail.com" };
        _mockValidator.Setup(v => v.ValidateAsync(It.IsAny<Cliente>(), default)).ReturnsAsync(new ValidationResult(new List<ValidationFailure> { new ValidationFailure("Codigo", Mensajes.MESSAGE_CODE_EXITS) }));
        _mockRepository.Setup(r => r.ListarClientesAsync()).ReturnsAsync(new List<ClienteResponse> { new ClienteResponse { Codigo = "CLI01" } });
        var result = await _service.RegistrarClienteAsync(cliente);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual(Mensajes.MESSAGE_CODE_EXITS, result.Message);
    }

    [TestMethod]
    public async Task EditarCliente_DeberiaFallar_SiClienteEsNulo()
    {
        var result = await _service.EditarClienteAsync(null);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual(Mensajes.MESSAGE_NULL, result.Message);
    }

    [TestMethod]
    public async Task EditarCliente_DeberiaFallar_SiCamposObligatoriosVacios()
    {
        var cliente = new Cliente { Nombres = "", Apellidos = "", Cedula = "", Telefono = "", Correo_Electronico = "" };
        _mockValidator.Setup(v => v.ValidateAsync(It.IsAny<Cliente>(), default)).ReturnsAsync(new ValidationResult(new List<ValidationFailure> { new ValidationFailure("Codigo", Mensajes.MESSAGE_EMPTY) }));
        var result = await _service.EditarClienteAsync(cliente);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual(Mensajes.MESSAGE_EMPTY, result.Message);
    }

    [TestMethod]
    public async Task EditarCliente_DeberiaFallar_SiClienteNoExiste()
    {
        var cliente = new Cliente { Id_Cliente = 1, Nombres = "Juan", Apellidos = "Perez", Cedula = "1234567890", Telefono = "0987654321", Correo_Electronico = "test@mail.com" };
        _mockValidator.Setup(v => v.ValidateAsync(It.IsAny<Cliente>(), default)).ReturnsAsync(new ValidationResult());
        _mockRepository.Setup(r => r.ObtenerClienteAsync(1)).ReturnsAsync((ClienteResponse)null);
        var result = await _service.EditarClienteAsync(cliente);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual(Mensajes.MESSAGE_QUERY_NOT_FOUND, result.Message);
    }

    [TestMethod]
    public async Task EditarCliente_DeberiaFallar_SiNombresInvalidos()
    {
        var cliente = new Cliente { Id_Cliente = 1, Nombres = "Juan123", Apellidos = "Perez", Cedula = "1234567890", Telefono = "0987654321", Correo_Electronico = "test@mail.com" };
        _mockValidator.Setup(v => v.ValidateAsync(It.IsAny<Cliente>(), default)).ReturnsAsync(new ValidationResult(new List<ValidationFailure> { new ValidationFailure("Nombres Apellidos", "Los nombres y apellidos solo puede contener letras y espacios") }));
        _mockRepository.Setup(r => r.ObtenerClienteAsync(1)).ReturnsAsync(new ClienteResponse());
        var result = await _service.EditarClienteAsync(cliente);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual("Los nombres y apellidos solo puede contener letras y espacios", result.Message);
    }

    [TestMethod]
    public async Task EditarCliente_DeberiaFallar_SiCedulaOTelefonoInvalidos()
    {
        var cliente = new Cliente { Id_Cliente = 1, Nombres = "Juan", Apellidos = "Perez", Cedula = "123", Telefono = "098", Correo_Electronico = "test@mail.com" };
        _mockValidator.Setup(v => v.ValidateAsync(It.IsAny<Cliente>(), default)).ReturnsAsync(new ValidationResult(new List<ValidationFailure> { new ValidationFailure("Cedula", "La c�dula y el tel�fono deben contener exactamente 10 d�gitos num�ricos") }));
        _mockRepository.Setup(r => r.ObtenerClienteAsync(1)).ReturnsAsync(new ClienteResponse());
        var result = await _service.EditarClienteAsync(cliente);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual("La c�dula y el tel�fono deben contener exactamente 10 d�gitos num�ricos", result.Message);
    }

    [TestMethod]
    public async Task EditarCliente_DeberiaFallar_SiCorreoInvalido()
    {
        var cliente = new Cliente { Id_Cliente = 1, Nombres = "Juan", Apellidos = "Perez", Cedula = "1234567890", Telefono = "0987654321", Correo_Electronico = "correo_invalido" };
        _mockValidator.Setup(v => v.ValidateAsync(It.IsAny<Cliente>(), default)).ReturnsAsync(new ValidationResult(new List<ValidationFailure> { new ValidationFailure("Correo Electronico", "El correo electr�nico no tiene un formato v�lido") }));
        _mockRepository.Setup(r => r.ObtenerClienteAsync(1)).ReturnsAsync(new ClienteResponse());
        var result = await _service.EditarClienteAsync(cliente);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual("El correo electr�nico no tiene un formato v�lido", result.Message);
    }

    [TestMethod]
    public async Task EditarCliente_DeberiaFallar_SiCedulaOTelefonoDuplicados()
    {
        var cliente = new Cliente { Id_Cliente = 1, Nombres = "Juan", Apellidos = "Perez", Cedula = "1234567890", Telefono = "0987654321", Correo_Electronico = "test@mail.com" };
        _mockValidator.Setup(v => v.ValidateAsync(It.IsAny<Cliente>(), default)).ReturnsAsync(new ValidationResult());
        _mockRepository.Setup(r => r.ObtenerClienteAsync(1)).ReturnsAsync(new ClienteResponse());
        _mockRepository.Setup(r => r.ListarClientesAsync()).ReturnsAsync(new List<ClienteResponse>{ new ClienteResponse { Id_Cliente = 2, Cedula = "1234567890" }});
        var result = await _service.EditarClienteAsync(cliente);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual(Mensajes.MESSAGE_CEDULA_EXITS, result.Message);
    }

    [TestMethod]
    public async Task EditarCliente_DeberiaSerExitoso()
    {
        var cliente = new Cliente { Id_Cliente = 1, Nombres = "Juan", Apellidos = "Perez", Cedula = "1234567890", Telefono = "0987654321", Correo_Electronico = "test@mail.com" };
        _mockValidator.Setup(v => v.ValidateAsync(It.IsAny<Cliente>(), default)).ReturnsAsync(new ValidationResult());
        _mockRepository.Setup(r => r.ObtenerClienteAsync(1)).ReturnsAsync(new ClienteResponse());
        _mockRepository.Setup(r => r.ListarClientesAsync()).ReturnsAsync(new List<ClienteResponse>());
        _mockRepository.Setup(r => r.EditarClienteAsync(cliente, 1)).ReturnsAsync(1);
        var result = await _service.EditarClienteAsync(cliente);

        Assert.IsTrue(result.IsSuccess);
        Assert.AreEqual(Mensajes.MESSAGE_UPDATE, result.Message);
    }

    [TestMethod]
    public async Task EliminarCliente_DeberiaFallar_SiClienteNoExiste()
    {
        _mockRepository.Setup(r => r.ObtenerClienteAsync(99)).ReturnsAsync((ClienteResponse)null);
        var result = await _service.EliminarClienteAsync(99);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual(Mensajes.MESSAGE_QUERY_NOT_FOUND, result.Message);
    }

    [TestMethod]
    public async Task EliminarCliente_DeberiaSerExitoso()
    {
        _mockRepository.Setup(r => r.ObtenerClienteAsync(1)).ReturnsAsync(new ClienteResponse());
        _mockRepository.Setup(r => r.EliminarClienteAsync(1)).ReturnsAsync(1);
        var result = await _service.EliminarClienteAsync(1);

        Assert.IsTrue(result.IsSuccess);
        Assert.AreEqual(Mensajes.MESSAGE_DELETE, result.Message);
    }

    [TestMethod]
    public async Task EliminarCliente_DeberiaFallar_SiOperacionDevuelveCero()
    {
        _mockRepository.Setup(r => r.ObtenerClienteAsync(1)).ReturnsAsync(new ClienteResponse());
        _mockRepository.Setup(r => r.EliminarClienteAsync(1)).ReturnsAsync(0);
        var result = await _service.EliminarClienteAsync(1);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual(Mensajes.MESSAGE_DELETE_FAILLED, result.Message);
    }

    [TestMethod]
    public async Task ListarClientes_DebePropagarExcepcion_YRegistrarError()
    {
        var excepcion = new InvalidOperationException("Error de repositorio");
        _mockRepository.Setup(r => r.ListarClientesAsync()).ThrowsAsync(excepcion);

        await Assert.ThrowsExceptionAsync<InvalidOperationException>(() => _service.ListarClientesAsync());

        _mockAuditoria.Verify(a => a.RegistrarErrorAsync("Listar Clientes", excepcion), Times.Once);
    }
}

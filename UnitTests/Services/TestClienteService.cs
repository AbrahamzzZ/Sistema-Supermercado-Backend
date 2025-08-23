using DataBaseFirst.Models;
using DataBaseFirst.Repository.InterfacesRepository;
using DataBaseFirst.Services;
using Moq;
using Utilities.Shared;

namespace UnitTests.Services;

[TestClass]
public class TestClienteService
{
    private Mock<IClienteRepository> _mockRepository;
    private ClienteService _service;

    [TestInitialize]
    public void Setup()
    {
        _mockRepository = new Mock<IClienteRepository>();
        //_service = new ClienteService(_mockRepository.Object);
    }

    [TestMethod]
    public async Task RegistrarCliente_DeberiaFallar_SiClienteEsNull()
    {
        var result = await _service.RegistrarClienteAsync(null);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual(Mensajes.MESSAGE_VALIDATE, result.Message);
    }

    [TestMethod]
    public async Task RegistrarCliente_DeberiaFallar_SiCamposObligatoriosVacios()
    {
        var cliente = new Cliente { Codigo = "", Nombres = "", Apellidos = "", Cedula = "", Telefono = "", Correo_Electronico = "" };

        var result = await _service.RegistrarClienteAsync(cliente);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual(Mensajes.MESSAGE_EMPTY, result.Message);
    }

    [TestMethod]
    public async Task RegistrarCliente_DeberiaFallar_SiNombresInvalidos()
    {
        var cliente = new Cliente { Codigo = "CLI01", Nombres = "Juan123", Apellidos = "Perez", Cedula = "1234567890", Telefono = "0987654321", Correo_Electronico = "test@mail.com" };

        var result = await _service.RegistrarClienteAsync(cliente);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual("Los nombres y apellidos solo puede contener letras y espacios", result.Message);
    }

    [TestMethod]
    public async Task RegistrarCliente_DeberiaFallar_SiCedulaOTelefonoInvalidos()
    {
        var cliente = new Cliente { Codigo = "CLI01", Nombres = "Juan", Apellidos = "Perez", Cedula = "123", Telefono = "098", Correo_Electronico = "test@mail.com" };

        var result = await _service.RegistrarClienteAsync(cliente);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual("La cédula y el teléfono deben contener exactamente 10 dígitos numéricos", result.Message);
    }

    [TestMethod]
    public async Task RegistrarCliente_DeberiaFallar_SiCorreoInvalido()
    {
        var cliente = new Cliente { Codigo = "CLI01", Nombres = "Juan", Apellidos = "Perez", Cedula = "1234567890", Telefono = "0987654321", Correo_Electronico = "correo_invalido" };

        var result = await _service.RegistrarClienteAsync(cliente);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual("El correo electrónico no tiene un formato válido", result.Message);
    }

    [TestMethod]
    public async Task RegistrarCliente_DeberiaFallar_SiCodigoDuplicado()
    {
        var cliente = new Cliente { Codigo = "CLI01", Nombres = "Juan", Apellidos = "Perez", Cedula = "1234567890", Telefono = "0987654321", Correo_Electronico = "test@mail.com" };

        _mockRepository.Setup(r => r.ListarClientesAsync()).ReturnsAsync(new List<Cliente> { new Cliente { Codigo = "CLI01" } });

        var result = await _service.RegistrarClienteAsync(cliente);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual("El código ya existe", result.Message);
    }

    [TestMethod]
    public async Task EditarCliente_DeberiaFallar_SiClienteEsNull()
    {
        var result = await _service.EditarClienteAsync(null);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual(Mensajes.MESSAGE_VALIDATE, result.Message);
    }

    [TestMethod]
    public async Task EditarCliente_DeberiaFallar_SiCamposObligatoriosVacios()
    {
        var cliente = new Cliente { Nombres = "", Apellidos = "", Cedula = "", Telefono = "", Correo_Electronico = "" };

        var result = await _service.EditarClienteAsync(cliente);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual(Mensajes.MESSAGE_EMPTY, result.Message);
    }

    [TestMethod]
    public async Task EditarCliente_DeberiaFallar_SiClienteNoExiste()
    {
        var cliente = new Cliente { Id_Cliente = 1, Nombres = "Juan", Apellidos = "Perez", Cedula = "1234567890", Telefono = "0987654321", Correo_Electronico = "test@mail.com" };

        _mockRepository.Setup(r => r.ObtenerClienteAsync(1)).ReturnsAsync((Cliente)null);

        var result = await _service.EditarClienteAsync(cliente);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual(Mensajes.MESSAGE_QUERY_NOT_FOUND, result.Message);
    }

    [TestMethod]
    public async Task EditarCliente_DeberiaFallar_SiNombresInvalidos()
    {
        var cliente = new Cliente { Id_Cliente = 1, Nombres = "Juan123", Apellidos = "Perez", Cedula = "1234567890", Telefono = "0987654321", Correo_Electronico = "test@mail.com" };

        _mockRepository.Setup(r => r.ObtenerClienteAsync(1)).ReturnsAsync(new Cliente());

        var result = await _service.EditarClienteAsync(cliente);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual("Los nombres y apellidos solo puede contener letras y espacios", result.Message);
    }

    [TestMethod]
    public async Task EditarCliente_DeberiaFallar_SiCedulaOTelefonoInvalidos()
    {
        var cliente = new Cliente { Id_Cliente = 1, Nombres = "Juan", Apellidos = "Perez", Cedula = "123", Telefono = "098", Correo_Electronico = "test@mail.com" };

        _mockRepository.Setup(r => r.ObtenerClienteAsync(1)).ReturnsAsync(new Cliente());

        var result = await _service.EditarClienteAsync(cliente);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual("La cédula y el teléfono deben contener exactamente 10 dígitos numéricos", result.Message);
    }

    [TestMethod]
    public async Task EditarCliente_DeberiaFallar_SiCorreoInvalido()
    {
        var cliente = new Cliente { Id_Cliente = 1, Nombres = "Juan", Apellidos = "Perez", Cedula = "1234567890", Telefono = "0987654321", Correo_Electronico = "correo_invalido" };

        _mockRepository.Setup(r => r.ObtenerClienteAsync(1)).ReturnsAsync(new Cliente());

        var result = await _service.EditarClienteAsync(cliente);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual("El correo electrónico no tiene un formato válido", result.Message);
    }

    [TestMethod]
    public async Task EditarCliente_DeberiaFallar_SiCedulaOTelefonoDuplicados()
    {
        var cliente = new Cliente { Id_Cliente = 1, Nombres = "Juan", Apellidos = "Perez", Cedula = "1234567890", Telefono = "0987654321", Correo_Electronico = "test@mail.com" };

        _mockRepository.Setup(r => r.ObtenerClienteAsync(1)).ReturnsAsync(new Cliente());
        _mockRepository.Setup(r => r.ListarClientesAsync()).ReturnsAsync(new List<Cliente>
        {
            new Cliente { Id_Cliente = 2, Cedula = "1234567890" }
        });

        var result = await _service.EditarClienteAsync(cliente);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual("El cédula ya existe.", result.Message);
    }

    [TestMethod]
    public async Task EditarCliente_DeberiaSerExitoso()
    {
        var cliente = new Cliente { Id_Cliente = 1, Nombres = "Juan", Apellidos = "Perez", Cedula = "1234567890", Telefono = "0987654321", Correo_Electronico = "test@mail.com" };

        _mockRepository.Setup(r => r.ObtenerClienteAsync(1)).ReturnsAsync(new Cliente());
        _mockRepository.Setup(r => r.ListarClientesAsync()).ReturnsAsync(new List<Cliente>());
        _mockRepository.Setup(r => r.EditarClienteAsync(cliente)).ReturnsAsync(1);

        var result = await _service.EditarClienteAsync(cliente);

        Assert.IsTrue(result.IsSuccess);
        Assert.AreEqual(Mensajes.MESSAGE_UPDATE, result.Message);
    }

    [TestMethod]
    public async Task EliminarCliente_DeberiaFallar_SiClienteNoExiste()
    {
        _mockRepository.Setup(r => r.ObtenerClienteAsync(99)).ReturnsAsync((Cliente)null);

        var result = await _service.EliminarClienteAsync(99);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual(Mensajes.MESSAGE_QUERY_NOT_FOUND, result.Message);
    }

    [TestMethod]
    public async Task EliminarCliente_DeberiaSerExitoso()
    {
        _mockRepository.Setup(r => r.ObtenerClienteAsync(1)).ReturnsAsync(new Cliente());
        _mockRepository.Setup(r => r.EliminarClienteAsync(1)).ReturnsAsync(1);

        var result = await _service.EliminarClienteAsync(1);

        Assert.IsTrue(result.IsSuccess);
        Assert.AreEqual(Mensajes.MESSAGE_DELETE, result.Message);
    }

    [TestMethod]
    public async Task EliminarCliente_DeberiaFallar_SiOperacionDevuelveCero()
    {
        _mockRepository.Setup(r => r.ObtenerClienteAsync(1)).ReturnsAsync(new Cliente());
        _mockRepository.Setup(r => r.EliminarClienteAsync(1)).ReturnsAsync(0);

        var result = await _service.EliminarClienteAsync(1);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual(Mensajes.MESSAGE_DELETE_FAILLED, result.Message);
    }
}

using DataBaseFirst.Models;
using DataBaseFirst.Repository.InterfacesRepository;
using DataBaseFirst.Services;
using Moq;
using Utilities.Shared;

namespace UnitTests.Services;

[TestClass]
public class TestTransportistaService
{
    private Mock<ITransportistaRepository> _mockRepository;
    private TransportistaService _service;

    [TestInitialize]
    public void Setup()
    {
        _mockRepository = new Mock<ITransportistaRepository>();
        //_service = new TransportistaService(_mockRepository.Object);
    }

    [TestMethod]
    public async Task RegistrarTransportista_DeberiaFallar_SiClienteEsNull()
    {
        var result = await _service.RegistrarTransportistaAsync(null);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual(Mensajes.MESSAGE_NULL, result.Message);
    }

    [TestMethod]
    public async Task RegistrarTransportista_DeberiaFallar_SiCamposObligatoriosVacios()
    {
        var transportista = new Transportistum { Codigo = "", Nombres = "", Apellidos = "", Cedula = "", Telefono = "", Correo_Electronico = "" };

        var result = await _service.RegistrarTransportistaAsync(transportista);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual(Mensajes.MESSAGE_EMPTY, result.Message);
    }

    [TestMethod]
    public async Task RegistrarTransportista_DeberiaFallar_SiNombresInvalidos()
    {
        var transportista = new Transportistum { Codigo = "CLI01", Nombres = "Juan123", Apellidos = "Perez", Cedula = "1234567890", Telefono = "0987654321", Correo_Electronico = "test@mail.com" };

        var result = await _service.RegistrarTransportistaAsync(transportista);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual("Los nombres y apellidos solo puede contener letras y espacios", result.Message);
    }

    [TestMethod]
    public async Task RegistrarTransportista_DeberiaFallar_SiCedulaOTelefonoInvalidos()
    {
        var transportista = new Transportistum { Codigo = "CLI01", Nombres = "Juan", Apellidos = "Perez", Cedula = "123", Telefono = "098", Correo_Electronico = "test@mail.com" };

        var result = await _service.RegistrarTransportistaAsync(transportista);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual("La cédula y el teléfono deben contener exactamente 10 dígitos numéricos", result.Message);
    }

    [TestMethod]
    public async Task RegistrarTransportista_DeberiaFallar_SiCorreoInvalido()
    {
        var transportista = new Transportistum { Codigo = "CLI01", Nombres = "Juan", Apellidos = "Perez", Cedula = "1234567890", Telefono = "0987654321", Correo_Electronico = "correo_invalido" };

        var result = await _service.RegistrarTransportistaAsync(transportista);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual("El correo electrónico no tiene un formato válido", result.Message);
    }

    [TestMethod]
    public async Task RegistrarTransportista_DeberiaFallar_SiCodigoDuplicado()
    {
        var transportista = new Transportistum { Codigo = "CLI01", Nombres = "Juan", Apellidos = "Perez", Cedula = "1234567890", Telefono = "0987654321", Correo_Electronico = "test@mail.com" };

        _mockRepository.Setup(r => r.ListarTransportistasAsync()).ReturnsAsync(new List<Transportistum> { new Transportistum { Codigo = "CLI01" } });

        var result = await _service.RegistrarTransportistaAsync(transportista);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual("El código ya existe", result.Message);
    }

    [TestMethod]
    public async Task EditarTransportista_DeberiaFallar_SiClienteEsNull()
    {
        var result = await _service.EditarTransportistaAsync(null);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual(Mensajes.MESSAGE_NULL, result.Message);
    }

    [TestMethod]
    public async Task EditarTransportista_DeberiaFallar_SiCamposObligatoriosVacios()
    {
        var transportista = new Transportistum { Nombres = "", Apellidos = "", Cedula = "", Telefono = "", Correo_Electronico = "" };

        var result = await _service.EditarTransportistaAsync(transportista);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual(Mensajes.MESSAGE_EMPTY, result.Message);
    }

    [TestMethod]
    public async Task EditarTranspotista_DeberiaFallar_SiClienteNoExiste()
    {
        var transportista = new Transportistum { Id_Transportista = 1, Nombres = "Juan", Apellidos = "Perez", Cedula = "1234567890", Telefono = "0987654321", Correo_Electronico = "test@mail.com" };

        _mockRepository.Setup(r => r.ObtenerTransportistaAsync(1)).ReturnsAsync((Transportistum)null);

        var result = await _service.EditarTransportistaAsync(transportista);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual(Mensajes.MESSAGE_QUERY_NOT_FOUND, result.Message);
    }

    [TestMethod]
    public async Task EditarTranspotista_DeberiaFallar_SiNombresInvalidos()
    {
        var transportista = new Transportistum { Id_Transportista = 1, Nombres = "Juan123", Apellidos = "Perez", Cedula = "1234567890", Telefono = "0987654321", Correo_Electronico = "test@mail.com" };

        _mockRepository.Setup(r => r.ObtenerTransportistaAsync(1)).ReturnsAsync(new Transportistum());

        var result = await _service.EditarTransportistaAsync(transportista);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual("Los nombres y apellidos solo puede contener letras y espacios", result.Message);
    }

    [TestMethod]
    public async Task EditarTransportista_DeberiaFallar_SiCedulaOTelefonoInvalidos()
    {
        var cliente = new Transportistum { Id_Transportista = 1, Nombres = "Juan", Apellidos = "Perez", Cedula = "123", Telefono = "098", Correo_Electronico = "test@mail.com" };

        _mockRepository.Setup(r => r.ObtenerTransportistaAsync(1)).ReturnsAsync(new Transportistum());

        var result = await _service.EditarTransportistaAsync(cliente);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual("La cédula y el teléfono deben contener exactamente 10 dígitos numéricos", result.Message);
    }

    [TestMethod]
    public async Task EditarTransportista_DeberiaFallar_SiCorreoInvalido()
    {
        var cliente = new Transportistum { Id_Transportista = 1, Nombres = "Juan", Apellidos = "Perez", Cedula = "1234567890", Telefono = "0987654321", Correo_Electronico = "correo_invalido" };

        _mockRepository.Setup(r => r.ObtenerTransportistaAsync(1)).ReturnsAsync(new Transportistum());

        var result = await _service.EditarTransportistaAsync(cliente);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual("El correo electrónico no tiene un formato válido", result.Message);
    }

    [TestMethod]
    public async Task EditarTransportista_DeberiaFallar_SiCedulaOTelefonoDuplicados()
    {
        var cliente = new Transportistum { Id_Transportista = 1, Nombres = "Juan", Apellidos = "Perez", Cedula = "1234567890", Telefono = "0987654321", Correo_Electronico = "test@mail.com" };

        _mockRepository.Setup(r => r.ObtenerTransportistaAsync(1)).ReturnsAsync(new Transportistum());
        _mockRepository.Setup(r => r.ListarTransportistasAsync()).ReturnsAsync(new List<Transportistum>
        {
            new Transportistum { Id_Transportista = 2, Cedula = "1234567890" }
        });

        var result = await _service.EditarTransportistaAsync(cliente);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual("El cédula ya existe.", result.Message);
    }

    [TestMethod]
    public async Task EditarTransportista_DeberiaSerExitoso()
    {
        var transportista = new Transportistum { Id_Transportista = 1, Nombres = "Juan", Apellidos = "Perez", Cedula = "1234567890", Telefono = "0987654321", Correo_Electronico = "test@mail.com" };

        _mockRepository.Setup(r => r.ObtenerTransportistaAsync(1)).ReturnsAsync(new Transportistum());
        _mockRepository.Setup(r => r.ListarTransportistasAsync()).ReturnsAsync(new List<Transportistum>());
        _mockRepository.Setup(r => r.EditarTransportistaAsync(transportista)).ReturnsAsync(1);

        var result = await _service.EditarTransportistaAsync(transportista);

        Assert.IsTrue(result.IsSuccess);
        Assert.AreEqual(Mensajes.MESSAGE_UPDATE, result.Message);
    }

    [TestMethod]
    public async Task EliminarTransportista_DeberiaFallar_SiClienteNoExiste()
    {
        _mockRepository.Setup(r => r.ObtenerTransportistaAsync(99)).ReturnsAsync((Transportistum)null);

        var result = await _service.EliminarTransportistaAsync(99);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual(Mensajes.MESSAGE_QUERY_NOT_FOUND, result.Message);
    }

    [TestMethod]
    public async Task EliminarTransportista_DeberiaSerExitoso()
    {
        _mockRepository.Setup(r => r.ObtenerTransportistaAsync(1)).ReturnsAsync(new Transportistum());
        _mockRepository.Setup(r => r.EliminarTransportistaAsync(1)).ReturnsAsync(1);

        var result = await _service.EliminarTransportistaAsync(1);

        Assert.IsTrue(result.IsSuccess);
        Assert.AreEqual(Mensajes.MESSAGE_DELETE, result.Message);
    }

    [TestMethod]
    public async Task EliminarTransportista_DeberiaFallar_SiOperacionDevuelveCero()
    {
        _mockRepository.Setup(r => r.ObtenerTransportistaAsync(1)).ReturnsAsync(new Transportistum());
        _mockRepository.Setup(r => r.EliminarTransportistaAsync(1)).ReturnsAsync(0);

        var result = await _service.EliminarTransportistaAsync(1);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual(Mensajes.MESSAGE_DELETE_FAILLED, result.Message);
    }
}

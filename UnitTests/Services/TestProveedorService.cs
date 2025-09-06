using DataBaseFirst.Models;
using Infrastructure.Repository.InterfacesRepository;
using Infrastructure.Services;
using Moq;
using Utilities.Shared;

namespace UnitTests.Services;

[TestClass]
public class TestProveedorService
{
    private Mock<IProveedorRepository> _mockRepository;
    private ProveedorService _service;

    [TestInitialize]
    public void Setup()
    {
        _mockRepository = new Mock<IProveedorRepository>();
        //_service = new ProveedorService(_mockRepository.Object);
    }

    [TestMethod]
    public async Task RegistrarProveedor_DeberiaFallar_SiProveedorEsNull()
    {
        var result = await _service.RegistrarProveedorAsync(null);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual(Mensajes.MESSAGE_NULL, result.Message);
    }

    [TestMethod]
    public async Task RegistrarProveedor_DeberiaFallar_SiCamposObligatoriosVacios()
    {
        var proveedor = new Proveedor { Codigo = "", Nombres = "", Apellidos = "", Cedula = "", Telefono = "", Correo_Electronico = "" };

        var result = await _service.RegistrarProveedorAsync(proveedor);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual(Mensajes.MESSAGE_EMPTY, result.Message);
    }

    [TestMethod]
    public async Task RegistrarProveedor_DeberiaFallar_SiNombresInvalidos()
    {
        var proveedor = new Proveedor { Codigo = "CLI01", Nombres = "Juan123", Apellidos = "Perez", Cedula = "1234567890", Telefono = "0987654321", Correo_Electronico = "test@mail.com" };

        var result = await _service.RegistrarProveedorAsync(proveedor);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual("Los nombres y apellidos solo puede contener letras y espacios", result.Message);
    }

    [TestMethod]
    public async Task RegistrarProveedor_DeberiaFallar_SiCedulaOTelefonoInvalidos()
    {
        var proveedor = new Proveedor { Codigo = "CLI01", Nombres = "Juan", Apellidos = "Perez", Cedula = "123", Telefono = "098", Correo_Electronico = "test@mail.com" };

        var result = await _service.RegistrarProveedorAsync(proveedor);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual("La cédula y el teléfono deben contener exactamente 10 dígitos numéricos", result.Message);
    }

    [TestMethod]
    public async Task RegistrarProveedor_DeberiaFallar_SiCorreoInvalido()
    {
        var proveedor = new Proveedor { Codigo = "CLI01", Nombres = "Juan", Apellidos = "Perez", Cedula = "1234567890", Telefono = "0987654321", Correo_Electronico = "correo_invalido" };

        var result = await _service.RegistrarProveedorAsync(proveedor);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual("El correo electrónico no tiene un formato válido", result.Message);
    }

    [TestMethod]
    public async Task RegistrarProveedor_DeberiaFallar_SiCodigoDuplicado()
    {
        var cliente = new Proveedor { Codigo = "CLI01", Nombres = "Juan", Apellidos = "Perez", Cedula = "1234567890", Telefono = "0987654321", Correo_Electronico = "test@mail.com" };

        _mockRepository.Setup(r => r.ListarProveedoresAsync()).ReturnsAsync(new List<Proveedor> { new Proveedor { Codigo = "CLI01" } });

        var result = await _service.RegistrarProveedorAsync(cliente);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual("El código ya existe", result.Message);
    }

    [TestMethod]
    public async Task EditarProveedor_DeberiaFallar_SiProveedorEsNull()
    {
        var result = await _service.EditarProveedorAsync(null);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual(Mensajes.MESSAGE_NULL, result.Message);
    }

    [TestMethod]
    public async Task EditarProveedor_DeberiaFallar_SiCamposObligatoriosVacios()
    {
        var proveedor = new Proveedor { Nombres = "", Apellidos = "", Cedula = "", Telefono = "", Correo_Electronico = "" };

        var result = await _service.EditarProveedorAsync(proveedor);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual(Mensajes.MESSAGE_EMPTY, result.Message);
    }

    [TestMethod]
    public async Task EditarProveedor_DeberiaFallar_SiProveedorNoExiste()
    {
        var proveedor = new Proveedor { Id_Proveedor = 1, Nombres = "Juan", Apellidos = "Perez", Cedula = "1234567890", Telefono = "0987654321", Correo_Electronico = "test@mail.com" };

        _mockRepository.Setup(r => r.ObtenerProveedorAsync(1)).ReturnsAsync((Proveedor)null);

        var result = await _service.EditarProveedorAsync(proveedor);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual(Mensajes.MESSAGE_QUERY_NOT_FOUND, result.Message);
    }

    [TestMethod]
    public async Task EditarProveedor_DeberiaFallar_SiNombresInvalidos()
    {
        var proveedor = new Proveedor { Id_Proveedor = 1, Nombres = "Juan123", Apellidos = "Perez", Cedula = "1234567890", Telefono = "0987654321", Correo_Electronico = "test@mail.com" };

        _mockRepository.Setup(r => r.ObtenerProveedorAsync(1)).ReturnsAsync(new Proveedor());

        var result = await _service.EditarProveedorAsync(proveedor);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual("Los nombres y apellidos solo puede contener letras y espacios", result.Message);
    }

    [TestMethod]
    public async Task EditarProveedor_DeberiaFallar_SiCedulaOTelefonoInvalidos()
    {
        var proveedor = new Proveedor { Id_Proveedor = 1, Nombres = "Juan", Apellidos = "Perez", Cedula = "123", Telefono = "098", Correo_Electronico = "test@mail.com" };

        _mockRepository.Setup(r => r.ObtenerProveedorAsync(1)).ReturnsAsync(new Proveedor());

        var result = await _service.EditarProveedorAsync(proveedor);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual("La cédula y el teléfono deben contener exactamente 10 dígitos numéricos", result.Message);
    }

    [TestMethod]
    public async Task EditarProveedor_DeberiaFallar_SiCorreoInvalido()
    {
        var proveedor = new Proveedor { Id_Proveedor = 1, Nombres = "Juan", Apellidos = "Perez", Cedula = "1234567890", Telefono = "0987654321", Correo_Electronico = "correo_invalido" };

        _mockRepository.Setup(r => r.ObtenerProveedorAsync(1)).ReturnsAsync(new Proveedor());

        var result = await _service.EditarProveedorAsync(proveedor);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual("El correo electrónico no tiene un formato válido", result.Message);
    }

    [TestMethod]
    public async Task EditarProveedor_DeberiaFallar_SiCedulaOTelefonoDuplicados()
    {
        var proveedor = new Proveedor { Id_Proveedor = 1, Nombres = "Juan", Apellidos = "Perez", Cedula = "1234567890", Telefono = "0987654321", Correo_Electronico = "test@mail.com" };

        _mockRepository.Setup(r => r.ObtenerProveedorAsync(1)).ReturnsAsync(new Proveedor());
        _mockRepository.Setup(r => r.ListarProveedoresAsync()).ReturnsAsync(new List<Proveedor>
        {
            new Proveedor { Id_Proveedor = 2, Cedula = "1234567890" }
        });

        var result = await _service.EditarProveedorAsync(proveedor);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual("El cédula ya existe.", result.Message);
    }

    [TestMethod]
    public async Task EditarProveedor_DeberiaSerExitoso()
    {
        var proveedor = new Proveedor { Id_Proveedor = 1, Nombres = "Juan", Apellidos = "Perez", Cedula = "1234567890", Telefono = "0987654321", Correo_Electronico = "test@mail.com" };

        _mockRepository.Setup(r => r.ObtenerProveedorAsync(1)).ReturnsAsync(new Proveedor());
        _mockRepository.Setup(r => r.ListarProveedoresAsync()).ReturnsAsync(new List<Proveedor>());
        _mockRepository.Setup(r => r.EditarProveedorAsync(proveedor)).ReturnsAsync(1);

        var result = await _service.EditarProveedorAsync(proveedor);

        Assert.IsTrue(result.IsSuccess);
        Assert.AreEqual(Mensajes.MESSAGE_UPDATE, result.Message);
    }

    [TestMethod]
    public async Task EliminarProveedo_DeberiaFallar_SiProveedorNoExiste()
    {
        _mockRepository.Setup(r => r.ObtenerProveedorAsync(99)).ReturnsAsync((Proveedor)null);

        var result = await _service.EliminarProveedorAsync(99);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual(Mensajes.MESSAGE_QUERY_NOT_FOUND, result.Message);
    }

    [TestMethod]
    public async Task EliminarProveedor_DeberiaSerExitoso()
    {
        _mockRepository.Setup(r => r.ObtenerProveedorAsync(1)).ReturnsAsync(new Proveedor());
        _mockRepository.Setup(r => r.EliminarProveedorAsync(1)).ReturnsAsync(1);

        var result = await _service.EliminarProveedorAsync(1);

        Assert.IsTrue(result.IsSuccess);
        Assert.AreEqual(Mensajes.MESSAGE_DELETE, result.Message);
    }

    [TestMethod]
    public async Task EliminarProveedor_DeberiaFallar_SiOperacionDevuelveCero()
    {
        _mockRepository.Setup(r => r.ObtenerProveedorAsync(1)).ReturnsAsync(new Proveedor());
        _mockRepository.Setup(r => r.EliminarProveedorAsync(1)).ReturnsAsync(0);

        var result = await _service.EliminarProveedorAsync(1);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual(Mensajes.MESSAGE_DELETE_FAILLED, result.Message);
    }
}

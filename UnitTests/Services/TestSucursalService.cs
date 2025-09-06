using DataBaseFirst.Models;
using Infrastructure.Repository.InterfacesRepository;
using Infrastructure.Services;
using Moq;
using Utilities.Shared;

namespace UnitTests.Services;

[TestClass]
public class TestSucursalService
{
    private Mock<ISucursalRepository> _mockRepository;
    private SucursalService _service;

    [TestInitialize]
    public void Setup()
    {
        _mockRepository = new Mock<ISucursalRepository>();
        //_service = new SucursalService(_mockRepository.Object);
    }

    [TestMethod]
    public async Task ListarSucursalesAsync_ReturnsSuccess_WhenDataExists()
    {
        var sucursales = new List<Sucursal>
            {
                new Sucursal { Id_Sucursal = 1, Nombre_Sucursal = "Sucursal A" }
            };

        _mockRepository.Setup(r => r.ListarSucursalesAsync())
            .ReturnsAsync(sucursales);

        var result = await _service.ListarSucursalesAsync();

        Assert.IsTrue(result.IsSuccess);
        Assert.AreEqual(Mensajes.MESSAGE_QUERY, result.Message);
        Assert.AreEqual(1, result.Data.Count);
    }

    [TestMethod]
    public async Task ListarSucursalesAsync_ReturnsEmpty_WhenNoData()
    {
        _mockRepository.Setup(r => r.ListarSucursalesAsync())
            .ReturnsAsync(new List<Sucursal>());

        var result = await _service.ListarSucursalesAsync();

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual(Mensajes.MESSAGE_QUERY_EMPTY, result.Message);
    }

    [TestMethod]
    public async Task ObtenerSucursalAsync_ReturnsSuccess_WhenFound()
    {
        var sucursal = new Sucursal { Id_Sucursal = 1, Nombre_Sucursal = "Sucursal A" };

        _mockRepository.Setup(r => r.ObtenerSucursalAsync(1))
            .ReturnsAsync(sucursal);

        var result = await _service.ObtenerSucursalAsync(1);

        Assert.IsTrue(result.IsSuccess);
        Assert.AreEqual(sucursal.Nombre_Sucursal, result.Data.Nombre_Sucursal);
    }

    [TestMethod]
    public async Task ObtenerSucursalAsync_ReturnsNotFound_WhenNotExists()
    {
        _mockRepository.Setup(r => r.ObtenerSucursalAsync(99))
            .ReturnsAsync((Sucursal)null);

        var result = await _service.ObtenerSucursalAsync(99);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual(Mensajes.MESSAGE_QUERY_NOT_FOUND, result.Message);
    }

    [TestMethod]
    public async Task RegistrarSucursalAsync_ReturnsValidate_WhenNull()
    {
        var result = await _service.RegistrarSucursalAsync(null);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual(Mensajes.MESSAGE_NULL, result.Message);
    }

    [TestMethod]
    public async Task RegistrarSucursalAsync_ReturnsEmpty_WhenFieldsEmpty()
    {
        var sucursal = new Sucursal();

        var result = await _service.RegistrarSucursalAsync(sucursal);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual(Mensajes.MESSAGE_EMPTY, result.Message);
    }

    [TestMethod]
    public async Task RegistrarSucursalAsync_ReturnsSuccess_WhenValid()
    {
        var sucursal = new Sucursal
        {
            Codigo = "S001",
            Nombre_Sucursal = "Sucursal Test",
            Direccion_Sucursal = "Av. Central",
            Ciudad_Sucursal = "Quito",
            Latitud = -2.1,
            Longitud = -79.8
        };

        _mockRepository.Setup(r => r.ListarSucursalesAsync())
            .ReturnsAsync(new List<Sucursal>());

        _mockRepository.Setup(r => r.RegistrarSucursalAsync(sucursal))
            .ReturnsAsync(1);

        var result = await _service.RegistrarSucursalAsync(sucursal);

        Assert.IsTrue(result.IsSuccess);
        Assert.AreEqual(Mensajes.MESSAGE_REGISTER, result.Message);
    }

    [TestMethod]
    public async Task EditarSucursalAsync_ReturnsNotFound_WhenSucursalDoesNotExist()
    {
        var sucursal = new Sucursal { Id_Sucursal = 1, Nombre_Sucursal = "Sucursal A", Direccion_Sucursal = "Av. Central", Ciudad_Sucursal = "Quito" };

        _mockRepository.Setup(r => r.ObtenerSucursalAsync(1))
            .ReturnsAsync((Sucursal)null);

        var result = await _service.EditarSucursalAsync(sucursal);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual(Mensajes.MESSAGE_QUERY_NOT_FOUND, result.Message);
    }

    [TestMethod]
    public async Task EditarSucursalAsync_ReturnsSuccess_WhenValid()
    {
        var sucursal = new Sucursal { Id_Sucursal = 1, Nombre_Sucursal = "Sucursal Editada", Direccion_Sucursal = "Av. Central", Ciudad_Sucursal = "Guayaquil", Latitud = -2.1, Longitud = -79.8  };

        _mockRepository.Setup(r => r.ObtenerSucursalAsync(1))
            .ReturnsAsync(sucursal);

        _mockRepository.Setup(r => r.ListarSucursalesAsync())
            .ReturnsAsync(new List<Sucursal>());

        _mockRepository.Setup(r => r.EditarSucursalAsync(sucursal))
            .ReturnsAsync(1);

        var result = await _service.EditarSucursalAsync(sucursal);

        Assert.IsTrue(result.IsSuccess);
        Assert.AreEqual(Mensajes.MESSAGE_UPDATE, result.Message);
    }

    [TestMethod]
    public async Task EliminarSucursalAsync_ReturnsNotFound_WhenSucursalDoesNotExist()
    {
        _mockRepository.Setup(r => r.ObtenerSucursalAsync(99))
            .ReturnsAsync((Sucursal)null);

        var result = await _service.EliminarSucursalAsync(99);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual(Mensajes.MESSAGE_QUERY_NOT_FOUND, result.Message);
    }

    [TestMethod]
    public async Task EliminarSucursalAsync_ReturnsSuccess_WhenDeleted()
    {
        var sucursal = new Sucursal { Id_Sucursal = 1, Nombre_Sucursal = "Sucursal A" };

        _mockRepository.Setup(r => r.ObtenerSucursalAsync(1))
            .ReturnsAsync(sucursal);

        _mockRepository.Setup(r => r.EliminarSucursalAsync(1))
            .ReturnsAsync(1);

        var result = await _service.EliminarSucursalAsync(1);

        Assert.IsTrue(result.IsSuccess);
        Assert.AreEqual(Mensajes.MESSAGE_DELETE, result.Message);
    }
}


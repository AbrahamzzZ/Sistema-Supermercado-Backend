using DataBaseFirst.Models;
using DataBaseFirst.Models.Dto;
using DataBaseFirst.Repository.InterfacesRepository;
using Infrastructure.Services;
using Moq;
using Utilities.Shared;

namespace UnitTests.Services;

[TestClass]
public class TestOfertaService
{
    private Mock<IOfertaRepository> _mockRepository;
    private OfertaService _service;

    [TestInitialize]
    public void Setup()
    {
        _mockRepository = new Mock<IOfertaRepository>();
        //_service = new OfertaService(_mockRepository.Object);
    }

    [TestMethod]
    public async Task ListarOfertasAsync_ReturnsSuccess_WhenDataExists()
    {
        var ofertas = new List<OfertaProducto> { new OfertaProducto { Id_Oferta = 1, Nombre_Oferta = "Oferta A" } };

        _mockRepository.Setup(r => r.ListarOfertasAsync())
            .ReturnsAsync(ofertas);

        var result = await _service.ListarOfertasAsync();

        Assert.IsTrue(result.IsSuccess);
        Assert.AreEqual(Mensajes.MESSAGE_QUERY, result.Message);
        Assert.AreEqual(1, result.Data.Count);
    }

    [TestMethod]
    public async Task ListarOfertasAsync_ReturnsEmpty_WhenNoData()
    {
        _mockRepository.Setup(r => r.ListarOfertasAsync())
            .ReturnsAsync(new List<OfertaProducto>());

        var result = await _service.ListarOfertasAsync();

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual(Mensajes.MESSAGE_QUERY_EMPTY, result.Message);
    }

    [TestMethod]
    public async Task ObtenerOfertaAsync_ReturnsSuccess_WhenFound()
    {
        var oferta = new Ofertum { Id_Oferta = 1, Nombre_Oferta = "Oferta A" };

        _mockRepository.Setup(r => r.ObtenerOfertaAsync(1))
            .ReturnsAsync(oferta);

        var result = await _service.ObtenerOfertaAsync(1);

        Assert.IsTrue(result.IsSuccess);
        Assert.AreEqual(oferta.Nombre_Oferta, result.Data.Nombre_Oferta);
    }

    [TestMethod]
    public async Task ObtenerOfertaAsync_ReturnsNotFound_WhenNotExists()
    {
        _mockRepository.Setup(r => r.ObtenerOfertaAsync(99))
            .ReturnsAsync((Ofertum)null);

        var result = await _service.ObtenerOfertaAsync(99);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual(Mensajes.MESSAGE_QUERY_NOT_FOUND, result.Message);
    }

    [TestMethod]
    public async Task RegistrarOfertaAsync_ReturnsValidate_WhenNull()
    {
        var result = await _service.RegistrarOfertaAsync(null);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual(Mensajes.MESSAGE_NULL, result.Message);
    }

    [TestMethod]
    public async Task RegistrarOfertaAsync_ReturnsEmpty_WhenFieldsEmpty()
    {
        var oferta = new Ofertum();

        var result = await _service.RegistrarOfertaAsync(oferta);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual(Mensajes.MESSAGE_EMPTY, result.Message);
    }

    [TestMethod]
    public async Task RegistrarOfertaAsync_ReturnsError_WhenDiscountInvalid()
    {
        var oferta = new Ofertum { Id_Producto = 1, Codigo = "OF001", Nombre_Oferta = "Oferta Test", Descripcion = "Descripción", Descuento = 150, Fecha_Fin = DateOnly.FromDateTime(DateTime.Now.AddDays(10)) };

        var result = await _service.RegistrarOfertaAsync(oferta);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual("El descuento debe estar entre 0 y 100", result.Message);
    }

    [TestMethod]
    public async Task RegistrarOfertaAsync_ReturnsError_WhenEndDatePast()
    {
        var oferta = new Ofertum { Id_Producto = 1, Codigo = "OF001",  Nombre_Oferta = "Oferta Test", Descripcion = "Descripción", Descuento = 10, Fecha_Fin = DateOnly.FromDateTime(DateTime.Now.AddDays(-1)) };

        var result = await _service.RegistrarOfertaAsync(oferta);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual("La fecha de fin debe ser una fecha futura", result.Message);
    }

    [TestMethod]
    public async Task RegistrarOfertaAsync_ReturnsSuccess_WhenValid()
    {
        var oferta = new Ofertum { Id_Producto = 1, Codigo = "OF001", Nombre_Oferta = "Oferta Test", Descripcion = "Descripción", Descuento = 20, Fecha_Fin = DateOnly.FromDateTime(DateTime.Now.AddDays(5)) };

        _mockRepository.Setup(r => r.ListarOfertasAsync())
            .ReturnsAsync(new List<OfertaProducto>());

        _mockRepository.Setup(r => r.RegistrarOfertaAsync(oferta))
            .ReturnsAsync(1);

        var result = await _service.RegistrarOfertaAsync(oferta);

        Assert.IsTrue(result.IsSuccess);
        Assert.AreEqual(Mensajes.MESSAGE_REGISTER, result.Message);
    }

    [TestMethod]
    public async Task EditarOfertaAsync_ReturnsNotFound_WhenNotExists()
    {
        var oferta = new Ofertum { Id_Oferta = 1, Id_Producto = 1, Nombre_Oferta = "Oferta Editar", Descripcion = "Desc", Descuento = 10, Fecha_Fin = DateOnly.FromDateTime(DateTime.Now.AddDays(5)) };

        _mockRepository.Setup(r => r.ObtenerOfertaAsync(1))
            .ReturnsAsync((Ofertum)null);

        var result = await _service.EditarOfertaAsync(oferta);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual(Mensajes.MESSAGE_QUERY_NOT_FOUND, result.Message);
    }

    [TestMethod]
    public async Task EditarOfertaAsync_ReturnsSuccess_WhenValid()
    {
        var oferta = new Ofertum { Id_Oferta = 1, Id_Producto = 1, Nombre_Oferta = "Oferta Editada", Descripcion = "Desc", Descuento = 15, Fecha_Fin = DateOnly.FromDateTime(DateTime.Now.AddDays(10)) };

        var ofertaExistente = new Ofertum { Id_Oferta = 1, Nombre_Oferta = "Oferta Original" };

        _mockRepository.Setup(r => r.ObtenerOfertaAsync(1))
            .ReturnsAsync(ofertaExistente);

        _mockRepository.Setup(r => r.ListarOfertasAsync())
            .ReturnsAsync(new List<OfertaProducto>());

        _mockRepository.Setup(r => r.EditarOfertaAsync(oferta))
            .ReturnsAsync(1);

        var result = await _service.EditarOfertaAsync(oferta);

        Assert.IsTrue(result.IsSuccess);
        Assert.AreEqual(Mensajes.MESSAGE_UPDATE, result.Message);
    }

    [TestMethod]
    public async Task EliminarOfertaAsync_ReturnsNotFound_WhenNotExists()
    {
        _mockRepository.Setup(r => r.ObtenerOfertaAsync(99))
            .ReturnsAsync((Ofertum)null);

        var result = await _service.EliminarOfertaAsync(99);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual(Mensajes.MESSAGE_QUERY_NOT_FOUND, result.Message);
    }

    [TestMethod]
    public async Task EliminarOfertaAsync_ReturnsSuccess_WhenDeleted()
    {
        var oferta = new Ofertum { Id_Oferta = 1, Nombre_Oferta = "Oferta A" };

        _mockRepository.Setup(r => r.ObtenerOfertaAsync(1))
            .ReturnsAsync(oferta);

        _mockRepository.Setup(r => r.EliminarOfertaAsync(1))
            .ReturnsAsync(1);

        var result = await _service.EliminarOfertaAsync(1);

        Assert.IsTrue(result.IsSuccess);
        Assert.AreEqual(Mensajes.MESSAGE_DELETE, result.Message);
    }
}

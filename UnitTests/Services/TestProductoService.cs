using Domain.Models;
using Domain.Models.Dto;
using Infrastructure.Repository.InterfacesRepository;
using Infrastructure.Services;
using Moq;
using Utilities.Shared;

namespace UnitTests.Services;

[TestClass]
public class TestProductoService
{
    private Mock<IProductoRepository> _mockRepository;
    private ProductoService _service;

    [TestInitialize]
    public void Setup()
    {
        _mockRepository = new Mock<IProductoRepository>();
        //_service = new ProductoService(_mockRepository.Object);
    }

    [TestMethod]
    public async Task ListarProductosAsync_ReturnsSuccess_WhenDataExists()
    {
        var productos = new List<ProductoCategoria> { new ProductoCategoria { Id_Producto = 1, Nombre_Producto = "Producto A" } };

        _mockRepository.Setup(r => r.ListarProductosAsync())
            .ReturnsAsync(productos);

        var result = await _service.ListarProductosAsync();

        Assert.IsTrue(result.IsSuccess);
        Assert.AreEqual(Mensajes.MESSAGE_QUERY, result.Message);
        Assert.AreEqual(1, result.Data.Count);
    }

    [TestMethod]
    public async Task ListarProductosAsync_ReturnsEmpty_WhenNoData()
    {
        _mockRepository.Setup(r => r.ListarProductosAsync())
            .ReturnsAsync(new List<ProductoCategoria>());

        var result = await _service.ListarProductosAsync();

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual(Mensajes.MESSAGE_QUERY_EMPTY, result.Message);
    }

    [TestMethod]
    public async Task ObtenerProductoAsync_ReturnsSuccess_WhenFound()
    {
        var producto = new ProductoRespuesta { Id_Producto = 1, Nombre_Producto = "Producto A" };

        _mockRepository.Setup(r => r.ObtenerProductoAsync(1))
            .ReturnsAsync(producto);

        var result = await _service.ObtenerProductoAsync(1);

        Assert.IsTrue(result.IsSuccess);
        Assert.AreEqual(producto.Nombre_Producto, result.Data.Nombre_Producto);
    }

    [TestMethod]
    public async Task ObtenerProductoAsync_ReturnsNotFound_WhenNotExists()
    {
        _mockRepository.Setup(r => r.ObtenerProductoAsync(99))
            .ReturnsAsync((ProductoRespuesta)null);

        var result = await _service.ObtenerProductoAsync(99);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual(Mensajes.MESSAGE_QUERY_NOT_FOUND, result.Message);
    }

    [TestMethod]
    public async Task RegistrarProductoAsync_ReturnsValidate_WhenNull()
    {
        var result = await _service.RegistrarProductoAsync(null);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual(Mensajes.MESSAGE_NULL, result.Message);
    }

    [TestMethod]
    public async Task RegistrarProductoAsync_ReturnsEmpty_WhenFieldsEmpty()
    {
        var producto = new Producto();

        var result = await _service.RegistrarProductoAsync(producto);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual(Mensajes.MESSAGE_EMPTY, result.Message);
    }

    [TestMethod]
    public async Task RegistrarProductoAsync_ReturnsSuccess_WhenValid()
    {
        var producto = new Producto { Codigo = "P001", Id_Categoria = 1, Nombre_Producto = "Producto Test", Descripcion = "Desc", Pais_Origen = "Ecuador" };

        _mockRepository.Setup(r => r.ListarProductosAsync())
            .ReturnsAsync(new List<ProductoCategoria>());

        _mockRepository.Setup(r => r.RegistrarProductoAsync(producto))
            .ReturnsAsync(1);

        var result = await _service.RegistrarProductoAsync(producto);

        Assert.IsTrue(result.IsSuccess);
        Assert.AreEqual(Mensajes.MESSAGE_REGISTER, result.Message);
    }

    [TestMethod]
    public async Task EditarProductoAsync_ReturnsNotFound_WhenNotExists()
    {
        var producto = new Producto { Id_Producto = 1,  Id_Categoria = 1, Nombre_Producto = "Producto Editar",  Descripcion = "Desc", Pais_Origen = "Ecuador" };

        _mockRepository.Setup(r => r.ObtenerProductoAsync(1))
            .ReturnsAsync((ProductoRespuesta)null);

        var result = await _service.EditarProductoAsync(producto);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual(Mensajes.MESSAGE_QUERY_NOT_FOUND, result.Message);
    }

    [TestMethod]
    public async Task EditarProductoAsync_ReturnsSuccess_WhenValid()
    {
        var producto = new Producto { Id_Producto = 1, Id_Categoria = 1, Nombre_Producto = "Producto Editado", Descripcion = "Desc",  Pais_Origen = "Ecuador" };

        var productoExistente = new ProductoRespuesta { Id_Producto = 1, Nombre_Producto = "Producto Original" };

        _mockRepository.Setup(r => r.ObtenerProductoAsync(1))
            .ReturnsAsync(productoExistente);

        _mockRepository.Setup(r => r.ListarProductosAsync())
            .ReturnsAsync(new List<ProductoCategoria>());

        _mockRepository.Setup(r => r.EditarProductoAsync(producto))
            .ReturnsAsync(1);

        var result = await _service.EditarProductoAsync(producto);

        Assert.IsTrue(result.IsSuccess);
        Assert.AreEqual(Mensajes.MESSAGE_UPDATE, result.Message);
    }

    [TestMethod]
    public async Task EliminarProductoAsync_ReturnsNotFound_WhenNotExists()
    {
        _mockRepository.Setup(r => r.ObtenerProductoAsync(99))
            .ReturnsAsync((ProductoRespuesta)null);

        var result = await _service.EliminarProductoAsync(99);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual(Mensajes.MESSAGE_QUERY_NOT_FOUND, result.Message);
    }

    [TestMethod]
    public async Task EliminarProductoAsync_ReturnsSuccess_WhenDeleted()
    {
        var producto = new ProductoRespuesta { Id_Producto = 1, Nombre_Producto = "Producto A" };

        _mockRepository.Setup(r => r.ObtenerProductoAsync(1))
            .ReturnsAsync(producto);

        _mockRepository.Setup(r => r.EliminarProductoAsync(1))
            .ReturnsAsync(1);

        var result = await _service.EliminarProductoAsync(1);

        Assert.IsTrue(result.IsSuccess);
        Assert.AreEqual(Mensajes.MESSAGE_DELETE, result.Message);
    }
}

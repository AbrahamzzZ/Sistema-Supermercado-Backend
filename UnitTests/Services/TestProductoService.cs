using Domain.Models;
using Domain.Models.Dto;
using FluentValidation;
using FluentValidation.Results;
using Infrastructure.Repository.InterfacesRepository;
using Infrastructure.Services;
using Moq;
using Utilities.Shared;

namespace UnitTests.Services;

[TestClass]
public class TestProductoService
{
    private Mock<IProductoRepository> _mockRepository;
    private Mock<IValidator<Producto>> _mockValidator;
    private ProductoService _service;

    [TestInitialize]
    public void Setup()
    {
        _mockRepository = new Mock<IProductoRepository>();
        _mockValidator = new Mock<IValidator<Producto>>();
        _service = new ProductoService(
            _mockRepository.Object,
            _mockValidator.Object
        );
    }

    [TestMethod]
    public async Task ListarProductosAsync_ReturnsSuccess_WhenDataExists()
    {
        var productos = new List<ProductoCategoria> { new ProductoCategoria { Id_Producto = 1, Nombre_Producto = "Producto A" } };
        _mockRepository.Setup(r => r.ListarProductosAsync()).ReturnsAsync(productos);
        var result = await _service.ListarProductosAsync();

        Assert.IsTrue(result.IsSuccess);
        Assert.AreEqual(Mensajes.MESSAGE_QUERY, result.Message);
        Assert.AreEqual(1, result.Data.Count);
    }

    [TestMethod]
    public async Task ListarProductosAsync_ReturnsEmpty_WhenNoData()
    {
        _mockRepository.Setup(r => r.ListarProductosAsync()).ReturnsAsync(new List<ProductoCategoria>());
        var result = await _service.ListarProductosAsync();

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual(Mensajes.MESSAGE_QUERY_EMPTY, result.Message);
    }

    [TestMethod]
    public async Task ObtenerProductoAsync_ReturnsSuccess_WhenFound()
    {
        var producto = new ProductoRespuesta { Id_Producto = 1, Nombre_Producto = "Producto A" };
        _mockRepository.Setup(r => r.ObtenerProductoAsync(1)).ReturnsAsync(producto);
        var result = await _service.ObtenerProductoAsync(1);

        Assert.IsTrue(result.IsSuccess);
        Assert.AreEqual(producto.Nombre_Producto, result.Data.Nombre_Producto);
    }

    [TestMethod]
    public async Task ObtenerProductoAsync_ReturnsNotFound_WhenNotExists()
    {
        _mockRepository.Setup(r => r.ObtenerProductoAsync(99)).ReturnsAsync((ProductoRespuesta)null);
        var result = await _service.ObtenerProductoAsync(99);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual(Mensajes.MESSAGE_QUERY_NOT_FOUND, result.Message);
    }

    [TestMethod]
    public async Task RegistrarProducto_DeberiaFallar_SiProductoEsNull()
    {
        var result = await _service.RegistrarProductoAsync(null);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual(Mensajes.MESSAGE_NULL, result.Message);
    }

    [TestMethod]
    public async Task RegistrarProducto_DeberiaFallar_SiCamposObligatoriosVacios()
    {
        var producto = new Producto { Codigo = "", Nombre_Producto = "", Descripcion = "", Pais_Origen = "", Id_Categoria = 0, Precio_Compra = 0, Precio_Venta = 0 };
        _mockValidator.Setup(v => v.ValidateAsync(It.IsAny<Producto>(), default)).ReturnsAsync(new ValidationResult(new List<ValidationFailure> { new ValidationFailure("Codigo", Mensajes.MESSAGE_EMPTY) }));
        var result = await _service.RegistrarProductoAsync(producto);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual(Mensajes.MESSAGE_EMPTY, result.Message);
    }

    [TestMethod]
    public async Task RegistrarProducto_DeberiaSerExitoso()
    {
        var producto = new Producto { Codigo = "P001", Id_Categoria = 1, Nombre_Producto = "Producto Test", Descripcion = "Desc", Pais_Origen = "Ecuador" };
        _mockValidator.Setup(v => v.ValidateAsync(It.IsAny<Producto>(), default)).ReturnsAsync(new ValidationResult());
        _mockRepository.Setup(r => r.ListarProductosAsync()).ReturnsAsync(new List<ProductoCategoria>());
        _mockRepository.Setup(r => r.RegistrarProductoAsync(producto)).ReturnsAsync(1);
        var result = await _service.RegistrarProductoAsync(producto);

        Assert.IsTrue(result.IsSuccess);
        Assert.AreEqual(Mensajes.MESSAGE_REGISTER, result.Message);
    }

    [TestMethod]
    public async Task EditarProducto_DeberiaFallar_SiProductoNoExiste()
    {
        var producto = new Producto { Id_Producto = 1,  Id_Categoria = 1, Nombre_Producto = "Producto Editar",  Descripcion = "Desc", Pais_Origen = "Ecuador" };
        _mockValidator.Setup(v => v.ValidateAsync(It.IsAny<Producto>(), default)).ReturnsAsync(new ValidationResult());
        _mockRepository.Setup(r => r.ObtenerProductoAsync(1)).ReturnsAsync((ProductoRespuesta)null);
        var result = await _service.EditarProductoAsync(producto);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual(Mensajes.MESSAGE_QUERY_NOT_FOUND, result.Message);
    }

    [TestMethod]
    public async Task EditarProducto_DeberiaSerExitoso()
    {
        var producto = new Producto { Id_Producto = 1, Id_Categoria = 1, Nombre_Producto = "Producto Editado", Descripcion = "Desc",  Pais_Origen = "Ecuador" };
        _mockValidator.Setup(v => v.ValidateAsync(It.IsAny<Producto>(), default)).ReturnsAsync(new ValidationResult());
        _mockRepository.Setup(r => r.ObtenerProductoAsync(1)).ReturnsAsync(new ProductoRespuesta());
        _mockRepository.Setup(r => r.ListarProductosAsync()).ReturnsAsync(new List<ProductoCategoria>());
        _mockRepository.Setup(r => r.EditarProductoAsync(producto)).ReturnsAsync(1);
        var result = await _service.EditarProductoAsync(producto);

        Assert.IsTrue(result.IsSuccess);
        Assert.AreEqual(Mensajes.MESSAGE_UPDATE, result.Message);
    }

    [TestMethod]
    public async Task EliminarProducto_DeberiaFallar_SiProductoNoExiste()
    {
        _mockRepository.Setup(r => r.ObtenerProductoAsync(99)).ReturnsAsync((ProductoRespuesta)null);
        var result = await _service.EliminarProductoAsync(99);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual(Mensajes.MESSAGE_QUERY_NOT_FOUND, result.Message);
    }

    [TestMethod]
    public async Task EliminarProducto_DeberiaSerExitos()
    {

        _mockRepository.Setup(r => r.ObtenerProductoAsync(1)).ReturnsAsync(new ProductoRespuesta());
        _mockRepository.Setup(r => r.EliminarProductoAsync(1)).ReturnsAsync(1);
        var result = await _service.EliminarProductoAsync(1);

        Assert.IsTrue(result.IsSuccess);
        Assert.AreEqual(Mensajes.MESSAGE_DELETE, result.Message);
    }
}

using APIRestSistemaVentas.Controllers;
using Domain.Models;
using Domain.Models.Dto;
using Infrastructure.Repository.InterfacesServices;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Utilities.Shared;

namespace UnitTests.Controller;

[TestClass]
public class TestProductoController
{
    private Mock<IProductoService> _mockService;
    private ProductoController _controller;

    [TestInitialize]
    public void Setup()
    {
        _mockService = new Mock<IProductoService>();
        //_controller = new ProductoController(_mockService.Object);
    }

    // Test 1: GET all productos
    [TestMethod]
    public async Task GetProductos_DeberiaRetornarOk()
    {
        var data = new List<ProductoCategoria> { new ProductoCategoria { Id_Producto = 1, Nombre_Producto = "Snacks" } };
        _mockService.Setup(s => s.ListarProductosAsync())
            .ReturnsAsync(new ApiResponse<List<ProductoCategoria>> { IsSuccess = true, Data = data });

        var result = await _controller.GetProductos();

        var okResult = result.Result as OkObjectResult;
        Assert.IsNotNull(okResult);
        var response = okResult.Value as ApiResponse<List<ProductoCategoria>>;
        Assert.IsTrue(response.IsSuccess);
        Assert.AreEqual(1, response.Data.Count);
    }

    [TestMethod]
    public async Task GetProducto_CuandoExiste_DeberiaRetornarOk()
    {
        var producto = new ProductoRespuesta { Id_Producto = 1, Nombre_Producto = "Snacks" };
        _mockService.Setup(s => s.ObtenerProductoAsync(1))
            .ReturnsAsync(new ApiResponse<ProductoRespuesta> { IsSuccess = true, Data = producto });

        var result = await _controller.GetProducto(1);

        var okResult = result.Result as OkObjectResult;
        Assert.IsNotNull(okResult);
        var response = okResult.Value as ApiResponse<ProductoRespuesta>;
        Assert.IsTrue(response.IsSuccess);
        Assert.AreEqual("Snacks", response.Data.Nombre_Producto);
    }

    [TestMethod]
    public async Task GetProducto_CuandoNoExiste_DeberiaRetornarNotFound()
    {
        _mockService.Setup(s => s.ObtenerProductoAsync(999))
            .ReturnsAsync(new ApiResponse<ProductoRespuesta> { IsSuccess = false, Message = "No encontrado" });

        var result = await _controller.GetProducto(999);

        var notFoundResult = result.Result as NotFoundObjectResult;
        Assert.IsNotNull(notFoundResult);
        var response = notFoundResult.Value as ApiResponse<ProductoRespuesta>;
        Assert.IsFalse(response.IsSuccess);
    }

    [TestMethod]
    public async Task RegistrarProducto_CuandoExito_DeberiaRetornarOk()
    {
        var nuevoProducto = new Producto { Nombre_Producto = "Snacks" };
        _mockService.Setup(s => s.RegistrarProductoAsync(nuevoProducto))
            .ReturnsAsync(new ApiResponse<object> { IsSuccess = true, Message = "Registrado" });

        var result = await _controller.RegistrarUsuario(nuevoProducto);

        var okResult = result.Result as OkObjectResult;
        Assert.IsNotNull(okResult);
        var response = okResult.Value as ApiResponse<object>;
        Assert.IsTrue(response.IsSuccess);
        Assert.AreEqual("Registrado", response.Message);
    }

    [TestMethod]
    public async Task RegistrarProducto_CuandoError_DeberiaRetornarBadRequest()
    {
        var nuevoProducto = new Producto { Nombre_Producto = "" }; 
        _mockService.Setup(s => s.RegistrarProductoAsync(nuevoProducto))
            .ReturnsAsync(new ApiResponse<object> { IsSuccess = false, Message = "Error" });

        var result = await _controller.RegistrarUsuario(nuevoProducto);

        var badRequestResult = result.Result as BadRequestObjectResult;
        Assert.IsNotNull(badRequestResult);
        var response = badRequestResult.Value as ApiResponse<object>;
        Assert.IsFalse(response.IsSuccess);
    }

    [TestMethod]
    public async Task EditarCategoria_DeberiaRetornarOk_SiValido()
    {
        var categoria = new Producto { Id_Producto = 1, Nombre_Producto = "Leche" };

        _mockService.Setup(s => s.EditarProductoAsync(categoria))
            .ReturnsAsync(new ApiResponse<object> { IsSuccess = true, Message = Mensajes.MESSAGE_UPDATE });

        var result = await _controller.EditarProducto(1, categoria);

        Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
    }

    [TestMethod]
    public async Task EditarProducto_DeberiaRetornarBadRequest_SiInvalido()
    {
        var productos = new Producto { Id_Producto = 1, Nombre_Producto = "" };

        _mockService.Setup(s => s.EditarProductoAsync(productos))
            .ReturnsAsync(new ApiResponse<object> { IsSuccess = false, Message = Mensajes.MESSAGE_EMPTY });

        var result = await _controller.EditarProducto(1, productos);

        Assert.IsInstanceOfType(result.Result, typeof(BadRequestObjectResult));
    }

    [TestMethod]
    public async Task EliminarProducto_CuandoExito_DeberiaRetornarOk()
    {
        _mockService.Setup(s => s.EliminarProductoAsync(1))
            .ReturnsAsync(new ApiResponse<int> { IsSuccess = true, Message = "Eliminado" });

        var result = await _controller.EliminarProducto(1);

        var okResult = result.Result as OkObjectResult;
        Assert.IsNotNull(okResult);
        var response = okResult.Value as ApiResponse<int>;
        Assert.IsTrue(response.IsSuccess);
    }

    [TestMethod]
    public async Task EliminarProducto_CuandoNoExiste_DeberiaRetornarNotFound()
    {
        _mockService.Setup(s => s.EliminarProductoAsync(999))
            .ReturnsAsync(new ApiResponse<int> { IsSuccess = false, Message = "No encontrado" });

        var result = await _controller.EliminarProducto(999);

        var notFoundResult = result.Result as NotFoundObjectResult;
        Assert.IsNotNull(notFoundResult);
        var response = notFoundResult.Value as ApiResponse<int>;
        Assert.IsFalse(response.IsSuccess);
    }
}

using APIRestSistemaVentas.Controllers;
using DataBaseFirst.Models;
using Infrastructure.Repository.InterfacesServices;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Utilities.Shared;

namespace UnitTests.Controller;

[TestClass]
public class TestCategoriaController
{
    private Mock<ICategoriaService> _mockService;
    private CategoriaController _controller;

    [TestInitialize]
    public void Setup()
    {
        _mockService = new Mock<ICategoriaService>();
        //_controller = new CategoriaController(_mockService.Object);
    }

    [TestMethod]
    public async Task GetCategorias_DeberiaRetornarOk()
    {
        _mockService.Setup(s => s.ListarCategoriasAsync())
            .ReturnsAsync(new ApiResponse<List<Categorium>> { IsSuccess = true,Data = new List<Categorium> { new Categorium { Id_Categoria = 1, Nombre_Categoria = "Snacks" } }});

        var result = await _controller.GetCategorias();

        var okResult = result.Result as OkObjectResult;
        Assert.IsNotNull(okResult);
        var response = okResult.Value as ApiResponse<List<Categorium>>;
        Assert.IsTrue(response.IsSuccess);
        Assert.AreEqual(1, response.Data.Count);
    }

    [TestMethod]
    public async Task GetCategoria_DeberiaRetornarOk_SiExiste()
    {
        _mockService.Setup(s => s.ObtenerCategoriaAsync(1))
            .ReturnsAsync(new ApiResponse<Categorium> { IsSuccess = true, Data = new Categorium { Id_Categoria = 1, Nombre_Categoria = "Snacks" } });

        var result = await _controller.GetCategoria(1);

        var okResult = result.Result as OkObjectResult;
        Assert.IsNotNull(okResult);
        var response = okResult.Value as ApiResponse<Categorium>;
        Assert.IsTrue(response.IsSuccess);
        Assert.AreEqual("Snacks", response.Data.Nombre_Categoria);
    }

    [TestMethod]
    public async Task GetCategoria_DeberiaRetornarNotFound_SiNoExiste()
    {
        _mockService.Setup(s => s.ObtenerCategoriaAsync(99))
            .ReturnsAsync(new ApiResponse<Categorium> { IsSuccess = false, Message = Mensajes.MESSAGE_QUERY_NOT_FOUND });

        var result = await _controller.GetCategoria(99);

        Assert.IsInstanceOfType(result.Result, typeof(NotFoundObjectResult));
    }

    [TestMethod]
    public async Task RegistrarCategoria_DeberiaRetornarOk_SiValido()
    {
        var nuevaCategoria = new Categorium { Codigo = "CAT01", Nombre_Categoria = "Bebidas" };

        _mockService.Setup(s => s.RegistrarCategoriaAsync(nuevaCategoria))
            .ReturnsAsync(new ApiResponse<object> { IsSuccess = true, Message = Mensajes.MESSAGE_REGISTER });

        var result = await _controller.RegistrarCategoria(nuevaCategoria);

        Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
    }

    [TestMethod]
    public async Task RegistrarCategoria_DeberiaRetornarBadRequest_SiInvalido()
    {
        var nuevaCategoria = new Categorium { Codigo = "", Nombre_Categoria = "" };

        _mockService.Setup(s => s.RegistrarCategoriaAsync(nuevaCategoria))
            .ReturnsAsync(new ApiResponse<object> { IsSuccess = false, Message = Mensajes.MESSAGE_EMPTY });

        var result = await _controller.RegistrarCategoria(nuevaCategoria);

        Assert.IsInstanceOfType(result.Result, typeof(BadRequestObjectResult));
    }

    [TestMethod]
    public async Task EditarCategoria_DeberiaRetornarOk_SiValido()
    {
        var categoria = new Categorium { Id_Categoria = 1, Nombre_Categoria = "Lácteos" };

        _mockService.Setup(s => s.EditarCategoriaAsync(categoria))
            .ReturnsAsync(new ApiResponse<object> { IsSuccess = true, Message = Mensajes.MESSAGE_UPDATE });

        var result = await _controller.EditarCategoria(1, categoria);

        Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
    }

    [TestMethod]
    public async Task EditarCategoria_DeberiaRetornarBadRequest_SiInvalido()
    {
        var categoria = new Categorium { Id_Categoria = 1, Nombre_Categoria = "" };

        _mockService.Setup(s => s.EditarCategoriaAsync(categoria))
            .ReturnsAsync(new ApiResponse<object> { IsSuccess = false, Message = Mensajes.MESSAGE_EMPTY });

        var result = await _controller.EditarCategoria(1, categoria);

        Assert.IsInstanceOfType(result.Result, typeof(BadRequestObjectResult));
    }

    [TestMethod]
    public async Task EliminarCategoria_DeberiaRetornarOk_SiExiste()
    {
        _mockService.Setup(s => s.EliminarCategoriaAsync(1))
            .ReturnsAsync(new ApiResponse<int> { IsSuccess = true, Message = Mensajes.MESSAGE_DELETE });

        var result = await _controller.EliminarCategoria(1);

        Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
    }

    [TestMethod]
    public async Task EliminarCategoria_DeberiaRetornarNotFound_SiNoExiste()
    {
        _mockService.Setup(s => s.EliminarCategoriaAsync(99))
            .ReturnsAsync(new ApiResponse<int> { IsSuccess = false, Message = Mensajes.MESSAGE_QUERY_NOT_FOUND });

        var result = await _controller.EliminarCategoria(99);

        Assert.IsInstanceOfType(result.Result, typeof(NotFoundObjectResult));
    }
}

using APIRestSistemaVentas.Controllers;
using DataBaseFirst.Models;
using DataBaseFirst.Repository.InterfacesServices;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Utilities.Shared;

namespace UnitTests.Controller;

[TestClass]
public class TestClienteController
{
    private Mock<IClienteService> _mockService;
    private ClienteController _controller;

    [TestInitialize]
    public void Setup()
    {
        _mockService = new Mock<IClienteService>();
        //_controller = new ClienteController(_mockService.Object);
    }

    [TestMethod]
    public async Task GetClientes_ReturnsOk_WithClientes()
    {
        var expectedResponse = new ApiResponse<List<Cliente>> { IsSuccess = true, Data = new List<Cliente>() };
        _mockService.Setup(s => s.ListarClientesAsync()).ReturnsAsync(expectedResponse);

        var result = await _controller.GetClientes();
        var okResult = result.Result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);
        Assert.AreEqual(expectedResponse, okResult.Value);
    }

    [TestMethod]
    public async Task GetClientesPaginacion_ReturnsOk_WithData()
    {
        var expectedResponse = new ApiResponse<Paginacion<Cliente>> { IsSuccess = true, Data = new Paginacion<Cliente>() };
        _mockService.Setup(s => s.ListarClientesPaginacionAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(expectedResponse);

        var result = await _controller.GetClientesPaginacion();
        var okResult = result.Result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);
        Assert.AreEqual(expectedResponse, okResult.Value);
    }

    [TestMethod]
    public async Task GetCliente_ReturnsOk_WhenFound()
    {
        var expectedResponse = new ApiResponse<Cliente> { IsSuccess = true, Data = new Cliente() };
        _mockService.Setup(s => s.ObtenerClienteAsync(It.IsAny<int>())).ReturnsAsync(expectedResponse);

        var result = await _controller.GetCliente(1);

        var okResult = result.Result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);
        Assert.AreEqual(expectedResponse, okResult.Value);
    }

    [TestMethod]
    public async Task GetCliente_ReturnsNotFound_WhenNotFound()
    {
        var expectedResponse = new ApiResponse<Cliente> { IsSuccess = false, Message = "Not Found" };
        _mockService.Setup(s => s.ObtenerClienteAsync(It.IsAny<int>())).ReturnsAsync(expectedResponse);

        var result = await _controller.GetCliente(1);

        var notFoundResult = result.Result as NotFoundObjectResult;
        Assert.IsNotNull(notFoundResult);
        Assert.AreEqual(404, notFoundResult.StatusCode);
    }

    [TestMethod]
    public async Task RegistrarCliente_ReturnsOk_WhenSuccess()
    {
        var cliente = new Cliente();
        var expectedResponse = new ApiResponse<object> { IsSuccess = true };
        _mockService.Setup(s => s.RegistrarClienteAsync(cliente)).ReturnsAsync(expectedResponse);

        var result = await _controller.RegistrarCliente(cliente);

        var okResult = result.Result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);
    }

    [TestMethod]
    public async Task RegistrarCliente_ReturnsBadRequest_WhenFail()
    {
        var cliente = new Cliente();
        var expectedResponse = new ApiResponse<object> { IsSuccess = false };
        _mockService.Setup(s => s.RegistrarClienteAsync(cliente)).ReturnsAsync(expectedResponse);

        var result = await _controller.RegistrarCliente(cliente);

        var badRequestResult = result.Result as BadRequestObjectResult;
        Assert.IsNotNull(badRequestResult);
        Assert.AreEqual(400, badRequestResult.StatusCode);
    }

    [TestMethod]
    public async Task EditarCliente_ReturnsOk_WhenSuccess()
    {
        var cliente = new Cliente();
        var expectedResponse = new ApiResponse<object> { IsSuccess = true };
        _mockService.Setup(s => s.EditarClienteAsync(cliente)).ReturnsAsync(expectedResponse);

        var result = await _controller.EditarCliente(1, cliente);

        var okResult = result.Result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);
    }

    [TestMethod]
    public async Task EditarCliente_ReturnsBadRequest_WhenFail()
    {
        var cliente = new Cliente();
        var expectedResponse = new ApiResponse<object> { IsSuccess = false };
        _mockService.Setup(s => s.EditarClienteAsync(cliente)).ReturnsAsync(expectedResponse);

        var result = await _controller.EditarCliente(1, cliente);

        var badRequestResult = result.Result as BadRequestObjectResult;
        Assert.IsNotNull(badRequestResult);
        Assert.AreEqual(400, badRequestResult.StatusCode);
    }

    [TestMethod]
    public async Task EliminarCliente_ReturnsOk_WhenSuccess()
    {
        var expectedResponse = new ApiResponse<int> { IsSuccess = true };
        _mockService.Setup(s => s.EliminarClienteAsync(It.IsAny<int>())).ReturnsAsync(expectedResponse);

        var result = await _controller.EliminarCliente(1);

        var okResult = result.Result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);
    }

    [TestMethod]
    public async Task EliminarCliente_ReturnsNotFound_WhenFail()
    {
        var expectedResponse = new ApiResponse<int> { IsSuccess = false };
        _mockService.Setup(s => s.EliminarClienteAsync(It.IsAny<int>())).ReturnsAsync(expectedResponse);

        var result = await _controller.EliminarCliente(1);

        var notFoundResult = result.Result as NotFoundObjectResult;
        Assert.IsNotNull(notFoundResult);
        Assert.AreEqual(404, notFoundResult.StatusCode);
    }
}

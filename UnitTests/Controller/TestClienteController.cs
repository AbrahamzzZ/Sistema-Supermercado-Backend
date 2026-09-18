using APIRestSistemaVentas.Controllers;
using Domain.Models;
using Domain.Models.Dto.Response.Cliente;
using Infrastructure.Repository.InterfacesServices;
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
        _controller = new ClienteController(_mockService.Object);
    }

    [TestMethod]
    public async Task ObtenerClientes_DebeRetornarOk_ConClientes()
    {
        var expectedResponse = new ApiResponse<List<ClienteResponse>> { IsSuccess = true, Data = new List<ClienteResponse>() };
        _mockService.Setup(s => s.ListarClientesAsync()).ReturnsAsync(expectedResponse);

        var result = await _controller.GetClientes();
        var okResult = result.Result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);
        Assert.AreEqual(expectedResponse, okResult.Value);
    }

    [TestMethod]
    public async Task ObtenerClientesPaginados_DebeRetornarOk_ConDatos()
    {
        var expectedResponse = new ApiResponse<Paginacion<ClienteResponse>> { IsSuccess = true, Data = new Paginacion<ClienteResponse>() };
        _mockService.Setup(s => s.ListarClientesPaginacionAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(expectedResponse);

        var result = await _controller.GetClientesPaginacion();
        var okResult = result.Result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);
        Assert.AreEqual(expectedResponse, okResult.Value);
    }

    [TestMethod]
    public async Task ObtenerCliente_DebeRetornarOk_CuandoExiste()
    {
        var expectedResponse = new ApiResponse<ClienteResponse> { IsSuccess = true, Data = new ClienteResponse() };
        _mockService.Setup(s => s.ObtenerClienteAsync(It.IsAny<int>())).ReturnsAsync(expectedResponse);

        var result = await _controller.GetCliente(1);

        var okResult = result.Result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);
        Assert.AreEqual(expectedResponse, okResult.Value);
    }

    [TestMethod]
    public async Task ObtenerCliente_DebeRetornarNoEncontrado_CuandoNoExiste()
    {
        var expectedResponse = new ApiResponse<ClienteResponse> { IsSuccess = false, Message = "Not Found" };
        _mockService.Setup(s => s.ObtenerClienteAsync(It.IsAny<int>())).ReturnsAsync(expectedResponse);

        var result = await _controller.GetCliente(1);

        var notFoundResult = result.Result as NotFoundObjectResult;
        Assert.IsNotNull(notFoundResult);
        Assert.AreEqual(404, notFoundResult.StatusCode);
    }

    [TestMethod]
    public async Task RegistrarCliente_DebeRetornarOk_CuandoTieneExito()
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
    public async Task RegistrarCliente_DebeRetornarSolicitudIncorrecta_CuandoFalla()
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
    public async Task EditarCliente_DebeRetornarOk_CuandoTieneExito()
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
    public async Task EditarCliente_DebeRetornarSolicitudIncorrecta_CuandoFalla()
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
    public async Task EliminarCliente_DebeRetornarOk_CuandoTieneExito()
    {
        var expectedResponse = new ApiResponse<int> { IsSuccess = true };
        _mockService.Setup(s => s.EliminarClienteAsync(It.IsAny<int>())).ReturnsAsync(expectedResponse);

        var result = await _controller.EliminarCliente(1);

        var okResult = result.Result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);
    }

    [TestMethod]
    public async Task EliminarCliente_DebeRetornarNoEncontrado_CuandoFalla()
    {
        var expectedResponse = new ApiResponse<int> { IsSuccess = false };
        _mockService.Setup(s => s.EliminarClienteAsync(It.IsAny<int>())).ReturnsAsync(expectedResponse);

        var result = await _controller.EliminarCliente(1);

        var notFoundResult = result.Result as NotFoundObjectResult;
        Assert.IsNotNull(notFoundResult);
        Assert.AreEqual(404, notFoundResult.StatusCode);
    }
}

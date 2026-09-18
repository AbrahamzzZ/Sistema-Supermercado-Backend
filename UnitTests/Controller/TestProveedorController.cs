using APIRestSistemaVentas.Controllers;
using Domain.Models;
using Domain.Models.Dto.Response.Provedor;
using Infrastructure.Repository.InterfacesServices;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Utilities.Shared;

namespace UnitTests.Controller;

[TestClass]
public class TestProveedorController
{
    private Mock<IProveedorService> _mockService;
    private ProveedorController _controller;

    [TestInitialize]
    public void Setup()
    {
        _mockService = new Mock<IProveedorService>();
        _controller = new ProveedorController(_mockService.Object);
    }

    [TestMethod]
    public async Task ObtenerProveedores_DebeRetornarOk_ConProveedores()
    {
        var expectedResponse = new ApiResponse<List<ProveedorResponse>> { IsSuccess = true, Data = new List<ProveedorResponse>() };
        _mockService.Setup(s => s.ListarProveedoresAsync()).ReturnsAsync(expectedResponse);

        var result = await _controller.GetProveedores();

        var okResult = result.Result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);
        Assert.AreEqual(expectedResponse, okResult.Value);
    }

    [TestMethod]
    public async Task ObtenerProveedor_DebeRetornarOk_CuandoExiste()
    {
        var expectedResponse = new ApiResponse<ProveedorResponse> { IsSuccess = true, Data = new ProveedorResponse() };
        _mockService.Setup(s => s.ObtenerProveedorAsync(It.IsAny<int>())).ReturnsAsync(expectedResponse);

        var result = await _controller.GetProveedor(1);

        var okResult = result.Result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);
        Assert.AreEqual(expectedResponse, okResult.Value);
    }

    [TestMethod]
    public async Task ObtenerProveedor_DebeRetornarNoEncontrado_CuandoNoExiste()
    {
        var expectedResponse = new ApiResponse<ProveedorResponse> { IsSuccess = false, Message = "Not Found" };
        _mockService.Setup(s => s.ObtenerProveedorAsync(It.IsAny<int>())).ReturnsAsync(expectedResponse);

        var result = await _controller.GetProveedor(1);

        var notFoundResult = result.Result as NotFoundObjectResult;
        Assert.IsNotNull(notFoundResult);
        Assert.AreEqual(404, notFoundResult.StatusCode);
    }

    [TestMethod]
    public async Task ObtenerProveedoresPaginados_DebeRetornarOk_ConDatos()
    {
        var expectedResponse = new ApiResponse<Paginacion<ProveedorResponse>> { IsSuccess = true, Data = new Paginacion<ProveedorResponse>() };
        _mockService.Setup(s => s.ListarProveedoresPaginacionAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(expectedResponse);

        var result = await _controller.GetProveedoresPaginacion();

        var okResult = result.Result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);
        Assert.AreEqual(expectedResponse, okResult.Value);
    }

    [TestMethod]
    public async Task RegistrarProveedor_DebeRetornarOk_CuandoTieneExito()
    {
        var proveedor = new Proveedor();
        var expectedResponse = new ApiResponse<object> { IsSuccess = true };
        _mockService.Setup(s => s.RegistrarProveedorAsync(proveedor)).ReturnsAsync(expectedResponse);

        var result = await _controller.RegistrarProveedor(proveedor);

        var okResult = result.Result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);
    }

    [TestMethod]
    public async Task RegistrarProveedor_DebeRetornarSolicitudIncorrecta_CuandoFalla()
    {
        var proveedor = new Proveedor();
        var expectedResponse = new ApiResponse<object> { IsSuccess = false };
        _mockService.Setup(s => s.RegistrarProveedorAsync(proveedor)).ReturnsAsync(expectedResponse);

        var result = await _controller.RegistrarProveedor(proveedor);

        var badRequestResult = result.Result as BadRequestObjectResult;
        Assert.IsNotNull(badRequestResult);
        Assert.AreEqual(400, badRequestResult.StatusCode);
    }

    [TestMethod]
    public async Task EditarProveedor_DebeRetornarOk_CuandoTieneExito()
    {
        var proveedor = new Proveedor();
        var expectedResponse = new ApiResponse<object> { IsSuccess = true };
        _mockService.Setup(s => s.EditarProveedorAsync(proveedor)).ReturnsAsync(expectedResponse);

        var result = await _controller.EditarProveedor(1, proveedor);

        var okResult = result.Result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);
    }

    [TestMethod]
    public async Task EditarProveedor_DebeRetornarSolicitudIncorrecta_CuandoFalla()
    {
        var proveedor = new Proveedor();
        var expectedResponse = new ApiResponse<object> { IsSuccess = false };
        _mockService.Setup(s => s.EditarProveedorAsync(proveedor)).ReturnsAsync(expectedResponse);

        var result = await _controller.EditarProveedor(1, proveedor);

        var badRequestResult = result.Result as BadRequestObjectResult;
        Assert.IsNotNull(badRequestResult);
        Assert.AreEqual(400, badRequestResult.StatusCode);
    }

    [TestMethod]
    public async Task EliminarProveedor_DebeRetornarOk_CuandoTieneExito()
    {
        var expectedResponse = new ApiResponse<int> { IsSuccess = true };
        _mockService.Setup(s => s.EliminarProveedorAsync(It.IsAny<int>())).ReturnsAsync(expectedResponse);

        var result = await _controller.EliminarProveedor(1);

        var okResult = result.Result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);
    }

    [TestMethod]
    public async Task EliminarProveedor_DebeRetornarNoEncontrado_CuandoFalla()
    {
        var expectedResponse = new ApiResponse<int> { IsSuccess = false };
        _mockService.Setup(s => s.EliminarProveedorAsync(It.IsAny<int>())).ReturnsAsync(expectedResponse);

        var result = await _controller.EliminarProveedor(1);

        var notFoundResult = result.Result as NotFoundObjectResult;
        Assert.IsNotNull(notFoundResult);
        Assert.AreEqual(404, notFoundResult.StatusCode);
    }
}

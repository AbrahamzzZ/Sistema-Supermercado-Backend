using APIRestSistemaVentas.Controllers;
using Domain.Models;
using Domain.Models.Dto.Response.Sucursal;
using Infrastructure.Repository.InterfacesServices;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Utilities.Shared;

namespace UnitTests.Controller;

[TestClass]
public class TestSucursalController
{
    private Mock<ISucursalService> _mockService;
    private SucursalController _controller;

    [TestInitialize]
    public void Setup()
    {
        _mockService = new Mock<ISucursalService>();
        _controller = new SucursalController(_mockService.Object);
    }

    [TestMethod]
    public async Task ObtenerSucursales_DebeRetornarOk_ConSucursales()
    {
        var expectedResponse = new ApiResponse<List<SucursalResponse>> { IsSuccess = true, Data = new List<SucursalResponse>() };
        _mockService.Setup(s => s.ListarSucursalesAsync()).ReturnsAsync(expectedResponse);

        var result = await _controller.GetSucursales();

        var okResult = result.Result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);
        Assert.AreEqual(expectedResponse, okResult.Value);
    }

    [TestMethod]
    public async Task ObtenerSucursalesPaginadas_DebeRetornarOk_ConDatos()
    {
        var expectedResponse = new ApiResponse<Paginacion<SucursalResponse>> { IsSuccess = true, Data = new Paginacion<SucursalResponse>() };
        _mockService.Setup(s => s.ListarSucursalesPaginacionAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(expectedResponse);

        var result = await _controller.GetSucursalesPaginacion();

        var okResult = result.Result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);
        Assert.AreEqual(expectedResponse, okResult.Value);
    }

    [TestMethod]
    public async Task ObtenerSucursal_DebeRetornarOk_CuandoExiste()
    {
        var expectedResponse = new ApiResponse<SucursalResponse> { IsSuccess = true, Data = new SucursalResponse() };
        _mockService.Setup(s => s.ObtenerSucursalAsync(It.IsAny<int>())).ReturnsAsync(expectedResponse);

        var result = await _controller.GetSucursal(1);

        var okResult = result.Result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);
        Assert.AreEqual(expectedResponse, okResult.Value);
    }

    [TestMethod]
    public async Task ObtenerSucursal_DebeRetornarNoEncontrado_CuandoNoExiste()
    {
        var expectedResponse = new ApiResponse<SucursalResponse> { IsSuccess = false, Message = "Not Found" };
        _mockService.Setup(s => s.ObtenerSucursalAsync(It.IsAny<int>())).ReturnsAsync(expectedResponse);

        var result = await _controller.GetSucursal(1);

        var notFoundResult = result.Result as NotFoundObjectResult;
        Assert.IsNotNull(notFoundResult);
        Assert.AreEqual(404, notFoundResult.StatusCode);
    }

    [TestMethod]
    public async Task RegistrarSucursal_DebeRetornarOk_CuandoTieneExito()
    {
        var sucursal = new Sucursal();
        var expectedResponse = new ApiResponse<object> { IsSuccess = true };
        _mockService.Setup(s => s.RegistrarSucursalAsync(sucursal)).ReturnsAsync(expectedResponse);

        var result = await _controller.RegistrarSucursal(sucursal);

        var okResult = result.Result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);
    }

    [TestMethod]
    public async Task RegistrarSucursal_DebeRetornarSolicitudIncorrecta_CuandoFalla()
    {
        var sucursal = new Sucursal();
        var expectedResponse = new ApiResponse<object> { IsSuccess = false };
        _mockService.Setup(s => s.RegistrarSucursalAsync(sucursal)).ReturnsAsync(expectedResponse);

        var result = await _controller.RegistrarSucursal(sucursal);

        var badRequestResult = result.Result as BadRequestObjectResult;
        Assert.IsNotNull(badRequestResult);
        Assert.AreEqual(400, badRequestResult.StatusCode);
    }

    [TestMethod]
    public async Task EditarSucursal_DebeRetornarOk_CuandoTieneExito()
    {
        var sucursal = new Sucursal();
        var expectedResponse = new ApiResponse<object> { IsSuccess = true };
        _mockService.Setup(s => s.EditarSucursalAsync(sucursal)).ReturnsAsync(expectedResponse);

        var result = await _controller.EditarSucursal(1, sucursal);

        var okResult = result.Result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);
    }

    [TestMethod]
    public async Task EditarSucursal_DebeRetornarSolicitudIncorrecta_CuandoFalla()
    {
        var sucursal = new Sucursal();
        var expectedResponse = new ApiResponse<object> { IsSuccess = false };
        _mockService.Setup(s => s.EditarSucursalAsync(sucursal)).ReturnsAsync(expectedResponse);

        var result = await _controller.EditarSucursal(1, sucursal);

        var badRequestResult = result.Result as BadRequestObjectResult;
        Assert.IsNotNull(badRequestResult);
        Assert.AreEqual(400, badRequestResult.StatusCode);
    }

    [TestMethod]
    public async Task EliminarSucursal_DebeRetornarOk_CuandoTieneExito()
    {
        var expectedResponse = new ApiResponse<int> { IsSuccess = true };
        _mockService.Setup(s => s.EliminarSucursalAsync(It.IsAny<int>())).ReturnsAsync(expectedResponse);

        var result = await _controller.EliminarSucursal(1);

        var okResult = result.Result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);
    }

    [TestMethod]
    public async Task EliminarSucursal_DebeRetornarNoEncontrado_CuandoFalla()
    {
        var expectedResponse = new ApiResponse<int> { IsSuccess = false };
        _mockService.Setup(s => s.EliminarSucursalAsync(It.IsAny<int>())).ReturnsAsync(expectedResponse);

        var result = await _controller.EliminarSucursal(1);

        var notFoundResult = result.Result as NotFoundObjectResult;
        Assert.IsNotNull(notFoundResult);
        Assert.AreEqual(404, notFoundResult.StatusCode);
    }
}

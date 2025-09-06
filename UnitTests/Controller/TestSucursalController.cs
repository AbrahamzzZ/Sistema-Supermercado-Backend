using APIRestSistemaVentas.Controllers;
using DataBaseFirst.Models;
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
        //_controller = new SucursalController(_mockService.Object);
    }

    [TestMethod]
    public async Task GetSucursales_ReturnsOk_WithSucursales()
    {
        var expectedResponse = new ApiResponse<List<Sucursal>> { IsSuccess = true, Data = new List<Sucursal>() };
        _mockService.Setup(s => s.ListarSucursalesAsync()).ReturnsAsync(expectedResponse);

        var result = await _controller.GetSucursales();

        var okResult = result.Result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);
        Assert.AreEqual(expectedResponse, okResult.Value);
    }

    [TestMethod]
    public async Task GetSucursalesPaginacion_ReturnsOk_WithData()
    {
        var expectedResponse = new ApiResponse<Paginacion<Sucursal>> { IsSuccess = true, Data = new Paginacion<Sucursal>() };
        _mockService.Setup(s => s.ListarSucursalesPaginacionAsync(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(expectedResponse);

        var result = await _controller.GetSucursalesPaginacion();

        var okResult = result.Result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);
        Assert.AreEqual(expectedResponse, okResult.Value);
    }

    [TestMethod]
    public async Task GetSucursal_ReturnsOk_WhenFound()
    {
        var expectedResponse = new ApiResponse<Sucursal> { IsSuccess = true, Data = new Sucursal() };
        _mockService.Setup(s => s.ObtenerSucursalAsync(It.IsAny<int>())).ReturnsAsync(expectedResponse);

        var result = await _controller.GetSucursal(1);

        var okResult = result.Result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);
        Assert.AreEqual(expectedResponse, okResult.Value);
    }

    [TestMethod]
    public async Task GetSucursal_ReturnsNotFound_WhenNotFound()
    {
        var expectedResponse = new ApiResponse<Sucursal> { IsSuccess = false, Message = "Not Found" };
        _mockService.Setup(s => s.ObtenerSucursalAsync(It.IsAny<int>())).ReturnsAsync(expectedResponse);

        var result = await _controller.GetSucursal(1);

        var notFoundResult = result.Result as NotFoundObjectResult;
        Assert.IsNotNull(notFoundResult);
        Assert.AreEqual(404, notFoundResult.StatusCode);
    }

    [TestMethod]
    public async Task RegistrarSucursal_ReturnsOk_WhenSuccess()
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
    public async Task RegistrarSucursal_ReturnsBadRequest_WhenFail()
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
    public async Task EditarSucursal_ReturnsOk_WhenSuccess()
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
    public async Task EditarSucursal_ReturnsBadRequest_WhenFail()
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
    public async Task EliminarSucursal_ReturnsOk_WhenSuccess()
    {
        var expectedResponse = new ApiResponse<int> { IsSuccess = true };
        _mockService.Setup(s => s.EliminarSucursalAsync(It.IsAny<int>())).ReturnsAsync(expectedResponse);

        var result = await _controller.EliminarSucursal(1);

        var okResult = result.Result as OkObjectResult;
        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);
    }

    [TestMethod]
    public async Task EliminarSucursal_ReturnsNotFound_WhenFail()
    {
        var expectedResponse = new ApiResponse<int> { IsSuccess = false };
        _mockService.Setup(s => s.EliminarSucursalAsync(It.IsAny<int>())).ReturnsAsync(expectedResponse);

        var result = await _controller.EliminarSucursal(1);

        var notFoundResult = result.Result as NotFoundObjectResult;
        Assert.IsNotNull(notFoundResult);
        Assert.AreEqual(404, notFoundResult.StatusCode);
    }
}

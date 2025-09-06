using APIRestSistemaVentas.Controllers;
using DataBaseFirst.Models;
using DataBaseFirst.Models.Dto;
using Infrastructure.Repository.InterfacesServices;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Utilities.Shared;
using WebApiRest.Dto;

namespace UnitTests.Controller;

[TestClass]
public class TestNegocioController
{
    private Mock<INegocioService> _mockService;
    private NegocioController _controller;

    [TestInitialize]
    public void Setup()
    {
        _mockService = new Mock<INegocioService>();
        //_controller = new NegocioController(_mockService.Object);
    }

    [TestMethod]
    public async Task GetNegocio_ReturnsOk_WhenFound()
    {
        var response = new ApiResponse<Negocio> { IsSuccess = true, Data = new Negocio { Id_Negocio = 1, Nombre = "Mi Empresa" } };

        _mockService.Setup(s => s.ObtenerNegocioAsync(1))
                    .ReturnsAsync(response);

        var result = await _controller.GetNegocio(1);
        var okResult = result.Result as OkObjectResult;

        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);
        Assert.AreEqual(response, okResult.Value);
    }

    [TestMethod]
    public async Task GetNegocio_ReturnsNotFound_WhenNotFound()
    {
        var response = new ApiResponse<Negocio> { IsSuccess = false, Message = "No encontrado" };

        _mockService.Setup(s => s.ObtenerNegocioAsync(99))
                    .ReturnsAsync(response);

        var result = await _controller.GetNegocio(99);
        var notFoundResult = result.Result as NotFoundObjectResult;

        Assert.IsNotNull(notFoundResult);
        Assert.AreEqual(404, notFoundResult.StatusCode);
        Assert.AreEqual(response, notFoundResult.Value);
    }

    [TestMethod]
    public async Task EditarNegocio_ReturnsOk_WhenSuccess()
    {
        var request = new Negocio { Id_Negocio = 1, Nombre = "Actualizado" };
        var response = new ApiResponse<object> { IsSuccess = true, Message = "Editado correctamente" };

        _mockService.Setup(s => s.EditarNegocioAsync(request))
                    .ReturnsAsync(response);

        var result = await _controller.EditarNegocio(1, request);
        var okResult = result.Result as OkObjectResult;

        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);
        Assert.AreEqual(response, okResult.Value);
    }

    [TestMethod]
    public async Task EditarNegocio_ReturnsBadRequest_WhenFails()
    {
        var request = new Negocio { Id_Negocio = 1, Nombre = "Actualizado" };
        var response = new ApiResponse<object> { IsSuccess = false, Message = "Error al editar" };

        _mockService.Setup(s => s.EditarNegocioAsync(request))
                    .ReturnsAsync(response);

        var result = await _controller.EditarNegocio(1, request);
        var badRequestResult = result.Result as BadRequestObjectResult;

        Assert.IsNotNull(badRequestResult);
        Assert.AreEqual(400, badRequestResult.StatusCode);
        Assert.AreEqual(response, badRequestResult.Value);
    }

    [TestMethod]
    public async Task ObtenerProductosMasComprados_ReturnsOk_WhenSuccess()
    {
        var response = new ApiResponse<List<ProductoMasComprado>>
        { IsSuccess = true, Data = new List<ProductoMasComprado> { new ProductoMasComprado { Nombre_Producto = "Producto A", Cantidad_Comprada = 10 } } };

        _mockService.Setup(s => s.ObtenerProductoMasComprado())
                    .ReturnsAsync(response);

        var result = await _controller.ObtenerProductosMasComprados();
        var okResult = result.Result as OkObjectResult;

        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);
        Assert.AreEqual(response, okResult.Value);
    }

    [TestMethod]
    public async Task ObtenerProductosMasVendidos_ReturnsOk_WhenSuccess()
    {
        var response = new ApiResponse<List<ProductoMasVendido>> { IsSuccess = true, Data = new List<ProductoMasVendido>  { new ProductoMasVendido { Nombre_Producto = "Producto X", Cantidad_Vendida = 20 } } };

        _mockService.Setup(s => s.ObtenerProductoMasVendido())
                    .ReturnsAsync(response);

        var result = await _controller.ObtenerProductosMasVendidos();
        var okResult = result.Result as OkObjectResult;

        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);
        Assert.AreEqual(response, okResult.Value);
    }

    [TestMethod]
    public async Task ObtenerTopClientes_ReturnsOk_WhenSuccess()
    {
        var response = new ApiResponse<List<TopCliente>>
        { IsSuccess = true, Data = new List<TopCliente> { new TopCliente { Nombre_Completo = "Cliente A", Compras_Totales = 500 } } };

        _mockService.Setup(s => s.ObtenerTopClientes())
                    .ReturnsAsync(response);

        var result = await _controller.ObtenerTopClientes();
        var okResult = result.Result as OkObjectResult;

        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);
        Assert.AreEqual(response, okResult.Value);
    }

    [TestMethod]
    public async Task ObtenerTopProveedores_ReturnsOk_WhenSuccess()
    {
        var response = new ApiResponse<List<TopProveedor>>
        { IsSuccess = true, Data = new List<TopProveedor> {  new TopProveedor { Nombre_Completo = "Proveedor A", Compras_Totales = 300 } } };

        _mockService.Setup(s => s.ObtenerTopProveedores())
                    .ReturnsAsync(response);

        var result = await _controller.ObtenerProveedorPreferido();
        var okResult = result.Result as OkObjectResult;

        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);
        Assert.AreEqual(response, okResult.Value);
    }

    [TestMethod]
    public async Task ObtenerTransportistaViajes_ReturnsOk_WhenSuccess()
    {
        var response = new ApiResponse<List<ViajesTransportista>> { IsSuccess = true, Data = new List<ViajesTransportista> { new ViajesTransportista { Nombre_Completo = "Juan", Viajes_Realizados = 15 } } };

        _mockService.Setup(s => s.ObtenerViajesTransportista())
                    .ReturnsAsync(response);

        var result = await _controller.ObtenerTransportistaViajes();
        var okResult = result.Result as OkObjectResult;

        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);
        Assert.AreEqual(response, okResult.Value);
    }

    [TestMethod]
    public async Task ObtenerEmpleadosProductivos_ReturnsOk_WhenSuccess()
    {
        var response = new ApiResponse<List<EmpleadoProductivo>> { IsSuccess = true, Data = new List<EmpleadoProductivo> { new EmpleadoProductivo { Nombre_Completo = "Pedro", Ventas_Empleado = 50 } } };

        _mockService.Setup(s => s.ObtenerEmpleadosProductivos())
                    .ReturnsAsync(response);

        var result = await _controller.ObtenerEmpleadosProductivos();
        var okResult = result.Result as OkObjectResult;

        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);
        Assert.AreEqual(response, okResult.Value);
    }
}


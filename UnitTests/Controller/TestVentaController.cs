using APIRestSistemaVentas.Controllers;
using Domain.Models.Dto.Venta;
using Infrastructure.Repository.InterfacesServices;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Utilities.Shared;

namespace UnitTests.Controller;

[TestClass]
public class TestVentaController
{
    private Mock<IVentaService> _mockService;
    private VentaController _controller;

    [TestInitialize]
    public void Setup()
    {
        _mockService = new Mock<IVentaService>();
        //_controller = new VentaController(_mockService.Object);
    }


    [TestMethod]
    public async Task GetObtenerNumeroDocumento_ReturnsOkResult()
    {
        var response = new ApiResponse<string> { IsSuccess = true, Message = "OK", Data = "VEN-0001" };

        _mockService.Setup(s => s.ObtenerNumeroDocumentoAsync())
                    .ReturnsAsync(response);

        var actionResult = await _controller.GetObtenerNumeroDocumento();
        var okResult = actionResult as OkObjectResult;

        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);
        Assert.AreEqual(response, okResult.Value);
    }

    [TestMethod]
    public async Task GetObtenerVenta_ReturnsOkWithVenta()
    {
        var response = new ApiResponse<VentaRespuesta> { IsSuccess = true, Message = "OK", Data = new VentaRespuesta { Numero_Documento = "VEN-0001", Monto_Total = 100, Id_Cliente = 1 } };

        _mockService.Setup(s => s.ObtenerVentaAsync("VEN-0001"))
                    .ReturnsAsync(response);

        var actionResult = await _controller.GetObtenerVenta("VEN-0001");
        var okResult = actionResult.Result as OkObjectResult;

        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);
        Assert.AreEqual(response, okResult.Value);
    }

    [TestMethod]
    public async Task GetObtenerDetallesVenta_ReturnsOkWithDetalles()
    {
        var response = new ApiResponse<List<DetalleVentasRepuesta>>
        {
            IsSuccess = true,
            Message = "OK",
            Data = new List<DetalleVentasRepuesta> { new DetalleVentasRepuesta { Id_Producto = 1, Cantidad = 2, Precio_Venta = 10 } }
        };

        _mockService.Setup(s => s.ObtenerDetallesVentaAsync(1))
                    .ReturnsAsync(response);

        var actionResult = await _controller.GetObtenerDetallesVenta(1);
        var okResult = actionResult.Result as OkObjectResult;

        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);
        Assert.AreEqual(response, okResult.Value);
    }

    [TestMethod]
    public async Task PostRegistrarVenta_ReturnsOkWhenSuccess()
    {
        var ventaDto = new Ventas { Numero_Documento = "VEN-0002", Monto_Total = 200 };
        var response = new ApiResponse<object> { IsSuccess = true, Message = "Venta registrada correctamente" };

        _mockService.Setup(s => s.RegistrarVentaAsync(ventaDto))
                    .ReturnsAsync(response);

        var actionResult = await _controller.PostRegistrarVenta(ventaDto);
        var okResult = actionResult as OkObjectResult;

        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);
        Assert.AreEqual(response, okResult.Value);
    }

    [TestMethod]
    public async Task PostRegistrarVenta_ReturnsBadRequestWhenFails()
    {
        var ventaDto = new Ventas { Numero_Documento = "VEN-0003", Monto_Total = 300 };
        var response = new ApiResponse<object> { IsSuccess = false, Message = "Error al registrar venta" };

        _mockService.Setup(s => s.RegistrarVentaAsync(ventaDto))
                    .ReturnsAsync(response);

        var actionResult = await _controller.PostRegistrarVenta(ventaDto);
        var badResult = actionResult as BadRequestObjectResult;

        Assert.IsNotNull(badResult);
        Assert.AreEqual(400, badResult.StatusCode);
        Assert.AreEqual(response, badResult.Value);
    }
}

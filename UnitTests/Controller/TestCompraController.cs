using APIRestSistemaVentas.Controllers;
using DataBaseFirst.Models.Dto;
using DataBaseFirst.Repository.InterfacesServices;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Utilities.Shared;

namespace UnitTests.Controller;

[TestClass]
public class TestCompraController
{
    private Mock<ICompraService> _mockService;
    private CompraController _controller;

    [TestInitialize]
    public void Setup()
    {
        _mockService = new Mock<ICompraService>();
        //_controller = new CompraController(_mockService.Object);
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
    public async Task GetObtenerCompra_ReturnsOkWithVenta()
    {
        var response = new ApiResponse<CompraRespuesta> { IsSuccess = true, Message = "OK", Data = new CompraRespuesta { Numero_Documento = "VEN-0001", Monto_Total = 100, Id_Proveedor = 1 } };

        _mockService.Setup(s => s.ObtenerCompraAsync("VEN-0001"))
                    .ReturnsAsync(response);

        var actionResult = await _controller.GetObtenerCompra("VEN-0001");
        var okResult = actionResult.Result as OkObjectResult;

        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);
        Assert.AreEqual(response, okResult.Value);
    }

    [TestMethod]
    public async Task GetObtenerDetallesCompra_ReturnsOkWithDetalles()
    {
        var response = new ApiResponse<List<DetalleComprasRepuesta>>
        {
            IsSuccess = true,
            Message = "OK",
            Data = new List<DetalleComprasRepuesta> { new DetalleComprasRepuesta { Id_Producto = 1, Cantidad = 2, Precio_Venta = 10 } }
        };

        _mockService.Setup(s => s.ObtenerDetallesCompraAsync(1))
                    .ReturnsAsync(response);

        var actionResult = await _controller.GetObtenerDetallesCompra(1);
        var okResult = actionResult.Result as OkObjectResult;

        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);
        Assert.AreEqual(response, okResult.Value);
    }

    [TestMethod]
    public async Task PostRegistrarCompra_ReturnsOkWhenSuccess()
    {
        var compraDto = new Compras { Numero_Documento = "VEN-0002", Monto_Total = 200 };
        var response = new ApiResponse<object> { IsSuccess = true, Message = "Compra registrada correctamente" };

        _mockService.Setup(s => s.RegistrarCompraAsync(compraDto))
                    .ReturnsAsync(response);

        var actionResult = await _controller.PostRegistrarCompra(compraDto);
        var okResult = actionResult as OkObjectResult;

        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);
        Assert.AreEqual(response, okResult.Value);
    }

    [TestMethod]
    public async Task PostRegistrarVenta_ReturnsBadRequestWhenFails()
    {
        var compraDto = new Compras { Numero_Documento = "VEN-0003", Monto_Total = 300 };
        var response = new ApiResponse<object> { IsSuccess = false, Message = "Error al registrar compra" };

        _mockService.Setup(s => s.RegistrarCompraAsync(compraDto))
                    .ReturnsAsync(response);

        var actionResult = await _controller.PostRegistrarCompra(compraDto);
        var badResult = actionResult as BadRequestObjectResult;

        Assert.IsNotNull(badResult);
        Assert.AreEqual(400, badResult.StatusCode);
        Assert.AreEqual(response, badResult.Value);
    }
}

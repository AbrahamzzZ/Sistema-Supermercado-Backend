using APIRestSistemaVentas.Controllers;
using DataBaseFirst.Models;
using Infrastructure.Repository.InterfacesServices;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Utilities.Shared;

namespace UnitTests.Controller;

[TestClass]
public class TestTranportistaController
{
    private Mock<ITransportistaService> _mockService;
    private TransportistaController _controller;

    [TestInitialize]
    public void Setup()
    {
        _mockService = new Mock<ITransportistaService>();
        //_controller = new TransportistaController(_mockService.Object);
    }


    [TestMethod]
    public async Task GetTransportistas_ReturnsOkWithList()
    {
        var response = new ApiResponse<List<Transportistum>>
        {
            IsSuccess = true,
            Data = new List<Transportistum> { new Transportistum { Id_Transportista = 1, Nombres = "Juan Perez" } }
        };

        _mockService.Setup(s => s.ListarTransportistasAsync())
                    .ReturnsAsync(response);

        var actionResult = await _controller.GetTransportistas();
        var okResult = actionResult.Result as OkObjectResult;

        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);
        Assert.AreEqual(response, okResult.Value);
    }


    [TestMethod]
    public async Task GetTransportista_ReturnsOk_WhenFound()
    {
        var response = new ApiResponse<Transportistum> { IsSuccess = true, Data = new Transportistum { Id_Transportista = 1, Nombres = "Juan Perez" } };

        _mockService.Setup(s => s.ObtenerTransportistaAsync(1))
                    .ReturnsAsync(response);

        var actionResult = await _controller.GetTransportista(1);
        var okResult = actionResult.Result as OkObjectResult;

        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);
        Assert.AreEqual(response, okResult.Value);
    }

    [TestMethod]
    public async Task GetTransportista_ReturnsNotFound_WhenNotFound()
    {
        var response = new ApiResponse<Transportistum> { IsSuccess = false, Message = "No encontrado" };

        _mockService.Setup(s => s.ObtenerTransportistaAsync(99))
                    .ReturnsAsync(response);

        var actionResult = await _controller.GetTransportista(99);
        var notFoundResult = actionResult.Result as NotFoundObjectResult;

        Assert.IsNotNull(notFoundResult);
        Assert.AreEqual(404, notFoundResult.StatusCode);
        Assert.AreEqual(response, notFoundResult.Value);
    }

    [TestMethod]
    public async Task RegistrarProveedor_ReturnsOk_WhenSuccess()
    {
        var request = new Transportistum { Id_Transportista = 1, Nombres = "Nuevo Transportista" };
        var response = new ApiResponse<object> { IsSuccess = true, Message = "Registrado correctamente" };

        _mockService.Setup(s => s.RegistrarTransportistaAsync(request))
                    .ReturnsAsync(response);

        var actionResult = await _controller.RegistrarProveedor(request);
        var okResult = actionResult.Result as OkObjectResult;

        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);
        Assert.AreEqual(response, okResult.Value);
    }

    [TestMethod]
    public async Task RegistrarProveedor_ReturnsBadRequest_WhenFails()
    {
        var request = new Transportistum { Id_Transportista = 1, Nombres = "Nuevo Transportista" };
        var response = new ApiResponse<object> { IsSuccess = false, Message = "Error al registrar" };

        _mockService.Setup(s => s.RegistrarTransportistaAsync(request))
                    .ReturnsAsync(response);

        var actionResult = await _controller.RegistrarProveedor(request);
        var badRequestResult = actionResult.Result as BadRequestObjectResult;

        Assert.IsNotNull(badRequestResult);
        Assert.AreEqual(400, badRequestResult.StatusCode);
        Assert.AreEqual(response, badRequestResult.Value);
    }

    [TestMethod]
    public async Task EditarTransportista_ReturnsOk_WhenSuccess()
    {
        var request = new Transportistum { Id_Transportista = 1, Nombres = "Editado" };
        var response = new ApiResponse<object> { IsSuccess = true, Message = "Editado correctamente" };

        _mockService.Setup(s => s.EditarTransportistaAsync(request))
                    .ReturnsAsync(response);

        var actionResult = await _controller.EditarTransportista(1, request);
        var okResult = actionResult.Result as OkObjectResult;

        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);
        Assert.AreEqual(response, okResult.Value);
    }

    [TestMethod]
    public async Task EditarTransportista_ReturnsBadRequest_WhenFails()
    {
        var request = new Transportistum { Id_Transportista = 1, Nombres = "Editado" };
        var response = new ApiResponse<object> { IsSuccess = false, Message = "Error al editar" };

        _mockService.Setup(s => s.EditarTransportistaAsync(request))
                    .ReturnsAsync(response);

        var actionResult = await _controller.EditarTransportista(1, request);
        var badRequestResult = actionResult.Result as BadRequestObjectResult;

        Assert.IsNotNull(badRequestResult);
        Assert.AreEqual(400, badRequestResult.StatusCode);
        Assert.AreEqual(response, badRequestResult.Value);
    }

    [TestMethod]
    public async Task EliminarTransportista_ReturnsOk_WhenSuccess()
    {
        var response = new ApiResponse<int> { IsSuccess = true, Data = 1, Message = "Eliminado correctamente" };

        _mockService.Setup(s => s.EliminarTransportistaAsync(1))
                    .ReturnsAsync(response);

        var actionResult = await _controller.EliminarTransportista(1);
        var okResult = actionResult.Result as OkObjectResult;

        Assert.IsNotNull(okResult);
        Assert.AreEqual(200, okResult.StatusCode);
        Assert.AreEqual(response, okResult.Value);
    }

    [TestMethod]
    public async Task EliminarTransportista_ReturnsNotFound_WhenFails()
    {
        var response = new ApiResponse<int> { IsSuccess = false, Message = "No se pudo eliminar" };

        _mockService.Setup(s => s.EliminarTransportistaAsync(1))
                    .ReturnsAsync(response);

        var actionResult = await _controller.EliminarTransportista(1);
        var notFoundResult = actionResult.Result as NotFoundObjectResult;

        Assert.IsNotNull(notFoundResult);
        Assert.AreEqual(404, notFoundResult.StatusCode);
        Assert.AreEqual(response, notFoundResult.Value);
    }
}


using APIRestSistemaVentas.Controllers;
using DataBaseFirst.Models;
using DataBaseFirst.Models.Dto;
using DataBaseFirst.Repository.InterfacesServices;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Utilities.Shared;

namespace UnitTests.Controller;

[TestClass]
public class TestOfertaController
{
    private Mock<IOfertaService> _mockService;
    private OfertaController _controller;

    [TestInitialize]
    public void Setup()
    {
        _mockService = new Mock<IOfertaService>();
        //_controller = new OfertaController(_mockService.Object);
    }

    [TestMethod]
    public async Task GetOfertas_DeberiaRetornarOk()
    {
        _mockService.Setup(s => s.ListarOfertasAsync())
            .ReturnsAsync(new ApiResponse<List<OfertaProducto>>
            { IsSuccess = true,Data = new List<OfertaProducto> { new OfertaProducto { Id_Oferta = 1, Nombre_Oferta = "Descuento Snacks" } } });

        var result = await _controller.GetOfertas();

        var okResult = result.Result as OkObjectResult;
        Assert.IsNotNull(okResult);
        var response = okResult.Value as ApiResponse<List<OfertaProducto>>;
        Assert.IsTrue(response.IsSuccess);
        Assert.AreEqual(1, response.Data.Count);
    }

    [TestMethod]
    public async Task GetOfertasPaginacion_DeberiaRetornarOk()
    {
        var paginacion = new Paginacion<OfertaProducto>
        {
            Items = new List<OfertaProducto> { new OfertaProducto { Id_Oferta = 1, Nombre_Oferta = "Oferta Especial" } }, TotalCount = 1
        };

        _mockService.Setup(s => s.ListarOfertasPaginacionAsync(1, 10))
            .ReturnsAsync(new ApiResponse<Paginacion<OfertaProducto>> { IsSuccess = true, Data = paginacion });

        var result = await _controller.GetOfertasPaginacion(1, 10);

        var okResult = result.Result as OkObjectResult;
        Assert.IsNotNull(okResult);
        var response = okResult.Value as ApiResponse<Paginacion<OfertaProducto>>;
        Assert.IsTrue(response.IsSuccess);
        Assert.AreEqual(1, response.Data.Items.Count);
    }

    [TestMethod]
    public async Task GetOferta_Existe_DeberiaRetornarOk()
    {
        var oferta = new Ofertum { Id_Oferta = 1, Nombre_Oferta = "Descuento 50%" };
        _mockService.Setup(s => s.ObtenerOfertaAsync(1))
            .ReturnsAsync(new ApiResponse<Ofertum> { IsSuccess = true, Data = oferta });

        var result = await _controller.GetOferta(1);

        var okResult = result.Result as OkObjectResult;
        Assert.IsNotNull(okResult);
        var response = okResult.Value as ApiResponse<Ofertum>;
        Assert.IsTrue(response.IsSuccess);
        Assert.AreEqual("Descuento 50%", response.Data.Nombre_Oferta);
    }

    [TestMethod]
    public async Task GetOferta_NoExiste_DeberiaRetornarNotFound()
    {
        _mockService.Setup(s => s.ObtenerOfertaAsync(99))
            .ReturnsAsync(new ApiResponse<Ofertum> { IsSuccess = false, Message = "No se encontró la oferta" });

        var result = await _controller.GetOferta(99);

        var notFoundResult = result.Result as NotFoundObjectResult;
        Assert.IsNotNull(notFoundResult);
        var response = notFoundResult.Value as ApiResponse<Ofertum>;
        Assert.IsFalse(response.IsSuccess);
        Assert.AreEqual("No se encontró la oferta", response.Message);
    }

    [TestMethod]
    public async Task RegistrarOferta_Valida_DeberiaRetornarOk()
    {
        var oferta = new Ofertum { Id_Oferta = 1, Nombre_Oferta = "Promo Verano" };
        _mockService.Setup(s => s.RegistrarOfertaAsync(oferta))
            .ReturnsAsync(new ApiResponse<object> { IsSuccess = true, Message = "Registrado correctamente" });

        var result = await _controller.RegistrarOferta(oferta);

        var okResult = result.Result as OkObjectResult;
        Assert.IsNotNull(okResult);
        var response = okResult.Value as ApiResponse<object>;
        Assert.IsTrue(response.IsSuccess);
        Assert.AreEqual("Registrado correctamente", response.Message);
    }

    [TestMethod]
    public async Task RegistrarOferta_Invalida_DeberiaRetornarBadRequest()
    {
        var oferta = new Ofertum { Id_Oferta = 1, Nombre_Oferta = "" }; 
        _mockService.Setup(s => s.RegistrarOfertaAsync(oferta))
            .ReturnsAsync(new ApiResponse<object> { IsSuccess = false,  Message = "Datos incompletos" });

        var result = await _controller.RegistrarOferta(oferta);

        var badRequestResult = result.Result as BadRequestObjectResult;
        Assert.IsNotNull(badRequestResult);
        var response = badRequestResult.Value as ApiResponse<object>;
        Assert.IsFalse(response.IsSuccess);
        Assert.AreEqual("Datos incompletos", response.Message);
    }

    [TestMethod]
    public async Task EditarOferta_DeberiaRetornarOk()
    {
        var oferta = new Ofertum { Id_Oferta = 1, Nombre_Oferta = "Promo Actualizada" };
        _mockService.Setup(s => s.EditarOfertaAsync(oferta))
            .ReturnsAsync(new ApiResponse<object> { IsSuccess = true, Message = "Actualizado correctamente" });

        var result = await _controller.EditarOferta(1, oferta);

        var okResult = result.Result as OkObjectResult;
        Assert.IsNotNull(okResult);
        var response = okResult.Value as ApiResponse<object>;
        Assert.IsTrue(response.IsSuccess);
    }

    [TestMethod]
    public async Task EditarOferta_DeberiaRetornarBadRequest_SiInvalido()
    {
        var oferta = new Ofertum { Id_Oferta = 1, Nombre_Oferta = "" };

        _mockService.Setup(s => s.EditarOfertaAsync(oferta))
            .ReturnsAsync(new ApiResponse<object> { IsSuccess = false, Message = Mensajes.MESSAGE_EMPTY });

        var result = await _controller.EditarOferta(1, oferta);

        Assert.IsInstanceOfType(result.Result, typeof(BadRequestObjectResult));
    }

    [TestMethod]
    public async Task EliminarOferta_DeberiaRetornarOk()
    {
        _mockService.Setup(s => s.EliminarOfertaAsync(1))
            .ReturnsAsync(new ApiResponse<int> { IsSuccess = true, Message = "Eliminado correctamente" });

        var result = await _controller.EliminarOferta(1);

        var okResult = result.Result as OkObjectResult;
        Assert.IsNotNull(okResult);
        var response = okResult.Value as ApiResponse<int>;
        Assert.IsTrue(response.IsSuccess);
        Assert.AreEqual("Eliminado correctamente", response.Message);
    }
}

using Domain.Models;
using Domain.Models.Dto.Response.Oferta;
using FluentValidation;
using FluentValidation.Results;
using Infrastructure.Repository.InterfacesRepository;
using Infrastructure.Repository.InterfacesBusiness;
using Infrastructure.Repository.InterfacesServices;
using Infrastructure.Services;
using Moq;
using Utilities.Shared;

namespace UnitTests.Services;

[TestClass]
public class TestOfertaService
{
    private Mock<IOfertaRepository> _mockRepository;
    private Mock<IValidator<Ofertum>> _mockValidator;
    private Mock<ICurrentUser> _mockCurrentUser;
    private Mock<IAuditoriaService> _mockAuditoria;
    private OfertaService _service;

    [TestInitialize]
    public void Setup()
    {
        _mockRepository = new Mock<IOfertaRepository>();
        _mockValidator = new Mock<IValidator<Ofertum>>();
        _mockCurrentUser = new Mock<ICurrentUser>();
        _mockCurrentUser.Setup(user => user.GetUserId()).Returns(1);
        _mockAuditoria = new Mock<IAuditoriaService>();
        _service = new OfertaService(
            _mockRepository.Object,
            _mockValidator.Object,
            _mockCurrentUser.Object,
            _mockAuditoria.Object);
    }

    [TestMethod]
    public async Task ListarOfertasAsync_DebeRetornarExito_CuandoExistenDatos()
    {
        var ofertas = new List<OfertaProductoResponse> { new OfertaProductoResponse { Id_Oferta = 1, Nombre_Oferta = "Oferta A" } };
        _mockRepository.Setup(r => r.ListarOfertasAsync()).ReturnsAsync(ofertas);
        var result = await _service.ListarOfertasAsync();

        Assert.IsTrue(result.IsSuccess);
        Assert.AreEqual(Mensajes.MESSAGE_QUERY, result.Message);
        Assert.AreEqual(1, result.Data.Count);
    }

    [TestMethod]
    public async Task ListarOfertasAsync_DebeRetornarVacio_CuandoNoExistenDatos()
    {
        _mockRepository.Setup(r => r.ListarOfertasAsync()).ReturnsAsync(new List<OfertaProductoResponse>());
        var result = await _service.ListarOfertasAsync();

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual(Mensajes.MESSAGE_QUERY_EMPTY, result.Message);
    }

    [TestMethod]
    public async Task ObtenerOfertaAsync_DebeRetornarExito_CuandoExiste()
    {
        var oferta = new OfertaProductoResponse { Id_Oferta = 1, Nombre_Oferta = "Oferta A" };
        _mockRepository.Setup(r => r.ObtenerOfertaAsync(1)).ReturnsAsync(oferta);
        var result = await _service.ObtenerOfertaAsync(1);

        Assert.IsTrue(result.IsSuccess);
        Assert.AreEqual(oferta.Nombre_Oferta, result.Data.Nombre_Oferta);
    }

    [TestMethod]
    public async Task ObtenerOfertaAsync_DebeRetornarNoEncontrado_CuandoNoExiste()
    {
        _mockRepository.Setup(r => r.ObtenerOfertaAsync(99)).ReturnsAsync((OfertaProductoResponse)null);
        var result = await _service.ObtenerOfertaAsync(99);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual(Mensajes.MESSAGE_QUERY_NOT_FOUND, result.Message);
    }

    [TestMethod]
    public async Task RegistrarOfertaAsync_DebeRetornarError_CuandoEsNula()
    {
        var result = await _service.RegistrarOfertaAsync(null);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual(Mensajes.MESSAGE_NULL, result.Message);
    }

    [TestMethod]
    public async Task RegistrarOferta_DeberiaFallar_SiCamposObligatoriosVacios()
    {
        var oferta = new Ofertum { Nombre_Oferta = "", Descripcion = "", Descuento = 0, Id_Producto = 0, Fecha_Inicio = DateOnly.FromDateTime(DateTime.Now.AddDays(10)) };
        _mockValidator.Setup(v => v.ValidateAsync(It.IsAny<Ofertum>(), default)).ReturnsAsync(new ValidationResult(new List<ValidationFailure> { new ValidationFailure("Codigo", Mensajes.MESSAGE_EMPTY) }));
        var result = await _service.RegistrarOfertaAsync(oferta);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual(Mensajes.MESSAGE_EMPTY, result.Message);
    }

    [TestMethod]
    public async Task RegistrarOferta_DeberiaFallar_SiDescuentoEsInvalido()
    {
        var oferta = new Ofertum { Id_Producto = 1, Codigo = "OF001", Nombre_Oferta = "Oferta Test", Descripcion = "Descripci�n", Descuento = 150, Fecha_Fin = DateOnly.FromDateTime(DateTime.Now.AddDays(10)) };
        _mockValidator.Setup(v => v.ValidateAsync(It.IsAny<Ofertum>(), default)).ReturnsAsync(new ValidationResult(new List<ValidationFailure> { new ValidationFailure("Descuento", "El descuento debe estar entre 0 y 100") }));
        var result = await _service.RegistrarOfertaAsync(oferta);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual("El descuento debe estar entre 0 y 100", result.Message);
    }

    [TestMethod]
    public async Task RegistrarOferta_DeberiaFallar_SiFechaFinNoEsFutura()
    {
        var oferta = new Ofertum { Id_Producto = 1, Codigo = "OF001",  Nombre_Oferta = "Oferta Test", Descripcion = "Descripci�n", Descuento = 10, Fecha_Fin = DateOnly.FromDateTime(DateTime.Now.AddDays(-1)) };
        _mockValidator.Setup(v => v.ValidateAsync(It.IsAny<Ofertum>(), default)).ReturnsAsync(new ValidationResult(new List<ValidationFailure>{ new ValidationFailure("Fecha_Fin", "La fecha de fin debe ser una fecha futura") }));
        var result = await _service.RegistrarOfertaAsync(oferta);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual("La fecha de fin debe ser una fecha futura", result.Message);
    }

    [TestMethod]
    public async Task RegistrarOferta_DeberiaSerExitoso()
    {
        var oferta = new Ofertum { Id_Producto = 1, Codigo = "OF001", Nombre_Oferta = "Oferta Test", Descripcion = "Descripci�n", Descuento = 20, Fecha_Fin = DateOnly.FromDateTime(DateTime.Now.AddDays(5)) };
        _mockValidator.Setup(v => v.ValidateAsync(It.IsAny<Ofertum>(), default)).ReturnsAsync(new ValidationResult());
        _mockRepository.Setup(r => r.ListarOfertasAsync()).ReturnsAsync(new List<OfertaProductoResponse>());
        _mockRepository.Setup(r => r.RegistrarOfertaAsync(oferta, 1)).ReturnsAsync(1);
        var result = await _service.RegistrarOfertaAsync(oferta);

        Assert.IsTrue(result.IsSuccess);
        Assert.AreEqual(Mensajes.MESSAGE_REGISTER, result.Message);
    }

    [TestMethod]
    public async Task EditarOferta_DeberiaFallar_SiOfertaNoExiste()
    {
        var oferta = new Ofertum { Id_Oferta = 1, Id_Producto = 1, Nombre_Oferta = "Oferta Editar", Descripcion = "Desc", Descuento = 10, Fecha_Inicio = DateOnly.FromDateTime(DateTime.Now.AddDays(5)), Fecha_Fin = DateOnly.FromDateTime(DateTime.Now.AddDays(5)) };
        _mockValidator.Setup(v => v.ValidateAsync(It.IsAny<Ofertum>(), default)).ReturnsAsync(new ValidationResult());
        _mockRepository.Setup(r => r.ObtenerOfertaAsync(1)).ReturnsAsync((OfertaProductoResponse)null);
        var result = await _service.EditarOfertaAsync(oferta);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual(Mensajes.MESSAGE_QUERY_NOT_FOUND, result.Message);
    }

    [TestMethod]
    public async Task EditarOferta_DeberiaSerExitoso()
    {
        var oferta = new Ofertum { Id_Oferta = 1, Id_Producto = 1, Nombre_Oferta = "Oferta Editada", Descripcion = "Desc", Descuento = 15, Fecha_Fin = DateOnly.FromDateTime(DateTime.Now.AddDays(10)) };
        _mockValidator.Setup(v => v.ValidateAsync(It.IsAny<Ofertum>(), default)).ReturnsAsync(new ValidationResult());
        _mockRepository.Setup(r => r.ObtenerOfertaAsync(1)).ReturnsAsync(new OfertaProductoResponse());
        _mockRepository.Setup(r => r.ListarOfertasAsync()).ReturnsAsync(new List<OfertaProductoResponse>());
        _mockRepository.Setup(r => r.EditarOfertaAsync(oferta, 1)).ReturnsAsync(1);
        var result = await _service.EditarOfertaAsync(oferta);

        Assert.IsTrue(result.IsSuccess);
        Assert.AreEqual(Mensajes.MESSAGE_UPDATE, result.Message);
    }

    [TestMethod]
    public async Task EliminarOfertaAsync_DebeRetornarNoEncontrado_CuandoNoExiste()
    {
        _mockRepository.Setup(r => r.ObtenerOfertaAsync(99)).ReturnsAsync((OfertaProductoResponse)null);
        var result = await _service.EliminarOfertaAsync(99);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual(Mensajes.MESSAGE_QUERY_NOT_FOUND, result.Message);
    }

    [TestMethod]
    public async Task EliminarOfertaAsync_DebeRetornarExito_CuandoSeElimina()
    {
        var oferta = new OfertaProductoResponse { Id_Oferta = 1, Nombre_Oferta = "Oferta A" };
        _mockRepository.Setup(r => r.ObtenerOfertaAsync(1)).ReturnsAsync(oferta);
        _mockRepository.Setup(r => r.EliminarOfertaAsync(1)).ReturnsAsync(1);
        var result = await _service.EliminarOfertaAsync(1);

        Assert.IsTrue(result.IsSuccess);
        Assert.AreEqual(Mensajes.MESSAGE_DELETE, result.Message);
    }

    [TestMethod]
    public async Task ListarOfertas_DebePropagarExcepcion_YRegistrarError()
    {
        var excepcion = new InvalidOperationException("Error de repositorio");
        _mockRepository.Setup(r => r.ListarOfertasAsync()).ThrowsAsync(excepcion);

        await Assert.ThrowsExceptionAsync<InvalidOperationException>(() => _service.ListarOfertasAsync());

        _mockAuditoria.Verify(a => a.RegistrarErrorAsync("Listar Ofertas", excepcion), Times.Once);
    }
}

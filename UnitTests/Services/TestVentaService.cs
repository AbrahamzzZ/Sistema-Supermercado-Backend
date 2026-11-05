using Domain.Models.Dto.Response.Venta;
using FluentValidation;
using FluentValidation.Results;
using Infrastructure.Repository.InterfacesBusiness;
using Infrastructure.Repository.InterfacesRepository;
using Infrastructure.Repository.InterfacesServices;
using Infrastructure.Services;
using Moq;
using Utilities.Shared;

namespace UnitTests.Services;

[TestClass]
public class TestVentaService
{
    private Mock<IVentaRepository> _mockRepository;
    private Mock<IValidator<Ventas>> _mockValidator;
    private Mock<IAuditoriaService> _mockAuditoria;
    private Mock<ICurrentUser> _mockCurrentUser;
    private VentaService _service;

    [TestInitialize]
    public void Setup()
    {
        _mockRepository = new Mock<IVentaRepository>();
        _mockValidator = new Mock<IValidator<Ventas>>();
        _mockAuditoria = new Mock<IAuditoriaService>();
        _mockCurrentUser = new Mock<ICurrentUser>();
        _mockCurrentUser.Setup(u => u.GetUserId()).Returns(7);
        _service = new VentaService(
            _mockRepository.Object,
            _mockValidator.Object,
            _mockAuditoria.Object,
            _mockCurrentUser.Object);
    }

    private static Ventas VentaValida() => new()
    {
        Id_Sucursal = 1,
        Id_Cliente = 1,
        Tipo_Documento = "Factura",
        Monto_Pago = 150,
        Detalles = new List<DetalleVentas> { new DetalleVentas { Id_Producto = 3, Cantidad = 2 } }
    };

    [TestMethod]
    public async Task ObtenerNumeroDocumentoAsync_DebeRetornarNumero()
    { 
        var numeroEsperado = "DOC-001";
        _mockRepository.Setup(r => r.ObtenerNumeroDocumentoAsync()).ReturnsAsync(numeroEsperado);
        var result = await _service.ObtenerNumeroDocumentoAsync();

        Assert.IsTrue(result.IsSuccess);
        Assert.AreEqual(numeroEsperado, result.Data);
        Assert.AreEqual("Número de documento generado correctamente.", result.Message);
    }

    [TestMethod]
    public async Task ObtenerVentaAsync_SiNoExisteDebeRetornarError()
    {
        _mockRepository.Setup(r => r.ObtenerVentaAsync("DOC-002")).ReturnsAsync((VentaResponse)null);
        var result = await _service.ObtenerVentaAsync("DOC-002");

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual(Mensajes.MESSAGE_QUERY_EMPTY, result.Message);
    }

    [TestMethod]
    public async Task ObtenerVentaAsync_SiExisteDebeRetornarVenta()
    {
        var venta = new VentaResponse { Id_Venta = 1, Numero_Documento = "DOC-003" };
        _mockRepository.Setup(r => r.ObtenerVentaAsync("DOC-003")).ReturnsAsync(venta);
        var result = await _service.ObtenerVentaAsync("DOC-003");

        Assert.IsTrue(result.IsSuccess);
        Assert.AreEqual(venta, result.Data);
    }

    [TestMethod]
    public async Task RegistrarVentaAsync_SiSolicitudEsNulaDebeRetornarError()
    {
        var result = await _service.RegistrarVentaAsync(null);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual(Mensajes.MESSAGE_NULL, result.Message);
    }

    [TestMethod]
    public async Task RegistrarVentaAsync_SiCamposInvalidosDebeRetornarError()
    {
        var venta = new Ventas { Id_Sucursal = 0, Id_Cliente = 0, Tipo_Documento = "", Monto_Pago = 0, Detalles = null };
        _mockValidator.Setup(v => v.ValidateAsync(It.IsAny<Ventas>(), default)).ReturnsAsync(new ValidationResult(new List<ValidationFailure> { new ValidationFailure("Tipo Documento", Mensajes.MESSAGE_EMPTY) }));
        var result = await _service.RegistrarVentaAsync(venta);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual(Mensajes.MESSAGE_EMPTY, result.Message);
    }

    [TestMethod]
    public async Task RegistrarVentaAsync_SiTipoDocumentoInvalidoDebeRetornarError()
    {
        var venta = VentaValida();
        venta.Tipo_Documento = "DOC123";
        _mockValidator.Setup(v => v.ValidateAsync(It.IsAny<Ventas>(), default)).ReturnsAsync(new ValidationResult(new List<ValidationFailure> { new ValidationFailure("Tipo Documento", "El tipo de documento solo pueden contener letras.") }));
        var result = await _service.RegistrarVentaAsync(venta);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual("El tipo de documento solo pueden contener letras.", result.Message);
    }

    [TestMethod]
    public async Task RegistrarVentaAsync_SiVentaValidaDebeRegistrar()
    {
        var venta = VentaValida();
        var calculada = new VentaRegistradaResponse { Numero_Documento = "00001", Monto_Total = 36, Descuento = 4, Monto_Pago = 150, Monto_Cambio = 114 };
        _mockValidator.Setup(v => v.ValidateAsync(It.IsAny<Ventas>(), default)).ReturnsAsync(new ValidationResult());
        _mockRepository.Setup(r => r.RegistrarVentaAsync(venta, 7)).ReturnsAsync(calculada);
        var result = await _service.RegistrarVentaAsync(venta);

        Assert.IsTrue(result.IsSuccess);
        Assert.AreEqual(Mensajes.MESSAGE_REGISTER, result.Message);
        Assert.AreEqual(calculada, result.Data);
    }

    [TestMethod]
    public async Task RegistrarVentaAsync_DebeUsarElUsuarioDelToken()
    {
        var venta = VentaValida();
        _mockValidator.Setup(v => v.ValidateAsync(It.IsAny<Ventas>(), default)).ReturnsAsync(new ValidationResult());
        _mockRepository.Setup(r => r.RegistrarVentaAsync(It.IsAny<Ventas>(), It.IsAny<int>())).ReturnsAsync(new VentaRegistradaResponse());

        await _service.RegistrarVentaAsync(venta);

        _mockRepository.Verify(r => r.RegistrarVentaAsync(venta, 7), Times.Once);
    }

    [TestMethod]
    public async Task RegistrarVentaAsync_SiNoHayUsuarioEnTokenDebeRetornarError()
    {
        _mockCurrentUser.Setup(u => u.GetUserId()).Returns(0);
        _mockValidator.Setup(v => v.ValidateAsync(It.IsAny<Ventas>(), default)).ReturnsAsync(new ValidationResult());

        var result = await _service.RegistrarVentaAsync(VentaValida());

        Assert.IsFalse(result.IsSuccess);
        _mockRepository.Verify(r => r.RegistrarVentaAsync(It.IsAny<Ventas>(), It.IsAny<int>()), Times.Never);
    }

    [TestMethod]
    public async Task RegistrarVentaAsync_SiIncumpleReglaDeNegocioDebeRetornarSuMensaje()
    {
        const string mensaje = "Stock insuficiente: Leche (disponible 1, solicitado 3)";
        _mockValidator.Setup(v => v.ValidateAsync(It.IsAny<Ventas>(), default)).ReturnsAsync(new ValidationResult());
        _mockRepository.Setup(r => r.RegistrarVentaAsync(It.IsAny<Ventas>(), 7)).ThrowsAsync(new ReglaNegocioException(mensaje));

        var result = await _service.RegistrarVentaAsync(VentaValida());

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual(mensaje, result.Message);
        _mockAuditoria.Verify(a => a.RegistrarFalloAsync("Registrar Venta", mensaje), Times.Once);
        _mockAuditoria.Verify(a => a.RegistrarErrorAsync(It.IsAny<string>(), It.IsAny<Exception>()), Times.Never);
    }

    [TestMethod]
    public async Task ObtenerVenta_DebePropagarExcepcion_YRegistrarError()
    {
        var excepcion = new InvalidOperationException("Error de repositorio");
        _mockRepository.Setup(r => r.ObtenerVentaAsync("DOC-500")).ThrowsAsync(excepcion);

        await Assert.ThrowsExceptionAsync<InvalidOperationException>(() => _service.ObtenerVentaAsync("DOC-500"));

        _mockAuditoria.Verify(a => a.RegistrarErrorAsync("Obtener Venta", excepcion), Times.Once);
    }
}

using APIRestSistemaVentas.Controllers;
using Domain.Models;
using Infrastructure.Repository.InterfacesRepository;
using Infrastructure.Repository.InterfacesServices;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Utilities.Shared;

namespace UnitTests.Services;

[TestClass]
public class TestLogService
{
    private Mock<ILogRepository> _mockRepository;
    private LogService _service;

    [TestInitialize]
    public void Setup()
    {
        _mockRepository = new Mock<ILogRepository>();
        _service = new LogService(_mockRepository.Object);
    }

    [TestMethod]
    public async Task ListarLogsPaginacion_DebePasarElRangoDeFechasAlRepositorio()
    {
        var desde = new DateOnly(2026, 10, 1);
        var hasta = new DateOnly(2026, 10, 31);
        _mockRepository.Setup(r => r.ListarLogsPaginacionAsync(1, 10, "", desde, hasta))
            .ReturnsAsync(new Paginacion<Log> { Items = new List<Log> { new Log { Id_Log = 1 } }, TotalCount = 1 });

        var result = await _service.ListarLogsPaginacionAsync(1, 10, "", desde, hasta);

        Assert.IsTrue(result.IsSuccess);
        _mockRepository.Verify(r => r.ListarLogsPaginacionAsync(1, 10, "", desde, hasta), Times.Once);
    }

    [TestMethod]
    public async Task ListarLogsPaginacion_SiDesdeEsPosteriorAHasta_DebeRechazarSinConsultar()
    {
        var result = await _service.ListarLogsPaginacionAsync(1, 10, "", new DateOnly(2026, 11, 5), new DateOnly(2026, 11, 1));

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual("La fecha 'desde' no puede ser posterior a la fecha 'hasta'.", result.Message);
        _mockRepository.Verify(r => r.ListarLogsPaginacionAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<DateOnly?>(), It.IsAny<DateOnly?>()), Times.Never);
    }

    [TestMethod]
    public async Task Controlador_SiElRangoEsInvalido_DebeResponder400()
    {
        var controller = new LogController(new Mock<ILogService>().Object);

        var actionResult = await controller.ListarLogsPaginacion(1, 10, "", new DateOnly(2026, 11, 5), new DateOnly(2026, 11, 1));

        Assert.IsInstanceOfType(actionResult.Result, typeof(BadRequestObjectResult));
    }
}

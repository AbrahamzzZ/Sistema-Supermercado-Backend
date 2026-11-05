using Domain.Models.Dto.Response.Usuario;
using Infrastructure.Repository.InterfacesRepository;
using Infrastructure.Services.business;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Moq;

namespace UnitTests.Security;

[TestClass]
public class TestVerificacionUsuario
{
    private Mock<IUsuarioRepository> _mockRepository;
    private VerificacionUsuarioService _service;

    [TestInitialize]
    public void Setup()
    {
        _mockRepository = new Mock<IUsuarioRepository>();
        var configuracion = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?> { ["Seguridad:VerificacionUsuarioSegundos"] = "60" })
            .Build();
        _service = new VerificacionUsuarioService(_mockRepository.Object, new MemoryCache(new MemoryCacheOptions()), configuracion);
    }

    private void UsuarioEnBase(bool activo, string rol) =>
        _mockRepository.Setup(r => r.ObtenerUsuarioAsync(5))
            .ReturnsAsync(new UsuarioRolResponse { Id_Usuario = 5, Estado = activo, Nombre_Rol = rol });

    [TestMethod]
    public async Task UsuarioActivoConElMismoRol_EsValido()
    {
        UsuarioEnBase(true, "Empleado");

        Assert.IsTrue(await _service.EsSesionValidaAsync(5, "Empleado"));
    }

    [TestMethod]
    public async Task UsuarioDesactivado_NoEsValido()
    {
        UsuarioEnBase(false, "Empleado");

        Assert.IsFalse(await _service.EsSesionValidaAsync(5, "Empleado"));
    }

    [TestMethod]
    public async Task RolCambiado_NoEsValido()
    {
        UsuarioEnBase(true, "Empleado");

        Assert.IsFalse(await _service.EsSesionValidaAsync(5, "Administrador"));
    }

    [TestMethod]
    public async Task UsuarioInexistenteOIdInvalido_NoEsValido()
    {
        _mockRepository.Setup(r => r.ObtenerUsuarioAsync(It.IsAny<int>())).ReturnsAsync((UsuarioRolResponse?)null);

        Assert.IsFalse(await _service.EsSesionValidaAsync(5, "Empleado"));
        Assert.IsFalse(await _service.EsSesionValidaAsync(0, "Empleado"));
    }

    [TestMethod]
    public async Task ConsultaLaBaseUnaSolaVezMientrasDuraLaCache()
    {
        UsuarioEnBase(true, "Empleado");

        await _service.EsSesionValidaAsync(5, "Empleado");
        await _service.EsSesionValidaAsync(5, "Empleado");

        _mockRepository.Verify(r => r.ObtenerUsuarioAsync(5), Times.Once);
    }

    [TestMethod]
    public async Task Invalidar_HaceQueElCambioSeApliqueEnLaSiguientePeticion()
    {
        UsuarioEnBase(true, "Empleado");
        Assert.IsTrue(await _service.EsSesionValidaAsync(5, "Empleado"));

        UsuarioEnBase(false, "Empleado");
        _service.Invalidar(5);

        Assert.IsFalse(await _service.EsSesionValidaAsync(5, "Empleado"));
    }
}

using DataBaseFirst.Models;
using DataBaseFirst.Models.Dto;
using DataBaseFirst.Repository.InterfacesRepository;
using DataBaseFirst.Services;
using Moq;
using Utilities.Shared;

namespace UnitTests.Services;

[TestClass]
public class TestUsuarioService
{
    private Mock<IUsuarioRepository> _mockRepository;
    private UsuarioService _service;

    [TestInitialize]
    public void Setup()
    {
        _mockRepository = new Mock<IUsuarioRepository>();
        //_service = new UsuarioService(_mockRepository.Object);
    }

    [TestMethod]
    public async Task RegistrarUsuario_DeberiaFallar_SiUsuarioEsNull()
    {
        var result = await _service.RegistrarUsuarioAsync(null);
        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual(Mensajes.MESSAGE_NULL, result.Message);
    }

    [TestMethod]
    public async Task RegistrarUsuario_DeberiaFallar_SiCamposObligatoriosVacios()
    {
        var usuario = new Usuario { Nombre_Completo = "", Correo_Electronico = "", Clave = "" };

        var result = await _service.RegistrarUsuarioAsync(usuario);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual(Mensajes.MESSAGE_EMPTY, result.Message);
    }

    [TestMethod]
    public async Task RegistrarUsuario_DeberiaFallar_SiNombreInvalido()
    {
        var usuario = new Usuario { Codigo = "USR01", Nombre_Completo = "Juan123", Correo_Electronico = "test@mail.com", Clave = "ClaveValida#123" };

        var result = await _service.RegistrarUsuarioAsync(usuario);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual("El nombre completo solo puede contener letras y espacios.", result.Message);
    }

    [TestMethod]
    public async Task RegistrarUsuario_DeberiaFallar_SiClaveInvalida()
    {
        var usuario = new Usuario { Codigo = "USR01", Nombre_Completo = "Juan Perez", Correo_Electronico = "test@mail.com", Clave = "123" };

        var result = await _service.RegistrarUsuarioAsync(usuario);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual("La clave debe tener al menos 10 caracteres, incluyendo letras, números y caracteres especiales.", result.Message);
    }

    [TestMethod]
    public async Task RegistrarUsuario_DeberiaFallar_SiCorreoInvalido()
    {
        var usuario = new Usuario { Codigo = "USR01", Nombre_Completo = "Juan Perez", Correo_Electronico = "correo_invalido", Clave = "ClaveValida#123" };

        var result = await _service.RegistrarUsuarioAsync(usuario);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual("El correo electrónico no tiene un formato válido", result.Message);
    }

    [TestMethod]
    public async Task RegistrarUsuario_DeberiaFallar_SiCodigoDuplicado()
    {
        var usuario = new Usuario { Codigo = "USR01", Nombre_Completo = "Juan Perez", Correo_Electronico = "test@mail.com", Clave = "ClaveValida#123" };

        _mockRepository.Setup(r => r.ListarUsuariosAsync()).ReturnsAsync(new List<UsuarioRol>
        {
            new UsuarioRol { Codigo = "USR01" }
        });

        var result = await _service.RegistrarUsuarioAsync(usuario);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual("El código ya existe", result.Message);
    }


    [TestMethod]
    public async Task EditarUsuario_DeberiaFallar_SiUsuarioEsNull()
    {
        var result = await _service.EditarUsuarioAsync(null);
        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual(Mensajes.MESSAGE_NULL, result.Message);
    }

    [TestMethod]
    public async Task EditarUsuario_DeberiaFallar_SiCamposObligatoriosVacios()
    {
        var usuario = new Usuario { Nombre_Completo = "", Correo_Electronico = "" };

        var result = await _service.EditarUsuarioAsync(usuario);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual(Mensajes.MESSAGE_EMPTY, result.Message);
    }

    [TestMethod]
    public async Task EditarUsuario_DeberiaFallar_SiNoExisteUsuario()
    {
        var usuario = new Usuario { Id_Usuario = 1, Nombre_Completo = "Juan Perez", Correo_Electronico = "test@mail.com" };

        _mockRepository.Setup(r => r.ObtenerUsuarioAsync(usuario.Id_Usuario)).ReturnsAsync((UsuarioRol)null);

        var result = await _service.EditarUsuarioAsync(usuario);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual(Mensajes.MESSAGE_QUERY_NOT_FOUND, result.Message);
    }

    [TestMethod]
    public async Task EditarUsuario_DeberiaFallar_SiEsUnicoAdministradorYSeQuiereInactivar()
    {
        var usuarioActual = new UsuarioRol { Id_Usuario = 1, Nombre_Rol = "Administrador", Estado = true, Id_Rol = 1 };
        var usuarioEditado = new Usuario { Id_Usuario = 1, Nombre_Completo = "Juan Perez", Correo_Electronico = "test@mail.com", Estado = false, Id_Rol = 1 };

        _mockRepository.Setup(r => r.ObtenerUsuarioAsync(usuarioEditado.Id_Usuario)).ReturnsAsync(usuarioActual);
        _mockRepository.Setup(r => r.ListarUsuariosAsync()).ReturnsAsync(new List<UsuarioRol> { usuarioActual });

        var result = await _service.EditarUsuarioAsync(usuarioEditado);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual("No se puede modificar el rol al único administrador activo.", result.Message);
    }


    [TestMethod]
    public async Task EliminarUsuario_DeberiaFallar_SiUsuarioNoExiste()
    {
        _mockRepository.Setup(r => r.ObtenerUsuarioAsync(1)).ReturnsAsync((UsuarioRol)null);

        var result = await _service.EliminarUsuarioAsync(1);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual(Mensajes.MESSAGE_QUERY_NOT_FOUND, result.Message);
    }

    [TestMethod]
    public async Task EliminarUsuario_DeberiaFallar_SiEsUnicoAdministrador()
    {
        var usuario = new UsuarioRol { Id_Usuario = 1, Nombre_Rol = "Administrador", Estado = true };

        _mockRepository.Setup(r => r.ObtenerUsuarioAsync(1)).ReturnsAsync(usuario);
        _mockRepository.Setup(r => r.ListarUsuariosAsync()).ReturnsAsync(new List<UsuarioRol> { usuario });

        var result = await _service.EliminarUsuarioAsync(1);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual("No se puede eliminar al único administrador activo.", result.Message);
    }


    [TestMethod]
    public async Task IniciarSesion_DeberiaFallar_SiLoginEsNull()
    {
        var result = await _service.IniciarSesionAsync(null);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual("Correo y clave son obligatorios.", result.Message);
    }

    [TestMethod]
    public async Task IniciarSesion_DeberiaFallar_SiCredencialesInvalidas()
    {
        var login = new Login { Correo_Electronico = "test@mail.com", Clave = "123456" };

        _mockRepository.Setup(r => r.IniciarSesionAsync(login)).ReturnsAsync((UsuarioRol)null);

        var result = await _service.IniciarSesionAsync(login);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual("Credenciales inválidas.", result.Message);
    }

    [TestMethod]
    public async Task IniciarSesion_DeberiaFallar_SiUsuarioInactivo()
    {
        var login = new Login { Correo_Electronico = "test@mail.com", Clave = "123456" };
        var usuario = new UsuarioRol { Estado = false };

        _mockRepository.Setup(r => r.IniciarSesionAsync(login)).ReturnsAsync(usuario);

        var result = await _service.IniciarSesionAsync(login);

        Assert.IsFalse(result.IsSuccess);
        Assert.AreEqual("Usuario inactivo. Contacte con el administrador.", result.Message);
    }
}

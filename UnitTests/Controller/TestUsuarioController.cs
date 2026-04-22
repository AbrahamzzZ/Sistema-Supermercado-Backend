using APIRestSistemaVentas.Controllers;
using Domain.Models;
using Domain.Models.Dto;
using Infrastructure.Helpers;
using Infrastructure.Repository.InterfacesRepository;
using Infrastructure.Repository.InterfacesServices;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Utilities.Shared;

namespace UnitTests.Controller;

[TestClass]
public class TestUsuarioController
{
    private Mock<IUsuarioService> _mockService;
    private Mock<IMenuService> _mockMenuService;
    private Mock<IToken> _mockToken;
    private UsuarioController _controller;

    [TestInitialize]
    public void Setup()
    {
        _mockService = new Mock<IUsuarioService>();
        _mockMenuService = new Mock<IMenuService>();
        _mockToken = new Mock<IToken>();
        /*_controller = new UsuarioController(
            _mockService.Object,
            _mockToken.Object,
            _mockMenuService.Object
        );*/
    }

    [TestMethod]
    public async Task GetUsuarios_DeberiaRetornarOk()
    {
        _mockService.Setup(s => s.ListarUsuariosAsync())
            .ReturnsAsync(new ApiResponse<List<UsuarioRol>> { IsSuccess = true,Data = new List<UsuarioRol> { new UsuarioRol { Id_Usuario = 1, Nombre_Completo = "Snacks" } }});

        var result = await _controller.GetUsuarios();

        var okResult = result.Result as OkObjectResult;
        Assert.IsNotNull(okResult);
    }

    [TestMethod]
    public async Task GetUsuario_DeberiaRetornarNotFound_SiNoExiste()
    {
        _mockService.Setup(s => s.ObtenerUsuarioAsync(99))
            .ReturnsAsync(new ApiResponse<UsuarioRol>
            {
                IsSuccess = false
            });

        var result = await _controller.GetUsuario(99);

        Assert.IsInstanceOfType(result.Result, typeof(NotFoundObjectResult));
    }

    [TestMethod]
    public async Task IniciarSesion_DeberiaRetornarOk_SiCredencialesValidas()
    {
        var login = new Login { Correo_Electronico = "test@mail.com", Clave = "123456" };
        var usuario = new UsuarioRol { Id_Usuario = 1 };

        _mockService.Setup(s => s.IniciarSesionAsync(login))
            .ReturnsAsync(new ApiResponse<UsuarioRol>{ IsSuccess = true, Data = usuario});

        _mockMenuService.Setup(m => m.ObtenerMenusAsync(usuario.Id_Usuario))
            .ReturnsAsync(new ApiResponse<List<Menu>>
            { IsSuccess = true, Data = new List<Menu>() });

        _mockToken.Setup(t => t.GenerarToken(usuario, It.IsAny<List<Menu>>()))
            .Returns("token_fake");

        var result = await _controller.IniciarSesion(login);
        Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
    }

    [TestMethod]
    public async Task IniciarSesion_DeberiaRetornarUnauthorized_SiFalla()
    {
        var login = new Login { Correo_Electronico = "test@mail.com", Clave = "123456" };

        _mockService.Setup(s => s.IniciarSesionAsync(login))
            .ReturnsAsync(new ApiResponse<UsuarioRol>
            {
                IsSuccess = false
            });

        var result = await _controller.IniciarSesion(login);

        Assert.IsInstanceOfType(result.Result, typeof(UnauthorizedObjectResult));
    }


    [TestMethod]
    public async Task RegistrarUsuario_DeberiaRetornarOk()
    {
        var usuario = new Usuario();

        _mockService.Setup(s => s.RegistrarUsuarioAsync(usuario))
            .ReturnsAsync(new ApiResponse<object> { IsSuccess = true });

        var result = await _controller.RegistrarUsuario(usuario);

        Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
    }

    [TestMethod]
    public async Task RegistrarUsuario_DeberiaRetornarBadRequest()
    {
        var usuario = new Usuario();

        _mockService.Setup(s => s.RegistrarUsuarioAsync(usuario))
            .ReturnsAsync(new ApiResponse<object> { IsSuccess = false });

        var result = await _controller.RegistrarUsuario(usuario);

        Assert.IsInstanceOfType(result.Result, typeof(BadRequestObjectResult));
    }

    [TestMethod]
    public async Task EditarUsuario_DeberiaRetornarOk()
    {
        var usuario = new Usuario();

        _mockService.Setup(s => s.EditarUsuarioAsync(usuario))
            .ReturnsAsync(new ApiResponse<object> { IsSuccess = true });

        var result = await _controller.EditarUsuario(1, usuario);

        Assert.IsInstanceOfType(result.Result, typeof(OkObjectResult));
    }

    [TestMethod]
    public async Task EliminarUsuario_DeberiaRetornarNotFound()
    {
        _mockService.Setup(s => s.EliminarUsuarioAsync(99))
            .ReturnsAsync(new ApiResponse<int> { IsSuccess = false });

        var result = await _controller.EliminarUsuario(99);

        Assert.IsInstanceOfType(result.Result, typeof(NotFoundObjectResult));
    }

}

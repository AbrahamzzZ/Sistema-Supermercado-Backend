using System.Net;
using System.Reflection;
using System.Security.Claims;
using APIRestSistemaVentas.Controllers;
using APIRestSistemaVentas.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Utilities.Security;

namespace UnitTests.Security;

[TestClass]
public class TestSeguridadApi
{
    private static AuthorizeAttribute[] Autorizaciones(Type controlador, string accion)
    {
        var metodo = controlador.GetMethod(accion) ?? throw new InvalidOperationException($"{controlador.Name}.{accion} no existe");
        return metodo.GetCustomAttributes<AuthorizeAttribute>()
            .Concat(controlador.GetCustomAttributes<AuthorizeAttribute>())
            .ToArray();
    }

    private static bool SoloAdministrador(Type controlador, string accion) =>
        Autorizaciones(controlador, accion).Any(a => a.Roles == Roles.Administrador);

    [DataTestMethod]
    [DataRow(typeof(UsuarioController), nameof(UsuarioController.ListarUsuarios))]
    [DataRow(typeof(UsuarioController), nameof(UsuarioController.ListarUsuariosPaginacion))]
    [DataRow(typeof(UsuarioController), nameof(UsuarioController.ObtenerUsuario))]
    [DataRow(typeof(UsuarioController), nameof(UsuarioController.RegistrarUsuario))]
    [DataRow(typeof(UsuarioController), nameof(UsuarioController.EditarUsuario))]
    [DataRow(typeof(UsuarioController), nameof(UsuarioController.EliminarUsuario))]
    [DataRow(typeof(RolController), nameof(RolController.ListarRoles))]
    [DataRow(typeof(LogController), nameof(LogController.ListarLogsPaginacion))]
    [DataRow(typeof(LogController), nameof(LogController.ObtenerLog))]
    [DataRow(typeof(NegocioController), nameof(NegocioController.EditarNegocio))]
    [DataRow(typeof(ClienteController), nameof(ClienteController.RegistrarCliente))]
    [DataRow(typeof(ClienteController), nameof(ClienteController.EditarCliente))]
    [DataRow(typeof(ClienteController), nameof(ClienteController.EliminarCliente))]
    [DataRow(typeof(ProveedorController), nameof(ProveedorController.RegistrarProveedor))]
    [DataRow(typeof(ProveedorController), nameof(ProveedorController.EditarProveedor))]
    [DataRow(typeof(ProveedorController), nameof(ProveedorController.EliminarProveedor))]
    [DataRow(typeof(TransportistaController), nameof(TransportistaController.RegistrarTransportista))]
    [DataRow(typeof(TransportistaController), nameof(TransportistaController.EditarTransportista))]
    [DataRow(typeof(TransportistaController), nameof(TransportistaController.EliminarTransportista))]
    [DataRow(typeof(SucursalController), nameof(SucursalController.RegistrarSucursal))]
    [DataRow(typeof(SucursalController), nameof(SucursalController.EditarSucursal))]
    [DataRow(typeof(SucursalController), nameof(SucursalController.EliminarSucursal))]
    public void AccionesDeAdministracion_SoloAdministrador(Type controlador, string accion)
    {
        Assert.IsTrue(SoloAdministrador(controlador, accion), $"{controlador.Name}.{accion} debe exigir el rol Administrador");
    }

    // Lo que el empleado necesita para vender y comprar debe seguir disponible
    [DataTestMethod]
    [DataRow(typeof(VentaController), nameof(VentaController.RegistrarVenta))]
    [DataRow(typeof(CompraController), nameof(CompraController.RegistrarCompra))]
    [DataRow(typeof(ProductoController), nameof(ProductoController.RegistrarProducto))]
    [DataRow(typeof(ClienteController), nameof(ClienteController.ListarClientes))]
    [DataRow(typeof(SucursalController), nameof(SucursalController.ListarSucursales))]
    [DataRow(typeof(ProveedorController), nameof(ProveedorController.ListarProveedores))]
    [DataRow(typeof(TransportistaController), nameof(TransportistaController.ListarTransportistas))]
    [DataRow(typeof(NegocioController), nameof(NegocioController.ObtenerNegocio))]
    public void AccionesDelEmpleado_NoExigenAdministrador(Type controlador, string accion)
    {
        var autorizaciones = Autorizaciones(controlador, accion);

        Assert.IsTrue(autorizaciones.Length > 0, $"{controlador.Name}.{accion} debe exigir sesión");
        Assert.IsFalse(SoloAdministrador(controlador, accion), $"{controlador.Name}.{accion} no debe exigir Administrador");
    }

    [TestMethod]
    public void Login_EsPublico()
    {
        Assert.AreEqual(0, Autorizaciones(typeof(UsuarioController), nameof(UsuarioController.IniciarSesion)).Length);
    }

    private static HttpContext Contexto(string ruta, string? idUsuario = null)
    {
        var contexto = new DefaultHttpContext();
        contexto.Request.Path = ruta;
        contexto.Connection.RemoteIpAddress = IPAddress.Parse("10.0.0.8");
        if (idUsuario != null)
            contexto.User = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.NameIdentifier, idUsuario) }, "Bearer"));
        return contexto;
    }

    [TestMethod]
    public void LimitePeticiones_CadaUsuarioTieneSuPropioContador()
    {
        Assert.AreEqual("usuario:3", LimitePeticiones.ObtenerParticion(Contexto("/api/venta", "3")));
        Assert.AreEqual("usuario:4", LimitePeticiones.ObtenerParticion(Contexto("/api/venta", "4")));
    }

    [TestMethod]
    public void LimitePeticiones_SinSesionSeCuentaPorIp()
    {
        Assert.AreEqual("ip:10.0.0.8", LimitePeticiones.ObtenerParticion(Contexto("/api/producto")));
    }

    [TestMethod]
    public void LimitePeticiones_ElLoginTieneSuPropioContadorPorIp()
    {
        Assert.AreEqual("login:10.0.0.8", LimitePeticiones.ObtenerParticion(Contexto("/api/Usuario/login")));
        Assert.AreEqual("login:10.0.0.8", LimitePeticiones.ObtenerParticion(Contexto("/api/usuario/login/")));
    }
}

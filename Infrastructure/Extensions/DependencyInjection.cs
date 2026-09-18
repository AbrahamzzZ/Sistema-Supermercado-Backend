using Domain.Models;
using Domain.Models.Dto.Response.Compra;
using Domain.Models.Dto.Response.Venta;
using FluentValidation;
using Infrastructure.Repository;
using Infrastructure.Repository.InterfacesBusiness;
using Infrastructure.Repository.InterfacesRepository;
using Infrastructure.Repository.InterfacesServices;
using Infrastructure.Services;
using Infrastructure.Services.business;
using Infrastructure.Services.Validators;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<ICategoriaRepository, CategoriaRepository>();
            services.AddScoped<IClienteRepository, ClienteRepository>();
            services.AddScoped<IProveedorRepository, ProveedorRepository>();
            services.AddScoped<RolRepository>();
            services.AddScoped<MenuRepository>();
            services.AddScoped<ITransportistaRepository, TransportistaRepository>();
            services.AddScoped<ILogRepository, LogRepository>();
            services.AddScoped<IUsuarioRepository, UsuarioRepository>();
            services.AddScoped<INegocioRepository, NegocioRepository>();
            services.AddScoped<IProductoRepository, ProductoRepository>();
            services.AddScoped<IOfertaRepository, OfertaRepository>();
            services.AddScoped<ISucursalRepository, SucursalRepository>();
            services.AddScoped<ICompraRepository, CompraRepository>();
            services.AddScoped<IVentaRepository, VentaRepository>();
            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<ICategoriaService, CategoriaService>();
            services.AddScoped<IClienteService, ClienteService>();
            services.AddScoped<IProveedorService, ProveedorService>();
            services.AddScoped<IMenuService, MenuService>();
            services.AddScoped<RolService>();
            services.AddScoped<ITransportistaService, TransportistaService>();
            services.AddScoped<ILogService, LogService>();
            services.AddScoped<ICurrentUser, CurrentUserService>();
            services.AddScoped<IAuditoriaService, AuditoriaService>();
            services.AddScoped<IRequestContext, RequestContextService>();
            services.AddScoped<IUsuarioService, UsuarioService>();
            services.AddScoped<INegocioService, NegocioService>();
            services.AddScoped<IProductoService, ProductoService>();
            services.AddScoped<IOfertaService, OfertaService>();
            services.AddScoped<ISucursalService, SucursalService>();
            services.AddScoped<ICompraService, CompraService>();
            services.AddScoped<IVentaService, VentaService>();
            return services;
        }

        public static IServiceCollection AddValidators(this IServiceCollection services)
        {
            services.AddScoped<IValidator<Categorium>, CategoriaValidator>();
            services.AddScoped<IValidator<Cliente>, ClienteValidator>();
            services.AddScoped<IValidator<Proveedor>, ProveedorValidator>();
            services.AddScoped<IValidator<Transportistum>, TransportistaValidator>();
            services.AddScoped<IValidator<Usuario>, UsuarioValidator>();
            services.AddScoped<IValidator<Negocio>, NegocioValidator>();
            services.AddScoped<IValidator<Producto>, ProductoValidator>();
            services.AddScoped<IValidator<Ofertum>, OfertaValidator>();
            services.AddScoped<IValidator<Sucursal>, SucursalValidator>();
            services.AddScoped<IValidator<Compras>, CompraValidator>();
            services.AddScoped<IValidator<Ventas>, VentaValidator>();
            return services;
        }
    }
}

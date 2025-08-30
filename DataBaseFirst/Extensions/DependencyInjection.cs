using DataBaseFirst.Models;
using DataBaseFirst.Models.Dto;
using DataBaseFirst.Repository;
using DataBaseFirst.Services;
using DataBaseFirst.Services.Validators;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace DataBaseFirst.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<CategoriaRepository>();
            services.AddScoped<ClienteRepository>();
            services.AddScoped<ProveedorRepository>();
            services.AddScoped<RolRepository>();
            services.AddScoped<TransportistaRepository>();
            services.AddScoped<UsuarioRepository>();
            services.AddScoped<NegocioRepository>();
            services.AddScoped<ProductoRepository>();
            services.AddScoped<OfertaRepository>();
            services.AddScoped<SucursalRepository>();
            services.AddScoped<CompraRepository>();
            services.AddScoped<VentaRepository>();
            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<CategoriaService>();
            services.AddScoped<ClienteService>();
            services.AddScoped<ProveedorService>();
            services.AddScoped<MenuService>();
            services.AddScoped<RolService>();
            services.AddScoped<TransportistaService>();
            services.AddScoped<UsuarioService>();
            services.AddScoped<NegocioService>();
            services.AddScoped<ProductoService>();
            services.AddScoped<OfertaService>();
            services.AddScoped<SucursalService>();
            services.AddScoped<CompraService>();
            services.AddScoped<VentaService>();
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

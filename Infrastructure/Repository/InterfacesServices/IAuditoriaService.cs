namespace Infrastructure.Repository.InterfacesServices
{
    public interface IAuditoriaService
    {
        Task RegistrarExitoAsync(string operacion, string descripcion, int? idUsuario, string endpoint, string metodo);
        Task RegistrarFalloAsync(string operacion, string razon, int? idUsuario, string endpoint, string metodo);
        Task RegistrarErrorAsync(string operacion, Exception ex, int? idUsuario, string endpoint, string metodo);
    }
}

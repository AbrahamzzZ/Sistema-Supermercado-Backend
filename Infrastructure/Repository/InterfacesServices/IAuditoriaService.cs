namespace Infrastructure.Repository.InterfacesServices
{
    public interface IAuditoriaService
    {
        Task RegistrarExitoAsync(string operacion, string descripcion);
        Task RegistrarFalloAsync(string operacion, string razon);
        Task RegistrarErrorAsync(string operacion, Exception ex);
    }
}

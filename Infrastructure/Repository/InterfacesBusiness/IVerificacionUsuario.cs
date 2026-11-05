namespace Infrastructure.Repository.InterfacesBusiness
{
    public interface IVerificacionUsuario
    {
        Task<bool> EsSesionValidaAsync(int idUsuario, string? rolDelToken);
        void Invalidar(int idUsuario);
    }
}

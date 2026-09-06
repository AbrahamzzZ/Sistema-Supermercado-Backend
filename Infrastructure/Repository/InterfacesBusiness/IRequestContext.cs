namespace Infrastructure.Repository.InterfacesBusiness
{
    public interface IRequestContext
    {
        string GetEndpoint();
        string GetMethod();
    }
}

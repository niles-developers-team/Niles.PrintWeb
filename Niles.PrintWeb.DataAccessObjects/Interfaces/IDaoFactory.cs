namespace Niles.PrintWeb.DataAccessObjects.Interfaces
{
    public interface IDaoFactory
    {
        IUserDao UserDao { get; }
        ITenantDao TenantDao { get; }
    }
}
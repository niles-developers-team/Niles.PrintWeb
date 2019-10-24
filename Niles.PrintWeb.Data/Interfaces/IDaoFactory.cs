namespace Niles.PrintWeb.Data.Interfaces
{
    public interface IDaoFactory
    {
        IUserDao UserDao { get; }
    }
}
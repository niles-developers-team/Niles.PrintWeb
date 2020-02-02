namespace Niles.PrintWeb.Models.Enumerations
{
    ///<summary>Users roles enumeration.</summary>
    public enum Roles
    {
        ///<summary>Admin user, who control users and tenants colloboration.</summary>
        Admin,
        ///<summary>Tenant user, who can print files.</summary>
        TenantUser,
        ///<summary>Client, who want to print his files.</summary>
        Client
    }
}
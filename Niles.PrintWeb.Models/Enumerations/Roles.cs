using System.ComponentModel;

namespace Niles.PrintWeb.Models.Enumerations
{
    ///<summary>Users roles enumeration.</summary>
    public enum Roles
    {
        ///<summary>Admin user, who control users and tenants colloboration.</summary>
        [Description("Admin")]
        Admin,
        ///<summary>Tenant user, who can print files.</summary>
        [Description("Tenant")]
        TenantUser,
        ///<summary>Client, who want to print his files.</summary>
        [Description("Client")]
        Client
    }
}
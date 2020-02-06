using System;

namespace Niles.PrintWeb.Models.Entities
{
    public class Tenant
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DateCreated { get; set; }
        public int SubscriptionId { get; set; }
        public DateTime SubscribeDate { get; set; }
    }

    public class TenantGetOptions : BaseGetOptions
    {
        public string NormalizedSearch => !string.IsNullOrEmpty(Search) ? $"%{Search}%" : string.Empty;
        public string Search { get; set; }
    }

    public class TenantValidateOptions
    {
        public int? Id { get; set; }
        public string Name { get; set; }
    }
}
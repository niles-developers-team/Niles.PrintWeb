namespace Niles.PrintWeb.Models.Enumerations
{
    ///<summary>Orders statuses enumeration.</summary>
    public enum OrderStatus
    {
        ///<summary>New order.</summary>
        New,
        ///<summary>Order in process.</summary>
        InProcessing,
        ///<summary>Order in progress.</summary>
        InProgress,
        ///<summary>Order canceled.</summary>
        Canceled,
        ///<summary>Order completed.</summary>
        Completed,
        ///<summary>Order draft.</summary>
        Draft
    }
}
namespace Niles.PrintWeb.Models.Enumerations
{
    ///<summary>File printing details statuses enumeration.</summary>
    public enum PrintingStatus
    {
        ///<summary>File in process.</summary>
        InProcess,
        ///<summary>File in printing order.</summary>
        InOrder,
        ///<summary>File is printing now.</summary>
        Printing,
        ///<summary>File is ready now.</summary>
        Ready,
        ///<summary>File printing canceled.</summary>
        Canceled
    }
}
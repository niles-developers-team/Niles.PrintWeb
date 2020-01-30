using System;

namespace Niles.PrintWeb.Shared
{    
    [AttributeUsage(AttributeTargets.Field)]
    public class EnumerationDescription : Attribute
    {
        private string _description;

        public EnumerationDescription(string description)
        {
            _description = description;
        }        
    }
}
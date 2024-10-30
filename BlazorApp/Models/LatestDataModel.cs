using System;

namespace BlazorApp.Models
{
    public class LatestDataModel
    {
        public string LatestType { get; set; }  // Can be "Quote", "Order", or "Invoice"
        public string Details { get; set; }     // Description or details of the latest item
        public DateTime DateCreated { get; set; }  // Date when this item was created
        public string Message { get; set; }     // Message for when no data is available
    }
}

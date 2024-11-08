using System;

namespace BlazorApp.Models
{
    public class ContactComment
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Text { get; set; }
        public int ContactId { get; set; }
        public Contact Contact { get; set; }
    }
}

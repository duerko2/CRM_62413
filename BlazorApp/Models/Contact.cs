using BlazorApp.Components.Pages;

namespace BlazorApp.Models
{
    public class Contact
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Person> Persons { get; set; }
        public string Address { get; set; }
        public string Company { get; set; }
        public string Notes { get; set; }
        public string VAT { get; set; }
        public string Type { get; set; }  // Either "Lead" or "Customer"
        public List<Campaign> Campaigns { get; set; }
        public List<Comment> Comments { get; set; }
        public List<Activity> Activities { get; set; }

    }
}

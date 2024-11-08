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
        public ContactType Type { get; set; }

        public List<Pipeline> Pipelines { get; set; }
        public List<ContactComment> Comments { get; set; }
        public List<Activity> Activities { get; set; }

    }
}

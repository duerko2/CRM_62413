namespace BlazorApp.Models
{
    public class Contact
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<string> Emails { get; set; }
        public List<string> PhoneNumbers { get; set; }
        public List<string> RelatedPersons { get; set; }
        public string Address { get; set; }
        public string Company { get; set; }
        public string Notes { get; set; }
    }
}

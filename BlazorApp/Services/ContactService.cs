using BlazorApp.Models;

namespace BlazorApp.Services
{        
    //For demonstration purposes, we'll change it to data from a database later
    public class ContactService
    {
        public Contact GetContact()
        {
            return new Contact
            {
                Name = "Elgiganten",
                Emails = new List<string> { "el.giganten@company.dk", "el.giganten.topchef@company.dk" },
                PhoneNumbers = new List<string> { "+45 56952608", "+45 56952600" },
                RelatedPersons = new List<string> { "Fie Laudrup", "Søren Svendsen" },
                Address = "Herlev Hovedgade 45, 2730 Herlev, Denmark",
                Company = "Elgiganten A/S",
                Notes = "VIP client, prefers email communication."
            };
        }
    }
}

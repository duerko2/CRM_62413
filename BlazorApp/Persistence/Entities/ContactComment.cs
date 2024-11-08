namespace BlazorApp.Persistence.Entities;

public class ContactComment
{
    public int Id { get; set; }
    public string Text { get; set; }
    public DateTime Date { get; set; }
    public int ContactId { get; set; }
    public virtual Contact Contact { get; set; }
}
using BlazorApp.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Task = BlazorApp.Persistence.Entities.Task;

namespace BlazorApp.Persistence;

public class CrmDbContext : DbContext
{
    public CrmDbContext(DbContextOptions<CrmDbContext> options) : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasOne(u => u.Company)
            .WithMany(c => c.Users)
            .HasForeignKey(u => u.CompanyId)
            .OnDelete(DeleteBehavior.Restrict);  // Disable cascading delete on the Company relationship
    
        modelBuilder.Entity<Contact>()
            .HasOne(c => c.User)
            .WithMany(u => u.Contacts)
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Restrict);  // Disable cascading delete on the User relationship
    }
    
    public DbSet<Contact> Contacts { get; set; }
    public DbSet<Person> Persons { get; set; }
    public DbSet<Pipeline> Pipelines { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Company> Companies { get; set; }
    public DbSet<Campaign> Campaigns { get; set; }
    public DbSet<Task> Tasks { get; set; }
    public DbSet<ContactComment> ContactComments { get; set; }
    public DbSet<PipelineComment> PipelineComments { get; set; }
    public DbSet<ActivityLog> ActivityLogs { get; set; }
}
using BlazorApp.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
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

        modelBuilder.Entity<Campaign>()
            .HasMany(c => c.Stages)
            .WithOne(s => s.Campaign)
            .HasForeignKey(s => s.CampaignId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Pipeline>()
            .HasOne(p => p.Contact)
            .WithMany(c => c.Pipelines)
            .HasForeignKey(p => p.ContactId)
            .OnDelete(DeleteBehavior.Restrict); // Prevent deletion of Contact if Pipelines exist

        modelBuilder.Entity<Pipeline>()
            .HasOne(p => p.Campaign)
            .WithMany(c => c.Pipelines)
            .HasForeignKey(p => p.CampaignId)
            .OnDelete(DeleteBehavior.Restrict); // Prevent deletion of Campaign if Pipelines exist

        modelBuilder.Entity<Pipeline>()
            .HasMany(p => p.Tasks)
            .WithOne(t => t.Pipeline)
            .HasForeignKey(t => t.PipelineId)
            .OnDelete(DeleteBehavior.Cascade); // Delete Tasks when Pipeline is deleted
    }
    
    public DbSet<Contact> Contacts { get; set; }
    public DbSet<Person> Persons { get; set; }
    public DbSet<Pipeline> Pipelines { get; set; }
    public DbSet<PipelineTask> PipelineTasks { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Company> Companies { get; set; }
    public DbSet<Campaign> Campaigns { get; set; }
    public DbSet<CampaignStage> CampaignStages { get; set; }
    public DbSet<PipelineTask> Tasks { get; set; }
    public DbSet<ContactComment> ContactComments { get; set; }
    public DbSet<PipelineComment> PipelineComments { get; set; }
    public DbSet<ActivityLog> ActivityLogs { get; set; }
    public DbSet<UserInvitation> UserInvitations { get; set; }
    public DbSet<UserSession> UserSessions { get; set; }
}
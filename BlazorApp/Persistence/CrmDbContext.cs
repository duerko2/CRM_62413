using BlazorApp.Models;
using Microsoft.EntityFrameworkCore;

namespace BlazorApp.Persistence;

public class CrmDbContext : DbContext
{
    public CrmDbContext(DbContextOptions<CrmDbContext> options) : base(options)
    {
    }
    public DbSet<Contact> Contacts { get; set; }
}
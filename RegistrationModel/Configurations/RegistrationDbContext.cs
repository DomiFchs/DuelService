using Microsoft.EntityFrameworkCore;
using RegistrationModel.Entities;

namespace RegistrationModel.Configurations; 

public class RegistrationDbContext : DbContext {

    public RegistrationDbContext(DbContextOptions<RegistrationDbContext> options) : base(options) { }


    public DbSet<User> Users { get; set; } = null!;
    
    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        
    }
}
using Microsoft.EntityFrameworkCore;

class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options)
        : base(options) { }

    public DbSet<Organization> Organizations => Set<Organization>();
    public DbSet<Employee> Employees => Set<Employee>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.Entity<Organization>(entity =>
        {
            entity.HasMany(e => e.Employees)
                .WithOne(e => e.Organization);
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.Property(e => e.Email)
                .IsRequired();
            entity.Property(e => e.PhoneNumber)
                .IsRequired();
        });
    }
}
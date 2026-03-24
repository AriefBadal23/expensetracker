using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace expensetrackerapi.Models;

public class ExpenseTrackerContext : IdentityDbContext<IdentityUser> // Takes care of Identity related tables.
{
    public ExpenseTrackerContext(DbContextOptions<ExpenseTrackerContext> options) : base(options)
    {
    }
    // DBSet verwijst naar de entities/tables in een database
    public DbSet<Bucket> Buckets { get; set; }
    public DbSet<Transaction> Transactions { get; set; }




    // Make use of FluentAPI to define the relations between the entities.
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Transaction>()
            .Property(x => x.Created_at)
            .HasColumnType("date");

        modelBuilder.Entity<Bucket>()
            .HasMany<Transaction>()
            .WithOne()
            .HasForeignKey(_ => _.BucketId)
            .OnDelete(DeleteBehavior.Cascade);
    }

}
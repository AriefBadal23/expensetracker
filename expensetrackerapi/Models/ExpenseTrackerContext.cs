namespace expensetrackerapi.Models;

using Microsoft.EntityFrameworkCore;


public class ExpenseTrackerContext : DbContext
{
    public ExpenseTrackerContext(DbContextOptions<ExpenseTrackerContext> options) : base(options)
    {

    }
    // DBSet verwijst naar de entities/tables in een database
    public DbSet<Bucket> Buckets { get; set; }
    public DbSet<Transaction> Transactions { get; set; }

    public DbSet<User> Users { get; set; }




    // Make use of FluentAPI to define the relations between the entities.
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Transaction>()
        .Property(x => x.Created_at)
        .HasColumnType("date");

        modelBuilder.Entity<Bucket>()
        .HasMany<Transaction>()
        .WithOne()
        .HasForeignKey(_ => _.BucketId)
        .OnDelete(DeleteBehavior.Cascade);


        modelBuilder.Entity<User>()
        .HasMany<Transaction>()
        .WithOne()
        .HasForeignKey(_ => _.UserId)
        .OnDelete(DeleteBehavior.Cascade);
    }

}
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace expensetrackerapi.Models;

public class ExpenseTrackerContext : IdentityDbContext<ApplicationUser> // Takes care of Identity related tables.
{
    public ExpenseTrackerContext(DbContextOptions<ExpenseTrackerContext> options) : base(options)
    {
    }
    // DBSet verwijst naar de entities/tables in een database
    public DbSet<Bucket> Buckets { get; set; }
    public DbSet<Transaction> Transactions { get; set; }

    public DbSet<UserBuckets> UserBuckets { get; set; }




    // Make use of FluentAPI to define the relations between the entities.
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Transaction>()
            .HasOne<ApplicationUser>()
            .WithMany()
            .HasForeignKey(transaction => transaction.ApplicationUserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Transaction>()
            .Property(transaction => transaction.CreatedAt)
            .HasColumnType("date");

        builder.Entity<Bucket>()
            .HasMany<Transaction>()
            .WithOne()
            .HasForeignKey(transaction => transaction.BucketId)
            .OnDelete(DeleteBehavior.Cascade);

        // optional; because the relationship is set with collection navigation properties.
        // https://learn.microsoft.com/en-us/ef/core/modeling/relationships/many-to-many#many-to-many-with-class-for-join-entity
        builder.Entity<Bucket>()
            .HasMany<ApplicationUser>(x => x.Users)
            .WithMany(e => e.Buckets)
            .UsingEntity<UserBuckets>();
    }

}
using Microsoft.EntityFrameworkCore;
using System;
using traq_web_project_assessment.Models;

namespace traq_web_project_assessment.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // These are the tables in the DB (indicated by DbSet)
        public DbSet<Person> Persons { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Status> Statuses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Setting up the person table
            modelBuilder.Entity<Person>(entity =>
            {
                entity.HasKey(p => p.Id);
                entity.HasIndex(p => p.IdNumber).IsUnique(); // ID Number must be unique
                entity.Property(p => p.IdNumber).IsRequired().HasMaxLength(13); //13 digit SA ID
                entity.Property(p => p.FirstName).IsRequired().HasMaxLength(100);
                entity.Property(p => p.Surname).IsRequired().HasMaxLength(100);

                // This then makes it possible for a single person to have many accounts
                entity.HasMany(p => p.Accounts)
                      .WithOne(a => a.Person)
                      .HasForeignKey(a => a.PersonId)
                      .OnDelete(DeleteBehavior.Restrict); // If the account exists then do not delete the account
            });

            // Configuring the status
            modelBuilder.Entity<Status>(entity =>
            {
                entity.HasKey(s => s.Id);
                entity.Property(s => s.StatusName).IsRequired().HasMaxLength(50);

                // The seed data for needed for the Status table
                //There are 2 statuses
                entity.HasData(
                    new Status { Id = 1, StatusName = "Open" },
                    new Status { Id = 2, StatusName = "Closed" }
                );
            });

            // Configuring the acount
            modelBuilder.Entity<Account>(entity =>
            {
                entity.HasKey(a => a.Id);
                entity.HasIndex(a => a.AccountNumber).IsUnique(); // Must be unique
                entity.Property(a => a.AccountNumber).IsRequired().HasMaxLength(20);
                entity.Property(a => a.AccountName).IsRequired().HasMaxLength(100);
                entity.Property(a => a.OutstandingBalance).HasColumnType("decimal(18,2)");

                // The relationship with Person
                entity.HasOne(a => a.Person)
                      .WithMany(p => p.Accounts)
                      .HasForeignKey(a => a.PersonId)
                      .OnDelete(DeleteBehavior.Restrict);

                // The relationship with Status
                entity.HasOne(a => a.Status)
                      .WithMany(s => s.Accounts)
                      .HasForeignKey(a => a.StatusId)
                      .OnDelete(DeleteBehavior.Restrict);

                // Making it possible that one Account can have many Transactions
                entity.HasMany(a => a.Transactions)
                      .WithOne(t => t.Account)
                      .HasForeignKey(t => t.AccountId)
                      .OnDelete(DeleteBehavior.Cascade); // Delete transactions if the account has been deleted
            });

            // Configuring Transaction
            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.HasKey(t => t.Id);
                entity.Property(t => t.DebitAmount).HasColumnType("decimal(18,2)");
                entity.Property(t => t.CreditAmount).HasColumnType("decimal(18,2)");
                entity.Property(t => t.Description).IsRequired().HasMaxLength(500);
                entity.Property(t => t.TransactionDate).IsRequired();
                entity.Property(t => t.CaptureDate).IsRequired();

                
                entity.HasOne(t => t.Account) //This is the relationship with the account
                      .WithMany(a => a.Transactions)
                      .HasForeignKey(t => t.AccountId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // This is the user Configuration that is used for authentication
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.Id);
                entity.HasIndex(u => u.Username).IsUnique();
                entity.Property(u => u.Username).IsRequired().HasMaxLength(50);
                entity.Property(u => u.PasswordHash).IsRequired().HasMaxLength(255);
                entity.Property(u => u.CreatedDate).IsRequired();

                // Creating a default admin user
                // Username: admin
                // Password: Admin123
                
                entity.HasData(
                    new User
                    {
                        Id = 1,
                        Username = "admin",
                        PasswordHash = "$2a$12$84mSCPey6C.XPpLm15ROPuPiuTZs1YbOTUiM.N2BWko8MS/r39QC2",  // BCrypt hash of the password for added security
                        FullName = "System Administrator",
                        CreatedDate = new DateTime(2024, 1, 1),
                        IsActive = true
                    }
                );
            });
        }
    }
}
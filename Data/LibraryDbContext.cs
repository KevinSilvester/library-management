﻿using library_management.Models;
using Microsoft.EntityFrameworkCore;

namespace library_management.Data
{
    public class LibraryDbContext : DbContext
    {
        public LibraryDbContext(DbContextOptions<LibraryDbContext> options) : base(options) { }

        public DbSet<Book> Books { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<Borrowing> Borrowings { get; set; }
        public DbSet<User> Users { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            var passwordHasher = new System.Security.Cryptography.SHA256Managed();
            var password = "test123";
            var passwordHash = Convert.ToBase64String(passwordHasher.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password)));

            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, Username = "admin", PasswordHash = passwordHash }
            );

            modelBuilder.Entity<Book>()
                .HasKey(b => b.ISBN);

            modelBuilder.Entity<Member>()
                .HasMany(m => m.Borrowings)
                .WithOne(b => b.Member)
                .HasForeignKey(b => b.MemberId);

            modelBuilder.Entity<Borrowing>()
                .HasOne(b => b.Book)
                .WithMany()
                .HasForeignKey(b => b.BookISBN);
        }
    }
}

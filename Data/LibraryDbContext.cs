﻿
using library_management.Models;
using Microsoft.EntityFrameworkCore;

public class LibraryDbContext : DbContext
{
    public LibraryDbContext(DbContextOptions<LibraryDbContext> options) : base(options) { }

    public DbSet<Book> Books { get; set; }
    public DbSet<Member> Members { get; set; }
    public DbSet<Borrowing> Borrowings { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Member>()
            .HasMany(m => m.Borrowings)
            .WithOne(b => b.Member)
            .HasForeignKey(b => b.MemberId);

        modelBuilder.Entity<Borrowing>()
            .HasOne(b => b.Book)
            .WithMany()
            .HasForeignKey(b => b.BookId);
    }
}
﻿using Microsoft.EntityFrameworkCore;
using OnlineBookstore.Domain.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineBookstore.Infrastructure.Context
{
    public class BookdbContext : DbContext
    {
        public BookdbContext(DbContextOptions<BookdbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Author> Authors { get; set; }
        public virtual DbSet<Book> Books { get; set; }
        public virtual DbSet<Admin_User> Admin_Users { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseCollation("Cyrillic_General_CI_AS");

            modelBuilder.Entity<Book>(entity =>
            {
                entity.HasKey(e => e.Id)
                   .HasName("PK__Book_M__1788CC4D1FCDBCEB")
                   .IsClustered(false);
                entity.HasIndex(e => e.BookName, "UQ__Book_A__3091033107020F21").IsUnique();
                entity.HasOne(d => d.author).WithMany(p => p.book)
                    .HasForeignKey(d => d.AuthorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_authors_books");
            });

            modelBuilder.Entity<Author>(entity =>
            {
                entity.HasKey(e => e.Id)
                   .HasName("PK__Author_M__1788CC4D1FCDBCEB")
                   .IsClustered(false);
                entity.HasIndex(e => e.AuthorName, "UQ__Author_A__3091033107020F21").IsUnique();
               
            });

            modelBuilder.Entity<Admin_User>(entity =>
            {
                entity.HasKey(e => e.Id)
                   .HasName("PK__Admin_User_M__1788CC4D1FCDBCEB")
                   .IsClustered(false);
                entity.HasIndex(e => e.APIkey, "UQ__Admin_User_A__3091033107020F21").IsUnique();

            });

        }

    }
}
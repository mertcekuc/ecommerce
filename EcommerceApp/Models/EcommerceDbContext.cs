using System;
using System.Collections.Generic;
using EcommerceApp.Models.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EcommerceApp.Models;

public  class EcommerceDbContext : IdentityDbContext<User, AppRole, int>
{
    public EcommerceDbContext(DbContextOptions<EcommerceDbContext> dbContext) : base(dbContext) { }

 

    public virtual DbSet<User> Users { get; set; }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=DESKTOP-L5KQOIL;Initial Catalog=ecommerce;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(e => e.LastName)
                .HasMaxLength(100)
                .IsFixedLength();

            entity.Property(e => e.FirstName)
            .HasMaxLength(100)
				.IsFixedLength();
        });
    }
}

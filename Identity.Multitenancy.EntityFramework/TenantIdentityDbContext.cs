using Identity.MultiTenancy.EntityFramework;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace Identity.MultiTenancy
{
    public class TenantIdentityDbContext<TKey> : IdentityDbContext<TenantIdentityUser<TKey>, IdentityRole<TKey>, TKey>
        where TKey : IEquatable<TKey>
    {
        public TenantIdentityDbContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<Tenant<TKey>> Tenants { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Tenant<TKey>>(b =>
            {
                b.HasKey(t => t.Id);
                b.HasIndex(t => t.Name).HasName("NameIndex");
                b.ToTable("AspNetTenants");

                b.Property(t => t.Name).HasMaxLength(256);
            });
            base.OnModelCreating(builder);
        }
    }
}

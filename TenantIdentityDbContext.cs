using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.AspNet.Identity.MultiTenancy
{
    public class TenantIdentityDbContext : TenantIdentityDbContext<Tenant, TenantIdentityUser, IdentityRole, string, string>
    {
    }

    public class TenantIdentityDbContext<TUser> : TenantIdentityDbContext<Tenant, TUser, IdentityRole, string, string>
        where TUser : TenantIdentityUser<Tenant, string>
    {
    }

    public class TenantIdentityDbContext<TTenant, TUser> : TenantIdentityDbContext<TTenant, TUser, IdentityRole, string, string>
        where TTenant : Tenant<string>
        where TUser : TenantIdentityUser<TTenant, string>
    {
    }

    public class TenantIdentityDbContext<TTenant, TUser, TRole, TTenantKey, TUserKey> : IdentityDbContext<TUser, TRole, TUserKey>
        where TTenant : Tenant<TTenantKey>
        where TUser : TenantIdentityUser<TTenant, TTenantKey, TUserKey>
        where TTenantKey : IEquatable<TTenantKey>
        where TUserKey : IEquatable<TUserKey>
        where TRole : IdentityRole<TUserKey>
    {
        public DbSet<TTenant> Tenants { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<TTenant>(b =>
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

using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.AspNet.Identity.MultiTenancy
{
    public class TenantIdentityDbContext : TenantIdentityDbContext<TenantIdentityUser, IdentityRole, string, string>
    {
    }

    public class TenantIdentityDbContext<TUser, TRole, TTenantKey, TUserKey> : IdentityDbContext<TUser, TRole, TUserKey>
        where TUser : TenantIdentityUser<TTenantKey, TUserKey>
        where TTenantKey : IEquatable<TTenantKey>
        where TUserKey : IEquatable<TUserKey>
        where TRole : IdentityRole<TUserKey>
    {
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Tenant<TTenantKey>>(b =>
            {
                b.HasKey(t => t.Id);
                b.HasIndex(t => t.Name).HasName("NameIndex");
                b.ToTable("AspNetTenants");

                b.Property(t => t.Name).HasMaxLength(256);
            });
        }
    }
}

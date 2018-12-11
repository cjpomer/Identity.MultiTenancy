using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Identity.MultiTenancy.EntityFramework
{
    /// <summary>
    /// Represents a new instance of a persistence store for the specified user and role types.
    /// </summary>
    /// <typeparam name="TUser">The type representing a user.</typeparam>
    /// <typeparam name="TRole">The type representing a role.</typeparam>
    /// <typeparam name="TContext">The type of the data context class used to access the store.</typeparam>
    /// <typeparam name="TKey">The type of the primary key for a role.</typeparam>
    public class TenantUserStore<TTenant, TUser, TRole, TContext, TKey> : UserStore<TUser, TRole, TContext, TKey>, ITenantUserStore<TTenant, TUser>
    where TTenant : Tenant<TKey>
    where TUser : TenantIdentityUser<TKey>
    where TRole : IdentityRole<TKey>
    where TContext : DbContext
    where TKey : IEquatable<TKey>
    {
        public TenantUserStore(TContext context, IdentityErrorDescriber describer = null) : base(context, describer) { }

        public virtual Task SetTenantAsync(TUser user, TTenant tenant, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            user.Tenant = tenant ?? throw new ArgumentNullException(nameof(tenant));
            return Task.FromResult(0);
        }
    }
}

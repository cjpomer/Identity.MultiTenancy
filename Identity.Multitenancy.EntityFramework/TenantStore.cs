using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Identity.MultiTenancy.EntityFramework
{
    public class TenantStore<TContext, TTenant, TKey> : ITenantStore<TTenant>
        where TContext : TenantIdentityDbContext<TKey>
        where TTenant : Tenant<TKey>
        where TKey : IEquatable<TKey>
    {
        public TContext Context { get; private set; }

        private readonly IdentityErrorDescriber _errorDescriber;
        private bool _disposed = false;

        private DbSet<TTenant> TenantSet { get { return Context.Set<TTenant>(); } }

        public TenantStore(TContext dbContext, IdentityErrorDescriber errorDescriber = null)
        {
            Context = dbContext;
            _errorDescriber = errorDescriber ?? new IdentityErrorDescriber();
        }

        public Task<TTenant> FindByIdAsync(string id, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();

            return TenantSet.FindAsync(new object[] { id }, cancellationToken);
        }

        public async Task<IdentityResult> CreateAsync(TTenant tenant, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (tenant == null)
            {
                throw new ArgumentNullException(nameof(tenant));
            }
            Context.Add(tenant);
            await Context.SaveChangesAsync(cancellationToken);
            return IdentityResult.Success;
        }

        public async Task<IdentityResult> DeleteAsync(TTenant tenant, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (tenant == null)
            {
                throw new ArgumentNullException(nameof(tenant));
            }

            Context.Remove(tenant);
            try
            {
                await Context.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateConcurrencyException)
            {
                return IdentityResult.Failed(_errorDescriber.ConcurrencyFailure());
            }
            return IdentityResult.Success;
        }

        /// <summary>
        /// Throws if this class has been disposed.
        /// </summary>
        protected void ThrowIfDisposed()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(GetType().Name);
            }
        }
        
        /// <summary>
        /// Dispose the store
        /// </summary>
        public void Dispose()
        {
            _disposed = true;
        }
    }
}

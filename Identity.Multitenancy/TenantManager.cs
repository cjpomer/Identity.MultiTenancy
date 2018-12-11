using Microsoft.AspNetCore.Identity;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Identity.MultiTenancy
{
    public class TenantManager<TTenant> : IDisposable where TTenant : class 
    {
        private readonly ITenantStore<TTenant> _store;
        private bool _disposed = false;

        /// <summary>
        /// The cancellation token used to cancel operations.
        /// </summary>
        protected virtual CancellationToken CancellationToken => CancellationToken.None;

        public TenantManager(ITenantStore<TTenant> store)
        {
            _store = store;
        }

        public Task<TTenant> FindByIdAsync(string id)
        {
            ThrowIfDisposed();
            return _store.FindByIdAsync(id, CancellationToken);
        }

        public Task<IdentityResult> CreateAsync(TTenant tenant)
        {
            ThrowIfDisposed();
            if (tenant == null)
            {
                throw new ArgumentNullException(nameof(tenant));
            }

            return _store.CreateAsync(tenant, CancellationToken);
        }

        public Task<IdentityResult> DeleteAsync(TTenant tenant)
        {
            ThrowIfDisposed();
            if (tenant == null)
            {
                throw new ArgumentNullException(nameof(tenant));
            }

            return _store.DeleteAsync(tenant, CancellationToken);
        }

        /// <summary>
        /// Releases all resources used by the user manager.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases the unmanaged resources used by the role manager and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing && !_disposed)
            {
                _store.Dispose();
                _disposed = true;
            }
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
    }
}

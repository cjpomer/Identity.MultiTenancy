using Microsoft.AspNetCore.Identity;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Identity.MultiTenancy
{
    public interface ITenantStore<TTenant> : IDisposable where TTenant : class
    {
        Task<IdentityResult> CreateAsync(TTenant tenant, CancellationToken cancellationToken);
        Task<IdentityResult> DeleteAsync(TTenant tenant, CancellationToken cancellationToken);
        Task<TTenant> FindByIdAsync(string id, CancellationToken cancellationToken);
    }
}
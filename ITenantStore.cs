using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Identity.MultiTenancy
{
    public interface ITenantStore<TTenant> where TTenant : class
    {
        Task<IdentityResult> CreateAsync(TTenant tenant);
        Task<IdentityResult> DeleteAsync(TTenant tenant);
        Task<TTenant> FindByIdAsync(string id);
    }
}
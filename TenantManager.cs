using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Identity.MultiTenancy
{
    public class TenantManager<TTenant> where TTenant : class
    {
        private readonly ITenantStore<TTenant> _store;
        public TenantManager(ITenantStore<TTenant> store)
        {
            _store = store;
        }

        public Task<TTenant> FindByIdAsync(string id)
        {
            return _store.FindByIdAsync(id);
        }

        public Task<IdentityResult> CreateAsync(TTenant tenant)
        {
            return _store.CreateAsync(tenant);
        }

        public Task<IdentityResult> DeleteAsync(TTenant tenant)
        {
            return _store.DeleteAsync(tenant);
        }
    }
}

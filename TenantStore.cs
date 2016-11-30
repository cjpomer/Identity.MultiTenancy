using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.MultiTenancy.EntityFramework;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Identity.MultiTenancy
{
    public class TenantStore<TKey> : ITenantStore<Tenant<TKey>>
        where TKey : IEquatable<TKey>
    {
        private readonly TenantIdentityDbContext<TKey> dbContext;

        public TenantStore(TenantIdentityDbContext<TKey> dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Tenant<TKey>> FindByIdAsync(string id)
        {
            return await dbContext.Tenants.FirstOrDefaultAsync(t => t.Id.Equals(id));
        }

        public async Task<IdentityResult> CreateAsync(Tenant<TKey> tenant)
        {
            dbContext.Tenants.Add(tenant);
            try
            {
                await dbContext.SaveChangesAsync();
                return IdentityResult.Success;
            }
            catch (Exception ex)
            {
                return IdentityResult.Failed(new IdentityError { Description = ex.Message });
            }
        }

        public async Task<IdentityResult> DeleteAsync(Tenant<TKey> tenant)
        {
            dbContext.Tenants.Remove(tenant);
            try
            {
                await dbContext.SaveChangesAsync();
                return IdentityResult.Success;
            }
            catch (Exception ex)
            {
                return IdentityResult.Failed(new IdentityError { Description = ex.Message });
            }
        }
    }
}

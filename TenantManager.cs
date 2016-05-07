using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.AspNet.Identity.MultiTenancy
{
    public class TenantManager<TTenant, TUser, TRole, TTenantKey, TUserKey>
        where TTenant : Tenant<TTenantKey>
        where TUser : TenantIdentityUser<TTenant, TTenantKey, TUserKey>
        where TTenantKey : IEquatable<TTenantKey>
        where TUserKey : IEquatable<TUserKey>
        where TRole : IdentityRole<TUserKey>
    {
        private TenantIdentityDbContext<TTenant, TUser, TRole, TTenantKey, TUserKey> dbContext;

        public TenantManager(TenantIdentityDbContext<TTenant, TUser, TRole, TTenantKey, TUserKey> dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<TTenant> FindByIdAsync(TTenantKey id)
        {
            return await dbContext.Tenants.FirstOrDefaultAsync(t => t.Id.Equals(id));
        }

        public async Task<IdentityResult> CreateAsync(TTenant tenant)
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

        public async Task<IdentityResult> DeleteAsync(TTenant tenant)
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

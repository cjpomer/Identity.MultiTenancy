using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using System.Threading;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Identity.MultiTenancy
{
    public class TenantUserManager<TTenant, TUser> : UserManager<TUser>
        where TUser : class
        where TTenant : class
    {
        ITenantUserStore<TTenant, TUser> _store;

        public TenantUserManager(ITenantUserStore<TTenant, TUser> store,
            IOptions<IdentityOptions> optionsAccessor,
            IPasswordHasher<TUser> passwordHasher,
            IEnumerable<IUserValidator<TUser>> userValidators,
            IEnumerable<IPasswordValidator<TUser>> passwordValidators,
            ILookupNormalizer keyNormalizer,
            IdentityErrorDescriber errors,
            IServiceProvider services,
            ILogger<UserManager<TUser>> logger)
            : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        {
            _store = store;
        }

        public async Task<IdentityResult> SetTenantAsync(TUser user, TTenant tenant)
        {
            ThrowIfDisposed();
            if (Store == null)
            {
                throw new ArgumentNullException(nameof(Store));
            }
            await _store.SetTenantAsync(user, tenant, CancellationToken);
            await UpdateSecurityStampAsync(user);
            return await UpdateAsync(user);
        }
    }
}

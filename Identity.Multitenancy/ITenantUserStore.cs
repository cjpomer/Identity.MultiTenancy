using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Identity.MultiTenancy
{
    public interface ITenantUserStore<TTenant, TUser> : IUserStore<TUser>
        where TUser : class
        where TTenant : class
    {
        Task SetTenantAsync(TUser user, TTenant tenant, CancellationToken cancellationToken);
    }
}

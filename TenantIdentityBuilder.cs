using Microsoft.AspNetCore.Identity.MultiTenancy.EntityFramework;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Microsoft.AspNetCore.Identity.MultiTenancy
{
    public class TenantIdentityBuilder : IdentityBuilder
    {
        public Type TenantType { get; }

        public TenantIdentityBuilder(Type tenant, Type user, Type role, IServiceCollection services) : base(user, role, services)
        {
            TenantType = tenant;
        }

        public virtual TenantIdentityBuilder AddTenantStore<TKey>()
            where TKey : IEquatable<TKey>
        {
            Services.AddScoped(typeof(ITenantStore<>).MakeGenericType(TenantType), typeof(Tenant<TKey>));
            return this;
        }
    }
}

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;

namespace Identity.MultiTenancy
{
    public class TenantIdentityBuilder : IdentityBuilder
    {
        public Type TenantType { get; private set; }

        public TenantIdentityBuilder(Type tenant, Type user, Type role, IServiceCollection services) : base(user, role, services)
        {
            TenantType = tenant;
        }
    }
}

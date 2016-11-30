using System;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.AspNetCore.Identity.MultiTenancy;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class TenantIdentityEntityFrameworkBuilderExtensions
    {
        /// <summary>
        /// Adds an Entity Framework implementation of identity information stores.
        /// </summary>
        /// <typeparam name="TContext">The Entity Framework database context to use.</typeparam>
        /// <typeparam name="TKey">The type of the primary key used for the users and roles.</typeparam>
        /// <param name="builder">The <see cref="IdentityBuilder"/> instance this method extends.</param>
        /// <returns>The <see cref="IdentityBuilder"/> instance this method extends.</returns>
        public static IdentityBuilder AddEntityFrameworkStores<TContext, TKey>(this TenantIdentityBuilder builder)
            where TContext : DbContext
            where TKey : IEquatable<TKey>
        {
            IdentityEntityFrameworkBuilderExtensions.AddEntityFrameworkStores<TContext, TKey>(builder);
            builder.Services.TryAdd(GetDefaultServices(builder.TenantType, typeof(TKey)));
            return builder;
        }

        private static IServiceCollection GetDefaultServices(Type tenantType, Type keyType)
        {
            Type tenantStoreType;
            tenantStoreType = typeof(TenantStore<>).MakeGenericType(keyType);

            var services = new ServiceCollection();
            services.AddScoped(
                typeof(ITenantStore<>).MakeGenericType(tenantType),
                tenantStoreType);
            return services;
        }
    }
}
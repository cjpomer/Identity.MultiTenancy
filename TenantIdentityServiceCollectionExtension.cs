// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.AspNetCore.Identity.MultiTenancy;
using Microsoft.AspNetCore.Builder;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Contains extension methods to <see cref="IServiceCollection"/> for configuring identity services.
    /// </summary>
    public static class TenantIdentityServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the default identity system configuration for the specified Tenant, User and Role types.
        /// </summary>
        /// <typeparam name="TTenant">The type representing a Tenant in the system.</typeparam>
        /// <typeparam name="TUser">The type representing a User in the system.</typeparam>
        /// <typeparam name="TRole">The type representing a Role in the system.</typeparam>
        /// <param name="services">The services available in the application.</param>
        /// <returns>An <see cref="IdentityBuilder"/> for creating and configuring the identity system.</returns>
        public static TenantIdentityBuilder AddIdentity<TTenant, TUser, TRole>(
            this IServiceCollection services)
            where TTenant : class
            where TUser : class
            where TRole : class
        {
            return services.AddIdentity<TTenant, TUser, TRole>(setupAction: null);
        }

        /// <summary>
        /// Adds and configures the identity system for the specified Tenant, User and Role types.
        /// </summary>
        /// <typeparam name="TTenant">The type representing a Tenant in the system.</typeparam>
        /// <typeparam name="TUser">The type representing a User in the system.</typeparam>
        /// <typeparam name="TRole">The type representing a Role in the system.</typeparam>
        /// <param name="services">The services available in the application.</param>
        /// <param name="setupAction">An action to configure the <see cref="IdentityOptions"/>.</param>
        /// <returns>An <see cref="IdentityBuilder"/> for creating and configuring the identity system.</returns>
        public static TenantIdentityBuilder AddIdentity<TTenant, TUser, TRole>(
            this IServiceCollection services,
            Action<IdentityOptions> setupAction)
            where TTenant : class
            where TUser : class
            where TRole : class
        {
            services.TryAddScoped<TenantManager<TTenant>, TenantManager<TTenant>>();
            services.AddIdentity<TUser, TRole>(setupAction);
            return new TenantIdentityBuilder(typeof(TTenant), typeof(TUser), typeof(TRole), services);
        }
    }
}
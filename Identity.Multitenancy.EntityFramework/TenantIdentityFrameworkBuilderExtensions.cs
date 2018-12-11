using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Identity.MultiTenancy.EntityFramework
{
    public static class TenantIdentityFrameworkBuilderExtensions
    {
        public static TenantIdentityBuilder AddEntityFrameworkStores<TContext>(this TenantIdentityBuilder builder) where TContext : DbContext
        {
            var keyType = InferKeyType(typeof(TContext));

            builder.Services.AddScoped(
                typeof(ITenantStore<>).MakeGenericType(builder.TenantType),
                typeof(TenantStore<,,>).MakeGenericType(typeof(TContext), builder.TenantType, keyType));
            builder.Services.AddScoped(
                typeof(ITenantUserStore<,>).MakeGenericType(builder.TenantType, builder.UserType),
                typeof(TenantUserStore<,,,,>).MakeGenericType(builder.TenantType, builder.UserType, builder.RoleType, typeof(TContext), keyType));
            builder.Services.AddScoped(
                typeof(IUserStore<>).MakeGenericType(builder.UserType),
                typeof(TenantUserStore<,,,,>).MakeGenericType(builder.TenantType, builder.UserType, builder.RoleType, typeof(TContext), keyType));
            builder.Services.AddScoped(
                typeof(IRoleStore<>).MakeGenericType(builder.RoleType),
                typeof(RoleStore<,,>).MakeGenericType(builder.RoleType, typeof(TContext), InferKeyType(typeof(TContext))));

            return builder;
        }

        private static Type InferKeyType(Type contextType)
        {
            var type = contextType.GetTypeInfo();
            while (type.BaseType != null)
            {
                type = type.BaseType.GetTypeInfo();
                var genericType = type.IsGenericType ? type.GetGenericTypeDefinition() : null;
                if (genericType != null && genericType == typeof(IdentityDbContext<,,>))
                {
                    return type.GenericTypeArguments[2];
                }
            }
            // Default is string
            return typeof(string);
        }
    }
}

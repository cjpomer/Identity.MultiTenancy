using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;

namespace Microsoft.AspNetCore.Identity.MultiTenancy.EntityFramework
{
    public class TenantIdentityUser : TenantIdentityUser<string>
    {
        public TenantIdentityUser() 
        {
        }

        public TenantIdentityUser(string userName) : base(userName)
        {
        }
    }

    public class TenantIdentityUser<TKey> : IdentityUser<TKey>
        where TKey : IEquatable<TKey>
    {
        virtual public TKey TenantId { get; set; }

        public TenantIdentityUser()
        {
        }

        public TenantIdentityUser(string userName)
        {
            UserName = userName;
        }

        virtual public Tenant<TKey> Tenant { get; set; }
    }
}

using Microsoft.AspNetCore.Identity;
using System;

namespace Identity.MultiTenancy.EntityFramework
{
    public class TenantIdentityUser<TKey> : IdentityUser<TKey> where TKey : IEquatable<TKey>
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

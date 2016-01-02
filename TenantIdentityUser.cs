using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.AspNet.Identity.EntityFramework
{
    public class TenantIdentityUser : TenantIdentityUser<Tenant, string, string>
    {
        public TenantIdentityUser() 
        {
        }

        public TenantIdentityUser(string userName) : base(userName)
        {
        }
    }

    public class TenantIdentityUser<TTenant, TTenantKey> : TenantIdentityUser<TTenant, TTenantKey, string>
        where TTenant : Tenant<TTenantKey>
        where TTenantKey : IEquatable<TTenantKey>
    {
        public TenantIdentityUser()
        {
            Id = Guid.NewGuid().ToString();
        }

        public TenantIdentityUser(string userName) : base(userName)
        {
        }
    }

    public class TenantIdentityUser<TTenant, TTenantKey, TUserKey> : IdentityUser<TUserKey>
        where TTenant : Tenant<TTenantKey>
        where TTenantKey : IEquatable<TTenantKey>
        where TUserKey : IEquatable<TUserKey>
    {
        virtual public TTenantKey TenantId { get; set; }

        public TenantIdentityUser() : base()
        {
        }

        public TenantIdentityUser(string userName) : base(userName)
        {
        }

        virtual public TTenant Tenant { get; set; }
    }
}

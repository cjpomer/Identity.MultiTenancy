using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.AspNet.Identity.EntityFramework
{
    public class TenantIdentityUser : TenantIdentityUser<string, string>
    {
    }

    public class TenantIdentityUser<TTenantKey, TUserKey> : IdentityUser<TUserKey>
        where TTenantKey : IEquatable<TTenantKey>
        where TUserKey : IEquatable<TUserKey>
    {
        virtual public TTenantKey TenantId { get; set; }

        virtual public Tenant<TTenantKey> Tenant { get; set; }
    }
}

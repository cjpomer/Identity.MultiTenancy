using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Identity.MultiTenancy.EntityFramework
{
    public class Tenant : Tenant<string>
    {

    }

    public class Tenant<TTenantKey> where TTenantKey : IEquatable<TTenantKey>
    {
        virtual public TTenantKey Id { get; set; }
        virtual public string Name { get; set; }
    }
}

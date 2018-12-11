using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.MultiTenancy.EntityFramework
{
    public class Tenant : Tenant<string>
    {

    }

    public class Tenant<TKey> where TKey : IEquatable<TKey>
    {
        virtual public TKey Id { get; set; }
        virtual public string Name { get; set; }
    }
}

# Identity.MultiTenancy
aspnet/Identity extended for multitenancy

In aspnet/Identity (https://github.com/aspnet/Identity), Microsoft provides version 3 of their Identity framework for ASP.NET.  It is part of ASP.NET v5.

The out-of-the-box implementation provided is implemented with Entity Framework.

Implementations are hidden behind 3 classes consumers are encouraged to use:  `UserManager<TUser>`, `RoleManager<TRole>`, and `SignInManager<TUser>`.

MultiTenancy can be added without changing the UserManager facade.  `UserManager<TUser>` consumes an `IUserStore<TUser>`.  Identity.MultiTenancy implements an `IUserStore<TUser>` called `TenantUserStore<TUser>` where `TUser` : `TenantUser`.  It is implemented using Entity Framework.

The key to using this library is instantiating `UserManager<TUser>` with the added `TenantUserStore<TUser>`:
```c#
return new UserManager<TenantUser>(
  new TenantUserStore<Tenant, TenantUser, IdentityRole, dbContext>(
    isp.GetService<dbContext>(), 
    tenantId),
  isp.GetService<IOptions<IdentityOptions>>(), 
  isp.GetService<IPasswordHasher<TenantUser>>(), 
  isp.GetService<IEnumerable<IUserValidator<TenantUser>>>(), 
  isp.GetService<IEnumerable<IPasswordValidator<TenantUser>>>(), 
  isp.GetService<ILookupNormalizer>(),
  isp.GetService<IdentityErrorDescriber>(), 
  isp,
  isp.GetService<ILogger<UserManager<TenantUser>>>(),
  isp.GetService<IHttpContextAccessor>());
});
```
Not that the `TenantUserStore<Tenant, TenantUser, IdentityRole, dbContext>` ctor takes the Tenant ID.  By default the Tenant ID is of type string.

Because ASP.NET v6 uses DI, it is convenient to provide a factory to get instances of the `UserManager<TUser>` given a Tenant ID.  This can be done by registering such a factory in Startup.cs:
```c#
public delegate UserManager<CazadorUser> UserManagerFactory(string tenantId);
...
public void ConfigureServices(IServiceCollection services)
{
  // Add framework services.
  services.AddApplicationInsightsTelemetry(Configuration);

  services.AddEntityFramework()
      .AddSqlServer()
      .AddDbContext<TenantDbContext>(options =>
          options.UseSqlServer(Configuration["Data:DefaultConnection:ConnectionString"]));

  services.AddIdentity<TenantUser, IdentityRole>()
      .AddEntityFrameworkStores<TenantDbContext>()
      .AddDefaultTokenProviders();

  services.TryAddScoped(typeof(UserManagerFactory), isp =>
  {
      return (UserManagerFactory)((string tenantId) =>
      {
          return new UserManager<TenantUser>(
              new TenantUserStore<Tenant, TenantUser, IdentityRole, TenantDbContext>(isp.GetService<TenantDbContext>(), tenantId),
              isp.GetService<IOptions <IdentityOptions>>(), 
              isp.GetService<IPasswordHasher<TenantUser>>(), 
              isp.GetService<IEnumerable<IUserValidator<TenantUser>>>(), 
              isp.GetService<IEnumerable<IPasswordValidator<TenantUser>>>(), 
              isp.GetService<ILookupNormalizer>(),
              isp.GetService<IdentityErrorDescriber>(), 
              isp,
              isp.GetService<ILogger<UserManager<TenantUser>>>(),
              isp.GetService<IHttpContextAccessor>());
      });
  });
  ...
```

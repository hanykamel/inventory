using AutoMapper;
using Hangfire;
using inventory.Engines.LdapAuth.Entities;
using inventory.Engines.LdapAuth.LdapAuthConfig;
using Inventory.CrossCutting.NotificationHub;
using Inventory.Data;
using Inventory.Service.Interfaces;
using Inventory.Service.Mapping;
using Inventory.Web;
using Inventory.Web.Extensions;
using Inventory.Web.Helpers;
using Inventory.Web.Middlewares;
using MediatR;
using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Server.IISIntegration;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.OData.Edm;
using Newtonsoft.Json.Serialization;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
namespace Inventory
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }


        private static IEdmModel GetEdmModel()
        {
            ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
            builder.AddOdataApiConfig();
            return builder.GetEdmModel();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<LdapConfig>(this.Configuration.GetSection("ldap"));

            services.AddSignalR();

            services.Configure<FormOptions>(options =>
            {
                options.ValueCountLimit = int.MaxValue;
            });

            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);

            services.AddLocalization(o => o.ResourcesPath = "Resources");
            services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[]
                {
                    new CultureInfo("ar-EG"),

                };
                options.DefaultRequestCulture = new RequestCulture("ar-EG");

                // You must explicitly state which cultures your application supports.
                // These are the cultures the app supports for formatting 
                // numbers, dates, etc.

                options.SupportedCultures = supportedCultures;

                // These are the cultures the app supports for UI strings, 
                // i.e. we have localized resources for.

                options.SupportedUICultures = supportedCultures;
            });

            services.AddMvc(config =>
            {
                // Requiring authenticated users on the site globally
                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()

                    // You can chain more requirements here
                    // .RequireRole(...) OR
                    // .RequireClaim(...) OR
                    // .Requirements.Add(...)         

                    .Build();
                config.Filters.Add(new AuthorizeFilter(policy));
                //config.Filters.Add(new Class4());
                config.EnableEndpointRouting = false;
            })
                .AddJsonOptions(
              options =>
              {
                  options.SerializerSettings
                  .ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                  if (options.SerializerSettings.ContractResolver != null)
                  {
                      var resolver = options.SerializerSettings.ContractResolver
                      as DefaultContractResolver;
                      resolver.NamingStrategy = null;
                  }
              })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            var cookiesConfig = this.Configuration.GetSection("cookies").Get<CookiesConfig>();

            services.AddAuthentication
                (options =>
               {
                   options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                   options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                   options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
               }
               )
                .AddCookie(options =>
                {
                    options.Cookie.Name = cookiesConfig.CookieName;
                    options.LoginPath = cookiesConfig.LoginPath;
                    options.LogoutPath = cookiesConfig.LogoutPath;
                    options.AccessDeniedPath = cookiesConfig.AccessDeniedPath;
                    options.SlidingExpiration = true;
                    options.ReturnUrlParameter = cookiesConfig.ReturnUrlParameter;
                });

            services.AddSession();

            services.AddAuthorization(options =>
            {
                options.AddPolicy(InventoryAuthorizationPolicy.SystemAdmin,
                     policy => policy.RequireClaim(ClaimTypes.Role, UserGroups.SystemAdmin));
                options.AddPolicy(InventoryAuthorizationPolicy.TechnicalDepartments,
                   policy => policy.RequireClaim(ClaimTypes.Role, UserGroups.TechnicalDepartments));
                options.AddPolicy(InventoryAuthorizationPolicy.StoreKeeper,
                   policy => policy.RequireClaim(ClaimTypes.Role, UserGroups.StoreKeeper));
                options.AddPolicy(InventoryAuthorizationPolicy.WarehouseManager,
                   policy => policy.RequireClaim(ClaimTypes.Role, UserGroups.WarehouseManager));


                options.AddPolicy(InventoryAuthorizationPolicy.ViewADStoreAdmins,
                   policy => policy.RequireClaim(ClaimTypes.Role, ClaimTypes.Role, UserGroups.SystemAdmin, UserGroups.WarehouseManager, UserGroups.StoreKeeper));


                options.AddPolicy(InventoryAuthorizationPolicy.AllValidRoles,
                    policy => policy.RequireClaim(ClaimTypes.Role, UserGroups.SystemAdmin, UserGroups.TechnicalDepartments, UserGroups.WarehouseManager, UserGroups.StoreKeeper,UserGroups.AssistantTechnicalDepartments));

                options.AddPolicy(InventoryAuthorizationPolicy.SystemLookups,
                    policy => policy.RequireClaim(ClaimTypes.Role, UserGroups.StoreKeeper,
                    UserGroups.SystemAdmin));

                options.AddPolicy(InventoryAuthorizationPolicy.AddEmployees,
                    policy => policy.RequireClaim(ClaimTypes.Role, UserGroups.StoreKeeper, UserGroups.TechnicalDepartments,
                    UserGroups.SystemAdmin));

                options.AddPolicy(InventoryAuthorizationPolicy.AllTransactions,
                    policy => policy.RequireClaim(ClaimTypes.Role, UserGroups.StoreKeeper, UserGroups.TechnicalDepartments));

                options.AddPolicy(InventoryAuthorizationPolicy.ViewData,
                   policy => policy.RequireClaim(ClaimTypes.Role, UserGroups.TechnicalDepartments, UserGroups.WarehouseManager, UserGroups.StoreKeeper,UserGroups.AssistantTechnicalDepartments));

                options.AddPolicy(InventoryAuthorizationPolicy.ChangeTenant,
                 policy => policy.RequireClaim(ClaimTypes.Role, UserGroups.TechnicalDepartments, UserGroups.StoreKeeper, UserGroups.AssistantTechnicalDepartments));

            });
            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });

            services.AddDbContext<InventoryContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddOData();

            //to get assembly containing the commands and handlers
            services.AddMediatR(typeof(Inventory.Service.Entities.UserRequest.Commands.LoggedInUserCommand).Assembly);

            services.AddAuthentication(IISDefaults.AuthenticationScheme);

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddEntitiesScope();

            services.AddHangfire(configuration => configuration
        .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
        .UseSimpleAssemblyNameTypeSerializer()
        .UseRecommendedSerializerSettings()
        .UseSqlServerStorage(Configuration.GetConnectionString("DefaultConnection")));

            services.AddHangfireServer();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        [System.Obsolete]
        public void Configure(IApplicationBuilder app,
            IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }
            loggerFactory.AddLog4Net();

            app.UseStaticFiles();
            app.UseSpaStaticFiles();
            app.UseHangfireServer();
            app.UseSignalR(routes =>
            {
                routes.MapHub<NotificationHub>("/notificationHub");
            });

            app.UseCookiePolicy();
            app.UseSession();
            app.UseAuthentication();
           // app.UseRefreshToken(150);
            app.UseRequestLocalization();
            app.UseMiddleware(typeof(ExceptionHandlingMiddleware));
            RecurringJob.AddOrUpdate<IExchangeOrderBussiness>(
             ExchangeOrder => ExchangeOrder.CancelExchangeOrder_Service(),
             Cron.MinuteInterval(5));

            app.UseMvc(routes =>
            {
                routes.EnableDependencyInjection();
                routes.Select().Filter().Expand().Count().OrderBy().MaxTop(1000);
                routes.MapODataServiceRoute("odata", "odata", GetEdmModel());
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });
        }
    }
}

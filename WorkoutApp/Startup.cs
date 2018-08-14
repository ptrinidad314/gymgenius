using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using WorkoutApp.Data;
using WorkoutApp.Security;
using WorkoutApp.Services;

namespace WorkoutApp
{
    public class Startup
    {
        private IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddScoped<IWorkoutRepository, WorkoutRepository>();
            services.AddScoped<IUtilities, Utilities>();

            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            var connectionString = _configuration["ConnectionStrings:WorkoutDBConnectionLocal"]; 
            services.AddDbContext<WorkoutContext>(o=>o.UseSqlServer(connectionString));

            services.AddAuthentication(options=> {
                options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
                .AddOpenIdConnect(options=> {
                    options.Authority = _configuration["AzureAd:Authority"]; 
                    options.ClientId = _configuration["AzureAd:ClientId"]; 
                    options.ResponseType = OpenIdConnectResponseType.IdToken;
                    options.CallbackPath = _configuration["AzureAd:CallbackPath"];
                    options.SignedOutRedirectUri = _configuration["AzureAd:SignedOutRedirectUriLocal"];
                    options.TokenValidationParameters.NameClaimType = "name";
                })
                .AddCookie();

            services.AddAuthorization(options=> 
            {
                options.AddPolicy("RequireElevatedRights", policy=> {
                    policy.AddRequirements(new RequireElevatedRights(_configuration["AzureAd:AdminObjectId"], new HttpContextAccessor()));
                });

            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
                app.UseExceptionHandler();

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc(ConfigureRoutes);

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Hello World!");
            });
        }

        private void ConfigureRoutes(IRouteBuilder routeBuilder)
        {
            routeBuilder.MapRoute("Default", "{controller=Home}/{action=Index}/{id?}");
        }

    }
}

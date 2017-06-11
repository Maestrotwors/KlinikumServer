using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using System.Reflection.Metadata;

namespace MyKlinikumCore
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; set; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        { 
            //var routeBuilder = new RouteBuilder(app);
            //routeBuilder.MapRoute("default", "{Controller=Main}/{Action=Index}");
            // строим маршрут
            //app.UseRouter(routeBuilder.Build());
            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute("login", "login", new { controller = "Auth", action = "Login" });
                routes.MapRoute("logout", "logout", new { controller = "Auth", action = "Logout" });
                routes.MapRoute( 
                    name: "Main",
                    template: "{Controller=Main}/{Action=Index}");
                routes.MapRoute(
                    name: "Default",
                    template: "{Controller=Main}/{Action=Index}");
                //defaults: new { controller = "Main", action = "Index" });
                routes.MapRoute(
                    name: "Api",
                    template: "{Controller=Api}/{Action=Index}");

                routes.MapRoute("Patients", "patients", new { controller = "Main", action = "Index" });
                routes.MapRoute("Patient", "patient/{id}", new { controller = "Main", action = "Index" });
                routes.MapRoute("PatientNew", "patient/new", new { controller = "Main", action = "Index" });
                routes.MapRoute("Notfalls", "notfalls", new { controller = "Main", action = "Index" });
                routes.MapRoute("Notfall", "notfalls/{NotfallId}", new { controller = "Main", action = "Index" });
                routes.MapRoute("Statistik", "statistik", new { controller = "Main", action = "Index" });
                routes.MapRoute("NotfallNew", "notfall/new", new { controller = "Main", action = "Index" });
                routes.MapRoute("Settings", "settings", new { controller = "Main", action = "Index" });
            });
        }
    }
}

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SqlServerWebDemo.Models;
using Steeltoe.CloudFoundry.Connector;
using Steeltoe.CloudFoundry.Connector.Services;
using Steeltoe.CloudFoundry.Connector.SqlServer;
using Steeltoe.Extensions.Configuration.CloudFoundry;

namespace SqlServerWebDemo
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        
        private IConfiguration Configuration { get; }
        
        private string DbConnectionString()
        {
            // The Steeltoe Microsoft SQL Server connector expects a `database` key in the service instance
            // credentials blob, which is not provided by our service broker, so we have to do a little bit
            // of extra work here to intercept the automagically discovered database connection string and
            // append it with our desired database name.
            var serviceInfo = Configuration.GetSingletonServiceInfo<SqlServerServiceInfo>();
            var connectionOptions = new SqlServerProviderConnectorOptions(Configuration);
            var sqlProviderFactory = new SqlServerProviderConnectorFactory(serviceInfo, connectionOptions, null);
            var connection = sqlProviderFactory.CreateConnectionString();
            if (!connection.Contains("Initial Catalog="))
            {
                // TODO: Allow database name to be overridden from the environment?
                connection += "Initial Catalog=EFDemoDB;";
            }
            return connection;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.ConfigureCloudFoundryOptions(Configuration);
            
            services.AddDbContext<DemoContext>(options => options.UseSqlServer(DbConnectionString()));
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
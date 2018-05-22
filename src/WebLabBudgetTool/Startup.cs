using System;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using EntityFramework.DbContextScope;
using EntityFramework.DbContextScope.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebLabBudgetTool.DataService;

namespace WebLabBudgetTool
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IContainer ApplicationContainer { get; private set; }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            // We have to set the connection string twice here to init the context afterwards correctly for the ambient context.
            var connectionString = Configuration.GetConnectionString("DefaultConnection");
            ApplicationContext.ConnectionString = connectionString;
            services.AddDbContext<ApplicationContext>(options => options.UseMySQL(connectionString));

            // Create the container builder.
            var builder = new ContainerBuilder();

            builder.RegisterType<AmbientDbContextLocator>().As<IAmbientDbContextLocator>();
            builder.RegisterType<DbContextScopeFactory>().As<IDbContextScopeFactory>();
            builder.RegisterType<AccountDataService>().As<IAccountDataService>();
            builder.RegisterInstance(Configuration);

            builder.Populate(services);
            this.ApplicationContainer = builder.Build();

            // Create the IServiceProvider based on the container.
            return new AutofacServiceProvider(this.ApplicationContainer);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApplicationLifetime appLifetime)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
            appLifetime.ApplicationStopped.Register(() => this.ApplicationContainer.Dispose());
        }
    }
}

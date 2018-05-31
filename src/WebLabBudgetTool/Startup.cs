using System;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using Autofac.Core;
using Autofac.Extensions.DependencyInjection;
using EntityFramework.DbContextScope;
using EntityFramework.DbContextScope.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using WebLabBudgetTool.DataService;
using WebLabBudgetTool.Entities;

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
            services.Configure<JWTSettings>(Configuration.GetSection("JWTSettings"));
            // We have to set the connection string twice here to init the context afterwards correctly for the ambient context.
            var connectionString = Configuration.GetConnectionString("DefaultConnection");
            ApplicationContext.ConnectionString = connectionString;
            services.AddDbContext<ApplicationContext>(options => options.UseMySQL(connectionString));

            services.AddIdentity<AppUser, AppRole>()
                    .AddEntityFrameworkStores<ApplicationContext>()
                    .AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(options =>
                {
                    // avoid redirecting REST clients on 401
                    options.Events = new CookieAuthenticationEvents
                    {
                        OnRedirectToLogin = ctx =>
                        {
                            ctx.Response.StatusCode = (int) HttpStatusCode.Unauthorized;
                            return Task.FromResult(0);
                        }
                    };
                }
            );

            services.AddAuthentication(o => { o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; })
                    .AddJwtBearer(options =>
                    {
                        options.RequireHttpsMetadata = false;
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidateAudience = true,
                            ValidateLifetime = true,
                            ValidateIssuerSigningKey = true,
                            ValidIssuer = "weblab_budget_tool",
                            ValidAudience = "weblab_budget_tool",
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("weblab-is-awesome"))
                        };
                    });

            services.AddCors();
            
            services.AddCors(options =>
            {
                options.AddPolicy("AllowEverything",
                                  policyBuilder => policyBuilder.AllowAnyOrigin()
                                  .AllowAnyHeader()
                                  .AllowAnyMethod());
            });

            services.AddMvc();

            services.Configure<Microsoft.AspNetCore.Mvc.MvcOptions>(options =>
            {
                options.Filters.Add(new Microsoft.AspNetCore.Mvc.Cors.Internal.CorsAuthorizationFilterFactory("AllowEverything"));
            });

            // Create the container builder.
            var builder = new ContainerBuilder();

            builder.RegisterType<AmbientDbContextLocator>().As<IAmbientDbContextLocator>();
            builder.RegisterType<DbContextScopeFactory>().As<IDbContextScopeFactory>();

            builder.RegisterAssemblyTypes(typeof(AccountDataService).Assembly)
                   .Where(t => t.Name.EndsWith("Service", StringComparison.OrdinalIgnoreCase))
                   .AsImplementedInterfaces();

            builder.RegisterInstance(Configuration);

            builder.Populate(services);
            this.ApplicationContainer = builder.Build();
            
            // Create the IServiceProvider based on the container.
            return new AutofacServiceProvider(this.ApplicationContainer);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public async void Configure(IApplicationBuilder app, IHostingEnvironment env, IApplicationLifetime appLifetime)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAuthentication();
            app.UseCors("AllowEverything");

            app.UseMvc();
            appLifetime.ApplicationStopped.Register(() => this.ApplicationContainer.Dispose());
            await StartClearJob();
        }

        private async Task StartClearJob()
        {
            await Task.Run(async () => {
                while (true)
                {
                    try
                    {
                        var paymentDataServices = ApplicationContainer.Resolve<IPaymentDataService>();
                        var payments = (await paymentDataServices.GetUnclearedPayments(DateTime.Now)).ToList();

                        foreach (var payment in payments)
                        {
                            payment.ClearPayment();
                        }

                        await paymentDataServices.SavePayments(payments.ToArray());

                    }
                    catch (Exception ex)
                    {
                    }
                    finally
                    {
                        // Sleep for 3 hours
                        Thread.Sleep(new TimeSpan(3, 0, 0));
                    }

                }

            });
        }
    }
}

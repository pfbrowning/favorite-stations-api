using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using FavoriteStations.Config;
using FavoriteStations.Data;
using FavoriteStations.Services;
using FavoriteStations.Models;

namespace FavoriteStations.API {
    public class Startup {
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {
            IConfigurationSection authConfigSection = Configuration.GetSection("Authentication");
            AuthConfig authConfig = authConfigSection.Get<AuthConfig>();

            services.AddControllers();
            // Configure EF DB Context
            services.AddDbContext<FavoriteStationsContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            // Add Http Context to the pipeline for DI
            services.AddHttpContextAccessor();
            // Configure a typed User object based on access token claims
            services.AddScoped<User>(s => new User(s.GetService<IHttpContextAccessor>().HttpContext.User));
            
            services.AddScoped<IDataLayer, DataLayer>();
            
            // Configure JWT Bearer token authentication
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options => {
                    options.Authority = authConfig.Authority;
                    options.Audience = authConfig.Audience;
                });
        }
        
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            /* Fix default claim mapping because "Microsoft knows best"
            https://leastprivilege.com/2016/08/21/why-does-my-authorize-attribute-not-work/ */
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

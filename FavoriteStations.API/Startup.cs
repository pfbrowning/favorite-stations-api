using System;
using System.Diagnostics.CodeAnalysis;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using FavoriteStations.Config;
using FavoriteStations.Data;
using FavoriteStations.Services;
using FavoriteStations.Models;
using FavoriteStations.Mapping;
using FavoriteStations.Middlewares;
using AutoMapper;
using Serilog;

namespace FavoriteStations.API {
    [ExcludeFromCodeCoverage]
    public class Startup {
        public Startup(IConfiguration configuration) {
            Configuration = configuration;

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(Configuration)
                .CreateLogger();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {
            IConfigurationSection authConfigSection = Configuration.GetSection("Authentication");
            IConfigurationSection corsConfigSection = Configuration.GetSection("Cors");
            AuthConfig authConfig = authConfigSection.Get<AuthConfig>();
            CorsConfig corsConfig = corsConfigSection.Get<CorsConfig>();

            // Configure CORS based on the origins specified in our configuration
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder
                    .WithOrigins(corsConfig.AllowedCorsOrigins)
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials() );
            });

            services.AddControllers();
            // Configure EF DB Context
            services.AddDbContext<FavoriteStationsContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            // Add Http Context to the pipeline for DI
            services.AddHttpContextAccessor();
            // Configure a typed User object based on access token claims
            services.AddScoped<IUser>(s => new User(s.GetService<IHttpContextAccessor>().HttpContext.User));
            
            services.AddScoped<IDataLayer, DataLayer>();
            services.AddScoped<IBusinessLayer, BusinessLayer>();

            // Auto Mapper Configurations
            var mappingConfig = new MapperConfiguration(mc => {
                mc.AddProfile(new MappingProfile());
            });

            services.AddSingleton(mappingConfig.CreateMapper());

            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new OpenApiInfo {
                    Title = "Favorite Stations API",
                    Description = "A simple RESTful CRUD api used to persist users' favorite internet radio stations",
                    Version = "v1"
                });

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
            });
            
            /* Disable automatic model validation so that we can use our own custom ValidateModelAttribute.
            https://stackoverflow.com/questions/51125569/net-core-2-1-override-automatic-model-validation */
            services.Configure<ApiBehaviorOptions>(opts => opts.SuppressModelStateInvalidFilter = true);
            
            // Configure JWT Bearer token authentication
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options => {
                    options.Authority = authConfig.Authority;
                    options.Audience = authConfig.Audience;
                    options.Events = new JwtBearerEvents() {
                        OnChallenge = c => {
                            c.HandleResponse();
                            return c.HttpContext.WriteStatusCodeResponse(401);
                        }
                    };
                });
        }
        
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseExceptionHandler(env.IsDevelopment());

            /* Fix default claim mapping because "Microsoft knows best"
            https://leastprivilege.com/2016/08/21/why-does-my-authorize-attribute-not-work/ */
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            // Configure Serilog to listen to the global .NET Core logging pipeline
            loggerFactory.AddSerilog();
            
            app.UseCors("CorsPolicy");

            app.UseSwagger();
            app.UseSwaggerUI(c => {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Favorite Stations API V1");
                c.RoutePrefix = string.Empty;
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseStatusCodeResponses();
        }
    }
}

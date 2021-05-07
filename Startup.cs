using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using DigitalProductAPI.Contexts;
using DigitalProductAPI.Filters;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace DigitalProductAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpClient();
            services.AddCors(setupAction =>
            {
                setupAction.AddDefaultPolicy(p =>
                {
                    p.AllowAnyOrigin();
                    p.AllowAnyHeader();
                    p.AllowAnyMethod();
                });
            });
            services.AddControllers(opts =>
            {
                opts.Filters.Add(new AuthorizeFilter());
            });
            services.AddMvc()
                .AddXmlDataContractSerializerFormatters();

            var connString = Configuration["ConnectionString:mssqlConnString"];
            services.AddDbContext<SecuritiesContext>(options => 
            {
                options.UseSqlServer(connString);
            });

            services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
               .AddIdentityServerAuthentication(options =>
               {
                   options.Authority = Configuration["Authority"];
                   options.ApiName = Configuration["API-NAME"];
                   options.ApiSecret = Configuration["API-SECRET"];
               });

            services.AddSwaggerGen(setupAction =>
            {
                setupAction.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Contact = new Microsoft.OpenApi.Models.OpenApiContact
                    {
                        Email = "appsupport@fsdhgroup.com",
                        Name = "Application Support",

                    },
                    Description = "A set of API'S for FSDH Digital Product",
                    Title = "Digital Product"
                });
                setupAction.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows
                    {
                        ClientCredentials = new OpenApiOAuthFlow
                        {
                            AuthorizationUrl = new Uri($"{Configuration["Authority"]}/connect/authorize"),
                            TokenUrl = new Uri($"{Configuration["Authority"]}/connect/token"),
                            Scopes = new Dictionary<string, string>
                            {
                                {"digital-api-scope", "DIGITAL PRODUCT API"}
                            }
                        }
                    },
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                });

                setupAction.OperationFilter<AuthenticationRequirementsOperationFilter>();
                setupAction.OperationFilter<AuthorizeCheckOperationFilter>();

                //var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                //var xmlFilePath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                //setupAction.IncludeXmlComments(xmlFilePath);
            });
            //services.AddScoped<ISecuritiesRespository, SecuritiesRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {            

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(c =>
            {
                c.AllowAnyOrigin();
                c.AllowAnyMethod();
                c.AllowAnyHeader();
            });

            app.UseHttpsRedirection();
            app.UseSwagger();

            app.UseSwaggerUI(setupAction =>
            {
                setupAction.DocumentTitle = "DIGITAL PRODUCT API";
                setupAction.SwaggerEndpoint("/swagger/v1/swagger.json", "DIGITAL PRODUCT API");
                setupAction.RoutePrefix = string.Empty;
                setupAction.OAuthClientId(Configuration["ClientId"]);
                setupAction.OAuthClientSecret(Configuration["ClientSecret"]);
            });
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

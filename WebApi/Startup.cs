﻿using System.IdentityModel.Tokens.Jwt;
using AppAuthorizationService;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Logging;
using Microsoft.OpenApi.Models;

namespace WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var stsServer = Configuration["StsServer"];
            services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
              .AddIdentityServerAuthentication(options =>
              {
                  options.Authority = stsServer;
                  options.ApiName = "NativeAPI";
                  options.ApiSecret = "native_api_secret";
                  options.RequireHttpsMetadata = true;
              });

            services.AddHttpContextAccessor();

            services.AddSingleton<IAuthorizationHandler, ValuesCheckQueryParameterHandler>();
            services.AddSingleton<IAuthorizationHandler, ValuesCheckRequestBodyHandler>();
            services.AddSingleton<IAuthorizationHandler, ValuesCheckRouteParameterHandler>();

            services.AddAuthorization(options =>
            {
                options.AddPolicy("protectedScope", policy =>
                {
                    policy.RequireClaim("scope", "native_api");
                });
                options.AddPolicy("ValuesRoutePolicy", valuesRoutePolicy =>
                {
                    valuesRoutePolicy.Requirements.Add(new ValuesRouteRequirement());
                });
                options.AddPolicy("ValuesQueryPolicy", valuesQueryPolicy =>
                {
                    valuesQueryPolicy.Requirements.Add(new ValuesCheckQueryParamRequirement());
                });
                options.AddPolicy("ValuesRequestBodyCheckPolicy", valuesRequestBodyCheckPolicy =>
                {
                    valuesRequestBodyCheckPolicy.Requirements.Add(new ValuesRequestBodyRequirement());
                });
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Payload View API",
                });
            });

            services.AddControllers().AddNewtonsoftJson();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            IdentityModelEventSource.ShowPII = true;

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //Registered before static files to always set header
            app.UseHsts(hsts => hsts.MaxAge(365).IncludeSubdomains());
            app.UseXContentTypeOptions();
            app.UseReferrerPolicy(opts => opts.NoReferrer());
            app.UseXXssProtection(options => options.EnabledWithBlockMode());
            app.UseXfo(options => options.Deny());

            app.UseCsp(opts => opts
                .BlockAllMixedContent()
                .StyleSources(s => s.Self())
                .StyleSources(s => s.UnsafeInline())
                .FontSources(s => s.Self())
                .FormActions(s => s.Self())
                .FrameAncestors(s => s.Self())
                .ImageSources(s => s.Self())
                .ScriptSources(s => s.Self())
                .ScriptSources(s => s.UnsafeInline())
            );

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
            });
        }
    }
}

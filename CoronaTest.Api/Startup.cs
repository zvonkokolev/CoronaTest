using CoronaTest.Core.Interfaces;
using CoronaTest.Core.Services;
using CoronaTest.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using StringRandomizer;
using System;
using System.Collections.Generic;
using System.Text;

namespace CoronaTest.Api
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
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = Configuration["Jwt:Issuer"],

                    ValidateAudience = true,
                    ValidAudience = Configuration["Jwt:Audience"],

                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(Configuration["Jwt:SecretKey"])),

                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });

            services.AddSwaggerGen(config =>
            {
                config.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "CoronaTest Api",
                    Description = "Die API liefert alle wichtigen Infos zu den Corona Tests",
                    Contact = new OpenApiContact
                    {
                        Name = "Zvonko Kolev",
                        Email = "zoni_ko@yahoo.com"
                    },
                    License = new OpenApiLicense
                    {
                        Name = "MIT"
                    }
                });

                config.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.ApiKey,
                    Name = "Authorization",
                    Description = "Type into the textbox: Bearer {your JWT token}",
                    In = ParameterLocation.Header
                });

                config.AddSecurityRequirement(new OpenApiSecurityRequirement { {
                    new OpenApiSecurityScheme {
                    Reference = new OpenApiReference {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                    },
                    new List<string>()
                }});
            });

            services.AddTransient<IUnitOfWork, UnitOfWork>();
            //services.AddScoped<IUnitOfWork, UnitOfWork>(serviceProvider => new UnitOfWork());
            services.AddSingleton<ISmsService>(_ => new TwilioSmsService(
                Configuration["Twilio:AccountSid"],
                Configuration["Twilio:AuthToken"]));

            services.AddSingleton<Randomizer>(_ => new Randomizer());
            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();
            app.UseSwaggerUI(config =>
            {
                config.SwaggerEndpoint("/swagger/v1/swagger.json", "CoronaTest API V1");
                config.DisplayRequestDuration();
            });
        }
    }
}

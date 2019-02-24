using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthService.Logics;
using AuthService.Logics.Impl;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Swagger;

namespace AuthService
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
      const string signingSecurityKey = "IcdbWillNeverDie";
      var signingKey = new SigningSymmetricKey(signingSecurityKey);
      services.AddSingleton<IJwtSigningEncodingKey>(signingKey);

      services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

      services.AddSwaggerGen(c =>
      {
        c.SwaggerDoc("v1", new Info { Title = "My API", Version = "v1" });
      });

      const string jwtSchemeName = "JwtBearer";
      var signingDecodingKey = (IJwtSigningDecodingKey)signingKey;
      services
          .AddAuthentication(options => {
            options.DefaultAuthenticateScheme = jwtSchemeName;
            options.DefaultChallengeScheme = jwtSchemeName;
          })
          .AddJwtBearer(jwtSchemeName, jwtBearerOptions => {
            jwtBearerOptions.TokenValidationParameters = new TokenValidationParameters
            {
              ValidateIssuerSigningKey = true,
              IssuerSigningKey = signingDecodingKey.GetKey(),

              ValidateIssuer = true,
              ValidIssuer = "DemoApp",

              ValidateAudience = true,
              ValidAudience = "DemoAppClient",

              ValidateLifetime = true,

              ClockSkew = TimeSpan.FromSeconds(5)
            };
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
      {
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
      }

      app.UseStaticFiles();

      // Enable middleware to serve generated Swagger as a JSON endpoint.
      app.UseSwagger();

      // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
      // specifying the Swagger JSON endpoint.
      app.UseSwaggerUI(c =>
      {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
      });

      app.UseAuthentication();

      //app.UseHttpsRedirection();
      app.UseMvc();
    }
  }
}

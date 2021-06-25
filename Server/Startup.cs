using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Server
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
            services.AddAuthentication("OAuth")
                 .AddJwtBearer("OAuth", config =>
                 {
                     var secretBytes = Encoding.UTF8.GetBytes(Constants.Secret);
                     var key = new SymmetricSecurityKey(secretBytes);

                     // Passar/recebe o token via url, parametro, tem alguns clientes que envia por url ?access_token=
                     config.Events = new JwtBearerEvents()
                     {
                         OnMessageReceived = context =>
                         {
                             if (context.Request.Query.ContainsKey("access_token"))
                             {
                                 context.Token = context.Request.Query["access_token"];
                             }

                             return Task.CompletedTask;
                         }
                     };

                     config.TokenValidationParameters = new TokenValidationParameters()
                     {
                         ClockSkew = TimeSpan.Zero, // nao deixar zero em produçao, controla o tempo de expiraçao do token, padrao é 5 minutos 
                         ValidIssuer = Constants.Issuer,
                         ValidAudience = Constants.Audiance,
                         IssuerSigningKey = key,
                     };
                 });


            services.AddControllersWithViews()
                .AddRazorRuntimeCompilation(); // Permitir voce alterar a View em tempo de execuçao
        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                //endpoints.MapDefaultControllerRoute();
            });
        }
    }
}

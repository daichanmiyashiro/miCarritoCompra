using miCarritoDeCompra.DataBase;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace miCarritoDeCompra
{   //ACA ESTAN TODAS LAS CONFIGURACIONES QUE SE VAN A USAR EN LA APP 
    
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

       // IMPORTANTE 
        /*LA CONEXIÓN DE BASE DE DATOS TAMBIEN SE CONFIGURA ACA, SOLO UNA VEZ 
        -DENTRO DEL METODO CONFIGURE SERVICE VAMOS A AGREGAR LA FORMA QUE LA APP 
        PARA GENERAR UNA INSTANCIA DE CONTEXTO DE BASE DE DATO( DECIMOS DONDE SE CONECTA Y COMO DE CONECTA A LA BASE DE DATOS) */

     public void ConfigureServices(IServiceCollection services)
     {

         services.AddDbContext<CarritoDbContext>(options => options.UseSqlite("filename=carrito.db"));
         services.AddControllersWithViews();
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

         app.UseAuthorization();

         app.UseEndpoints(endpoints =>
         {
             endpoints.MapControllerRoute(
                 name: "default",
                 pattern: "{controller=Home}/{action=Index}/{id?}");
         });
     }
    }
    }

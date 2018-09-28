using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NNProjekat.Data;
using NNProjekat.Services;

namespace NNProjekat
{
    public class Startup
    {
        private IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<NNProjekatDbContext>(options => options.UseSqlServer(_configuration.GetConnectionString("NNProjekat")));
            services.AddScoped<IAktivnostData, SqlAktivnostData>();
            services.AddScoped<IPredmetData, SqlPredmetData>();
            services.AddScoped<IPolaganjaData, SqlPolaganjaData>();
            services.AddScoped<IStudentData, SqlStudentData>();
            services.AddScoped<INastavnikData, SqlNastavnikData>();

            services.AddMvc().AddJsonOptions(options =>
            {
                options.SerializerSettings.ContractResolver
                    = new Newtonsoft.Json.Serialization.DefaultContractResolver();
                options.SerializerSettings.ReferenceLoopHandling =
Newtonsoft.Json.ReferenceLoopHandling.Ignore;

            }); ;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseStaticFiles();
            app.UseNodeModules(env.ContentRootPath);
            app.UseMvc(ConfigureRoutes);
        }

        private void ConfigureRoutes(IRouteBuilder obj)
        {
            obj.MapRoute("Default", "{controller=Home}/{action=Index}/{id?}");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookMgtApi.Data;
using BookMgtApi.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace BookMgtApi
{
    public class Startup
    {
        public string ConnectionString {get; set;}
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            ConnectionString = Configuration.GetConnectionString("DefaultConnectionString");
            
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "BookMgtApi", Version = "v1" });
            });

            //configure DbContext with Database

            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(ConnectionString));

        }

        public void MigrateDatabaseContexts(IServiceProvider svp)
        {
            // var authenticationDbContext = svp.GetRequiredService<AuthenticationDbContext>();
            // authenticationDbContext.Database.Migrate();

            var applicationDbContext = svp.GetRequiredService<AppDbContext>();
            applicationDbContext.Database.Migrate();
        }
         
         
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "BookMgtApi v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

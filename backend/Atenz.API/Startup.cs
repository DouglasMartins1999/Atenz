using System;
using System.Collections;
using Atenz.Repository;
using Atenz.Repository.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Atenz.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Variables = Environment.GetEnvironmentVariables();
            Configuration = configuration;
        }

        public IDictionary Variables { get; }
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<AtenzDBContext>(opt => opt.UseNpgsql(Variables["DB_CONN"].ToString()));
            services.AddAutoMapper(typeof(Startup));
            services.AddScoped<CourseRepository>();
            services.AddScoped<BookRepository>();
            services.AddScoped<UserRepository>();
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

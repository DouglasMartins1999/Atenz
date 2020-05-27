using System;
using System.Collections;
using System.Text;
using Atenz.API.Helpers;
using Atenz.Repository;
using Atenz.Repository.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

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
            var key = Encoding.ASCII.GetBytes(Variables["JWT_SECRET"].ToString());
            
            services.AddDbContext<AtenzDBContext>(opt => opt.UseNpgsql(Variables["DB_CONN"].ToString()));
            services.AddAutoMapper(typeof(Startup));
            services.AddScoped<CourseRepository>();
            services.AddScoped<BookRepository>();
            services.AddScoped<UserRepository>();
            services.AddScoped<StorageService>();
            services.AddControllers();

            services
                .AddAuthentication(opt => {
                    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(opt => {
                    opt.SaveToken = true;
                    opt.RequireHttpsMetadata = false;
                    opt.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

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

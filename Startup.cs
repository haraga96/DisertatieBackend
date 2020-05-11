using Backend_Dis_App.Database;
using Backend_Dis_App.Services.Implementation;
using Backend_Dis_App.Services.Interfaces;
using Backend_Dis_App.Validators;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace Backend_Dis_App
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
            var connectionString = Configuration["ConnectionStrings:PostreSQL"];

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAllHeaders",
                      builder =>
                      {
                          builder.AllowAnyOrigin()
                                 .AllowAnyHeader()
                                 .AllowAnyMethod();
                      });
            });

            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "TaxApp API",
                    Version = "v1"
                });
            });

            services.AddEntityFrameworkNpgsql().AddDbContext<TaxAppContext>(options =>
            {
                options.UseNpgsql(connectionString);
            });

            services.AddScoped<TaxAppContext, TaxAppContext>();
            services.AddSingleton<IEmailService, EmailService>();
            services.AddSingleton<IUserService, UserService>();
            services.AddSingleton<IEmailValidator, EmailValidator>();
            services.AddSingleton<IPasswordValidator, PasswordValidator>();

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRouting();

            app.UseSwagger();
            app.UseSwaggerUI(c => {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Weather API v1");
            });

            app.UseCors("AllowAllHeaders");

            app.UseAuthentication(); ;
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

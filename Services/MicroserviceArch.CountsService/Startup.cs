using MicroserviceArch.DAL.Context;
using MicroserviceArch.DAL.Repositories;
using MicroserviceArch.InitializeDB;
using MicroserviceArch.Interfaces.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System.Net.Http;

namespace MicroserviceArch.CountsService
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
            services.AddDbContext<DataDB>(options =>
            options.UseNpgsql(Configuration
                .GetConnectionString("VehicleQuotesContext"), m => m.MigrationsAssembly("MicroserviceArch.Dal.PGSQL"))
                .UseSnakeCaseNamingConvention()
            );

            string urlToCurrencyConverter = $"https://api.exchangeratesapi.io/v1/latest?access_key={Configuration.GetSection("AccessKeyToExchangeratesapi").Value}";

            services.AddScoped(typeof(ICountRepository<>), typeof(CountRepository<>));
            services.AddScoped(sp => new HttpClient() { BaseAddress = new System.Uri(urlToCurrencyConverter) });

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "MicroserviceArch.CountsService", Version = "v1" });
            });

            services.AddTransient<DataDBInitializer>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, DataDBInitializer db)
        {
            db.Initialize();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MicroserviceArch.CountsService v1"));
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

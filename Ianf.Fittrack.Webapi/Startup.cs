using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Ianf.Fittrack.Workouts.Repositories;
using Microsoft.EntityFrameworkCore;
using Ianf.Fittrack.Workouts.Services.Interfaces;
using Ianf.Fittrack.Workouts.Services;
using Ianf.Fittrack.Workouts.Persistance.Interfaces;

namespace Ianf.Fittrack.Webapi
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
            services.AddDbContext<FittrackDbContext>(options => 
                options.UseSqlServer(Configuration.GetConnectionString("FittrackDatabase")));
            services.AddTransient<IWorkoutService, WorkoutService>();
            services.AddTransient<IWorkoutRepository, WorkoutRepository>();
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Ianf.Fittrack.Webapi", Version = "v1" });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Ianf.Fittrack.Webapi v1"));
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

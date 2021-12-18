using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PokeApi.Services.Services;
using PokeApi.Services.Settings;
using System.Net.Http;

namespace PokeApi.Api
{
    public class Startup
    {
        private const string swaggerEndpoint = "/swagger/v1/swagger.json";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));
            services.AddTransient<IPokemonService, PokemonService>();
            services.AddSingleton(provider => new HttpClient());

            #region CORS

            services.AddCors(options => options.AddPolicy("AllowAllPolicy", builder => builder
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials()
                    .SetIsOriginAllowed(x => true)));

            #endregion

            services.AddSwaggerGen();
            services.AddControllers(options => options.EnableEndpointRouting = false);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();

            app.UseSwaggerUI(c => c.SwaggerEndpoint(swaggerEndpoint, "Poke API V1"));

            app.UseRouting();

            app.UseCors("AllowAllPolicy");

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseMvc();
        }
    }
}

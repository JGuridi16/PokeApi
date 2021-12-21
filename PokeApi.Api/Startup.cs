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
        private const string policyName = "AllowAllPolicy";

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

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAllPolicy",
                      builder =>
                      {
                          builder
                                 .AllowAnyHeader()
                                 .AllowAnyMethod()
                                 .AllowCredentials();

                          builder.SetIsOriginAllowed(x => true);
                          builder.WithExposedHeaders("Token-Expired");
                      });
            });

            #endregion

            services.AddSwaggerGen();
            services.AddControllers(options => options.EnableEndpointRouting = false).AddNewtonsoftJson();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();

            app.UseSwaggerUI(c => c.SwaggerEndpoint(swaggerEndpoint, "Poke API V1"));

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseCors(policyName);
            
            app.UseMvc();
        }
    }
}

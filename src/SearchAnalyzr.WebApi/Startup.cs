using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using SearchAnalyzr.WebApi.Interfaces;
using SearchAnalyzr.WebApi.Services;
using System;
using System.Net.Http;
using Serilog;

namespace SearchAnalyzr.WebApi
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
            services.AddControllers();
            
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Search Analyzr Web API", Version = "v1" });
            });
            services.AddSwaggerGen(c => { c.EnableAnnotations(); });

            //[TODO] Manage configurable data (e.g. Search base URL) via AWS Systems Manager Parameter Store

            services.AddHttpClient<ISearchService, SearchService>(c =>
            {
                c.BaseAddress = new Uri("https://www.google.com.au/search");
            }).ConfigurePrimaryHttpMessageHandler(handler =>
            new HttpClientHandler()
            {
                AutomaticDecompression = System.Net.DecompressionMethods.GZip
            });

            services.AddTransient<IAnalyzrService, AnalyzrService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Search Analyzr Web API (v1)");
                c.RoutePrefix = string.Empty;
            });

            app.UseExceptionHandler(c => c.Run(async context =>
            {
                var exception = context.Features
                    .Get<IExceptionHandlerPathFeature>()
                    .Error;
                var response = new { code = context.Response.StatusCode, message = exception.Message };
                await context.Response.WriteAsJsonAsync(response);
            }));

            app.UseSerilogRequestLogging();

            app.UseRouting();

            //[TODO] Implement access control to API using Amazon Cognito 
            //[TODO] Make the API more resilient using Polly (http://www.thepollyproject.org/) - e.g. retries,circuit breakers & fallbacks

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProducerService.DbContext;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using ProducerService.Repositories;
using ProducerService.Services;
using ProducerService.Models.Configuration;
using ProducerService.Services.Producer;

namespace ProducerService
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
            services.Configure<AccountDbSettings>(Configuration.GetSection("AccountDbSettings"));
            services.AddSingleton<IAccountDbSettings>(_ => _.GetRequiredService<IOptions<AccountDbSettings>>().Value);
            services.Configure<KafkaConfiguration>(Configuration.GetSection("KafkaConfigs"));
            services.Configure<ServiceAddressConfiguration>(Configuration.GetSection("ServiceAddress"));

            services.AddSingleton<IAccountRepository, AccountRepository>();
            services.AddSingleton<IAccountService, AccountService>();
            services.AddSingleton<KafkaProducerService>();
            services.AddHttpClient<IConsumerService, ConsumerService>();

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ProducerService", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ProducerService v1"));
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

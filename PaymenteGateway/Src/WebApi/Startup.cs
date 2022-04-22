using FluentValidation.AspNetCore;
using Mapster;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using WebApi.Bank;
using WebApi.Database;
using WebApi.Database.DynamoDb;
using WebApi.Maskers;
using WebApi.Models;
using WebApi.Database.Models;
using WebApi.Database.EFCore;

namespace WebApi
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
            TypeAdapterConfig<PayRequest, BankPaymentRequest>.NewConfig();
            TypeAdapterConfig<PayRequest, PaymentDto>.NewConfig();
            TypeAdapterConfig<PaymentDto, PaymentEntity>.NewConfig();

            services.AddHealthChecks();
           
            //var connection = @"";
            services.AddDbContext<PaymentEFCoreDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("SqlConnection")));

            
            services.AddScoped<IPaymentDbContext<PaymentEntity>, PaymentEFCoreDbContext>();
            services.AddScoped<IValueMasker, ValueMasker>();
            services.AddScoped<IDatabaseService, DatabaseService>();
            services.AddScoped<IBankService, SimulatorBankService>();
            services.AddControllers()
                .AddFluentValidation(s =>
                {
                    s.RegisterValidatorsFromAssemblyContaining<Startup>();
                });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebApi", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApi v1"));
            }

            app.UseRouting(); 

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health");
            });

            MigrateDb.PrePopulation(app);
        }
    }
}

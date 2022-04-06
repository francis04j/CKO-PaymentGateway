using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using WebApi.Database.DynamoDb;
using WebApi.Database.EFCore;
using WebApi.Database.Models;

namespace WebApi.Database
{
    public static class MigrateDb
    {
        public static void PrePopulation(IApplicationBuilder app)
        {
            using(var scope = app.ApplicationServices.CreateScope())
            {
                SeedData(scope.ServiceProvider.GetService<PaymentEFCoreDbContext>());
            }
        }

        public static void SeedData(PaymentEFCoreDbContext context)
        {
            System.Console.WriteLine("Applying Migrations..");

            context.Database.Migrate();

            if (!context.Payments.Any())
            {
                System.Console.WriteLine("Seeding..");
                context.Payments.AddRange(
                    new PaymentEntity { PaymentId = "1", Amount = 1, CardExpiryDay = "01" },
                    new PaymentEntity { PaymentId = "2", Amount = 2, CardExpiryDay = "02" }
                    );
            }
            else
            {
                System.Console.WriteLine("No Seeding done..");
            }
        }
    }
}

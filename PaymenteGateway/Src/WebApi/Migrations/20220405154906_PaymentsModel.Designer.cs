﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WebApi.Database.DynamoDb;
using WebApi.Database.EFCore;

namespace WebApi.Migrations
{
    [DbContext(typeof(PaymentEFCoreDbContext))]
    [Migration("20220405154906_PaymentsModel")]
    partial class PaymentsModel
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.15")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("WebApi.Database.DynamoDb.PaymentEntity", b =>
                {
                    b.Property<string>("PaymentId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(5,2)");

                    b.Property<string>("BankMessage")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BankStatus")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BankTransactionCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CardCvv")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CardExpiryDay")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CardExpiryMonth")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CardNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Currency")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("PaymentId");

                    b.ToTable("Payments");
                });
#pragma warning restore 612, 618
        }
    }
}

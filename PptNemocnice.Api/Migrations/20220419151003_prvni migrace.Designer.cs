// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Shop.Api.Data;

#nullable disable

namespace Shop.Api.Migrations
{
    [DbContext(typeof(ShopDbContext))]
    [Migration("20220419151003_prvni migrace")]
    partial class prvnimigrace
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.4");

            modelBuilder.Entity("PptNemocnice.Api.Data.Vybaveni", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("BoughtDateTime")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("LastRevision")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("PriceCzk")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Vybavenis");
                });
#pragma warning restore 612, 618
        }
    }
}

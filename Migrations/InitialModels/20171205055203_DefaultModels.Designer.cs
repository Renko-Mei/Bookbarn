﻿// <auto-generated />
using BookBarn.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace BookBarn.Migrations.InitialModels
{
    [DbContext(typeof(InitialModelsContext))]
    [Migration("20171205055203_DefaultModels")]
    partial class DefaultModels
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.0.0-rtm-26452");

            modelBuilder.Entity("BookBarn.Models.Address", b =>
                {
                    b.Property<int>("AddressId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("City")
                        .IsRequired();

                    b.Property<string>("Country")
                        .IsRequired();

                    b.Property<string>("LegalName")
                        .IsRequired();

                    b.Property<string>("PhoneNumber");

                    b.Property<string>("PostalCode")
                        .IsRequired();

                    b.Property<string>("Province")
                        .IsRequired();

                    b.Property<string>("StreetAddress")
                        .IsRequired();

                    b.Property<string>("UserKey");

                    b.HasKey("AddressId");

                    b.ToTable("Address");
                });

            modelBuilder.Entity("BookBarn.Models.Book", b =>
                {
                    b.Property<string>("Isbn")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(20);

                    b.Property<string>("Author")
                        .IsRequired();

                    b.Property<string>("Publisher");

                    b.Property<string>("Title")
                        .IsRequired();

                    b.HasKey("Isbn");

                    b.ToTable("Book");
                });

            modelBuilder.Entity("BookBarn.Models.Order", b =>
                {
                    b.Property<int>("OrderId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("BuyerId");

                    b.Property<bool>("IsSold");

                    b.Property<DateTime>("OrderDate");

                    b.Property<float>("SalePrice");

                    b.Property<string>("SellerId");

                    b.Property<DateTime>("ShippedDate");

                    b.HasKey("OrderId");

                    b.ToTable("Order");
                });

            modelBuilder.Entity("BookBarn.Models.SaleItem", b =>
                {
                    b.Property<int>("SaleItemId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Authors");

                    b.Property<string>("Description");

                    b.Property<byte[]>("Image");

                    b.Property<string>("ImageLinks");

                    b.Property<bool>("IsSold");

                    b.Property<string>("Isbn")
                        .IsRequired();

                    b.Property<string>("Isbn10Or13");

                    b.Property<int?>("OrderId");

                    b.Property<float>("Price");

                    b.Property<string>("PublishedData");

                    b.Property<string>("Publisher");

                    b.Property<string>("QualityString")
                        .HasColumnName("Quality");

                    b.Property<string>("Subtitle");

                    b.Property<string>("Title");

                    b.Property<string>("UserKey");

                    b.HasKey("SaleItemId");

                    b.HasIndex("OrderId");

                    b.ToTable("SaleItem");
                });

            modelBuilder.Entity("BookBarn.Models.School", b =>
                {
                    b.Property<int>("SchoolId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("SchoolId");

                    b.ToTable("School");
                });

            modelBuilder.Entity("BookBarn.Models.ShoppingCartItem", b =>
                {
                    b.Property<int>("ShoppingCartItemId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Amount");

                    b.Property<int?>("SaleItemId");

                    b.Property<string>("ShoppingCartTemp");

                    b.HasKey("ShoppingCartItemId");

                    b.HasIndex("SaleItemId");

                    b.ToTable("ShoppingCartItems");
                });

            modelBuilder.Entity("BookBarn.Models.SaleItem", b =>
                {
                    b.HasOne("BookBarn.Models.Order")
                        .WithMany("SaleItems")
                        .HasForeignKey("OrderId");
                });

            modelBuilder.Entity("BookBarn.Models.ShoppingCartItem", b =>
                {
                    b.HasOne("BookBarn.Models.SaleItem", "SaleItem")
                        .WithMany()
                        .HasForeignKey("SaleItemId");
                });
#pragma warning restore 612, 618
        }
    }
}
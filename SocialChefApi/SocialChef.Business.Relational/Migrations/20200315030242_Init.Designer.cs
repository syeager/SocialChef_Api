﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SocialChef.Business.Relational;

namespace SocialChef.Business.Relational.Migrations
{
    [DbContext(typeof(SqlDbContext))]
    [Migration("20200315030242_Init")]
    partial class Init
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("SocialChef.Business.Relational.Models.Chef", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("UserID")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("ID");

                    b.ToTable("Chefs");
                });

            modelBuilder.Entity("SocialChef.Business.Relational.Models.ChefRecipe", b =>
                {
                    b.Property<Guid>("ChefID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("RecipeID")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("ChefID", "RecipeID");

                    b.ToTable("ChefRecipes");
                });

            modelBuilder.Entity("SocialChef.Business.Relational.Models.ChefRecipe", b =>
                {
                    b.HasOne("SocialChef.Business.Relational.Models.Chef", "Chef")
                        .WithMany("Recipes")
                        .HasForeignKey("ChefID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}

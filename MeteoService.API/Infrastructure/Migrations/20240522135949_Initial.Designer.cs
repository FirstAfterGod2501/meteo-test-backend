﻿// <auto-generated />
using System;
using MeteoService.API.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MeteoService.API.Infrastructure.Migrations
{
    [DbContext(typeof(ApplicationContext))]
    [Migration("20240522135949_Initial")]
    partial class Initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("MeteoService.API.Core.Entities.WeatherData", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<double>("Latitude")
                        .HasColumnType("double precision");

                    b.Property<double>("Longitude")
                        .HasColumnType("double precision");

                    b.Property<double>("Temperature")
                        .HasColumnType("double precision");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("WindDirection")
                        .HasColumnType("integer");

                    b.Property<double>("WindSpeed")
                        .HasColumnType("double precision");

                    b.HasKey("Id");

                    b.HasIndex("Timestamp")
                        .HasDatabaseName("IX_WeatherData_Timestamp");

                    b.HasIndex("Latitude", "Longitude")
                        .HasDatabaseName("IX_WeatherData_Latitude_Longitude");

                    b.ToTable("WeatherData");
                });
#pragma warning restore 612, 618
        }
    }
}
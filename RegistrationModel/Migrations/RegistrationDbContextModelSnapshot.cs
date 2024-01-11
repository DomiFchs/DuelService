﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RegistrationModel.Configurations;

#nullable disable

namespace RegistrationModel.Migrations
{
    [DbContext(typeof(RegistrationDbContext))]
    partial class RegistrationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("RegistrationModel.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("USER_ID");

                    b.Property<float>("EloRating")
                        .HasColumnType("float")
                        .HasColumnName("ELO_RATING");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("USERNAME");

                    b.Property<int>("State")
                        .HasColumnType("int")
                        .HasColumnName("STATE");

                    b.HasKey("Id");

                    b.ToTable("USERS");
                });
#pragma warning restore 612, 618
        }
    }
}

﻿// <auto-generated />
using System;
using ContactFormProject.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ContactFormProject.Data.Migrations
{
    [DbContext(typeof(ContactFormDbContext))]
    [Migration("20240227174317_Init1")]
    partial class Init1
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.16")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("ContactFormProject.Data.Mail", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FromAddress")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("GuestFirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("GuestLastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("GuestMailAddress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<bool>("IsIncomingMail")
                        .HasColumnType("bit");

                    b.Property<bool>("IsRead")
                        .HasColumnType("bit");

                    b.Property<DateTime>("SendDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Subject")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ToAddress")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Mails");
                });
#pragma warning restore 612, 618
        }
    }
}

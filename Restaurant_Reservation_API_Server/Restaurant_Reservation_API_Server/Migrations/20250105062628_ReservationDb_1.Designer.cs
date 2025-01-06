﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Restaurant_Reservation_API_Server.Data;

#nullable disable

namespace Restaurant_Reservation_API_Server.Migrations
{
    [DbContext(typeof(ReservationDbContext))]
    [Migration("20250105062628_ReservationDb_1")]
    partial class ReservationDb_1
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Restaurant_Reservation_API_Server.Models.Reservation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("BookingDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("ChildSeat")
                        .HasColumnType("int");

                    b.Property<string>("CustomerName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("SeatRequirement")
                        .HasColumnType("int");

                    b.Property<int>("ArrivalTimeId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ArrivalTimeId");

                    b.ToTable("Reservations");
                });

            modelBuilder.Entity("Restaurant_Reservation_API_Server.Models.ArrivalTime", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Period")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("ArrivalTimes");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Period = "11:30 - 13:00"
                        },
                        new
                        {
                            Id = 2,
                            Period = "13:00 - 14:30"
                        },
                        new
                        {
                            Id = 3,
                            Period = "14:30 - 16:00"
                        },
                        new
                        {
                            Id = 4,
                            Period = "16:00 - 17:30"
                        },
                        new
                        {
                            Id = 5,
                            Period = "17:30 - 19:00"
                        },
                        new
                        {
                            Id = 6,
                            Period = "19:00 - 20:30"
                        });
                });

            modelBuilder.Entity("Restaurant_Reservation_API_Server.Models.Reservation", b =>
                {
                    b.HasOne("Restaurant_Reservation_API_Server.Models.ArrivalTime", "ArrivalTime")
                        .WithMany()
                        .HasForeignKey("ArrivalTimeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ArrivalTime");
                });
#pragma warning restore 612, 618
        }
    }
}

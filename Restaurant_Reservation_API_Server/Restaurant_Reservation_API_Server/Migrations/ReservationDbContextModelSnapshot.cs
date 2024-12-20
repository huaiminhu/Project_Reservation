﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Restaurant_Reservation_API_Server.Data;

#nullable disable

namespace Restaurant_Reservation_API_Server.Migrations
{
    [DbContext(typeof(ReservationDbContext))]
    partial class ReservationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Restaurant_Reservation_API_Server.Models.ArrivedTime", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Period")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("arrivedTimes");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Period = "11:30"
                        },
                        new
                        {
                            Id = 2,
                            Period = "13:00"
                        },
                        new
                        {
                            Id = 3,
                            Period = "14:30"
                        },
                        new
                        {
                            Id = 4,
                            Period = "16:00"
                        },
                        new
                        {
                            Id = 5,
                            Period = "17:30"
                        },
                        new
                        {
                            Id = 6,
                            Period = "19:00"
                        });
                });

            modelBuilder.Entity("Restaurant_Reservation_API_Server.Models.Reservation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("ArrivedTimeId")
                        .HasColumnType("int");

                    b.Property<string>("BookingDate")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ChildSeat")
                        .HasColumnType("int");

                    b.Property<string>("CustomerName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("SeatRequirement")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("Phone")
                        .IsUnique();

                    b.ToTable("Reservations");
                });
#pragma warning restore 612, 618
        }
    }
}

﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Restaurant_Reservation_API_Server.Migrations
{
    /// <inheritdoc />
    public partial class ReservationDb_1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "arrivedTimes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Period = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_arrivedTimes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Reservations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BookingDate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CustomerName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    SeatRequirement = table.Column<int>(type: "int", nullable: false),
                    ChildSeat = table.Column<int>(type: "int", nullable: false),
                    ArrivedTimeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reservations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reservations_arrivedTimes_ArrivedTimeId",
                        column: x => x.ArrivedTimeId,
                        principalTable: "arrivedTimes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "arrivedTimes",
                columns: new[] { "Id", "Period" },
                values: new object[,]
                {
                    { 1, "11:30" },
                    { 2, "13:00" },
                    { 3, "14:30" },
                    { 4, "16:00" },
                    { 5, "17:30" },
                    { 6, "19:00" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_ArrivedTimeId",
                table: "Reservations",
                column: "ArrivedTimeId");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_Phone",
                table: "Reservations",
                column: "Phone",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Reservations");

            migrationBuilder.DropTable(
                name: "arrivedTimes");
        }
    }
}

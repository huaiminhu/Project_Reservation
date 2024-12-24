using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Restaurant_Reservation_API_Server.Migrations
{
    /// <inheritdoc />
    public partial class ModifyPeriods_2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Reservations_ArrivedTimeId",
                table: "Reservations",
                column: "ArrivedTimeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_arrivedTimes_ArrivedTimeId",
                table: "Reservations",
                column: "ArrivedTimeId",
                principalTable: "arrivedTimes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_arrivedTimes_ArrivedTimeId",
                table: "Reservations");

            migrationBuilder.DropIndex(
                name: "IX_Reservations_ArrivedTimeId",
                table: "Reservations");
        }
    }
}

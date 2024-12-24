using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Restaurant_Reservation_API_Server.Migrations
{
    /// <inheritdoc />
    public partial class ModifyPeriods_1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "arrivedTimes",
                keyColumn: "Id",
                keyValue: 1,
                column: "Period",
                value: "11:30 - 13:00");

            migrationBuilder.UpdateData(
                table: "arrivedTimes",
                keyColumn: "Id",
                keyValue: 2,
                column: "Period",
                value: "13:00 - 14:30");

            migrationBuilder.UpdateData(
                table: "arrivedTimes",
                keyColumn: "Id",
                keyValue: 3,
                column: "Period",
                value: "14:30 - 16:00");

            migrationBuilder.UpdateData(
                table: "arrivedTimes",
                keyColumn: "Id",
                keyValue: 4,
                column: "Period",
                value: "16:00 - 17:30");

            migrationBuilder.UpdateData(
                table: "arrivedTimes",
                keyColumn: "Id",
                keyValue: 5,
                column: "Period",
                value: "17:30 - 19:00");

            migrationBuilder.UpdateData(
                table: "arrivedTimes",
                keyColumn: "Id",
                keyValue: 6,
                column: "Period",
                value: "19:00 - 20:30");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "arrivedTimes",
                keyColumn: "Id",
                keyValue: 1,
                column: "Period",
                value: "11:30");

            migrationBuilder.UpdateData(
                table: "arrivedTimes",
                keyColumn: "Id",
                keyValue: 2,
                column: "Period",
                value: "13:00");

            migrationBuilder.UpdateData(
                table: "arrivedTimes",
                keyColumn: "Id",
                keyValue: 3,
                column: "Period",
                value: "14:30");

            migrationBuilder.UpdateData(
                table: "arrivedTimes",
                keyColumn: "Id",
                keyValue: 4,
                column: "Period",
                value: "16:00");

            migrationBuilder.UpdateData(
                table: "arrivedTimes",
                keyColumn: "Id",
                keyValue: 5,
                column: "Period",
                value: "17:30");

            migrationBuilder.UpdateData(
                table: "arrivedTimes",
                keyColumn: "Id",
                keyValue: 6,
                column: "Period",
                value: "19:00");
        }
    }
}

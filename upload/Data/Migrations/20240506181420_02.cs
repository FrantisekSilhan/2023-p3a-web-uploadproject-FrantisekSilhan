using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace upload.Data.Migrations
{
    /// <inheritdoc />
    public partial class _02 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("e2760bc3-e0cf-48fc-8be8-0d195eda4663"));

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { new Guid("0cdaab13-8fd9-43d7-bc12-95648d2b18d2"), null, "moderator", "MODERATOR" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                columns: new[] { "ConcurrencyStamp", "NormalizedUserName", "PasswordHash", "UserName" },
                values: new object[] { "15e95fd6-ae29-43b4-bc4c-e8d016b1d153", "ADMIN@LOCAL.SLHN.CZ", "AQAAAAIAAYagAAAAEPRGLwOTlPzXxHR6+rKdC7+can9Bn79oGCIbPHmQj7bkgtFm3YW82+abljqf5wF9KA==", "admin@local.slhn.cz" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("0cdaab13-8fd9-43d7-bc12-95648d2b18d2"));

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { new Guid("e2760bc3-e0cf-48fc-8be8-0d195eda4663"), null, "moderator", "MODERATOR" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                columns: new[] { "ConcurrencyStamp", "NormalizedUserName", "PasswordHash", "UserName" },
                values: new object[] { "d45d96b2-15d1-443f-a14c-6c4b24923462", "ADMIN", "AQAAAAIAAYagAAAAEOtaT3T5T2GgZkr6TBHoNj7DEXxWy08BwAJboAeY3j1AQJWhwXGt36BMFbS3kxNJVw==", "admin" });
        }
    }
}

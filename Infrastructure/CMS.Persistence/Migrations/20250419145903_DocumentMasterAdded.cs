using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CMS.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class DocumentMasterAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "MasterEmployees",
                keyColumn: "ValueId",
                keyValue: 1,
                column: "Password",
                value: "AQAAAAIAAYagAAAAEPBQC/woKo5yS8rj+852+w9Bw/cbXU40VU8LMZRPuZiMwI9iLk4kT0HoQJZpYmQ28g==");

            migrationBuilder.UpdateData(
                table: "MasterEmployees",
                keyColumn: "ValueId",
                keyValue: 2,
                column: "Password",
                value: "AQAAAAIAAYagAAAAEFrKBxk++ZsaYzc+YcX4Z9Fyl429JsOcyliARH9y+KKpb5gredae/cuqTHf5INIzYQ==");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "MasterEmployees",
                keyColumn: "ValueId",
                keyValue: 1,
                column: "Password",
                value: "AQAAAAIAAYagAAAAEAxo4qHNocpyVTxCmbM8mlZ1YfwFkYMnKWwiNtB2N0/mIpHwsICWJjDKJB4vVSwT7w==");

            migrationBuilder.UpdateData(
                table: "MasterEmployees",
                keyColumn: "ValueId",
                keyValue: 2,
                column: "Password",
                value: "AQAAAAIAAYagAAAAEMtmlIFhszPgfOYrbHgp5zzyLFgZ5otmQad1YYFCmE5aBxkpkDyOy8BsJmWPsZj92g==");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CMS.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class EscalationMatrixAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TriggerDaysEscalation1",
                table: "MasterEscalationMatrixContracts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TriggerDaysEscalation2",
                table: "MasterEscalationMatrixContracts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TriggerDaysEscalation3",
                table: "MasterEscalationMatrixContracts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "MasterEmployees",
                keyColumn: "ValueId",
                keyValue: 1,
                column: "Password",
                value: "AQAAAAIAAYagAAAAEDZKOIcdNRzK/SPp9MP4p44jpeDHdRkceBuwCLhh1AZ7dSDite7ZiLk4GRkCYOYotA==");

            migrationBuilder.UpdateData(
                table: "MasterEmployees",
                keyColumn: "ValueId",
                keyValue: 2,
                column: "Password",
                value: "AQAAAAIAAYagAAAAEGksgR+wpEFOqfIQYqHFREpnei4bh87Pd7uFoM71SElTbrkMlm5gxZTkLH54Ku/HEg==");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TriggerDaysEscalation1",
                table: "MasterEscalationMatrixContracts");

            migrationBuilder.DropColumn(
                name: "TriggerDaysEscalation2",
                table: "MasterEscalationMatrixContracts");

            migrationBuilder.DropColumn(
                name: "TriggerDaysEscalation3",
                table: "MasterEscalationMatrixContracts");

            migrationBuilder.UpdateData(
                table: "MasterEmployees",
                keyColumn: "ValueId",
                keyValue: 1,
                column: "Password",
                value: "AQAAAAIAAYagAAAAEAL5yhzRrL97Cy5gq+qz106Oz4gLVBZK3VihlAoWS6Z23cMIwTcuSx+yEH/LHI6ijA==");

            migrationBuilder.UpdateData(
                table: "MasterEmployees",
                keyColumn: "ValueId",
                keyValue: 2,
                column: "Password",
                value: "AQAAAAIAAYagAAAAEMv20OP6w/Mxhoa8aTyR019DKpy9vgSuzQ86owwajI+g/xa7H37303i2FdnObfE2KA==");
        }
    }
}

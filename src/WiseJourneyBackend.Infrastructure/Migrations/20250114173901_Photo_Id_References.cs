using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WiseJourneyBackend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Photo_Id_References : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PhotoId",
                table: "Places",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhotoId",
                table: "Places");
        }
    }
}

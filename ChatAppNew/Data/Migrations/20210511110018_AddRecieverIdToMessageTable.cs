using Microsoft.EntityFrameworkCore.Migrations;

namespace ChatAppNew.Data.Migrations
{
    public partial class AddRecieverIdToMessageTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RecieverId",
                table: "Messages",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RecieverId",
                table: "Messages");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApiRN.Migrations
{
    /// <inheritdoc />
    public partial class FixIdentityRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AspNetUserLogins",
                type: "character varying(34)",
                maxLength: 34,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "UserId1",
                table: "AspNetUserLogins",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId1",
                table: "AspNetUserLogins",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserLogins_AspNetUsers_UserId1",
                table: "AspNetUserLogins",
                column: "UserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserLogins_AspNetUsers_UserId1",
                table: "AspNetUserLogins");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUserLogins_UserId1",
                table: "AspNetUserLogins");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AspNetUserLogins");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "AspNetUserLogins");
        }
    }
}

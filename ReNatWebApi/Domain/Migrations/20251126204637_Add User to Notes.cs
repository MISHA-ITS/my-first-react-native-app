using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Domain.Migrations
{
    /// <inheritdoc />
    public partial class AddUsertoNotes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tblNotes_AspNetUsers_UserEntityId",
                table: "tblNotes");

            migrationBuilder.DropIndex(
                name: "IX_tblNotes_UserEntityId",
                table: "tblNotes");

            migrationBuilder.DropColumn(
                name: "UserEntityId",
                table: "tblNotes");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "tblNotes",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<long>(
                name: "UserId",
                table: "tblNotes",
                type: "bigint",
                nullable: false,
                defaultValue: 1L);

            migrationBuilder.CreateIndex(
                name: "IX_tblNotes_UserId",
                table: "tblNotes",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_tblNotes_AspNetUsers_UserId",
                table: "tblNotes",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tblNotes_AspNetUsers_UserId",
                table: "tblNotes");

            migrationBuilder.DropIndex(
                name: "IX_tblNotes_UserId",
                table: "tblNotes");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "tblNotes");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "tblNotes",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(1000)",
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AddColumn<long>(
                name: "UserEntityId",
                table: "tblNotes",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_tblNotes_UserEntityId",
                table: "tblNotes",
                column: "UserEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_tblNotes_AspNetUsers_UserEntityId",
                table: "tblNotes",
                column: "UserEntityId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace Isitar.TimeTracking.Persistence.Migrations
{
    public partial class DeleteCascadeAuditTrail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuditTrailEntry_Projects_ProjectId",
                table: "AuditTrailEntry");

            migrationBuilder.DropForeignKey(
                name: "FK_AuditTrailEntry_Users_UserId",
                table: "AuditTrailEntry");

            migrationBuilder.AddForeignKey(
                name: "FK_AuditTrailEntry_Projects_ProjectId",
                table: "AuditTrailEntry",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AuditTrailEntry_Users_UserId",
                table: "AuditTrailEntry",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuditTrailEntry_Projects_ProjectId",
                table: "AuditTrailEntry");

            migrationBuilder.DropForeignKey(
                name: "FK_AuditTrailEntry_Users_UserId",
                table: "AuditTrailEntry");

            migrationBuilder.AddForeignKey(
                name: "FK_AuditTrailEntry_Projects_ProjectId",
                table: "AuditTrailEntry",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AuditTrailEntry_Users_UserId",
                table: "AuditTrailEntry",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

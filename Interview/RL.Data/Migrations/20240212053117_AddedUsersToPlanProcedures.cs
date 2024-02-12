using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RL.Data.Migrations
{
    public partial class AddedUsersToPlanProcedures : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PlanProcedureUser",
                columns: table => new
                {
                    AssignedUsersUserId = table.Column<int>(type: "INTEGER", nullable: false),
                    PlanProceduresPlanId = table.Column<int>(type: "INTEGER", nullable: false),
                    PlanProceduresProcedureId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanProcedureUser", x => new { x.AssignedUsersUserId, x.PlanProceduresPlanId, x.PlanProceduresProcedureId });
                    table.ForeignKey(
                        name: "FK_PlanProcedureUser_PlanProcedures_PlanProceduresPlanId_PlanProceduresProcedureId",
                        columns: x => new { x.PlanProceduresPlanId, x.PlanProceduresProcedureId },
                        principalTable: "PlanProcedures",
                        principalColumns: new[] { "PlanId", "ProcedureId" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlanProcedureUser_Users_AssignedUsersUserId",
                        column: x => x.AssignedUsersUserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlanProcedureUser_PlanProceduresPlanId_PlanProceduresProcedureId",
                table: "PlanProcedureUser",
                columns: new[] { "PlanProceduresPlanId", "PlanProceduresProcedureId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlanProcedureUser");
        }
    }
}

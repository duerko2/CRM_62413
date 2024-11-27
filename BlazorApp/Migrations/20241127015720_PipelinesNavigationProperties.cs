using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlazorApp.Migrations
{
    /// <inheritdoc />
    public partial class PipelinesNavigationProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pipelines_Campaigns_CampaignId",
                table: "Pipelines");

            migrationBuilder.DropForeignKey(
                name: "FK_Pipelines_Contacts_ContactId",
                table: "Pipelines");

            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Pipelines_PipelineId",
                table: "Tasks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tasks",
                table: "Tasks");

            migrationBuilder.RenameTable(
                name: "Tasks",
                newName: "PipelineTask");

            migrationBuilder.RenameIndex(
                name: "IX_Tasks_PipelineId",
                table: "PipelineTask",
                newName: "IX_PipelineTask_PipelineId");

            migrationBuilder.AddColumn<string>(
                name: "ActiveStage",
                table: "Pipelines",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsCompleted",
                table: "PipelineTask",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsMasterTask",
                table: "PipelineTask",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Stage",
                table: "PipelineTask",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PipelineTask",
                table: "PipelineTask",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Pipelines_Campaigns_CampaignId",
                table: "Pipelines",
                column: "CampaignId",
                principalTable: "Campaigns",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Pipelines_Contacts_ContactId",
                table: "Pipelines",
                column: "ContactId",
                principalTable: "Contacts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PipelineTask_Pipelines_PipelineId",
                table: "PipelineTask",
                column: "PipelineId",
                principalTable: "Pipelines",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pipelines_Campaigns_CampaignId",
                table: "Pipelines");

            migrationBuilder.DropForeignKey(
                name: "FK_Pipelines_Contacts_ContactId",
                table: "Pipelines");

            migrationBuilder.DropForeignKey(
                name: "FK_PipelineTask_Pipelines_PipelineId",
                table: "PipelineTask");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PipelineTask",
                table: "PipelineTask");

            migrationBuilder.DropColumn(
                name: "ActiveStage",
                table: "Pipelines");

            migrationBuilder.DropColumn(
                name: "IsCompleted",
                table: "PipelineTask");

            migrationBuilder.DropColumn(
                name: "IsMasterTask",
                table: "PipelineTask");

            migrationBuilder.DropColumn(
                name: "Stage",
                table: "PipelineTask");

            migrationBuilder.RenameTable(
                name: "PipelineTask",
                newName: "Tasks");

            migrationBuilder.RenameIndex(
                name: "IX_PipelineTask_PipelineId",
                table: "Tasks",
                newName: "IX_Tasks_PipelineId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tasks",
                table: "Tasks",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Pipelines_Campaigns_CampaignId",
                table: "Pipelines",
                column: "CampaignId",
                principalTable: "Campaigns",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Pipelines_Contacts_ContactId",
                table: "Pipelines",
                column: "ContactId",
                principalTable: "Contacts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Pipelines_PipelineId",
                table: "Tasks",
                column: "PipelineId",
                principalTable: "Pipelines",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

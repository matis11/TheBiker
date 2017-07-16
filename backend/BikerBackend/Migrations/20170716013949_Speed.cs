using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BikerBackend.Migrations
{
    public partial class Speed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Speed",
                table: "VibrationDatas",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AlterColumn<double>(
                name: "EndLocationLongitude",
                table: "Routes",
                nullable: true,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<double>(
                name: "EndLocationLatitude",
                table: "Routes",
                nullable: true,
                oldClrType: typeof(double));

            migrationBuilder.AddColumn<double>(
                name: "Speed",
                table: "FinalDatas",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Speed",
                table: "VibrationDatas");

            migrationBuilder.DropColumn(
                name: "Speed",
                table: "FinalDatas");

            migrationBuilder.AlterColumn<double>(
                name: "EndLocationLongitude",
                table: "Routes",
                nullable: false,
                oldClrType: typeof(double),
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "EndLocationLatitude",
                table: "Routes",
                nullable: false,
                oldClrType: typeof(double),
                oldNullable: true);
        }
    }
}

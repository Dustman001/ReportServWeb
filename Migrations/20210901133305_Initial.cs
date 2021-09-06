using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ReportServiceWeb02.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Areports",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TimeStamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    CapturTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Station = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    Point = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    Operator = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    Sname = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    Priority = table.Column<string>(type: "nvarchar(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Areports", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Areports");
        }
    }
}

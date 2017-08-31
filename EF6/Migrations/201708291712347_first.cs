namespace EF6.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class first : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CategorizeSetting",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Text = c.String(),
                        Color = c.String(),
                        FontColor = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DefaultSchedule",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Number = c.String(),
                        Job = c.String(),
                        ScheduleFor = c.DateTime(nullable: false),
                        StartDate = c.DateTime(nullable: false),
                        DueDate = c.DateTime(nullable: false),
                        IssueDate = c.DateTime(nullable: false),
                        Subject = c.String(),
                        AllDay = c.Boolean(nullable: false),
                        Recurrence = c.Boolean(nullable: false),
                        StartTime = c.DateTime(nullable: false),
                        EndTime = c.DateTime(nullable: false),
                        Description = c.String(),
                        RecurrenceRule = c.String(),
                        Category = c.String(),
                        Equipment = c.String(),
                        Resource = c.String(),
                        Employee = c.String(),
                        Customer = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Equipment",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.EquipmentDefaultSchedules",
                c => new
                    {
                        Equipment_Id = c.Int(nullable: false),
                        DefaultSchedule_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Equipment_Id, t.DefaultSchedule_Id })
                .ForeignKey("dbo.Equipment", t => t.Equipment_Id, cascadeDelete: true)
                .ForeignKey("dbo.DefaultSchedule", t => t.DefaultSchedule_Id, cascadeDelete: true)
                .Index(t => t.Equipment_Id)
                .Index(t => t.DefaultSchedule_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.EquipmentDefaultSchedules", "DefaultSchedule_Id", "dbo.DefaultSchedule");
            DropForeignKey("dbo.EquipmentDefaultSchedules", "Equipment_Id", "dbo.Equipment");
            DropIndex("dbo.EquipmentDefaultSchedules", new[] { "DefaultSchedule_Id" });
            DropIndex("dbo.EquipmentDefaultSchedules", new[] { "Equipment_Id" });
            DropTable("dbo.EquipmentDefaultSchedules");
            DropTable("dbo.Equipment");
            DropTable("dbo.DefaultSchedule");
            DropTable("dbo.CategorizeSetting");
        }
    }
}

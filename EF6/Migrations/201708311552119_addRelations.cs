namespace EF6.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addRelations : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Customer",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Employee",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Resource",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DefaultScheduleCustomers",
                c => new
                    {
                        DefaultSchedule_Id = c.Int(nullable: false),
                        Customer_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.DefaultSchedule_Id, t.Customer_Id })
                .ForeignKey("dbo.DefaultSchedule", t => t.DefaultSchedule_Id, cascadeDelete: true)
                .ForeignKey("dbo.Customer", t => t.Customer_Id, cascadeDelete: true)
                .Index(t => t.DefaultSchedule_Id)
                .Index(t => t.Customer_Id);
            
            CreateTable(
                "dbo.EmployeeDefaultSchedules",
                c => new
                    {
                        Employee_Id = c.Int(nullable: false),
                        DefaultSchedule_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Employee_Id, t.DefaultSchedule_Id })
                .ForeignKey("dbo.Employee", t => t.Employee_Id, cascadeDelete: true)
                .ForeignKey("dbo.DefaultSchedule", t => t.DefaultSchedule_Id, cascadeDelete: true)
                .Index(t => t.Employee_Id)
                .Index(t => t.DefaultSchedule_Id);
            
            CreateTable(
                "dbo.ResourceDefaultSchedules",
                c => new
                    {
                        Resource_Id = c.Int(nullable: false),
                        DefaultSchedule_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Resource_Id, t.DefaultSchedule_Id })
                .ForeignKey("dbo.Resource", t => t.Resource_Id, cascadeDelete: true)
                .ForeignKey("dbo.DefaultSchedule", t => t.DefaultSchedule_Id, cascadeDelete: true)
                .Index(t => t.Resource_Id)
                .Index(t => t.DefaultSchedule_Id);
            
            DropColumn("dbo.DefaultSchedule", "Resource");
            DropColumn("dbo.DefaultSchedule", "Employee");
            DropColumn("dbo.DefaultSchedule", "Customer");
        }
        
        public override void Down()
        {
            AddColumn("dbo.DefaultSchedule", "Customer", c => c.String());
            AddColumn("dbo.DefaultSchedule", "Employee", c => c.String());
            AddColumn("dbo.DefaultSchedule", "Resource", c => c.String());
            DropForeignKey("dbo.ResourceDefaultSchedules", "DefaultSchedule_Id", "dbo.DefaultSchedule");
            DropForeignKey("dbo.ResourceDefaultSchedules", "Resource_Id", "dbo.Resource");
            DropForeignKey("dbo.EmployeeDefaultSchedules", "DefaultSchedule_Id", "dbo.DefaultSchedule");
            DropForeignKey("dbo.EmployeeDefaultSchedules", "Employee_Id", "dbo.Employee");
            DropForeignKey("dbo.DefaultScheduleCustomers", "Customer_Id", "dbo.Customer");
            DropForeignKey("dbo.DefaultScheduleCustomers", "DefaultSchedule_Id", "dbo.DefaultSchedule");
            DropIndex("dbo.ResourceDefaultSchedules", new[] { "DefaultSchedule_Id" });
            DropIndex("dbo.ResourceDefaultSchedules", new[] { "Resource_Id" });
            DropIndex("dbo.EmployeeDefaultSchedules", new[] { "DefaultSchedule_Id" });
            DropIndex("dbo.EmployeeDefaultSchedules", new[] { "Employee_Id" });
            DropIndex("dbo.DefaultScheduleCustomers", new[] { "Customer_Id" });
            DropIndex("dbo.DefaultScheduleCustomers", new[] { "DefaultSchedule_Id" });
            DropTable("dbo.ResourceDefaultSchedules");
            DropTable("dbo.EmployeeDefaultSchedules");
            DropTable("dbo.DefaultScheduleCustomers");
            DropTable("dbo.Resource");
            DropTable("dbo.Employee");
            DropTable("dbo.Customer");
        }
    }
}

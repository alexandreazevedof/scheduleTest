namespace EF6.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addRelationsStrings : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DefaultSchedule", "ResourceString", c => c.String());
            AddColumn("dbo.DefaultSchedule", "EmployeeString", c => c.String());
            AddColumn("dbo.DefaultSchedule", "CustomerString", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.DefaultSchedule", "CustomerString");
            DropColumn("dbo.DefaultSchedule", "EmployeeString");
            DropColumn("dbo.DefaultSchedule", "ResourceString");
        }
    }
}

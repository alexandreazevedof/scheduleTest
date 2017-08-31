namespace EF6.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class categoryID : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.DefaultSchedule", "Category", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.DefaultSchedule", "Category", c => c.String());
        }
    }
}

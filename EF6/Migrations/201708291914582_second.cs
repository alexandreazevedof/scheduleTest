namespace EF6.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class second : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DefaultSchedule", "EquipmentString", c => c.String());
            DropColumn("dbo.DefaultSchedule", "Equipment");
        }
        
        public override void Down()
        {
            AddColumn("dbo.DefaultSchedule", "Equipment", c => c.String());
            DropColumn("dbo.DefaultSchedule", "EquipmentString");
        }
    }
}

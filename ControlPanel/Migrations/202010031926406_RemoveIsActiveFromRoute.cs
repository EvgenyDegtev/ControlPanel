namespace ControlPanel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveIsActiveFromRoute : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Routes", "IsActive");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Routes", "IsActive", c => c.Boolean(nullable: false));
        }
    }
}

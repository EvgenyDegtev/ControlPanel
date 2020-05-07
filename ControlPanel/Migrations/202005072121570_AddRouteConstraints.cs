namespace ControlPanel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRouteConstraints : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Routes", "Name", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.Routes", "Key", c => c.String(nullable: false, maxLength: 100));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Routes", "Key", c => c.String());
            AlterColumn("dbo.Routes", "Name", c => c.String());
        }
    }
}

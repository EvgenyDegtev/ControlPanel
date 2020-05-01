namespace ControlPanel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AgentbaseConstraints : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Agents", "Name", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.Agents", "Login", c => c.String(nullable: false, maxLength: 100));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Agents", "Login", c => c.String());
            AlterColumn("dbo.Agents", "Name", c => c.String());
        }
    }
}

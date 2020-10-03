namespace ControlPanel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveIsActive : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Agents", "IsActive");
            DropColumn("dbo.AgentToSkills", "IsActive");
            DropColumn("dbo.Skills", "IsActive");
            DropColumn("dbo.Groups", "IsActive");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Groups", "IsActive", c => c.Boolean(nullable: false));
            AddColumn("dbo.Skills", "IsActive", c => c.Boolean(nullable: false));
            AddColumn("dbo.AgentToSkills", "IsActive", c => c.Boolean(nullable: false));
            AddColumn("dbo.Agents", "IsActive", c => c.Boolean(nullable: false));
        }
    }
}

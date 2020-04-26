namespace ControlPanel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAgentGroupRelation : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Agents", "Groupid", c => c.Int());
            CreateIndex("dbo.Agents", "Groupid");
            AddForeignKey("dbo.Agents", "Groupid", "dbo.Groups", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Agents", "Groupid", "dbo.Groups");
            DropIndex("dbo.Agents", new[] { "Groupid" });
            DropColumn("dbo.Agents", "Groupid");
        }
    }
}

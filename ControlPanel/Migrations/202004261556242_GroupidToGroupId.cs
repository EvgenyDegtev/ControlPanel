namespace ControlPanel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GroupidToGroupId : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Agents", new[] { "Groupid" });
            CreateIndex("dbo.Agents", "GroupId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Agents", new[] { "GroupId" });
            CreateIndex("dbo.Agents", "Groupid");
        }
    }
}

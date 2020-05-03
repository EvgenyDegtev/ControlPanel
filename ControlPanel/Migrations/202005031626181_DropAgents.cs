namespace ControlPanel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DropAgents : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Agents", "Group_Id", "dbo.Groups");
            DropForeignKey("dbo.Agents", "GroupId", "dbo.Groups");
            DropIndex("dbo.Agents", new[] { "GroupId" });
            DropIndex("dbo.Agents", new[] { "Group_Id" });
            DropTable("dbo.Agents");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Agents",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        Login = c.String(nullable: false, maxLength: 100),
                        Algorithm = c.Int(nullable: false),
                        IsAlgorithmAllowServiceLevel = c.Boolean(nullable: false),
                        WorkloadMaxContactsCount = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                        GroupId = c.Int(),
                        Group_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateIndex("dbo.Agents", "Group_Id");
            CreateIndex("dbo.Agents", "GroupId");
            AddForeignKey("dbo.Agents", "GroupId", "dbo.Groups", "Id");
            AddForeignKey("dbo.Agents", "Group_Id", "dbo.Groups", "Id");
        }
    }
}

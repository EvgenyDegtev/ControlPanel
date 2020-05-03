namespace ControlPanel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAgent1 : DbMigration
    {
        public override void Up()
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
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Groups", t => t.GroupId)
                .Index(t => t.GroupId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Agents", "GroupId", "dbo.Groups");
            DropIndex("dbo.Agents", new[] { "GroupId" });
            DropTable("dbo.Agents");
        }
    }
}

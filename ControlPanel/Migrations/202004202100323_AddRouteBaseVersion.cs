namespace ControlPanel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddRouteBaseVersion : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Routes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Key = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        SkillId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Skills", t => t.SkillId)
                .Index(t => t.SkillId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Routes", "SkillId", "dbo.Skills");
            DropIndex("dbo.Routes", new[] { "SkillId" });
            DropTable("dbo.Routes");
        }
    }
}

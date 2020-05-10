namespace ControlPanel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAgentToSkill : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AgentToSkills",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Level = c.String(nullable: false),
                        OrderIndex = c.Int(nullable: false),
                        BreakingMode = c.String(nullable: false),
                        Percent = c.Int(nullable: false),
                        AgentId = c.Int(nullable: false),
                        SkillId = c.Int(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Agents", t => t.AgentId, cascadeDelete: true)
                .ForeignKey("dbo.Skills", t => t.SkillId, cascadeDelete: true)
                .Index(t => t.AgentId)
                .Index(t => t.SkillId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AgentToSkills", "SkillId", "dbo.Skills");
            DropForeignKey("dbo.AgentToSkills", "AgentId", "dbo.Agents");
            DropIndex("dbo.AgentToSkills", new[] { "SkillId" });
            DropIndex("dbo.AgentToSkills", new[] { "AgentId" });
            DropTable("dbo.AgentToSkills");
        }
    }
}

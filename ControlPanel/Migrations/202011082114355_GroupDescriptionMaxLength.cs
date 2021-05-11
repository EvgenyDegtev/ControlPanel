namespace ControlPanel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GroupDescriptionMaxLength : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Reports",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        DateFrom = c.DateTime(nullable: false),
                        DateTo = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            //CreateIndex("dbo.Agents", "Name");
            //CreateIndex("dbo.Agents", "Login", unique: true);
            //CreateIndex("dbo.Skills", "Name");
            //CreateIndex("dbo.Skills", "Key", unique: true);
            //CreateIndex("dbo.Routes", "Name");
            //CreateIndex("dbo.Routes", "Key", unique: true);
            //CreateIndex("dbo.Groups", "Name", unique: true);
        }
        
        public override void Down()
        {
            DropIndex("dbo.Groups", new[] { "Name" });
            DropIndex("dbo.Routes", new[] { "Key" });
            DropIndex("dbo.Routes", new[] { "Name" });
            DropIndex("dbo.Skills", new[] { "Key" });
            DropIndex("dbo.Skills", new[] { "Name" });
            DropIndex("dbo.Agents", new[] { "Login" });
            DropIndex("dbo.Agents", new[] { "Name" });
            DropTable("dbo.Reports");
        }
    }
}

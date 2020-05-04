namespace ControlPanel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SkillBaseConstraints : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Skills", "Name", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.Skills", "Key", c => c.String(nullable: false, maxLength: 100));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Skills", "Key", c => c.String());
            AlterColumn("dbo.Skills", "Name", c => c.String());
        }
    }
}

namespace ControlPanel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GroupDescriptionMaxLength2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Groups", "Description", c => c.String(maxLength: 8000));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Groups", "Description", c => c.String());
        }
    }
}

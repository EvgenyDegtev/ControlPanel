namespace ControlPanel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIndexes : DbMigration
    {
        public override void Up()
        {
            CreateIndex("Agents", "Name", unique: false);
            CreateIndex("Routes", "Key", unique: true);
            CreateIndex("Routes", "Name", unique: false);
            CreateIndex("Skills", "Key", unique: true);
            CreateIndex("Skills", "Name", unique: false);
            CreateIndex("Groups", "Name", unique: true);
        }
        
        public override void Down()
        {
        }
    }
}

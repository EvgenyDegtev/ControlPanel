namespace ControlPanel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAgentIndex : DbMigration
    {
        public override void Up()
        {
            CreateIndex("Agents", "Login", unique: true,"UniqueLogin");
        }
        
        public override void Down()
        {
        }
    }
}

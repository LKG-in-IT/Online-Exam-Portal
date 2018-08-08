namespace OEP.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MakeNameUnique_LanguageTable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Languages", "Name", c => c.String(nullable: false, maxLength: 250));
            CreateIndex("dbo.Languages", "Name", unique: true, name: "Index");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Languages", "Index");
            AlterColumn("dbo.Languages", "Name", c => c.String(maxLength: 250));
        }
    }
}

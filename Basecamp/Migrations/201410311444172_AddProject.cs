namespace Basecamp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddProject : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Projects",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        name = c.String(),
                        description = c.String(),
                        archived = c.Boolean(nullable: false),
                        is_client_project = c.Boolean(nullable: false),
                        created_at = c.DateTime(nullable: false),
                        updated_at = c.DateTime(nullable: false),
                        trashed = c.Boolean(nullable: false),
                        color = c.String(),
                        draft = c.Boolean(nullable: false),
                        template = c.Boolean(nullable: false),
                        last_event_at = c.DateTime(nullable: false),
                        starred = c.Boolean(nullable: false),
                        url = c.String(),
                        app_url = c.String(),
                    })
                .PrimaryKey(t => t.id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Projects");
        }
    }
}

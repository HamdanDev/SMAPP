namespace SMAPP.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_tag_columns_to_Sponsor : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Sponsors", "UsernameTagType", c => c.String());
            AddColumn("dbo.Sponsors", "PasswordTagType", c => c.String());
            AddColumn("dbo.Sponsors", "SubmitTagType", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Sponsors", "SubmitTagType");
            DropColumn("dbo.Sponsors", "PasswordTagType");
            DropColumn("dbo.Sponsors", "UsernameTagType");
        }
    }
}

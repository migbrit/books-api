using FluentMigrator;

namespace BooksApi.Migrations
{
    public class CreateBooksTable : Migration
    {
        public override void Down()
        {
            Delete.Table("Books");
        }

        public override void Up()
        {
            Create.Table("Books")
            .WithColumn("Id").AsInt32().PrimaryKey().Identity()
            .WithColumn("Title").AsString(255).NotNullable()
            .WithColumn("Author").AsString(255).NotNullable()
            .WithColumn("Genre").AsString(255).NotNullable()
            .WithColumn("ReleaseDate").AsDateTime().Nullable();
        }
    }
}
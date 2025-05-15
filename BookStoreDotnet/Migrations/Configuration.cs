namespace BookStoreDotnet.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<BookStoreDotnet.BookStore>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "BookStoreDotnet.Config.BookStore";
        }

        protected override void Seed(BookStoreDotnet.BookStore context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.
        }
    }
}

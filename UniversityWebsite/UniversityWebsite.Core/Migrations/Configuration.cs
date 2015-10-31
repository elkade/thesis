using System.Data.Entity.Migrations;

namespace UniversityWebsite.Core.Migrations
{
    public class Configuration : DbMigrationsConfiguration<DomainContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(DomainContext context)
        {
            base.Seed(context);
            new Core.Migrations.InitialDataLoader(context).WithDefault();
        }
    }
}
using System.Data.Entity.Migrations;

namespace UniversityWebsite.Domain.Migrations
{
    public class Configuration : DbMigrationsConfiguration<DomainContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }
    }
}
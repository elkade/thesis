using System.Data.Entity.Migrations;

namespace UniversityWebsite.Core.Migrations
{
    /// <summary>
    /// Odpowiada za konfigurację migracji wykonywanych przy użyciu frameworka Entity
    /// </summary>
    public class Configuration : DbMigrationsConfiguration<DomainContext>
    {
        /// <summary>
        /// Konfiguruje migracje.
        /// </summary>
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
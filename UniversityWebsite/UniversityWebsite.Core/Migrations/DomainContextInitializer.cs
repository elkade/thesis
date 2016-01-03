using System.Data.Entity;

namespace UniversityWebsite.Core.Migrations
{
    /// <summary>
    /// Odpowiada za wywołanie inicjalizacji stanu początkowego bazy danych systemu.
    /// </summary>
    public class DomainContextInitializer : DropCreateDatabaseIfModelChanges<DomainContext>
    {
        protected override void Seed(DomainContext context)
        {
            base.Seed(context);
            new InitialDataLoader(context).WithDefault();
        }
    }
}
using System.Data.Entity;

namespace UniversityWebsite.Core.Migrations
{
    public class DomainContextInitializer : DropCreateDatabaseIfModelChanges<DomainContext>
    {
        protected override void Seed(DomainContext context)
        {
            base.Seed(context);
            new InitialDataLoader(context).WithDefault();
        }
    }
}
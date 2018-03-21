using System.Data.Entity;

namespace OEP.Data
{
    public class ApplicationDbInitializer:CreateDatabaseIfNotExists<OepDbContext>
    {
        public ApplicationDbInitializer()
        {
            
        }
    }
}
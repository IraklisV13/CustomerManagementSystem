using CustomerManagementSystem.DBContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace CustomerManagementSystem.Data
{
    public class DbInitializer
    {
        public static void Initialize(CMSContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context), "Database context is null. Cannot initialize the database.");
            }

            // Migrate schema and data
            context.Database.Migrate();

            // Check if database already exists else create it and initialize with data
            if (!(context.Database.GetService<IDatabaseCreator>() as RelationalDatabaseCreator).Exists())
            {
                // Ensure the database is created
                context.Database.EnsureCreated();
            }
        }
    }
}

using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using ContosoUniversity.Data;

namespace ContosoUniversityUnitTest.Utilities
{
    public static class Utilities
    {
        
        public static DbContextOptions<SchoolContext> TestSchoolContextOptions()
        {
            //Create a new service provider to create a new in-memory database.
            var serviceProvider = new ServiceCollection().AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();

            //Create a new options instance using an in-memory database and 
            //IServiceProvider that the context should resolve all of its services from. 
            var builder = new DbContextOptionsBuilder<SchoolContext>().UseInMemoryDatabase("InMemoryDb").UseInternalServiceProvider(serviceProvider);
                
            return builder.Options;
        }
    }
}

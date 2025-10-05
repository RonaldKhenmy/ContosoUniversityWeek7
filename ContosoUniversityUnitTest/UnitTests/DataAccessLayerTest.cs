using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ContosoUniversityUnitTest.UnitTests;
using ContosoUniversity.Data;
using Xunit;
using ContosoUniversityUnitTest.Utilities;
using Microsoft.EntityFrameworkCore;
using ContosoUniversity.Models;
using Microsoft.Extensions.DependencyInjection;


namespace ContosoUniversityUnitTest.UnitTests
{
    public class DataAccessLayerTest
    {
        [Fact]
        public async Task GetStudentsAsync_StudentsAreReturned()
        { 
            //Add another Utilities to get the correct class path.
            using (var db = new SchoolContext(Utilities.Utilities.TestSchoolContextOptions()))
            {
                //Arrange
                var expectedStudents = SchoolContext.GetSeedingStudents();
                await db.AddRangeAsync(expectedStudents);
                await db.SaveChangesAsync();

                //Act
                var result = await db.GetStudentsAsync();

                //Assert
                var actualStudents = Assert.IsAssignableFrom<List<Student>>(result);
                Assert.Equal(expectedStudents.OrderBy(m => m.ID).Select(m => m.LastName),
                    actualStudents.OrderBy(m => m.ID).Select(m => m.LastName));
            }
        }

        [Fact]
        public async Task AddStudentsAsync_StudentIsAdded()
        {
            using (var db = new SchoolContext(Utilities.Utilities.TestSchoolContextOptions()))
            {
                //Arrange
                var recId = 10;
                var expectedStudents = new Student() { ID = recId, FirstMidName = "IronFist", LastName = "Alexander", EnrollmentDate = DateTime.Now};

                //Act 
                await db.AddStudentsAsync(expectedStudents);

                //Assert
                var actualStudents = await db.FindAsync<Student>(recId);
                //Eliminates null value.
                Assert.NotNull(actualStudents);
                Assert.Equal(expectedStudents.LastName, actualStudents.LastName);

            }
        }

        [Fact]
        public async Task DeleteAllStudentsAsync_StudentsAreDeleted()
        {
            using (var db = new SchoolContext(Utilities.Utilities.TestSchoolContextOptions()))
            {
                //Arrange
                var seedStudents = SchoolContext.GetSeedingStudents();
                await db.AddRangeAsync(seedStudents);
                await db.SaveChangesAsync();

                //Act
                await db.DeleteAllStudentsAsync();

                //Assert
                Assert.Empty(await db.Students.AsNoTracking().ToListAsync());

            }
        }
        [Fact]
        public async Task DeleteStudentAsync_StudentIsDeleted_WhenStudentIsFound()
        {
            using (var db = new SchoolContext(Utilities.Utilities.TestSchoolContextOptions()))
            {
                //Arrange
                var seedStudents = SchoolContext.GetSeedingStudents();
                await db.AddRangeAsync(seedStudents);
                await db.SaveChangesAsync();
                var recId = 1;
                var expectedStudents =
                    seedStudents.Where(student => student.ID != recId).ToList();

                //Act 
                await db.DeleteStudentAsync(recId);

                //Assert
                var actualStudents = await db.Students.AsNoTracking().ToListAsync();
                Assert.Equal(
                    expectedStudents.OrderBy(m => m.ID).Select(m => m.ID),
                    actualStudents.OrderBy(m => m.ID).Select(m => m.ID));
            }
        }

        [Fact]
        public async Task DeleteStudentsAsync_NoStudentIsDeleted_WhenStudentIsNotFound()
        {
            using (var db = new SchoolContext(Utilities.Utilities.TestSchoolContextOptions()))
            {
                var expectedStudents = SchoolContext.GetSeedingStudents();
                await db.AddRangeAsync(expectedStudents);
                await db.SaveChangesAsync();
                var recId = 4;

                //Act
                try
                {
                    await db.DeleteStudentAsync(recId);
                } 
                catch
                {
                    // recId doesn't exist
                }

                //Assert 
                var actualStudents = await db.Students.AsNoTracking().ToListAsync();
                Assert.Equal(
                    expectedStudents.OrderBy(s => s.ID).Select(s => s.ID),
                    actualStudents.OrderBy(s => s.ID).Select(s => s.ID));
            }
        }
        
    }
} 

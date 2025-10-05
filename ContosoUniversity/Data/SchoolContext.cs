using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ContosoUniversity.Models;


namespace ContosoUniversity.Data
{
    public class SchoolContext : DbContext
    {
        public SchoolContext (DbContextOptions<SchoolContext> options)
            : base(options)
        {
        }
        public virtual DbSet<Student> Students { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Course> Courses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Course>().ToTable("Course");
            modelBuilder.Entity<Enrollment>().ToTable("Enrollment");
            modelBuilder.Entity<Student>().ToTable("Student");
        }
        //public DbSet<ContosoUniversity.Models.Student> Student { get; set; } = default!;


        public static List<Student> GetSeedingStudents()
        {
            return new List<Student>
            {
                new Student { ID = 1, FirstMidName = "Ronald", LastName = "Khenmy", EnrollmentDate = DateTime.Now },
                new Student { ID = 2, FirstMidName = "Sam", LastName = "Fischer", EnrollmentDate = DateTime.Now },
                new Student { ID = 3, FirstMidName = "Tony", LastName = "Redgrave", EnrollmentDate = DateTime.Now }
            };
        }
        public async virtual Task<List<Student>> GetStudentsAsync()
        {
            return await Students.OrderBy(S => S.LastName).AsNoTracking().ToListAsync();
        } 

        public async virtual Task AddStudentsAsync(Student student)
        {
            await Students.AddAsync(student);
            await SaveChangesAsync();
        } 

        public async virtual Task DeleteAllStudentsAsync()
        {
            foreach (Student student in Students)
            {
                Students.Remove(student);
            }
            await SaveChangesAsync();
        
        }
        public async virtual Task DeleteStudentAsync(int id)
        {
            var student = await Students.FindAsync(id);

            if (student != null)
            {
                Students.Remove(student);
                await SaveChangesAsync();
            } 

        } 

        public void Initialize()
        {
            Students.AddRange(GetSeedingStudents());
            SaveChanges();
        }
    }   
        
}

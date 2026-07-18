using Microsoft.EntityFrameworkCore;
using University_system.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
namespace University_system.Data
{
    public class DataContext : IdentityDbContext<User>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }


        public DbSet<User> Users { get; set; }

        public DbSet<WorkflowRequest>WorkflowRequests { get; set; }
        public DbSet<WorkflowTemplate> WorkflowTemplates { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }

        public DbSet<Semester> Semesters { get; set; }
        public DbSet<Student> Students { get; set; }

        public DbSet<Instructor> Instructors { get; set; }

        public DbSet<Department> Departments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<WorkflowRequest>()
                .HasOne(w => w.WorkflowTemplate)
              .WithMany() 
              .HasForeignKey(w => w.WorkflowTemplateId)
              .OnDelete(DeleteBehavior.Cascade); 


            modelBuilder.Entity<Course>()
                .HasOne(c => c.Instructor)
                .WithMany(i => i.Courses)
                .HasForeignKey(c => c.InstructorId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Course>()
                .HasOne(c =>c.Semester)
                .WithMany(s =>s.Courses)
                .HasForeignKey(c =>c.SemesterId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Enrollment>()
                .HasOne(e => e.Course)
                .WithMany(c  => c.Enrollments)
                .HasForeignKey(e  => e.CourseId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Enrollment>()
                .HasOne(e =>e.Student)
                .WithMany(s =>s.Enrollments)
                .HasForeignKey(e =>e.StudentId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Student>()
               .HasOne(s => s.User)
               .WithOne()
               .HasForeignKey<Student>(s => s.UserId)
               .OnDelete(DeleteBehavior.Cascade);     

            modelBuilder.Entity<Student>()
                .HasOne(s => s.Department)
                .WithMany(d => d.Students)
                .HasForeignKey(s => s.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);



            modelBuilder.Entity<Instructor>()
             .HasOne(i => i.User)
             .WithOne()
             .HasForeignKey<Instructor>(i => i.UserId)
             .OnDelete(DeleteBehavior.Cascade);


            modelBuilder.Entity<Instructor>()
             .HasOne(i => i.Department)
             .WithMany(d => d.Instructors)
             .HasForeignKey(i => i.DepartmentId)
             .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<WorkflowRequest>()
                .HasOne(w =>w.Student)
                .WithMany(s =>s.WorkflowRequests)
                .HasForeignKey(w=>w.StudentId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<WorkflowRequest>()
                .HasOne(w=>w.Semester)
                .WithMany(s => s.WorkflowRequests)
                .HasForeignKey(w =>w.SemesterId)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<Department>()
                .HasOne(d =>d.HeadInstructor)
                .WithOne()
                .HasForeignKey<Department>(d => d.HeadInstructorId)
                .OnDelete(DeleteBehavior.SetNull); 

        }

        public class DataContextFactory : IDesignTimeDbContextFactory<DataContext>
        {
            public DataContext CreateDbContext(string[] args)
            {
                var optionsBuilder = new DbContextOptionsBuilder<DataContext>();
                optionsBuilder.UseSqlServer("Server=.;Database=UniversityDb;Trusted_Connection=True;TrustServerCertificate=True;");

                return new DataContext(optionsBuilder.Options);
            }
        }
    }
}
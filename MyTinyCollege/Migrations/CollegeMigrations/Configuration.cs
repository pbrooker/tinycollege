namespace MyTinyCollege.Migrations.CollegeMigrations
{
    using Models;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<MyTinyCollege.DAL.SchoolContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            MigrationsDirectory = @"Migrations\CollegeMigrations";
        }

        protected override void Seed(MyTinyCollege.DAL.SchoolContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            //1. Add some students

            var students = new List<Student>
            {
                 new Student {FirstName="Carson",
                             LastName ="Alexander",
                             EnrollmentDate=DateTime.Parse("2015-02-01"),
                             Email = "calexander@tinycollege.com"},
                 new Student {FirstName="Peters",
                             LastName ="Jessica",
                             EnrollmentDate=DateTime.Parse("2015-02-01"),
                             Email = "jpeters@tinycollege.com"},
                 new Student {FirstName="Gizmoodo",
                             LastName ="Frank",
                             EnrollmentDate=DateTime.Parse("2015-02-01"),
                             Email = "fgizmodo@tinycollege.com"},
                 new Student {FirstName="April",
                             LastName ="May",
                             EnrollmentDate=DateTime.Parse("2015-02-01"),
                             Email = "amay@tinycollege.com"},
                 new Student {FirstName="Cartwright",
                             LastName ="Seth",
                             EnrollmentDate=DateTime.Parse("2015-02-01"),
                             Email = "scartwright@tinycollege.com"},
                 new Student {FirstName="Crabby",
                             LastName ="Jones",
                             EnrollmentDate=DateTime.Parse("2015-02-01"),
                             Email = "cjones@tinycollege.com"}
            };

            //Loop each student and add to database (only if they are not already present)
            //using the AddOrUpdate Method
            //THe first parameter of this method specifies the property to check if a row already
            //exists

            students.ForEach(s => context.Students.AddOrUpdate(p => p.Email, s));
            context.SaveChanges();


            //2. Add some instructors

            var instructors = new List<Instructor>
            {
                new Instructor  {FirstName="Paul",
                                LastName ="Brooker",
                                HireDate=DateTime.Parse("2000-09-01"),
                                Email = "pbrooker@faculty.tinycollege.com"},
                new Instructor  {FirstName="Frank",
                                LastName ="Bekkering",
                                HireDate=DateTime.Parse("1992-09-01"),
                                Email = "fbekkering@faculty.tinycollege.com"}
            };

            instructors.ForEach(s => context.Instructors.AddOrUpdate(p => p.Email, s));
            context.SaveChanges();

            //3. Add some departments

            var departments = new List<Department>
            {
                new Department {Name="Engineering", Budget=350000, StartDate=DateTime.Parse("2010-06-01"),
                                InstructorID=1 },
                new Department {Name="Business", Budget=100000, StartDate=DateTime.Parse("2011-09-01"),
                                InstructorID=2}
            };
            departments.ForEach(s => context.Departments.AddOrUpdate(p => p.Name, s));
            context.SaveChanges();

            //4. Add some courses


            var courses = new List<Course>
            {
                new Course {CourseID = 1045, Title = "Computer Science", Credits= 5, DepartmentID=1 },
                new Course {CourseID = 1095, Title = "Physics", Credits= 3, DepartmentID=1},
                new Course {CourseID = 1105, Title = "Calculus", Credits= 3, DepartmentID=1},
                new Course {CourseID = 1115, Title = "Business", Credits= 1, DepartmentID=2},

            };

            courses.ForEach(s => context.Courses.AddOrUpdate(p => p.CourseID, s));
            context.SaveChanges();


            //5. Add some enrollments

            var enrollments = new List<Enrollment>
            {
                new Enrollment {StudentID=1, CourseID=1045, Grade=Grade.A },
                new Enrollment {StudentID=1, CourseID=1095, Grade=Grade.B },
                new Enrollment {StudentID=2, CourseID=1105, Grade=Grade.A },
                new Enrollment {StudentID=2, CourseID=1115, Grade=Grade.C },
                new Enrollment {StudentID=3, CourseID=1045, Grade=Grade.A },
                new Enrollment {StudentID=3, CourseID=1095, Grade=Grade.D },
            };


            foreach(Enrollment e in enrollments)
            {
                var enrollmentInDataBase = context.Enrollments.Where(
                    s =>
                    s.StudentID == e.StudentID &&
                    s.course.CourseID == e.CourseID).SingleOrDefault();


                //SingleOrDefault:
                //Returns a single, specific element of a sequence of values,
                //or a default value if no such element is found.
                //Use when expecting 0 or 1 item
                //You get 0 when 0 or 1 expected (ok)
                //You get 1 when 0 or 1 expected (ok)
                //You get 2 or more when 0 or 1 was expected (error)

                //Single: returns a single, specific element of a sequence
                //Use when 1 item expected (not 0 or 2 and more)
                //You get 0 when 1 expected (error)
                //You get 1 when 1 expected (ok)
                //You get 2 or more when 1 was expected (error)

                if(enrollmentInDataBase == null)
                {
                    //enrollment was not found - add it to db context
                    context.Enrollments.Add(e);
                }


            }//end of foreach
            context.SaveChanges();



        }
    }
}

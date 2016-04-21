﻿
using System.Collections.Generic;

namespace MyTinyCollege.Models
{
    public class Course
    {
        public int CourseID { get; set; }
        public string Title { get; set; }
        public int Credits { get; set; }
        public int DepartmentID { get; set; }//FK to department

        //=================== Navigation properties ===================================

            //one course to many enrollments
        public virtual ICollection<Enrollment> Enrollment { get; set; }

        //1 course to many instructors
        public virtual ICollection<Instructor>  Instructors { get; set; }

        public virtual Department Department { get; set; }

        //Combine course id and title in one property
        public string CourseIdTitle
        {
            get
            {
                return CourseID + ": " + Title;
            }
        }
    }
}
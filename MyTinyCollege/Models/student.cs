using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyTinyCollege.Models
{
    public class Student:Person
    {
        public DateTime EnrollmentDate { get; set; }

        //1 student with many enrollments
        public virtual ICollection<Enrollment> Enrollments { get; set; }
    }
}
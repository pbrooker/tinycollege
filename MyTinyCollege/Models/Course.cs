
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyTinyCollege.Models
{
    public class Course
    {
        /* By default, the ID property will become the Primary Key of the database table
         * that corresponds to this class. By default the EF (Entity Framework)
         * interprets a property thats named ID or ClassNameID as the PK.
         * Also, this PK will have an IDENTITY property , you can override it using
         * the DatabaseGenerateOption enum:
         * -Computed: Database generates a value when row is inserted or updated
         * -Identity: Database generates a value when row is inserted 
         * -None:     Database does not generate values
         */
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Display(Name ="Number")]
        public int CourseID { get; set; }  //PK - Note: with No Identity Property

        [StringLength(50, MinimumLength =3)]
        public string Title { get; set; }

        [Range(0,5)]//Credits can be between 0 and 5
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
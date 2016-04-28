﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MyTinyCollege.DAL;
using MyTinyCollege.Models;
using MyTinyCollege.ViewModels;

namespace MyTinyCollege.Controllers
{
    public class InstructorController : Controller
    {
        private SchoolContext db = new SchoolContext();

        // GET: Instructor
        public ActionResult Index(int? id, int? courseID )
        {
            //int? id -> for determining whihc instructor was selected -> load related courses
            //int? courseID - >  determining which course was selected -> load related students
            // var instructor = db.Instructors.Include(i => i.OfficeAssignment);
            //return View(instructor.ToList());

            var viewModel = new InstructorIndexData();

            viewModel.Instructors = db.Instructors
                .Include(i => i.OfficeAssignment)
                .Include(i => i.Courses)
                .OrderBy(i => i.LastName);

            //check for id (an instructor has been selected -> get course that instructor teaches)

            if(id!=null)
            {

                ViewBag.InstructorID = id.Value;  //for UI: which instructor row is selected
                //Lazy loading
                viewModel.Courses = viewModel.Instructors
                                    .Where(i => i.ID == id.Value).Single().Courses;

                var InstructorName = viewModel.Instructors.Where(i => i.ID == id.Value).Single();
                ViewBag.InstructorName = InstructorName.FullName;
            }

            //check for courseID (a course has been selected -> get enrolled students)
            if(courseID!=null)
            {

                ViewBag.courseID = courseID.Value; //for UI: which course row is selected
                ////Lazy Loading
                //viewModel.Enrollments = viewModel.Courses
                //                        .Where(x => x.CourseID == courseID.Value)
                //                        .Single().Enrollment;

                //Explicity Loading
                var selectedCourse = viewModel.Courses
                                    .Where(x => x.CourseID == courseID.Value)
                                    .Single();
                db.Entry(selectedCourse).Collection(x => x.Enrollments).Load();
                foreach (Enrollment enrollment in selectedCourse.Enrollments)
                {
                    db.Entry(enrollment).Reference(x => x.student).Load();
                }
                viewModel.Enrollments = selectedCourse.Enrollments;

                //to send selected course title to UI
                ViewBag.CourseTitle = selectedCourse.Title;
            }
            //return the view attaching the viewModel (instructors, course, enrollments)
            return View(viewModel);
        }

        // GET: Instructor/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Instructor instructor = db.Instructors.Find(id);
            if (instructor == null)
            {
                return HttpNotFound();
            }
            return View(instructor);
        }

        // GET: Instructor/Create
        public ActionResult Create()
        {
            ViewBag.ID = new SelectList(db.OfficeAssignments, "InstructorID", "Location");
            return View();
        }

        // POST: Instructor/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,LastName,FirstName,Email,HireDate")] Instructor instructor)
        {
            if (ModelState.IsValid)
            {
                db.Instructors.Add(instructor);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ID = new SelectList(db.OfficeAssignments, "InstructorID", "Location", instructor.ID);
            return View(instructor);
        }

        // GET: Instructor/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Instructor instructor = db.Instructors.Find(id);
            //pbrooker: replaced scaffoleded code to include office assingments and courses
            //using eager loading
            Instructor instructor = db.Instructors
                                    .Include(i => i.OfficeAssignment)
                                    .Include(i => i.Courses)
                                    .Where(i => i.ID == id).Single();

            //for retrieving all courses and which ones are assigned to the 
            //currently selected instructor
            PopulateAssignedCourseData(instructor);

            if (instructor == null)
            {
                return HttpNotFound();
            }
            ViewBag.ID = new SelectList(db.OfficeAssignments, "InstructorID", "Location", instructor.ID);
            return View(instructor);
        }

        private void PopulateAssignedCourseData(Instructor instructor)
        {
            var allCourses = db.Courses; //need all courses to be displayed in view
            //populate a hash set of integers representing the courseID for each course that this instructor is teaching
            var instructorCourses = new HashSet<int>(instructor.Courses.Select(c => c.CourseID));

            var viewModel = new List<AssignedCourseData>();

            //Loop all courses and see if there is a match between all courses and instructor courses
            //if so, set assigned boolean accordingly
            foreach(var course in allCourses)
            {
                //Instantiate and Populate the AssignedCourseData object
                //and add this object to the viewModel
                viewModel.Add(new AssignedCourseData
                {
                    CourseId = course.CourseID,
                    Title = course.Title,
                    //set the assigned book if it contains the instructor course id
                    Assigned = instructorCourses.Contains(course.CourseID)
                });
            }

            //populate ViewBag object with viewModel
            ViewBag.Courses = viewModel;
        }

        // POST: Instructor/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,LastName,FirstName,Email,HireDate")] Instructor instructor)
        {
            if (ModelState.IsValid)
            {
                db.Entry(instructor).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ID = new SelectList(db.OfficeAssignments, "InstructorID", "Location", instructor.ID);
            return View(instructor);
        }

        // GET: Instructor/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Instructor instructor = db.Instructors.Find(id);
            if (instructor == null)
            {
                return HttpNotFound();
            }
            return View(instructor);
        }

        // POST: Instructor/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Instructor instructor = db.Instructors.Find(id);
            db.Instructors.Remove(instructor);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

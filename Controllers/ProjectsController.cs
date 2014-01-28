using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MvcPersonalSite.Models;

namespace MvcPersonalSite.Controllers
{ 
    public class ProjectsController : Controller
    {
        private PersonalSiteDBEntities1 db = new PersonalSiteDBEntities1();

        //
        // GET: /Projects/
        


        public ViewResult Index()
        {

            //This bit of code is meant to grab the Tag Names for the specific 
            // Projects, loop through the result and spit them out into a ViewBag.tagList
            // for the Index View. I am caught up on the where clause for the statememnt 
            //because I can't seem to 

            //var tags = (from t in db.PSTechTags
            //            join p in db.PSProjectTechTags on t.techTagID equals p.techTagID
            //            join q in db.PSProjects on p.projectID equals q.projectID
            //            where p.projectID == 
            //            select t.techName).ToList();


            //string tagCollection = String.Empty;

            //System.Text.StringBuilder sb = new System.Text.StringBuilder();


            //foreach (var tag in tags)
            //{
            //    sb.AppendFormat("  {0}  ", tag);
            //}

            //ViewBag.tagList = sb;


            var psprojects = db.PSProjects.Include("PSBizSolution").Include("PSCategory");

             return View(psprojects.ToList());
        }



        //
        // GET: /Projects/Details/5

        public ViewResult Details(int id)
        {

            PSProject psproject = db.PSProjects.Single(p => p.projectID == id);
            
            return View(psproject);
        }

        //
        // GET: /Projects/Create
        [Authorize(Roles = "admin")]
        public ActionResult Create()
        {
            ViewBag.businessSolution = new SelectList(db.PSBizSolutions, "bizID", "bizSolnName");
            ViewBag.category = new SelectList(db.PSCategories, "categoryID", "categoryName");
            ViewBag.techTag = new SelectList(db.PSTechTags, "techTagID", "techName");
           
            return View();
        } 

        //
        // POST: /Projects/Create
        [Authorize(Roles = "admin")]
        [HttpPost]
        public ActionResult Create(PSProject psproject, HttpPostedFileBase screenShotUrl)
        {
            if (ModelState.IsValid)
            {

                if (screenShotUrl != null)
                {
                    SetPicture(psproject, screenShotUrl);

                }
                else
                {
                    psproject.screenshot = "noImage.gif";
                }


                db.PSProjects.AddObject(psproject);
                db.SaveChanges();
                return RedirectToAction("Index");  
            }

            ViewBag.businessSolution = new SelectList(db.PSBizSolutions, "bizID", "bizSolnName", psproject.businessSolution);
            ViewBag.category = new SelectList(db.PSCategories, "categoryID", "categoryName", psproject.category);
            ViewBag.techTag = new SelectList(db.PSTechTags, "techTagID", "techName");

            return View(psproject);
        }
        
        //
        // GET: /Projects/Edit/5
        [Authorize(Roles = "admin")]
        public ActionResult Edit(int id)
        {
            PSProject psproject = db.PSProjects.Single(p => p.projectID == id);
            ViewBag.businessSolution = new SelectList(db.PSBizSolutions, "bizID", "bizSolnName", psproject.businessSolution);
            ViewBag.category = new SelectList(db.PSCategories, "categoryID", "categoryName", psproject.category);
            return View(psproject);
        }

        //
        // POST: /Projects/Edit/5
        [Authorize(Roles = "admin")]
        [HttpPost]
        public ActionResult Edit(PSProject psproject, HttpPostedFileBase screenShotUrl)
        {
            if (ModelState.IsValid)
            {
                if (screenShotUrl != null)
                {
                    SetPicture(psproject, screenShotUrl);
                }
                else
                {
                    psproject.screenshot = (from p in db.PSProjects
                                    where p.projectID == psproject.projectID
                                    select p.screenshot).Single();
                }

                db.PSProjects.Attach(psproject);
                db.ObjectStateManager.ChangeObjectState(psproject, EntityState.Modified);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.businessSolution = new SelectList(db.PSBizSolutions, "bizID", "bizSolnName", psproject.businessSolution);
            ViewBag.category = new SelectList(db.PSCategories, "categoryID", "categoryName", psproject.category);
            return View(psproject);
        }

        //
        // GET: /Projects/Delete/5
        [Authorize(Roles = "admin")]
        public ActionResult Delete(int id)
        {
            PSProject psproject = db.PSProjects.Single(p => p.projectID == id);
            return View(psproject);
        }

        //
        // POST: /Projects/Delete/5
        [Authorize(Roles = "admin")]
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            PSProject psproject = db.PSProjects.Single(p => p.projectID == id);
            db.PSProjects.DeleteObject(psproject);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
        [Authorize(Roles = "admin")]
        private void SetPicture(PSProject psproject, HttpPostedFileBase screenShotUrl)
        {
            //Takes the posted screenShotUrl and uploads it to the the data base

            string imgName = screenShotUrl.FileName;

            string ext = imgName.Substring(imgName.LastIndexOf("."));

            string newImgName = Guid.NewGuid().ToString();

            newImgName = newImgName + ext;

            screenShotUrl.SaveAs(Server.MapPath("~/Content/screenShots/" + newImgName));

            psproject.screenshot = newImgName;


        }


    }
}
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Ames.Entities;
using Ames.Infrastructue;

namespace EFileServer.Controllers
{
    public class EFileInfoController : Controller
    {
        private EFAmesInfra db = new EFAmesInfra();

        // GET: EFileInfo
        [ProfileAction]
        public ActionResult Index()
        {
            return View(db.EFileInfo.ToList());
        }

        // GET: EFileInfo/Details/5
        [ProfileAction]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EFileInfo eFileInfo = db.EFileInfo.Find(id);
            if (eFileInfo == null)
            {
                return HttpNotFound();
            }
            return View(eFileInfo);
        }

        // GET: EFileInfo/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: EFileInfo/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EFileID,CreatedDateTime,Year,Month,EFileName,Location,Brand,Department,Type,GeneratedFrom,ExpiryDate")] EFileInfo eFileInfo)
        {
            if (ModelState.IsValid)
            {
                db.EFileInfo.Add(eFileInfo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(eFileInfo);
        }

        // GET: EFileInfo/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EFileInfo eFileInfo = db.EFileInfo.Find(id);
            if (eFileInfo == null)
            {
                return HttpNotFound();
            }
            return View(eFileInfo);
        }

        // POST: EFileInfo/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ProfileAction]
        public ActionResult Edit([Bind(Include = "EFileID,CreatedDateTime,Year,Month,EFileName,Location,Brand,Department,Type,GeneratedFrom,ExpiryDate")] EFileInfo eFileInfo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(eFileInfo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(eFileInfo);
        }

        // GET: EFileInfo/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            EFileInfo eFileInfo = db.EFileInfo.Find(id);
            if (eFileInfo == null)
            {
                return HttpNotFound();
            }
            return View(eFileInfo);
        }

        // POST: EFileInfo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [ProfileAction]
        public ActionResult DeleteConfirmed(int id)
        {
            EFileInfo eFileInfo = db.EFileInfo.Find(id);
            db.EFileInfo.Remove(eFileInfo);
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

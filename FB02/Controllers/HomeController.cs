using FB02.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FB02.Controllers
{
    public class HomeController : Controller
    {
        ModelDB DB = new ModelDB();
        #region Index
        public ActionResult Index()
        {
            ViewBag.Categories= DB.Categories.ToList();
            var c = DB.Products.ToList();
            return View(c);
        }
        #endregion

        #region About
        public ActionResult About()
        {
            var c = DB.Categories.ToList();
            return View(c);
        }
        #endregion

        #region Contact
        public ActionResult Contact()
        {
            var c = DB.Categories.ToList();
            return View(c);
        }
        [HttpPost]
        public ActionResult Contact(Contact contact)
        {
            DB.Entry(contact).State = EntityState.Added;
            DB.SaveChanges();
            return RedirectToAction("Index");
        }
        #endregion

        #region Shop
        public ActionResult Shop()
        {
            ViewBag.Categories = DB.Categories.ToList();
            var c = DB.Products.ToList();
            return View(c);
        }
        #endregion

        #region Shopsingle
        public ActionResult Shopsingle(int id)
        {
            ViewBag.Categories = DB.Categories.ToList();
            var c = DB.Products.Find(id);
            return View(c);
        }
        #endregion

        #region portfolioitem
        public ActionResult portfolioitem(int id)
        {
            ViewBag.Categories = DB.Categories.ToList();
            var c = DB.Products.Find(id);
            return View(c);
        }
        #endregion

        #region portfoliocategory
        public ActionResult portfoliocategory(int id)
        {
            ViewBag.Categories = DB.Categories.ToList();
            var cat = DB.Categories.Find(id);
            ViewBag.Cat = cat.CName;
            var c = DB.Products.Where(i=>i.PCategory==id).ToList();
            return View(c);
        }
        #endregion
    }
}
using FB02.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FB02.Controllers
{
    public class AdminController : Controller
    {
        ModelDB DB = new ModelDB();
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        #region Logine
        public ActionResult login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult login(string username, string pass)
        {
            var u = DB.Useres.ToList();
            foreach (var item in u)
            {
                if (item.UserName == username && item.Password == pass)
                {
                    return RedirectToAction("Index");
                }
            }
            return View();
        }
        #endregion

        #region AddCategory
        [HttpGet]
        public ActionResult AddCategory()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddCategory(HttpPostedFileBase file, string name)
        {
            if (file != null && file.ContentLength > 0)
                try
                {
                    string path = Path.Combine(Server.MapPath("~/UPImage"),
                                               Path.GetFileName(file.FileName));
                    file.SaveAs(path);
                    Category C = new Category();
                    C.CName = name;
                    C.CImage = path;
                    DB.Entry(C).State = EntityState.Added;
                    DB.SaveChanges();
                    ViewBag.Message = "File uploaded successfully";
                }
                catch (Exception ex)
                {
                    ViewBag.Message = "ERROR:" + ex.Message.ToString();
                }
            else
            {
                ViewBag.Message = "You have not specified a file.";
            }
            return RedirectToAction("ShowCategory");
        }
        #endregion

        #region ShowCategory
        public ActionResult ShowCategory()
        {
             ViewBag.Cat = DB.Categories.ToList();
            var cats = DB.Products.ToList();
            return View(cats);
        }
        public ActionResult RetrieveImage(string id)
        {
            string cover = GetImageFromDataBase(id);
            if (cover != null)
            {
                return File(cover, "image/jpg");
            }
            else
            {
                return null;
            }
        }
        public string GetImageFromDataBase(string Id)
        {
            var q = from temp in DB.Categories where temp.CName == Id select temp.CImage;
            var cover = q.First();
            return cover;
        }


        #endregion

        #region CategoryDelete
        public ActionResult CategoryDelete(int id)
        {
            var emp = DB.Categories.Find(id);
            DB.Entry(emp).State = System.Data.Entity.EntityState.Deleted;
            DB.SaveChanges();
            return RedirectToAction("ShowCategory");
        }
        #endregion

        #region CategoryEdit
        [HttpGet]
        public ActionResult CategoryEdit(int id)
        {
            var cat = DB.Categories.Find(id);
            return View(cat);
        }

        [HttpPost]
        public ActionResult CategoryEdit(Category C, HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0)
                try
                {
                    string path = Path.Combine(Server.MapPath("~/UPImage"),
                                               Path.GetFileName(file.FileName));
                    file.SaveAs(path);
                    C.CImage = path;
                    DB.Entry(C).State = EntityState.Modified;
                    DB.SaveChanges();
                    ViewBag.Message = "File uploaded successfully";
                }
                catch (Exception ex)
                {
                    ViewBag.Message = "ERROR:" + ex.Message.ToString();
                }
            else
            {
                ViewBag.Message = "You have not specified a file.";
            }
            return RedirectToAction("ShowCategory");
        }
        #endregion

        #region AddProduct
        [HttpGet]
        public ActionResult AddProduct()
        {
            ViewBag.categories = DB.Categories.ToList();
            return View();
        }

        [HttpPost]
        public ActionResult AddProduct(HttpPostedFileBase file, string name, decimal salary, int Cat, string discraption)
        {
            if (file != null && file.ContentLength > 0)
                try
                {
                    string path = Path.Combine(Server.MapPath("~/UPImage"),
                                               Path.GetFileName(file.FileName));
                    file.SaveAs(path);
                    Product C = new Product();
                    C.PName = name;
                    C.PImage = path;
                    C.Salary = salary;
                    C.PCategory = Cat;
                    C.Discraption = discraption;
                    DB.Entry(C).State = EntityState.Added;
                    DB.SaveChanges();
                    ViewBag.Message = "File uploaded successfully";
                }
                catch (Exception ex)
                {
                    ViewBag.Message = "ERROR:" + ex.Message.ToString();
                }
            else
            {
                ViewBag.Message = "You have not specified a file.";
            }
            return RedirectToAction("ShowProduct");
        }
        #endregion

        #region ShowProduct
        public ActionResult ShowProduct()
        {
            var cats = DB.Products.ToList();
            return View(cats);
        }
        public ActionResult RetrieveImageProduct(string id)
        {
            string cover = GetImageFromDataBaseProduct(id);
            if (cover != null)
            {
                return File(cover, "image/jpg");
            }
            else
            {
                return null;
            }
        }
        public string GetImageFromDataBaseProduct(string Id)
        {
            var q = from temp in DB.Products where temp.PName == Id select temp.PImage;
            var cover = q.First();
            return cover;
        }

        #endregion

        #region ProductDelete
        public ActionResult ProductDelete(int id)
        {
            var emp = DB.Products.Find(id);
            DB.Entry(emp).State = System.Data.Entity.EntityState.Deleted;
            DB.SaveChanges();
            return RedirectToAction("ShowProduct");
        }
        #endregion

        #region ProductEdit
        [HttpGet]
        public ActionResult ProductEdit(int id)
        {
            var cat = DB.Products.Find(id);
            ViewBag.categories = DB.Categories.Where(i=>i.Cid != cat.Category.Cid).ToList();
            return View(cat);
        }

        [HttpPost]
        public ActionResult ProductEdit(Product C, HttpPostedFileBase file)
        {
            if (file != null && file.ContentLength > 0)
                try
                {
                    string path = Path.Combine(Server.MapPath("~/UPImage"),Path.GetFileName(file.FileName));
                    file.SaveAs(path);
                    C.PImage = path;
                    DB.Entry(C).State = EntityState.Modified;
                    DB.SaveChanges();
                    ViewBag.Message = "File uploaded successfully";
                }
                catch (Exception ex)
                {
                    ViewBag.Message = "ERROR:" + ex.Message.ToString();
                }
            else
            {
                ViewBag.Message = "You have not specified a file.";
            }
            return RedirectToAction("ShowProduct");
        }
        #endregion

        #region AddUser
        public ActionResult AddUser()
        {
            return View();
        }

        // POST: Employee/Create
        [HttpPost]
        public ActionResult AddUser(Usere newemp)
        {
            try
            {
                DB.Entry(newemp).State = EntityState.Added;
                DB.SaveChanges();
                return RedirectToAction("ShowUser");
            }
            catch
            {
                return View();
            }
        }

        #endregion

        #region ShowCategory
        public ActionResult ShowUser()
        {
            var cats = DB.Useres.ToList();
            return View(cats);
        }
        #endregion

        #region CategoryDelete
        public ActionResult UserDelete(int id)
        {
            var emp = DB.Useres.Find(id);
            DB.Entry(emp).State = EntityState.Deleted;
            DB.SaveChanges();
            return RedirectToAction("ShowUser");
        }
        #endregion

        #region UserEdit
        [HttpGet]
        public ActionResult UserEdit(int id)
        {
            var cat = DB.Useres.Find(id);
            return View(cat);
        }

        [HttpPost]
        public ActionResult UserEdit(Usere C)
        {
            try
            {
                DB.Entry(C).State = System.Data.Entity.EntityState.Modified;
                DB.SaveChanges();
                return RedirectToAction("ShowUser");
            }
            catch
            {
                return View();
            }
        }
        #endregion

        #region Showfeadback 
        public ActionResult feadback()
        {
            ViewBag.categories = DB.Categories.ToList();
            var c = DB.Contacts.ToList();
            return View(c);
        }
        #endregion

        #region feadbackDelete
        public ActionResult feadbackDelete(int id)
        {
            var emp = DB.Contacts.Find(id);
            DB.Entry(emp).State = EntityState.Deleted;
            DB.SaveChanges();
            return RedirectToAction("feadback");
        }
        #endregion
    }
}
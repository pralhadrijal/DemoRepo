using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Shopping.Models;
using System.Data;
using PagedList;

namespace SecondhandShopping.Controllers
{
    public class AdminController : Controller
    {
        ProjectDbEntities _entities = new ProjectDbEntities();
        public ActionResult Home()
        {
            ViewData["CountUser"] = _entities.UserInfoes.Where(x => x.UserRole == "User").Count();
            ViewData["CountNormal"] = _entities.PostAds.Where(x => !x.IsFeatured).Count();
            ViewData["CountFeature"] = _entities.PostAds.Where(x => x.IsFeatured && x.Status == "active").Count();
            ViewData["PendingAds"] = _entities.PostAds.Where(x => x.Status == "pending").Count();

            ViewData["PendingFeatures"] = _entities.PostAds.Where(x => x.Status == "pending" && !x.IsHide).ToList();
            return View();
        }
        public ActionResult NormalAds(int page = 1)
        {
            IPagedList<PostAd> ads = _entities.PostAds.OrderByDescending(x => x.Id).Where(x => !x.IsFeatured).ToPagedList(page, 8);
            return View(ads);
        }
        public ActionResult FeatureAds(int page = 1)
        {
            IPagedList<PostAd> ads = _entities.PostAds.OrderByDescending(x => x.Id).Where(x => x.IsFeatured && x.Status == "active").ToPagedList(page, 8);
            return View(ads);
        }
        public ActionResult FeatureTrue(int id)
        {
            PostAd postAd = _entities.PostAds.Find(id);
            postAd.Status = "active";
            _entities.Entry(postAd).State = EntityState.Modified;
            _entities.SaveChanges();
            return RedirectToAction("Home", "Admin");
        }

        /*Administrator Login*/
        public ActionResult AdminLogin()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AdminLogin(UserInfo user)
        {
            var v = _entities.UserInfoes.Where(a => a.Email.Equals(user.Email) && a.Password.Equals(user.Password) && a.UserRole.Equals("Admin")).FirstOrDefault();
            if (v != null)
            {
                Session["LoggedAdminId"] = v.Id.ToString();
                Session["LoggedUserName"] = v.UserName.ToString();
                return RedirectToAction("Home", "Admin");
            }
            else
            {
                Session["LoginFailed"] = "Username or Password doesn't match";
                return RedirectToAction("AdminLogin", "Admin");
            }
        }
        /*Administrator Login*/
        /*Administrator Logout*/
        public ActionResult LogOut()
        {
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("AdminLogin", "Admin");
        }
        /*Administrator Logout*/
        /*Category & Product*/
        public ActionResult CategoryAndProduct()
        {
            ViewData["CategoryId"] = new SelectList(_entities.Categories, "Id", "Category");
            ViewData["CountCategories"] = _entities.Categories.Count();
            ViewData["CountProducts"] = _entities.Products.Count();
            return View();
        }
        [HttpPost, ActionName("CategoryAndProduct")]
        public ActionResult SaveCategory(Category category)
        {
            category.Name = Convert.ToString(Session["Category"]);
            if (ModelState.IsValid)
            {
                _entities.Categories.Add(category);
                _entities.SaveChanges();
                return RedirectToAction("CategoryAndProduct");
            }
            return View(category);
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetAllProductByCategory(int categoryid)
        {
            bool Isvalid = Convert.ToBoolean(categoryid);
            var products = _entities.Products.Where(x => x.CategoryId == categoryid).ToList();
            var result = (from p in products select new { id = p.Id, product = p.Name }).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        /*Category & Product*/



        /*User Record*/
        public ActionResult UserRecord()
        {
            ViewData["CountUser"] = _entities.UserInfoes.Where(x => x.UserRole == "User").Count();
            return View(_entities.UserInfoes.OrderByDescending(x => x.Id).Where(x => x.UserRole == "User").ToList());
        }
        /*User Record*/

        public ActionResult UserDetails(int id)
        {
            ViewData["User'sPost"] = _entities.PostAds.Where(x => x.UserId == id).ToList();
            ViewData["TotalPosts"] = _entities.PostAds.Where(x => x.UserId == id).Count();
            UserInfo userdetails = _entities.UserInfoes.Where(x => x.Id == id).Single();
            return View(userdetails);

        }
        [HttpGet]

        [HttpPost]
        public ActionResult AddCategory(Category category)
        {
            if (ModelState.IsValid)
            {
                _entities.Categories.Add(category);
                _entities.SaveChanges();
                ViewData["SuccessMessage"] = "Category Successfully Added";
                return RedirectToAction("AddCategory");
            }
            return View(category);
        }

        /*Spam*/
        public ActionResult Spam()
        {
            return View();
        }
        /*Spam*/

        /*Feature Ads*/
        public ActionResult FeatureRequest()
        {
            IList<PostAd> ads = _entities.PostAds.OrderByDescending(x => x.Id).Where(x => x.IsFeatured && x.Status == "Pending").ToList();
            return View(ads);
        }
        /*Feature Ads*/

        /*Delet Feature Ads*/
        public bool DeletePost(int id)
        {
            try
            {
                PostAd ads = _entities.PostAds.Find(id);
                _entities.PostAds.Remove(ads);
                _entities.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public JsonResult DeleteFeatureAd(int id)
        {
            bool result = DeletePost(id);
            return Json(result);

        }
    }
}

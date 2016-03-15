using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Shopping.Models;

namespace Shopping.Controllers
{
    public class HomeController : Controller
    {
        ProjectDbEntities _entities;
        public HomeController()
        {
            _entities = new ProjectDbEntities();
        }
        public ActionResult Index()
        {
            /*Displaying Category*/
            ViewData["Category"] = _entities.Categories.OrderBy(x => x.Name).Where(x => x.IsHide == false).ToList();
            ViewData["Featured"] = _entities.PostAds.Where(x => (!x.IsHide && x.IsFeatured == true && x.Status == "active")).OrderByDescending(x => x.Id).Take(4);
            var model = _entities.PostAds.Where(x => (!x.IsHide && x.IsFeatured == false)).OrderByDescending(x => x.Id).Take(4);
            ViewData["OurStore"] = _entities.OurStores.ToList();
            return View(model);
        }      

        /*Search*/
        public ActionResult SearchLayout()
        {
            return View();
        }
        public ActionResult Search(string search)
        {
            ViewData["Category"] = _entities.Categories.OrderBy(x => x.Name).Where(x => x.IsHide == false).ToList();
            if (_entities.PostAds.Include("Category").Where(x => (!x.IsHide) && (x.Status == "active")).Where(x => x.Title == search.ToLower() || search == null || x.Product.Name == search.ToLower() || search == null || x.Category.Name == search || search == null || x.UserInfo.UserName == search || search == null).Count() == 0)
            {
                TempData["NoProduct"] = "No product found with this Search...try Again";
            }
            else
            {
                ViewData["Search"] = _entities.PostAds.Include("Category").Where(x => (!x.IsHide) && (x.Status == "active")).Where(x => x.Title == search.ToLower() || search == null || x.Product.Name == search.ToLower() || search == null || x.Category.Name == search || search == null || x.UserInfo.UserName == search || search == null).Take(10);
            }

            return View();
        }
        public ActionResult SearchByCategory(int id)
        {
            ViewData["Category"] = _entities.Categories.OrderBy(x => x.Name).Where(x => x.IsHide == false).ToList();
            if (_entities.PostAds.Where(x => x.CategoryId == id).Where(x => (!x.IsHide) && (x.Status == "active")).Count() == 0)
            {
                TempData["NoProduct"] = "Currently,no more products available in this category...try visiting later";
            }
            else
            {
                ViewData["ProductsByCategory"] = _entities.PostAds.Where(x => x.CategoryId == id).Where(x => (!x.IsHide) && (x.Status == "active")).ToList();
            }
            return View();
        }
        public ActionResult SeeAllAds()
        {
            ViewData["Category"] = _entities.Categories.OrderBy(x => x.Name).Where(x => x.IsHide == false).ToList();
            return View(_entities.PostAds.Where(x => (!x.IsHide) && (!x.IsFeatured) && (x.Status == "active")).ToList());
        }
        public ActionResult SeeAllFeature()
        {
            ViewData["Category"] = _entities.Categories.OrderBy(x => x.Name).Where(x => x.IsHide == false).ToList();
            return View(_entities.PostAds.Where(x => (!x.IsHide) && (x.IsFeatured) && (x.Status == "active")).ToList());
        }
        public ActionResult RelatedProducts(int id)
        {
            ViewData["Category"] = _entities.Categories.OrderBy(x => x.Name).Where(x => x.IsHide == false).ToList();
            var model = _entities.PostAds.Where(x => (!x.IsHide) && (x.Status == "active")).Where(x => x.ProductId == id).ToList();
            return View(model);
        }
        //public ActionResult SeeAllProductByUser(int id)
        //{
        //    ViewData["Category"] = _entities.Categories.OrderBy(x => x.Name).Where(x => x.IsHide == false).ToList();
        //    var model = _entities.PostAds.Where(x => (!x.IsHide) && (x.Status == "active")).Where(x => x.UserId == id).ToList();
        //    return View(model);
        //}
        public ActionResult AdvanceSearch(string category, string location, string condition)
        {
            ViewData["Category"] = _entities.Categories.OrderBy(x => x.Name).Where(x => x.IsHide == false).ToList();
            if (_entities.PostAds.Include("Category").Where(x => (!x.IsHide) && (x.Status == "active")).Where(x => x.Category.Name == category.ToLower() || category == null || x.UserInfo.City == location.ToLower() || location == null || x.Condition == condition || condition == null).Count() == 0)
            {
                TempData["NoProduct"] = "No product found with this Search...try Again";
            }
            else
            {
                var model = (_entities.PostAds.Include("Category").Where(x => (!x.IsHide) && (x.Status == "active")).Where(x => x.Category.Name == category.ToLower() || category == null || x.UserInfo.City == location.ToLower() || location == null || x.Condition == condition || condition == null).Take(10));
                return View(model);
            }
            return View();

        }

        /*Search*/



        /*Displaying Recent Product Details*/
        public ActionResult RecentProductDetails(int id)
        {
            PostAd ads = _entities.PostAds.Find(id);
            int userid = ads.UserId;
            int productid = ads.ProductId;
            int LoggedId = Convert.ToInt16(Session["LoggedUserId"]);

            /*Displaying Category*/
            ViewData["Category"] = _entities.Categories.OrderBy(x => x.Name).Where(x => x.IsHide == false).ToList();
            /*Displaying Category*/

            /*Displaying location*/
            ViewData["location"] = _entities.UserInfoes.OrderBy(x => x.City).Distinct();
            /*Displaying location*/

            /*Checking Wish*/
            if (userid == Convert.ToInt32(Session["LoggedUserId"]))
            {
                TempData[""] = "";
            }
            else if (_entities.WishLists.Where(x => x.UserId == LoggedId && x.PostId == id).Count() == 0)
            {
                TempData["AddWish"] = "Add to Wishlist";
            }
            else
            {
                ViewData["Wishlist"] = _entities.WishLists.Where(x => x.PostId == id).Single();
            }
            /*Checking Wish*/


            /*Displaying Related Products*/
            if (_entities.PostAds.Where(x => x.ProductId == productid).Count() > 0)
            {
                ViewData["Related Products"] = _entities.PostAds.Where(x => x.ProductId == productid).Take(4).ToList();
            }
            /*Displaying Related Products*/

            var recentpoatadsdetail = _entities.PostAds.Include("UserInfo");
            ViewData["PostDetails"] = recentpoatadsdetail.Where(x => x.Id == id).Single();
            

            if (_entities.PostAds.Where(x => (!x.IsHide) && (x.Status == "active")).Where(x => x.UserId == userid).Count() == 1)
            {
                ViewData["No More Ads"] = "No more product available with this seller";
            }
            else
            {
                ViewData["MoreAds"] = _entities.PostAds.Where(x => (!x.IsHide) && (x.Status == "active")).Where(x => x.UserId.Equals(userid)).OrderByDescending(x => x.Id).Take(4).ToList();
            }
            return View();
        }
        /*Displaying Recent Product Details*/

        /*Add item to WishList*/
        public ActionResult AddtoWishlist(int id)
        {
            WishList mywish = new WishList();
            mywish.PostId = id;
            mywish.UserId = Convert.ToInt32(Session["LoggedUserId"]);
            mywish.At = DateTime.Now;
            mywish.IsHide = false;
            mywish.Remark = "Hello,this is remarks";

            _entities.WishLists.Add(mywish);
            _entities.SaveChanges();
            return RedirectToAction("RecentProductDetails", new { id = id });
        }
        /*Add item to WishList*/


        /*Remove item from WishList*/
        public ActionResult RemoveWishlist(int id)
        {
            WishList wish = _entities.WishLists.Find(id);
            int Recentid = wish.PostId;
            _entities.WishLists.Remove(wish);
            _entities.SaveChanges();
            return RedirectToAction("RecentProductDetails", new { id = Recentid });
        }
        /*Remove item from WishList*/
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Shopping.Models;
using System.IO;
using System.Data;

namespace SecondhandShopping.Controllers
{
    public class UserController : Controller
    {
        ProjectDbEntities _entities;
        Validation _validation;
        public UserController()
        {
            _entities = new ProjectDbEntities();
            _validation = new Validation();
        }

        /*Registration for the user*/
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Register(UserInfo member)
        {
            if (_validation.UserNameIfExist(member.UserName))
            {
                ModelState.AddModelError("UserName", "Username already exist. Try using Symbols and Numbers");
                return View(member);
            }
            if (_validation.EmailIfExist(member.Email))
            {
                ModelState.AddModelError("Email", "Email already exist");
                return View(member);
            }
            member.JoinDate = DateTime.Now;
            member.UserRole = "User";
            member.IsActive = true;
            if (ModelState.IsValid)
            {
                _entities.UserInfoes.Add(member);
                _entities.SaveChanges();
                ModelState.Clear();
                member = null;
                ViewBag.Message = "Registration Successfully Completed";
            }
            return View(member);
        }
        /*Registration for the user*/

        /*Login for the user*/
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public String Login(string username, string password)
        {
            var v = _entities.UserInfoes.Where(a => a.UserName.Equals(username) && a.Password.Equals(password)).FirstOrDefault();
            if (v != null && v.UserRole == "User")
            {
                Session["LoggedUserId"] = v.Id.ToString();
                Session["LoggedUserName"] = v.UserName.ToString();

                return "1";
            }
            else
            {
                return "2";
            }
        }
        /*Login for the user*/



        /*Welcome User after Login*/
        public ActionResult WelcomeUser()
        {
            if (Session["LoggedUserId"] != null)
            {
                ViewData["UserId"] = Session["LoggedUserId"];
                return View();
            }
            else
            {
                return RedirectToAction("Index");
            }
        }
        /*Welcome User after Login*/



        /*Logged Out the User*/
        public ActionResult LogOut()
        {
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("Index", "Home");
        }
        /*Logged Out the User*/


        /*User Home*/
        public ActionResult DashboardforMyHome()
        {
            ViewData["CountUser"] = _entities.UserInfoes.Where(x => x.UserRole == "User").Count();
            ViewData["CountMyPosts"] = _entities.PostAds.Where(x => x.IsHide == false).Count();
            return View();
        }
        /*User Home*/



        /*Post Ads by User*/
        [HttpGet]
        public ActionResult DashBoardforNewPost()
        {
            ViewData["CategoryId"] = new SelectList(_entities.Categories, "Id", "Name");
            ViewData["ProductId"] = new SelectList(_entities.Products, "Id", "Name");
            return View();
        }

        [HttpPost, ActionName("DashBoardforNewPost")]
        public ActionResult AddPost(PostAd postads)
        {
            postads.IsHide = false;
            postads.PostedDate = DateTime.Now;
            if (postads.IsFeatured)
                postads.Status = "pending";
            else
                postads.Status = "active";


            HttpPostedFileBase file1 = Request.Files["file1"];
            HttpPostedFileBase file2 = Request.Files["file2"];
            HttpPostedFileBase file3 = Request.Files["file3"];

            #region VariableDeclaration
            string filename1 = Path.GetFileName(file1.FileName);
            string filename2 = Path.GetFileName(file2.FileName);
            string filename3 = Path.GetFileName(file3.FileName);
            string DefaultImage1 = "Anon1.jpg";
            string DefaultImage2 = "Anon2.jpg";
            string DefaultImage3 = "Anon3.jpg";

            string fileExtension1 = Path.GetExtension(file1.FileName);
            if (fileExtension1 == "")
                fileExtension1 = ".jpg";

            string fileExtension2 = Path.GetExtension(file2.FileName);
            if (fileExtension2 == "")
                fileExtension2 = ".jpg";

            string fileExtension3 = Path.GetExtension(file3.FileName);
            if (fileExtension3 == "")
                fileExtension3 = ".jpg";

            int fileSize1 = file1.ContentLength;
            if (fileSize1 == 0)
                fileSize1 = 5995;

            int fileSize2 = file2.ContentLength;
            if (fileSize2 == 0)
                fileSize2 = 5995;

            int fileSize3 = file3.ContentLength;
            if (fileSize3 == 0)
                fileSize3 = 5995;
            #endregion

            if (ModelState.IsValid)
            {
                if (filename1 == "" && filename2 == "" && filename3 == "")
                {
                    TempData["AllImageNull"] = "Please upload atleast one image of your Product";
                }
                else
                {
                    if (!(fileExtension1.ToLower() == ".jpg" || fileExtension1.ToLower() == ".jpeg" || fileExtension1.ToLower() == ".png"))
                    {
                        TempData["FileExtensionError"] = "Image(s) extension must be in .jpg OR .jpeg OR .png format ";
                    }
                    else if (!(fileExtension2.ToLower() == ".jpg" || fileExtension2.ToLower() == ".jpeg" || fileExtension2.ToLower() == ".png"))
                    {
                        TempData["FileExtensionError"] = "Image(s) extension must be in .jpg OR .jpeg OR .png format ";
                    }
                    else if (!(fileExtension3.ToLower() == ".jpg" || fileExtension3.ToLower() == ".jpeg" || fileExtension3.ToLower() == ".png"))
                    {
                        TempData["FileExtensionError"] = "Image(s) extension must be in .jpg OR .jpeg OR .png format ";
                    }
                    else if (fileSize1 > 102400 || fileSize2 > 102400 || fileSize3 > 102400)
                    {
                        TempData["FileSizeError"] = "Product image exceed the maximum size of 100KB.";
                    }
                    else
                    {
                        if (filename1 != "" && filename2 != "" && filename3 != "")
                        {
                            file1.SaveAs(Server.MapPath("~/Content/Images/img1/" + filename1));
                            file2.SaveAs(Server.MapPath("~/Content/Images/img2/" + filename2));
                            file3.SaveAs(Server.MapPath("~/Content/Images/img3/" + filename3));

                            string filepath1 = "../../Content/Images/img1/" + filename1;
                            string filepath2 = "../../Content/Images/img2/" + filename2;
                            string filepath3 = "../../Content/Images/img3/" + filename3;

                            postads.Image1 = Convert.ToString(filepath1);
                            postads.Image2 = Convert.ToString(filepath2);
                            postads.Image3 = Convert.ToString(filepath3);
                        }
                        else if (filename1 == "" && filename2 != "" && filename3 != "")
                        {
                            file1.SaveAs(Server.MapPath("~/Content/Images/img1/" + DefaultImage1));
                            file2.SaveAs(Server.MapPath("~/Content/Images/img2/" + filename2));
                            file3.SaveAs(Server.MapPath("~/Content/Images/img3/" + filename3));

                            string filepath1 = "../../Content/Images/img1/" + DefaultImage1;
                            string filepath2 = "../../Content/Images/img2/" + filename2;
                            string filepath3 = "../../Content/Images/img3/" + filename3;

                            postads.Image1 = Convert.ToString(filepath1);
                            postads.Image2 = Convert.ToString(filepath2);
                            postads.Image3 = Convert.ToString(filepath3);
                        }
                        else if (filename1 == "" && filename2 != "" && filename3 == "")
                        {
                            file1.SaveAs(Server.MapPath("~/Content/Images/img1/" + DefaultImage1));
                            file2.SaveAs(Server.MapPath("~/Content/Images/img2/" + filename2));
                            file3.SaveAs(Server.MapPath("~/Content/Images/img3/" + DefaultImage3));

                            string filepath1 = "../../Content/Images/img1/" + DefaultImage1;
                            string filepath2 = "../../Content/Images/img2/" + filename2;
                            string filepath3 = "../../Content/Images/img3/" + DefaultImage3;

                            postads.Image1 = Convert.ToString(filepath1);
                            postads.Image2 = Convert.ToString(filepath2);
                            postads.Image3 = Convert.ToString(filepath3);
                        }
                        else if (filename1 == "" && filename2 == "" && filename3 != "")
                        {
                            file1.SaveAs(Server.MapPath("~/Content/Images/img1/" + DefaultImage1));
                            file2.SaveAs(Server.MapPath("~/Content/Images/img2/" + DefaultImage2));
                            file3.SaveAs(Server.MapPath("~/Content/Images/img3/" + filename3));

                            string filepath1 = "../../Content/Images/img1/" + DefaultImage1;
                            string filepath2 = "../../Content/Images/img2/" + DefaultImage2;
                            string filepath3 = "../../Content/Images/img3/" + filename3;

                            postads.Image1 = Convert.ToString(filepath1);
                            postads.Image2 = Convert.ToString(filepath2);
                            postads.Image3 = Convert.ToString(filepath3);
                        }
                        else
                        {
                            file1.SaveAs(Server.MapPath("~/Content/Images/img1/" + filename1));
                            file2.SaveAs(Server.MapPath("~/Content/Images/img2/" + DefaultImage2));
                            file3.SaveAs(Server.MapPath("~/Content/Images/img3/" + DefaultImage3));

                            string filepath1 = "../../Content/Images/img1/" + filename1;
                            string filepath2 = "../../Content/Images/img2/" + DefaultImage2;
                            string filepath3 = "../../Content/Images/img3/" + DefaultImage3;

                            postads.Image1 = Convert.ToString(filepath1);
                            postads.Image2 = Convert.ToString(filepath2);
                            postads.Image3 = Convert.ToString(filepath3);
                        }

                        try
                        {
                            _entities.PostAds.Add(postads);
                            _entities.SaveChanges();
                            ModelState.Clear();
                            postads = null;
                            TempData["SuccessMessage"] = "You have Posted your Ads Successfully";
                        }
                        catch (Exception)
                        {
                            return RedirectToAction("DashBoardforNewPost");
                        }

                    }
                }
            }
            return RedirectToAction("DashBoardforNewPost");
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult GetAllProductByCategory(int categoryid)
        {
            bool Isvalid = Convert.ToBoolean(categoryid);
            var products = _entities.Products.Where(x => x.CategoryId == categoryid).ToList();
            var result = (from p in products select new { id = p.Id, product = p.Name }).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        /*Post Ads by User*/



        /*User's Profile*/
        public ActionResult DashBoardforMyProfile(int id)
        {
            var model = _entities.UserInfoes.Where(x => x.Id == id).Single();
            return View(model);
        }

        [HttpGet]
        public ActionResult ChangePassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ChangePassword(UserInfo user)
        {
            if (ModelState.IsValid)
            {
                _entities.UserInfoes.Add(user);
                _entities.SaveChanges();
                return RedirectToAction("DashBoardforMyProfile/" + Session["LoggedUserId"]);
            }
            return View(user);
        }

        [HttpGet]
        public ActionResult ChangePersonalInfo(int id)
        {
            var model = _entities.UserInfoes.Find(id);
            return View(model);
        }
        [HttpPost]
        [ActionName("ChangePersonalInfo")]
        public ActionResult ChangePersonalInfo_Post(UserInfo user)
        {
            if (ModelState.IsValid)
            {
            _entities.Entry(user).State = EntityState.Modified;
            _entities.SaveChanges();
            return RedirectToAction("DashBoardforMyProfile", new { id = user.Id });
            }
            return View(user);
        }

        [HttpGet]
        public ActionResult ChangeContactInfo(int id)
        {
            var model = _entities.UserInfoes.Where(x => x.Id == id).Single();
            return View(model);
        }
        [HttpPost]
        public ActionResult ChangeContactInfo(UserInfo user)
        {
            if (ModelState.IsValid)
            {
                _entities.Entry(user).State = EntityState.Modified;
                _entities.SaveChanges();
                //return RedirectToAction("DashBoardforMyProfile/" + Session["Id"]);
                return RedirectToAction("DashBoardforMyProfile", new { id = user.Id });
            }
            return View(user);
        }
        /*User's Profile*/



        /*User's Post*/
        public ActionResult DashBoardforMyPost()
        {
            int userid = Convert.ToInt32(Session["LoggedUserId"]);
            var details = _entities.PostAds.Where(x => x.UserId.Equals(userid)).ToList();
            ViewData["TotalPosts"] = details.Count();
            return View(details);
        }

        [HttpGet]
        public ActionResult UserPostEdit(int id)
        {
            PostAd edit = _entities.PostAds.Find(id);
            if (edit == null)
            {
                return HttpNotFound();
            }
            ViewData["Details"] = edit;
            Session["Id"] = edit.Id;
            Session["CategoryId"] = edit.CategoryId;
            Session["ProductId"] = edit.ProductId;
            Session["UserId"] = edit.UserId;
            return View(edit);
        }

        [HttpPost, ActionName("UserPostEdit")]
        public ActionResult SaveChanges(PostAd Ads)
        {
            Ads.CategoryId = Convert.ToInt32(Session["CategoryId"]);
            Ads.ProductId = Convert.ToInt32(Session["ProductId"]);
            Ads.UserId = Convert.ToInt32(Session["UserId"]);

            //HttpPostedFileBase file1 = Request.Files["file1"];
            //HttpPostedFileBase file2 = Request.Files["file2"];
            //HttpPostedFileBase file3 = Request.Files["file3"];

            //#region VariableDeclaration
            //string filename1 = Path.GetFileName(file1.FileName);
            //string filename2 = Path.GetFileName(file2.FileName);
            //string filename3 = Path.GetFileName(file3.FileName);
            //string DefaultImage = "Default.jpg";

            //string fileExtension1 = Path.GetExtension(file1.FileName);
            //if (fileExtension1 == "")
            //    fileExtension1 = ".jpg";

            //string fileExtension2 = Path.GetExtension(file2.FileName);
            //if (fileExtension2 == "")
            //    fileExtension2 = ".jpg";

            //string fileExtension3 = Path.GetExtension(file3.FileName);
            //if (fileExtension3 == "")
            //    fileExtension3 = ".jpg";

            //int fileSize1 = file1.ContentLength;
            //if (fileSize1 == 0)
            //    fileSize1 = 500000;

            //int fileSize2 = file2.ContentLength;
            //if (fileSize2 == 0)
            //    fileSize2 = 500000;

            //int fileSize3 = file3.ContentLength;
            //if (fileSize3 == 0)
            //    fileSize3 = 500000;
            //#endregion

            //if (filename1 == "" && filename2 == "" && filename3 == "")
            //{
            //    Session["AllImageNull"] = "Please upload atleast one photo of your Product";
            //}
            //else
            //{
            //    //if ((fileExtension1.ToLower() != ".jpg" || fileExtension1.ToLower() != ".jpeg" || fileExtension1.ToLower() != ".png") && (fileExtension2.ToLower() != ".jpg" || fileExtension2.ToLower() != ".jpeg" || fileExtension2.ToLower() != ".png") && (fileExtension3.ToLower() != ".jpg" || fileExtension3.ToLower() != ".jpeg" || fileExtension3.ToLower() != ".png"))
            //    //{
            //    //    Session["FileExtensionError"] = "File extension does't match.";
            //    //}
            //    //else
            //    //{
            //    if (fileSize1 > 512000 || fileSize2 > 512000 || fileSize3 > 512000)
            //    {
            //        Session["FileSizeError"] = "Product image size exceed the maximum size of 500KB.";
            //    }
            //    else
            //    {
            //        if (filename1 != "" && filename2 != "" && filename3 != "")
            //        {
            //            file1.SaveAs(Server.MapPath("~/Content/Images/img1/" + filename1));
            //            file2.SaveAs(Server.MapPath("~/Content/Images/img2/" + filename2));
            //            file3.SaveAs(Server.MapPath("~/Content/Images/img3/" + filename3));

            //            string filepath1 = "../../Content/Images/img1/" + filename1;
            //            string filepath2 = "../../Content/Images/img2/" + filename2;
            //            string filepath3 = "../../Content/Images/img3/" + filename3;

            //            Ads.Image1 = Convert.ToString(filepath1);
            //            Ads.Image2 = Convert.ToString(filepath2);
            //            Ads.image3 = Convert.ToString(filepath3);
            //        }
            //        else if (filename1 == "" && filename2 != "" && filename3 != "")
            //        {
            //            file1.SaveAs(Server.MapPath("~/Content/Images/img1/" + DefaultImage));
            //            file2.SaveAs(Server.MapPath("~/Content/Images/img2/" + filename2));
            //            file3.SaveAs(Server.MapPath("~/Content/Images/img3/" + filename3));

            //            string filepath1 = "../../Content/Images/img1/" + DefaultImage;
            //            string filepath2 = "../../Content/Images/img2/" + filename2;
            //            string filepath3 = "../../Content/Images/img3/" + filename3;

            //            Ads.Image1 = Convert.ToString(filepath1);
            //            Ads.Image2 = Convert.ToString(filepath2);
            //            Ads.image3 = Convert.ToString(filepath3);
            //        }
            //        else if (filename1 == "" && filename2 != "" && filename3 == "")
            //        {
            //            file1.SaveAs(Server.MapPath("~/Content/Images/img1/" + DefaultImage));
            //            file2.SaveAs(Server.MapPath("~/Content/Images/img2/" + filename2));
            //            file3.SaveAs(Server.MapPath("~/Content/Images/img3/" + DefaultImage));

            //            string filepath1 = "../../Content/Images/img1/" + DefaultImage;
            //            string filepath2 = "../../Content/Images/img2/" + filename2;
            //            string filepath3 = "../../Content/Images/img3/" + DefaultImage;

            //            Ads.Image1 = Convert.ToString(filepath1);
            //            Ads.Image2 = Convert.ToString(filepath2);
            //            Ads.image3 = Convert.ToString(filepath3);
            //        }
            //        else if (filename1 == "" && filename2 == "" && filename3 != "")
            //        {
            //            file1.SaveAs(Server.MapPath("~/Content/Images/img1/" + DefaultImage));
            //            file2.SaveAs(Server.MapPath("~/Content/Images/img2/" + DefaultImage));
            //            file3.SaveAs(Server.MapPath("~/Content/Images/img3/" + filename3));

            //            string filepath1 = "../../Content/Images/img1/" + DefaultImage;
            //            string filepath2 = "../../Content/Images/img2/" + DefaultImage;
            //            string filepath3 = "../../Content/Images/img3/" + filename3;

            //            Ads.Image1 = Convert.ToString(filepath1);
            //            Ads.Image2 = Convert.ToString(filepath2);
            //            Ads.image3 = Convert.ToString(filepath3);
            //        }
            //        else
            //        {
            //            file1.SaveAs(Server.MapPath("~/Content/Images/img1/" + filename1));
            //            file2.SaveAs(Server.MapPath("~/Content/Images/img2/" + DefaultImage));
            //            file3.SaveAs(Server.MapPath("~/Content/Images/img3/" + DefaultImage));

            //            string filepath1 = "../../Content/Images/img1/" + filename1;
            //            string filepath2 = "../../Content/Images/img2/" + DefaultImage;
            //            string filepath3 = "../../Content/Images/img3/" + DefaultImage;

            //            Ads.Image1 = Convert.ToString(filepath1);
            //            Ads.Image2 = Convert.ToString(filepath2);
            //            Ads.image3 = Convert.ToString(filepath3);
            //        }
            //    }                
            //}
            if (ModelState.IsValid)
            {
                _entities.Entry(Ads).State = EntityState.Modified;
                _entities.SaveChanges();
                return RedirectToAction("UserPostDetails/" + Session["Id"]);
            }
            return View(Ads);
        }

        public bool CheckPostDelete(int id)
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
        [HttpPost]
        public JsonResult DeletePost(int id)
        {
            bool result = CheckPostDelete(id);
            return Json(result);
        }

        public bool Sold(int id)
        {
            try
            {
                PostAd ads = _entities.PostAds.Find(id);
                ads.IsHide = true;
                ads.Status = "sold";
                _entities.Entry(ads).State = EntityState.Modified;
                _entities.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        [HttpPost]
        public JsonResult SoldPost(int id)
        {
            bool sold = Sold(id);
            return Json(sold);
        }

        public bool UnSold(int id)
        {
            try
            {
                PostAd ads = _entities.PostAds.Find(id);
                ads.Status = "active";
                ads.IsHide = false;
                _entities.Entry(ads).State = EntityState.Modified;
                _entities.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public JsonResult UnSoldPost(int id)
        {
            bool unsold = UnSold(id);
            return Json(unsold);
        }

        public ActionResult UserPostDetails(int postid=0,int wishid=0)
        {
            if (postid != 0)
            {
                TempData["postid"] = "";
                ViewBag.Ads = _entities.PostAds.Find(postid);
            }
            if (wishid != 0)
            {
                TempData["wishid"] = "";
                ViewBag.wish = _entities.WishLists.Find(wishid);
            }
            return View();
            
        }
        /*User's Post*/


        /*Wish List*/
        public ActionResult DashBoardforWishList()
        {
            int userid = Convert.ToInt32(Session["LoggedUserId"]);
            var details = _entities.WishLists.Where(x => x.UserId.Equals(userid)).ToList();
            return View(details);
        }
        public bool CheckWishDelete(int id)
        {
            try
            {
                WishList ads = _entities.WishLists.Find(id);
                _entities.WishLists.Remove(ads);
                _entities.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public JsonResult DeleteWish(int id)
        {
            bool result = CheckWishDelete(id);
            return Json(result);
        }
        /*Wish List*/



        /*Need Help*/
        public ActionResult DashBoardforNeedHelp(int id)
        {
            return View();
        }
        /*Need Help*/
    }
}


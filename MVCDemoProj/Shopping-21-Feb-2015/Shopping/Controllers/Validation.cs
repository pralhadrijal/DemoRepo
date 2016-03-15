using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Shopping.Models;


namespace SecondhandShopping.Controllers
{
    public class Validation
    {
        ProjectDbEntities _entities;
        public Validation()
        {
            _entities = new ProjectDbEntities();
        }

        /*Validation for already exist Username and Email Address*/
        public bool UserNameIfExist(string UName)
        {
            var UserName = _entities.UserInfoes.Where(x => x.UserName == UName).FirstOrDefault();
            
            if (UserName != null)
                return true;
            else
                return false;
        }
        public bool EmailIfExist(string mail)
        {

            var Email = _entities.UserInfoes.Where(x => x.Email == mail).FirstOrDefault();
            if (Email != null)
                return true;
            else
                return false;
        }
        }
        /*Validation for already exist Username and Email Address*/
    }

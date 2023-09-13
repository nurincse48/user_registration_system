using RegistrationWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RegistrationWebApp.Controllers
{
    public class UserController : Controller
    {
        User_Info user_Info = new User_Info();
        public ActionResult user_registration()
        {
            return View();
        }
        [HttpPost]
        public ActionResult user_registration(User_Info user_Info)
        {
            user_Info.password = CreateMD5(user_Info.password).ToLower();
            using (UserRegistrationDBEntities dBEntities = new UserRegistrationDBEntities())
            {
                if (dBEntities.User_Info.Any(x => x.user_id == user_Info.user_id))
                {
                    return Json(new { success = false, message = "User Already Exists" });
                }
                dBEntities.User_Info.Add(user_Info);
                dBEntities.SaveChanges();
            }
            ModelState.Clear();
            return Json(new { success = true, message = "Registration has Completed Successfully." });
        }
        public string CreateMD5(string password)
        {
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(password);
                byte[] hashBytes = md5.ComputeHash(inputBytes);
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }
    }
}
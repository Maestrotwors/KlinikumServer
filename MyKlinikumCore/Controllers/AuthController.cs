using System;
using Microsoft.AspNetCore.Mvc;

namespace MyKlinikumCore
{
    public class AuthController : Controller
    {
        public ActionResult Logout()
        {
            if (Request.Cookies["Session"] != null)
            {
                //Response.Cookies.Delete("Session");
                string CookieSession = Request.Cookies["Session"];
                Response.Cookies.Delete("Login");
                Response.Cookies.Delete("Session");
                //Response.Cookies.Append("Login", "");
                //Response.Cookies.Append("Session", "");
                Params.Sess.Remove(CookieSession);
            }
            return RedirectToAction("Index", "Main");
            //return Content("<script>window.location.replace('/login')</script>");
            //return RedirectToAction("Login", "Auth");
            //return View("Logout");
        }
    }
}

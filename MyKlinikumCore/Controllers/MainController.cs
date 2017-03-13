using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MyKlinikumCore
{
    //Ubrat
    public class User
    {
        public string Id { get; set; }
        public string Namer { get; set; }
        public string Password { get; set; }
    }

    public class ViewDataFormat
    {
        public List<Dictionary<string, string>> Questions;
        public List<Dictionary<string, string>> Answers; 
    }


    public partial class MainController : Controller
    {


        public IActionResult Index()
        {
            string CookieSession = Request.Cookies["Session"];
            DB db = new DB();

            if (CookieSession != null)
            {
                if (Params.Sess.ContainsKey(CookieSession)) {
                    ViewDataFormat DataToView = new ViewDataFormat();
                    ViewData["Questions"] = db.SelectQuerySimple("select Questions.Id,Questions.Question,questionsfolder.FolderId,ShowTable,AnswersCount FROM questionsfolder  INNER JOIN Questions ON Questions.Id= questionsfolder.QuestionId ORDER BY Questions.Id");
                    ViewData["Answers"] = db.SelectQuerySimple("select Answers.Id,QuestionId,Answer,Type,Description,Min,Max,Einheit from Answers LEFT JOIN normwerte ON normwerte.AnswerId=Answers.Id order by Id ");
                    return View("Main",ViewBag);
                }
                else { return AuthError(); }
            }
            else
            {
                return AuthError();
            }
        }

        public ContentResult AuthError()
        {
            var contentAuthError = "<script>window.location.replace('/login')</script>";
            return new ContentResult()
            {
                Content = contentAuthError,
                ContentType = "text/html",
            };
            //return Content("<html><body>Hello World</body></html>","text/html");
        }
    }
}

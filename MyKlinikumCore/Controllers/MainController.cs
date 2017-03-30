using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace MyKlinikumCore
{ 
    public class ViewDataFormat
    {
        public List<Dictionary<string, string>> Questions;
        public List<Dictionary<string, string>> Answers; 
    }


    public partial class MainController : Controller
    {


        public IActionResult Index()
        {
            /*List<List<string>> GetFiltered(List<List<string>> data)
            {
                List<List<string>> ReturnData = null;
                //List <List<string>> InputData = db.SelectQuerySimple("SELECT n.Id, n.PatientId,n.ReanimationDateTime,p.Name,p.Vorname,p.GebDate,p.Gender  FROM Notfalls n INNER JOIN Patients p ON p.id=n.PatientId");
                foreach (var element in data)
                {
                    ReturnData.Add(element);
                }
                return ReturnData;
            }*/

            string CookieSession = Request.Cookies["Session"];
            DB db = new DB();

            if (CookieSession != null)
            {
                if (Params.Sess.ContainsKey(CookieSession)) {
                    //ViewDataFormat DataToView = new ViewDataFormat();
                    //Models.Models_Statistik Model = new Models.Models_Statistik();// "------------------";

                    ViewData["Questions"] = db.SelectQuerySimple("select Questions.Id,Questions.Question,questionsfolder.FolderId,ShowTable,AnswersCount FROM questionsfolder  INNER JOIN Questions ON Questions.Id= questionsfolder.QuestionId ORDER BY Questions.Id");
                    ViewData["Answers"] = db.SelectQuerySimple("select Answers.Id,QuestionId,Answer,Type,Description,Min,Max,Einheit from Answers LEFT JOIN normwerte ON normwerte.AnswerId=Answers.Id order by Id ");
                    ViewData["Statistik_Tabs"] = db.SelectQuerySimple("Select * from Statistik_Tabs");
                    ViewData["Statistik_Tables"] = db.SelectQuerySimple("SELECT st.Id AS TableId,st.Data, st.Name,stit.TabId AS TabId FROM Statistik_Tables st INNER JOIN Statistik_Tables_In_Tabs stit ON st.Id = stit.TableId");
                    ViewData["Statistik_Columns"] = db.SelectQuerySimple("SELECT stc.Id,stc.Table_Id,stc.Column_Name, stc.Formule  FROM Statistik_Table_Columns stc");
                    ViewData["Statistik_Notfalls"] = db.SelectQuerySimple("SELECT n.Id, n.PatientId,n.ReanimationDateTime,p.Name,p.Vorname,p.GebDate,p.Gender  FROM Notfalls n INNER JOIN Patients p ON p.id=n.PatientId");
                    //ViewData["Statistik_ParamValues"] = db.SelectQuerySimple("SELECT AnswerId, Value from NotfallParamValue");
                    ViewData["Statistik_ParamValues"] = db.SelectQuerySimple("SELECT AnswerId, Value,NotfallId,a.QuestionId from NotfallParamValue npv INNER JOIN Answers a ON a.Id=npv.AnswerId");
                    //List <List<string>> Columns = db.SelectQuerySimple("SELECT stc.Id,stc.Table_Id,stc.Column_Name, stc.Formule  FROM Statistik_Table_Columns stc");

                    return View("Main" );
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

using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;

namespace MyKlinikumCore
{
    public class SessionElement
    {
        public string UserId;
        public string UserLogin;
        public string UserPassword;
    }

    public static class Params {
        public static Dictionary<string, SessionElement> Sess = new Dictionary<string, SessionElement>();
        /*public static string GetUserIdFromSession(string Val)
        {
            return Sess[Val]; 
        }*/
        public static void SessionAdd(string Key, SessionElement Element)
        {
            try { Sess.Add(Key, Element); } catch { }
        }
    }

 
    
    public partial class ApiController : Controller
    {
        
        [HttpPost]
        public ActionResult new_notfall()
        {
            DB db = new DB();
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("PatName", Request.Form["PatVorname"]);
            dic.Add("PatVorName", Request.Form["PatName"]);
            dic.Add("PatGebDate", Request.Form["PatGebDate"]);
            dic.Add("ReanimationDateTime", Request.Form["ReanimationDateTime"]);
            dic.Add("Gender", Request.Form["Gender"]);
            return Content(db.ExecuteProcedure("NotfallCreate", dic));
        }

        [HttpPost]
        public ActionResult get_patient_info()
        {
            DB db = new DB();
            SqlCommand command = new SqlCommand("SELECT n.PatientId,N.Id AS NotfallId, Name,Vorname,p.GebDate, p.Gender,n.ReanimationDateTime FROM Patients p INNER JOIN Notfalls n ON p.Id = n.PatientId where n.Id=@NotfallID", db.myConnection);
            string NotfallId = Request.Form["NotfallId"].ToString();
            command.Parameters.Add(new SqlParameter("NotfallID", NotfallId));
            return Content(JsonConvert.SerializeObject(db.SelectCommandParam(command)));
        }

        

        [HttpPost]
        public ActionResult get_notfall_data()
        {
            DB db = new DB();
            SqlCommand command =new SqlCommand("SELECT n.Id, n.PatientId,n.ReanimationDateTime,p.Name,p.Vorname,p.GebDate,p.Gender  FROM Notfalls n INNER JOIN Patients p ON p.id=n.PatientId WHERE n.Id=@NotfallID", db.myConnection);
            string param1 = Request.Form["NotfallId"].ToString();
            command.Parameters.Add(new SqlParameter("NotfallID", param1));
            return Content(JsonConvert.SerializeObject(db.SelectCommandParam(command)));
        }

        [HttpPost]
        public ActionResult get_notfall_values()
        {
            DB db = new DB();
            SqlCommand command = new SqlCommand("SELECT AnswerId, Value from NotfallParamValue where NotfallId=@NotfallID", db.myConnection);
            string param1 = Request.Form["NotfallId"].ToString();
            command.Parameters.Add(new SqlParameter("NotfallID", param1));
            return Content(JsonConvert.SerializeObject(db.SelectCommandParam(command)));
        }

        public class Statistik_ReturnData_Class
        {
            //public List<Dictionary<string, string>> Tables;
            //public List<Dictionary<string, string>> Columns;
            public List<Dictionary<string, string>> Notfalls;
            public List<Dictionary<string, string>> Param_Values;
        }
        public ActionResult get_statistik_data()
        {
            DB db = new DB();
            //SqlCommand command_Tables = new SqlCommand("SELECT st.Id AS TableId,st.Data, st.Name,stit.TabId AS TabId FROM Statistik_Tables st INNER JOIN Statistik_Tables_In_Tabs stit ON st.Id = stit.TableId", db.myConnection);
            //List<Dictionary<string, string>> Tables = db.SelectCommandParam(command_Tables);

            //SqlCommand command_Columns = new SqlCommand("SELECT stc.Id,stc.Table_Id,stc.Column_Name, stc.Formule  FROM Statistik_Table_Columns stc", db.myConnection);
            //List<Dictionary<string, string>> Columns = db.SelectCommandParam(command_Columns);

            SqlCommand command_Notfalls = new SqlCommand("SELECT n.Id, n.PatientId,n.ReanimationDateTime,p.Name,p.Vorname,p.GebDate,p.Gender  FROM Notfalls n INNER JOIN Patients p ON p.id=n.PatientId", db.myConnection);
            List<Dictionary<string, string>> Notfalls = db.SelectCommandParam(command_Notfalls);

            SqlCommand command_Param_Values = new SqlCommand("SELECT AnswerId, Value,NotfallId,a.QuestionId from NotfallParamValue npv INNER JOIN Answers a ON a.Id=npv.AnswerId", db.myConnection);
            List<Dictionary<string, string>> Param_Values = db.SelectCommandParam(command_Param_Values);

            Statistik_ReturnData_Class ReturnData =new Statistik_ReturnData_Class();
            //ReturnData.Tables = Tables;
            //ReturnData.Columns = Columns;
            ReturnData.Notfalls = Notfalls;
            ReturnData.Param_Values = Param_Values;
            return Content(JsonConvert.SerializeObject(ReturnData));
        }

        [HttpPost]
        public ActionResult save_param_value()
        {
            if (Request.Cookies["Session"] != null)
            {
                SessionElement Session = Params.Sess[Request.Cookies["Session"]];
                string UserId = Session.UserId;
                if (UserId!=null)
                {
                    DB db = new DB();
                    Dictionary<string, string> dic = new Dictionary<string, string>();
                    dic.Add("NotfallId", Request.Form["NotfallId"]);
                    dic.Add("AnswerId", Request.Form["AnswerId"]);
                    dic.Add("Value", Request.Form["ThisValue"]);
                    dic.Add("UserId", UserId);
                    dic.Add("DT", System.DateTime.Now.ToString());
                    return Content(db.ExecuteProcedure("ParameterChange", dic));
                }
            }
            return Content("Error");
        }

        [HttpPost]
        public ActionResult delete_notfall()
        {
            if (Request.Cookies["Session"] != null)
            {
                SessionElement Session = Params.Sess[Request.Cookies["Session"]];
                string UserId = Session.UserId;
                if (UserId != null)
                {
                    DB db = new DB();
                    Dictionary<string, string> dic = new Dictionary<string, string>();
                    dic.Add("PatientId", Request.Form["PatientId"]);
                    db.ExecuteProcedure("NotfallDelete", dic);
                    return Content("1");
                }
            }
            return Content("Error");
            
        }

        [HttpGet]
        public ActionResult Notfalls()
        {
            DB db = new DB();
            return Content(JsonConvert.SerializeObject(db.SelectQuery(
              "select Notfalls.Id,Patients.Name AS PatName,Patients.Vorname as PatVorname,Patients.GebDate,Patients.Gender,Notfalls.ReanimationDateTime,PatientId from notfalls INNER JOIN Patients ON PatientId = Patients.Id")));
        }

        /*[HttpGet]
        public ActionResult getNotfallsDataMap()
        {
            DB db = new DB();
            return Content("[{\"Questions\":" + JsonConvert.SerializeObject(db.SelectQuery(
              "select Id,ParamName as ParamName, ParentId as ParentId FROM Questions")) + "},{\"Answers\":" + JsonConvert.SerializeObject(db.SelectQuery(
              "select Id,QuestionId,Answer,Type from Answers"))+"}]");
        }*/

        [HttpPost]
        public ActionResult passwordChange()
        {
            string AltePass = Request.Form["altePass"].ToString();
            string NeuePass = Request.Form["neuePass"].ToString();
            string UserId="0";

            string CookieSession = Request.Cookies["Session"];
            DB db = new DB();

            if (CookieSession != null)
            {
                if (Params.Sess.ContainsKey(CookieSession))
                {
                    SessionElement Session = Params.Sess[Request.Cookies["Session"]];
                    UserId = Session.UserId;
                }
            }


            

            SqlCommand command = new SqlCommand("select * from users where Id=@Id and Password=@Password", db.myConnection);
            command.Parameters.Add(new SqlParameter("Id", UserId));
            command.Parameters.Add(new SqlParameter("Password", AltePass));
            List<Dictionary<string, string>> responce = db.SelectCommandParam(command);
            int content = responce.Count;

            if (content > 0)
            { 
                db.ExecuteQuery("update users SET Password=" + NeuePass + " where Id=" + UserId);
                Params.Sess.Remove(Request.Cookies["Session"]); 
            }
            else
            {
                return Content("0");
            }

            return Content("1");
        }    

        [HttpPost]
        public ActionResult Login()
        {
            DB db = new DB();
            SqlCommand command = new SqlCommand("SELECT Id,Login,Password from users where Login=@Login and Password=@Password", db.myConnection);
            string login = Request.Form["Login"].ToString();
            string password = Request.Form["Password"].ToString();
            command.Parameters.Add(new SqlParameter("Login", login));
            command.Parameters.Add(new SqlParameter("Password", password));
            List<Dictionary<string,string>> responce = db.SelectCommandParam(command);
            int content = responce.Count;
            
             

            if (content==1)
            {
                string UserId = responce[0]["Id"].ToString();
                string Login = responce[0]["Login"].ToString();
                string Password = responce[0]["Password"].ToString();
                string source = Login + "---"+ System.DateTime.Now.ToString();
                MD5 md5Hash = MD5.Create();
                string SessionNummer = GetMd5Hash(md5Hash, source); 
                Response.Cookies.Append("Session", SessionNummer);
                Response.Cookies.Append("Login", Login);

                SessionElement Element=new SessionElement();
                Element.UserId = UserId;
                Element.UserLogin= Login;
                Element.UserPassword = Password;
                Params.SessionAdd(SessionNummer, Element);
                return Content("1");
            }
            else
            {
                string CookieSession = null;
                try { CookieSession=Request.Cookies["Session"]; } catch { }
                Response.Cookies.Append("Login", "");
                Response.Cookies.Append("Session", "");
                if (CookieSession != null) { Params.Sess.Remove(CookieSession); }
                return Content("Login/Password ist ungültig");
            }
        }

        [HttpGet]
        public ActionResult Error()
        {
            return Content("Api путь не сущеаствует");
        }




        static string GetMd5Hash(MD5 md5Hash, string input)
        {

            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

    }
}

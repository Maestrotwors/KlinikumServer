﻿using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;

namespace MyKlinikumCore
{
    public static class Params {
        public static Dictionary<string, string> Sess = new Dictionary<string, string>();
        public static string GetUserIdFromSession(string Val)
        {
            return Sess[Val]; 
        }
        public static void SessionAdd(string Key, string Val)
        {
            Sess.Add(Key, Val);
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
            return Content(db.ExecuteProcedure("NotfallCreate", dic));
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

        [HttpPost]
        public ActionResult save_param_value()
        {
            if (Request.Cookies["Session"] != null)
            {
                string UserId = Params.Sess[Request.Cookies["Session"]];
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


        [HttpGet]
        public ActionResult Notfalls()
        {
            DB db = new DB();
            return Content(JsonConvert.SerializeObject(db.SelectQuery(
              "select Notfalls.Id,Patients.Name AS PatName,Patients.Vorname as PatVorname,Patients.GebDate,Patients.Gender,Notfalls.ReanimationDateTime from notfalls INNER JOIN Patients ON PatientId = Patients.Id")));
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
                string source = Login + "---"+ System.DateTime.Now.ToString();
                MD5 md5Hash = MD5.Create();
                string SessionNummer = GetMd5Hash(md5Hash, source); 
                Response.Cookies.Append("Session", SessionNummer);
                Response.Cookies.Append("Login", Login);

                Params.SessionAdd(SessionNummer, UserId);
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
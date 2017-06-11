using System;
using System.Collections.Generic; 
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;

namespace MyKlinikumCore
{
    public partial class DB 
    { 
        public SqlConnection myConnection = new SqlConnection(ConnectionString);
        public bool ExecuteQuery(string Query)
        {
            //SqlConnection myConnection = new SqlConnection(ConnectionString);
            try
            {
                myConnection.Open();
                SqlCommand myCommand = new SqlCommand(Query, myConnection); 
                myCommand.ExecuteNonQuery(); 
                myConnection.Close();
                return true;
            } catch (Exception e){ myConnection.Close(); return false; };  
        }

        /*public string TestProcedure(string PatName, string PatVorName, string PatGebDate, string ReanimationDateTime)
        {
            try
            {
                myConnection.Open();
                SqlCommand command = new SqlCommand("NotfallCreate", myConnection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                SqlParameter nameParam = new SqlParameter
                {
                    ParameterName = "@PatName",
                    Value = PatName
                }; command.Parameters.Add(nameParam);

                SqlParameter VorNameParam = new SqlParameter
                {
                    ParameterName = "@PatVorName",
                    Value = PatVorName
                }; command.Parameters.Add(VorNameParam);

                SqlParameter GebDateParam = new SqlParameter
                {
                    ParameterName = "@PatGebDate",
                    Value = PatGebDate
                }; command.Parameters.Add(GebDateParam);

                SqlParameter SqlReanimationDateTime = new SqlParameter
                {
                    ParameterName = "@ReanimationDateTime",
                    Value = ReanimationDateTime
                }; command.Parameters.Add(SqlReanimationDateTime);


                var result = command.ExecuteScalar();
                //var result = command.ExecuteNonQuery(); 
                return result.ToString();
            }
            catch (Exception e) { return "0"; };
        }*/
        public string ExecuteProcedure(string ProcedureName,Dictionary<string,string> DataIn)
        {
            //SqlConnection myConnection = new SqlConnection(ConnectionString);
            try
            {
                myConnection.Open();
                SqlCommand command = new SqlCommand(ProcedureName, myConnection);
                command.CommandType = System.Data.CommandType.StoredProcedure;

                foreach (KeyValuePair<string, string> item in DataIn)
                {
                    SqlParameter nameParam = new SqlParameter
                    {
                        ParameterName = "@" + item.Key,
                        Value = item.Value
                    };
                    command.Parameters.Add(nameParam);
                }
                var result = command.ExecuteScalar();
                //var result = command.ExecuteNonQuery(); 
                myConnection.Close();
                return result.ToString();

                /*SqlParameter nameParam = new SqlParameter
                {
                    ParameterName = "@PatName",
                    Value = PatName
                }; command.Parameters.Add(nameParam);*/
            }
            catch (Exception e) { myConnection.Close();  return "0"; };
        }

        public List<Dictionary<string, string>> SelectQuery(string Query)
        {
            //SqlConnection myConnection = new SqlConnection(ConnectionString);
            try
            {
                myConnection.Open();
                List<Dictionary<string, string>> DataList = new List<Dictionary<string, string>>();
                SqlCommand myCommand = new SqlCommand(Query, myConnection);
                using (SqlDataReader reader = myCommand.ExecuteReader())
                {
                    int Fieldscount = reader.FieldCount;

                    while (reader.Read())
                    {
                        Dictionary<string, string> Columns = new Dictionary<string, string>();
                        for (int i = 0; i < Fieldscount; i++)
                        {
                            Columns.Add(reader.GetName(i), reader[i].ToString());
                        }
                        DataList.Add(Columns);
                    }
                }
                myConnection.Close();
                return DataList;
            }
            finally
            {
                myConnection.Close();
            }
        }

        public List<Dictionary<string, string>> SelectCommandParam(SqlCommand myCommand)
        {
            //SqlConnection myConnection = new SqlConnection(ConnectionString);
            try
            {
                myConnection.Open();
                List<Dictionary<string, string>> DataList = new List<Dictionary<string, string>>();
 
                using (SqlDataReader reader = myCommand.ExecuteReader())
                {
                    int Fieldscount = reader.FieldCount;
                    while (reader.Read())
                    {
                        Dictionary<string, string> Columns = new Dictionary<string, string>();
                        for (int i = 0; i < Fieldscount; i++)
                        {
                            Columns.Add(reader.GetName(i), reader[i].ToString());
                        }
                        DataList.Add(Columns);
                    }
                }
                myConnection.Close();
                return DataList;
            }
            finally
            {
                myConnection.Close();
            }
        }

        public List<List<string>> SelectQuerySimple(string Query)
        {
            //SqlConnection myConnection = new SqlConnection(ConnectionString);
            try
            {
                myConnection.Open();
                List<List<string>> DataList = new List<List<string>>();
                SqlCommand myCommand = new SqlCommand(Query, myConnection);
                using (SqlDataReader reader = myCommand.ExecuteReader())
                {
                    int Fieldscount = reader.FieldCount;

                    while (reader.Read())
                    {
                        List<string> Columns = new List<string>();
                        for (int i = 0; i < Fieldscount; i++)
                        {
                            //reader.GetName(i),
                            Columns.Add( reader[i].ToString());
                        }
                        DataList.Add(Columns);
                    }
                }
                myConnection.Close();
                return DataList;
            }
            finally
            {
                myConnection.Close();
            }
        }
    }
}

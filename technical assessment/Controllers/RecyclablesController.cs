using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web.Mvc;
using technical_assessment.Models;
using Newtonsoft.Json;

namespace technical_assessment.Controllers
{
    public class RecyclablesController : Controller
    {
        private string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;  
        public ActionResult Index()
        {
            List<Recyclables> recyclables = new List<Recyclables>();
            return View(recyclables);
        }
        [HttpPost]
        public string Create(Recyclables recyclables)
        {
            return recyclables.Insert();
        }

        [HttpPost]
        public string Update(Recyclables recyclables)
        {
            return recyclables.Update();
        }

        public string Delete(Recyclables recyclables)
        {
            return recyclables.Delete();
        }
        [HttpPost]
        public string GetDetails(string id)
        {
            List<Recyclables> recyclables = new List<Recyclables>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * from recyclables WHERE id = @id ";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@id", id);
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        var item = new Recyclables
                        {
                            id = reader.GetInt32(0),
                            type_name = reader.GetString(1),
                            rate = Convert.ToDouble(reader.GetDecimal(2)),
                            min_kg = Convert.ToDouble(reader.GetDecimal(3)),
                            max_kg = Convert.ToDouble(reader.GetDecimal(4)),
                        };
                        recyclables.Add(item);
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    ViewBag.ErrorMessage = "Error fetching data: " + ex.Message;
                }
            }
            return JsonConvert.SerializeObject(recyclables);
        }

        [HttpPost]
        public string GetData(string search,int page = 1,int per_page = 10)
        {
            string search_string = search + "%";
            int off_set = (page - 1) * per_page;
            if (search == null)
            {
                search = "";
            }
            List<Recyclables> recyclables = new List<Recyclables>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * from recyclables " +
                    " WHERE type_name LIKE @search_string " +
                    "ORDER BY date_created DESC OFFSET @off_set ROWS FETCH NEXT @per_page ROWS ONLY;";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@search_string", search_string);
                command.Parameters.AddWithValue("@off_set", off_set);
                command.Parameters.AddWithValue("@per_page", per_page);
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        var item = new Recyclables
                        {
                            id = reader.GetInt32(0),    
                            type_name = reader.GetString(1),
                            rate = Convert.ToDouble(reader.GetDecimal(2)),
                            min_kg = Convert.ToDouble(reader.GetDecimal(3)),
                            max_kg = Convert.ToDouble(reader.GetDecimal(4)),
                        };
                        recyclables.Add(item);
                    }
                    reader.Close();  
                }
                catch (Exception ex)
                {
                    ViewBag.ErrorMessage = "Error fetching data: " + ex.Message;
                }
            }
            return JsonConvert.SerializeObject(recyclables);
        }
        public string GetRecyclableData(string search, int page = 1, int per_page = 10)
        {
            string search_string = search + "%";
            int off_set = (page - 1) * per_page;
            if (search == null)
            {
                search = "";
            }
            List<Recyclables> recyclables = new List<Recyclables>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT * from recyclables " +
                    " WHERE type_name LIKE @search_string " +
                    "ORDER BY type_name ASC OFFSET @off_set ROWS FETCH NEXT @per_page ROWS ONLY;";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@search_string", search_string);
                command.Parameters.AddWithValue("@off_set", off_set);
                command.Parameters.AddWithValue("@per_page", per_page);
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        var item = new Recyclables
                        {
                            id = reader.GetInt32(0),
                            type_name = reader.GetString(1),
                            rate = Convert.ToDouble(reader.GetDecimal(2)),
                            min_kg = Convert.ToDouble(reader.GetDecimal(3)),
                            max_kg = Convert.ToDouble(reader.GetDecimal(4)),
                        };
                        recyclables.Add(item);
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    ViewBag.ErrorMessage = "Error fetching data: " + ex.Message;
                }
            }
            return JsonConvert.SerializeObject(recyclables);
        }
    }

}

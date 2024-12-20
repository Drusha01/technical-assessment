﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using technical_assessment.Models;
using Newtonsoft.Json;
using System.Data.SqlClient;

namespace technical_assessment.Controllers
{
    public class RecyclableItemsController : Controller
    {
        private string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        public ActionResult Index()
        {
            return View();
        }

        public string GetData(string search, int page = 1, int per_page = 10)
        {
            string search_string = search + "%";
            int off_set = (page - 1) * per_page;
            if (search == null)
            {
                search = "";
            }
            List<RecyclableItems> recyclabletItems = new List<RecyclableItems>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT ri.id,ri.recyclable_type_id,r.type_name,ri.weight,ri.computed_rate,ri.item_description,ri.date_created,ri.date_updated from recyclable_items as ri " +
                     "JOIN recyclables as r ON r.id = ri.recyclable_type_id " +
                    " WHERE ri.item_description LIKE @search_string " +
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
                        var item = new RecyclableItems
                        {
                            id = reader.GetInt32(reader.GetOrdinal("id")),
                            recyclable_type_id = reader.GetInt32(reader.GetOrdinal("recyclable_type_id")),
                            type_name = reader.GetString(reader.GetOrdinal("type_name")),
                            weight = Convert.ToDouble(reader.GetDecimal(reader.GetOrdinal("weight"))),
                            computed_rate = Convert.ToDouble(reader.GetDecimal(reader.GetOrdinal("computed_rate"))),
                            item_description = reader.GetString(reader.GetOrdinal("item_description")),
                        };
                        recyclabletItems.Add(item);
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    ViewBag.ErrorMessage = "Error fetching data: " + ex.Message;
                }
            }
            Dictionary<string, object> content = new Dictionary<string, object>();
            content["table_data"] = recyclabletItems;
            content["total_rows"] = getTotalRows(search);
            return JsonConvert.SerializeObject(content);
        }

        public int getTotalRows(string search)
        {
            int total_rows = 0;
            string search_string = search + "%";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT count(*) as total_rows from recyclable_items as ri " +
                     "JOIN recyclables as r ON r.id = ri.recyclable_type_id " +
                    " WHERE ri.item_description LIKE @search_string ";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@search_string", search_string);
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        total_rows = reader.GetInt32(reader.GetOrdinal("total_rows"));
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    ViewBag.ErrorMessage = "Error fetching data: " + ex.Message;
                }
            }
            return total_rows;
        }
        public string Create(RecyclableItems recyclableItems)
        {
            return recyclableItems.Insert();
        }
        public string Update(RecyclableItems recyclableItems)
        {
            return recyclableItems.Update();
        }
        public string Delete(RecyclableItems recyclableItems)
        {
            return recyclableItems.Delete();
        }

        public string GetDetails(string id)
        {
            List<RecyclableItems> recyclableitem = new List<RecyclableItems>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "SELECT ri.id,ri.recyclable_type_id,r.rate,r.type_name,r.min_kg,r.max_kg,ri.weight,ri.computed_rate,ri.item_description,ri.date_created,ri.date_updated from recyclable_items as ri " +
                     "JOIN recyclables as r ON r.id = ri.recyclable_type_id WHERE ri.id = @id ";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@id", id);
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        var item = new RecyclableItems
                        {
                            id = reader.GetInt32(reader.GetOrdinal("id")),
                            recyclable_type_id = reader.GetInt32(reader.GetOrdinal("recyclable_type_id")),
                            type_name = reader.GetString(reader.GetOrdinal("type_name")),
                            weight = Convert.ToDouble(reader.GetDecimal( reader.GetOrdinal("weight"))),
                            computed_rate = Convert.ToDouble(reader.GetDecimal(reader.GetOrdinal("computed_rate"))),
                            item_description = reader.GetString(reader.GetOrdinal("item_description")),
                        };
                        recyclableitem.Add(item);
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    ViewBag.ErrorMessage = "Error fetching data: " + ex.Message;
                }
            }
            return JsonConvert.SerializeObject(recyclableitem);
        }
    }
}
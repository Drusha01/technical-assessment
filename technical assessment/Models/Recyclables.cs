using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace technical_assessment.Models
{
    public class Recyclables
    {
        private string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        public int id { get; set; }
        public string type_name { get; set; }
        public double rate { get; set; }
        public double min_kg { get; set; }
        public double max_kg { get; set; }
        public DateTime date_created { get; set; }
        public DateTime date_updated { get; set; }

        public string Insert()
        {
            if (string.IsNullOrEmpty(this.type_name))
            {
                return "Please input type_name";
            }

            bool valid = true;
            string query = "";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                query = "SELECT * from recyclables WHERE type_name=@type_name";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@type_name", this.type_name);
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        valid = false;
                    }
                    
                    reader.Close();  
                }
                catch (Exception ex)
                {
                   Console.WriteLine( "Error fetching data: " + ex.Message);
                }
            }
            if (!valid)
            {
                return "Type exist";
            }
            if (this.rate <= 0 )
            {
                return "Rate must be greater than 0";
            }
            if (this.min_kg < 0)
            {
                return "Min must be equal or greater than 0";
            }
            if (this.max_kg <= 0)
            {
                return "Max Kg must be greater than 0";
            }
            if (this.max_kg < this.min_kg)
            {
                return "Max Kg must be greater than Min Kg";
            }

            query = "INSERT INTO recyclables (type_name,rate,min_kg,max_kg) VALUES(@type_name,@rate,@min_kg,@max_kg);";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@type_name", this.type_name);
                    command.Parameters.AddWithValue("@rate", this.rate);
                    command.Parameters.AddWithValue("@min_kg", this.min_kg);
                    command.Parameters.AddWithValue("@max_kg", this.max_kg);
                    try
                    {
                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            return "1";
                        }
                        else
                        {
                            Console.WriteLine("No rows were inserted.");
                            return "0";
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error inserting data: " + ex.Message);
                    }
                }
            }
            return "0";
        }
        public string Update()
        {
            if (string.IsNullOrEmpty(this.type_name))
            {
                return "Please input type_name";
            }

            bool valid = true;
            string query = "";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                query = "SELECT * from recyclables WHERE type_name=@type_name AND id <> @id";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@type_name", this.type_name);
                command.Parameters.AddWithValue("@id", this.id);
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        valid = false;
                    }

                    reader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error fetching data: " + ex.Message);
                }
            }
            if (!valid)
            {
                return "Type exist";
            }
            if (this.rate <= 0)
            {
                return "Rate must be greater than 0";
            }
            if (this.min_kg <= 0)
            {
                return "Min must be equal or greater than 0";
            }
            if (this.max_kg <= 0)
            {
                return "Max Kg must be greater than 0";
            }
            if (this.max_kg < this.min_kg)
            {
                return "Max Kg must be greater than Min Kg";
            }
            query = "UPDATE recyclables SET " +
                    "type_name = @type_name, rate = @rate, min_kg = @min_kg, max_kg = @max_kg " +
                    "WHERE id = @id;";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", this.id);
                    command.Parameters.AddWithValue("@type_name", this.type_name);
                    command.Parameters.AddWithValue("@rate", this.rate);
                    command.Parameters.AddWithValue("@min_kg", this.min_kg);
                    command.Parameters.AddWithValue("@max_kg", this.max_kg);
                    try
                    {
                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            return "1";
                        }
                        else
                        {
                            Console.WriteLine("No rows were updated.");
                            return "0";
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error updating data: " + ex.Message);
                    }
                }
            }
            return "0";
        }
        public string Delete()
        {
            string query = "DELETE FROM recyclables WHERE id = @id;";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", this.id);
                    try
                    {
                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            return "1";
                        }
                        else
                        {
                            Console.WriteLine("No rows were inserted.");
                            return "0";
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error inserting data: " + ex.Message);
                    }
                }
            }
            return "0";
        }
    }
}
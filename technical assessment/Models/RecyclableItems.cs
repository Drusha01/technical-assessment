using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace technical_assessment.Models
{
    public class RecyclableItems
    {
        private string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        public int id { get; set; }
        public int recyclable_type_id { get; set; }
        public string type_name { get; set; }
        public double weight { get; set; }
        public double computed_rate { get; set; }
        public string item_description { get; set; }
        public DateTime date_created { get; set; }
        public DateTime date_updated { get; set; }


        public string Insert()
        {
            if (weight <=0)
            {
                return "weight must be greater than 0";
            }
            if (string.IsNullOrEmpty(this.item_description))
            {
                return "Please input item description";
            }

            bool valid = true;
            double rate = 0;
            double min_kg = 0;
            double max_kg = 0;
            string query = "";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                query = "SELECT * from recyclables WHERE id = @id";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@id", this.recyclable_type_id);
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (!reader.HasRows)
                    {
                        valid = false;
                    }
                    else
                    {
                        while (reader.Read())
                        {
                            rate = Convert.ToDouble(reader.GetDecimal(2));
                            min_kg = Convert.ToDouble(reader.GetDecimal(3));
                            max_kg = Convert.ToDouble(reader.GetDecimal(4));
                        }
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
                return "Please select recyclable type";
            }
            this.computed_rate = weight * rate;
            if (this.computed_rate < min_kg || this.computed_rate > max_kg)
            {
                if (this.computed_rate < min_kg)
                {
                    return "Please increase up the weight at least "+ min_kg;
                }
                if (this.computed_rate > max_kg)
                {
                    return "Please lower down the weight until " + max_kg ;
                }
            }

            query = "INSERT INTO recyclable_items (recyclable_type_id,weight,computed_rate,item_description) VALUES(@recyclable_type_id, @weight, @computed_rate,@item_description);";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@recyclable_type_id", this.recyclable_type_id);
                    command.Parameters.AddWithValue("@computed_rate", this.computed_rate);
                    command.Parameters.AddWithValue("@weight", this.weight);
                    command.Parameters.AddWithValue("@item_description", this.item_description);
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
            if (weight <= 0)
            {
                return "weight must be greater than 0";
            }
            if (string.IsNullOrEmpty(this.item_description))
            {
                return "Please input item description";
            }

            bool valid = true;
            double rate = 0;
            double min_kg = 0;
            double max_kg = 0;
            string query = "";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                query = "SELECT * from recyclables WHERE id = @id";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@id", this.recyclable_type_id);
                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (!reader.HasRows)
                    {
                        valid = false;
                    }
                    else
                    {
                        while (reader.Read())
                        {
                            rate = Convert.ToDouble(reader.GetDecimal(2));
                            min_kg = Convert.ToDouble(reader.GetDecimal(3));
                            max_kg = Convert.ToDouble(reader.GetDecimal(4));
                        }
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
                return "Please select recyclable type";
            }
            this.computed_rate = weight * rate;
            if (this.computed_rate < min_kg || this.computed_rate > max_kg)
            {
                if (this.computed_rate < min_kg)
                {
                    return "Please increase up the weight at least " + min_kg;
                }
                if (this.computed_rate > max_kg)
                {
                    return "Please lower down the weight until " + max_kg;
                }
            }

            query = "UPDATE recyclable_items SET recyclable_type_id = @recyclable_type_id, weight = @weight, computed_rate = @computed_rate, item_description = @item_description WHERE id = @id;";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@recyclable_type_id", this.recyclable_type_id);
                    command.Parameters.AddWithValue("@computed_rate", this.computed_rate);
                    command.Parameters.AddWithValue("@weight", this.weight);
                    command.Parameters.AddWithValue("@item_description", this.item_description);
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

        public string Delete()
        {
            string query = "DELETE FROM recyclable_items WHERE id = @id;";
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
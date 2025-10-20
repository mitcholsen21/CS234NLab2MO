using MMABooksBusinessClasses;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace MMABooksDBClasses
{
    public static class ProductDB
    {
        public static Product GetProduct(string productCode)
        {
            using var conn = MMABooksDB.GetConnection();
            string sql = "SELECT ProductCode, Description, OnHandQuantity, UnitPrice FROM Product WHERE ProductCode = @ProductCode";
            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@ProductCode", productCode);
            conn.Open();
            using var rdr = cmd.ExecuteReader(CommandBehavior.SingleRow);
            if (rdr.Read())
            {
                return new Product
                {
                    ProductCode = rdr["ProductCode"].ToString(),
                    Description = rdr["Description"].ToString(),
                    OnHandQuantity = Convert.ToInt32(rdr["OnHandQuantity"]),
                    UnitPrice = Convert.ToDecimal(rdr["UnitPrice"])
                };
            }
            return null;
        }

        public static List<Product> GetProducts()
        {
            var results = new List<Product>();
            using var conn = MMABooksDB.GetConnection();
            string sql = "SELECT ProductCode, Description, OnHandQuantity, UnitPrice FROM Product";
            using var cmd = new MySqlCommand(sql, conn);
            conn.Open();
            using var rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                results.Add(new Product
                {
                    ProductCode = rdr["ProductCode"].ToString(),
                    Description = rdr["Description"].ToString(),
                    OnHandQuantity = Convert.ToInt32(rdr["OnHandQuantity"]),
                    UnitPrice = Convert.ToDecimal(rdr["UnitPrice"])
                });
            }
            return results;
        }

        public static bool AddProduct(Product product)
        {
            using var conn = MMABooksDB.GetConnection();
            string sql = @"INSERT INTO Product (ProductCode, Description, OnHandQuantity, UnitPrice)
                           VALUES (@ProductCode, @Description, @OnHandQuantity, @UnitPrice)";
            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@ProductCode", product.ProductCode);
            cmd.Parameters.AddWithValue("@Description", product.Description);
            cmd.Parameters.AddWithValue("@OnHandQuantity", product.OnHandQuantity);
            cmd.Parameters.AddWithValue("@UnitPrice", product.UnitPrice);
            conn.Open();
            int rows = cmd.ExecuteNonQuery();
            return rows == 1;
        }

        public static bool DeleteProduct(Product product)
        {
            using var conn = MMABooksDB.GetConnection();
            string sql = @"DELETE FROM Product
                           WHERE ProductCode = @ProductCode
                             AND Description = @Description
                             AND OnHandQuantity = @OnHandQuantity
                             AND UnitPrice = @UnitPrice";
            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@ProductCode", product.ProductCode);
            cmd.Parameters.AddWithValue("@Description", product.Description);
            cmd.Parameters.AddWithValue("@OnHandQuantity", product.OnHandQuantity);
            cmd.Parameters.AddWithValue("@UnitPrice", product.UnitPrice);
            conn.Open();
            int rows = cmd.ExecuteNonQuery();
            return rows == 1;
        }

        public static bool UpdateProduct(Product oldProduct, Product newProduct)
        {
            using var conn = MMABooksDB.GetConnection();
            string sql = @"UPDATE Product SET
                            Description = @NewDescription,
                            OnHandQuantity = @NewOnHandQuantity,
                            UnitPrice = @NewUnitPrice
                           WHERE ProductCode = @ProductCode
                             AND Description = @OldDescription
                             AND OnHandQuantity = @OldOnHandQuantity
                             AND UnitPrice = @OldUnitPrice";
            using var cmd = new MySqlCommand(sql, conn);

            // New values
            cmd.Parameters.AddWithValue("@NewDescription", newProduct.Description);
            cmd.Parameters.AddWithValue("@NewOnHandQuantity", newProduct.OnHandQuantity);
            cmd.Parameters.AddWithValue("@NewUnitPrice", newProduct.UnitPrice);

            // Concurrency / key
            cmd.Parameters.AddWithValue("@ProductCode", oldProduct.ProductCode);
            cmd.Parameters.AddWithValue("@OldDescription", oldProduct.Description);
            cmd.Parameters.AddWithValue("@OldOnHandQuantity", oldProduct.OnHandQuantity);
            cmd.Parameters.AddWithValue("@OldUnitPrice", oldProduct.UnitPrice);

            conn.Open();
            int rows = cmd.ExecuteNonQuery();
            return rows == 1;
        }
    }
}
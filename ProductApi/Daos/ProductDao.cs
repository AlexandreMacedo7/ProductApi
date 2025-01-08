using MySql.Data.MySqlClient;
using ProductApi.Dtos;
using ProductApi.Models;
using System.Text;

namespace ProductApi.Daos
{
    public class ProductDao
    {
        private readonly string _connectionString;

        public ProductDao(string connectionString)
        {
            _connectionString = connectionString;
        }

        public int Create(ProductRequestDto product)
        {
            using(var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                using (var command = new MySqlCommand("INSERT INTO Products (Name, Price, Stock)" +
                    " VALUES (@Name, @Price, @Stock); SELECT LAST_INSERT_ID();", connection))
                {
                    command.Parameters.AddWithValue("@Name", product.Name);
                    command.Parameters.AddWithValue("@Price", product.Price);
                    command.Parameters.AddWithValue("@Stock", product.Stock);

                    try
                    {
                        int newId = Convert.ToInt32(command.ExecuteScalar());
                        return newId;
                    }
                    catch (MySqlException ex)
                    {
                        Console.WriteLine($"Erro de SQL: {ex.Message}");
                        throw;
                    }
                }
            }
        }

        public int UpdateProduct(int id, ProductRequestDto product)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new MySqlCommand("UPDATE Products SET Name = @Name, Price = @Price, Stock = @Stock WHERE Id = @Id", connection))
                {
                    // Adiciona os parâmetros de forma segura
                    command.Parameters.AddWithValue("@Name", product.Name);
                    command.Parameters.AddWithValue("@Price", product.Price);
                    command.Parameters.AddWithValue("@Stock", product.Stock);
                    command.Parameters.AddWithValue("@Id", id);

                    // Executa o comando
                    command.ExecuteNonQuery();
                    return id;
                }
            }
        }

        public void UpdateFields(int id, ProductUpdateRequestDto product)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                // Construir a consulta SQL dinamicamente
                var sql = new StringBuilder("UPDATE Products SET ");
                var parameters = new List<MySqlParameter>();

                if (!string.IsNullOrEmpty(product.Name))
                {
                    sql.Append("Name = @Name, ");
                    parameters.Add(new MySqlParameter("@Name", product.Name));
                }
                if (product.Price != default(decimal))
                {
                    sql.Append("Price = @Price, ");
                    parameters.Add(new MySqlParameter("@Price", product.Price));
                }
                if (product.Stock != default(int))
                {
                    sql.Append("Stock = @Stock, ");
                    parameters.Add(new MySqlParameter("@Stock", product.Stock));
                }

                // Remover a última vírgula e espaço
                if (parameters.Count > 0)
                {
                    sql.Length -= 2;
                    sql.Append(" WHERE Id = @Id");
                    parameters.Add(new MySqlParameter("@Id", id));

                    using (var command = new MySqlCommand(sql.ToString(), connection))
                    {
                        command.Parameters.AddRange(parameters.ToArray());
                        command.ExecuteNonQuery();
                    }
                }
            }
        }

        public List<Product> GetAll()
        {
            var products = new List<Product>();

            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new MySqlCommand("SELECT * FROM Products", connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            products.Add(new Product
                            {
                                Id = reader.GetInt32("Id"),
                                Name = reader.GetString("Name"),
                                Price = reader.GetDecimal("Price"),
                                Stock = reader.GetInt32("Stock")
                            });
                        }
                    }
                }
            }
            return products;
        }

        public Product GetById(int id)
        {
            Product product = null;

            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new MySqlCommand("SELECT * FROM Products WHERE Id = @Id", connection))
                {
                    command.Parameters.AddWithValue("@Id", id);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            product = new Product
                            {
                                Id = reader.GetInt32("Id"),
                                Name = reader.GetString("Name"),
                                Price = reader.GetDecimal("Price"),
                                Stock = reader.GetInt32("Stock")

                            };
                        }
                    }
                }
            }
            return product;
        }
        public void Delete(int id)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                using (var command = new MySqlCommand("DELETE FROM Products WHERE Id = @Id", connection))
                {
                    // Adiciona o parâmetro de forma segura
                    command.Parameters.AddWithValue("@Id", id);

                    // Executa o comando
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
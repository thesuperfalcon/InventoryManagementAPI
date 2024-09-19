using InventoryManagementAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using Microsoft.Data.SqlClient;
using Microsoft.Identity.Client.Extensions.Msal;

namespace InventoryManagementAPI.DAL
{
    public class StatisticData
    {
        private static string connectionString = "Server=.\\SQLEXPRESS; Database=InventoryManagement; Trusted_Connection=True; Encrypt=false";

        public static async Task<List<Models.Statistic>> GetStatisticsAsync(int? id)
        {
            var statistics = new List<Models.Statistic>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                string sqlQuery = @"
                SELECT
                    o.Id,
                    o.OrderTime,
                    o.ProductQuantity,
                    p.Name AS ProductName,
                    p.Price,
                    p.CurrentStock,
                    sInitial.Name AS InitialStorageName,
                    sInitial.MaxCapacity AS InitialMaxCapacity,
                    sDestination.Name AS DestinationStorageName,
                    sDestination.MaxCapacity AS DestinationMaxCapacity,
                    uReporter.FirstName + ' ' + uReporter.LastName AS ReporterName,
                    uReporter.EmployeeNumber AS ReporterEmployeeNumber
                FROM
                    [Statistics] o
                        LEFT JOIN
                    Products p ON o.ProductId = p.Id
                LEFT JOIN
                    Storages sInitial ON o.InitialStorageId = sInitial.Id
                LEFT JOIN
                    Storages sDestination ON o.DestinationStorageId = sDestination.Id
                LEFT JOIN
                    [AspNetUsers] uReporter ON o.UserId = uReporter.Id";

                if (id.HasValue && id.Value > 0)
                {
                    sqlQuery += " WHERE o.Id = @id;";
                }

                using (SqlCommand data = new SqlCommand(sqlQuery, connection))
                {
                    if (id.HasValue)
                    {
                        data.Parameters.AddWithValue("@id", id.Value);
                    }

                    using (SqlDataReader reader = await data.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            Models.Statistic statistic = new Models.Statistic
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                OrderTime = reader.IsDBNull(reader.GetOrdinal("OrderTime")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("OrderTime")),
                                ProductQuantity = reader.GetInt32(reader.GetOrdinal("ProductQuantity")),
                                Product = new Models.Product
                                {
                                    Name = reader.IsDBNull(reader.GetOrdinal("ProductName")) ? null : reader.GetString(reader.GetOrdinal("ProductName")),
                                    Price = reader.IsDBNull(reader.GetOrdinal("Price")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("Price")),
                                    CurrentStock = reader.IsDBNull(reader.GetOrdinal("CurrentStock")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("CurrentStock"))
                                },
                                InitialStorage = new Models.Storage
                                {
                                    Name = reader.IsDBNull(reader.GetOrdinal("InitialStorageName")) ? null : reader.GetString(reader.GetOrdinal("InitialStorageName")),
                                    MaxCapacity = reader.IsDBNull(reader.GetOrdinal("InitialMaxCapacity")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("InitialMaxCapacity"))
                                },
                                DestinationStorage = new Models.Storage
                                {
                                    Name = reader.IsDBNull(reader.GetOrdinal("DestinationStorageName")) ? null : reader.GetString(reader.GetOrdinal("DestinationStorageName")),
                                    MaxCapacity = reader.IsDBNull(reader.GetOrdinal("DestinationMaxCapacity")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("DestinationMaxCapacity"))
                                },
                                User = new User
                                {
                                    FirstName = reader.IsDBNull(reader.GetOrdinal("ReporterName")) ? null : reader.GetString(reader.GetOrdinal("ReporterName")),
                                    EmployeeNumber = reader.IsDBNull(reader.GetOrdinal("ReporterEmployeeNumber")) ? null : reader.GetString(reader.GetOrdinal("ReporterEmployeeNumber"))
                                }
                            };

                            statistics.Add(statistic);
                        }
                    }
                }
            }
            return statistics;
        }
    }
}


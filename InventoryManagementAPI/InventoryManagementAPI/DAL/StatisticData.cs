using InventoryManagementAPI.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using Microsoft.Data.SqlClient;
using Microsoft.Identity.Client.Extensions.Msal;

namespace InventoryManagementAPI.DAL
{
    public class StatisticData
    {
        private static string connectionString = "Server=.\\SQLEXPRESS; Database=InventoryManagement; Trusted_Connection=True; Encrypt=false";

        public static async Task<List<StatisticResponseDto>> GetStatisticsAsync()
        {
            var statistics = new List<StatisticResponseDto>();

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

                using (SqlCommand data = new SqlCommand(sqlQuery, connection))
                {
                    using (SqlDataReader reader = await data.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            var statistic = new StatisticResponseDto
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                OrderTime = reader.IsDBNull(reader.GetOrdinal("OrderTime")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("OrderTime")),
                                ProductQuantity = reader.GetInt32(reader.GetOrdinal("ProductQuantity")),
                                ProductName = reader.IsDBNull(reader.GetOrdinal("ProductName")) ? null : reader.GetString(reader.GetOrdinal("ProductName")),
                                Price = reader.IsDBNull(reader.GetOrdinal("Price")) ? (decimal?)null : reader.GetDecimal(reader.GetOrdinal("Price")),
                                CurrentStock = reader.IsDBNull(reader.GetOrdinal("CurrentStock")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("CurrentStock")),
                                InitialStorageName = reader.IsDBNull(reader.GetOrdinal("InitialStorageName")) ? null : reader.GetString(reader.GetOrdinal("InitialStorageName")),
                                InitialMaxCapacity = reader.IsDBNull(reader.GetOrdinal("InitialMaxCapacity")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("InitialMaxCapacity")),
                                DestinationStorageName = reader.IsDBNull(reader.GetOrdinal("DestinationStorageName")) ? null : reader.GetString(reader.GetOrdinal("DestinationStorageName")),
                                DestinationMaxCapacity = reader.IsDBNull(reader.GetOrdinal("DestinationMaxCapacity")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("DestinationMaxCapacity")),
                                ReporterName = reader.IsDBNull(reader.GetOrdinal("ReporterName")) ? null : reader.GetString(reader.GetOrdinal("ReporterName")),
                                ReporterEmployeeNumber = reader.IsDBNull(reader.GetOrdinal("ReporterEmployeeNumber")) ? null : reader.GetString(reader.GetOrdinal("ReporterEmployeeNumber"))
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


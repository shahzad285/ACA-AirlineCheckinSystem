using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace ACA_AirlineCheckinSystem
{
    public class CheckinSystemWithoutLockingApproach
    {
        private readonly string _connectionString;

        public CheckinSystemWithoutLockingApproach(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void CheckinSeats()
        {
           Stopwatch sw = Stopwatch.StartNew();
            sw.Start();
            using (var dbContext = new DatabaseContext(_connectionString))
            {
                dbContext.OpenConnection();
                string query = "UPDATE FlightUsers SET UserId = NULL";
                dbContext.ExecuteNonQuery(query);
            }

            List<Task> tasks = new List<Task>();

            for (int i = 1; i <= 120; i++)
            {
                int userId = i;
                tasks.Add(Task.Run(() => AssignSeat(userId)));
            }

            Task.WaitAll(tasks.ToArray());
            Console.WriteLine("Time taken in milliseconds in without lock execution " + sw.ElapsedMilliseconds);
            PrintSeatingArrangement();
        }

        public void AssignSeat(int userId)
        {
            using (var dbContext = new DatabaseContext(_connectionString))
            {
                try
                {
                    dbContext.OpenConnection();
                    dbContext.BeginTransaction();

                    // Find the next available seat
                    string selectQuery = "SELECT TOP 1 Seat FROM FlightUsers WHERE UserId IS NULL";

                    DataTable result = dbContext.ExecuteQuery(selectQuery);
                    if (result.Rows.Count > 0)
                    {
                        string seat = result.Rows[0]["Seat"].ToString();

                        // Assign the seat to the user
                        string updateQuery = $"UPDATE FlightUsers SET UserId = {userId} WHERE Seat = '{seat}'";
                        dbContext.ExecuteNonQuery(updateQuery);

                        // Retrieve and print the user's name and the assigned seat
                        string getUserQuery = "SELECT Name FROM Users WHERE Id = @UserId";
                        SqlParameter[] userParams = {
                        new SqlParameter("@UserId", userId)
                    };

                        DataTable userResult = dbContext.ExecuteQuery(getUserQuery, userParams);
                        if (userResult.Rows.Count > 0)
                        {
                            string userName = userResult.Rows[0]["Name"].ToString();
                            Console.WriteLine($"User '{userName}' has been assigned seat '{seat}'.");
                        }
                    }

                    dbContext.CommitTransaction();
                }
                catch (Exception ex)
                {
                    dbContext.RollbackTransaction();
                    Console.WriteLine(ex.Message);
                }
            }
        }

       
        public void PrintSeatingArrangement()
        {
            using (var dbContext = new DatabaseContext(_connectionString))
            {
                dbContext.OpenConnection();
                string query = "SELECT Seat, UserId FROM FlightUsers ORDER BY Seat";
                DataTable seats = dbContext.ExecuteQuery(query);

                int seatCount = 0;
                StringBuilder row = new StringBuilder();

                foreach (DataRow seat in seats.Rows)
                {
                    if (seat["UserId"] != DBNull.Value)
                    {
                        row.Append(" * ");
                    }
                    else
                    {
                        row.Append(" . ");
                    }

                    seatCount++;

                    if (seatCount % 6 == 0)
                    {
                        Console.WriteLine(row.ToString().TrimEnd());
                        row.Clear();
                    }
                }

                // Print any remaining seats that didn't fill a complete row
                if (row.Length > 0)
                {
                    Console.WriteLine(row.ToString().TrimEnd());
                }
            }
        }
    }
    }

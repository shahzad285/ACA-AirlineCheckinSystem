using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACA_AirlineCheckinSystem
{
    public class DatabaseContext : IDisposable
    {
        private readonly string _connectionString;
        private SqlConnection _connection;
        private SqlTransaction _transaction;

        public DatabaseContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void OpenConnection()
        {
            if (_connection == null)
            {
                _connection = new SqlConnection(_connectionString);
            }

            if (_connection.State != ConnectionState.Open)
            {
                _connection.Open();
            }
        }

        public void BeginTransaction()
        {
            if (_connection == null)
            {
                throw new InvalidOperationException("Connection must be opened before starting a transaction.");
            }

            if (_connection.State != ConnectionState.Open)
            {
                _connection.Open();
            }

            _transaction = _connection.BeginTransaction();
        }

        public void CommitTransaction()
        {
            if (_transaction != null)
            {
                _transaction.Commit();
                _transaction.Dispose();
                _transaction = null;
            }
        }

        public void RollbackTransaction()
        {
            if (_transaction != null)
            {
                _transaction.Rollback();
                _transaction.Dispose();
                _transaction = null;
            }
        }

        public DataTable ExecuteQuery(string query, SqlParameter[] parameters = null)
        {
            if (_connection == null)
            {
                throw new InvalidOperationException("Connection must be opened before executing a query.");
            }

            using (SqlCommand cmd = new SqlCommand(query, _connection, _transaction))
            {
                if (parameters != null)
                {
                    cmd.Parameters.AddRange(parameters);
                }

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return dt;
            }
        }

        public int ExecuteNonQuery(string query, SqlParameter[] parameters = null)
        {
            if (_connection == null)
            {
                throw new InvalidOperationException("Connection must be opened before executing a non-query.");
            }

            using (SqlCommand cmd = new SqlCommand(query, _connection, _transaction))
            {
                if (parameters != null)
                {
                    cmd.Parameters.AddRange(parameters);
                }

                return cmd.ExecuteNonQuery();
            }
        }

        public void Dispose()
        {
            if (_transaction != null)
            {
                _transaction.Dispose();
                _transaction = null;
            }

            if (_connection != null)
            {
                _connection.Close();
                _connection.Dispose();
                _connection = null;
            }
        }
    }
}
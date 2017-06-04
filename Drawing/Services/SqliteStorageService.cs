using Drawing.Models;
using log4net;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SQLite;
using System.IO;
using System.Linq;

namespace Drawing.Services
{
    class SqliteStorageService : IStorageService
    {
        private ILog _logger = LogManager.GetLogger(typeof(SqliteStorageService));

        private readonly string ConnectionString;

        public SQLiteConnection SQLiteConnection
        {
            get { return new SQLiteConnection(ConnectionString); }
        }

        public SqliteStorageService(string connectionString= "Data Source=Drawing.db;Version=3;")
        {
            ConnectionString = connectionString;

            if (!File.Exists("Drawing.db"))
            {
                CreateDb();
            }
        }

        private void CreateDb()
        {
            SQLiteConnection.CreateFile("Drawing.db");

            var sql = @"CREATE TABLE [Diagram] ( [Id] TEXT NOT NULL
                                               , [Name] TEXT NOT NULL unique
                                               , [JsonValue] TEXT NOT NULL
                                               , CONSTRAINT[PK_Diagram] PRIMARY KEY([Id])
                                               );";
            ExecuteNonQuery(sql);
        }

        public void Add(Diagram diagram)
        {
            var sql = $"INSERT INTO [Diagram] VALUES ('{diagram.Id.ToUpperString()}','{diagram.Name}','{diagram.JsonValue}')";
            ExecuteNonQuery(sql);
        }

        public void Delete(Guid id)
        {
            var sql = $"DELETE FROM [Diagram] WHERE [Id] = '{id.ToUpperString()}'";
            ExecuteNonQuery(sql);
        }

        public List<Diagram> GetAll()
        {
            var sql = $"SELECT [Id],[Name],[JsonValue] FROM [Diagram] ORDER BY [Name]";
            return ExecuteTableQuery(sql);
        }

        public Diagram GetById(Guid id)
        {
            var sql = $"SELECT [Id],[Name],[JsonValue] FROM [Diagram] WHERE [Id] = '{id.ToUpperString()}'";
            return ExecuteTableQuery(sql).FirstOrDefault();
        }

        public Diagram GetByName(string name)
        {
            var sql = $"SELECT [Id],[Name],[JsonValue] FROM [Diagram] WHERE [Name] = '{name}'";
            return ExecuteTableQuery(sql).FirstOrDefault();
        }

        public void Update(Diagram diagram)
        {
            var sql = $"UPDATE [Diagram] SET [JsonValue] = '{diagram.JsonValue}' WHERE [Id] = '{diagram.Id.ToUpperString()}'";
            ExecuteNonQuery(sql);
        }

        private List<Diagram> ExecuteTableQuery(string sql)
        {
            List<Diagram> diagrams = new List<Diagram>();

            DbConnection conn = SQLiteConnection;
            conn.Open();
            try
            {
                DbCommand command = conn.CreateCommand();
                command.CommandText = sql;
                DbDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Diagram diagram = new Diagram()
                    {
                        Id = Guid.Parse(reader.GetString(0)),
                        Name = reader.GetString(1)
                    };
                    diagram.Pase(reader.GetString(2));
                    diagrams.Add(diagram);
                }

            }
            catch (Exception e)
            {
                _logger.Error(e);
            }
            finally
            {
                conn.Close();
            }
            return diagrams;
        }

        private void ExecuteNonQuery(string sql)
        {
            DbConnection conn = SQLiteConnection;
            conn.Open();
            try
            {
                DbCommand command = conn.CreateCommand();
                command.CommandText = sql;
                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                _logger.Error(e);
            }
            finally
            {
                conn.Close();
            }
        }
    }
}

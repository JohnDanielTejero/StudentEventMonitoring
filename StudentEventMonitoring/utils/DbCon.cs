using System;
using System.Collections.Generic;
using System.Linq;
using MySql.Data.MySqlClient;

namespace StudentEventMonitoring.utils
{
    class DbCon
    {
        private DbCon() { }

        public string Server { get; set; } = "localhost";
        public string DatabaseName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public MySqlConnection Connection { get; set; }

        private static DbCon _instance = null;
        private static readonly object _lock = new object(); // Ensure thread-safety

        /**
        * Checks if there is an existing instance, create one if not created yet.
        * 
        * !IMPORTANT
        * Only use this if you already created a db instance via the builder DbConf.cs
        * This is to ensure every configuration is properly set. Otherwise the database will not connect properly.
        *
        * @return A DbCon instance to adhere to singleton pattern
        * 
        * Example usage:
        * <code>
        * DbCon con = con.Instance();
        * </code>
        *
        */
        public static DbCon Instance()
        {
            lock (_lock)
            {
                if (_instance == null)
                    _instance = new DbCon();
                return _instance;
            }
        }

        /**
         * Closes the database connection if there's any open connection
         * @return A boolean and opens up a mysql connection
         * 
         * Example usage:
         * <code>
         * bool connection = con.IsConnect();
         * </code>
         *
         */
        public bool IsConnect()
        {
            if (Connection == null || Connection.State == System.Data.ConnectionState.Closed)
            {
                if (String.IsNullOrEmpty(DatabaseName))
                    return false;

                string connstring = string.Format("Server={0}; database={1}; UID={2}; password={3}", Server, DatabaseName, UserName, Password);
                Connection = new MySqlConnection(connstring);

                try
                {
                    Connection.Open();
                }
                catch (MySqlException ex)
                {
                    Console.WriteLine($"Connection error: {ex.Message}");
                    return false;
                }
            }

            return true;
        }

        /**
         * Closes the database connection if there's any open connection
         * 
         * Example usage:
         * <code>
         * con.Close();
         * </code>
         */
        public void Close()
        {
            if (Connection != null && Connection.State == System.Data.ConnectionState.Open)
            {
                Connection.Close();
            }
        }

         /**
          * Selects data from a specified table based on provided parameters.
          *
          * @param table The name of the table to select data from.
          * @param parameters A dictionary where the key is the column name and the value is the condition for that column.
          * @return A MySqlDataReader containing the results of the query.
          * 
          * Example usage:
          * <code>
          * var parameters = new Dictionary<string, string> { { "id", "1" }, { "name", "John" } };
          * var reader = ReadData("users", parameters);
          * </code>
          */
        public MySqlDataReader ReadData(string table, Dictionary<string, string> parameters)
        {
            this.IsConnect();

            string selectQuery = $"SELECT * FROM {table} WHERE 1=1";
            MySqlCommand command = new MySqlCommand(selectQuery, Connection);

            foreach (var kvp in parameters)
            {
                selectQuery += $" AND {kvp.Key} = @{kvp.Key}";
                command.Parameters.AddWithValue($"@{kvp.Key}", kvp.Value);
            }
            command.CommandText = selectQuery;
            return command.ExecuteReader();
        }

        /**
         * Inserts data into a specified table.
         *
         * @param table The name of the table to insert data into.
         * @param parameters A dictionary where the key is the column name and the value is the data to be inserted into that column.
         * @return True if the data was successfully inserted, otherwise false.
         * 
         * Example usage:
         * <code>
         * var parameters = new Dictionary<string, string> { { "name", "John" }, { "age", "30" } };
         * bool success = InsertData("users", parameters);
         * </code>
         */
        public bool InsertData(string table, Dictionary<string, string> parameters)
        {
            this.IsConnect();

            string columns = string.Join(", ", parameters.Keys);
            string values = string.Join(", ", parameters.Keys.Select(key => $"@{key}"));

            string insertQuery = $"INSERT INTO {table} ({columns}) VALUES ({values})";

            using (MySqlCommand command = new MySqlCommand(insertQuery, Connection))
            {
                foreach (var kvp in parameters)
                {
                    command.Parameters.AddWithValue($"@{kvp.Key}", kvp.Value);
                }

                int rowsAffected = command.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }

        /**
         * Deletes data from a specified table based on provided conditions.
         *
         * @param table The name of the table to delete data from.
         * @param conditions A dictionary where the key is the column name and the value is the condition for the deletion.
         * @return True if the data was successfully deleted, otherwise false.
         * 
         * Example usage:
         * <code>
         * var conditions = new Dictionary<string, string> { { "id", "1" } };
         * bool success = DeleteData("users", conditions);
         * </code>
         */
        public bool DeleteData(string table, Dictionary<string, string> conditions)
        {
            this.IsConnect();

            string deleteQuery = $"DELETE FROM {table} WHERE 1=1";
            MySqlCommand command = new MySqlCommand(deleteQuery, Connection);

            foreach (var kvp in conditions)
            {
                deleteQuery += $" AND {kvp.Key} = @{kvp.Key}";
                command.Parameters.AddWithValue($"@{kvp.Key}", kvp.Value);
            }
            command.CommandText = deleteQuery;

            int rowsAffected = command.ExecuteNonQuery();
            return rowsAffected > 0;
        }

        /**
         * Updates data in a specified table based on provided parameters and conditions.
         *
         * @param table The name of the table to update data in.
         * @param parameters A dictionary where the key is the column name and the value is the new data for that column.
         * @param conditions A dictionary where the key is the column name and the value is the condition for updating the data.
         * @return True if the data was successfully updated, otherwise false.
         * 
         * Example usage:
         * <code>
         * var parameters = new Dictionary<string, string> { { "name", "John" } };
         * var conditions = new Dictionary<string, string> { { "id", "1" } };
         * bool success = UpdateData("users", parameters, conditions);
         * </code>
         */
        public bool UpdateData(string table, Dictionary<string, string> parameters, Dictionary<string, string> conditions)
        {
            this.IsConnect();

            string updateQuery = $"UPDATE {table} SET ";
            updateQuery += string.Join(", ", parameters.Select(kvp => $"{kvp.Key} = @{kvp.Key}"));

            updateQuery += " WHERE 1=1";
            MySqlCommand command = new MySqlCommand(updateQuery, Connection);

            foreach (var kvp in parameters)
            {
                command.Parameters.AddWithValue($"@{kvp.Key}", kvp.Value);
            }

            foreach (var kvp in conditions)
            {
                updateQuery += $" AND {kvp.Key} = @{kvp.Key}_cond";
                command.Parameters.AddWithValue($"@{kvp.Key}_cond", kvp.Value);
            }

            command.CommandText = updateQuery;

            int rowsAffected = command.ExecuteNonQuery();
            return rowsAffected > 0;
        }

    }
}

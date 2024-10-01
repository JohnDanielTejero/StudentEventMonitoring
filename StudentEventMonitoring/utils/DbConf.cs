using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentEventMonitoring.utils
{
    /**
     * Configuration class for setting up the database connection using the builder pattern.
     * This class ensures that only one instance of the configuration is created (singleton).
     * 
     * It allows chaining methods to set various parameters of the database connection (password, database, user, server),
     * and provides a `Build()` method to finalize and retrieve the database connection.
     * 
     * Example usage:
     * <code>
     * DbCon connection = DbConf.Instance()
     *                         .SetUser("admin")
     *                         .SetPassword("password")
     *                         .SetDatabase("StudentDB")
     *                         .SetServer("localhost")
     *                         .Build();
     * </code>
     */
    class DbConf
    {
        private static DbConf _instance = null;

        private static readonly object _lock = new object();

        private DbCon _connection = DbCon.Instance();

        private DbConf() { }

        /**
         * Returns the single instance of the `DbConf` class (singleton).
         * Ensures that only one instance is created and shared across threads.
         * 
         * @return The instance of `DbConf`.
         */
        public static DbConf Instance()
        {
            lock (_lock)
            {
                if (_instance == null)
                    _instance = new DbConf();
                return _instance;
            }
        }

        /**
         * Sets the password for the database connection.
         *
         * @param password The password to connect to the database.
         * @return The `DbConf` instance for chaining method calls.
         */
        public DbConf SetPassword(string password)
        {
            _connection.Password = password;
            return this;
        }

        /**
         * Sets the name of the database to connect to.
         *
         * @param database The name of the database.
         * @return The `DbConf` instance for chaining method calls.
         */
        public DbConf SetDatabase(string database)
        {
            _connection.DatabaseName = database;
            return this;
        }

        /**
         * Sets the user name for the database connection.
         *
         * @param user The username for the database connection.
         * @return The `DbConf` instance for chaining method calls.
         */
        public DbConf SetUser(string user)
        {
            _connection.UserName = user;
            return this;
        }

        /**
         * Sets the server for the database connection.
         *
         * @param server The server address of the database.
         * @return The `DbConf` instance for chaining method calls.
         */
        public DbConf SetServer(string server)
        {
            _connection.Server = server;
            return this;
        }

        /**
         * Finalizes the configuration and returns the `DbCon` instance, representing the database connection.
         *
         * @return The `DbCon` instance with the configured settings.
         */
        public DbCon Build()
        {
            // Connection is built and returned
            return _connection;
        }

        /**
         * Resets the configuration settings for the database connection.
         * Useful if you want to clear the current settings and start fresh.
         */
        public void Reset()
        {
            _connection = DbCon.Instance();
        }
    }
}

# StudentEventMonitoring

## Tools and Technologies
1. Ensure to install Visual Studio 2019 or latest that supports .NET v7 and above
2. Install MySQL 

## Configuration
1. Locate **programs.cs** and open the file.
2. Edit the Class Builder configuration
```cs
namespace StudentEventMonitoring
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Create an instance of db con with proper configurations
            DbCon configuration = DbConf.Instance()
                    .SetServer("localhost") //default server is localhost, this can be excluded
                    .SetUser("root") //sets the username    
                    .SetPassword("password") //sets the password
                    .SetDatabase("wam1_midterm_db") //sets the database
                    .Build(); //returns DbCon Instance with the specified configurations
            Application.Run(new Form1());
        }
    }
}

```
## Use Case
1. Include this code upon form instantiation
```cs
    public partial class Form1 : Form
    {
        private DbCon connection;
        public Form1()
        {
            InitializeComponent();
            connection = DbCon.Instance(); //retrieves the instance of DbCon created earlier
        }

    }
```
2. Example Case with ReadData method
```cs
//always use try/catch block
try
{
    MySqlDataReader records = connection.ReadData(
        "users", // Param for table name
        new Dictionary<string, string>
        {
            // Define the key-value pairs for the condition
            /**
             * { "id", "1" },
             * { "name", "John" }
             */
        }
    );

    // Use a while loop to iterate through all records
    while (records.Read())
    {
        MessageBox.Show($"{records["data"]}");

        // Example of mapping the record to a model
        Users user = new Users(
            records["username"].ToString(),
            records["password"].ToString(),
            Convert.ToInt32(records["uid"].ToString())
        );

    }

    // If no records were found, show a message (optional)
    if (!records.HasRows)
    {
        MessageBox.Show("No records found.");
    }
}
catch (Exception ex)
{
    MessageBox.Show($"{ex}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
}

```

using System;
using System.Data.SqlClient;

namespace SQLRunner
{
    class Program
    {
        static void Main(string[] args)
        {
            var options = new Options();
            if (CommandLine.Parser.Default.ParseArguments(args, options))
            {
                var connectionString = new SqlConnectionStringBuilder
                {
                    ApplicationName = "SQLRunner",
                    DataSource = options.Server,
                    InitialCatalog = options.Database,
                    IntegratedSecurity = true,

                };
                // Get list of static data tables and their file path
                var fileDictionary =
                    FileInteraction.GetStaticDataTableNames(FileInteraction.GetStaticDataFilenames(options.Folder));
                // Build query to only include static data tables
                var depQuery = DependencyBuilder.DependencyQueryBuilder(fileDictionary);


                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString.ToString()))
                    {
                        connection.Open();
                        // Get all dependencies between static data tables
                        var tablesAndDependencies =
                            DependencyBuilder.GetTablesAndDependencies(
                                SQLInteraction.GetTablesWithDependencies(connection, depQuery), fileDictionary);
                        // Sort into dependency order
                        var tablesInDependencyOrder = TopologicalSort.Sort(tablesAndDependencies, x => x.Dependencies);
                        // Run the inserts
                        SQLInteraction.InsertRows(fileDictionary, connection, tablesInDependencyOrder);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error occured with static data");
                    Console.WriteLine(e.Message);
                    Console.WriteLine(e);
                    // Return an exit code of 1 so PowerShell runners see it as a failure
                    Environment.Exit(1);
                }
            }
        }
    }
}

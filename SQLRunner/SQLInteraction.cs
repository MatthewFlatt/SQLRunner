using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Text;

namespace SQLRunner
{
    class SQLInteraction
    {
        /// <summary>
        /// Given a list of static data tables in dependency order, look through them and insert all rows from their respective .sql file
        /// </summary>
        /// <param name="fileDictionary">Dictionary of static data table names and their .sql filepath</param>
        /// <param name="connection">SQL Connection</param>
        /// <param name="tablesInDependencyOrder">Static data table names in execution order</param>
        public static void InsertRows(Dictionary<string, string> fileDictionary, SqlConnection connection,
            IList<Item> tablesInDependencyOrder)
        {
            // Use same transaction for all inserts to improve performance
            using (var t = connection.BeginTransaction())
            {
                var command = new SqlCommand
                    {
                        Connection = connection,
                        CommandTimeout = 6000,
                        Transaction = t
                    };
                var builder = new StringBuilder();
                
                // loop through tables in order
                foreach (var table in tablesInDependencyOrder)
                {
                    var rowsAdded = 0;
                    var fileEnded = false;
                    string filename;
                    fileDictionary.TryGetValue(table.Name, out filename);
                    
                    using (StreamReader reader = File.OpenText(filename))
                    {
                        while (!fileEnded)
                        {
                            // Split into batches of 100 inserts
                            for (int i = 0; i < 100; i++)
                            {
                                var line = reader.ReadLine();
                                if (line != null)
                                {
                                    builder.AppendLine(line);
                                }
                                else
                                {
                                    fileEnded = true;
                                    break;
                                }
                            }
                            command.CommandText = builder.ToString();
                            if (!string.IsNullOrWhiteSpace(command.CommandText))
                            {
                                rowsAdded += command.ExecuteNonQuery();
                            }                            
                            builder.Clear();
                        }
                        Console.WriteLine("Rows added for {0}: {1}", table.Name, rowsAdded);
                    }
                }
                t.Commit();
            }
        }


        public static List<Dependency> GetTablesWithDependencies(SqlConnection connection, string query)
        {
            var tablesWithDependencies = new List<Dependency>();
            var command = new SqlCommand
            {
                Connection = connection,
                CommandTimeout = 6000,
                CommandText = query
            };
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    tablesWithDependencies.Add(new Dependency(reader.GetString(0), reader.GetString(1)));
                }
                reader.Close();
            }
            return tablesWithDependencies;
        }
    }
}
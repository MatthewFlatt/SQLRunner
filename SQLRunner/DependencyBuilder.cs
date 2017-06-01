using System.Collections.Generic;
using System.Linq;

namespace SQLRunner
{
    class DependencyBuilder
    {
        /// <summary>
        /// Get all foreign keys in the database which have a static data table as both source and target
        /// </summary>
        /// <param name="tables"></param>
        /// <returns></returns>
        public static string DependencyQueryBuilder(Dictionary<string, string> tables)
        {
            var inClause = tables.Aggregate("(", (current, table) => current + $"'{table.Key}',");

            inClause = inClause.TrimEnd(',');
            inClause += ")";
            return $"SELECT tab.name AS sourceTable, ref.name AS targetTable FROM sys.foreign_keys INNER JOIN sys.tables tab ON tab.object_id = foreign_keys.parent_object_id INNER JOIN sys.tables ref ON ref.object_id = foreign_keys.referenced_object_id WHERE tab.name IN {inClause} AND ref.name IN {inClause}";
        }

        /// <summary>
        /// Based on all the foreign key dependencies, collate dependencies for each table
        /// </summary>
        /// <param name="dependencies"></param>
        /// <param name="tables"></param>
        /// <returns></returns>
        public static List<Item> GetTablesAndDependencies(List<Dependency> dependencies, Dictionary<string, string> tables)
        {
            var tablesWithDependencies = new Dictionary<string, Item>();

            foreach (var dependency in dependencies)
            {
                // skip any foreign keys with same source/target
                if (dependency.Target.Equals(dependency.Source)) continue;
                Item dep;
                Item src;
                // See if the target of the FK exists, if not create it
                if (!tablesWithDependencies.TryGetValue(dependency.Target, out dep))
                {
                    dep = new Item(dependency.Target, new List<Item>());
                    tablesWithDependencies.Add(dependency.Target, dep);
                }
                // If source of FK exists, add the target as a dependency of it
                if (tablesWithDependencies.TryGetValue(dependency.Source, out src))
                {
                    src.AddDependency(dep);
                }
                // Create source and add target as dependency
                else
                {
                    src = new Item(dependency.Source, new List<Item> { dep });
                    tablesWithDependencies.Add(dependency.Source, src);
                }
            }
            // Add any other static tables we have into the list
            foreach (var table in tables)
            {
                if (!tablesWithDependencies.ContainsKey(table.Key))
                {
                    tablesWithDependencies.Add(table.Key, new Item(table.Key, new List<Item>()));
                }
            }

            return tablesWithDependencies.Values.ToList();
        }
    }
}

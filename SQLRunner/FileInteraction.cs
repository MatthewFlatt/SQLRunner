using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SQLRunner
{
    class FileInteraction
    {
        public static List<string> GetStaticDataFilenames(string p)
        {
            return Directory.GetFiles(p, "*.sql").ToList();
        }

        /// <summary>
        /// Determine table name from filename
        /// </summary>
        /// <param name="fileNames"></param>
        /// <returns></returns>
        public static Dictionary<string, string> GetStaticDataTableNames(List<string> fileNames)
        {
            var tableNames = new Dictionary<string, string>();
            foreach (var fileName in fileNames)
            {
                var name = Path.GetFileNameWithoutExtension(fileName);
                var parts = name.Split('.');
                tableNames.Add(RemoveTableNameEnd(parts[1]), fileName);
            }
            return tableNames;
        }

        public static string RemoveTableNameEnd(string name)
        {
            var suffix = "_Data";
            return name.EndsWith(suffix) ? name.Substring(0, name.Length - suffix.Length) : name;
        }
    }
}


namespace SQLRunner
{
    /// <summary>
    /// A dependency represents a foreign key relationship between two static data tables
    /// </summary>
    class Dependency
    {
        public string Source;
        public string Target;

        public Dependency(string source, string target)
        {
            Source = source;
            Target = target;
        }
    }
}

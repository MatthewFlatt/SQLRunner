
namespace SQLRunner
{
    /// <summary>
    /// A dependency represents a foreign key relationship between two static data tables
    /// </summary>
    class Dependency
    {
        public string Source;
        public string Target;
        public string Name;

        public Dependency(string source, string target, string name)
        {
            Source = source;
            Target = target;
            Name = name;
        }
    }
}

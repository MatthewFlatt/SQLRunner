using System.Collections.Generic;

namespace SQLRunner
{
    /// <summary>
    /// An Item represents a Static data table name and all other tables is depends on via foreign key 
    /// </summary>
    public class Item
    {
        public string Name { get; private set; }
        public List<Item> Dependencies { get; private set; }
        public List<string> ForeignKeysToSelf { get; private set; }

        public Item(string name, List<Item> dependencies)
        {
            Name = name;
            Dependencies = dependencies;
        }

        public bool Equals(Item obj)
        {
            return obj.Name.Equals(Name);
        }

        public void AddDependency(Item depItem)
        {
            Dependencies.Add(depItem);    
        }

        public void AddForeignKeyToSelfName(string name)
        {
            ForeignKeysToSelf.Add(name);
        }
    }
}

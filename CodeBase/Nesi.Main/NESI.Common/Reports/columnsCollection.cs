using System;
using System.Configuration;

namespace NESI.Common
{
    [Serializable]
    public class columnsCollection : ConfigurationElementCollection
    {
        public override ConfigurationElementCollectionType CollectionType =>
            ConfigurationElementCollectionType.AddRemoveClearMap;

        protected override ConfigurationElement CreateNewElement() => 
            new SchemaSection();

        protected override object GetElementKey(ConfigurationElement element) => 
            ((SchemaSection)element).name;

        public SchemaSection this[int index]
        {
            get => (SchemaSection)BaseGet(index);
            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }
                BaseAdd(index, value);
            }
        }

        public new SchemaSection this[string name] => 
            (SchemaSection)BaseGet(name);
        
        public int IndexOf(SchemaSection url) => 
            BaseIndexOf(url);

        public void Add(SchemaSection url) => 
            BaseAdd(url);

        protected override void BaseAdd(ConfigurationElement element) => 
            BaseAdd(element, false);

        public void Remove(SchemaSection url)
        {
            if (BaseIndexOf(url) < 0) return;
            BaseRemove(url.name);
            Console.WriteLine("columnsCollection: {0}", "Removed collection element!");
        }

        public void RemoveAt(int index) => 
            BaseRemoveAt(index);

        public void Remove(string name) => 
            BaseRemove(name);

        public void Clear()
        {
            BaseClear();
            Console.WriteLine("columnsCollection: {0}", "Removed entire collection!");
        }

    }
}

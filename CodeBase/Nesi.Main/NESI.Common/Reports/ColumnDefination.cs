using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NESI.Common
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false)]
    public class ColumnDefination : Attribute
    {
        public SchemaSection Schema { get; set; }


        public ColumnDefination(string configSection, string name)
        {
            NameValueCollection settings = (NameValueCollection)ConfigurationManager.GetSection(configSection);

           
        }

    }
}

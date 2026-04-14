using NESI.Common.Interface;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;

namespace NESI.Common
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class ModelDefination : Attribute
    {
        public string ConfigSection { get; set; }

        public IReport ReportSchema { get; set; }

        public ModelDefination(string configSection)
        {
            this.ConfigSection = configSection;
          
            var configMap = new ExeConfigurationFileMap
            {
                ExeConfigFilename =
                    System.Web.HttpContext.Current.Server.MapPath(
                        @WebConfigurationManager.AppSettings["GridConfigFileName"])
            };

            var config = ConfigurationManager.OpenMappedExeConfiguration(configMap, ConfigurationUserLevel.None);
            var section = config.GetSection(configSection) as ReportSection;
            
            ReportSchema = GetReport(section);
        }

        public IReport GetReport(ReportSection section)
        {
            var report = ObjectHelper.Cast<Report>(section);

            var sc = new List<Schema>();
            foreach (SchemaSection item in section.columns)
            {
                var obj = ObjectHelper.Cast<Schema>(item);
                sc.Add(obj);
            }

            report.columns = sc.OrderBy(p=>p.ordinal).ToArray();
            return report;
        }

    }
}

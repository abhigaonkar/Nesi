using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using JetBrains.Annotations;
using log4net;

namespace core
{
    /// <summary>
    /// This class is used as an assembly marker for
    /// reflection purposes
    /// </summary>
    public class CoreMarkerType
    {
    }

    /// <summary>
    /// Utility class that adds the core classes to the XPO store
    /// </summary>
    public static class XpoUtility
    {
        private static readonly ILog Logger = 
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Store the valid assemblies from the assembly to XPO store
        /// </summary>
        /// <param name="connectionString"></param>
        public static void StoreCoreXpoAssemblies([NotNull] string connectionString)
        {
            if (connectionString == null) throw new ArgumentNullException(nameof(connectionString));
            Logger.Info("Processing XPA assemblies");

            try
            {
                DevExpress.Xpo.Metadata.XPDictionary dict = 
                    new DevExpress.Xpo.Metadata.ReflectionDictionary();

                var coreTypes = typeof(CoreMarkerType)
                                .Assembly
                                .GetTypes()
                                .Where(t => (t.FullName?.StartsWith("ne_xpo.cs") ?? false) &&
                                            !(t.FullName.Contains("+")) &&
                                            !(t.FullName.Contains("ConnectionHelper")));

                foreach (var coreType in coreTypes)
                {
                    Logger.Debug($"Adding XPO type information for {coreType.Name}");
                    dict.CollectClassInfos(coreType);
                }
                IDataStore store = XpoDefault.GetConnectionProvider(connectionString, AutoCreateOption.SchemaAlreadyExists);
                XpoDefault.DataLayer = new ThreadSafeDataLayer(dict, store);
                XpoDefault.Session = null;
            }
            catch (Exception exception)
            {
                Logger.Error("Error processing XPO assemblies", exception);
                throw;
            }
        }


    }
}

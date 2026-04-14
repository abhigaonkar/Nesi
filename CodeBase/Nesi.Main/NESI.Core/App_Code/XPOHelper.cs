using System;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using DevExpress.Xpo.Metadata;
using System.Configuration;

public static class XpoHelper {
   public static Session GetNewSession() {
       return new Session(DataLayer);
   }

   public static UnitOfWork GetNewUnitOfWork() {
       return new UnitOfWork(DataLayer);
   }

   private static readonly object lockObject = new object();

   static volatile IDataLayer fDataLayer;

    static IDataLayer DataLayer {
       get {
           if(fDataLayer == null) {
               lock(lockObject) {
                   if(fDataLayer == null) {
                       fDataLayer = GetDataLayer();
                   }
               }
           }
           return fDataLayer;
       }
   }

   private static IDataLayer GetDataLayer() {
       XpoDefault.Session = null;
       string conn = ConfigurationManager.ConnectionStrings["MySQLXPO"].ConnectionString;
       XPDictionary dict = new ReflectionDictionary();
       IDataStore store = XpoDefault.GetConnectionProvider(conn, AutoCreateOption.SchemaAlreadyExists);
        dict.GetDataStoreSchema(System.Reflection.Assembly.GetExecutingAssembly());
       IDataLayer dl = new ThreadSafeDataLayer(dict, store);
       return dl;
   }
}
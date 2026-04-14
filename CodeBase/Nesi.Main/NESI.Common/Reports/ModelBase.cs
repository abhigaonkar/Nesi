using NESI.Common.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NESI.Common
{
    public abstract class ModelBase<T> : IModelBase
    {
        public IReport GetSchema()
        {
            var objT = Activator.CreateInstance<T>();

            var attr = typeof(T).GetCustomAttributes(typeof(ModelDefination), true).FirstOrDefault() as ModelDefination;

            return attr.ReportSchema;

        }

        public List<string> Metadata { get; set; }

        public int Count { get; set; }

        public List<string> Summary { get; set; }

        //public object this[string propertyName]
        //{
        //    get
        //    {

        //        Type myType = typeof(T);
        //        PropertyInfo myPropInfo = myType.GetProperty(propertyName);
        //        return myPropInfo.GetValue(this, null);
        //    }
        //    set
        //    {
        //        Type myType = typeof(T);
        //        PropertyInfo myPropInfo = myType.GetProperty(propertyName);
        //        myPropInfo.SetValue(this, value, null);

        //    }

        //}
    }
}

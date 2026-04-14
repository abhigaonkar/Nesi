using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NESI.DataProcessor
{
   public interface INesiDataTableParser
    {
        DataTable GetDataTableFromObjects<TDataClass>(List<TDataClass> dataList) where TDataClass : class;
        List<T> ConvertToList<T>(DataTable dt);
    }
}

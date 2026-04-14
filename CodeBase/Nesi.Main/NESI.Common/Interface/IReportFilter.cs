using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NESI.Common.Interface
{
   public interface IReportFilter<T> where T : IModelBase
    {
        Task<List<string>> GetDistinct();
    }
}

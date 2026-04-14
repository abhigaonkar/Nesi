using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NESI.Common.Interface
{
    public interface IReportViewer<T> where T : class,IModelBase
    {
        Task<Response<T>> Search();

        Task<Response<T>> Search(List<string> listParams);

        Task<Report> GetSchema();

        Task<List<string>> GetDistinct();
    }
}

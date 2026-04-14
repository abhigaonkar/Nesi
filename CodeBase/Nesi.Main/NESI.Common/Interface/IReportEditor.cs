using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NESI.Common.Interface
{
    public interface IReportEditor<T> where T : IModelBase
    {

        Task<int> CreateModel(T model);

        Task<string> DeleteModel(string id);

        Task<bool> EditModel(T model);

        Task<List<KeyValuePair<string, bool>>> BulkEdit(T[] cvms);
    }
}

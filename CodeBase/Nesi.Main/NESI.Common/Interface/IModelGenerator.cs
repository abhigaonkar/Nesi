using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NESI.Common.Interface
{
	public interface IModelGenerator<T>
	 where T : class, IModelBase
	{


		IQueryable<T> GetQueryable(Expression<Func<T, bool>> match);

		IQueryable<T> GlobalSearch(string searchText, Expression<Func<T, bool>> match);
		List<string> SetColumnList { get; set; }
		Dictionary<string, string> GetColumnSummary { get; set; }
		List<string> GetDistinct(BodyParams _param);
		DataTable GetDatafromSource();
		DataTable GetDataFromStore();
		IReport GetSchemaFromStore();
		List<T> GetGroupFromStore(string item);
		DataTable GetFilterDataFromStore(string item);
		void SetFilterDataStore(string item, DataTable cache);
		DataTable GetGroupSummaryFromStore(string item);
		void SetGroupSummaryStore(string item, DataTable cache);

		void SetGroupStore(string item, List<T> cache);

		void SetSchemaStore(IReport cache);

		List<T> ConvertToList(List<DataRow> dtRows, List<T> wolist, bool addGroupbyMeta = false, string[] metadata = null);

		int GetRecordCountFromDataTable { get; set; }

		object ProcessExtra( DataTable table);
        object GetProcessExtra();

        object GroupSummary { get; set; }
        object CaculationsOnAllColumns { get; set; }
	}
}

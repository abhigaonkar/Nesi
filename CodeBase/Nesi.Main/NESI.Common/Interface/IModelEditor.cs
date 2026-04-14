using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NESI.Common.Interface
{
	public interface IModelEditor<T>
	  where T : class, IModelBase
	{


		Task<int> CreateModel(T model);

		Task<int> EditModel(T model);


		Task<string> DeleteModel(int id);

	}
}

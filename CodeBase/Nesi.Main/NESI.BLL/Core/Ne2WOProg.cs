using System.Linq;
using NESI.BLL.Base;

namespace NESI.BLL.Core
{
	public class Ne2WOProg : BLLBase
	{
		public readonly DTO.Models.Core.WoProg Entity;

		public Ne2WOProg(int woProgId)
		{
			Entity = AutoMapper.Mapper.Map<DTO.Models.Core.WoProg>(
				_db.woprog.FirstOrDefault(x => x.woprog_id == woProgId)
				);
		}
	}
}
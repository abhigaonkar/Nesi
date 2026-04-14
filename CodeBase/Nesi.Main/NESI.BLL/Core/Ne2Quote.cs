using System.Linq;
using NESI.BLL.Base;

namespace NESI.BLL.Core
{
	public class Ne2Quote : BLLBase
	{
		public readonly DTO.Models.Core.Quote_Master Entity;

		public Ne2Quote(int quoteId, int revision)
		{
			Entity = AutoMapper.Mapper.Map<DTO.Models.Core.Quote_Master>(
				_db.quote_master.FirstOrDefault(x => x.quote_id == quoteId && x.revision==revision)
			);
		}
	}
}
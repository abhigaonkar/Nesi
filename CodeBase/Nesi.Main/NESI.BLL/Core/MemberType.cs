using System.Linq;
using NESI.BLL.Base;

namespace NESI.BLL.Core
{
	public class MemberType : BLLBase
	{
		public readonly DTO.Models.Core.MemberType Entity;

		public MemberType(int Id)
		{
			Entity = GetMemberTypeById(Id);
		}

		protected DTO.Models.Core.MemberType GetMemberTypeById(int Id)
		{
			return AutoMapper.Mapper.Map<DTO.Models.Core.MemberType>(
				_db.membertype.FirstOrDefault(x => x.membertype_id == Id)
				);
		}
	}
}
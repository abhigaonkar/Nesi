using System;
using NESI.BLL.Base;

namespace NESI.BLL.Core.Logger
{
	public class ClientErrorHandler : BLLBase
	{
		public void AddLog(DTO.Models.Core.ErrorLog model)
		{
			if (string.Equals(model.error_short, "The operation was canceled.", StringComparison.CurrentCultureIgnoreCase))
			{
				return;
			}
			var err = AutoMapper.Mapper.Map<Data.Entities.error_log>(model);
			_db.error_log.Add(err);
			try
			{
				_db.SaveChanges();
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
				// throw;
			}

			if (model.Member_ID == 0) return;
			try
			{
				bllToolbox.doSQL_void(@" call error_log.insertError(@p0,@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8,@p9,@p10,@p11,@p12)",
					model.Member_ID, model.error_short, "", model.host_url, model.full_stacktrace, model.user_ip, model.origin, "", model.error_on_page,
					"", "", "", "");
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
			}

		}
	}
}
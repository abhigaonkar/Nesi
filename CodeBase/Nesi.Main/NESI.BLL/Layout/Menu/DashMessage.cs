using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using NESI.BLL.Base;
using NESI.BLL.Core.Employee;
using NESI.Data.Entities;

namespace NESI.BLL.Layout.Menu
{
	public class DashMessage : BLLBase
	{
	

		public DashMessage(Employee employee) : base(employee)
		{
	
		}

		public DTO.ViewModels.CurrentUser.Layout.DashMessage[] GetDashMessageListByBusinessUnitId(int id)
		{
			if (! CurrentUser.IsVisibleBusinessUnitId(id)) return null;

			var oDate = DateTime.Now.AddMonths(-6);
			var list = (from msg in _db.messageboard_chat
						join m in _db.member on msg.messageboard_memberid equals m.Member_ID
						where msg.business_unit_id == id
							  && msg.messageboard_date > oDate
						orderby msg.messageboard_chat_id
						select new DTO.ViewModels.CurrentUser.Layout.DashMessage()
						{
							Id = msg.messageboard_chat_id,
							Date = msg.messageboard_date,
							Name = m.member_fullname,
							Text = msg.messageboard_text,
							BusinessUnitId = m.business_unit_id,
							MemberId = m.Member_ID
						}).ToList();
			return list.ToArray();
		}

		public bool InsertDashMessage(DTO.ViewModels.CurrentUser.Layout.DashMessage msg)
		{
			var entity = new Data.Entities.messageboard_chat
			{
				business_unit_id = msg.BusinessUnitId,
				messageboard_date = DateTime.Now,
				messageboard_memberid = CurrentUser.Id,
				messageboard_text = msg.Text
			};
			_db.messageboard_chat.AddOrUpdate(entity);
			try
			{
				_db.SaveChanges();
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}
	}
}
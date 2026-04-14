using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using NESI.BLL.Base;
using NESI.BLL.Core.Employee;
using NESI.Data.Entities;
using NESI.DTO.ViewModels.Page.ReleaseSystem;
//using SharpSvn;
//using SharpSvn.Security;

namespace NESI.BLL.Pages.ReleaseSystem
{

	public class ReleaseSystem : BLLBase
	{
	  
		private string _trunkSrc = "https://ne-azserver-02.newelectric.local:8443/svn/NESI-Intranet/trunk/";
		private readonly NetworkCredential _credentials = new NetworkCredential("iis", "#33x66x99");
		private string _tagsPath = "https://ne-azserver-02.newelectric.local:8443/svn/NESI-Intranet/tags/";

		public ReleaseSystem()
		{

		}

		public ReleaseSystem(Employee _user) : base(_user)
		{

		}

		public string SetRebootTime(RebootTime _model)
		{
			try
			{
				if (_model.HostName.ToLower().StartsWith("www"))
				{
					_model.HostName = _model.HostName.Substring(3);
				}
				var obj = _db.n2_profile_property
							  .FirstOrDefault(_x => _x.property_name.ToLower() == _model.HostName.ToLower() &&
													_x.property_category_id == 2)
						  ?? new n2_profile_property();

				obj.property_category_id = 2;
				obj.property_name = _model.HostName;
				obj.property_defaullt = DateTime.Now.AddMinutes(_model.Minutes).ToString("yyyy-MM-dd HH:mm:ss");
				obj.property_description = (_model.ShutDownTime * 60).ToString();

				_db.n2_profile_property.AddOrUpdate(obj);
				_db.SaveChanges();
				return "Success";
			}
			catch (Exception ee)
			{
				return ee.Message;
			}
		}

		public int GetRebootTime(string _hostName)
		{
			_hostName = _hostName.Replace("www", "");
			var obj = _db.n2_profile_property 
				.FirstOrDefault(_x => _x.property_name == _hostName &&
									 _x.property_category_id == 2);
			if (obj == null)
			{
				return 0;
			}
			var isTime = false;
			var d = DateTime.Now;

			try
			{
				isTime = DateTime.TryParse(obj?.property_defaullt, out d);

			}
			catch (Exception)
			{
				//ignore
				// return 0;
			}

			var shutDowntime = 300;
			try
			{
				if (!int.TryParse(obj?.property_description, out shutDowntime))
				{
					shutDowntime = 300;
				}
			}
			catch (Exception)
			{
				//ignore 
			}
			return (isTime && d.AddSeconds(-1) > DateTime.Now) ? Convert.ToInt32((d - DateTime.Now).TotalSeconds)
				: d.AddSeconds(shutDowntime) > DateTime.Now ? Convert.ToInt32((DateTime.Now - d.AddSeconds(shutDowntime)).TotalSeconds) : 0;
		}
        

	}
}
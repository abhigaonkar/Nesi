using System.Data;

namespace nesi.core
	{
	/// <summary>
	/// Summary description for NeProvince
	/// </summary>
	public class NeProvince
		{
		public DataTable LoadAbbvList()
			{
			return Toolbox.doSQL_dt(@"SELECT * FROM Prov"  , null);
			}
		public DataTable Load()
			{
			var _provs					= Toolbox.doSQL_dt(@"SELECT prov_desc,prov_abbv FROM prov"  , null);
			return _provs;
			}
		}
	}
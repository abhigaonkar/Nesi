using System;
using System.Data;
using System.Web;

namespace nesi.core
{
	/// <summary>
	/// Summary description for NeQuotes
	/// </summary>
	public class NeERItem
	{

		private int _eritem_id = 0;

		private string _eritem_description;
		private string _eritem_notes;
		private DateTime _eritem_dateadded;
		private double _branch_newprice;
		private double _branch_repairprice;
		private double _branch_qtyinstock;
		private string _eritem_makename;
		private string _eritem_modelname;
		private DataTable _eritem_vendor_data;


		public int eritem_id
		{
			get { return _eritem_id; }
			set { _eritem_id = value; }
		}



		public string eritem_description
		{
			get { return _eritem_description; }
			set { _eritem_description = value; }
		}
		public string eritem_notes
		{
			get { return _eritem_notes; }
			set { _eritem_notes = value; }
		}
		public DateTime eritem_dateadded
		{
			get { return _eritem_dateadded; }
			set { _eritem_dateadded = value; }
		}
		public int eritem_addedby
		{
			get; set;
		}
		public double branch_newprice
		{
			get { return _branch_newprice; }
			set { _branch_newprice = value; }
		}
		public double branch_repairprice
		{
			get { return _branch_repairprice; }
			set { _branch_repairprice = value; }
		}
		public double branch_qtyinstock
		{
			get { return _branch_qtyinstock; }
			set { _branch_qtyinstock = value; }
		}
		public string eritem_makename
		{
			get { return _eritem_makename; }
			set { _eritem_makename = value; }
		}
		public string eritem_modelname
		{
			get { return _eritem_modelname; }
			set { _eritem_modelname = value; }
		}
		public DataTable eritem_vendor_data
		{
			get { return _eritem_vendor_data; }
			set { _eritem_vendor_data = value; }
		}

		public NeERItem()
		{
			//
			//  
			//
		}



		/// <summary>
		/// 
		/// </summary>
		/// <param name="business_unit_id"></param>
		/// <param name="eritem_id"></param>
		public NeERItem(int business_unit_id, int eritem_id)
		{
			//        OdbcConnection conn = NeDB.getQuotesAccessCon();

			var _tools = new Toolbox();
			var dt = Toolbox.doSQL_dt(@"Select * from eritem,er_branch_data  where er_branch_data_itemid = eritem_id and eritem_id= @v0 and business_unit_id = @v1", new object[] { eritem_id,business_unit_id });

			foreach (DataRow dr in dt.Rows)
			{
				_eritem_id = Convert.ToInt32(dr["eritem_id"]);

				eritem_addedby = Convert.ToInt32(dr["eritem_addedby"]);
				_eritem_dateadded = Convert.ToDateTime(dr["eritem_dateadded"]);
				_eritem_description = _tools.value_from(dr["eritem_description"].ToString()); ;
				_eritem_notes = _tools.value_from(dr["eritem_notes"].ToString());
				_branch_qtyinstock = Convert.ToDouble(dr["er_branch_data_qtyinstock"]);
				_branch_repairprice = Convert.ToDouble(dr["er_branch_data_repairprice"]);
				_branch_newprice = Convert.ToDouble(dr["er_branch_data_newprice"]);
				_eritem_makename = dr["eritem_make"].ToString();
				_eritem_modelname = dr["eritem_model"].ToString();
				_eritem_vendor_data = Toolbox.doSQL_dt(@"Select * from er_vendor_data,vendor  where er_vendor_data_vendor_id = vendor_id and er_vendor_data_itemid = @v0", new object[] { eritem_id });

			}

		}

		public void CreateFolder(int eriemid)
		{
			var item = new NeERItem(eriemid, 1);
			try
			{
				var name = item.eritem_makename + " " + item.eritem_modelname;
				var activeDir = "";
				if (HttpContext.Current != null)
				{
					activeDir = HttpContext.Current.Server.MapPath("/ERItemFolders");
				}
				var newPath = System.IO.Path.Combine(activeDir, "ER-" + item._eritem_id + "-" + name.Replace("&", " AND ").Replace("'", "").Replace("\"", "").Replace(".", "").Replace("+", "-").Replace("*", ""));
				// Create the subfolder
				System.IO.Directory.CreateDirectory(newPath);
			}
			catch { }
		}
		public string GetProjectFolder(int eriemid)
		{
			var item = new NeERItem(eriemid, 1);
			try
			{
				var name = item.eritem_makename + " " + item.eritem_modelname;
				var activeDir = "";
				if (HttpContext.Current != null)
				{
					activeDir = HttpContext.Current.Server.MapPath("/ERItemFolders");
				}
				var newPath = System.IO.Path.Combine(activeDir, "ER-" + item._eritem_id + "-" + name.Replace("&", " AND ").Replace("'", "").Replace("\"", "").Replace(".", "").Replace("+", "-").Replace("*", ""));
				var dirs = System.IO.Directory.GetDirectories(activeDir, "ER-" + item._eritem_id + "*", System.IO.SearchOption.TopDirectoryOnly);
				if (dirs.Length > 0)
				{
					return newPath;
				}
				else
				{
					return "";
				}



			}
			catch { return ""; }
		}
		public bool Save(int business_unit_id)
		{

			var _tools = new Toolbox();
			try
			{
				if (_eritem_id == 0)
				{

					_eritem_id = Convert.ToInt32(_tools.returnSQL_id(@"Insert into eritem
(eritem_make,eritem_model,eritem_description,eritem_dateadded,eritem_addedby)
Values (@v1,@v2,@v3,@v4)",
new object[] {
	_tools.value_to(_eritem_makename),
	_tools.value_to(_eritem_modelname),
	_tools.value_to(_eritem_description),
	_eritem_dateadded.ToString("yyyy-MM-dd"),
	eritem_addedby}));


					_tools.getSQL_void(@"Insert into er_branch_data (er_branch_data_itemid,business_unit_id,er_branch_data_repairprice,er_branch_data_newprice,er_branch_data_qtyinstock) values (@v0,@v1,@v2,@v3,@v4)", new Object[] { _eritem_id ,business_unit_id,_branch_repairprice,_branch_newprice,_branch_qtyinstock} );

					//               foreach (DataRow dr in _eritem_vendor_data.Rows)
					//               {
					//                   _tools.getSQL_void(@"Insert into er_vendor_data (er_vendor_data_itemid,er_vendor_data_vendor_id,er_vendor_data_cost,er_vendor_data_date,er_vendor_data_vendorpart)  values (@v0,@v1,@v2,@v3,@v4)",new object[] { dr["er_vendor_data_itemid"],dr["er_vendor_data_vendor_id"],dr["er_vendor_data_cost"],dr["er_vendor_data_date"],_tools.value_to(dr["er_vendor_data_vendorpart"]) } );
					//               }


				}
				else
				{
					_tools.getSQL_void(@"Update eritem 
set eritem_model = @v0, 
eritem_make =@v1, 
eritem_description =@v2, 
where eritem_id =@v3", new object[] { _tools.value_to(_eritem_modelname), _tools.value_to(_eritem_makename), _tools.value_to(_eritem_description), _eritem_id });
					_tools.getSQL_void(@"Update er_branch_data 
set er_branch_data_repairprice = @v0,  
er_branch_data_newprice = @v1, 
er_branch_data_qtyinstock =@v2 
where er_branch_data_itemid =@v3 ", new object[] { _branch_repairprice, _branch_newprice, _branch_qtyinstock, _eritem_id });
				}


			}
			catch (Exception ee)
			{
				throw new Exception(ee.Message);
			}
			return true;



		}
		public int add_vendor_row(int eritem_id, int vendorid, string vendorpart, double cost)
		{
			var id = "0";

			var _tools = new Toolbox();
			try
			{
				if (_eritem_id != 0)
				{

					id = _tools.returnSQL_id(@"Insert into er_vendor_data 
(er_vendor_data_itemid,er_vendor_data_vendor_id,er_vendor_data_cost,er_vendor_data_date,er_vendor_data_vendorpart) 
values (@v0,@v1,@v2,@v3,@v4)",
						new object[] {
eritem_id,vendorid, cost, DateTime.Today.ToString("yyyy-MM-dd") , _tools.value_to(vendorpart)});
				}

			}
			catch (Exception ee)
			{
				throw new Exception(ee.Message);
			}
			return Convert.ToInt32(id);


		}
		public void add_branch_data_row(int eritem_id, int business_unit_id, double newprice, double repairprice, double qtyinstock)
		{


			var _tools = new Toolbox();
			try
			{
				if (_eritem_id != 0)
				{

					_tools.getSQL_void(@"Insert into er_branch_data 
(er_branch_data_itemid,business_unit_id,er_branch_data_newprice,er_branch_data_date,er_branch_data_repairprice,er_branch_data_qtyinstock) 
values (@v0,@v1,@v2,@v3,@v4,@v5)",
						new object[] {
eritem_id,business_unit_id, newprice, DateTime.Today.ToString("yyyy-MM-dd"), repairprice , qtyinstock});
				}

			}
			catch (Exception ee)
			{
				throw new Exception(ee.Message);
			}



		}

	}
}
using NESI.Data.Entities;
using System;
using System.Linq;
using NESI.Common.Interface;
using System.Linq.Expressions;
using NESI.BLL.Base;
using NESI.DTO.ViewModels.Page.Reports;
using System.Collections.Generic;
using NESI.Common;
using System.Data;
using System.Threading.Tasks;

namespace NESI.BLL.Pages.Reports
{

	public class Address : BLLBase, IModelGenerator<DTO.ViewModels.Page.Reports.Address>
	{
		public int GetRecordCountFromDataTable { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
		public Dictionary<string, string> GetColumnSummary { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
		public List<string> SetColumnList { get; set; }
		public object GroupSummary { get; set; }
        public object CaculationsOnAllColumns { get; set; }
		public Task<int> CreateModel(DTO.ViewModels.Page.Reports.Address model)
		{
			throw new NotImplementedException();
		}

		public Task<int> EditModel(DTO.ViewModels.Page.Reports.Address model)
		{
			throw new NotImplementedException();
		}

		public Task<bool> DeleteModel(int id)
		{
			throw new NotImplementedException();
		}

		public IReport GetSchemaFromStore()
		{
			return null;
		}

		public List<string> GetDistinct(BodyParams _param)
		{
			var res = GetDataFromStore();
			var colName = _param.Selectby[0];
			return (res.Rows.Cast<DataRow>().Select(row => row[colName].ToString())).Distinct().ToList();
		}

		public List<DTO.ViewModels.Page.Reports.Address> GetGroupFromStore(string item)
		{
			return null;
		}

		public void SetGroupStore(string item, List<DTO.ViewModels.Page.Reports.Address> cache)
		{

		}

		public DataTable GetGroupSummaryFromStore(string item)
		{
			return null;
		}

		public void SetGroupSummaryStore(string item, DataTable cache)
		{

		}

		public DataTable GetFilterDataFromStore(string item)
		{
			return null;
		}

		public void SetFilterDataStore(string item, DataTable cache)
		{
		}

		public List<DTO.ViewModels.Page.Reports.Address> ConvertToList(List<DataRow> dtRows, List<DTO.ViewModels.Page.Reports.Address> wolist, bool addGroupbyMeta = false, string[] metadata = null)
		{
			throw new NotImplementedException();
		}

		public object ProcessExtra(DataTable table)
		{
			return null;
		}

		public DataTable GetDatafromSource()
		{
			throw new NotImplementedException();
		}

		public DataTable GetDataFromStore()
		{
			throw new NotImplementedException();
		}

		public void SetSchemaStore(IReport cache)
		{

		}
		public List<DTO.ViewModels.Page.Reports.Address> GetDataTableList()
		{
			throw new NotImplementedException();
		}

		public List<DTO.ViewModels.Page.Reports.Address> GetGlobalSearchList(string searchText)
		{
			throw new NotImplementedException();
		}

		public IQueryable<DTO.ViewModels.Page.Reports.Address> GetQueryable(Expression<Func<DTO.ViewModels.Page.Reports.Address, bool>> match = null)
		{
			//List<DTO.ViewModels.Page.Address> customer = new List<DTO.ViewModels.Page.Address>();
			var list = _db.address.Select(p => new DTO.ViewModels.Page.Reports.Address()
			{
				address_id = p.address_id,
				address_table = p.address_table,
				address_table_id = p.address_table_id,
				Address_Type = p.Address_Type,
				Address_Desc = p.Address_Desc,
				Address_Addr1 = p.Address_Addr1,
				Address_Addr2 = p.Address_Addr2,
				Address_Addr3 = p.Address_Addr3,
				Address_Addr4 = p.Address_Addr4,
				Address_City = p.Address_City,
				facebook = p.facebook,
				Address_Email = p.Address_Email,
				Address_Web = p.Address_Web,
				Location = "[" + p.Address_Type + "] - " + p.Address_Addr1
				/*
                address_sellprice = p.address_sellprice,
                Address_Prov = p.Address_Prov,
                Address_Postal = p.Address_Postal,
                Address_Country = p.Address_Country,
                Address_PhoneArea = p.Address_PhoneArea,
                Address_PhoneFirst = p.Address_PhoneFirst,
                Address_PhoneLast = p.Address_PhoneLast,
                Address_PhoneExt = p.Address_PhoneExt,
                address_phonefull = p.address_phonefull,
                Address_FaxArea = p.Address_FaxArea,
                Address_FaxFirst = p.Address_FaxFirst,
                Address_FaxLast = p.Address_FaxLast,
                Address_Terr = p.Address_Terr,
                Address_SalesPerson = p.Address_SalesPerson,
                Address_Ship = p.Address_Ship,
                Address_Tax1 = p.Address_Tax1,
                Address_Tax2 = p.Address_Tax2,
                Address_Tax3 = p.Address_Tax3,
                Address_Tax4 = p.Address_Tax4,
                Address_TaxEx1 = p.Address_TaxEx1,
                Address_TaxEx2 = p.Address_TaxEx2,
                Address_TaxEx3 = p.Address_TaxEx3,
                Address_TaxEx4 = p.Address_TaxEx4,
                Address_ContactName1 = p.Address_ContactName1,
                Address_ContactPhoneArea1 = p.Address_ContactPhoneArea1,
                facebook = p.facebook,
                */
			});
			if (match != null)
				return list.Where(match);
			else
				return list;
		}

		public IQueryable<DTO.ViewModels.Page.Reports.Address> GlobalSearch(string searchText, Expression<Func<DTO.ViewModels.Page.Reports.Address, bool>> match)
		{
			throw new NotImplementedException();
		}

        public object GetProcessExtra()
        {
            return null;
        }
    }
}

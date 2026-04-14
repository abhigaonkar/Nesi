using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NESI.Cache
{
	public static class Global
	{
		private static OnlineUser _onlineUser;
		private static BusinessUnit _businessUnit;
		private static TaxEntity _taxEntity;

		public static OnlineUser OnlineUser
		{
			get => _onlineUser ?? new OnlineUser();
			set => _onlineUser = value;
		}

		public static BusinessUnit BusinessUnit
		{
			get => _businessUnit ?? new BusinessUnit();
			set => _businessUnit = value;
		} 
		public static TaxEntity TaxEntity
		{
			get => _taxEntity ?? new TaxEntity();
			set => _taxEntity = value;
		}

		public static void RefreshBusinessUnit()
		{
			_onlineUser = null;
			_onlineUser=new OnlineUser();
		}
		public static void RefreshTaxEntity()
		{
			_taxEntity = null;
			_taxEntity = new TaxEntity();
		}

	}
}

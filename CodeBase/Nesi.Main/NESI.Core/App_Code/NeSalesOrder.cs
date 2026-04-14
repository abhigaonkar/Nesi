using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using MySql.Data.MySqlClient;
using NESI.Common.Models;

//using nesi.bv;

namespace nesi.core
	{
	/// <summary>
	/// Summary description for NeSalesOrder
	/// </summary>
	public class NeSalesOrder
		{
		#region private properties
		private string _OrderNumber = "";
		private string _CustomerNumber = "";
		private string _PONumber = "";
		private DateTime _OrderDate;
		private string _Status = "";
		private string _ReferenceNumber = "";
		private string _TerritoryCode = "/";
		private string _TerritoryDescription = "";
		private string _SalesPersonNumber = "/";
		private string _SalesPerson = "";
		private double _Discount = -100.01;
		private string _TermsCode = "/";
		private string _TermsDescription = "";
		private string _ShipToAddress = "";
		private string _DateRequired = "";
		private string _ShippingMethodCode = "/";
		private string _ShippingMethod = "";
		private bool _PrintPackingSlip;
		private DateTime _PackingSlipDate;
		private string _PackingSlipUser = "";
		private bool _PrintShippingLabel;
		private DateTime _ShippingLabelDate;
		private string _ShippingLabelUser = "";
		private string _FOB = "";
		private int _SalesTax1 = -1;
		private int _SalesTax2 = -1;
		private int _SalesTax3 = -1;
		private int _SalesTax4 = -1;
		private int _SellPriceLevel = -1;
		private string _Tax1Exempt = "/";
		private string _Tax2Exempt = "/";
		private double _Freight;
		private string _DSN = "";
		private int _Billing_Type_ID;
		private double _CURRENTCOST;
		private double _RETAILCOST;
		private double _AVERAGECOST;
		private double _TOTALCOST;
		private double _LABORCOST;
		private double _MATERIALCOST;
		private double _LABORPRICESELL;
		private double _MATERIALPRICESELL;
		private double _GROSSMARGIN;
		private double _PROGESSBILLED;
		private double _STILLTOBEBILLED;
		private double _TOTALTANDM;
		private string _PartNO = "";
		private string _DESC = "";
		private string _RECNUM = "";
		private double _QUANTITY;
		private double _ActualQUANTITY;
		private double _OrderedQuantity;
		private string _PARTLINEID = "";
		private bool _PartChanged;
		private bool _ForceNewPartLine;
		private string _OriginWorkOrder = "0";
		private double _CostOverRide;
		private string _POOrginInfo = "";
		private int _trackmemberid;
		private int _trackpaytypeid;
		private double _CustomerDiscount;
		private int _TrackPart;
		private bool _ManualChange;
		private double _TAX_MATERIAL;
		private double _TAX_LABOUR;
		private double _SATAX_MATERIAL;
		private double _SATAX_LABOUR;
		private double _TAX_MATADJUSTED;
		private double _TAX_LABADJUSTED;
		private double _TAX_QUOTEADJUSTED;
		private string _PO_RECNO = "";
        private double _THIS_LABORCOST;
        private double _THIS_MATERIALCOST;
        private double _THIS_LABORPRICESELL;
        private double _THIS_MATERIALPRICESELL;

        #endregion properties
        #region public properties
        public string POOrginInfo
			{
			get { return _POOrginInfo; }
			set
				{
				if (_POOrginInfo == value)
					{
					return;
					}
				_POOrginInfo = value;
				}
			}
		public string OriginWorkOrder { get { return _OriginWorkOrder; } set { _OriginWorkOrder = value; } }
		public bool forcenewpartline { get { return _ForceNewPartLine; } set { _ForceNewPartLine = value; } }
		public string DateRequired { get { return _DateRequired; } set { _DateRequired = value; } }
		public bool partchanged { get { return _PartChanged; } set { _PartChanged = value; } }
		public int BillingTypeID { get { return _Billing_Type_ID; } set { _Billing_Type_ID = value; } }
		public double Quantity { get { return _QUANTITY; } set { _QUANTITY = value; } }
		public double ActualQuantity { get { return _ActualQUANTITY; } set { _ActualQUANTITY = value; } }
		public double TAX_MATERIAL { get { return _TAX_MATERIAL; } set { _TAX_MATERIAL = value; } }
		public double TAX_LABOUR { get { return _TAX_LABOUR; } set { _TAX_LABOUR = value; } }
		public double SATAX_MATERIAL { get { return _SATAX_MATERIAL; } set { _SATAX_MATERIAL = value; } }
		public double SATAX_LABOUR { get { return _SATAX_LABOUR; } set { _SATAX_LABOUR = value; } }
		public double TAX_MATADJUSTED { get { return _TAX_MATADJUSTED; } set { _TAX_MATADJUSTED = value; } }
		public double TAX_LABADJUSTED { get { return _TAX_LABADJUSTED; } set { _TAX_LABADJUSTED = value; } }
		public double TAX_QUOTEADJUSTED { get { return _TAX_QUOTEADJUSTED; } set { _TAX_QUOTEADJUSTED = value; } }
		public string PO_RECNO { get { return _PO_RECNO; } set { _PO_RECNO = value; } }
		public double benchmark_material_sell  { get; set; }
		public double benchmark_labor_sell  { get; set; }

        public int wo_detail_current_Id { get; set; }

		public double OrderedQuantity
			{
			get { return _OrderedQuantity; }
			set
				{
				if (_OrderedQuantity == value)
					{
					return;
					}
				_OrderedQuantity = value;
				}
			}
		public string Notes {get;set;}
		public string partlineid  { get; set; }
		public int working_line_id  { get; set; }
		public string line_origin  { get; set; }
		public int transfer_to_line_id { get; set; }
		public string RecNum { get { return _RECNUM; } set { _RECNUM = value; } }
		public string PartNo { get { return _PartNO; } set { _PartNO = value; } }
		public string Desc { get { return _DESC; } set { _DESC = value; } }
		public string DSN { get { return _DSN; } set { _DSN = value; } }
		public string OrderNumber
			{
			get { return _OrderNumber; }
			set
				{
				if (value.Length <= 10)
					{
					_OrderNumber = value;
					}
				else
					{
					throw new Exception("Invalid Value");
					}
				}
			}
		public string CustomerNumber
			{
			get { return _CustomerNumber; }
			set
				{
				if (value.Length <= 20)
					{
					_CustomerNumber = value;
					}
				else
					{
					throw new Exception("Invalid Value");
					}
				}
			}
		public string PONumber
			{
			get { return _PONumber; }
			set
				{
				if (value.Length <= 20)
					{
					_PONumber = value;
					}
				else
					{
					throw new Exception("Invalid Value");
					}
				}
			}
		public DateTime OrderDate { get { return _OrderDate; } set { _OrderDate = value; } }
		public string Status
			{
			get { return _Status; }
			set
				{
				var strTemp = value.ToUpper();
				if (strTemp == "O" | strTemp == "H" | strTemp == "Q" | strTemp == "S" | strTemp == "L")
					{
					_Status = strTemp;
					}
				else
					{
					throw new Exception("Invalid Value");
					}
				}
			}
		public string ReferenceNumber
			{
			get { return _ReferenceNumber; }
			set
				{
				if (value.Length <= 20)
					{
					_ReferenceNumber = value;
					}
				else
					{
					throw new Exception("Invalid Value");
					}
				}
			}
		public string TerritoryCode
			{
			get { return _TerritoryCode; }
			set
				{
				if (value.Length <= 10)
					{
					_TerritoryCode = value;
					}
				else
					{
					throw new Exception("Invalid Value");
					}
				}
			}
		public string TerritoryDescription
			{
			get { return _TerritoryDescription; }
			set
				{
				if (value.Length <= 80)
					{
					_TerritoryDescription = value;
					}
				else
					{
					throw new Exception("Invalid Value");
					}
				}
			}
		public string SalesPersonNumber
			{
			get { return _SalesPersonNumber; }
			set
				{
				if (value.Length <= 10)
					{
					_SalesPersonNumber = value;
					}
				else
					{
					throw new Exception("Invalid Value");
					}
				}
			}
		public string SalesPersonName
			{
			get { return _SalesPerson; }
			set
				{
				if (value.Length <= 60)
					{
					_SalesPerson = value;
					}
				else
					{
					throw new Exception("Invalid Value");
					}
				}
			}
		public double Discount
			{
			get { return _Discount; }
			set
				{
				var dblTemp = Math.Round(value, 2);
				if (dblTemp >= -100.01 & dblTemp <= 100)
					{
					_Discount = value;
					}
				else
					{
					throw new Exception("Invalid value");
					}
				}
			}
		public string TermsCode
			{
			get { return _TermsCode; }
			set
				{
				if (value.Length <= 10)
					{
					_TermsCode = value;
					}
				else
					{
					throw new Exception("Invalid value");
					}
				}
			}
		public string TermsDescription
			{
			get { return _TermsDescription; }
			set
				{
				if (value.Length <= 60)
					{
					_TermsDescription = value;
					}
				else
					{
					throw new Exception("Invalid value");
					}
				}
			}
		public string ShipToAddress
			{
			get { return _ShipToAddress; }
			set
				{
				if (value.Length <= 20)
					{
					_ShipToAddress = value;
					}
				else
					{
					throw new Exception("Invalid value");
					}
				}
			}
		public string ShippingMethodCode
			{
			get { return _ShippingMethodCode; }
			set
				{
				if (value.Length <= 10)
					{
					_ShippingMethodCode = value;
					}
				else
					{
					throw new Exception("Invalid value");
					}
				}
			}
		public string ShippingMethod
			{
			get { return _ShippingMethod; }
			set
				{
				if (value.Length <= 60)
					{
					_ShippingMethod = value;
					}
				else
					{
					throw new Exception("Invalid value");
					}
				}
			}
		public bool PrintPackingSlip { get { return _PrintPackingSlip; } set { _PrintPackingSlip = value; } }
		public DateTime PackingSlipDate { get { return _PackingSlipDate; } set { _PackingSlipDate = value; } }
		public string PackingSlipUser
			{
			get { return _PackingSlipUser; }
			set
				{
				if (value.Length <= 3)
					{
					_PackingSlipUser = value;
					}
				else
					{
					throw new Exception("Invalid value");
					}
				}
			}
		public bool PrintShippingLabel { get { return _PrintShippingLabel; } set { _PrintShippingLabel = value; } }
		public DateTime ShippingLabelDate { get { return _ShippingLabelDate; } set { _ShippingLabelDate = value; } }
		public string ShippingLabelUser
			{
			get { return _ShippingLabelUser; }
			set
				{
				if (value.Length <= 3)
					{
					_ShippingLabelUser = value;
					}
				else
					{
					throw new Exception("Invalid value");
					}
				}
			}
		public string FOB
			{
			get { return _FOB; }
			set
				{
				if (value.Length <= 20)
					{
					_FOB = value;
					}
				else
					{
					throw new Exception("Invalid value");
					}
				}
			}
		public int SalesTax1
			{
			get { return _SalesTax1; }
			set
				{
				if (value >= -1)
					{
					_SalesTax1 = value;
					}
				else
					{
					throw new Exception("Invalid value");
					}
				}
			}
		public int SalesTax2
			{
			get { return _SalesTax2; }
			set
				{
				if (value >= -1)
					{
					_SalesTax2 = value;
					}
				else
					{
					throw new Exception("Invalid value");
					}
				}
			}
		public int SalesTax3
			{
			get { return _SalesTax3; }
			set
				{
				if (value >= -1)
					{
					_SalesTax3 = value;
					}
				else
					{
					throw new Exception("Invalid value");
					}
				}
			}
		public int SalesTax4
			{
			get { return _SalesTax4; }
			set
				{
				if (value >= -1)
					{
					_SalesTax4 = value;
					}
				else
					{
					throw new Exception("Invalid value");
					}
				}
			}
		public int SellPriceLevel
			{
			get { return _SellPriceLevel; }
			set
				{
				if ((value >= 1 & value <= 20) | value == -1)
					{
					_SellPriceLevel = value;
					}
				else
					{
					throw new Exception("Invalid value");
					}
				}
			}
		public string Tax1Exempt
			{
			get { return _Tax1Exempt; }
			set
				{
				if (value.Length <= 20)
					{
					_Tax1Exempt = value;
					}
				else
					{
					throw new Exception("Invalid value");
					}
				}
			}
		public string Tax2Exempt
			{
			get { return _Tax2Exempt; }
			set
				{
				if (value.Length <= 20)
					{
					_Tax2Exempt = value;
					}
				else
					{
					throw new Exception("Invalid value");
					}
				}
			}
		public double Freight
			{
			get { return _Freight; }
			set
				{
				var dblTemp = Math.Round(value, 2);
				_Freight = value;
				}
			}
		public double CurrentCost
			{
			get { return _CURRENTCOST; }
			set
				{
				var dblTemp = Math.Round(value, 2);
				_CURRENTCOST = value;
				}
			}
		public double SellPrice
			{
			get { return _RETAILCOST; }
			set
				{
				var dblTemp = Math.Round(value, 2);
				_RETAILCOST = value;
				}
			}
		public double AverageCost
			{
			get { return _AVERAGECOST; }
			set
				{
				var dblTemp = Math.Round(value, 2);
				_AVERAGECOST = value;
				}
			}
		public double SellOverride { get; set; }
		public double TotalCost
			{
			get { return _TOTALCOST; }
			set
				{
				var dblTemp = Math.Round(value, 2);
				_TOTALCOST = value;
				}
			}
		public double LaborCost
			{
			get { return _LABORCOST; }
			set
				{
				var dblTemp = Math.Round(value, 2);
				_LABORCOST = value;
				}
			}
		public double MaterialCost
			{
			get { return _MATERIALCOST; }
			set
				{
				var dblTemp = Math.Round(value, 2);
				_MATERIALCOST = value;
				}
			}
		public double LaborPriceSell
			{
			get { return _LABORPRICESELL; }
			set
				{
				var dblTemp = Math.Round(value, 2);
				_LABORPRICESELL = value;
				}
			}
		public double MaterialPriceSell
			{
			get { return _MATERIALPRICESELL; }
			set
				{
				var dblTemp = Math.Round(value, 2);
				_MATERIALPRICESELL = value;
				}
			}
		public double GrossMargin
			{
			get { return _GROSSMARGIN; }
			set
				{
				var dblTemp = Math.Round(value, 2);
				_GROSSMARGIN = value;
				}
			}
		public double ProgressBilled
			{
			get { return _PROGESSBILLED; }
			set
				{
				var dblTemp = Math.Round(value, 2);
				_PROGESSBILLED = value;
				}
			}
		public double StillToBeBilled
			{
			get { return _STILLTOBEBILLED; }
			set
				{
				var dblTemp = Math.Round(value, 2);
				_STILLTOBEBILLED = value;
				}
			}
		public double TotalTAndM
			{
			get { return _TOTALTANDM; }
			set
				{
				var dblTemp = Math.Round(value, 2);
				_TOTALTANDM = value;
				}
			}
		public double CostOverRide { get { return _CostOverRide; } set { _CostOverRide = value; } }
		public int trackmemberid
			{
			get { return _trackmemberid; }
			set
				{
				if (_trackmemberid == value)
					{
					return;
					}
				_trackmemberid = value;
				}
			}
		public int trackpaytypeid
			{
			get { return _trackpaytypeid; }
			set
				{
				if (_trackpaytypeid == value)
					{
					return;
					}
				_trackpaytypeid = value;
				}
			}
		public double CustomerDiscount
			{
			get { return _CustomerDiscount; }
			set
				{
				if (_CustomerDiscount == value)
					{
					return;
					}
				_CustomerDiscount = value;
				}
			}
		public int TrackPart
			{
			get { return _TrackPart; }
			set
				{
				if (_TrackPart == value)
					{
					return;
					}
				_TrackPart = value;
				}
			}
		public bool ManualChange
			{
			get { return _ManualChange; }
			set
				{
				if (_ManualChange == value)
					{
					return;
					}
				_ManualChange = value;
				}
			}
		public string committing_from_location		    { get; set; }
		public string uncommitting_to_location			{ get; set; }
        public double THIS_LABORCOST { get { return _THIS_LABORCOST; } set { _THIS_LABORCOST = value; } }
        public double THIS_MATERIALCOST { get { return _THIS_MATERIALCOST; } set { _THIS_MATERIALCOST = value; } }
        public double THIS_LABORPRICESELL { get { return _THIS_LABORPRICESELL; } set { _THIS_LABORPRICESELL = value; } }
        public double THIS_MATERIALPRICESELL { get { return _THIS_MATERIALPRICESELL; } set { _THIS_MATERIALPRICESELL = value; } }

		public string ActivityCode	{ get; set;}
		public string CostElement	{ get; set;}
		public string ClientWO	{ get; set;}
		public string ClientPO	{ get; set; }

		#endregion public properties
		#region methods

		//      public void LockOrder(string OrderNumber, string mem_name)
		//	{
		//	try
		//		{
		//		SalesOrder.LockOrder(_DSN, OrderNumber, mem_name);
		//		}
		//	catch (Exception ex)
		//		{
		//		Toolbox.do_errorLog_errorStack(ex);
		//		throw ex;
		//		}
		//	}
		//public string CheckOrderLock(string OrderNumber)
		//	{
		//	string strTemp;
		//	try
		//		{
		//		strTemp = SalesOrder.CheckOrderLock(_DSN, OrderNumber);
		//		return strTemp;
		//		}
		//	catch (Exception ex)
		//		{
		//		Toolbox.do_errorLog_errorStack(ex);
		//		throw ex;
		//		}
		//	}
		public bool getSalesOrderValues(string woid)
			{
			using(var conn = Toolbox.connect())
				{
				return getSalesOrderValues(conn, Convert.ToInt32(woid));
				}
			}
		public bool getSalesOrderValues(MySqlConnection _conn, int _woid)
			{
			var success = true;
			var dblQuantity = 0.00;
			var dblCost = 0.00;
			var dblPrice = 0.00;
			double laborcost = 0;
			double materialcost = 0;
			double laborprice = 0;
			double materialprice = 0;
			double bench_material = 0;
			double bench_labor = 0;
			double dblCostMultiplier = 1;
			var code = "";
			var SQL = "";
			double invisiblecredit = 0;
			double progressbillings = 0;
			double progressbillings_2 = 0;
			double visiblecredit = 0;
			double donotinclude = 0;
			double visiblenocharge = 0;
			decimal regular = 0;
			double BalanceForward = 0;
			double jobcostforquote = 0;
			double quoted = 0;
			double quoted_2 = 0;
			double grossmarginlaborcost = 0;
			double grossmarginmaterialcost = 0;
			var intPlacement = 0;
			var intPlacement2 = 0;
			var tablename = "wo_detail_current";
			double discount = 0;
			double tax_material = 0;
			double tax_material_Adjust = 0;
			double tax_labour = 0;
			double tax_labour_adjust = 0;
			double tax_quoted_adjust = 0;
			double satax_material = 0;
			double satax_labour = 0;
		
		
			var woprog = new NeWOProg(_woid);
			if (woprog.Status == "Invoiced" || woprog.Status == OpsWOStatus.WaitingToBeInvoiced)
				{
				tablename = "wo_detail_history";
				}

			//#2809 - if the WO is ProgressBill get the the value from JOBCOST_PROGRESSBILLED, since WOProg_InvoicedNetTotal will be 0 if nothing has been invoiced so far
			progressbillings = woprog.IsProgressBill ?
                Toolbox.doSQL_double(_conn, @"SELECT JOBCOST_PROGRESSBILLED(@v0)", new object[] { woprog.woprog_id }) :
			    Toolbox.doSQL_double(_conn, "SELECT IFNULL(SUM(WOProg_InvoicedNetTotal), 0) AS PBSUM FROM woprog WHERE WOProg_Associate_WOProg_ID =@v0 ", new object[] { _woid});

			var PayCost = Toolbox.doSQL_dt(_conn, "SELECT a.Member_ID, a.Member_Inv_BVNumber, b.WAGE, b.PAYTYPE FROM member AS a,currentwage AS b WHERE a.Member_ID = b.Member_ID AND a.Member_Status='Active'", null);
			try
				{
				//var strDetails = string.Format(@"SELECT * FROM {0} WHERE {0}_woprog_id = '{1}' AND {0}_billtypeid NOT IN (4,6,8,9)", tablename, woid);
				//	string strDetails = "SELECT * FROM wo_detail_" + tablename + " WHERE wo_detail_" + tablename + "_woprog_id = " + woid + " AND wo_detail_" + tablename + "_billtypeid != 4 AND wo_detail_" + tablename + "_billtypeid != 6 AND wo_detail_" + tablename + "_billtypeid != 8";
				//Inventory inventory = new Inventory()
				var details = Toolbox.doSQL_dt(_conn, string.Format(@"SELECT * FROM {0}  WHERE {0}_woprog_id = @v0  AND {0}_billtypeid NOT IN (4,6,8,9)", tablename), new object[] {   _woid } );
				foreach (DataRow dr in details.Rows)
					{
					discount = Convert.ToDouble(dr[tablename+"_discount"]);
					discount = 1 - discount/100;
					dblCostMultiplier = 1.25;
					code = dr[tablename+"_code"].ToString().ToUpper();
					double.TryParse(dr[tablename+"_price_cost"].ToString(), out dblCost);
					double.TryParse(dr[tablename+"_price_sell"].ToString(), out dblPrice);
					double.TryParse(dr[tablename+"_qty_committed"].ToString(), out dblQuantity);

					var linetype			= dr[tablename + "_type"].ToString();
					var line_cost		= Convert.ToDouble(dr[tablename+"_price_cost"]);
					var billtype_id			= Convert.ToInt32(dr[tablename + "_billtypeid"]);
					var extd_cost		= Math.Round(dblCost * dblQuantity, 4, MidpointRounding.AwayFromZero);
					var extd_sell		= Math.Round(dblPrice * dblQuantity, 3, MidpointRounding.AwayFromZero);
					var extd_sell_disc	= Math.Round(dblPrice * dblQuantity * discount, 3, MidpointRounding.AwayFromZero);

					if (code.StartsWith("LB") && billtype_id != 2 && billtype_id != 10)
						{
						intPlacement = code.IndexOf("DT");
						intPlacement2 = code.LastIndexOf("DT");
						if (intPlacement != -1)
							{
							if (intPlacement == intPlacement2 && intPlacement > 3)
								{
								code = code.Replace("DT", "");
								dblCostMultiplier += 1;
								}
							else if (intPlacement != intPlacement2 && intPlacement2 > 3)
								{
								code = code.Remove(intPlacement2, 2);
								dblCostMultiplier += 1;
								}
							}
						intPlacement = code.IndexOf("OT");
						intPlacement2 = code.LastIndexOf("OT");
						if (intPlacement != -1)
							{
							if (intPlacement == intPlacement2 && intPlacement > 3)
								{
								code = code.Replace("OT", "");
								dblCostMultiplier += .5;
								}
							else if (intPlacement != intPlacement2 && intPlacement2 > 3)
								{
								code = code.Remove(intPlacement2, 2);
								dblCostMultiplier += .5;
								}
							}
						intPlacement = code.IndexOf("SP");
						intPlacement2 = code.LastIndexOf("SP");
						if (intPlacement != -1)
							{
							if (intPlacement == intPlacement2 && intPlacement > 3)
								{
								code = code.Replace("SP", "");
								dblCostMultiplier += .1;
								}
							else if (intPlacement != intPlacement2 && intPlacement2 > 3)
								{
								code = code.Remove(intPlacement2, 2);
								dblCostMultiplier += .1;
								}
							}
						code = code.Trim();
						foreach (DataRow row in PayCost.Rows)
							{
							if (code == row[1].ToString() && row[3].ToString().Trim() != "Owner/NA")
								{
								dblCost = Convert.ToDouble(row[2].ToString())*dblCostMultiplier;
								}
							else if (code == row[1].ToString() && row[3].ToString().Trim() == "Owner/NA")
								{
								dblCost = 0;
								}
							}
						laborcost += extd_cost;
						if (dblCost == 0)
							{
							//	grossmarginlaborcost += Math.Round(dblPrice * dblQuantity, 4, MidpointRounding.AwayFromZero);
							}
						else
							{
							grossmarginlaborcost += extd_cost;
							}
						}
					else if (linetype == "L" && billtype_id != 7)
						{
						laborcost += extd_cost;
						
						if (dblCost == 0)
							{
							//					grossmarginlaborcost += Math.Round(dblPrice * dblQuantity, 4, MidpointRounding.AwayFromZero);
							}
						else
							{
							grossmarginlaborcost += extd_cost;
							}
						}
					else if (!Toolbox.Contains(billtype_id, new []{6,8,9,12})  && !(billtype_id == 3 && line_cost < 0.02))
						{
						materialcost += extd_cost;
						if (dblCost == 0)
							{
							//	grossmarginmaterialcost += Math.Round(dblPrice*dblQuantity,4, MidpointRounding.AwayFromZero);
							}
						else
							{
							grossmarginmaterialcost += extd_cost;
							}
						}

					// new code starts here
				
					switch (billtype_id)
						{
							case 0:
								regular += Math.Round(Math.Round((decimal)dblPrice * (decimal)dblQuantity, 3, MidpointRounding.AwayFromZero) * (decimal)discount, 2, MidpointRounding.AwayFromZero);
								if (linetype == "L")
									{
									laborprice += extd_sell_disc;
									bench_labor += extd_sell_disc;
									}
								else
									{
									materialprice += Convert.ToDouble(Math.Round(Math.Round((decimal)dblPrice * (decimal)dblQuantity, 3, MidpointRounding.AwayFromZero) * (decimal)discount, 2, MidpointRounding.AwayFromZero));
									bench_material += extd_sell;
									}
								break;
							case 1:
								jobcostforquote += extd_sell;
								if (linetype == "L")
									{
									bench_labor += extd_sell_disc;
									}
								else
									{
									bench_material += extd_sell;
									}
								break;
							case 2:
								invisiblecredit += extd_sell;
								if (linetype == "L")
									{
									laborprice += extd_sell_disc;
									bench_labor += extd_sell_disc;
									}
								else
									{
									materialprice += extd_sell;
									bench_material += extd_sell;
									}
								break;
							case 3:
								quoted += extd_sell;
								break;
							case 5:
								donotinclude += extd_sell;
								if (linetype == "L")
									{

									bench_labor += extd_sell_disc;
									}
								else
									{

									bench_material += extd_sell;
									}
								break;
							case 7:
								visiblenocharge += extd_sell_disc;
								break;
							case 9:
								// progressbillings += extd_sell;
								break;
							case 10:
								visiblecredit += Convert.ToDouble(Math.Round(Math.Round((decimal)dblPrice * (decimal)dblQuantity, 3, MidpointRounding.AwayFromZero) * (decimal)discount, 2, MidpointRounding.AwayFromZero));

								if (linetype == "L")
									{
									laborprice += extd_sell;
									bench_labor += extd_sell_disc;
									}
								else
									{
									materialprice += Convert.ToDouble(Math.Round(Math.Round((decimal)dblPrice * (decimal)dblQuantity, 3, MidpointRounding.AwayFromZero) * (decimal)discount, 2, MidpointRounding.AwayFromZero));
									bench_material += Convert.ToDouble(Math.Round(Math.Round((decimal)dblPrice * (decimal)dblQuantity, 3, MidpointRounding.AwayFromZero) * (decimal)discount, 2, MidpointRounding.AwayFromZero));
									}
								break;
							case 11:
								quoted_2 += Math.Round(dblPrice * dblQuantity, 3, MidpointRounding.AwayFromZero);
								break;
							case 12:
								// progressbillings_2 += Math.Round(dblPrice * dblQuantity, 3, MidpointRounding.AwayFromZero);
								break;
							case 13:
								BalanceForward += Math.Round(Math.Round(dblPrice * dblQuantity, 3, MidpointRounding.AwayFromZero) * discount, 2, MidpointRounding.AwayFromZero);
								break;
						}
					}
				}
			catch (Exception ex)
				{
				Toolbox.do_errorLog_errorStack(ex);
				success = false;
				}


			var add = new NEAddress(woprog.woprog_Address_ID);
			var comp = new NeBusinessUnit(woprog.business_unit_id);
			/*
			var tax1 = new NeTax(woprog.woprog_tax1);
			var tax2 = new NeTax(woprog.woprog_tax2);
			var tax3 = new NeTax(woprog.woprog_tax3);
			var tax4 = new NeTax(woprog.woprog_tax4);
			var query_tax	= string.Format(@"
SELECT
	ifnull(sum(((100-a.{0}_discount)*0.01)*a.{0}_qty_committed*a.{0}_price_sell*((ifnull(tax1.tax_percentage,0)+ifnull(tax2.tax_percentage,0)+ifnull(tax3.tax_percentage,0)+ifnull(tax4.tax_percentage,0))/100)),0) tax_total 
FROM
	{0} a
LEFT JOIN 
	tax tax1 ON a.{0}_tax1 = tax1.tax_id
LEFT JOIN 
	tax tax2 ON a.{0}_tax2 = tax2.tax_id
LEFT JOIN 
	tax tax3 ON a.{0}_tax3 = tax3.tax_id
LEFT JOIN 
	tax tax4 ON a.{0}_tax4 = tax4.tax_id
	", tablename);
			if (comp.TaxLabour == 1)
				{
				tax_labour = laborprice * ((tax1.Percentage + tax2.Percentage + tax3.Percentage + tax4.Percentage) / 100);
				tax_labour_adjust = Toolbox.doSQL_double(_conn, string.Format(@"
{0}
WHERE
	a.{1}_woprog_id = @v0 and 
	a.{1}_type = 'L' and 
	a.{1}_billtypeid in (0,2,10)", query_tax, tablename),new object[] {  woprog.woprog_id});	
		
				}
			if (comp.TaxMaterial == 1)
				{
				tax_material = materialprice * ((tax1.Percentage + tax2.Percentage + tax3.Percentage + tax4.Percentage) / 100);
				tax_material_Adjust = Toolbox.doSQL_double(_conn, string.Format(@"
{0}
WHERE
	a.{1}_woprog_id = @v0 AND 
	a.{1}_type = 'M' AND 
	a.{1}_billtypeid in (0,2,10)", query_tax, tablename), new object[] { woprog.woprog_id });


			}
			if (comp.selfassess_tax == 1)  // if company self assess
				{
				if (add.Tax1Exempt == "")  // if address is not tax exempt
					{
					if (comp.TaxQuotedJobs == 0 && woprog.QuoteID != "0")  // if it's a quoted job, the customer is not exempt and the branch doesn't charge tax on quoted jobs.. time to self assess material 
						{
						satax_material = materialcost * ((tax1.Percentage + tax2.Percentage + tax3.Percentage + tax4.Percentage) / 100);
						}
					else if (comp.TaxMaterial == 1 && materialprice != 0 && laborprice == 0)
						{
						satax_material = materialcost * ((tax1.Percentage + tax2.Percentage + tax3.Percentage + tax4.Percentage) / 100);
						}
					}
				else
					{
					if (comp.TaxMaterial == 1 && materialprice != 0 && laborprice == 0)
						{
						tax_material = materialprice * ((tax1.Percentage + tax2.Percentage + tax3.Percentage + tax4.Percentage) / 100);
						tax_material_Adjust = Toolbox.doSQL_double(_conn, string.Format(@"
{0}
WHERE
	a.{1}_woprog_id =@v0 AND 
	a.{1}_type = 'M' AND 
	a.{1}_billtypeid IN (0,2,10)", query_tax, tablename), new object[] { woprog.woprog_id });

					}
					}
				}
			if (comp.TaxQuotedJobs == 0 && woprog.QuoteID != "0")  // if it's a quoted job, the customer is not exempt and the branch doesn't charge tax on quoted jobs.. time to self assess material 
				{
				tax_material = 0;
				}
			if (comp.TaxQuotedJobs == 1)
				{
				tax_material = (quoted + quoted_2 + progressbillings_2 + (double) regular + invisiblecredit + visiblecredit - laborprice) * ((tax1.Percentage + tax2.Percentage + tax3.Percentage + tax4.Percentage) / 100);
				tax_material_Adjust = Toolbox.doSQL_double(_conn, string.Format(@"
{0}
WHERE
	a.{1}_woprog_id =@v0 AND
	(a.{1}_type = 'M' OR a.{1}_type = 'Q') AND 
	a.{1}_billtypeid in (0,2,3,10,11)", query_tax, tablename), new object[] { woprog.woprog_id });

			}
			*/
			_LABORCOST = laborcost;
			_LABORPRICESELL = laborprice;
			_MATERIALCOST = materialcost;
			_TAX_MATERIAL = tax_material;
			_TAX_LABOUR = tax_labour;
			_SATAX_LABOUR = satax_labour;
			_SATAX_MATERIAL = satax_material;
            //Created these variables for cl, or CLEAN numbers... Long decimal placed numbers cause problems when divided for gross margin.
			var cl_quoted = Math.Round(quoted, 2, MidpointRounding.AwayFromZero);
			var cl_regular = Math.Round(regular, 2, MidpointRounding.AwayFromZero);
			var cl_invisiblecredit = Math.Round(invisiblecredit, 2, MidpointRounding.AwayFromZero);
			var cl_visiblecredit = Math.Round(visiblecredit, 2, MidpointRounding.AwayFromZero);
			var cl_jobcostforquote = Math.Round(jobcostforquote, 2, MidpointRounding.AwayFromZero);
			var cl_donotinclude = Math.Round(donotinclude, 2, MidpointRounding.AwayFromZero);
			var cl_visiblenocharge = Math.Round(visiblenocharge, 2, MidpointRounding.AwayFromZero);
			var cl_quoted_2 = Math.Round(quoted_2, 2, MidpointRounding.AwayFromZero);
			var cl_progressbillings = Math.Round(progressbillings, 2, MidpointRounding.AwayFromZero);
			var cl_progressbillings_2 = cl_progressbillings * -1; //Math.Round(progressbillings_2, 2, MidpointRounding.AwayFromZero);

			//#2809 - Do not use negative cl_progressbillings_2 in order to mimic the behaviour of NeWOProg.fast_update_header_totals() method (lines to look in NeWOProg.cs ATM: 863, 977, 998 - 1009)
			if (woprog.IsProgressBill)
			{
				cl_progressbillings_2 = cl_progressbillings;
			}

			_MATERIALPRICESELL = Convert.ToDouble(Math.Round(Convert.ToDecimal(materialprice), 2, MidpointRounding.AwayFromZero));
            _STILLTOBEBILLED = cl_quoted + cl_quoted_2 + cl_progressbillings_2 + (double) cl_regular + cl_invisiblecredit + cl_visiblecredit - ((woprog.woprog_associate_woprog_id == 0 && woprog.Status == "Invoiced" && (progressbillings == 0 ||  progressbillings < cl_quoted + cl_quoted_2 && progressbillings + woprog.net_total == cl_quoted+cl_quoted_2 )) ? woprog.net_total : 0); // +progressbillvalue;
			_GROSSMARGIN	=	(
			            	 		cl_quoted + cl_quoted_2 + cl_progressbillings_2 + (double) cl_regular + cl_invisiblecredit + cl_visiblecredit + BalanceForward - 
			            	 		(grossmarginlaborcost + grossmarginmaterialcost)
			            	 	) /
			            	 	(cl_quoted + cl_quoted_2 + cl_progressbillings_2 + (double) cl_regular + cl_invisiblecredit + cl_visiblecredit + BalanceForward);
			if (_GROSSMARGIN.ToString().Contains("Infinity") ||double.IsNaN(_GROSSMARGIN) || _STILLTOBEBILLED == 0)
				{
				_GROSSMARGIN = 0;
				}
			_GROSSMARGIN = _GROSSMARGIN > 100 ? 100 : _GROSSMARGIN;
			_TOTALTANDM = (double) cl_regular +  cl_jobcostforquote + cl_donotinclude + cl_visiblenocharge + BalanceForward; // +invisiblecredit + visiblecredit;
			_PROGESSBILLED = progressbillings ;
			_TAX_LABADJUSTED = tax_labour_adjust;
			_TAX_MATADJUSTED = tax_material_Adjust;
			_TAX_QUOTEADJUSTED = tax_quoted_adjust;
			benchmark_labor_sell		= bench_labor;
			benchmark_material_sell		= bench_material;

            _THIS_LABORCOST = laborcost;
            _THIS_LABORPRICESELL = benchmark_labor_sell;
            _THIS_MATERIALCOST = materialcost;
            _THIS_MATERIALPRICESELL = benchmark_material_sell;

			if (woprog.QuoteID != "0")
				{
				benchmark_labor_sell = Toolbox.doSQL_double(_conn, @"SELECT GET_TOTAL_LABOR_BENCHMARK_SELL_FROM_ALL_WORKORDERS(@v0)", new object[] { woprog.woprog_id } );
				benchmark_material_sell = Toolbox.doSQL_double(_conn, @"SELECT GET_TOTAL_MATERIAL_BENCHMARK_SELL_FROM_ALL_WORKORDERS(@v0)", new object[] { woprog.woprog_id } );
				_TOTALTANDM = benchmark_labor_sell + benchmark_material_sell;
				_LABORCOST = NeWOProg.GetTotalLineCost(_conn, woprog.woprog_id, "L");
				_MATERIALCOST = NeWOProg.GetTotalLineCost(_conn, woprog.woprog_id, "M");

				}

			return success;
			}
		
		public int SavePart(int _woprog_id, string transferWO, string DSN, string strMemberName, int _member_id, bool adding_line, string holdcode, bool force = false, int consignment_id= 0, string original="", bool resetConsigmentId = false)
			{
			using (var conn = Toolbox.connect())
				{
				var detail = new NeWODetailCurrent();
				var woprog				= new NeWOProg();
				var inventory			= new inventory();
				var intFlag = -1;
				double used_cost = 0;
				var strIsQTY = "";
				double workordercost = 0;
				int master_id = 0;
				var isTransferRequest = transferWO.ToUpper() == "TRUE";
				int.TryParse(_PartNO, out master_id);
				var prev_values		= new NeWODetailCurrent();
				if(Convert.ToInt32(partlineid) > 0 && working_line_id == 0)
					{
					working_line_id = Convert.ToInt32(partlineid);
					}
				if(working_line_id > 0 && !isTransferRequest)
					{
					detail		= new NeWODetailCurrent(working_line_id);
					prev_values	= new NeWODetailCurrent(working_line_id);
					}
				else if(isTransferRequest && transfer_to_line_id > 0)
					{
					detail		= new NeWODetailCurrent(transfer_to_line_id);
					prev_values	= new NeWODetailCurrent(transfer_to_line_id);
					}
			

          
			woprog = new NeWOProg(_woprog_id);


			var bu = new NeBusinessUnit(woprog.business_unit_id);
            if (master_id < OpsSpecialPart.LaborThreshold)
            {
                inventory.Load(master_id, bu.warehouse_bu_id);
            }
            if (!adding_line)
				{
				workordercost = detail.cost;
				}
		
			try
				{
				if (master_id >= OpsSpecialPart.LaborThreshold)
					{
					//  dblCostPrice = workordercost;
					}
				else if (master_id != OpsSpecialPart.OldSubContractor)
					{
					try
						{
						used_cost = Toolbox.doSQL_double(conn, @"SELECT GET_CURRENT_COST(@v0,@v1)",new object[] { master_id,woprog.business_unit_id } );
						if (!adding_line && workordercost != 0 && _QUANTITY < 0)
							{
							used_cost = workordercost;
							}
						if (_CostOverRide != 0)
							{
							used_cost = _CostOverRide;
							}
						}
					catch (Exception ee)
						{
						Toolbox.do_errorLog_errorStack(ee);
						throw;
						}
					}
				else if(master_id == OpsSpecialPart.OldSubContractor)
					{
					used_cost = detail.cost;
					}
				if (inventory.Tag.id == OpsSpecialTag.GL)
					{
					throw new Exception("You Cannot Add A GL Part to a Work Order!!");
					}
				if (inventory.is_qty)
					{
					strIsQTY = "QTY";
					}
				var counter = detail.GetLineCount(woprog.woprog_id);
				double dblComQty = 0;
				var strComQty = "";
				if (master_id < OpsSpecialPart.LaborThreshold)
					{
					if (inventory.is_exclude&&_PO_RECNO!="")
						{
						strComQty = "SELECT IFNULL(SUM(wo_detail_current_qty_committed),0) FROM wo_detail_current WHERE wo_detail_current_woprog_id = " + _woprog_id + "  AND wo_detail_current_master_id = '" + master_id + "' AND wo_detail_current_origin like '%" + _PONumber + "-" + _PO_RECNO + "%' and wo_detail_current_price_cost = " + used_cost;

					}
					else if (inventory.is_exclude)
						{
						strComQty = "Select 0";
						}
					else
						{
						strComQty = "SELECT IFNULL(SUM(wo_detail_current_qty_committed),0) FROM wo_detail_current WHERE wo_detail_current_woprog_id = " + _woprog_id + "  AND wo_detail_current_master_id = '" + master_id + "'";
						}
					}
				else
					{
					strComQty = "SELECT IFNULL(SUM(wo_detail_current_qty_committed),0) FROM wo_detail_current WHERE wo_detail_current_woprog_id = " + _woprog_id + "  AND wo_detail_current_master_id = '" + master_id + "' and wo_detail_current_description = '" + Toolbox.AddSlashes(_DESC) + "'";
					}
				try
					{
					dblComQty = Toolbox.doSQL_double(conn, strComQty,null);
					}
				catch(Exception ee)
					{
					Toolbox.do_errorLog_errorStack(ee);
					dblComQty = 0;
					}

			//	detail.paytypeid				= _trackpaytypeid;
			//	detail.memberid					= _trackmemberid;
				detail.description				= _DESC ?? inventory.description_full;
				detail.billtypeid				= BillingTypeID;
				detail.business_unit_id			= woprog.business_unit_id;  // this needs to be changed when we go to mulitple bu's on one work order
				detail.woprog_id				= woprog.woprog_id;
				detail.date_required			= _DateRequired;
				detail.notes					= Notes;
				detail.added_by					= detail.id > 0 ? detail.added_by : _member_id;
				detail.date_added				= inventory.is_exclude? detail.date_added:Toolbox.MySQLNow_long();
				detail.date_modified			= Toolbox.MySQLNow_long();
				detail.master_id				= master_id;

				detail.ActivityCode				= ActivityCode ?? detail.ActivityCode;
				detail.CostElement				= CostElement ?? detail.CostElement;
				detail.ClientWO					= ClientWO ?? detail.ClientWO;
				detail.ClientPO					= ClientPO ?? detail.ClientPO;

                detail.qty_committed			= POOrginInfo == "" || force 
													? _QUANTITY 
													: detail.qty_committed; // affect commit

				detail.committing_from_location	= committing_from_location;
				detail.uncommitting_to_location	= uncommitting_to_location;
				//Does Line Already Exist
				detail.type                         = master_id == OpsSpecialPart.QuoteLine 
														? OpsWOLineType.Quote
														: master_id < OpsSpecialPart.LaborThreshold 
															? OpsWOLineType.Material 
															: _trackpaytypeid == OpsPayType.Mileage
																? OpsWOLineType.Mileage
																:OpsWOLineType.Labor;
				if(!inventory.allowed_to_stock && detail.master_id < OpsSpecialPart.LaborThreshold && !inventory.is_exclude && !isTransferRequest && PO_RECNO == "")
					{
					var temp_qty		= detail.qty_committed + dblComQty <= 0 ? 1 : detail.qty_committed + dblComQty;
					detail.cost			= detail.qty_committed > 0 
												? Toolbox.doSQL_double(@"SELECT GET_COST_AT_QTY(@v0 , @v1 , 0, @v2 )", new object[] {  detail.master_id, detail.business_unit_id, temp_qty } ) 
												: detail.cost;
					detail.sell			= detail.qty_committed > 0
                        ? woprog.use_fixed_material_markup ? woprog.fixed_material_markup * used_cost :
                            shared.GetSellPrice(detail.cost, 0, true, detail.qty_committed + dblComQty, detail.business_unit_id) 
						: ManualChange 
							? SellOverride
							: detail.sell; 
					detail.unit			= detail.sell;
					}
				else
					{
					detail.cost			= detail.cost == 0 ? used_cost : detail.cost;
					if(detail.type == OpsWOLineType.Labor || detail.type == OpsWOLineType.Mileage)
						{
						detail.cost		= CostOverRide > 0 ? CostOverRide : 0;
						detail.sell		= SellOverride;
						detail.unit		= SellOverride;
						}
					else if(detail.type == OpsWOLineType.Material)
						{
						var used_qty	= inventory.is_exclude 
											? detail.qty_committed + prev_values.qty_committed 
											: detail.qty_committed + dblComQty;
						CostSellManager(inventory, ref detail, used_qty);
						}
					}

				detail.unit                         = detail.sell;
				detail.qty_invoiced                 = detail.qty_committed;
				detail.qty_ordered                  = _OrderedQuantity;
				detail.rec_no                       = adding_line ? counter + 1 : detail.rec_no;
				detail.code					        = _Billing_Type_ID == OpsBillType.Comment 
															? "COMMENT" 
															: _Billing_Type_ID == OpsBillType.Blank 
																? "BLANKLINE" 
																: holdcode != "" 
																	? holdcode 
																	: master_id.ToString();
				var mobile_adding= false;
				if (HttpContext.Current != null)
					{
					var current_url = HttpContext.Current.Request.Url;
					mobile_adding = current_url.Port == 80 && current_url.AbsoluteUri.Contains("mobile/part_management/");
					}
				if(detail.id == 0)
					{
					line_origin						= string.IsNullOrEmpty(line_origin) && detail.id == 0 ? (mobile_adding ? "Scanner Added" : "Manually Added") : line_origin;
					detail.origin					= line_origin;
					}
				else
					{
					detail.origin					+= "\n"+line_origin;
					}
				detail.bvwo                         = Convert.ToInt32(woprog.OrderNumber);
				detail.discount                     = _CustomerDiscount;
				detail.track_part			        = adding_line || TrackPart == 0 ? 0 : TrackPart;
				if(!isTransferRequest && transfer_to_line_id > 0)
					{
					detail.is_transferring_to	= true;
					detail.transferring_to_bvwo	= transfer_to_line_id.ToString().PadLeft(10, '0');
					}
				if (isTransferRequest) 
					{
					#region transferWO
					if(detail.type == "L") throw new Exception("Labor may not be transferred");
					detail.qty_committed = POOrginInfo == "" ? _QUANTITY : detail.qty_committed; // effectcommit
					detail.qty_invoiced = detail.qty_committed;
					detail.qty_ordered = Quantity;
					//Set Price
					var dblExtistQty		= Toolbox.doSQL_double(conn, string.Format("SELECT IFNULL(MAX(wo_detail_current_qty_committed), 0) FROM wo_detail_current WHERE wo_detail_current_woprog_id = '{0}' AND business_unit_id = '{1}' AND wo_detail_current_master_id = '{2}'", woprog.woprog_id, woprog.business_unit_id, master_id),null);
					var strtransferqty = "0";
					//if (detail.master_id >= 990000)
					//	{
					//	detail.type = "L";
					//	detail.cost = workordercost;
					//	}
					var transfertrack = 0;
					detail.track_part = transfertrack;
					dblComQty = dblExtistQty;

					var newtransferqty = _QUANTITY + dblExtistQty;
					double transferprice = woprog.use_fixed_material_markup
												? woprog.fixed_material_markup*used_cost:  
												shared.GetSellPrice(used_cost, 0, strIsQTY, newtransferqty, detail.business_unit_id);
					if (transferprice == 0)
						{
						transferprice = _RETAILCOST;
						}
					//else
					//	{
					//	transferprice = _RETAILCOST;
					//	}
					detail.sell                     = transferprice;
					detail.unit                     = detail.sell;
					detail.discount		            = 0;
					detail.origin					= detail.origin == "" ? _QUANTITY + " Transferred From: " + _OriginWorkOrder : _QUANTITY + " Transferred From: " + _OriginWorkOrder +"\n"+detail.origin;
					detail.is_transferring_from		= true;
					detail.transferring_from_bvwo	= _OriginWorkOrder;
					var current_user				= new NeMember(_member_id);
					detail.notes					+= "[" + current_user.FullName + "] - " + Toolbox.MySQLNow_long() + "\nTransferred " + _QUANTITY + " From: " + _OriginWorkOrder;
					detail.billtypeid               = woprog.QuoteID == "0" ? 0: 1;
					#endregion
					}
				

                //
                // Bug fixed for [1122] when transferred a CC or Per diem  expense, the code should switch the consignment between the new wo line item and old one.
                //
				//if ( (master_id == SpecialParts.ExpenseReimbursement || master_id == SpecialParts.PerDiemExpense || master_id == SpecialParts.CompanyCreditCardExpense )
				//    && (original == "Expense Reimbursement" || original == "Per Diem Expense" || original == "Company Credit Card Expense") && resetConsigmentId == true)
				//{
                //    // we only update the  consignment_id when the users explicitly set resetConsigmentId to true in case of expense reimbursement.
                //    detail.consignment_id = consignment_id;
				//}

				detail.save(new NeMember(_member_id), "NESalesOrder - BVPartSave", ManualChange);
				intFlag = detail.rec_no;
				this.wo_detail_current_Id = detail.id;
				double emailqty = 0;
				if (adding_line && detail.track_part == 1 && detail.qty_committed > 0)
					{
					//Send Email
					emailqty = _ActualQUANTITY - dblComQty;
					PartTrackingEmails(detail.master_id.ToString(),
						detail.added_by,
						detail.woprog_id.ToString(), _QUANTITY, detail.description);
					}
				else if (detail.track_part == 1 && _ActualQUANTITY != dblComQty)
					{
					//Send Email
					emailqty = _ActualQUANTITY - dblComQty;
					PartTrackingEmails(detail.master_id.ToString(),
						detail.added_by,
						detail.woprog_id.ToString(), _QUANTITY, detail.description);
					}
				}
			catch (Exception ex)
				{
				Toolbox.do_errorLog_errorStack(ex);
				throw;
				}
			return intFlag;
				}
			}
		private void CostSellManager(inventory i, ref NeWODetailCurrent workOrderLine, double incomingQty)
			{
            var useFixedMarkUp		= false;
			var isNew				= workOrderLine.id == 0;
            double fixedMarkup		= 1;
            var woHeader			= Toolbox.doSQL_dt(@"SELECT use_fixed_material_markup, fixed_material_markup FROM woprog WHERE woprog_id=@v0", new object[] { workOrderLine.woprog_id });
            foreach (DataRow woLine in woHeader.Rows)
				{
                useFixedMarkUp = Convert.ToBoolean(woLine["use_fixed_material_markup"]);
                fixedMarkup = Convert.ToDouble(woLine["fixed_material_markup"]);
				}
			var currentQuantity		= workOrderLine.id == 0 
										? 0 
										: NeWODetailCurrent.CurrentCommittedQuantity(workOrderLine.id);

		    var UsedCost			= CostOverRide > 0 
										? CostOverRide
										: workOrderLine.cost;
			var costChanged			= Math.Abs(CostOverRide - workOrderLine.cost) > 0.0001;
			workOrderLine.cost		= UsedCost; 
			if(workOrderLine.master_id == OpsSpecialPart.QuoteLine)
				{ 
				if(ManualChange)
					{
					workOrderLine.sell = SellOverride;
					}
				}
			else
				{
				var costBlended			= costChanged && !ManualChange;
				var sellChanged			= ManualChange && Math.Abs(SellOverride - workOrderLine.sell) > 0.001;
				var receivingMore		= Math.Abs(incomingQty - currentQuantity) > 0.0001 &&
												!isNew && 
												workOrderLine.master_id != OpsSpecialPart.MiscMaterial;
				var fixedSellPrice		= useFixedMarkUp 
											? fixedMarkup * UsedCost : 
											0;
				var markedUpSellPrice	= shared.GetSellPrice(workOrderLine.cost, 0, i.is_qty, incomingQty, workOrderLine.business_unit_id);
				var sellOverrodeOnInsert	= isNew && Math.Abs(SellOverride - markedUpSellPrice) > 0.02;
				var zeroSellPrice			= Math.Abs(SellOverride) < 0.0001;

				if (!sellOverrodeOnInsert && costBlended || receivingMore || zeroSellPrice)
					{
					workOrderLine.sell = useFixedMarkUp
											? fixedSellPrice
											: markedUpSellPrice;
					}
				else if (sellChanged)
					{
					workOrderLine.sell = SellOverride;
					}
				}
			workOrderLine.unit			= workOrderLine.sell;
			}
		#region Part Tracking Emails
		//Send out Part Tracking Emails
		protected void PartTrackingEmails(string masterid, int memberid, string woprogid, double qtycom, string partdesc)
			{
			//get members who are tracking this part
			var mail = new NeEMail();
			var testid = "";
			var GLAccName = "";
			try
				{
				testid = Toolbox.doSQL_string(@"Select ifnull((SELECT gl_te.id FROM gl_te WHERE gl_te.id = @v0 ),'')", new object[] {  woprogid } );
				GLAccName = Toolbox.doSQL_string(@"Select ifnull((SELECT gl_te.account_no FROM gl_te WHERE gl_te.id = @v0) ,'')", new object[] {  woprogid } );
				}
			catch(Exception ex)
				{
				Toolbox.do_errorLog_errorStack(ex);

				}
			var bvwo = "";
			if (woprogid == "9999999" || woprogid == "9999998" || woprogid == "9999997")
				{
				bvwo = "stock";
				}
			else if (testid != "")
				{
				bvwo = "GL Account-" + GLAccName;
				}
			else
				{
				bvwo = new NeWOProg(Convert.ToInt32(woprogid)).OrderNumber;
				}
			var bv_wonumber		= 0;
			int.TryParse(bvwo, out bv_wonumber);
			var wo			= bv_wonumber == 0 ? new NeWOProg() : new NeWOProg(Convert.ToInt32(woprogid));
			var pm			= bv_wonumber == 0 ? new NeMember() : new NeMember(wo.intProjectManager);
			var informmember = new NeMember(memberid);
			var subject = string.Format(@"
{4} - {0} {2}. Qty: {1} has arrived and is in stock for WO {3}.",
				masterid,
				qtycom,
				Toolbox.do_value_from(partdesc).Replace("&quot;", ""),
				bvwo,
				pm.FirstName);
			if (informmember.NEEmail != "")
				{
				mail.To = informmember.NEEmail;
				mail.Subject = subject;
				mail.From = "administrator@thatsnew.com";
				mail.Body = "";
				try
					{
					mail.CC	= Toolbox.doSQL_string(@"SELECT member_neemail FROM member  WHERE member_membertype_id = 9 AND member_status='Active' AND business_unit_id=@v0 limit 1 ", new object[] { wo.business_unit_id });
					}
				catch (Exception ex)
					{
					Toolbox.do_errorLog_errorStack(ex);
					mail.CC = "";
					}
				mail.CC		= bv_wonumber == 0 ? mail.CC : mail.CC != "" ? mail.CC+";"+pm.NEEmail : pm.NEEmail;
				try
					{
					mail.Send();
					}
				catch (Exception ex)
					{
					Toolbox.do_errorLog_errorStack(ex);
					}
				}
			}
		#endregion
		//		public void MoveFromDetailsCurrentToSalesOrderDetails(string woid, int _business_unit_id, string DSN, string BVWO)
		//			{
		//			var strTemp = new NeSalesOrder();
		//			var this_wo = new NeWOProg(Convert.ToInt32(woid));
		//			//bool tax3;
		//			//bool tax4;
		//			strTemp.DSN = DSN;
		//			//var templock = strTemp.CheckOrderLock(BVWO); // Check if WO is Locked
		//			//string DecriptionHolder = "";
		//			//if (templock == "")
		//			//	{
		//			//	SalesOrder.LockOrder(DSN, BVWO, "DSM-Move");
		//			//	}
		//			//else
		//			//	{
		//			//	throw new Exception("This Work Order is locked by " + templock);
		//			//	}
		//			try
		//				{
		//				Toolbox.doSQL_void(@"UPDATE wo_detail_current set wo_detail_current_date_modified = wo_detail_current_date_modified, wo_detail_current_price_unit = wo_detail_current_price_sell  WHERE wo_detail_current_woprog_id =@v0", new object[] { woid });
		//				var strMemberName = "DSM";
		//				var strGetRegularDetails = "";
		//				strGetRegularDetails = @"
		//SELECT 
		//	wo_detail_current_description descrip, 
		//	wo_detail_current_qty_committed, 
		//	wo_detail_current_price_cost, 
		//	wo_detail_current_price_sell, 
		//	wo_detail_current_price_unit, 
		//	wo_detail_current_tax1,
		//	wo_detail_current_tax2, 
		//	wo_detail_current_code, 
		//	wo_detail_current_billtypeid, 
		//	wo_detail_current_tax3,
		//	wo_detail_current_tax4, 
		//	wo_detail_current_discount,
		//	wo_detail_current_master_id,
		//	wo_detail_current_type 
		//FROM 
		//	wo_detail_current 
		//WHERE 
		//	wo_detail_current_woprog_id = @v0 AND 
		//	wo_detail_current_billtypeid IN (0,3,6,7,8,10,11,12) 
		//ORDER BY 
		//	wo_detail_current_rec_no";
		//				var strGetInvisCreditDetails = "";
		//				strGetInvisCreditDetails = @"
		//SELECT 
		//	wo_detail_current_description descrip, 
		//	wo_detail_current_qty_committed, 
		//	wo_detail_current_price_cost, 
		//	wo_detail_current_price_sell, 
		//	wo_detail_current_price_unit, 
		//	wo_detail_current_tax1,
		//	wo_detail_current_tax2, 
		//	wo_detail_current_code, 
		//	wo_detail_current_billtypeid, 
		//	wo_detail_current_tax3,
		//	wo_detail_current_tax4, 
		//	wo_detail_current_discount,
		//	wo_detail_current_master_id,
		//	wo_detail_current_type 
		//FROM 
		//	wo_detail_current 
		//WHERE 
		//	wo_detail_current_woprog_id = @v0 AND 
		//	wo_detail_current_billtypeid = 2 
		//ORDER BY 
		//	wo_detail_current_rec_no";
		//				var myRegularTable = new DataTable();
		//				myRegularTable = Toolbox.doSQL_dt(strGetRegularDetails,new object[] { woid});
		//				var myInvisbleCreditTable = new DataTable();
		//				myInvisbleCreditTable = Toolbox.doSQL_dt(strGetInvisCreditDetails, new object[] { woid });
		//				var sa = new SalesOrder();
		//				sa.Load(DSN, BVWO);
		//				var this_address			= new NEAddress(this_wo.woprog_Address_ID);
		//				var this_businessUnit			= new NeBusinessUnit(this_wo.business_unit_id);
		//				var this_customer		= new NECustomer(this_wo.WOProg_Customer_ID);
		//				var current_user			= new NeMember(1316);

		//				sa.ne_address = this_address;
		//				sa.ne_company = this_businessUnit;
		//				sa.ne_customer = this_customer;
		//				sa.current_user = current_user;

		//				var st = sa.LineItems;
		//				var has_comment_line = false;
		//				foreach(SalesOrderItem line in st)
		//					{
		//					if(line.PartNumber.Trim() == "")
		//						{
		//						has_comment_line = true;
		//						break;
		//						}
		//					}
		//				if(!has_comment_line)
		//					{
		//					// This indicates something went wrong... need to fix.
		//					sa.LineItems.Clear();
		//					var soi = new SalesOrderItem
		//								{
		//								Warehouse = "",
		//								PartNumber = "",
		//								ProductDescription = this_wo.Description,
		//								Comment = this_wo.Description
		//								};
		//					sa.LineItems.Add(soi);
		//					sa.Save(DSN, current_user.Initials);
		//					sa.Load(DSN, BVWO);
		//					st = sa.LineItems;
		//					}
		//				while (st.Count > 1)
		//					{
		//					st.Remove(st.Count - 1);
		//					}
		//				if (myRegularTable.Rows.Count > 0)
		//					{
		//					PrepRows(myRegularTable, ref sa, DSN);
		//					}
		//				if (myInvisbleCreditTable.Rows.Count > 0)
		//					{
		//					InsertInventoryRow(ref sa, 0, "Invisible Credits", 0);
		//					PrepRows(myInvisbleCreditTable, ref sa, DSN);
		//					}
		//				sa.Discount = 0;
		//				sa.Save(DSN, strMemberName);
		//				myRegularTable.Dispose();
		//				}
		//			catch (Exception ex)
		//				{
		//				Toolbox.do_errorLog_errorStack(ex);
		//				throw;
		//				}
		//			finally
		//				{
		//				SalesOrder.UnlockOrder(DSN, BVWO, "DSM-Move");
		//				}
		//			}
		//private bool should_tax_simplifier(SalesOrder sa, bool billtype_override, string line_type, int current_tax)
		//	{
		//	var returned	= false;
		//	if(billtype_override)
		//		{
		//		switch(line_type)
		//			{
		//				case "L":
		//					returned = sa.ne_company.TaxLabour == 1 && current_tax > 0;
		//					break;
		//				case "M":
		//					returned = sa.ne_company.TaxMaterial == 1 && current_tax > 0;
		//					break;
		//				case "Q":
		//					returned = sa.ne_company.TaxQuotedJobs == 1 && current_tax > 0;
		//					break;
		//			}
		//		}
		//	return returned;
		//	}
		//public void PrepRows(DataTable dt, ref SalesOrder sa, string DSN)
		//	{
		//	var billtype_override	= true;
		//	var DescriptionHolder = "";
		//	double discountamount = 0;

		//	foreach (DataRow dr in dt.Rows)
		//		{
		//		var billtype_id		= Convert.ToInt32(dr["wo_detail_current_billtypeid"]);
		//		billtype_override	= !Toolbox.Contains(billtype_id, new int[]{1, 4, 2}); // If it contains these billtypes, it should override to false... true means apply tax as per normal

		//		var si	= new SalesOrderItem();
		//		var master_id		= Convert.ToInt32(dr["wo_detail_current_master_id"]);
		//		var line_type	= dr["wo_detail_current_type"].ToString();
		//		var tax1			= Convert.ToInt32(dr["wo_detail_current_tax1"]);
		//		var tax2			= Convert.ToInt32(dr["wo_detail_current_tax2"]);
		//		var tax3			= Convert.ToInt32(dr["wo_detail_current_tax3"]);
		//		var tax4			= Convert.ToInt32(dr["wo_detail_current_tax4"]);

		//		si.ApplyTax1		= should_tax_simplifier(sa, billtype_override, line_type, tax1);
		//		si.ApplyTax2		= should_tax_simplifier(sa, billtype_override, line_type, tax2);
		//		si.ApplyTax3		= should_tax_simplifier(sa, billtype_override, line_type, tax3);
		//		si.ApplyTax4		= should_tax_simplifier(sa, billtype_override, line_type, tax4);

		//		si.CostPrice		= Convert.ToDouble(dr["wo_detail_current_price_cost"]);
		//		si.OrderQuantity	= Convert.ToDouble(dr["wo_detail_current_qty_committed"]);
		//		si.PartNumber		= dr["wo_detail_current_code"].ToString();
		//		if (billtype_id == 7)
		//			{
		//			DescriptionHolder = "(No Charge)" + dr["descrip"];
		//			}
		//		else if (billtype_id == 10)
		//			{
		//			DescriptionHolder = "(Credit)" + dr["descrip"];
		//			}
		//		else
		//			{
		//			DescriptionHolder = dr["descrip"].ToString();
		//			}
		//		DescriptionHolder = DescriptionHolder.Replace("\"", "'").Replace("'", "''");
		//		si.ProductDescription = DescriptionHolder.Length > 80 ? DescriptionHolder.Substring(0, 79) : DescriptionHolder;
		//		if (billtype_id == 6)
		//			{
		//			si.Comment = DescriptionHolder;
		//			si.PartNumber = "";
		//			}
		//		if (billtype_id == 8)
		//			{
		//			si.ProductDescription = "";
		//			si.PartNumber = "";
		//			}
		//		discountamount = 0;
		//		try
		//			{
		//			discountamount = Convert.ToDouble(dr["wo_detail_current_discount"]);
		//			}
		//		catch(Exception ex)
		//			{
		//			Toolbox.do_errorLog_errorStack(ex);
		//			}
		//		if (discountamount != 0)
		//			{
		//			si.Discountable = true;
		//			si.Linedisc = Math.Round(discountamount, 1,MidpointRounding.AwayFromZero);
		//			_CustomerDiscount = 0;
		//			si.Linediscamt = Math.Round(Convert.ToDouble(dr["wo_detail_current_price_sell"]) * (discountamount / 100), 2, MidpointRounding.AwayFromZero) * Convert.ToDouble(dr["wo_detail_current_qty_committed"]);
		//			}
		//		else
		//			{
		//			si.Discountable = false;
		//			si.Linedisc = 0;
		//			si.Linediscamt = 0;
		//			}

		//		si.RetailPrice = Convert.ToDouble(dr["wo_detail_current_price_sell"])*(1 - discountamount/100);
		//		si.UnitPrice = si.RetailPrice;
		//		if (billtype_id == 7)
		//			{
		//			si.RetailPrice = 0;
		//			si.UnitPrice = 0;
		//			}
		//		si.UnitOfMeasure = "ea";
		//		si.Warehouse = "00";
		//		si.Status = 9;
		//		si.NonStocked = true;
		//		sa.LineItems.Add(si);
		//		}
		//	}
		//public void InsertInventoryRow(ref SalesOrder sa, double dblInvLine, string Description, int flag)
		//	{
		//	var tax3 = false;
		//	var tax4 = false;
		//	var Inv = new InventoryItem();
		//	Inv.UnitOfMeasure = "ea";
		//	Inv.Warehouse = "00";
		//	Inv.Tax1 = false;
		//	Inv.Tax2 = false;
		//	var NewSalesItem = new SalesOrderItem();
		//	NewSalesItem.ApplyTax1 = Inv.Tax1;
		//	NewSalesItem.ApplyTax2 = Inv.Tax2;
		//	NewSalesItem.ApplyTax3 = tax3;
		//	NewSalesItem.ApplyTax4 = tax4;
		//	NewSalesItem.CostPrice = 0;
		//	NewSalesItem.OrderQuantity = -1;
		//	if (flag == 1)
		//		{
		//		NewSalesItem.OrderQuantity = 1;
		//		}
		//	NewSalesItem.PartNumber = "INVENTORY";
		//	NewSalesItem.ProductDescription = Description;
		//	NewSalesItem.RetailPrice = dblInvLine;
		//	NewSalesItem.UnitPrice = dblInvLine;
		//	NewSalesItem.UnitOfMeasure = Inv.UnitOfMeasure;
		//	NewSalesItem.Warehouse = Inv.Warehouse;
		//	NewSalesItem.Status = 9;
		//	sa.LineItems.Add(NewSalesItem);
		//	}

		//public static Toolbox.boolstr create_bv_wo(bv_wo_packet _packet, ref int _error_level, ref SalesOrderItem _soi_quote)
		//	{
		//	var bs			= new Toolbox.boolstr();

		//	#region cut the BV work order

		//	try
		//		{

		//		#region BV Sales Order Header Items
		//		SalesOrderItem soi;
		//		var so = new SalesOrder();
		//		so.CustomerNumber = _packet.customer.Customer_Number;
		//		so.PONumber = _packet.customer_po;
		//		so.OrderDate = DateTime.Now;
		//		so.Status = "O";
		//		so.ReferenceNumber = "REF";
		//		so.TerritoryCode = _packet.division;
		//		so.TerritoryDescription = "";
		//		so.SalesPersonNumber = "/";
		//		so.SalesPersonName = "";
		//		so.Discount = 0;
		//		so.TermsCode = "/";
		//		so.TermsDescription = "";
		//		so.ShipToAddress = "";
		//		so.ShippingMethodCode = "/";
		//		so.ShippingMethod = "";
		//		so.PrintPackingSlip = false;
		//		so.PackingSlipDate = new DateTime();
		//		so.PackingSlipUser = _packet.user.Initials;
		//		so.PrintShippingLabel = true;
		//		so.ShippingLabelDate = new DateTime();
		//		so.ShippingLabelUser = "";
		//		try
		//			{
		//			if (_packet.quote_number == "0")
		//				{
		//				so.FOB = "FOB";
		//				}
		//			else
		//				{
		//				so.FOB = "Q-" + _packet.quote_number;
		//				}
		//			}
		//		catch (Exception ex)
		//			{
		//			Toolbox.do_errorLog_errorStack(ex);
		//			so.FOB = "FOB";
		//			}
		//		if (_packet.business_unit.is_er)
		//			{
		//			so.FOB = _packet.er_job_id;
		//			}


		//			so.SalesTax1 = _packet.address.Tax1 == _packet.business_unit.Tax1 ? _packet.address.Tax1_Consol : -1;
		//			so.SalesTax2 = _packet.address.Tax2 == _packet.business_unit.Tax2 ? _packet.address.Tax2_Consol : -1;
		//			so.SalesTax3 = _packet.address.Tax3 == _packet.business_unit.Tax3 ? _packet.address.Tax3_Consol : -1;
		//			so.SalesTax4 = _packet.address.Tax4 == _packet.business_unit.Tax4 ? _packet.address.Tax4_Consol : -1;

		//		so.Freight = 0;
		//		soi = new SalesOrderItem();
		//		soi.Warehouse = "";
		//		soi.PartNumber = "";
		//		soi.ProductDescription = _packet.description;
		//		soi.Comment = _packet.description;
		//		_error_level = 1;
		//		so.LineItems.Add(soi);
		//		_error_level = 2;
		//		if (_packet.is_progress)  //if it's a progress bill
		//			{
		//			double dbl_progress_price;
		//			var dbl_dispaly_hold = _packet.quote_percent * 100;  // this has been set to 0
		//			var str_display_perc = dbl_dispaly_hold.ToString();
		//			var inv = new InventoryItem();
		//			try
		//				{
		//				dbl_progress_price = Convert.ToDouble(_packet.progress_bill_wo.QuotedPrice);  // Get original job quoted price
		//				}
		//			catch (Exception ex)
		//				{
		//				Toolbox.do_errorLog_errorStack(ex);
		//				dbl_progress_price = 0.00;
		//				}
		//			try
		//				{
		//				inv.Load(_packet.dsn, "00", "QUOTE");  // not sure why this is needed 
		//				_error_level = 3;
		//				}
		//			catch (Exception ex)
		//				{
		//				Toolbox.do_errorLog_errorStack(ex);
		//				inv.Cost = 0;

		//					inv.Tax1 = _packet.customer.Address.Tax1_Consol != 0 && _packet.customer.Address.Tax1Exempt == "";
		//					inv.Tax2 = _packet.customer.Address.Tax2_Consol != 0 && _packet.customer.Address.Tax2Exempt == "";
		//					inv.Tax3 = _packet.customer.Address.Tax3_Consol != 0 && _packet.customer.Address.Tax3Exempt == "";
		//					inv.Tax4 = _packet.customer.Address.Tax4_Consol != 0 && _packet.customer.Address.Tax4Exempt == "";

		//				}
		//			_soi_quote.Warehouse = "00";
		//			_soi_quote.PartNumber = "QUOTE";

		//			// these descriptions should be used on the line itmes of the progress bill work order... not on the parent.
		//			if(_packet.progress_bill_wo.QuoteID == "") // Manual Credit/Down Payment/Progres billing? Probably shouldn't have a manual ProgBill... 
		//				{
		//				if (_packet.progress_bill_type == "Flat Amount")
		//					{
		//					_soi_quote.OrderQuantity = 1;
		//					_soi_quote.RetailPrice = _packet.quote_flat_amount;
		//					if (_packet.is_credit)
		//						{
		//						_soi_quote.ProductDescription = string.Format("Credit for {0:C2} for WO: {1}", _packet.quote_flat_amount, _packet.progress_bill_wo.OrderNumber);
		//						}
		//					else if (_packet.is_down_payment)
		//						{
		//						_soi_quote.ProductDescription = string.Format("Down Payment of {0:C2} for WO: {1}", _packet.quote_flat_amount, _packet.progress_bill_wo.OrderNumber);
		//						}
		//					else
		//						{
		//						_soi_quote.ProductDescription = string.Format("Progress Billing of {0:C2} of WO: {1}", _packet.quote_flat_amount, _packet.progress_bill_wo.OrderNumber);
		//						}
		//					}
		//				else
		//					{
		//					_soi_quote.OrderQuantity = _packet.quote_percent;
		//					_soi_quote.RetailPrice = dbl_progress_price;
		//					if (_packet.is_credit)
		//						{
		//						_soi_quote.ProductDescription = string.Format("Credit for {0:P2} ({1:C2}) of WO: {2}", _packet.quote_percent, _packet.quote_flat_amount, _packet.progress_bill_wo.OrderNumber);
		//						}
		//					else if (_packet.is_down_payment)
		//						{
		//						_soi_quote.ProductDescription = string.Format("Down Payment of {0:P2} ({1:C2}) of WO: {2}", _packet.quote_percent, _packet.quote_flat_amount, _packet.progress_bill_wo.OrderNumber);
		//						}
		//					else
		//						{
		//						_soi_quote.ProductDescription = string.Format("Progress Billing of {0:P2} ({1:C2}) of WO: {2}", _packet.quote_percent, _packet.quote_flat_amount, _packet.progress_bill_wo.OrderNumber);
		//						}
		//					}
		//				}
		//			else
		//				{
		//				var q = new quote().GetQuoteRev(Convert.ToInt32(_packet.progress_bill_wo.QuoteID));  // pull up the whole quote.
		//				_error_level = 4;
		//				if (q.Length > 6)
		//					{
		//					q = q.Substring(0, 6) + "-V" + q.Substring(6, 1);
		//					}
		//				if (_packet.progress_bill_type == "Flat Amount")
		//					{
		//					_soi_quote.OrderQuantity = 1;
		//					_soi_quote.RetailPrice = _packet.quote_flat_amount;
		//					if (_packet.is_credit)
		//						{
		//						_soi_quote.ProductDescription = string.Format("Credit for {0:C2} for Quote Q-{1} WO: {2}", _packet.quote_flat_amount, q, _packet.progress_bill_wo.OrderNumber);
		//						}
		//					else if (_packet.is_down_payment)
		//						{
		//						_soi_quote.ProductDescription = string.Format("Down Payment of {0:C2} for Quote Q-{1} WO: {2}", _packet.quote_flat_amount, q, _packet.progress_bill_wo.OrderNumber);
		//						}
		//					else
		//						{
		//						_soi_quote.ProductDescription = string.Format("Progress Billing of {0:C2} of Quote Q-{1} WO: {2}", _packet.quote_flat_amount, q, _packet.progress_bill_wo.OrderNumber);
		//						}
		//					}
		//				else
		//					{
		//					_soi_quote.OrderQuantity = _packet.quote_percent;
		//					_soi_quote.RetailPrice = dbl_progress_price;
		//					if (_packet.is_credit)
		//						{
		//						_soi_quote.ProductDescription = string.Format("Credit for {0:P2} ({1:C2}) of Quote Q-{2} WO: {3}", _packet.quote_percent, _packet.quote_flat_amount, q, _packet.progress_bill_wo.OrderNumber);
		//						}
		//					else if (_packet.is_down_payment)
		//						{
		//						_soi_quote.ProductDescription = string.Format("Down Payment of {0:P2} ({1:C2}) of Quote Q-{2} WO: {3}", _packet.quote_percent, _packet.quote_flat_amount, q, _packet.progress_bill_wo.OrderNumber);
		//						}
		//					else
		//						{
		//						_soi_quote.ProductDescription = string.Format("Progress Billing of {0:P2} ({1:C2}) of Quote Q-{2} WO: {3}", _packet.quote_percent, _packet.quote_flat_amount, q, _packet.progress_bill_wo.OrderNumber);
		//						}
		//					}
		//				}
		//			_soi_quote.UnitPrice = _soi_quote.RetailPrice;    // price of original quote
		//			if (_packet.is_credit)
		//				{
		//				_soi_quote.OrderQuantity = _soi_quote.OrderQuantity * -1;   // if its a credit, make it a negative
		//				}
		//			_soi_quote.CostPrice = inv.Cost;
		//			_soi_quote.ApplyTax1 = inv.Tax1;
		//			_soi_quote.ApplyTax2 = inv.Tax2;
		//			_soi_quote.ApplyTax3 = inv.Tax3;
		//			_soi_quote.ApplyTax4 = inv.Tax4;
		//			so.LineItems.Add(_soi_quote); // add the item to the bv work order
		//			_error_level = 5;
		//			// start to populate a line item of the original work order !!!!!!!
		//			}
		//		so.OrderNumber = _packet.order_number;
		//		so.Add_New(_packet.dsn, _packet.user.Initials);  // save the bv work order and grab the bvwo number.
		//		_error_level = 6;
		//		#endregion BV Sales Order Header Items

		//		if (_packet.is_progress)
		//			{
		//			var int_invoicewo = Convert.ToInt32(_packet.order_number);
		//			SalesOrder.UnlockOrder(_packet.dsn, _packet.progress_bill_wo.OrderNumber, "DSM"); // unlock the new work order?  wierd.  it wasn't even locked to begin with.
		//			}
		//		_error_level = 7;
		//		bs.success	= true;
		//		bs.message	= "";
		//		}
		//	catch (Exception ex3)
		//		{
		//		Toolbox.do_catch_error(ex3, 711);
		//		bs.success = false;
		//		bs.message = "Error Cutting WO in BV : " + _packet.business_unit.name + "-" + _packet.mysql_wo.woprog_id + ": Error_level: " + _error_level + " from " + _packet.dsn + ": " + ex3.Message + " - " + ex3.InnerException + "<br />";
		//		if (_packet.is_progress)
		//			{
		//			SalesOrder.UnlockOrder(_packet.dsn, _packet.progress_bill_wo.OrderNumber, "WO2");
		//			}
		//		}
		//	return bs;
		//	#endregion
		//	}
		//public struct bv_wo_packet
		//	{
		//	public NeMember user { get; set; }
		//	public NeBusinessUnit business_unit { get; set; }
		//	public string division { get; set; }
		//	public NECustomer customer { get; set; }
		//	public NeWOProg mysql_wo { get; set; }
		//	public NeWOProg progress_bill_wo { get; set; }
		//	public NEAddress address { get; set; }
		//	public string dsn {get; set;}
		//	public string customer_po { get; set; }
		//	public bool is_credit { get; set; }
		//	public bool is_rebill { get; set; }
		//	public bool is_progress { get; set; }
		//	public bool is_down_payment { get; set; }
		//	public string description { get; set; }
		//	public string quote_number { get; set; }
		//	public double quote_amount { get; set; }
		//	public double quote_flat_amount { get; set; }
		//	public double quote_percent { get; set; }
		//	public string er_job_id { get; set; }
		//	public string progress_bill_type { get; set; }
		//	public string order_number { get; set; }
		//	}
		#endregion methods
	}
}
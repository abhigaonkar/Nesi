using System;
using System.Data;
using System.Collections;

namespace nesi.core
	{
	/// <summary>
	/// Summary description for NeExWorkOrder
	/// </summary>
	public class NeExWorkOrder
		{
		#region BVWorkorder

		private string _ORDD_Description = "";
		private string _DSN = "";


		public string ErrorFlag { get; set; }

		public string ErrorList { get; set; }

		public string DSN
			{
			get { return _DSN; }
			set { _DSN = value; }

			}

		public string Number { get; set; }

		public string ORDDDescription
			{
			get { return _ORDD_Description; }
			set { _ORDD_Description = value; }
			}

		//Get all Open Work Orders for a Company


		//public ArrayList LoadCustWODetailLIST(string selcustno, string memcode)

		/*
    public ArrayList LoadCust1WOLIST(string selcustno)
    {

        OleDbConnection conn = NeDB.getPSQL1Con();
        //string strCustWO = "SELECT NUMBER, ORDD_Description FROM SALES_ORDER_DETAIL WHERE RECNO = '1' AND NUMBER IN (SELECT NUMBER FROM SALES_ORDER_HEADER WHERE CUST_NO = '" + selcustno.ToString().Trim() + "')";
        
        OleDbCommand comCustWOList = new OleDbCommand(strCustWO, conn);
        OleDbDataReader drCustWOList = comCustWOList.ExecuteReader();

        ArrayList list = new ArrayList();

        NeExWorkOrder item;

        while (drCustWOList.Read())
        {
            item = new NeExWorkOrder();

            item._Number = drCustWOList.GetValue(0).ToString();
            item._ORDD_Description = drCustWOList.GetValue(0).ToString() + " - " + drCustWOList.GetValue(2).ToString();

            list.Add(item);
        }
        conn.Close();
        return list;
       
    } */

		#endregion


		private string[] s			= new string[] 
		                  				{
		                  				"'Invoiced'",					// 0
		                  				"'Waiting BM Approval'",		// 1
		                  				"'Waiting To Be Invoiced'",		// 2
		                  				"'Waiting PM Approval'",		// 3
		                  				"'Questions For PM'",			// 4
		                  				"'Waiting For PO'",				// 5
		                  				"'Waiting Approval'",			// 6
		                  				"'Hold'",						// 7
		                  				"'Initial Prep'",				// 8
		                  				"'Rework'",						// 9
		                  				"'In Progress'",                // 10
		                  				"'Open Vendor POs'",			// 11
		                  				"'Waiting for Parts'"			// 12
		                  				};
		public ArrayList LoadCustWOPROGLIST(string customer_n, string business_unit_id, int memberid)
			{
			var _tools					= new Toolbox();
			var myMember				= new NeMember(memberid);
			var open_list				= myMember.AuthenticatedForPrivilege(15) || myMember.AuthenticatedForPrivilege(16) ? string.Format("{0},{1},{2},{3},{4}", s[0], s[1], s[2], s[7], s[5]) : "";
			var used_list				= string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10}", s[0],s[1],s[2],s[3],s[4],s[5],s[6],s[7],s[8],s[9],s[10]);
			if(open_list != "")
				{
				used_list					= open_list;
				}
			customer_n						= customer_n == "0" ? "b.Customer_Number" : customer_n;
			var strCustWO				= string.Format(@"
SELECT 
	a.woprog_description, 
	a.woprog_bvwo, 
	urldecode(a.woprog_customername) woprog_customername, 
	a.woprog_status 
FROM woprog a
LEFT JOIN
	customer b ON a.woprog_customer_id = b.customer_id
WHERE 
	b.customer_number = {0} AND 
	a.business_unit_id = {1} AND 
	a.woprog_status NOT IN ({2}) AND 
	a.woprog_associate_woprog_id = 0 AND 
	a.woprog_bvwo != 'Not Entered' AND 
	a.WOProg_Hold = 0 AND 
	b.customer_qc_member_id IS NOT NULL
ORDER BY 
	a.woprog_customername,
	a.woprog_bvwo
	",
				customer_n,         // {0}
				business_unit_id,			// {1}
				used_list			// {2}
			);
			var CustWoList				= new DataSet();
			var tablelist				= Toolbox.doSQL_dt(strCustWO  , null);
			ErrorList						= "";
			ErrorFlag						= "0";
			var list					= new ArrayList();
        
			try
				{
				NeExWorkOrder item;
				string description;
				string custname;

				foreach (DataRow drCustWO in tablelist.Rows)
					{
					item					= new NeExWorkOrder();
					item.Number			= drCustWO[1].ToString();
					description				= drCustWO[0].ToString().TrimEnd();
					if (description.Length > 20)
						{
						description			= description.Substring(0, 20);
						}
					custname = drCustWO[2].ToString().TrimEnd();
					if (custname.Length > 30)
						{
						custname			= custname.Substring(0, 30);
						}
					item._ORDD_Description	= string.Format("{0} - {1} - {2} - {3}", drCustWO[1], custname, description, drCustWO[3]);
					list.Add(item);
					}
				}
			catch { }
        
			return list;
			}


		public ArrayList LoadCustWOPROGWIPLIST(int in_customer_id, string business_unit_id, int memberid)
			{
			var _tools					= new Toolbox();
			var myMember				= new NeMember(memberid);
			var open_list				= myMember.AuthenticatedForPrivilege(15) || myMember.AuthenticatedForPrivilege(16) ? string.Format("{0},{1},{2},{3},{4}", s[0], s[1], s[2], s[7], s[4]) : "";
			var process_list             = myMember.AuthenticatedForPrivilege(91) ? string.Format("{0},{1},{2},{3},{4},{5},{6}", s[0], s[1], s[2], s[4], s[5], s[6], s[7]) : "";
			var used_list				= string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10}", s[0],s[1],s[2],s[3],s[4],s[5],s[6],s[7],s[8],s[9],s[10]);
			var customer_id = in_customer_id == 0 ? "b.customer_number_int" : in_customer_id.ToString();

            if (open_list != "")
				{
				used_list					= open_list;
				}
			if (process_list != "")
				{
				used_list = process_list;
				//Process List will superscede approved PM approve BM when present
				}
			var strCustWO				= string.Format(@"
SELECT 
	a.woprog_description descript, 
	a.woprog_id,
	IF(a.woprog_id < 1000000, a.woprog_bvwo, LPAD(a.woprog_id,10, '0')) wo, 
	a.woprog_customername customername, 
	a.woprog_status status
FROM 
	woprog a
LEFT JOIN
	customer b ON a.woprog_customer_id = b.customer_id
LEFT JOIN
	woprog c ON a.parent_woprog_id = c.woprog_id
LEFT JOIN 
	customer d ON c.woprog_customer_id = d.customer_id
WHERE 
	b.customer_number_int = {0} AND 
	a.business_unit_id = {1} AND 
	a.woprog_status NOT IN ({2}) AND
	a.woprog_status != 'Deleted' AND
	a.woprog_associate_woprog_id = 0 AND
	a.woprog_bvwo != 'Not Entered' AND 
	b.customer_hold = 'F' AND 
	a.woprog_hold = 0 AND 
	b.customer_qc_member_id IS NOT NULL AND
	a.woprog_isrebill = 0 AND
	a.woprog_iscredit = 0 AND
	IF(a.parent_woprog_id > 1,
		(
		c.woprog_status NOT IN ({2}) AND
		c.woprog_status != 'Deleted' AND
		c.woprog_associate_woprog_id = 0 AND
		c.woprog_bvwo != 'Not Entered' AND 
		d.customer_hold = 'F' AND 
		c.woprog_hold = 0 AND 
		d.customer_qc_member_id IS NOT NULL AND
		c.woprog_isrebill = 0	
		), true)
ORDER BY 
	a.woprog_bvwo
	", 
				customer_id,         // {0}
				business_unit_id,			// {1}
				used_list			// {2}
			);
			var tablelist				= Toolbox.doSQL_dt(strCustWO  , null);
			ErrorList						= "";
			ErrorFlag						= "0";
			var list = new ArrayList();
			    try
			        {
			        foreach (DataRow drCustWO in tablelist.Rows)
			            {
			            var item = new NeWOProg();
			            item.woprog_id = Convert.ToInt32(drCustWO["woprog_id"]);
			            var description = drCustWO["descript"].ToString().TrimEnd();
			            var custname = drCustWO["customername"].ToString().TrimEnd();
			            if (custname.Length > 30)
			                {
			                custname = custname.Substring(0, 30);
			                }
			            item.Description = string.Format("{0} - {1} - {2} - {3}", drCustWO["wo"], custname, description,
			                drCustWO["status"]);
			            list.Add(item);
			            }
			        }
			    catch (Exception ee)
			        {
			        Toolbox.do_errorLog_errorStack(ee);
			        }

			return list;
			}

		}
	}
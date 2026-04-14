using System;
using System.Data;
using System.Text;
using System.Collections;
using DevExpress.Xpo;
using System.IO;
using System.Net.Mail;
using System.Xml;
using System.Web;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using NESI.Common.Models;

// ReSharper disable InconsistentNaming

// ReSharper disable once CheckNamespace
namespace nesi.core
	{
	/// <summary>
	/// Summary description for NePOProg
	/// </summary>
	public class NePOProg
		{
		public DateTime poprog_last_modified        { get; set; }
		public DateTime poprog_ts                   { get; set; }
		public bool nesi_cut_po                     { get; set; }
		public double poprog_total_cost             { get; set; }
		public double poprog_total_recCost          { get; set; }
		public int business_unit_id                 { get; set; }
		public int cc_lastfour_id                   { get; set; }
		public int edit_after_issue                 { get; set; }
		public int location_master_id               { get; set; }
		public int notes_id                         { get; set; }
		public int notes_memberid                   { get; set; }
		public int notes_poprogid                   { get; set; }
		public int poprog_ack_req                   { get; set; }
		public int poprog_ack_req_rec               { get; set; }
		public int poprog_bpm_member_id             { get; set; }
		public int poprog_contact_id                { get; set; }
		public int poprog_customer_id               { get; set; }
		public int poprog_cutby_member_id           { get; set; }
		public int poprog_id                        { get; set; }
		public int poprog_poprog_payment_method_id  { get; set; }
		public int poprog_ship_note_req             { get; set; }
		public int poprog_ship_note_req_rec         { get; set; }
		public int poprog_shipping_address_id       { get; set; }
		public int poprog_shipping_method           { get; set; }
		public int poprog_status                    { get; set; }
		public int poprog_vendor_id                 { get; set; }
		public int poprog_woprog_id                 { get; set; }
		public string eventtext                     { get; set; }
		public string notes_date                    { get; set; }
		public string poprog_bvpo                   { get; set; }
		public string poprog_check_sent_date        { get; set; }
		public string poprog_country_code_currency  { get; set; } 
		public string poprog_cutdate                { get; set; }
		public string poprog_date_required          { get; set; }
		public string poprog_expected_order_date    { get; set; }
		public string poprog_expected_received_date { get; set; }
		public string poprog_invoice_number         { get; set; } 
		public string poprog_invoice_slip_scan_date { get; set; }
		public string poprog_manual_shipaddress     { get; set; }
		public string poprog_order_description      { get; set; }
		public string poprog_order_placed_date      { get; set; }
		public string poprog_packing_slip_number    { get; set;}
		public string poprog_packing_slip_scan_date { get; set; }
		public string poprog_received_date          { get; set; }

        public string Shipping_Addr1 { get; set; }
        public string Shipping_Addr2 { get; set; }
        public string Shipping_City { get; set; }
        public string Shipping_Province { get; set; }
        public string Shipping_Postal { get; set; }
        public string Shipping_Country { get; set; }

        public NePOProg()
			{
			//
			//  
			//
			}
		public NePOProg(int poid)
			{
			var potable = Toolbox.doSQL_dt(@"SELECT * FROM poprog_header  WHERE poprog_id =@v0", new object[] { poid });
			foreach (DataRow porow in potable.Rows)
				{
				#region Int32
				poprog_id = Convert.ToInt32(porow["poprog_id"]);
				business_unit_id = (int)porow["business_unit_id"];
				poprog_shipping_address_id = Convert.ToInt32(porow["poprog_shipping_address_id"]);
				poprog_woprog_id = Convert.ToInt32(porow["poprog_woprog_id"]);
				poprog_customer_id = Convert.ToInt32(porow["poprog_customer_id"]);
				poprog_bpm_member_id = Convert.ToInt32(porow["poprog_bpm_member_id"]);
				poprog_cutby_member_id = Convert.ToInt32(porow["poprog_cutby_member_id"]);
				poprog_vendor_id = Convert.ToInt32(porow["poprog_vendor_id"]);
				poprog_contact_id = Convert.ToInt32(porow["poprog_contact_id"]);
				nesi_cut_po = Convert.ToInt32(porow["nesi_cut_po"]) == 1;
				poprog_status = Convert.ToInt32(porow["poprog_status"]);
				edit_after_issue = Convert.ToInt32(porow["edit_after_issue"]);
				poprog_poprog_payment_method_id = Convert.ToInt32(porow["poprog_poprog_payment_method_id"]);
				poprog_ack_req = Convert.ToInt32(porow["poprog_ack_req"]);
				poprog_ack_req_rec = Convert.ToInt32(porow["poprog_ack_req_rec"]);
				poprog_ship_note_req = Convert.ToInt32(porow["poprog_ship_note_req"]);
				poprog_ship_note_req_rec = Convert.ToInt32(porow["poprog_ship_note_req_rec"]);
				poprog_shipping_method = Convert.ToInt32(porow["poprog_shipping_method"]);
				location_master_id = porow["location_master_id"] != DBNull.Value ? Convert.ToInt32(porow["location_master_id"]) : 0;
				cc_lastfour_id = porow["cc_lastfour_id"] != DBNull.Value ? Convert.ToInt32(porow["cc_lastfour_id"]) : 0;
				#endregion Int32
				#region Double
				poprog_total_cost = Convert.ToDouble(porow["poprog_total_cost"]);
				poprog_total_recCost = Convert.ToDouble(porow["poprog_total_recCost"]);
				#endregion Double
				#region DateTime -> String
				poprog_cutdate = porow["poprog_cutdate"] == DBNull.Value ? "" : Convert.ToDateTime(porow["poprog_cutdate"]).ToString("yyyy-MM-dd");
				poprog_packing_slip_scan_date = porow["poprog_packing_slip_scan_date"] == DBNull.Value ? "" : Convert.ToDateTime(porow["poprog_packing_slip_scan_date"]).ToString("yyyy-MM-dd");
				poprog_invoice_slip_scan_date = porow["poprog_invoice_slip_scan_date"] == DBNull.Value ? "" : Convert.ToDateTime(porow["poprog_invoice_slip_scan_date"]).ToString("yyyy-MM-dd");
				poprog_received_date = porow["poprog_received_date"] == DBNull.Value ? "" : Convert.ToDateTime(porow["poprog_received_date"]).ToString("yyyy-MM-dd");
				poprog_check_sent_date = porow["poprog_check_sent_date"] == DBNull.Value ? "" : Convert.ToDateTime(porow["poprog_check_sent_date"]).ToString("yyyy-MM-dd");
				poprog_order_placed_date = porow["poprog_order_placed_date"] == DBNull.Value ? "" : Convert.ToDateTime(porow["poprog_order_placed_date"]).ToString("yyyy-MM-dd");
				poprog_expected_order_date = porow["poprog_expected_order_date"] == DBNull.Value ? "" : Convert.ToDateTime(porow["poprog_expected_order_date"]).ToString("yyyy-MM-dd");
				poprog_expected_received_date = porow["poprog_expected_received_date"] == DBNull.Value ? "" : Convert.ToDateTime(porow["poprog_expected_received_date"]).ToString("yyyy-MM-dd");
				poprog_date_required = porow["poprog_date_required"] == DBNull.Value ? "" : Convert.ToDateTime(porow["poprog_date_required"]).ToString("yyyy-MM-dd");
				poprog_ts = Toolbox.ReturnBlankDateTimeIfNull(porow["poprog_ts"]);
				poprog_last_modified = Toolbox.ReturnBlankDateTimeIfNull(porow["poprog_last_modified"]);

				#endregion DateTime -> String
				#region String
				poprog_order_description = porow["poprog_order_description"].ToString();
				poprog_bvpo = porow["poprog_bvpo"].ToString();
				poprog_packing_slip_number = porow["poprog_packing_slip_number"].ToString();
				poprog_invoice_number = porow["poprog_invoice_number"].ToString();
				poprog_manual_shipaddress = porow["poprog_manual_shipaddress"].ToString();
				poprog_country_code_currency = porow["poprog_country_code_currency"].ToString();
                Shipping_Addr1 = porow["Shipping_Addr1"].ToString();
                Shipping_Addr2 = porow["Shipping_Addr2"].ToString();
                Shipping_City = porow["Shipping_City"].ToString();
                Shipping_Province = porow["Shipping_Province"].ToString();
                Shipping_Postal = porow["Shipping_Postal"].ToString();
                Shipping_Country = porow["Shipping_Country"].ToString();
                #endregion String
            }
			eventtext = GetNotesData(poid.ToString());
			}
		public string GetNotesData(string poid)
			{
			var notes = new StringBuilder();
			var Notes = Toolbox.doSQL_dt(@"SELECT a.eventtext, a.date, b.member_fullname FROM poprog_notes AS a, member AS b  where a.poprog_id =@v0 AND a.member_id = b.member_id order by a.date desc", new object[] { poid });
			foreach (DataRow row in Notes.Rows)
				{
				var eventtext = Toolbox.do_value_from(row["eventtext"], false);
				var date = row["date"];
				var name = row["member_fullname"];
				notes.AppendFormat("--\n{0} @ [{1}]\n{2}\n\n", name, date, eventtext);
				}
			return notes.ToString();
			}
		public static bool HasPermissionToApprove(int bu, double amount, int currentMemberID)
		{
			// Does the current person have approval limit for this branch?
		var canUserApprove = Toolbox.doSQL_int(@"SELECT COUNT(*) FROM business_unit_po_dist 
											WHERE business_unit_id = @v0 AND member_id = @v1 AND amount_to >= @v2 ", new object[] { bu, currentMemberID, amount}) > 0;
		
		
		if(canUserApprove)
			{
				return true;
			}
		else 
			{ 
			// Does the current user has anyone reporting to them who has the approval limit in this business unit
			var allPersonsReportToCurrentUser = Toolbox.doSQL_int(@"SELECT COUNT(*) FROM business_unit_po_dist a
														 LEFT JOIN member b ON a.member_id = b.Member_ID
														 WHERE b.reports_to = @v0 AND a.business_unit_id = @v1 AND amount_to >= @v2 ", 
										 new object[] { currentMemberID, bu, amount });
			if (allPersonsReportToCurrentUser>0)
			{
			return true;
			}
			}
		return false;
		}
        public static string GetShippingAddress(int poid)
        {
            string ShippingAddress = "";
            try
            {
                ShippingAddress=Toolbox.doSQL_string(@"select CONCAT(Shipping_Addr1,'<br />',IF(Shipping_Addr2 IS NULL,' ',Shipping_Addr2),' <br />',Shipping_City,' <br />',Shipping_Province,'<br />',Shipping_Country,' ',Shipping_Postal) as poprog_manual_shipaddress1 from poprog_header where poprog_id=@v0", new object[] { poid });

            }
            catch(Exception ex)
            {

            }
            return ShippingAddress;

        }

		/// <summary>
		/// Gets all the members that need to be notified of PO header status changes
		/// </summary>
		public static void send_email_to_trackers(string poprogid, string message, NeMember myMember, string body)
			{
			var _tools = new Toolbox();
			if (poprogid != "")
				{
				var trackers = Toolbox.doSQL_dt(@"SELECT distinct woprog.woprog_pm_memberid 
FROM wo_detail_current 
INNER JOIN woprog ON wo_detail_current.wo_detail_current_woprog_id = woprog.WOProg_ID 
INNER JOIN po_details_current ON woprog.WOProg_ID = po_details_current.po_details_woprog_id 
AND wo_detail_current.wo_detail_current_master_id = po_details_current.po_details_part_no  
where po_details_current.is_gl_account=false and wo_detail_current_track_part = 1 and po_details_current.po_details_poprog_id =@v0", new object[] { poprogid });
				if (trackers.Rows.Count > 0)
					{
					var po = new NePOProg(Convert.ToInt32(poprogid));
					var vendor = new NEVendor(Convert.ToInt32(po.poprog_vendor_id));
					var _lineitems = "<tr><th style='text-align:left'>Part No</th><th style='text-align:left'>Description</th><th>Qty Ordered</th><th>Qty Received</th></tr>";

					var dt_porows = get_po_line_items(po.poprog_id);
					foreach (NEPO_Details_Current detail in dt_porows)
						{
						_lineitems += string.Format("<tr><td style='text-align:left'>{0}</td><td style='text-align:left'>{1}</td><td>{2}</td><td>{3}</td></tr>", detail.po_details_part_no, detail.po_details_description, detail.po_details_qty_orderd, detail.po_details_qty_received);
						}

					foreach (DataRow tracker in trackers.Rows)
						{
						var m = new NeMember(Convert.ToInt32(tracker[0]));
						if (m.NEEmail != "")
							{
							var email = new NeEMail();
							email.To = m.NEEmail;
							email.From = myMember.NEEmail != "" ? myMember.NEEmail : "admin@" + Toolbox.app_setting("DomainForEmail");
							email.isHTML = true;
							email.Subject = message;
							email.Body = body != "" ? body : string.Format(@"<html><font-face='arial'><span style='font-family:arial'>
<table style='font-size:12px; font-family: Arial; border-collapse: collapse;' cellpadding='3'>
<tr><td nowrap='nowrap'><b>Purchase Order:</b></td><td width='100%'>{0}</td></tr>
<tr><td nowrap='nowrap'><b>Vendor:</b></td><td width='100%'>{1}</td></tr>
<tr><td nowrap='nowrap'><b>Expected Delivery:</b></td><td width='100%'>{2}</td></tr>
<tr><td nowrap='nowrap'><b>Vendor Contact:</b></td><td width='100%'>{3}</td></tr>
<tr><td nowrap='nowrap'><b>Vendor Phone Number:</b></td><td width='100%'>{4}</td></tr>
<tr><td nowrap='nowrap'></td><td></td width='100%'></tr>
<tr>
<td colspan='2'>
<table style='font-size:12px; text-align: center; font-family: Arial; border-collapse: collapse;' cellpadding='5'>
{5}
</table>
</td>
</tr></table></font-face></span></html>", po.poprog_bvpo.TrimStart('0'), vendor.Vendor_Name, po.poprog_expected_received_date, new NEContact(po.poprog_contact_id).name, vendor.Address.PhoneNumber, _lineitems);
							email.Send();
							}
						}
					}

				}
			}

		public static ArrayList get_po_line_items(int poid)
			{
			var details = new ArrayList();

			var dt = Toolbox.doSQL_dt(@"Select po_details_current.po_details_id from po_details_current  where po_details_current.po_details_poprog_id =@v0", new object[] { poid });
			foreach (DataRow dr in dt.Rows)
				{
				var detail = new NEPO_Details_Current();
				detail.po_details_current_line(Convert.ToInt32(dr[0]));
				details.Add(detail);
				}
			return details;
			}
		public static bool POExists(int poId)
			{
			return Toolbox.doSQL_int(@"SELECT COUNT(*) FROM poprog_header  WHERE poprog_id =@v0", new object[] { poId }) == 1;
			}
		public static int LineCount(int poId, bool includeCommentLines = true)
			{
			return includeCommentLines
						? Toolbox.doSQL_int(@"SELECT COUNT(*) FROM po_details_current WHERE po_details_poprog_id = @v0 ", new object[] { poId })
						: Toolbox.doSQL_int(@"SELECT COUNT(*) FROM po_details_current WHERE po_details_poprog_id = @v0 AND po_details_part_no != 0", new object[] { poId });
			}
		public static int LineCountActive(int poId)
			{
			return Toolbox.doSQL_int(@"SELECT COUNT(*) FROM po_details_current WHERE po_details_poprog_id = @v0 AND po_details_line_active = 1", new object[] { poId });
			}
		public static void AddNote(int poId, int memberId, string note)
			{
			Toolbox.doSQL_void(@"INSERT INTO poprog_notes (poprog_id, date, member_id, eventtext)  VALUES(@v0, NOW(), @v1, @v2)", new object[] { poId, memberId, note });
			}
		public static void UpdateStatus(int poId, int statusId, bool updateTimeStamp = true)
			{
			Toolbox.doSQL_void(updateTimeStamp
					? @"UPDATE poprog_header SET poprog_status = @v1, poprog_ts=NOW(), poprog_last_modified = NOW() WHERE poprog_id = @v0"
					: @"UPDATE poprog_header SET poprog_status = @v1, poprog_ts=poprog_ts WHERE poprog_id = @v0",
				new object[] {poId, statusId});
			}
		public static void Update_Received_Date(int poId)
		{
			Toolbox.doSQL_void(@"UPDATE poprog_header SET poprog_received_date = NOW() WHERE poprog_id = @v0", new object[] {poId} );
		}
		/// <summary>
		/// Sets poprog_id with the inserted ID
		/// </summary>
		public void POProg_Save()
			{
			try
				{
				poprog_cutdate = string.IsNullOrEmpty(poprog_cutdate) ? "NULL" : poprog_cutdate;
				poprog_packing_slip_scan_date = string.IsNullOrEmpty(poprog_packing_slip_scan_date) ? "NULL" : poprog_packing_slip_scan_date;
				poprog_invoice_slip_scan_date = string.IsNullOrEmpty(poprog_invoice_slip_scan_date) ? "NULL" : poprog_invoice_slip_scan_date;
				poprog_received_date = string.IsNullOrEmpty(poprog_received_date) ? "NULL" : poprog_received_date;
				poprog_check_sent_date = string.IsNullOrEmpty(poprog_check_sent_date) ? "NULL" : poprog_check_sent_date;
				poprog_order_placed_date = string.IsNullOrEmpty(poprog_order_placed_date) ? "NULL" : poprog_order_placed_date;
				poprog_expected_order_date = string.IsNullOrEmpty(poprog_expected_order_date) ? "NULL" : poprog_expected_order_date;
				poprog_expected_received_date = string.IsNullOrEmpty(poprog_expected_received_date) ? "NULL" : poprog_expected_received_date;
				poprog_date_required = string.IsNullOrEmpty(poprog_date_required) ? "NULL" : poprog_date_required;


				poprog_id = Toolbox.doSQL_return_id(@"
INSERT INTO poprog_header 
	(
	business_unit_id,
	poprog_shipping_address_id,
	poprog_bvpo, 
	poprog_woprog_id,
	poprog_customer_id,
	poprog_bpm_member_id,
	poprog_cutby_member_id,
	poprog_vendor_id,
	poprog_contact_id,

	poprog_cutdate,
	poprog_packing_slip_scan_date,
	poprog_invoice_slip_scan_date,
	poprog_received_date,
	poprog_check_sent_date,
	poprog_order_placed_date,
	poprog_expected_order_date,
	poprog_expected_received_date,
	poprog_date_required,

	poprog_order_description,
	poprog_packing_slip_number,
	poprog_invoice_number,
	poprog_status,
	poprog_total_cost,
	poprog_total_recCost,
	poprog_manual_shipaddress,
	poprog_shipping_method,
	poprog_country_code_currency, 
	edit_after_issue, 
	poprog_poprog_payment_method_id, 
	poprog_ack_req, 
	poprog_ack_req_rec, 
	poprog_ship_note_req, 
	poprog_ship_note_req_rec,
	location_master_id,
	nesi_cut_po,
	cc_lastfour_id,
	poprog_last_modified,
    Shipping_Addr1,
    Shipping_Addr2,
    Shipping_City,
    Shipping_Province,
    Shipping_Postal,
    Shipping_Country
    
	)
VALUES (@v0,@v1,@v2,@v3,@v4,@v5,@v6,@v7,@v8,@v9,@v10,@v11,@v12,@v13,@v14,@v15,@v16,@v17,@v18,@v19,@v20,@v21,@v22,@v23,@v24,@v25,@v26,@v27,@v28,@v29,@v30,@v31,@v32,@v33,@v34,@v35,@v36,@v37,@v38,@v39,@v40,@v41,@v42)",
new object[] {
					business_unit_id,								// {0}
					poprog_shipping_address_id,						// {1}
					poprog_bvpo,									// {2}
					poprog_woprog_id,								// {3}
					poprog_customer_id,								// {4}
					poprog_bpm_member_id,							// {5}
					poprog_cutby_member_id,							// {6}
					poprog_vendor_id,								// {7}
					poprog_contact_id,								// {8}

					poprog_cutdate,									// {9}
					poprog_packing_slip_scan_date,					// {10}
					poprog_invoice_slip_scan_date,					// {11}
					poprog_received_date,							// {12}
					poprog_check_sent_date,							// {13}
					poprog_order_placed_date,						// {14}
					poprog_expected_order_date,						// {15}
					poprog_expected_received_date,					// {16}
					poprog_date_required,							// {17}

					poprog_order_description,	// {18}
					poprog_packing_slip_number,						// {19}
					poprog_invoice_number,							// {20}
					poprog_status,									// {21}
					poprog_total_cost,								// {22}
					poprog_total_recCost,							// {23}
					poprog_manual_shipaddress,						// {24}
					poprog_shipping_method,	// {25}
					poprog_country_code_currency,					// {26}
					edit_after_issue,								// {27}
					poprog_poprog_payment_method_id,				// {28}
					poprog_ack_req,									// {29}
					poprog_ack_req_rec,								// {30}
					poprog_ship_note_req,							// {31}
					poprog_ship_note_req_rec,						// {32}
					(location_master_id == 0 ? "NULL" : location_master_id.ToString()),			// {33}
					nesi_cut_po,									// {34}
					cc_lastfour_id,									// {35}
					poprog_last_modified == null ? DateTime.Today.ToString("yyyy-MM-dd HH:mm:ss") : poprog_last_modified.ToString("yyyy-MM-dd HH:mm:ss") ,// {36}
                    Shipping_Addr1,
                    Shipping_Addr2,
                    Shipping_City,
                    Shipping_Province,
                    Shipping_Postal,
                    Shipping_Country

                });
				}
			catch (Exception POProgSaveEx)
				{
				throw new Exception(POProgSaveEx.ToString());
				}

			try
				{
				notes_poprogid = poprog_id;
				eventtext = "Purchase Order " + poprog_bvpo + " was created.";
				notes_memberid = poprog_cutby_member_id;
				SaveNotes();
				}
			catch (Exception ex)
				{
				throw new Exception(ex.ToString());
				}
			try
				{
				var p = new status_log {poprog_id = poprog_id, member_id = poprog_cutby_member_id, status_id = 1};
				p.status = status.by_id(p.status_id);
				p.dt = DateTime.Now;
				p.save();
				}
			catch (Exception ex)
				{
				throw new Exception(ex.ToString());
				}
			}

		
		public DataTable get_workorders(int poprog_id)
			{
			var dt = new DataTable();
			try
				{
				dt = Toolbox.doSQL_dt(@"SELECT distinct woprog.WOProg_ID, woprog.woprog_bvwo, woprog.woprog_customername, woprog.WOProg_Status FROM po_details_current INNER JOIN woprog ON po_details_current.po_details_woprog_id = woprog.WOProg_ID  WHERE po_details_current.is_gl_account=false and po_details_current.po_details_poprog_id =@v0", new object[] { poprog_id });
				}
			catch(Exception exce){Toolbox.do_errorLog_errorStack(exce);}
			return dt;
			}

		public void UpdatePOTotals()
			{
			try
				{
				var POTotal = Toolbox.doSQL_double(@"Select ifnull((SELECT SUM(po_details_qty_ordered * po_details_cost) FROM po_details_current  WHERE po_details_poprog_id =@v0),0) ", new object[] { poprog_id });
				POTotal = Math.Round(POTotal, 2);
				var POrecTotal = Toolbox.doSQL_double(@"Select ifnull((SELECT SUM(po_details_qty_received * po_details_cost) FROM po_details_current  WHERE po_details_poprog_id =@v0),0) ", new object[] { poprog_id });
				POrecTotal = Math.Round(POrecTotal, 2);

				Toolbox.doSQL_void(@"UPDATE poprog_header
SET poprog_total_cost =@v0 ,
poprog_total_recCost =@v1  
WHERE poprog_id =@v2", new object[] { POTotal, POrecTotal, poprog_id });
				}
			catch(Exception exce){Toolbox.do_errorLog_errorStack(exce);}
			}
	
        public static void send_to_ap_problems(object poid, NeMember sent_by, string notes)
        {
            NePOProg po = new NePOProg(Convert.ToInt32(poid));
            #region check for work orders that have been approved and snag them back!
            DataTable dt = Toolbox.doSQL_dt(@"SELECT DISTINCT
po_details_current.po_details_woprog_id,
woprog.WOProg_Status
FROM
po_details_current
INNER JOIN poprog_header ON po_details_current.po_details_poprog_id = poprog_header.poprog_id
INNER JOIN woprog ON po_details_current.po_details_woprog_id = woprog.woprog_id
WHERE
po_details_current.po_details_poprog_id = @v0 and
woprog.WOProg_Status in ('Waiting To Be Invoiced', 'Waiting BM Approval', 'Waiting Parent BM Approval', 'Waiting PM Approval')", new object[] { poid });

            foreach (DataRow dr in dt.Rows)
            {
                NeWOProg wo = new NeWOProg(Convert.ToInt32(dr["po_details_woprog_id"]));
                #region move back to reworks
                try
                {
                    NeWOProg.check_before_move(wo, OpsWOStatus.WaitingBMApproval);
                }
                catch (Exception check_move_error)
                {
                    throw new Exception(check_move_error.Message);
                }
                NeWOProg.move_status(new NeWOProg(Convert.ToInt32(dr["po_details_woprog_id"])), sent_by, "", OpsWOStatus.Enums.Rework);
				if(wo.Status == OpsWOStatus.WaitingToBeInvoiced)
					{
					NeWODetailCurrent.move_to_current(wo.woprog_id);
					}
                #endregion
                #region email people involved
                var msg = new NeEMail();
                msg.To = new NeMember(wo.intProjectManager).NEEmail;
                msg.From = sent_by.NEEmail;
                msg.Subject = wo.OrderNumber + " (" + wo.CustomerName + ") has been sent BACK to rework because of an AP Problems";
                
                msg.Body = string.Format(@"{0} sent a purchase order related to this work order to AP problems, so we had to revert this work order back to rework to become approved again, once the purchase order problem has been cleared.  The PO # is {1}.", sent_by.FullName, po.poprog_bvpo);
                msg.Send();
                #endregion
            }


            #endregion

            #region notifications
            NeBusinessUnit.EmailBranchPurchaser(po.business_unit_id, string.Format("PO {0} has been sent to AP Problems", po.poprog_bvpo), string.Format("<div style='font-family: Arial'><b>This PO needs your immediate attention!</b> <br /><a href='"+ Toolbox.app_setting("Domain") +"/sections/purchaseorder/po_prog_add.aspx?action=show&poprogid={0}'>Load PO</a></div>", po.poprog_id), false);



            #endregion
            #region change the status of PO and save the note
            Toolbox.doSQL_void(@"UPDATE poprog_header SET poprog_status = @v0  , poprog_last_modified=now() WHERE poprog_id = @v1  LIMIT 1", new object[] { 10, po.poprog_id });
            status_log.add(po.poprog_id, sent_by.id32, 10);
            Toolbox.doSQL_void(@"UPDATE poprog_header SET ap_problem_last_notice_sent = CURDATE(), poprog_last_modified=now() WHERE poprog_id = @v0  LIMIT 1", new object[] { po.poprog_id });
            


            #endregion
        }

        public static void use_scan(int poid, object business_unit_id, string filename, int _scantype, NeMember member, string _notes, string override_from_path )
			{
			try
				{
				using (var conn = Toolbox.connect())
					{
					var scan_id = Toolbox.doSQL_return_id(conn, @"INSERT INTO poprog_scans (poprog_scans_type,poprog_scans_poprog_id, poprog_scans_date_linked)  VALUES(@v0,@v1, now())", new object[] { _scantype.ToString(), poid });
					var popath = Toolbox.doSQL_string(conn, @"SELECT POPath from business_unit  WHERE ID =@v0", new object[] { business_unit_id }).TrimEnd('\\');
					var fileServer = NeTaxEntity.BaseFolder(business_unit_id, false);
					var wasAPProblems = filename.Contains("PO_NEEDED");
					try
						{
						if (override_from_path == "")
							{
							MoveXMLCommentsToPO(conn, popath + "\\" + filename, poid);
							File.Move(popath + "\\" + filename, popath + "\\LinkedPackingSlips\\" + scan_id + ".pdf");
							File.Copy(fileServer + "\\POs\\scanned_items\\" + filename, fileServer + "\\POs\\named_scans\\" + scan_id + ".pdf", true);
							File.Delete(fileServer + "\\POs\\scanned_items\\" + filename);
							}
						else
							{
							MoveXMLCommentsToPO(conn, override_from_path + "\\" + filename, poid);
							// Move file from global location and copy&delete from scanned_items location!
							File.Move(override_from_path + "\\" + filename, popath + "\\LinkedPackingSlips\\" + scan_id + ".pdf");
							File.Copy(fileServer + "\\POs\\scanned_items\\" + filename, fileServer + "\\POs\\named_scans\\" + scan_id + ".pdf", true);
							File.Delete(fileServer + "\\POs\\scanned_items\\" + filename);
							}
						}
					catch
						{
						throw new Exception("An error occured when the scanned file was being renamed.  Please make sure file is not being used by another program somewhere.");
						}


					
					if (_scantype == 1)
						{
						Toolbox.doSQL_void(conn, @"UPDATE poprog_header SET poprog_packing_slip_scan_date = now()  WHERE poprog_id =@v0", new object[] { poid });
						}
					else if (_scantype == 2)
						{
						Toolbox.doSQL_void(conn, @"UPDATE poprog_header SET poprog_invoice_slip_scan_date = now()  WHERE poprog_id =@v0", new object[] { poid });
						}

					var po = new NePOProg(poid);
					var branch = new NeBusinessUnit((int)po.business_unit_id);
					var vend = new NEVendor(po.poprog_vendor_id);
					var msg = new NeEMail
						          {
						          To = branch.country == "CDN"
							          ? "ap@" + Toolbox.app_setting("DomainForEmail")
                                      : "usap@"+ Toolbox.app_setting("DomainForEmail"),
						          From       = member.NEEmail,
						          Attachment = new Attachment(fileServer + "\\POs\\named_scans\\" + scan_id + ".pdf", "application/pdf"),
						          Body       = string.Format(@"{0} Added the attached invoice to PO #{1} ({2}).", member.FullName,
							          po.poprog_bvpo, vend.Vendor_Name)
						          };
					var scantype = "";
					var doSend = false;
					switch (_scantype)
						{
						case 1:
							scantype = "Packing Slip";
							break;
						case 2:
							scantype = "Invoice";
							doSend = true;
							break;
						case 3:
							scantype = "Quote";
							break;
						case 4:
							scantype = "Return Authourization";
							break;
						case 5:
							scantype = "Order Confirmation";
							break;
						case 6:
							scantype = "Shipping Documents";
							break;
						case 7:
							scantype = "AP Problems";

							po = new NePOProg(poid);
							if (po.poprog_status != 0 && po.poprog_status != 7 && po.poprog_status != 8)
								{
								send_to_ap_problems(poid, member, _notes);
								}
							_notes = "";
							break;
						}
					msg.Subject = scantype + " scan attached to PO " + po.poprog_bvpo;
					if (wasAPProblems)
						{
						doSend = true;
						msg.Subject = msg.Subject + " - Was in AP Problems";
						}
					if (doSend)
						{
						msg.Send();
						}
					var history_entry = string.Format("Purchase Order {0} had a {1} attached to it", poid, scantype);
					if (_notes != "")
						{
						AddComment(conn, poid, member.id, _notes);
						}

					Toolbox.doSQL_void(conn, @" INSERT INTO poprog_notes ( poprog_id, eventtext, date, member_id ) VALUES ( @v0 , @v1 , NOW(), @v2  )", new object[] { poid, history_entry, member.id });
					}
				}
			catch (Exception scan_move_ex)
				{
				throw new Exception("There Was an Error Attempting to Rename This Scan: " + scan_move_ex.ToString());
				}


			}

		public static void MoveXMLCommentsToPO(MySqlConnection _conn, string _path, int _poprog_id)
			{
			var poprog		= new NePOProg(_poprog_id);
			var dt			= Scan.GetXMLData(_path, 1, poprog.business_unit_id);
			using (var uow = new UnitOfWork())
				{
				foreach (DataRow dr in dt.Rows)
					{
					var member_id = 0;
					int.TryParse(dr["member"].ToString(), out member_id);
					var date		= DateTime.Parse(dr["date"].ToString());
					var comment		= HttpUtility.HtmlDecode(dr["data"].ToString());
					AddComment(_conn, _poprog_id, member_id, comment, date);
					}
				}
			Scan.Delete(_path+".xml");
			}
		public void poprog_update()
			{
			poprog_cutdate = string.IsNullOrEmpty(poprog_cutdate) ? "NULL" : poprog_cutdate;
			poprog_packing_slip_scan_date = string.IsNullOrEmpty(poprog_packing_slip_scan_date) ? "NULL" : poprog_packing_slip_scan_date;
			poprog_invoice_slip_scan_date = string.IsNullOrEmpty(poprog_invoice_slip_scan_date) ? "NULL" : poprog_invoice_slip_scan_date;
			poprog_received_date = string.IsNullOrEmpty(poprog_received_date) ? "NULL" : poprog_received_date;
			poprog_check_sent_date = string.IsNullOrEmpty(poprog_check_sent_date) ? "NULL" : poprog_check_sent_date;
			poprog_order_placed_date = string.IsNullOrEmpty(poprog_order_placed_date) ? "NULL" : poprog_order_placed_date;
			poprog_expected_order_date = string.IsNullOrEmpty(poprog_expected_order_date) ? "NULL" : poprog_expected_order_date;
			poprog_expected_received_date = string.IsNullOrEmpty(poprog_expected_received_date) ? "NULL" : poprog_expected_received_date;
			poprog_date_required = string.IsNullOrEmpty(poprog_date_required) ? "NULL" : poprog_date_required;

			Toolbox.doSQL_void(@"
UPDATE 
	poprog_header 
SET 
	business_unit_id				= @v0, 
	poprog_woprog_id				= @v1,
	poprog_customer_id				= @v2,
	poprog_vendor_id				= @v4,
	poprog_expected_order_date		= @v5,
	poprog_expected_received_date	= @v6,
	poprog_date_required			= @v7,
	poprog_order_description		= @v8,
	poprog_manual_shipaddress		= @v9,
	poprog_shipping_method			= @v10,
	poprog_country_code_currency	= @v11,
	edit_after_issue				= @v12,
	poprog_poprog_payment_method_id	= @v13,
	poprog_ack_req					= @v14,
	poprog_ack_req_rec				= @v15,
	poprog_contact_id				= @v16,
	poprog_ship_note_req			= @v17,
	poprog_ship_note_req_rec		= @v18,
	location_master_id				= @v19,
	nesi_cut_po						= @v21,
	cc_lastfour_id					= @v22,
	poprog_last_modified		= NOW(),
	poprog_bvpo						= @v23,
    Shipping_Addr1                  =@v24,
    Shipping_Addr2                  =@v25, 
    Shipping_Country                =@v26,
    Shipping_Postal                 =@v27,
    Shipping_Province               =@v28,
    Shipping_City                   =@v29
WHERE 
	poprog_id = @v20
LIMIT 1", new object[] {
				business_unit_id,                               // {0}
				poprog_woprog_id,                               // {1}
				poprog_customer_id,                             // {2}
				null,                           // {3}
				poprog_vendor_id,                               // {4}
				poprog_expected_order_date,                     // {5}
				poprog_expected_received_date,                  // {6}
				poprog_date_required,                           // {7}
				poprog_order_description,   // {8}
				poprog_manual_shipaddress,  // {9}
				poprog_shipping_method,                         // {10}
				poprog_country_code_currency,                   // {11}
				edit_after_issue,                               // {12}
				poprog_poprog_payment_method_id,                // {13}
				poprog_ack_req,                                 // {14}
				poprog_ack_req_rec,                             // {15}
				poprog_contact_id,                              // {16}
				poprog_ship_note_req,                           // {17}
				poprog_ship_note_req_rec,                       // {18}
				(location_master_id == 0 ? "NULL" : location_master_id.ToString()),                             // {19}
				poprog_id,                                      // {20}
				nesi_cut_po,                                    // {21}
				cc_lastfour_id,                                 // {22}
				poprog_bvpo, // {23}
				Shipping_Addr1,	//{24}
                Shipping_Addr2,
                Shipping_Country,
                Shipping_Postal,
                Shipping_Province,
                Shipping_City

            });

			try
				{
				notes_poprogid = poprog_id;
				eventtext = "Purchase Order " + poprog_bvpo + " was modified.";
				SaveNotes();
				}
			catch (Exception ex)
				{
				throw new Exception(ex.ToString());
				}
			}
		public void SaveNotes()
			{
			if (eventtext != "")
				{
				Toolbox.doSQL_void(@"
INSERT INTO poprog_notes 
	(
	poprog_id, 
	eventtext, 
	date, 
	member_id
	) 
VALUES
	(
	@v0,
	@v1,
	now(),
	@v2
	)", new object[] {
					notes_poprogid,
					eventtext,
					notes_memberid});
				}
			}

		/// <summary>
		/// Used for interacting with poprog_status table
		/// </summary>
		public class status
			{
			public int id { get; set; }
			public string name { get; set; }
			public status() { }
			public status(int _id)
				{
				if (exists(_id))
					{
					load(_id);
					}
				}
			public bool exists(int _id)
				{
				using (var uow = new UnitOfWork())
					{
					var s = uow.GetObjectByKey<ne_xpo.cs.poprog_status>(_id);
					return s != null;
					}
				}
			private void load(int _id)
				{
				using (var uow = new UnitOfWork())
					{
					var s = uow.GetObjectByKey<ne_xpo.cs.poprog_status>(_id);
					id = s.poprog_status_id;
					name = s.status_type;
					}
				}
			public static string by_id(int _id)
				{
				using (var uow = new UnitOfWork())
					{
					var s = uow.GetObjectByKey<ne_xpo.cs.poprog_status>(_id);
					return s.status_type;
					}
				}
			}
		/// <summary>
		/// Used for interacting with the poprogstatus table
		/// </summary>
		public class status_log
			{
			public int id { get; set; }
			public int poprog_id { get; set; }
			public int member_id { get; set; }
			public string status { get; set; }
			public DateTime dt { get; set; }
			public int status_id { get; set; }
			public double approved_amount { get; set; }

			public status_log() { }
			public status_log(int _id)
				{
				if (exists(_id))
					{
					load(_id);
					}
				}
			public static void add(int _poprog_id, int _member_id, int _status_id, double approvedAmount = 0)
				{
				var p = new NePOProg.status_log
							{
							poprog_id       = _poprog_id,
							member_id       = _member_id,
							status_id       = _status_id,
							approved_amount = approvedAmount
							};
				p.status = NePOProg.status.by_id(p.status_id);
				p.dt = DateTime.Now;
				p.save();
				}
			private void load(int _id)
				{
				using (var uow = new UnitOfWork())
					{
					var s = uow.GetObjectByKey<ne_xpo.cs.poprogstatus>(_id);
					id = s.poprogstatus_id;
					poprog_id = s.poprogstatus_poprog_id;
					member_id = s.poprogstatus_member_id.member_id;
					status = s.poprogstatus_status;
					dt = s.poprogstatus_datetime;
					approved_amount = s.approved_amount;
					status_id = s.poprogstatus_status_id.poprog_status_id;
					}
				}
			public bool exists(int _id)
				{
				using (var uow = new UnitOfWork())
					{
					var s = uow.GetObjectByKey<ne_xpo.cs.poprogstatus>(_id);
					return s != null;
					}
				}
			public void save()
				{
				using (var uow = new UnitOfWork())
					{
					var s = id == 0
						? new ne_xpo.cs.poprogstatus(uow)
						: uow.GetObjectByKey<ne_xpo.cs.poprogstatus>(id);
					s.poprogstatus_poprog_id = poprog_id;
					s.poprogstatus_member_id = uow.GetObjectByKey<ne_xpo.cs.member>(member_id);
					s.poprogstatus_status = status;
					s.poprogstatus_datetime = dt;
					s.approved_amount = approved_amount;
					s.poprogstatus_status_id = uow.GetObjectByKey<ne_xpo.cs.poprog_status>(status_id);
					s.Save();
					uow.CommitChanges();
					if (id == 0)
						{
						id = s.poprogstatus_id;
						}
					}
				}
			}
		/// <summary>
		/// Used for adding comments to PO's without a DateTime (NOW)
		/// </summary>
		/// <param name="_conn"></param>
		/// <param name="_poprog_id"></param>
		/// <param name="_member_id"></param>
		/// <param name="_comment"></param>
		public static void AddComment(MySqlConnection _conn, int _poprog_id, int _member_id, string _comment)
			{
			AddComment(_conn, _poprog_id, _member_id, _comment, DateTime.Now);
			}
		/// <summary>
		/// Used for adding comments to PO's with a DateTime
		/// </summary>
		/// <param name="_conn"></param>
		/// <param name="_poprog_id"></param>
		/// <param name="_member_id"></param>
		/// <param name="_comment"></param>
		/// <param name="_date"></param>
		public static void AddComment(MySqlConnection _conn, int _poprog_id, int _member_id, string _comment, DateTime _date)
			{
			Toolbox.doSQL_void(_conn, @"INSERT INTO poprogcomment (poprogcomment_poprog_id,poprogcomment_text,poprogcomment_datetime,poprogcomment_member_id)  values (@v0,@v1,@v3,@v2)", new object[] { _poprog_id, _comment, _member_id, _date });
			}
		public class Scan
			{
			/// <summary>
			/// Renames the scan, and accompanying XML comment files (if they exist)
			/// </summary>
			/// <param name="_fromPath"></param>
			/// <param name="_toPath"></param>
			public static void Rename(string _fromPath, string _toPath, bool move, int _businessUnitId = 0)
			{
				if(move)
				{
                    try
                    {
                        File.Move(_fromPath, _toPath);
                    }
                    catch (Exception ex)
                    {
                        Toolbox.do_errorLog_errorStack(ex);
                        throw ex;
                    }

					if(File.Exists(_fromPath+".xml"))
					{
                        try
                        {
                            File.Move(_fromPath + ".xml", _toPath + ".xml");
                        }
                        catch (Exception ex)
                        {
                            Toolbox.do_errorLog_errorStack(ex);
                            throw ex;
                        }
					}
					else // Create baseline XML file
					{
						SaveXMLData(_toPath, 0, "", _businessUnitId);
					}
				}
				else
				{
                    try
                    {
                        File.Copy(_fromPath, _toPath, true);
                    }
                    catch (Exception ex)
                    {
                        Toolbox.do_errorLog_errorStack(ex);
                        throw ex;
                    }

					if(File.Exists(_fromPath+".xml"))
					{
                        try
                        {
                            File.Copy(_fromPath + ".xml", _toPath + ".xml", true);
                        }
                        catch (Exception ex)
                        {
                            Toolbox.do_errorLog_errorStack(ex);
                            throw ex;
                        }
					}
					else // Create baseline XML file
					{
						SaveXMLData(_toPath, 0, "", _businessUnitId);
					}
				}
			}
			public static int GetAssociatedBusinessUnit(string _pdfFilePath)
				{
				if(!File.Exists(_pdfFilePath)) throw new FileNotFoundException(_pdfFilePath+" doesn't exist");
				var xmlFilePath			= _pdfFilePath+".xml";
                if (!File.Exists(xmlFilePath)) throw new FileNotFoundException(xmlFilePath + " doesn't exist");

                var xmlDoc	= new XmlDocument();
				using (var stream = File.Open(xmlFilePath, FileMode.Open))
					{
					xmlDoc.Load(stream);
					var buNode			= xmlDoc.SelectSingleNode("//business_unit_id");
					return buNode == null ? 0 : Convert.ToInt32(buNode.InnerText);
					}
				}
			/// <summary>
			/// Addon for Getting the XML data, and will send an email of the chat history
			/// </summary>
			/// <param name="_pdfFilePath"></param>
			/// <param name="_memberId"></param>
			/// <param name="_doEmail"></param>
			/// <returns></returns>
			public static DataTable GetXMLData(string _pdfFilePath, int _memberId, int _businessUnitId = 0, bool _doEmail = false)
				{
				if(!File.Exists(_pdfFilePath)) throw new FileNotFoundException(_pdfFilePath+" doesn't exist");
				var xmlFilePath			= _pdfFilePath+".xml";
				if(!File.Exists(xmlFilePath))
					{
					SaveXMLData(_pdfFilePath, _memberId, "");
					}
				var employee	= new NeMember(_memberId);
				var ds			= new DataSet();
				ds.ReadXml(xmlFilePath);
				var dt			= new DataTable();// ds.Tables["comment"];
				dt.Columns.Add("date", typeof(DateTime));
				dt.Columns.Add("member", typeof(int));
				dt.Columns.Add("data", typeof(string));
				if(ds.Tables["comments"] != null && ds.Tables["comments"].Rows.Count > 0)
					{
					foreach(DataRow dr in ds.Tables["comment"].Rows)
						{
						var dr_new = dt.NewRow();
						dr_new["date"]	= DateTime.Parse(dr["date"].ToString());
						dr_new["member"] = int.Parse(dr["member"].ToString());
						dr_new["data"] = dr["data"].ToString();
						dt.Rows.Add(dr_new);
						}
					var dv		= dt.DefaultView;
					dv.Sort		= "date desc";
					dt			= dv.ToTable();
					}
				if(_pdfFilePath.Contains("PO_NEEDED"))
					{
					var path = Path.GetDirectoryName(_pdfFilePath)+"/"; // Needs trailing "/"
					var fileName = Path.GetFileName(_pdfFilePath);
					var fileNameNoPONeeded = fileName.Replace("PO_NEEDED-", "");
					var xmlDoc	= new XmlDocument();
					xmlDoc.Load(xmlFilePath);
					var hasBUid = false;
					var name = "";
					var children = xmlDoc.DocumentElement.ChildNodes;
					for(var cn = 0; cn < xmlDoc.DocumentElement.ChildNodes.Count; cn++)
						{
						if(children[cn].Name == "business_unit_id")
							{
							hasBUid = true;
							}
						if(children[cn].Name == "filename")
							{
							name = children[cn].InnerText;
							}
						}
					if(!hasBUid)
						{
						var cleanPath = path.Replace("\\", "/");
						var cPaths	= Toolbox.doSQL_int(@"SELECT COUNT(*) FROM business_unit WHERE popath = @v0 AND active = 'T'", new object[] { cleanPath });

						// Are there multiple BUs with the same scan folder?
						if(cPaths == 1)// Only 1 - Write the BU associated to the scan
							{
							var associatedBUid = Toolbox.doSQL_int(@"SELECT id FROM business_unit WHERE popath = @v0", new object[] { cleanPath });
							using (var stream = File.Open(xmlFilePath, FileMode.Open))
								{
								xmlDoc.Load(stream);
								var buNode			= xmlDoc.CreateElement("business_unit_id");
								buNode.InnerText	= associatedBUid.ToString();
								var documentNode	= xmlDoc.SelectSingleNode("//document");
								documentNode.AppendChild(buNode);
								stream.SetLength(0);
								xmlDoc.Save(stream);
								}
							return GetXMLData(_pdfFilePath, _memberId, _businessUnitId, _doEmail); //Recursive
							}
						else if(cPaths > 1) // Multiple - Send it back to AP problems.
							{
							var associatedCountry = Toolbox.doSQL_string(@"SELECT IFNULL(MAX(country), 'CDN') FROM business_unit WHERE popath = @v0 AND active = 'T'", new object[] { cleanPath });
							string override_path = associatedCountry == "USA"
													? Toolbox.app_setting("po_all_scans_usa") 
													: Toolbox.app_setting("po_all_scans_can");
							var fromPath = _pdfFilePath;
							var toPath = override_path + fileNameNoPONeeded;
							SaveXMLData(fromPath, _memberId, "Moved back to AP Problems, multiple business units associated to path, and no originating business unit ID", 0);
							Rename(fromPath, toPath, true, 0);
							shared.alert_ap(string.Format("Scan {0} returned to regional scans by the system", fileNameNoPONeeded), string.Format("The scan {0} was returned back to regional scans by the system because it detected the scan didn't have a business unit associated with it, and there are multiple business units with the same po path.", fileNameNoPONeeded), associatedCountry == "CDN");
							_doEmail = false;
							}
						else // This should NEVER happen... but should alert us to a problem.
							{
							shared.alert_debug("Detected a PO Path that is blank", string.Format("The file {0} was being accessed by {1}, and a path wasn't returned from the associated file object.", _pdfFilePath, employee.FullName));
							_doEmail = false;
							}
						}
					}
				if(_doEmail)
					{
					var users	= new Dictionary<int, NeMember>();
					var sbEmail	= new StringBuilder();
					foreach(DataRow dr in dt.Rows)
						{
						var memberId = Convert.ToInt32(dr["member"]);
						var member	= new NeMember(memberId);
						var date = DateTime.Parse(dr["date"].ToString());
						var message = dr["data"].ToString();
						if(!users.ContainsKey(memberId) && _memberId != memberId)
							{
							users.Add(memberId, member);
							}
						sbEmail.AppendFormat(@"<div><div style='font-weight:bold'>{0} @ {1:g}</div><div>{2}</div></div><br/><br/>", member.FullName, date, message);
						}
					if(users.Count > 0 && sbEmail.Length > 0)
						{
						var emailTo	= "";
						var fromMember = new NeMember(_memberId);
						foreach(var u in users)
							{
							emailTo	+= u.Value.NEEmail+";";
							}
						var email = new NeEMail
							{
							To	= emailTo,
							From = "noreply@" + Toolbox.app_setting("DomainForEmail"),
							Subject = "Scan chat updated by "+fromMember.FullName,
							Body = sbEmail.ToString(),
							isHTML = true,
							Attachment = new Attachment(_pdfFilePath)
							};
						email.Send();
						}
					}
				return dt;
				}

			/// <summary>
			/// Allows the addition of comments to a "helper" XML file, named exactly as the PDF file, plus .xml at the end
			/// </summary>
			/// <param name="_pdfFilePath"></param>
			/// <param name="_memberId"></param>
			/// <param name="_comment"></param>
			public static void SaveXMLData(string _pdfFilePath, int _memberId, string _comment, int _businessUnitId = 0)
				{
				if(!File.Exists(_pdfFilePath)) throw new FileNotFoundException(_pdfFilePath+" doesn't exist");
				var xmlFilePath			= _pdfFilePath+".xml";
				var newFile				= !File.Exists(xmlFilePath);
				var xml	= new XmlDocument();
				if(newFile)
					{
					XmlWriter writer	= null;
					try
						{
						XmlWriterSettings settings = new XmlWriterSettings();
						settings.Indent = true;
						settings.IndentChars = ("\t");
						settings.OmitXmlDeclaration = true;
						writer	= XmlWriter.Create(xmlFilePath, settings);
						}
					finally
						{
						if(writer != null)
							{
							writer.Close();
							}
						}
					var docNode			= xml.CreateElement("document");
					var docName			= xml.CreateElement("filename");
					var docBU			= xml.CreateElement("business_unit");
					var docComments		= xml.CreateElement("comments");
					docName.InnerText	= _pdfFilePath;
					if(_businessUnitId > 0)
						{
						docBU.InnerText		= _businessUnitId.ToString();
						}
					docNode.AppendChild(docName);
					if(_businessUnitId > 0)
						{
						docNode.AppendChild(docBU);
						}
					docNode.AppendChild(docComments);

					xml.AppendChild(docNode);
					using (var stream = File.Open(xmlFilePath, FileMode.Open))
						{
						xml.Save(stream);
						}
					File.SetAttributes(xmlFilePath, FileAttributes.Hidden);
					}
				if(_comment == "" && _businessUnitId == 0) return;
				using (var stream = File.Open(xmlFilePath, FileMode.Open))
					{
					xml.Load(stream);
					if(_businessUnitId > 0)
						{
						var documentNode	= xml.SelectSingleNode("//document");
						var buNode			= xml.SelectSingleNode("//business_unit_id");
						var creatingNode	= buNode == null;
						if(creatingNode)
							{
							buNode			= xml.CreateElement("business_unit_id");
							}
						buNode.InnerText	= _businessUnitId.ToString();
						if(creatingNode)
							{
							documentNode.AppendChild(buNode);
							}
						else
							{
							documentNode.ReplaceChild(buNode, buNode);
							}
						}
					if(_comment != "")
						{
						var comment			= xml.CreateElement("comment");
						var date			= xml.CreateElement("date");
						date.InnerText		= Toolbox.MySQLNow_long();
						var member			= xml.CreateElement("member");
						member.InnerText	= _memberId.ToString();
						var data			= xml.CreateElement("data");
						data.InnerText		= HttpUtility.HtmlEncode(_comment);
						comment.AppendChild(date);
						comment.AppendChild(member);
						comment.AppendChild(data);
						var commentsNode	= xml.SelectSingleNode("//comments");
						commentsNode.AppendChild(comment);
						}
					stream.SetLength(0);
					xml.Save(stream);
					}
				}
			/// <summary>
			/// Used for deleting temporary XML comments files
			/// </summary>
			/// <param name="_path"></param>
			internal static void Delete(string _path)
				{
				if(!_path.EndsWith(".xml"))
					{
					throw new Exception("Must be an XML file referenced.");
					}
				if(File.Exists(_path))
					{
					File.Delete(_path);
					}
				}
			}

		public static decimal GetMostRecentApprovedValue(int _poProgId)
			{
			return Toolbox.doSQL_decimal(@"
SELECT IFNULL((
	SELECT 
		approved_amount 
	FROM 
		poprogstatus 
	WHERE 
		poprogstatus_poprog_id = @v0 AND 
		poprogstatus_status_id = @v1 
	ORDER BY 
		poprogstatus_id DESC 
	LIMIT 1
	), 0)", new object[]{_poProgId, OpsPOStatus.ApprovedtoOrder});
			}

		public static decimal TotalCostWithoutLine(int _poProgId, int _poDetailsId)
			{
			return Toolbox.doSQL_decimal(@"
SELECT 
	IFNULL(SUM(po_details_qty_ordered * po_details_cost), 0)
FROM 	
	po_details_current 
WHERE 
	po_details_poprog_id = @v0 AND
	po_details_id != @v1
", new object[]{ _poProgId, _poDetailsId});
			}
		public static decimal TotalCost(int _poProgId)
			{
			return Toolbox.doSQL_decimal(@"
SELECT 
	IFNULL(SUM(po_details_qty_ordered * po_details_cost), 0)
FROM 	
	po_details_current 
WHERE 
	po_details_poprog_id = @v0
", new object[]{ _poProgId });
			}

		public static bool ReceiptsExist(int _poProgId, double _poReference)
			{
			return Toolbox.doSQL_int(@"
SELECT 
	COUNT(*) 
FROM 
	poprog_part_history 
WHERE 
	poprog_id = @v0 AND 
	reference_no = @v1 AND 
	diff_qty_received != 0", new object[] { _poProgId, _poReference}) > 0;
			}
		}
	}
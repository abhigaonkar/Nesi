using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Linq;
using System.Web;
using NESI.Common.Models;

//using nesi.bv;

namespace nesi.core
{
	/// <summary>
	/// Summary description for NEPO_Details
	/// </summary>
	public class NEPO_Details_Current
	{

		private int _po_details_id;
		private int _po_details_rec_no;
		private int _po_details_reference;
		private int _po_details_part_no;
		private string _po_details_vendor_part_no = "";
		private string _po_details_description = "";
		private string _po_details_notes = "";
		private double _po_details_qty_orderd = 0;
		private double _po_details_qty_received = 0;
		private double _po_details_cost = 0;
		private double _po_details_sell_price = 0;
		private int _po_details_tax1 = 0;
		private int _po_details_tax2 = 0;
		private int _po_details_tax3 = 0;
		private int _po_details_tax4 = 0;
		private int _po_details_poprog_id;
		private int _po_details_woprog_id;
		private string _po_details_date_added;
		private string _po_detials_date_modified;
		private string _po_details_date_expected;
		private int _po_detials_add_member_id = 0;
		private int _po_details_audit_member_id = 0;
		private double _po_details_vendor_qty_per = 1;
		private int _to_wo;
		private int _po_details_line_active = 1;


		private int _part_history_id = 0;
		private int _reference_number = 0;
		private string _part_no = "";
		private string _vendor_part_no = "";
		private string _description = "";
		private double _qty_ordered = 0;
		private double _qty_received = 0;
		private double _cost = 0;
		private int _popprog_id = 0;
		private int _woprog_id = 0;
		private string _dateofcahnge = "";
		private int _member_id = 0;
		private int _part_added = 0;
		private int _part_deleted = 0;
		private string _dateexpectedchange = "";
		private string _gl_account = "";
		private int _currency_id = 0;
		private int _business_unit_id;
		private bool _is_gl_account = false;
		private int _expense_category_id = 0;

		public string gl_account { get; set; }
		public int currency_id { get; set; }

		public int po_details_id
		{
			set
			{
				_po_details_id = value;
			}
			get
			{
				return _po_details_id;
			}
		}
		public int po_details_rec_no
		{
			get
			{
				return _po_details_rec_no;
			}
			set
			{
				if (_po_details_rec_no == value)
					return;
				_po_details_rec_no = value;
			}
		}
		public int po_details_reference
		{
			get
			{
				return _po_details_reference;
			}
			set
			{
				if (_po_details_reference == value)
					return;
				_po_details_reference = value;
			}
		}
		public int po_details_part_no
		{
			set
			{
				_po_details_part_no = value;
			}
			get
			{
				return _po_details_part_no;
			}
		}
		public string po_details_vendor_part_no
		{
			set
			{
				_po_details_vendor_part_no = value;
			}
			get
			{
				return _po_details_vendor_part_no;
			}
		}
		public string po_details_description
		{
			set
			{
				_po_details_description = value;
			}
			get
			{
				return _po_details_description;
			}
		}
		public string po_details_notes
		{
			set
			{
				_po_details_notes = value;
			}
			get
			{
				return _po_details_notes;
			}

		}
		public double po_details_qty_orderd
		{
			set
			{
				_po_details_qty_orderd = value;
			}
			get
			{
				return _po_details_qty_orderd;
			}
		}
		public double po_details_qty_received
		{
			set
			{
				_po_details_qty_received = value;
			}
			get
			{
				return _po_details_qty_received;
			}
		}
		public double po_details_cost
		{
			set
			{
				_po_details_cost = value;
			}
			get
			{
				return _po_details_cost;
			}
		}
		public double po_details_sell_price
		{
			set
			{
				_po_details_sell_price = value;
			}
			get
			{
				return _po_details_sell_price;
			}
		}
		public int po_details_tax1
		{
			get
			{
				return _po_details_tax1;
			}
			set
			{
				if (_po_details_tax1 == value)
					return;
				_po_details_tax1 = value;
			}
		}
		public int po_details_tax2
		{
			get
			{
				return _po_details_tax2;
			}
			set
			{
				if (_po_details_tax2 == value)
					return;
				_po_details_tax2 = value;
			}
		}
		public int po_details_tax3
		{
			get
			{
				return _po_details_tax3;
			}
			set
			{
				if (_po_details_tax3 == value)
					return;
				_po_details_tax3 = value;
			}
		}
		public int po_details_tax4
		{
			get
			{
				return _po_details_tax4;
			}
			set
			{
				if (_po_details_tax4 == value)
					return;
				_po_details_tax4 = value;
			}
		}
		public int po_details_poprog_id
		{
			set
			{
				_po_details_poprog_id = value;
			}
			get
			{
				return _po_details_poprog_id;
			}
		}
		public int po_details_woprog_id
		{
			set
			{
				_po_details_woprog_id = value;
			}
			get
			{
				return _po_details_woprog_id;
			}
		}
		public string po_details_date_added
		{
			set
			{
				_po_details_date_added = value;
			}
			get
			{
				return _po_details_date_added;
			}
		}
		public string po_details_date_modified
		{
			set
			{
				_po_detials_date_modified = value;
			}
			get
			{
				return _po_detials_date_modified;
			}
		}
		public string po_details_date_expected
		{
			set
			{
				_po_details_date_expected = value;
			}
			get
			{
				return _po_details_date_expected;
			}
		}
		public int po_details_add_member_id
		{
			set
			{
				_po_detials_add_member_id = value;
			}
			get
			{
				return _po_detials_add_member_id;
			}
		}
		public int po_details_audit_member_id
		{
			set
			{
				_po_details_audit_member_id = value;
			}
			get
			{
				return _po_details_audit_member_id;
			}
		}
		public double po_details_vendor_qty_per
		{
			get
			{
				return _po_details_vendor_qty_per;
			}
			set
			{
				if (_po_details_vendor_qty_per == value)
					return;
				_po_details_vendor_qty_per = value;
			}
		}
		public int to_wo
		{
			get
			{
				return _to_wo;
			}
			set
			{
				if (_to_wo == value)
					return;
				_to_wo = value;
			}
		}
		public int po_details_line_active
		{
			get
			{
				return _po_details_line_active;
			}
			set
			{
				if (_po_details_line_active == value)
					return;
				_po_details_line_active = value;
			}
		}

		public int part_history_id
		{
			set
			{
				_part_history_id = value;
			}
			get
			{
				return _part_history_id;
			}
		}
		public int reference_number
		{
			set
			{
				_reference_number = value;
			}
			get
			{
				return _reference_number;
			}
		}
		public string part_no
		{
			get
			{
				return _part_no;
			}
			set
			{
				_part_no = value;
			}
		}
		public string vendor_part_no
		{
			get
			{
				return _vendor_part_no;
			}
			set
			{
				_vendor_part_no = value;
			}
		}
		public string description
		{
			set
			{
				_description = value;
			}
			get
			{
				return _description;
			}
		}
		public double qty_ordered
		{
			set
			{
				_qty_ordered = value;
			}
			get
			{
				return _qty_ordered;
			}
		}
		public double qty_received
		{
			set
			{
				_qty_received = value;
			}
			get
			{
				return _qty_received;
			}
		}
		public double cost
		{
			set
			{
				_cost = value;
			}
			get
			{
				return _cost;
			}
		}
		public int popprog_id
		{
			set
			{
				_popprog_id = value;
			}
			get
			{
				return _popprog_id;
			}
		}
		public int woprog_id
		{
			set
			{
				_woprog_id = value;
			}
			get
			{
				return _woprog_id;
			}
		}
		public string dateofcahnge
		{
			set
			{
				_dateofcahnge = value;
			}
			get
			{
				return _dateofcahnge;
			}
		}
		public int member_id
		{
			set
			{
				_member_id = value;
			}
			get
			{
				return _member_id;
			}
		}

		public int part_added
		{
			set
			{
				_part_added = value;
			}
			get
			{
				return _part_added;
			}
		}
		public int part_deleted
		{
			set
			{
				_part_deleted = value;
			}
			get
			{
				return _part_deleted;
			}
		}
		public string dateexpectedchange
		{
			get
			{
				return _dateexpectedchange;
			}
			set
			{
				if (_dateexpectedchange == value)
					return;
				_dateexpectedchange = value;
			}
		}

		public int business_unit_id
		{
			set
			{
				_business_unit_id = value;
			}
			get
			{
				return _business_unit_id;
			}
		}

		public bool is_gl_account
		{
			set
			{
				_is_gl_account = value;
			}
			get
			{
				return _is_gl_account;
			}
		}
		public int expense_category_id
		{
			set
			{
				_expense_category_id = value;
			}
			get
			{
				return _expense_category_id;
			}
		}

		public NEPO_Details_Current() { }
		public NEPO_Details_Current(int lineid)
		{
			load(lineid);
		}
		public void po_details_current_line(int lineid)
		{
			load(lineid);
		}

		public void LoadByPoIdRecNoWoId(int poID, int rec_no, int woId)
		{
			var detailrow = Toolbox.doSQL_dt(@"SELECT * FROM po_details_current  WHERE po_details_poprog_id = @v0 AND po_details_rec_no = @v1 AND po_details_woprog_id = @v2", 
				new object[] { poID, rec_no, woId });
			foreach (DataRow row in detailrow.Rows)
			{
				po_details_rec_no = Convert.ToInt32(row["po_details_rec_no"]);
				po_details_reference = Convert.ToInt32(row["po_details_reference"]);
				po_details_part_no = Convert.ToInt32(row["po_details_part_no"]);
				po_details_vendor_part_no = row["po_details_vendor_part_no"].ToString();
				po_details_description = row["po_details_description"].ToString();
				po_details_notes = row["po_details_notes"].ToString();
				po_details_qty_orderd = Convert.ToDouble(row["po_details_qty_ordered"]);
				po_details_qty_received = Convert.ToDouble(row["po_details_qty_received"]);
				po_details_cost = Convert.ToDouble(row["po_details_cost"]);
				po_details_sell_price = Convert.ToDouble(row["po_details_sell_price"]);
				po_details_tax1 = Convert.ToInt32(row["po_details_tax1"]);
				po_details_tax2 = Convert.ToInt32(row["po_details_tax2"]);
				po_details_tax3 = Convert.ToInt32(row["po_details_tax3"]);
				po_details_tax4 = Convert.ToInt32(row["po_details_tax4"]);
				po_details_poprog_id = Convert.ToInt32(row["po_details_poprog_id"]);
				po_details_woprog_id = Convert.ToInt32(row["po_details_woprog_id"]);
				po_details_date_added = row["po_details_date_added"].ToString();
				po_details_date_modified = row["po_details_date_modified"].ToString();
				po_details_date_expected = row["po_details_date_expected"].ToString();
				po_details_add_member_id = Convert.ToInt32(row["po_details_add_member_id"]);
				po_details_audit_member_id = Convert.ToInt32(row["po_details_audit_member_id"]);
				po_details_vendor_qty_per = Convert.ToDouble(row["po_details_vendor_qty_per"]);
				po_details_vendor_qty_per = po_details_vendor_qty_per == 0 ? 1 : po_details_vendor_qty_per;
				to_wo = Convert.ToInt32(row["to_wo"]);
				po_details_line_active = Convert.ToInt32(row["po_details_line_active"]);
				business_unit_id = Convert.ToInt32(row["business_unit_id"]);
				is_gl_account = Convert.ToBoolean(row["is_gl_account"]);
				expense_category_id = (Toolbox.ReturnZeroIfNull_int(row["expense_category_id"])==0 && is_gl_account==true) ? po_details_woprog_id : Toolbox.ReturnZeroIfNull_int(row["expense_category_id"]);
				po_details_id = Convert.ToInt32(row["po_details_id"]);
			}
		}

		private void load(int lineid)
		{

			var detailrow = Toolbox.doSQL_dt(@"SELECT * FROM po_details_current  WHERE po_details_id = @v0", new object[] { lineid });
			foreach (DataRow row in detailrow.Rows)
			{
				po_details_rec_no = Convert.ToInt32(row["po_details_rec_no"]);
				po_details_reference = Convert.ToInt32(row["po_details_reference"]);
				po_details_part_no = Convert.ToInt32(row["po_details_part_no"]);
				po_details_vendor_part_no = row["po_details_vendor_part_no"].ToString();
				po_details_description = row["po_details_description"].ToString();
				po_details_notes = row["po_details_notes"].ToString();
				po_details_qty_orderd = Convert.ToDouble(row["po_details_qty_ordered"]);
				po_details_qty_received = Convert.ToDouble(row["po_details_qty_received"]);
				po_details_cost = Convert.ToDouble(row["po_details_cost"]);
				po_details_sell_price = Convert.ToDouble(row["po_details_sell_price"]);
				po_details_tax1 = Convert.ToInt32(row["po_details_tax1"]);
				po_details_tax2 = Convert.ToInt32(row["po_details_tax2"]);
				po_details_tax3 = Convert.ToInt32(row["po_details_tax3"]);
				po_details_tax4 = Convert.ToInt32(row["po_details_tax4"]);
				po_details_poprog_id = Convert.ToInt32(row["po_details_poprog_id"]);
				po_details_woprog_id = Convert.ToInt32(row["po_details_woprog_id"]);
				po_details_date_added = row["po_details_date_added"].ToString();
				po_details_date_modified = row["po_details_date_modified"].ToString();
				po_details_date_expected = row["po_details_date_expected"].ToString();
				po_details_add_member_id = Convert.ToInt32(row["po_details_add_member_id"]);
				po_details_audit_member_id = Convert.ToInt32(row["po_details_audit_member_id"]);
				po_details_vendor_qty_per = Convert.ToDouble(row["po_details_vendor_qty_per"]);
				po_details_vendor_qty_per = po_details_vendor_qty_per == 0 ? 1 : po_details_vendor_qty_per;
				to_wo = Convert.ToInt32(row["to_wo"]);
				po_details_line_active = Convert.ToInt32(row["po_details_line_active"]);
				business_unit_id = Convert.ToInt32(row["business_unit_id"]);
				is_gl_account = Convert.ToBoolean(row["is_gl_account"]);
				expense_category_id = (Toolbox.ReturnZeroIfNull_int(row["expense_category_id"]) == 0 && is_gl_account == true) ? po_details_woprog_id : Toolbox.ReturnZeroIfNull_int(row["expense_category_id"]);
			}
		}
		public void PO_Details_Save()
		{
			try
			{
				var dateexpected = Convert.ToDateTime(po_details_date_expected);
				po_details_date_expected = dateexpected.ToString("yyyy-MM-dd hh:mm:ss");
			}
			catch
			{
				po_details_date_expected = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
			}
			var refnum = 0;
			var lines = Toolbox.doSQL_int(@"SELECT IFNULL(MAX(po_details_rec_no), 1) FROM po_details_current  WHERE po_details_poprog_id =@v0", new object[] { po_details_poprog_id.ToString() });
			po_details_rec_no = lines + 1;
			po_details_reference = refnum + 1;
			var sql = "";
			try
			{
				sql = "INSERT INTO po_details_current ";
				sql += "(";
				sql += "po_details_reference, "; //0
				sql += "po_details_rec_no, ";  //1
				sql += "po_details_part_no, "; //2
				sql += "po_details_vendor_part_no, "; //3
				sql += "po_details_description, "; //4
				sql += "po_details_notes, "; //5
				sql += "po_details_qty_ordered, "; //6
				sql += "po_details_qty_received, "; //7
				sql += "po_details_cost, "; //8
				sql += "po_details_sell_price, "; //9
				sql += "po_details_tax1, "; //10
				sql += "po_details_tax2, "; //11
				sql += "po_details_tax3, "; //12
				sql += "po_details_tax4, "; //13
				sql += "po_details_poprog_id, "; //14
				sql += "po_details_woprog_id, "; //15
				sql += "po_details_date_added, ";
				sql += "po_details_date_modified, ";
				sql += "po_details_date_expected, ";
				sql += "po_details_add_member_id, ";
				sql += "po_details_audit_member_id, ";
				sql += "po_details_vendor_qty_per, to_wo, ";
				sql += "business_unit_id, is_gl_account,expense_category_id ) ";
				sql += "VALUES (";
				sql += _po_details_reference + ", ";
				sql += _po_details_rec_no + ", ";
				sql += "'" + po_details_part_no + "', ";
				sql += "'" + Toolbox.AddSlashes(po_details_vendor_part_no) + "', ";
				sql += "'" + Toolbox.AddSlashes(po_details_description) + "', ";
				sql += "'" + Toolbox.AddSlashes(po_details_notes) + "', ";
				sql += po_details_qty_orderd + ", ";
				sql += po_details_qty_received + ", ";
				sql += po_details_cost + ", ";
				sql += po_details_sell_price + ", ";
				sql += po_details_tax1 + ", ";
				sql += po_details_tax2 + ", ";
				sql += po_details_tax3 + ", ";
				sql += po_details_tax4 + ", ";
				sql += po_details_poprog_id + ", ";
				sql += po_details_woprog_id + ", ";
				sql += "now(), now(),'" + _po_details_date_expected + "',";
				sql += po_details_add_member_id + ", ";
				sql += po_details_audit_member_id + ", ";
				sql += (po_details_vendor_qty_per == 0 ? 1 : po_details_vendor_qty_per) + ", 0, ";
				sql += business_unit_id + "," + _is_gl_account + "," + expense_category_id + ")";


				refnum = Toolbox.doSQL_return_id(sql, null);

				Toolbox.doSQL_void("update po_details_current set po_details_reference=" + refnum + " where po_details_id=" + refnum);

				var vendorId = Toolbox.doSQL_int("SELECT poprog_vendor_id FROM poprog_header WHERE poprog_id = @v0", new object[] { po_details_poprog_id });
				var isESAApplicable = Toolbox.doSQL_bool("SELECT ESA_applicable FROM vendor WHERE Vendor_ID = @v0", new object[] { vendorId });
				if (isESAApplicable)
				{
					var dataRows = Toolbox.doSQL_dt("SELECT inspection_link, inspection_required FROM woprog where woprog_id = @v0 AND business_unit_id = @v1", new object[] { po_details_woprog_id, _business_unit_id }).Rows;

                    foreach (DataRow row in dataRows)
                    {
						var inspectionLink = row["inspection_link"].ToString();
						var inspectionRequired = Convert.ToBoolean(row["inspection_required"]);

					if (inspectionRequired && !inspectionLink.Contains(po_details_poprog_id.ToString()))
						{
							var inspectionLinkArr= inspectionLink.Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();
							inspectionLinkArr.Add(po_details_poprog_id.ToString());
							Toolbox.doSQL_void("UPDATE woprog SET inspection_link = @v0, woprog_ts = woprog_ts WHERE woprog_id = @v1 AND business_unit_id = @v2", new object[] { string.Join(", ", inspectionLinkArr), po_details_woprog_id, _business_unit_id });
						}
					}
				}

				try
				{
					reference_number = po_details_reference;
					part_no = po_details_part_no.ToString();
					vendor_part_no = po_details_vendor_part_no;
					description = po_details_description;
					qty_ordered = po_details_qty_orderd;
					qty_received = po_details_qty_received;
					cost = po_details_cost;
					popprog_id = po_details_poprog_id;
					woprog_id = po_details_woprog_id;
					member_id = po_details_add_member_id;
					part_added = 1;
					part_deleted = 0;
					dateexpectedchange = _po_details_date_expected;
					is_gl_account = _is_gl_account;
					expense_category_id = _expense_category_id;

					HistoryInsert(new NEPO_Details_Current());


				}
				catch (Exception ex) { throw new Exception(ex.ToString()); }
			}
			catch (Exception EXPOSave)
			{
				throw new Exception("Failure to Add Part " + po_details_part_no + "to PO:" + EXPOSave + sql);
			}

		}

		public void PO_Details_Update()
		{
			var detailstable = Toolbox.doSQL_dt(@"SELECT * FROM po_details_current  WHERE po_details_id =@v0", new object[] { po_details_id.ToString() });
			double prevqtyrec = 0;
			double prevqtyord = 0;
			var prev_values = new NEPO_Details_Current(po_details_id);
			foreach (DataRow row in detailstable.Rows)
			{
				po_details_reference = Convert.ToInt32(po_details_id);
				prevqtyord = Convert.ToDouble(row["po_details_qty_ordered"]);
				prevqtyrec = Convert.ToDouble(row["po_details_qty_received"]);
				//po_details_poprog_id = Convert.ToInt32(row["po_details_poprog_id"]);
				//po_details_woprog_id = Convert.ToInt32(row["po_details_woprog_id"]);

			}
			try
			{
				var sql = "";
				sql = " UPDATE po_details_current ";
				sql += " SET ";
				sql += " po_details_part_no =@v0,";
				sql += " po_details_vendor_part_no =@v1,";
				sql += " po_details_description = @v2,";
				sql += " po_details_qty_ordered = @v3,";
				sql += " po_details_cost = @v4,";
				sql += " po_details_qty_received = @v5,";
				sql += " po_details_date_expected = @v6,";
				sql += " po_details_vendor_qty_per =@v7,";
				sql += " po_details_woprog_id = @v8,";
				sql += " po_details_date_modified = now(), ";
				sql += " po_details_audit_member_id = @v9,";
				sql += " po_details_line_active = @v10,";
				sql += " business_unit_id = @v11, ";
				sql += " is_gl_account = @v13,";
				sql += " expense_category_id  = @v14";
				sql += " WHERE po_details_id = @v12";

				var paramObjects = new object[]
				{
						po_details_part_no,
						po_details_vendor_part_no,
						po_details_description,
						po_details_qty_orderd,
						po_details_cost,
						po_details_qty_received,
						Convert.ToDateTime(po_details_date_expected).ToString("yyyy-MM-dd"),
						po_details_vendor_qty_per == 0 ? 1 : po_details_vendor_qty_per,
						po_details_woprog_id,
						po_details_audit_member_id,
						po_details_line_active,
						business_unit_id,
						po_details_id,
						is_gl_account,
						expense_category_id
				};

				Toolbox.doSQL_void(sql, paramObjects);

				try
				{
					reference_number = po_details_reference;
					part_no = po_details_part_no.ToString();
					vendor_part_no = po_details_vendor_part_no;
					description = po_details_description;
					qty_ordered = po_details_qty_orderd;
					qty_received = po_details_qty_received;
					cost = po_details_cost;
					popprog_id = po_details_poprog_id;
					woprog_id = po_details_woprog_id;
					member_id = po_details_audit_member_id;
					part_added = 0;
					part_deleted = 0;
					dateexpectedchange = Convert.ToDateTime(po_details_date_expected).ToString("yyyy-MM-dd");
					prev_values.qty_ordered = prevqtyord;
					prev_values.qty_received = prevqtyrec;
					HistoryInsert(prev_values);

				}
				catch (Exception exce) { Toolbox.do_errorLog_errorStack(exce); }

			}
			catch (Exception EXPOUpdate)
			{
				throw new Exception("Failure to Update Part " + po_details_part_no + " " + EXPOUpdate);
			}

			if (prevqtyrec != po_details_qty_received && po_details_part_no != 0)
			{
				/*
			 * Quantity for this part has been recieved, check to see if anyone is tracking it
			 * and send then and email.
			 */
				var qty = po_details_qty_received - prevqtyrec;
				PartTrackingEmails(po_details_part_no.ToString(), qty, po_details_notes);
				NePOProg.Update_Received_Date(po_details_poprog_id);
			}

		}

		public void PO_Details_Update2()
		{
			var detailstable = Toolbox.doSQL_dt(@"SELECT * FROM po_details_current  WHERE po_details_id =@v0", new object[] { po_details_id.ToString() });
			double prevqtyrec = 0;
			double prevqtyord = 0;
			foreach (DataRow row in detailstable.Rows)
			{
				po_details_reference = Convert.ToInt32(row["po_details_id"]);
				//po_details_poprog_id = Convert.ToInt32(row["po_details_poprog_id"]);
				//po_details_woprog_id = Convert.ToInt32(row["po_details_woprog_id"]);
				prevqtyord = Convert.ToDouble(row["po_details_qty_ordered"]);
				prevqtyrec = Convert.ToDouble(row["po_details_qty_received"]);

			}
			var prev_values = new NEPO_Details_Current(po_details_id);
			try
			{
				var sql = "";
				sql = " UPDATE po_details_current ";
				sql += " SET ";
				sql += " po_details_part_no =@v0,";
				sql += " po_details_vendor_part_no =@v1,";
				sql += " po_details_description =@v2,";
				sql += " po_details_qty_ordered =@v3,";
				sql += " po_details_cost =@v4,";
				sql += " po_details_qty_received =@v5,";
				sql += " po_details_date_expected =@v6,";
				sql += " po_details_woprog_id =@v7,";
				sql += " po_details_date_modified = now(), ";
				sql += " po_details_audit_member_id =@v8,";
				sql += " po_details_line_active =@v9, ";
				sql += " is_gl_account = @v11";
				sql += " WHERE po_details_id =@v10 ";

				var paramObjects = new object[]
				{
					po_details_part_no,
					po_details_vendor_part_no,
					po_details_description,
					po_details_qty_orderd,
					po_details_cost,
					po_details_qty_received,
					po_details_date_expected,
					po_details_woprog_id,
					po_details_audit_member_id,
					po_details_line_active,
					po_details_id,
					is_gl_account
				};

				Toolbox.doSQL_void(sql, paramObjects);

				try
				{
					reference_number = po_details_reference;
					part_no = po_details_part_no.ToString();
					vendor_part_no = po_details_vendor_part_no;
					description = po_details_description;
					qty_ordered = po_details_qty_orderd;
					qty_received = po_details_qty_received;
					cost = po_details_cost;
					popprog_id = po_details_poprog_id;
					woprog_id = po_details_woprog_id;
					member_id = po_details_audit_member_id;
					part_added = 0;
					part_deleted = 0;
					dateexpectedchange = po_details_date_expected;
					prev_values.qty_ordered = prevqtyord;
					prev_values.qty_received = prevqtyrec;
					HistoryInsert(prev_values);

				}
				catch (Exception ee) { Toolbox.do_errorLog_errorStack(ee); }

			}
			catch (Exception EXPOUpdate)
			{
				throw new Exception("Failure to Update Part " + po_details_part_no + " " + EXPOUpdate);
			}
			if (prevqtyrec != po_details_qty_received && po_details_part_no != 0)
			{
				/*
			 * Quantity for this part has been recieved, check to see if anyone is tracking it
			 * and send then and email.
			 */
				var qty = po_details_qty_received - prevqtyrec;
				PartTrackingEmails(po_details_part_no.ToString(), qty, po_details_notes);
			}

		}
		/// <summary>
		/// Makes the ordered quantity match the received quantity
		/// </summary>
		/// <param name="_poprogid"></param>
		public static void BalanceQuantities(int _poprogid)
		{
			var madeChanges = false;
			using(var uow = new UnitOfWork())
			{
				var po_details = from x in new XPQuery<ne_xpo.cs.po_details_current>(uow) 
								 where x.po_details_poprog_id == _poprogid select x;
				foreach(var po_detail in po_details)
				{
					if(po_detail.po_details_qty_received != po_detail.po_details_qty_ordered)
					{
						po_detail.po_details_qty_ordered = po_detail.po_details_qty_received;
						po_detail.Save();
						madeChanges = true;
					}
				}
				if(!madeChanges) return;
				uow.CommitChanges();	
			}
		}
		protected void HistoryInsert(NEPO_Details_Current prev_values)
		{
			var this_member_id = member_id;
			
			var sql = "INSERT INTO poprog_part_history ";
			sql += "(reference_no, part_no, vendor_part_no, description, ";
			sql += " prev_qty_ordered, qty_ordered, diff_qty_ordered, prev_qty_received, qty_received, diff_qty_received, cost, poprog_id, woprog_id, ";
			sql += " dateofchange, member_id, part_added, part_deleted, notes, dateexpectedchange, is_gl_account) ";
			sql += "VALUES(@v0,@v1,@v2,@v3,@v4,@v5,@v6,@v7,@v8,@v9,@v10,@v11,@v12,NOW(),@v13,@v14,@v15,@v16,@v17,@v18)";

			var paramObjects = new object[]
			{
				 reference_number  , //0
				   part_no  , //1
				   vendor_part_no  , //2
				   description  , //3
				 prev_values.qty_ordered  ,//4
				 qty_ordered  ,//5
				 qty_ordered - prev_values.qty_ordered  ,//6
				 prev_values.qty_received  ,//7
				 qty_received  ,//8
				 qty_received - prev_values.qty_received  ,//9
				 cost  ,//10
				 popprog_id  ,//11
				 woprog_id  ,//12
				 
				 this_member_id  ,//13
				 part_added  ,//14
				 part_deleted  ,//15
				   po_details_notes  ,//16
				   dateexpectedchange,//17
				   is_gl_account
			};
			Toolbox.doSQL_void(sql, paramObjects);

		}

		#region Part Tracking Emails
		//Send out Part Tracking Emails
		protected void PartTrackingEmails(string masterid, double amountrec, string notes)
		{
			//get members who are tracking this part
			var poprog = new NePOProg(po_details_poprog_id);
			var memberstable = Toolbox.doSQL_dt(@"SELECT DISTINCT(wo_detail_current_added_by) 
FROM wo_detail_current 
WHERE wo_detail_current_qty_committed < wo_detail_current_qty_ordered AND wo_detail_current_track_part = 1 AND wo_detail_current_master_id = @v0  AND business_unit_id =@v1  
GROUP BY wo_detail_current_added_by", new object[] { masterid, poprog.business_unit_id });


			var GLAccName = is_gl_account ? Toolbox.doSQL_string(@"SELECT IFNULL(MAX(gl_te.account_no), '') FROM gl_te WHERE gl_te.id = @v0 ", new object[] { po_details_woprog_id }) : "";
			var bvwo = "";
			if (po_details_woprog_id == 9999999 || po_details_woprog_id == 9999998 || po_details_woprog_id == 9999997)
				bvwo = "stock";
			else if (is_gl_account)
				bvwo = "GL Account-" + GLAccName;
			else
				bvwo = new NeWOProg(po_details_woprog_id).OrderNumber;

			foreach (DataRow memberrow in memberstable.Rows)
				{
				var informmember = new NeMember(Convert.ToInt32(memberrow[0]));

				var subject = string.Format(@"{0} {3}. Qty: {2} has been received on PO {1}" + Environment.NewLine + "Notes: ",
					masterid,
					poprog.poprog_bvpo,
					amountrec,
					po_details_description,
					bvwo, notes);


				if (informmember.NEEmail != "")
					{
					var mail = new NeEMail
								   {
								   To      = informmember.NEEmail,
								   Subject = subject,
								   From    = EmailID.Administrator + Toolbox.app_setting("DomainForEmail"),
								   Body    = ""
								   };
					try
						{
						mail.Send();
						}
					catch (Exception exce) { Toolbox.do_errorLog_errorStack(exce); }
					}
				}

			}
		#endregion

		public void MoveToWOProgDetails(string bvpo, string poprogid, string polineid, string memberid, double qtybeingadded, bool force = false)
			{
			try
				{
				var poprog = new NePOProg(Convert.ToInt32(poprogid));
				var inv_obj = new inventory();
				var WorkingBusinessUnit = new NeBusinessUnit(poprog.business_unit_id);
				var myMember = new NeMember(Convert.ToInt32(memberid));
				var Details = Toolbox.doSQL_dt(@"
									SELECT 
										po_details_id, 
										po_details_part_no, 
										po_details_vendor_part_no, 
										po_details_description, 
										po_details_woprog_id, 
										po_details_qty_ordered, 
										po_details_qty_received, 
										po_details_cost, 
										po_details_tax1, 
										po_details_tax2, 
										po_details_tax3, 
										po_details_tax4, 
										po_details_date_expected, 
										po_details_notes, 
										po_details_vendor_qty_per, 
										po_details_rec_no, 
										is_gl_account 
									FROM 
										po_details_current  
									WHERE 
										po_details_id = @v0 AND 
										to_wo = 0", new object[] { polineid });
				var lineid = "";
				var recno = "";
				var po_recno = "";
				var id = 0;
				var oldpartval = "";
				double oldcostval = 0;
				var oldcompanyval = "";
				double oldqtyperval = 0;
				double oldtotal = 0;
				double newrealcost = 0;
				var oldorigin = "";
				foreach (DataRow row in Details.Rows)
					{
					#region variable declaration
					var master_id = Convert.ToInt32(row["po_details_part_no"]);
					var vendor_code = row["po_details_vendor_part_no"].ToString();
					var __is_gl_account = (bool)row["is_gl_account"];
					var WarehouseBusinessUnit = new NeBusinessUnit(WorkingBusinessUnit.warehouse_bu_id);

					var po_cost = Convert.ToDouble(row["po_details_cost"]);
					var po_qty_per = Convert.ToDouble(row["po_details_vendor_qty_per"]);
					po_qty_per = po_qty_per == 0 ? 1 : po_qty_per;

					var po_description = row["po_details_description"].ToString();
					var po_woprog_id = Convert.ToInt32(row["po_details_woprog_id"]);
					po_recno = row["po_details_rec_no"].ToString();
					#endregion variable declaration
					#region Check Work Order Status and put in rework if needed
					var workorderstatus = Toolbox.doSQL_string(@"SELECT IFNULL(MAX(WOProg_Status), '') FROM woprog WHERE WOProg_ID = @v0 ", new object[] { po_woprog_id });
					if (workorderstatus == "")
						{
						continue;
						}
					newrealcost = Convert.ToDouble(row["po_details_cost"]) / Convert.ToDouble(row[14]);
					if (!__is_gl_account && !new List<string>(new[] {	OpsWOStatus.Open, 
																		OpsWOStatus.InitialPrep,
																		OpsWOStatus.Rework, 
																		OpsWOStatus.WaitingToBeInvoiced, 
																		OpsWOStatus.Invoiced
																		}).Contains(workorderstatus))
						{
						//Move This Work Order Into Reworks
						Toolbox.doSQL_void(@"UPDATE woprog SET WOProg_Status = 'Rework' WHERE WOProg_ID = @v0 and business_unit_id = @v1", new[] { row["po_details_woprog_id"], _business_unit_id});
						Toolbox.doSQL_void(@"INSERT INTO WOProgStatus (WOProgStatus_WOProg_ID,WOProgStatus_Member_ID,WOProgStatus_Status,WOProgStatus_DateTime)VALUES (@v0,@v1,'Rework',NOW())", new object[] {row["po_details_woprog_id"].ToString(), myMember.id});
						Toolbox.doSQL_void(@"INSERT INTO WOProgComment (WOProgComment_WOProg_ID,WOProgComment_Member_ID,WOProgComment_Text,WOProgComment_X,WOProgComment_Y,WOProgComment_DateTime) VALUES (@v0,@v1,CONCAT('This Work Order Moved to Rework by PO ',@v2),0,0,NOW())", new[] {row["po_details_woprog_id"], myMember.id, bvpo});
						}
					#endregion Check Work Order Status and put in rework if needed
					if (master_id != 0)
						{
						inv_obj.Load(master_id, WarehouseBusinessUnit.id);
						}
					#region Setting the Vendor And Branch Pricing 
					if (!inv_obj.is_exclude && inv_obj.Tag.id != OpsSpecialTag.GL && master_id != 0)
					{
						#region update vendor pricing
						var priceinfo = Toolbox.doSQL_dt(@"
											SELECT 
												id,
												vendor_code, 
												cost, 
												business_unit_id,
												qty, 
												total, 
												IFNULL(origin, '') origin 
											FROM 
												inventory_price 
											WHERE 
												master_id = @v0 AND 
												vendor_id = @v1 AND 
												vendor_code = @v2 AND 
												business_unit_id = @v3 ",
							new object[] {	master_id, 
											poprog.poprog_vendor_id, 
											vendor_code, 
											WarehouseBusinessUnit.id
											});
						foreach (DataRow price_row in priceinfo.Rows)
						{
							id = Convert.ToInt32(price_row["id"]);
						}

						var vprow = new vendor_price_row
						{
							price_id = id,
							master_id = master_id,
							member_id = myMember.id,
							vendor_code = vendor_code,
							business_unit_id = WarehouseBusinessUnit.id,
							cost = newrealcost,
							total = po_cost,
							qty = po_qty_per,
							vendor_id = poprog.poprog_vendor_id,
							origin = "PO: " + poprog.poprog_bvpo
						};
						vprow.save();
						#endregion update vendor pricing
					}
					#endregion

					try
					{
						if (master_id != 0)
						{
							//inv_obj.Load(master_id, _business_unit_id);
							inv_obj.Load(master_id, WarehouseBusinessUnit.id);

						}
						if (inv_obj.Tag.id != OpsSpecialTag.GL && master_id != 0) // NE GL part shall not pass.
						{
							//Add To Work Order
							var quote = "";
							var strwoid = "";
							var strbvwo = "";
							var recnumber = 0;
							int billtype;
							var _track = 0;
							var pm_id = 0;
							var MovementType = PartMovementClassification.NewWOPart;
							var use_fixed_markup = false;
							double fixed_markup = 1;
							var sales = new NeSalesOrder();
							var wo_header = Toolbox.doSQL_dt(@"
														SELECT 
															woprog_id,
															woprog_quoteid,
															woprog_bvwo,
															woprog_apply_discount,
															woprog_pm_memberid,
															use_fixed_material_markup,
															fixed_material_markup 
														FROM 
															woprog 
														WHERE 
															woprog_id = @v0", new object[] { po_woprog_id });
							foreach (DataRow wo_line in wo_header.Rows)
								{
								quote = wo_line["woprog_quoteid"].ToString();
								strwoid = wo_line["woprog_id"].ToString();
								strbvwo = wo_line["woprog_bvwo"].ToString();
								pm_id = Convert.ToInt32(wo_line["woprog_pm_memberid"]);
								use_fixed_markup = Convert.ToBoolean(wo_line["use_fixed_material_markup"]);
								fixed_markup = Convert.ToDouble(wo_line["fixed_material_markup"]);
								}
							billtype = quote == "0" 
										? OpsBillType.Regular 
										: OpsBillType.JobcostForQuote;
							var multiplyqty = po_qty_per;
							var woqty = qtybeingadded * multiplyqty;
							var sellpriceqty = woqty;
							double multiqty = 0;
							double QTYofParts = 0;
							// scan the work order for parts already on the work order.
							var TargetWODetail = Toolbox.doSQL_dt(@"
														SELECT 
															wo_detail_current_id id, 
															wo_detail_current_rec_no rec_no, 
															wo_detail_current_qty_committed qty_committed,
															wo_detail_current_track_part track_part
														FROM 
															wo_detail_current 
														WHERE
															wo_detail_current_woprog_id = @v0 AND 
															wo_detail_current_master_id = @v1 AND 
															wo_detail_current_master_id != 0", new object[] { strwoid, master_id });
							var rowCount = TargetWODetail.Rows.Count;
							var singleLine = rowCount == 1;
							var multiLine = rowCount > 1;
							if (singleLine && !inv_obj.is_exclude) // MH: This should always be the latter expression... a bit confusing.
								{
								var dr = TargetWODetail.Rows[0];
								lineid = dr["id"].ToString();
								recno = dr["rec_no"].ToString();
								QTYofParts += Convert.ToDouble(dr["qty_committed"]);
								sellpriceqty = woqty + QTYofParts;
								MovementType = PartMovementClassification.NonExclude.OneRowExists;
								_track = Convert.ToInt32(dr["track_part"]);
								if (_track == 1)
									{
									sales.TrackPart = _track;
									sales.trackmemberid = pm_id;
									}
								}
							else if (multiLine || inv_obj.is_exclude)
								{
								if (inv_obj.is_exclude)
									{    // set the default here to force a new line... but it will be overwritten if there are other parts from this very po line..
									sales.forcenewpartline = true;
									MovementType = PartMovementClassification.IsExclude.MoreThanOneRowExists;
									sales.PO_RECNO = po_recno;
									TargetWODetail = Toolbox.doSQL_dt(@"
														SELECT 
															wo_detail_current_id id, 
															wo_detail_current_rec_no rec_no, 
															wo_detail_current_qty_committed qty_committed 
														FROM 
															wo_detail_current 
														WHERE 
															wo_detail_current_woprog_id = @v0 AND 
															wo_detail_current_master_id = @v1 and 
															wo_detail_current_origin like CONCAT('%PO ',@v2,'-',@v3,'%')", new object[] { strwoid, master_id, poprog.poprog_bvpo, po_recno });
									if (TargetWODetail.Rows.Count > 0)
										{
										foreach (DataRow dr in TargetWODetail.Rows)
											{
											lineid = dr["id"].ToString();
											recno = dr["rec_no"].ToString();
											QTYofParts = Convert.ToDouble(dr["qty_committed"]);
											multiqty += QTYofParts;
											}
										sellpriceqty = woqty + QTYofParts;
										multiqty = multiqty + woqty;
										MovementType = PartMovementClassification.IsExclude.OneRowExists;
										}
									}
								else // MH: This should NEVER be hit?? Why is it here?
									{
									TargetWODetail	= Toolbox.doSQL_dt(@"
														SELECT 
															wo_detail_current_id id, 
															wo_detail_current_rec_no rec_no, 
															wo_detail_current_qty_committed qty_committed 
														FROM 
															wo_detail_current 
														WHERE 
															wo_detail_current_woprog_id = @v0 AND 
															wo_detail_current_master_id = @v1", new object[] { strwoid, master_id });
									foreach (DataRow dr in TargetWODetail.Rows)
										{
										lineid = dr["id"].ToString();
										recno = dr["rec_no"].ToString();
										QTYofParts = Convert.ToDouble(dr["qty_committed"]);
										multiqty += QTYofParts;
										}
									sellpriceqty = woqty + QTYofParts;
									multiqty += woqty;
									MovementType = PartMovementClassification.NonExclude.MoreThanOneExists;
									}
								}
							int.TryParse(lineid, out var int_line_id);
							var wodc = int_line_id > 0 
											? new NeWODetailCurrent(int_line_id) 
											: new NeWODetailCurrent();
							wodc.woprog_id = po_woprog_id;

							sales.PartNo = master_id.ToString();
							sales.ActualQuantity = sellpriceqty;
							sales.Quantity = woqty;
							sales.PO_RECNO = po_recno;
							var poCostPer = po_cost / po_qty_per;
							sales.CostOverRide = !inv_obj.is_exclude
													? Toolbox.doSQL_double(@"SELECT CALC_WO_MOVING_AVERAGE(@v0 , @v1 , @v2 , @v3 )", new object[] { po_woprog_id, master_id, woqty, poCostPer })
													: poCostPer;
							sales.SellOverride = use_fixed_markup
													? fixed_markup * sales.CostOverRide
													: shared.GetSellPrice(sales.CostOverRide, 0, inv_obj.is_qty, sellpriceqty, wodc.business_unit_id);
							sales.SellPrice = sales.SellOverride;
							sales.Desc = po_description;
							sales.BillingTypeID = billtype;
							sales.line_origin = $"PO {bvpo}-{po_recno} added:{woqty} {DateTime.Now:yyyy-MM-dd}\n";
							sales.POOrginInfo = $"PO {bvpo}-{po_recno} added:{woqty} {DateTime.Now:yyyy-MM-dd}\n";
							sales.OrderedQuantity = 0;
							sales.CustomerDiscount = 0;
							var bd = new bingo_data
							{
								before_cost = wodc.cost,
								before_qty = wodc.qty_committed,
								before_sell = wodc.sell,
								master_id = master_id,
								ca_cost = poCostPer,
								ca_qty = woqty,
								ca_sell = sales.SellOverride,
								after_cost = sales.CostOverRide,
								after_qty = woqty + QTYofParts,
								after_sell = sales.SellOverride,
								detail_id = wodc.id,
								woprog_id = po_woprog_id
							};
							bd.save();

							var qty_per = Convert.ToDouble(row["po_details_vendor_qty_per"]);
							qty_per = qty_per == 0 ? 1 : qty_per;
							try
								{
								switch (MovementType)
									{
									case PartMovementClassification.NewWOPart:
										try
											{
											sales.OrderedQuantity = Convert.ToDouble(row["po_details_qty_ordered"]) * qty_per;
											}
										catch (Exception exce) { Toolbox.do_errorLog_errorStack(exce); }
										sales.SavePart(wodc.woprog_id, "FALSE", "", myMember.FullName, myMember.id, true, "", force); 
									break;
									case PartMovementClassification.NonExclude.OneRowExists:
										sales.partlineid = lineid;
										sales.RecNum = recno;
										sales.SavePart(wodc.woprog_id, "FALSE", "", myMember.FullName, myMember.id, false, "", force);
									break;
									case PartMovementClassification.IsExclude.MoreThanOneRowExists:
										try
											{
											sales.OrderedQuantity = Convert.ToDouble(row["po_details_qty_ordered"]) * qty_per;
											}
										catch (Exception exce) { Toolbox.do_errorLog_errorStack(exce); }
										sales.SavePart(wodc.woprog_id, "FALSE", "", myMember.FullName, myMember.id, true, "", force);
									break;
									case PartMovementClassification.NonExclude.MoreThanOneExists:
										sales.SellPrice = use_fixed_markup ? fixed_markup * sales.CostOverRide : shared.GetSellPrice(poCostPer, 0, inv_obj.is_qty, multiqty, wodc.business_unit_id);
										sales.partlineid = lineid; // first instance of item on the work order
										sales.RecNum = recno;
										sales.SavePart(wodc.woprog_id, "FALSE", "", myMember.FullName, myMember.id, false, "", force);
										//Update All Parts with New Sell Price sales.RetailCost
										var cascade = new NeWODetailCurrent();
										cascade.sell = sales.SellPrice;
										cascade.unit = sales.SellPrice;
										var numbereffected = cascade.UpdateWODetailCurrentSellPriceCascade(strwoid, master_id.ToString());
									break;
									case PartMovementClassification.IsExclude.OneRowExists:
										sales.partlineid = lineid;
										sales.RecNum = recno;
										sales.SavePart(wodc.woprog_id, "FALSE", "", myMember.FullName, myMember.id, false, "", force);
									break;
									}
								try
									{
									NeWOProg.update_header_totals(strwoid, WarehouseBusinessUnit.id, strbvwo);
									}
								catch (Exception ee)
									{
									Toolbox.do_errorLog_errorStack(ee);
									}
							}
							catch (Exception ex)
							{
								var msg = bvpo + "There was a problem trying to add this part to the work order " + ex;
								throw;
							}
						}
					}
					catch (Exception ex1)
					{
						var msg = bvpo + "There was a problem trying to update the work order " + ex1;
						sendpartmessage(row["po_details_part_no"].ToString(), msg);
						throw;
					}
					//}
				}
			}
			catch (Exception ee)
			{
				Toolbox.do_errorLog_errorStack(ee);
				throw;
			}
		}
		private struct PartMovementClassification
			{
			/// <summary>
			/// 1
			/// </summary>
			public const int NewWOPart = 1;
			public struct NonExclude
				{
				/// <summary>
				/// 2
				/// </summary>
				public const int OneRowExists = 2;
				/// <summary>
				/// 4
				/// </summary>
				public const int MoreThanOneExists = 4;
				}
			public struct IsExclude
				{
				/// <summary>
				/// 3
				/// </summary>
				public const int MoreThanOneRowExists = 3;
				/// <summary>
				/// 5
				/// </summary>
				public const int OneRowExists = 5;
				}
			}
		protected void sendpartmessage(string part, string bvpo)
		{
			//Send Message concerning part
		var message_ = new NeEMail
						   {
						   From    = EmailID.Administrator + Toolbox.app_setting("DomainForEmail"),
						   To      = EmailID.Debug + Toolbox.app_setting("DomainForEmail"),
						   Subject = "Part " + part + " did not transfer",
						   Body    = "Part " + part + " on PO " + bvpo + " was not transferred"
						   };
		message_.Send();

		}

	}
}
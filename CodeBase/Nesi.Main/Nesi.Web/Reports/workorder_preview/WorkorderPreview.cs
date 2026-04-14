using System;
using DevExpress.Web;
using DevExpress.XtraReports.UI;
using System.Data;
using Nesi.Web.Reports.invoice_preview;

namespace nesi.core
	{
	/// <summary>
	/// Summary description for InvoicePreview
	/// </summary>
	/// 

	public class WorkOrderPreview : XtraReport
		{
		private DetailBand Detail; 
		private TopMarginBand TopMargin;
		private BottomMarginBand BottomMargin;
		private invoice_preview_ds invoice_preview_ds1;
		private Nesi.Web.Reports.invoice_preview.invoice_preview_dsTableAdapters.invoice_preview_dt_ta invoice_preview_dt_ta1; 
		public DevExpress.XtraReports.Parameters.Parameter @Param1;
		public DevExpress.XtraReports.Parameters.Parameter @brokenout;
		public int workorder_id		= 30771;
		private XRLabel branch_info;
		private XRPageInfo xrPageInfo1;
		private ReportFooterBand ReportFooter;
		private XRLabel net_amount;
		private XRLabel lbl_net_amount;
		private XRLabel lbl_sales_tax;
		private XRLabel sales_tax;
		private XRLabel lbl_total_due; 
		private XRLabel total_due;
		private PageHeaderBand PageHeader;
		private XRLabel total_material;
		private XRLabel total_material_lbl;
		private PageFooterBand PageFooter;
		private XRLabel lblremit;
		private XRLabel lblremitlabel;
		private XRLabel xrLabel3;
		private XRPictureBox xrPictureBox1;
		private XRLabel lbl_signoff;
		private int _broken_out=0;
		private XRLabel xrLabel4;
		private XRLabel line_amt;
		private XRLabel xrLabel10;
		private XRCrossBandBox xrCrossBandBox1;
		private XRLabel line_description;
		private XRPanel xrPanel1;
		private XRLabel attn;
		private XRLabel attn_name;
		private XRLabel customer_number;
		private XRLabel service_address;
		private XRLabel customer_address;
		private XRLabel lblsi;
		private XRLabel lbl_po_num;
		private XRLabel wo_number;
		private XRLabel lbl_wo_num;
		private XRLabel txtterms;
		private XRLabel lbl_terms;
		private XRLabel po_number;
		private XRLabel lbl_date;
		private XRLabel txtlocation;
		private XRLabel invoice_number;
		private XRLabel xrLabel11;
		private XRLabel lbl_invoice_num;
		private XRLabel txtDate;
		private XRLabel lbl_contact;
		private XRLabel txtContact;
		private XRLabel xrLabel7;
		private XRLabel xrLabel9;
		private XRLabel xrLabel8;
		private XRPanel pan_rates;
		private XRLine xrLine1;
		private XRLabel xrLabel27;
		private XRLabel xrLabel26;
		private XRLabel lb_tech_to;
		private XRLabel xrLabel24;
		private XRLabel lb_el_from;
		private XRLabel lb_app_from;
		private XRLabel xrLabel22;
		private XRLabel lb_el_to;
		private XRLabel lb_tech_from;
		private XRLabel lb_app_to;
		private XRLabel xrLabel17;
		private XRLabel xrLabel6;
		private XRPictureBox xrSignatureBox;
		private XRLabel lbl_work_completed;
		private XRTable lbl_pb_notes;
		private XRTableRow xrTableRow1;
		private XRTableCell xrTableCell2;
        private XRLabel lbl_tax_not_included;

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

		public WorkOrderPreview(int this_wo_id, int broken_out)
			{
			workorder_id			= this_wo_id;
			_broken_out				= broken_out;
			InitializeComponent();
			invoice_preview_dt_ta1.Fill(invoice_preview_ds1.invoice_preview_dt, workorder_id, broken_out);

			if (broken_out == 4)
				{
				total_material_lbl.Visible = false;
				total_material.Visible = false;
			
				}

			var wo = new NeWOProg(workorder_id);
			var _date = wo.woprog_InvoiceDate.ToString("yyyy-MM-dd") == "2005-12-01" ? DateTime.Today : wo.woprog_InvoiceDate;

			var mt = new NeMemberType();
			var dt = mt.GetBaseRates(wo.business_unit_id.ToString(),_date);
			pan_rates.Visible	= false; 


			var df			= new dollar_figures(); 
			foreach (DataRow dr in dt.Rows) 
				{
				var type_id				= (int)dr["membertype_chargeout_history_membertype_id"];
				var rate				= Convert.ToDouble(dr["membertype_chargeout_history_rate"]);
				// Per Ticket 3207 - April 15th
				// App From				= Lowest in the system for apprentices					-- ID = 18
				// App To				= Highest in the system + $10							-- ID = 22
				// Electrician From		= Lowest in the system for electricians					-- ID = 2
				// Electrician To		= Foreman / Project Mgr + $10
				// Foreman																	-- ID = 33
				// Project Mgr																-- ID = 4
				// Technical From		= Lowest in the system for PLC Programmer or Technician
				// PLC																		-- ID = 25
				// Technician																-- ID = 23
				// Technical To			= Automation Specialist + $10							-- ID = 17

				if (type_id == 2)
					{
					df.electrician_lowest	= rate;
					}
				if (type_id == 4)
					{
					df.projmgr_rate			= rate;
					}
				if (type_id == 17)
					{
					df.automation_rate		= rate;
					}
				if (type_id == 18)
					{
					df.apprentice_lowest	= rate;
					}
				if (type_id == 22)
					{
					df.apprentice_highest	= rate;
					}
				if(type_id == 23)
					{
					df.technician_rate		= rate;
					}
				if (type_id == 25)
					{
					df.plc_programmer_rate	= rate;
					}
				if(type_id == 33)
					{
					df.foreman_rate			= rate;
					}
				}

			// Only apply to: 
			//	Work orders that don't have a quote attached
			//	Child work orders
			//	Credit work orders
			if (	(wo.QuoteID == "0" || wo.QuoteID == "") && 
			    	wo.woprog_associate_woprog_id == 0 && 
			    	wo.woprog_iscredit == 0)
				{
				// Only fill out the rates if all rates have been supplied.
				df.apprentice_highest		= df.apprentice_highest + 10;
				df.electrician_highest		= df.foreman_rate > df.projmgr_rate ? df.foreman_rate + 10 : df.projmgr_rate + 10;
				df.technical_lowest			= df.plc_programmer_rate > df.technician_rate ? df.technician_rate : df.plc_programmer_rate;
				df.technical_highest		= df.automation_rate + 10;
				if(	df.apprentice_lowest > 0 && 
				   	df.apprentice_highest > 10 && 
				   	df.electrician_lowest > 0 && 
				   	df.electrician_highest > 10 &&
				   	df.technical_lowest > 0 &&
				   	df.technical_highest > 10)
					{
					pan_rates.Visible		= false; 
					lb_app_from.Text		= df.apprentice_lowest.ToString("C2");
					lb_app_to.Text			= df.apprentice_highest.ToString("C2");
					lb_el_from.Text			= df.electrician_lowest.ToString("C2");
					lb_el_to.Text			= df.electrician_highest.ToString("C2");
					lb_tech_from.Text		= df.technical_lowest.ToString("C2");
					lb_tech_to.Text			= df.technical_highest.ToString("C2");
					}
				}
			}
		private class dollar_figures
			{
			// Base Rates, these are what is outputted
			public double apprentice_lowest		{get;set;}
			public double apprentice_highest	{get;set;}	
			public double electrician_lowest	{get;set;}
			public double electrician_highest	{get;set;}
			public double technical_lowest		{get;set;}
			public double technical_highest		{get;set;}

			// Used when figuring out the base rates
			public double foreman_rate			{get;set;}
			public double projmgr_rate			{get;set;}
			public double plc_programmer_rate	{get;set;}
			public double technician_rate		{get;set;}
			public double automation_rate		{get;set;}
			}
		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose();
				}
			base.Dispose(disposing);
			}
		#region Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.line_description = new DevExpress.XtraReports.UI.XRLabel();
            this.line_amt = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel4 = new DevExpress.XtraReports.UI.XRLabel();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.invoice_preview_ds1 = new Nesi.Web.Reports.invoice_preview.invoice_preview_ds();
            this.invoice_preview_dt_ta1 = new Nesi.Web.Reports.invoice_preview.invoice_preview_dsTableAdapters.invoice_preview_dt_ta();
            this.xrPageInfo1 = new DevExpress.XtraReports.UI.XRPageInfo();
            this.branch_info = new DevExpress.XtraReports.UI.XRLabel();
            this.ReportFooter = new DevExpress.XtraReports.UI.ReportFooterBand();
            this.lbl_tax_not_included = new DevExpress.XtraReports.UI.XRLabel();
            this.lbl_pb_notes = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow1 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell2 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrSignatureBox = new DevExpress.XtraReports.UI.XRPictureBox();
            this.pan_rates = new DevExpress.XtraReports.UI.XRPanel();
            this.xrLine1 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLabel27 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel26 = new DevExpress.XtraReports.UI.XRLabel();
            this.lb_tech_to = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel24 = new DevExpress.XtraReports.UI.XRLabel();
            this.lb_el_from = new DevExpress.XtraReports.UI.XRLabel();
            this.lb_app_from = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel22 = new DevExpress.XtraReports.UI.XRLabel();
            this.lb_el_to = new DevExpress.XtraReports.UI.XRLabel();
            this.lb_tech_from = new DevExpress.XtraReports.UI.XRLabel();
            this.lb_app_to = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel17 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel6 = new DevExpress.XtraReports.UI.XRLabel();
            this.lbl_signoff = new DevExpress.XtraReports.UI.XRLabel();
            this.total_material_lbl = new DevExpress.XtraReports.UI.XRLabel();
            this.total_material = new DevExpress.XtraReports.UI.XRLabel();
            this.total_due = new DevExpress.XtraReports.UI.XRLabel();
            this.lbl_total_due = new DevExpress.XtraReports.UI.XRLabel();
            this.sales_tax = new DevExpress.XtraReports.UI.XRLabel();
            this.lbl_sales_tax = new DevExpress.XtraReports.UI.XRLabel();
            this.lbl_net_amount = new DevExpress.XtraReports.UI.XRLabel();
            this.net_amount = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel3 = new DevExpress.XtraReports.UI.XRLabel();
            this.lbl_work_completed = new DevExpress.XtraReports.UI.XRLabel();
            this.PageHeader = new DevExpress.XtraReports.UI.PageHeaderBand();
            this.xrLabel9 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel8 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel7 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel10 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrPictureBox1 = new DevExpress.XtraReports.UI.XRPictureBox();
            this.lblremit = new DevExpress.XtraReports.UI.XRLabel();
            this.lblremitlabel = new DevExpress.XtraReports.UI.XRLabel();
            this.xrPanel1 = new DevExpress.XtraReports.UI.XRPanel();
            this.lblsi = new DevExpress.XtraReports.UI.XRLabel();
            this.lbl_po_num = new DevExpress.XtraReports.UI.XRLabel();
            this.wo_number = new DevExpress.XtraReports.UI.XRLabel();
            this.lbl_wo_num = new DevExpress.XtraReports.UI.XRLabel();
            this.txtterms = new DevExpress.XtraReports.UI.XRLabel();
            this.lbl_terms = new DevExpress.XtraReports.UI.XRLabel();
            this.po_number = new DevExpress.XtraReports.UI.XRLabel();
            this.lbl_date = new DevExpress.XtraReports.UI.XRLabel();
            this.txtlocation = new DevExpress.XtraReports.UI.XRLabel();
            this.invoice_number = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel11 = new DevExpress.XtraReports.UI.XRLabel();
            this.lbl_invoice_num = new DevExpress.XtraReports.UI.XRLabel();
            this.txtDate = new DevExpress.XtraReports.UI.XRLabel();
            this.lbl_contact = new DevExpress.XtraReports.UI.XRLabel();
            this.txtContact = new DevExpress.XtraReports.UI.XRLabel();
            this.attn = new DevExpress.XtraReports.UI.XRLabel();
            this.attn_name = new DevExpress.XtraReports.UI.XRLabel();
            this.customer_number = new DevExpress.XtraReports.UI.XRLabel();
            this.service_address = new DevExpress.XtraReports.UI.XRLabel();
            this.customer_address = new DevExpress.XtraReports.UI.XRLabel();
            this.PageFooter = new DevExpress.XtraReports.UI.PageFooterBand();
            this.xrCrossBandBox1 = new DevExpress.XtraReports.UI.XRCrossBandBox();
            ((System.ComponentModel.ISupportInitialize)(this.invoice_preview_ds1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lbl_pb_notes)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // Detail
            // 
            this.Detail.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.line_description,
            this.line_amt,
            this.xrLabel4});
            this.Detail.HeightF = 25.08332F;
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.Detail.StylePriority.UseBorders = false;
            this.Detail.StylePriority.UseTextAlignment = false;
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // line_description
            // 
            this.line_description.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[description]")});
            this.line_description.Font = new System.Drawing.Font("Arial", 8F);
            this.line_description.KeepTogether = true;
            this.line_description.LocationFloat = new DevExpress.Utils.PointFloat(135F, 0F);
            this.line_description.Multiline = true;
            this.line_description.Name = "line_description";
            this.line_description.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.line_description.SizeF = new System.Drawing.SizeF(538.6837F, 20.91665F);
            this.line_description.StylePriority.UseFont = false;
            this.line_description.Text = "line_description";
            // 
            // line_amt
            // 
            this.line_amt.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[amount]"),
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "ForeColor", "Iif([amount] == 0, \'Transparent\', ?)"),
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Visible", "Iif([amount] == 0, \'False\', ?)")});
            this.line_amt.Font = new System.Drawing.Font("Arial", 9.75F);
            this.line_amt.LocationFloat = new DevExpress.Utils.PointFloat(686.4604F, 0F);
            this.line_amt.Name = "line_amt";
            this.line_amt.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.line_amt.SizeF = new System.Drawing.SizeF(100F, 20.91665F);
            this.line_amt.StylePriority.UseFont = false;
            this.line_amt.StylePriority.UseTextAlignment = false;
            this.line_amt.Text = "line_amt";
            this.line_amt.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.line_amt.TextFormatString = "{0:#,#.00}";
            // 
            // xrLabel4
            // 
            this.xrLabel4.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[qty]"),
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "ForeColor", "Iif([qty] == 0, \'Transparent\', ?)"),
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Visible", "Iif([qty] == 0, \'False\', ?)")});
            this.xrLabel4.Font = new System.Drawing.Font("Arial", 9.75F);
            this.xrLabel4.LocationFloat = new DevExpress.Utils.PointFloat(10.00001F, 0F);
            this.xrLabel4.Name = "xrLabel4";
            this.xrLabel4.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel4.SizeF = new System.Drawing.SizeF(81.24998F, 20.91665F);
            this.xrLabel4.StylePriority.UseFont = false;
            this.xrLabel4.StylePriority.UseTextAlignment = false;
            this.xrLabel4.Text = "xrLabel4";
            this.xrLabel4.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // TopMargin
            // 
            this.TopMargin.BackColor = System.Drawing.Color.Transparent;
            this.TopMargin.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.TopMargin.HeightF = 23F;
            this.TopMargin.Name = "TopMargin";
            this.TopMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.TopMargin.StylePriority.UseBackColor = false;
            this.TopMargin.StylePriority.UseBorders = false;
            this.TopMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // BottomMargin
            // 
            this.BottomMargin.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.BottomMargin.HeightF = 21F;
            this.BottomMargin.Name = "BottomMargin";
            this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.BottomMargin.StylePriority.UseBorders = false;
            this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // invoice_preview_ds1
            // 
            this.invoice_preview_ds1.DataSetName = "invoice_preview_ds";
            this.invoice_preview_ds1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // invoice_preview_dt_ta1
            // 
            this.invoice_preview_dt_ta1.ClearBeforeFill = true;
            // 
            // xrPageInfo1
            // 
            this.xrPageInfo1.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrPageInfo1.LocationFloat = new DevExpress.Utils.PointFloat(271.6017F, 25.33331F);
            this.xrPageInfo1.Name = "xrPageInfo1";
            this.xrPageInfo1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrPageInfo1.SizeF = new System.Drawing.SizeF(226.0408F, 23.00003F);
            this.xrPageInfo1.StylePriority.UseFont = false;
            this.xrPageInfo1.StylePriority.UseTextAlignment = false;
            this.xrPageInfo1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrPageInfo1.TextFormatString = "Page {0} of {1}";
            // 
            // branch_info
            // 
            this.branch_info.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.branch_info.LocationFloat = new DevExpress.Utils.PointFloat(237.2258F, 21.45831F);
            this.branch_info.Multiline = true;
            this.branch_info.Name = "branch_info";
            this.branch_info.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.branch_info.SizeF = new System.Drawing.SizeF(290.625F, 113.125F);
            this.branch_info.StylePriority.UseFont = false;
            this.branch_info.Text = "branch_info";
            // 
            // ReportFooter
            // 
            this.ReportFooter.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.ReportFooter.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.lbl_tax_not_included,
            this.lbl_pb_notes,
            this.xrSignatureBox,
            this.pan_rates,
            this.lbl_signoff,
            this.total_material_lbl,
            this.total_material,
            this.total_due,
            this.lbl_total_due,
            this.sales_tax,
            this.lbl_sales_tax,
            this.lbl_net_amount,
            this.net_amount,
            this.xrLabel3,
            this.lbl_work_completed});
            this.ReportFooter.HeightF = 215.7501F;
            this.ReportFooter.KeepTogether = true;
            this.ReportFooter.Name = "ReportFooter";
            this.ReportFooter.PageBreak = DevExpress.XtraReports.UI.PageBreak.AfterBand;
            this.ReportFooter.PrintAtBottom = true;
            this.ReportFooter.StylePriority.UseBorders = false;
            // 
            // lbl_tax_not_included
            // 
            this.lbl_tax_not_included.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.lbl_tax_not_included.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold);
            this.lbl_tax_not_included.LocationFloat = new DevExpress.Utils.PointFloat(178.8925F, 160.6251F);
            this.lbl_tax_not_included.Name = "lbl_tax_not_included";
            this.lbl_tax_not_included.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lbl_tax_not_included.SizeF = new System.Drawing.SizeF(492.7079F, 23F);
            this.lbl_tax_not_included.StylePriority.UseBorders = false;
            this.lbl_tax_not_included.StylePriority.UseFont = false;
            this.lbl_tax_not_included.StylePriority.UseTextAlignment = false;
            this.lbl_tax_not_included.Text = "Taxes not included in totals";
            this.lbl_tax_not_included.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // lbl_pb_notes
            // 
            this.lbl_pb_notes.Font = new System.Drawing.Font("Arial", 8.5F);
            this.lbl_pb_notes.LocationFloat = new DevExpress.Utils.PointFloat(63.12497F, 9.999974F);
            this.lbl_pb_notes.Name = "lbl_pb_notes";
            this.lbl_pb_notes.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow1});
            this.lbl_pb_notes.SizeF = new System.Drawing.SizeF(608.4755F, 9.375001F);
            this.lbl_pb_notes.StylePriority.UseFont = false;
            // 
            // xrTableRow1
            // 
            this.xrTableRow1.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell2});
            this.xrTableRow1.Name = "xrTableRow1";
            this.xrTableRow1.Weight = 1D;
            // 
            // xrTableCell2
            // 
            this.xrTableCell2.Font = new System.Drawing.Font("Arial", 9.5F);
            this.xrTableCell2.Multiline = true;
            this.xrTableCell2.Name = "xrTableCell2";
            this.xrTableCell2.StylePriority.UseFont = false;
            this.xrTableCell2.Weight = 1D;
            // 
            // xrSignatureBox
            // 
            this.xrSignatureBox.LocationFloat = new DevExpress.Utils.PointFloat(554.6851F, 125.4167F);
            this.xrSignatureBox.Name = "xrSignatureBox";
            this.xrSignatureBox.SizeF = new System.Drawing.SizeF(210.69F, 51.4583F);
            this.xrSignatureBox.Sizing = DevExpress.XtraPrinting.ImageSizeMode.Squeeze;
            this.xrSignatureBox.Visible = false;
            // 
            // pan_rates
            // 
            this.pan_rates.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLine1,
            this.xrLabel27,
            this.xrLabel26,
            this.lb_tech_to,
            this.xrLabel24,
            this.lb_el_from,
            this.lb_app_from,
            this.xrLabel22,
            this.lb_el_to,
            this.lb_tech_from,
            this.lb_app_to,
            this.xrLabel17,
            this.xrLabel6});
            this.pan_rates.LocationFloat = new DevExpress.Utils.PointFloat(37.85083F, 50.41663F);
            this.pan_rates.Name = "pan_rates";
            this.pan_rates.SizeF = new System.Drawing.SizeF(186.1074F, 75F);
            // 
            // xrLine1
            // 
            this.xrLine1.BorderColor = System.Drawing.Color.DimGray;
            this.xrLine1.BorderWidth = 0F;
            this.xrLine1.ForeColor = System.Drawing.Color.DarkGray;
            this.xrLine1.LineDirection = DevExpress.XtraReports.UI.LineDirection.Vertical;
            this.xrLine1.LocationFloat = new DevExpress.Utils.PointFloat(126.2084F, 17.22909F);
            this.xrLine1.Name = "xrLine1";
            this.xrLine1.SizeF = new System.Drawing.SizeF(7.350479F, 54.22923F);
            this.xrLine1.StylePriority.UseBorderColor = false;
            this.xrLine1.StylePriority.UseBorderWidth = false;
            this.xrLine1.StylePriority.UseForeColor = false;
            // 
            // xrLabel27
            // 
            this.xrLabel27.Font = new System.Drawing.Font("Arial", 8F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))));
            this.xrLabel27.LocationFloat = new DevExpress.Utils.PointFloat(133.5589F, 17.22909F);
            this.xrLabel27.Name = "xrLabel27";
            this.xrLabel27.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel27.SizeF = new System.Drawing.SizeF(43.75F, 13.64589F);
            this.xrLabel27.StylePriority.UseFont = false;
            this.xrLabel27.StylePriority.UseTextAlignment = false;
            this.xrLabel27.Text = "To";
            this.xrLabel27.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrLabel26
            // 
            this.xrLabel26.Font = new System.Drawing.Font("Arial", 8F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))));
            this.xrLabel26.LocationFloat = new DevExpress.Utils.PointFloat(82.80879F, 17.22909F);
            this.xrLabel26.Name = "xrLabel26";
            this.xrLabel26.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel26.SizeF = new System.Drawing.SizeF(43.39949F, 13.64589F);
            this.xrLabel26.StylePriority.UseFont = false;
            this.xrLabel26.StylePriority.UseTextAlignment = false;
            this.xrLabel26.Text = "From";
            this.xrLabel26.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // lb_tech_to
            // 
            this.lb_tech_to.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.lb_tech_to.Font = new System.Drawing.Font("Arial", 7.5F);
            this.lb_tech_to.LocationFloat = new DevExpress.Utils.PointFloat(133.5589F, 58.12499F);
            this.lb_tech_to.Name = "lb_tech_to";
            this.lb_tech_to.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lb_tech_to.SizeF = new System.Drawing.SizeF(44.45102F, 13.625F);
            this.lb_tech_to.StylePriority.UseBorders = false;
            this.lb_tech_to.StylePriority.UseFont = false;
            this.lb_tech_to.StylePriority.UseTextAlignment = false;
            this.lb_tech_to.Text = "tech_to";
            this.lb_tech_to.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrLabel24
            // 
            this.xrLabel24.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel24.Font = new System.Drawing.Font("Arial", 7.5F, System.Drawing.FontStyle.Bold);
            this.xrLabel24.LocationFloat = new DevExpress.Utils.PointFloat(4.750084F, 57.83336F);
            this.xrLabel24.Name = "xrLabel24";
            this.xrLabel24.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel24.SizeF = new System.Drawing.SizeF(77.70828F, 13.62497F);
            this.xrLabel24.StylePriority.UseBorders = false;
            this.xrLabel24.StylePriority.UseFont = false;
            this.xrLabel24.StylePriority.UseTextAlignment = false;
            this.xrLabel24.Text = "Technical:";
            this.xrLabel24.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // lb_el_from
            // 
            this.lb_el_from.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.lb_el_from.Font = new System.Drawing.Font("Arial", 7.5F);
            this.lb_el_from.LocationFloat = new DevExpress.Utils.PointFloat(82.4584F, 44.20835F);
            this.lb_el_from.Name = "lb_el_from";
            this.lb_el_from.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lb_el_from.SizeF = new System.Drawing.SizeF(43.74998F, 13.625F);
            this.lb_el_from.StylePriority.UseBorders = false;
            this.lb_el_from.StylePriority.UseFont = false;
            this.lb_el_from.StylePriority.UseTextAlignment = false;
            this.lb_el_from.Text = "er_from";
            this.lb_el_from.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // lb_app_from
            // 
            this.lb_app_from.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.lb_app_from.Font = new System.Drawing.Font("Arial", 7.5F);
            this.lb_app_from.LocationFloat = new DevExpress.Utils.PointFloat(82.80879F, 30.87496F);
            this.lb_app_from.Name = "lb_app_from";
            this.lb_app_from.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lb_app_from.SizeF = new System.Drawing.SizeF(43.39955F, 13.33338F);
            this.lb_app_from.StylePriority.UseBorders = false;
            this.lb_app_from.StylePriority.UseFont = false;
            this.lb_app_from.StylePriority.UseTextAlignment = false;
            this.lb_app_from.Text = "app_from";
            this.lb_app_from.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrLabel22
            // 
            this.xrLabel22.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel22.Font = new System.Drawing.Font("Arial", 7.5F, System.Drawing.FontStyle.Bold);
            this.xrLabel22.LocationFloat = new DevExpress.Utils.PointFloat(4.750084F, 30.87496F);
            this.xrLabel22.Name = "xrLabel22";
            this.xrLabel22.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel22.SizeF = new System.Drawing.SizeF(77.70828F, 13.33338F);
            this.xrLabel22.StylePriority.UseBorders = false;
            this.xrLabel22.StylePriority.UseFont = false;
            this.xrLabel22.StylePriority.UseTextAlignment = false;
            this.xrLabel22.Text = "Apprentice:";
            this.xrLabel22.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // lb_el_to
            // 
            this.lb_el_to.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.lb_el_to.Font = new System.Drawing.Font("Arial", 7.5F);
            this.lb_el_to.LocationFloat = new DevExpress.Utils.PointFloat(133.5589F, 44.49998F);
            this.lb_el_to.Name = "lb_el_to";
            this.lb_el_to.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lb_el_to.SizeF = new System.Drawing.SizeF(44.45102F, 13.33339F);
            this.lb_el_to.StylePriority.UseBorders = false;
            this.lb_el_to.StylePriority.UseFont = false;
            this.lb_el_to.StylePriority.UseTextAlignment = false;
            this.lb_el_to.Text = "er_to";
            this.lb_el_to.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // lb_tech_from
            // 
            this.lb_tech_from.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.lb_tech_from.Font = new System.Drawing.Font("Arial", 7.5F);
            this.lb_tech_from.LocationFloat = new DevExpress.Utils.PointFloat(82.80901F, 58.12494F);
            this.lb_tech_from.Name = "lb_tech_from";
            this.lb_tech_from.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lb_tech_from.SizeF = new System.Drawing.SizeF(43.39944F, 13.62501F);
            this.lb_tech_from.StylePriority.UseBorders = false;
            this.lb_tech_from.StylePriority.UseFont = false;
            this.lb_tech_from.StylePriority.UseTextAlignment = false;
            this.lb_tech_from.Text = "tech_from";
            this.lb_tech_from.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // lb_app_to
            // 
            this.lb_app_to.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.lb_app_to.Font = new System.Drawing.Font("Arial", 7.5F);
            this.lb_app_to.LocationFloat = new DevExpress.Utils.PointFloat(133.5589F, 30.87502F);
            this.lb_app_to.Name = "lb_app_to";
            this.lb_app_to.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lb_app_to.SizeF = new System.Drawing.SizeF(44.10048F, 13.625F);
            this.lb_app_to.StylePriority.UseBorders = false;
            this.lb_app_to.StylePriority.UseFont = false;
            this.lb_app_to.StylePriority.UseTextAlignment = false;
            this.lb_app_to.Text = "app_to";
            this.lb_app_to.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrLabel17
            // 
            this.xrLabel17.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel17.Font = new System.Drawing.Font("Arial", 7.5F, System.Drawing.FontStyle.Bold);
            this.xrLabel17.LocationFloat = new DevExpress.Utils.PointFloat(4.750084F, 44.20835F);
            this.xrLabel17.Name = "xrLabel17";
            this.xrLabel17.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel17.SizeF = new System.Drawing.SizeF(77.70828F, 13.625F);
            this.xrLabel17.StylePriority.UseBorders = false;
            this.xrLabel17.StylePriority.UseFont = false;
            this.xrLabel17.StylePriority.UseTextAlignment = false;
            this.xrLabel17.Text = "Electrician:";
            this.xrLabel17.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // xrLabel6
            // 
            this.xrLabel6.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel6.Font = new System.Drawing.Font("Arial", 7.5F, System.Drawing.FontStyle.Bold);
            this.xrLabel6.LocationFloat = new DevExpress.Utils.PointFloat(4.468792F, 3.250013F);
            this.xrLabel6.Name = "xrLabel6";
            this.xrLabel6.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel6.SizeF = new System.Drawing.SizeF(121.7395F, 13.625F);
            this.xrLabel6.StylePriority.UseBorders = false;
            this.xrLabel6.StylePriority.UseFont = false;
            this.xrLabel6.StylePriority.UseTextAlignment = false;
            this.xrLabel6.Text = "Current Rates ($/Hr):";
            this.xrLabel6.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // lbl_signoff
            // 
            this.lbl_signoff.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.lbl_signoff.Font = new System.Drawing.Font("Arial", 8F);
            this.lbl_signoff.LocationFloat = new DevExpress.Utils.PointFloat(42.60091F, 183.6252F);
            this.lbl_signoff.Multiline = true;
            this.lbl_signoff.Name = "lbl_signoff";
            this.lbl_signoff.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lbl_signoff.SizeF = new System.Drawing.SizeF(696.7077F, 32.12486F);
            this.lbl_signoff.StylePriority.UseBorders = false;
            this.lbl_signoff.StylePriority.UseFont = false;
            // 
            // total_material_lbl
            // 
            this.total_material_lbl.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.total_material_lbl.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.total_material_lbl.LocationFloat = new DevExpress.Utils.PointFloat(573.6851F, 30.83331F);
            this.total_material_lbl.Name = "total_material_lbl";
            this.total_material_lbl.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.total_material_lbl.SizeF = new System.Drawing.SizeF(97.91577F, 23F);
            this.total_material_lbl.StylePriority.UseBorders = false;
            this.total_material_lbl.StylePriority.UseFont = false;
            this.total_material_lbl.StylePriority.UseTextAlignment = false;
            this.total_material_lbl.Text = "Total Material: ";
            this.total_material_lbl.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomRight;
            this.total_material_lbl.Visible = false;
            // 
            // total_material
            // 
            this.total_material.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.total_material.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.total_material.LocationFloat = new DevExpress.Utils.PointFloat(684.3771F, 30.83331F);
            this.total_material.Name = "total_material";
            this.total_material.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.total_material.SizeF = new System.Drawing.SizeF(103.5394F, 23F);
            this.total_material.StylePriority.UseBorders = false;
            this.total_material.StylePriority.UseFont = false;
            this.total_material.StylePriority.UseTextAlignment = false;
            this.total_material.Text = "total_material";
            this.total_material.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomRight;
            this.total_material.Visible = false;
            // 
            // total_due
            // 
            this.total_due.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.total_due.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold);
            this.total_due.LocationFloat = new DevExpress.Utils.PointFloat(684.3771F, 107.4167F);
            this.total_due.Name = "total_due";
            this.total_due.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.total_due.SizeF = new System.Drawing.SizeF(103.5395F, 22.99995F);
            this.total_due.StylePriority.UseBorders = false;
            this.total_due.StylePriority.UseFont = false;
            this.total_due.StylePriority.UseTextAlignment = false;
            this.total_due.Text = "total_due";
            this.total_due.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // lbl_total_due
            // 
            this.lbl_total_due.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.lbl_total_due.Font = new System.Drawing.Font("Arial", 9.75F);
            this.lbl_total_due.LocationFloat = new DevExpress.Utils.PointFloat(571.6008F, 107.4167F);
            this.lbl_total_due.Name = "lbl_total_due";
            this.lbl_total_due.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lbl_total_due.SizeF = new System.Drawing.SizeF(100F, 23F);
            this.lbl_total_due.StylePriority.UseBorders = false;
            this.lbl_total_due.StylePriority.UseFont = false;
            this.lbl_total_due.StylePriority.UseTextAlignment = false;
            this.lbl_total_due.Text = "TOTAL DUE:";
            this.lbl_total_due.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // sales_tax
            // 
            this.sales_tax.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.sales_tax.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sales_tax.LocationFloat = new DevExpress.Utils.PointFloat(684.377F, 84.41658F);
            this.sales_tax.Name = "sales_tax";
            this.sales_tax.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.sales_tax.SizeF = new System.Drawing.SizeF(103.5396F, 22.99998F);
            this.sales_tax.StylePriority.UseBorders = false;
            this.sales_tax.StylePriority.UseFont = false;
            this.sales_tax.StylePriority.UseTextAlignment = false;
            this.sales_tax.Text = "sales_tax";
            this.sales_tax.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.sales_tax.Visible = false;
            // 
            // lbl_sales_tax
            // 
            this.lbl_sales_tax.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.lbl_sales_tax.Font = new System.Drawing.Font("Arial", 9.75F);
            this.lbl_sales_tax.LocationFloat = new DevExpress.Utils.PointFloat(248.9583F, 84.41658F);
            this.lbl_sales_tax.Name = "lbl_sales_tax";
            this.lbl_sales_tax.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lbl_sales_tax.SizeF = new System.Drawing.SizeF(422.6426F, 22.99998F);
            this.lbl_sales_tax.StylePriority.UseBorders = false;
            this.lbl_sales_tax.StylePriority.UseFont = false;
            this.lbl_sales_tax.StylePriority.UseTextAlignment = false;
            this.lbl_sales_tax.Text = "SALES TAX:";
            this.lbl_sales_tax.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.lbl_sales_tax.Visible = false;
            // 
            // lbl_net_amount
            // 
            this.lbl_net_amount.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.lbl_net_amount.Font = new System.Drawing.Font("Arial", 9.75F);
            this.lbl_net_amount.LocationFloat = new DevExpress.Utils.PointFloat(571.6008F, 61.41669F);
            this.lbl_net_amount.Name = "lbl_net_amount";
            this.lbl_net_amount.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lbl_net_amount.SizeF = new System.Drawing.SizeF(100F, 23F);
            this.lbl_net_amount.StylePriority.UseBorders = false;
            this.lbl_net_amount.StylePriority.UseFont = false;
            this.lbl_net_amount.StylePriority.UseTextAlignment = false;
            this.lbl_net_amount.Text = "NET AMOUNT:";
            this.lbl_net_amount.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // net_amount
            // 
            this.net_amount.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.net_amount.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold);
            this.net_amount.LocationFloat = new DevExpress.Utils.PointFloat(684.377F, 62.58329F);
            this.net_amount.Name = "net_amount";
            this.net_amount.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.net_amount.SizeF = new System.Drawing.SizeF(103.5396F, 21.83335F);
            this.net_amount.StylePriority.UseBorders = false;
            this.net_amount.StylePriority.UseFont = false;
            this.net_amount.StylePriority.UseTextAlignment = false;
            this.net_amount.Text = "net_amount";
            this.net_amount.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // xrLabel3
            // 
            this.xrLabel3.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel3.Font = new System.Drawing.Font("Arial", 7F);
            this.xrLabel3.LocationFloat = new DevExpress.Utils.PointFloat(176.8092F, 137.6251F);
            this.xrLabel3.Name = "xrLabel3";
            this.xrLabel3.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel3.SizeF = new System.Drawing.SizeF(494.7913F, 23F);
            this.xrLabel3.StylePriority.UseBorders = false;
            this.xrLabel3.StylePriority.UseFont = false;
            this.xrLabel3.StylePriority.UseTextAlignment = false;
            this.xrLabel3.Text = "2% per month charged on Overdue Accounts, 24% per annum";
            this.xrLabel3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // lbl_work_completed
            // 
            this.lbl_work_completed.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_work_completed.LocationFloat = new DevExpress.Utils.PointFloat(63.12501F, 4.791641F);
            this.lbl_work_completed.Multiline = true;
            this.lbl_work_completed.Name = "lbl_work_completed";
            this.lbl_work_completed.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lbl_work_completed.SizeF = new System.Drawing.SizeF(609.5174F, 3.625044F);
            this.lbl_work_completed.StylePriority.UseFont = false;
            // 
            // PageHeader
            // 
            this.PageHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel9,
            this.xrLabel8,
            this.xrLabel7,
            this.xrLabel10,
            this.xrPictureBox1,
            this.lblremit,
            this.lblremitlabel,
            this.branch_info,
            this.xrPanel1});
            this.PageHeader.HeightF = 395.0001F;
            this.PageHeader.Name = "PageHeader";
            // 
            // xrLabel9
            // 
            this.xrLabel9.BackColor = System.Drawing.Color.DarkBlue;
            this.xrLabel9.Font = new System.Drawing.Font("Arial", 9.75F);
            this.xrLabel9.ForeColor = System.Drawing.Color.White;
            this.xrLabel9.LocationFloat = new DevExpress.Utils.PointFloat(673.6841F, 370.625F);
            this.xrLabel9.Name = "xrLabel9";
            this.xrLabel9.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel9.SizeF = new System.Drawing.SizeF(125.274F, 24.37506F);
            this.xrLabel9.StylePriority.UseBackColor = false;
            this.xrLabel9.StylePriority.UseFont = false;
            this.xrLabel9.StylePriority.UseForeColor = false;
            this.xrLabel9.StylePriority.UseTextAlignment = false;
            this.xrLabel9.Text = "Amount";
            this.xrLabel9.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrLabel8
            // 
            this.xrLabel8.BackColor = System.Drawing.Color.DarkBlue;
            this.xrLabel8.Font = new System.Drawing.Font("Arial", 9.75F);
            this.xrLabel8.ForeColor = System.Drawing.Color.White;
            this.xrLabel8.LocationFloat = new DevExpress.Utils.PointFloat(136.0416F, 370.625F);
            this.xrLabel8.Name = "xrLabel8";
            this.xrLabel8.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel8.SizeF = new System.Drawing.SizeF(537.6426F, 24.37503F);
            this.xrLabel8.StylePriority.UseBackColor = false;
            this.xrLabel8.StylePriority.UseFont = false;
            this.xrLabel8.StylePriority.UseForeColor = false;
            this.xrLabel8.StylePriority.UseTextAlignment = false;
            this.xrLabel8.Text = "Description";
            this.xrLabel8.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrLabel7
            // 
            this.xrLabel7.BackColor = System.Drawing.Color.DarkBlue;
            this.xrLabel7.Font = new System.Drawing.Font("Arial", 9.75F);
            this.xrLabel7.ForeColor = System.Drawing.Color.White;
            this.xrLabel7.LocationFloat = new DevExpress.Utils.PointFloat(2.08327F, 370.625F);
            this.xrLabel7.Name = "xrLabel7";
            this.xrLabel7.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel7.SizeF = new System.Drawing.SizeF(133.9583F, 24.37506F);
            this.xrLabel7.StylePriority.UseBackColor = false;
            this.xrLabel7.StylePriority.UseFont = false;
            this.xrLabel7.StylePriority.UseForeColor = false;
            this.xrLabel7.StylePriority.UseTextAlignment = false;
            this.xrLabel7.Text = "Qty";
            this.xrLabel7.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrLabel10
            // 
            this.xrLabel10.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Bold);
            this.xrLabel10.LocationFloat = new DevExpress.Utils.PointFloat(178.8925F, 317.7083F);
            this.xrLabel10.Name = "xrLabel10";
            this.xrLabel10.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel10.SizeF = new System.Drawing.SizeF(450.6909F, 23.58264F);
            this.xrLabel10.StylePriority.UseFont = false;
            this.xrLabel10.StylePriority.UseTextAlignment = false;
            this.xrLabel10.Text = "Work Order Preview";
            this.xrLabel10.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrPictureBox1
            // 
            this.xrPictureBox1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 21.45831F);
            this.xrPictureBox1.Name = "xrPictureBox1";
            this.xrPictureBox1.SizeF = new System.Drawing.SizeF(214.7257F, 113.125F);
            this.xrPictureBox1.Sizing = DevExpress.XtraPrinting.ImageSizeMode.ZoomImage;
            // 
            // lblremit
            // 
            this.lblremit.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblremit.LocationFloat = new DevExpress.Utils.PointFloat(527.8508F, 37.70831F);
            this.lblremit.Multiline = true;
            this.lblremit.Name = "lblremit";
            this.lblremit.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblremit.SizeF = new System.Drawing.SizeF(272.1489F, 96.875F);
            this.lblremit.StylePriority.UseFont = false;
            this.lblremit.StylePriority.UseTextAlignment = false;
            this.lblremit.Text = "SparkPower Corporation.\r\n1345 Heine Court\r\nBurlington, Ontar" +
    "io, Canada\r\nL7L 6A7\r\n740696695\r\n";
            this.lblremit.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            // 
            // lblremitlabel
            // 
            this.lblremitlabel.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold);
            this.lblremitlabel.LocationFloat = new DevExpress.Utils.PointFloat(613.2673F, 21.45831F);
            this.lblremitlabel.Multiline = true;
            this.lblremitlabel.Name = "lblremitlabel";
            this.lblremitlabel.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblremitlabel.SizeF = new System.Drawing.SizeF(186.7325F, 16.25001F);
            this.lblremitlabel.StylePriority.UseFont = false;
            this.lblremitlabel.StylePriority.UseTextAlignment = false;
            this.lblremitlabel.Text = "REMIT PAYMENTS TO:";
            this.lblremitlabel.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            // 
            // xrPanel1
            // 
            this.xrPanel1.BackColor = System.Drawing.Color.Transparent;
            this.xrPanel1.BorderColor = System.Drawing.SystemColors.Control;
            this.xrPanel1.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrPanel1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.lblsi,
            this.lbl_po_num,
            this.wo_number,
            this.lbl_wo_num,
            this.txtterms,
            this.lbl_terms,
            this.po_number,
            this.lbl_date,
            this.txtlocation,
            this.invoice_number,
            this.xrLabel11,
            this.lbl_invoice_num,
            this.txtDate,
            this.lbl_contact,
            this.txtContact,
            this.attn,
            this.attn_name,
            this.customer_number,
            this.service_address,
            this.customer_address});
            this.xrPanel1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 142.7083F);
            this.xrPanel1.Name = "xrPanel1";
            this.xrPanel1.SizeF = new System.Drawing.SizeF(799.9998F, 170.8333F);
            this.xrPanel1.StylePriority.UseBackColor = false;
            this.xrPanel1.StylePriority.UseBorderColor = false;
            this.xrPanel1.StylePriority.UseBorders = false;
            // 
            // lblsi
            // 
            this.lblsi.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.lblsi.Font = new System.Drawing.Font("Arial", 9F);
            this.lblsi.LocationFloat = new DevExpress.Utils.PointFloat(497.4999F, 129.1257F);
            this.lblsi.Multiline = true;
            this.lblsi.Name = "lblsi";
            this.lblsi.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblsi.SizeF = new System.Drawing.SizeF(301.4582F, 35.04169F);
            this.lblsi.StylePriority.UseBorders = false;
            this.lblsi.StylePriority.UseFont = false;
            // 
            // lbl_po_num
            // 
            this.lbl_po_num.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.lbl_po_num.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)), true);
            this.lbl_po_num.LocationFloat = new DevExpress.Utils.PointFloat(612.2259F, 93.70886F);
            this.lbl_po_num.Name = "lbl_po_num";
            this.lbl_po_num.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lbl_po_num.SizeF = new System.Drawing.SizeF(60.41669F, 17.70844F);
            this.lbl_po_num.StylePriority.UseBorders = false;
            this.lbl_po_num.StylePriority.UseFont = false;
            this.lbl_po_num.StylePriority.UseTextAlignment = false;
            this.lbl_po_num.Text = "P.O. #:";
            this.lbl_po_num.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // wo_number
            // 
            this.wo_number.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.wo_number.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.wo_number.LocationFloat = new DevExpress.Utils.PointFloat(672.6425F, 76.0004F);
            this.wo_number.Name = "wo_number";
            this.wo_number.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.wo_number.SizeF = new System.Drawing.SizeF(126.3156F, 17.7084F);
            this.wo_number.StylePriority.UseBorders = false;
            this.wo_number.StylePriority.UseFont = false;
            this.wo_number.StylePriority.UseTextAlignment = false;
            this.wo_number.Text = "wo_number";
            this.wo_number.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // lbl_wo_num
            // 
            this.lbl_wo_num.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.lbl_wo_num.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_wo_num.LocationFloat = new DevExpress.Utils.PointFloat(612.2259F, 76.0004F);
            this.lbl_wo_num.Name = "lbl_wo_num";
            this.lbl_wo_num.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lbl_wo_num.SizeF = new System.Drawing.SizeF(60.41669F, 17.70844F);
            this.lbl_wo_num.StylePriority.UseBorders = false;
            this.lbl_wo_num.StylePriority.UseFont = false;
            this.lbl_wo_num.StylePriority.UseTextAlignment = false;
            this.lbl_wo_num.Text = "W.O. #:";
            this.lbl_wo_num.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // txtterms
            // 
            this.txtterms.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.txtterms.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtterms.LocationFloat = new DevExpress.Utils.PointFloat(672.6425F, 111.4173F);
            this.txtterms.Name = "txtterms";
            this.txtterms.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.txtterms.SizeF = new System.Drawing.SizeF(126.3157F, 17.70833F);
            this.txtterms.StylePriority.UseBorders = false;
            this.txtterms.StylePriority.UseFont = false;
            this.txtterms.StylePriority.UseTextAlignment = false;
            this.txtterms.Text = "Net 30 Days";
            this.txtterms.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // lbl_terms
            // 
            this.lbl_terms.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.lbl_terms.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)), true);
            this.lbl_terms.LocationFloat = new DevExpress.Utils.PointFloat(612.2258F, 111.4173F);
            this.lbl_terms.Name = "lbl_terms";
            this.lbl_terms.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lbl_terms.SizeF = new System.Drawing.SizeF(60.41669F, 17.7084F);
            this.lbl_terms.StylePriority.UseBorders = false;
            this.lbl_terms.StylePriority.UseFont = false;
            this.lbl_terms.StylePriority.UseTextAlignment = false;
            this.lbl_terms.Text = "Terms:";
            this.lbl_terms.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // po_number
            // 
            this.po_number.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.po_number.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.po_number.LocationFloat = new DevExpress.Utils.PointFloat(672.6425F, 93.7088F);
            this.po_number.Name = "po_number";
            this.po_number.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.po_number.SizeF = new System.Drawing.SizeF(126.3156F, 17.7084F);
            this.po_number.StylePriority.UseBorders = false;
            this.po_number.StylePriority.UseFont = false;
            this.po_number.StylePriority.UseTextAlignment = false;
            this.po_number.Text = "po_number";
            this.po_number.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // lbl_date
            // 
            this.lbl_date.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.lbl_date.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_date.LocationFloat = new DevExpress.Utils.PointFloat(612.2258F, 40.58348F);
            this.lbl_date.Name = "lbl_date";
            this.lbl_date.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lbl_date.SizeF = new System.Drawing.SizeF(60.41669F, 17.70842F);
            this.lbl_date.StylePriority.UseBorders = false;
            this.lbl_date.StylePriority.UseFont = false;
            this.lbl_date.StylePriority.UseTextAlignment = false;
            this.lbl_date.Text = "Date:";
            this.lbl_date.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // txtlocation
            // 
            this.txtlocation.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.txtlocation.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtlocation.LocationFloat = new DevExpress.Utils.PointFloat(672.6425F, 22.87499F);
            this.txtlocation.Name = "txtlocation";
            this.txtlocation.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.txtlocation.SizeF = new System.Drawing.SizeF(126.3156F, 16.99959F);
            this.txtlocation.StylePriority.UseBorders = false;
            this.txtlocation.StylePriority.UseFont = false;
            this.txtlocation.StylePriority.UseTextAlignment = false;
            this.txtlocation.Text = " ";
            this.txtlocation.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // invoice_number
            // 
            this.invoice_number.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.invoice_number.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.invoice_number.LocationFloat = new DevExpress.Utils.PointFloat(672.6425F, 58.29201F);
            this.invoice_number.Name = "invoice_number";
            this.invoice_number.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.invoice_number.SizeF = new System.Drawing.SizeF(126.3156F, 17.70839F);
            this.invoice_number.StylePriority.UseBorders = false;
            this.invoice_number.StylePriority.UseFont = false;
            this.invoice_number.StylePriority.UseTextAlignment = false;
            this.invoice_number.Text = "##########";
            this.invoice_number.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // xrLabel11
            // 
            this.xrLabel11.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel11.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel11.LocationFloat = new DevExpress.Utils.PointFloat(561.1846F, 22.87502F);
            this.xrLabel11.Name = "xrLabel11";
            this.xrLabel11.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel11.SizeF = new System.Drawing.SizeF(111.4579F, 16.99957F);
            this.xrLabel11.StylePriority.UseBorders = false;
            this.xrLabel11.StylePriority.UseFont = false;
            this.xrLabel11.StylePriority.UseTextAlignment = false;
            this.xrLabel11.Text = "Location in Plant:";
            this.xrLabel11.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // lbl_invoice_num
            // 
            this.lbl_invoice_num.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.lbl_invoice_num.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_invoice_num.LocationFloat = new DevExpress.Utils.PointFloat(612.2259F, 58.29188F);
            this.lbl_invoice_num.Name = "lbl_invoice_num";
            this.lbl_invoice_num.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lbl_invoice_num.SizeF = new System.Drawing.SizeF(60.41669F, 17.70839F);
            this.lbl_invoice_num.StylePriority.UseBorders = false;
            this.lbl_invoice_num.StylePriority.UseFont = false;
            this.lbl_invoice_num.StylePriority.UseTextAlignment = false;
            this.lbl_invoice_num.Text = "Invoice:";
            this.lbl_invoice_num.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // txtDate
            // 
            this.txtDate.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.txtDate.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDate.LocationFloat = new DevExpress.Utils.PointFloat(672.6425F, 40.58348F);
            this.txtDate.Name = "txtDate";
            this.txtDate.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.txtDate.SizeF = new System.Drawing.SizeF(126.3156F, 17.7084F);
            this.txtDate.StylePriority.UseBorders = false;
            this.txtDate.StylePriority.UseFont = false;
            this.txtDate.StylePriority.UseTextAlignment = false;
            this.txtDate.Text = "txtDate";
            this.txtDate.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // lbl_contact
            // 
            this.lbl_contact.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.lbl_contact.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_contact.LocationFloat = new DevExpress.Utils.PointFloat(561.1846F, 5.166626F);
            this.lbl_contact.Name = "lbl_contact";
            this.lbl_contact.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lbl_contact.SizeF = new System.Drawing.SizeF(111.4579F, 17.70842F);
            this.lbl_contact.StylePriority.UseBorders = false;
            this.lbl_contact.StylePriority.UseFont = false;
            this.lbl_contact.StylePriority.UseTextAlignment = false;
            this.lbl_contact.Text = "On Site Contact:";
            this.lbl_contact.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // txtContact
            // 
            this.txtContact.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.txtContact.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtContact.LocationFloat = new DevExpress.Utils.PointFloat(672.6425F, 5.166689F);
            this.txtContact.Name = "txtContact";
            this.txtContact.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.txtContact.SizeF = new System.Drawing.SizeF(126.3156F, 17.7084F);
            this.txtContact.StylePriority.UseBorders = false;
            this.txtContact.StylePriority.UseFont = false;
            this.txtContact.StylePriority.UseTextAlignment = false;
            this.txtContact.Text = "Unknown";
            this.txtContact.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // attn
            // 
            this.attn.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.attn.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.attn.LocationFloat = new DevExpress.Utils.PointFloat(1.041667F, 5.166687F);
            this.attn.Name = "attn";
            this.attn.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.attn.SizeF = new System.Drawing.SizeF(38.89249F, 17.7084F);
            this.attn.StylePriority.UseBorders = false;
            this.attn.StylePriority.UseFont = false;
            this.attn.StylePriority.UseTextAlignment = false;
            this.attn.Text = "Attn:";
            this.attn.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // attn_name
            // 
            this.attn_name.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.attn_name.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.attn_name.LocationFloat = new DevExpress.Utils.PointFloat(39.93416F, 5.166702F);
            this.attn_name.Name = "attn_name";
            this.attn_name.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.attn_name.SizeF = new System.Drawing.SizeF(341.7324F, 17.70839F);
            this.attn_name.StylePriority.UseBorders = false;
            this.attn_name.StylePriority.UseFont = false;
            this.attn_name.Text = "attn_name";
            // 
            // customer_number
            // 
            this.customer_number.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.customer_number.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.customer_number.LocationFloat = new DevExpress.Utils.PointFloat(39.93416F, 22.87509F);
            this.customer_number.Name = "customer_number";
            this.customer_number.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.customer_number.SizeF = new System.Drawing.SizeF(127.0833F, 17.70842F);
            this.customer_number.StylePriority.UseBorders = false;
            this.customer_number.StylePriority.UseFont = false;
            this.customer_number.StylePriority.UseTextAlignment = false;
            this.customer_number.Text = "customer_number";
            this.customer_number.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // service_address
            // 
            this.service_address.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.service_address.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Bold);
            this.service_address.LocationFloat = new DevExpress.Utils.PointFloat(1.041667F, 103.084F);
            this.service_address.Multiline = true;
            this.service_address.Name = "service_address";
            this.service_address.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.service_address.SizeF = new System.Drawing.SizeF(380.6249F, 57.7493F);
            this.service_address.StylePriority.UseBorders = false;
            this.service_address.StylePriority.UseFont = false;
            this.service_address.Text = "Service Address: Same";
            // 
            // customer_address
            // 
            this.customer_address.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.customer_address.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.customer_address.LocationFloat = new DevExpress.Utils.PointFloat(39.93416F, 40.58348F);
            this.customer_address.Multiline = true;
            this.customer_address.Name = "customer_address";
            this.customer_address.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.customer_address.SizeF = new System.Drawing.SizeF(341.7324F, 45.83327F);
            this.customer_address.StylePriority.UseBorders = false;
            this.customer_address.StylePriority.UseFont = false;
            this.customer_address.Text = "customer_address";
            // 
            // PageFooter
            // 
            this.PageFooter.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrPageInfo1});
            this.PageFooter.HeightF = 58.33333F;
            this.PageFooter.Name = "PageFooter";
            // 
            // xrCrossBandBox1
            // 
            this.xrCrossBandBox1.BorderWidth = 1F;
            this.xrCrossBandBox1.EndBand = this.PageFooter;
            this.xrCrossBandBox1.EndPointFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrCrossBandBox1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 370.8334F);
            this.xrCrossBandBox1.Name = "xrCrossBandBox1";
            this.xrCrossBandBox1.StartBand = this.PageHeader;
            this.xrCrossBandBox1.StartPointFloat = new DevExpress.Utils.PointFloat(0F, 370.8334F);
            this.xrCrossBandBox1.WidthF = 799.9998F;
            // 
            // WorkOrderPreview
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.ReportFooter,
            this.PageHeader,
            this.PageFooter});
            this.CrossBandControls.AddRange(new DevExpress.XtraReports.UI.XRCrossBandControl[] {
            this.xrCrossBandBox1});
            this.DataMember = "invoice_preview_dt";
            this.DataSource = this.invoice_preview_ds1;
            this.Margins = new System.Drawing.Printing.Margins(26, 24, 23, 21);
            this.RequestParameters = false;
            this.Version = "17.2";
            this.Watermark.Font = new System.Drawing.Font("Verdana", 50F, System.Drawing.FontStyle.Bold);
            this.Watermark.Text = "Work estimate - not an invoice";
            this.Watermark.TextTransparency = 200;
            this.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.InvoicePreview_BeforePrint);
            ((System.ComponentModel.ISupportInitialize)(this.invoice_preview_ds1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lbl_pb_notes)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

			}

		#endregion

		private void InvoicePreview_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
			{
			xrPageInfo1.Format			= "Page {0} of {1} - T"+_broken_out; 
			}
		}
	}
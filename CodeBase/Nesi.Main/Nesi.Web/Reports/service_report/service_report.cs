using System;
using DevExpress.XtraReports.UI;
using Nesi.Web.Reports.service_report;

namespace nesi.core.print
	{
	/// <summary>
	/// Summary description for XtraReport1
	/// </summary>
	public class sr_main : XtraReport
		{
		private DetailBand Detail;
		private TopMarginBand TopMargin;
		private BottomMarginBand BottomMargin; 
		private XRTable xrTable2;
		private XRTableRow xrTableRow2;
		private XRTableCell xrTableCell2;
		private XRTableCell xrTableCell4;
		private XRLabel xrLabel8;
		private XRLabel xrLabel9;
		private XRLabel xrLabel6;
		private XRLabel lb_phone;
		private XRLabel lb_fax;
		private XRLabel xrLabel10;
		private XRLabel lb_wo;
		private XRLabel lb_custpo;
		private XRLabel lb_attn;
		private XRLabel lb_date;
		private XRLabel xrLabel4;
		private XRLabel xrLabel5;
		private DevExpress.XtraReports.Parameters.Parameter woprog_id;
		private XRLabel lb_job;
		private XRLabel xrLabel3;
		private XRLabel xrLabel1;
		private XRLabel xrLabel2;
		private XRLabel lb_address;
		private XRLine xrLine1;
		private XRLine xrLine2;
		private XRLabel xrLabel11;
		private ds_service_report ds_service_report1;
		private Nesi.Web.Reports.service_report.ds_service_reportTableAdapters.service_report_data_ta data_ta1;
		private DevExpress.XtraReports.Parameters.Parameter Param1;
		private XRPictureBox xrPictureBox1;
		private XRTable xrTable1;
		private XRTableRow xrTableRow1;
		private XRTableCell xrTableCell1;
		private XRTableCell xrTableCell3;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		public sr_main()
			{
			InitializeComponent();
			//
			//  
			//
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
            string resourceFileName = "service_report.resx";
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.xrTable1 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow1 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell1 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell3 = new DevExpress.XtraReports.UI.XRTableCell();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.xrPictureBox1 = new DevExpress.XtraReports.UI.XRPictureBox();
            this.xrTable2 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow2 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell2 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell4 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrLabel8 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel9 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel6 = new DevExpress.XtraReports.UI.XRLabel();
            this.lb_phone = new DevExpress.XtraReports.UI.XRLabel();
            this.lb_fax = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel10 = new DevExpress.XtraReports.UI.XRLabel();
            this.lb_wo = new DevExpress.XtraReports.UI.XRLabel();
            this.lb_custpo = new DevExpress.XtraReports.UI.XRLabel();
            this.lb_attn = new DevExpress.XtraReports.UI.XRLabel();
            this.lb_date = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel4 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel5 = new DevExpress.XtraReports.UI.XRLabel();
            this.lb_job = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel3 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel2 = new DevExpress.XtraReports.UI.XRLabel();
            this.lb_address = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLine1 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLine2 = new DevExpress.XtraReports.UI.XRLine();
            this.woprog_id = new DevExpress.XtraReports.Parameters.Parameter();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.xrLabel11 = new DevExpress.XtraReports.UI.XRLabel();
            this.ds_service_report1 = new ds_service_report();
            this.data_ta1 = new Nesi.Web.Reports.service_report.ds_service_reportTableAdapters.service_report_data_ta();
            this.Param1 = new DevExpress.XtraReports.Parameters.Parameter();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ds_service_report1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrTable1});
            this.Detail.HeightF = 28.41669F;
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrTable1
            // 
            this.xrTable1.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTable1.Font = new System.Drawing.Font("Arial", 8F);
            this.xrTable1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrTable1.Name = "xrTable1";
            this.xrTable1.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow1});
            this.xrTable1.SizeF = new System.Drawing.SizeF(726F, 28.41669F);
            this.xrTable1.StylePriority.UseBorders = false;
            this.xrTable1.StylePriority.UseFont = false;
            // 
            // xrTableRow1
            // 
            this.xrTableRow1.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell1,
            this.xrTableCell3});
            this.xrTableRow1.Name = "xrTableRow1";
            this.xrTableRow1.Weight = 1D;
            // 
            // xrTableCell1
            // 
            this.xrTableCell1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.xrTableCell1.Name = "xrTableCell1";
            this.xrTableCell1.Padding = new DevExpress.XtraPrinting.PaddingInfo(4, 4, 5, 4, 100F);
            this.xrTableCell1.StylePriority.UseFont = false;
            this.xrTableCell1.StylePriority.UsePadding = false;
            this.xrTableCell1.StylePriority.UseTextAlignment = false;
            this.xrTableCell1.Text = "1";
            this.xrTableCell1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            this.xrTableCell1.Weight = 0.99999977874151846D;
            // 
            // xrTableCell3
            // 
            this.xrTableCell3.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "service_report_data.descr")});
            this.xrTableCell3.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.xrTableCell3.Multiline = true;
            this.xrTableCell3.Name = "xrTableCell3";
            this.xrTableCell3.Padding = new DevExpress.XtraPrinting.PaddingInfo(4, 4, 5, 4, 100F);
            this.xrTableCell3.StylePriority.UseFont = false;
            this.xrTableCell3.StylePriority.UsePadding = false;
            this.xrTableCell3.Text = "Description";
            this.xrTableCell3.Weight = 6.2288836765389322D;
            // 
            // TopMargin
            // 
            this.TopMargin.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrPictureBox1,
            this.xrTable2,
            this.xrLabel8,
            this.xrLabel9,
            this.xrLabel6,
            this.lb_phone,
            this.lb_fax,
            this.xrLabel10,
            this.lb_wo,
            this.lb_custpo,
            this.lb_attn,
            this.lb_date,
            this.xrLabel4,
            this.xrLabel5,
            this.lb_job,
            this.xrLabel3,
            this.xrLabel1,
            this.xrLabel2,
            this.lb_address,
            this.xrLine1,
            this.xrLine2});
            this.TopMargin.HeightF = 450.9167F;
            this.TopMargin.Name = "TopMargin";
            this.TopMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.TopMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrPictureBox1
            // 
            this.xrPictureBox1.ImageUrl = Toolbox.app_setting("Domain") + @"/images/Logos/nesi-logo-blue-invert.png";
            this.xrPictureBox1.LocationFloat = new DevExpress.Utils.PointFloat(3.083293F, 22.37509F);
            this.xrPictureBox1.Name = "xrPictureBox1";
            this.xrPictureBox1.SizeF = new System.Drawing.SizeF(136.875F, 109.3332F);
            this.xrPictureBox1.Sizing = DevExpress.XtraPrinting.ImageSizeMode.ZoomImage;
            // 
            // xrTable2
            // 
            this.xrTable2.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.xrTable2.Font = new System.Drawing.Font("Arial", 8F);
            this.xrTable2.LocationFloat = new DevExpress.Utils.PointFloat(0F, 422.5F);
            this.xrTable2.Name = "xrTable2";
            this.xrTable2.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow2});
            this.xrTable2.SizeF = new System.Drawing.SizeF(726F, 28.41669F);
            this.xrTable2.StylePriority.UseBorders = false;
            this.xrTable2.StylePriority.UseFont = false;
            // 
            // xrTableRow2
            // 
            this.xrTableRow2.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell2,
            this.xrTableCell4});
            this.xrTableRow2.Name = "xrTableRow2";
            this.xrTableRow2.Weight = 1D;
            // 
            // xrTableCell2
            // 
            this.xrTableCell2.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.xrTableCell2.Name = "xrTableCell2";
            this.xrTableCell2.Padding = new DevExpress.XtraPrinting.PaddingInfo(4, 4, 5, 4, 100F);
            this.xrTableCell2.StylePriority.UseFont = false;
            this.xrTableCell2.StylePriority.UsePadding = false;
            this.xrTableCell2.StylePriority.UseTextAlignment = false;
            this.xrTableCell2.Text = "Quantity";
            this.xrTableCell2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            this.xrTableCell2.Weight = 0.99999977874151846D;
            // 
            // xrTableCell4
            // 
            this.xrTableCell4.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.xrTableCell4.Name = "xrTableCell4";
            this.xrTableCell4.Padding = new DevExpress.XtraPrinting.PaddingInfo(4, 4, 5, 4, 100F);
            this.xrTableCell4.StylePriority.UseFont = false;
            this.xrTableCell4.StylePriority.UsePadding = false;
            this.xrTableCell4.Text = "Description";
            this.xrTableCell4.Weight = 6.2599997382438239D;
            // 
            // xrLabel8
            // 
            this.xrLabel8.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold);
            this.xrLabel8.LocationFloat = new DevExpress.Utils.PointFloat(446.8333F, 321.4583F);
            this.xrLabel8.Name = "xrLabel8";
            this.xrLabel8.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel8.SizeF = new System.Drawing.SizeF(115.6667F, 21.70825F);
            this.xrLabel8.StylePriority.UseFont = false;
            this.xrLabel8.Text = "Date:";
            // 
            // xrLabel9
            // 
            this.xrLabel9.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold);
            this.xrLabel9.LocationFloat = new DevExpress.Utils.PointFloat(0F, 360.9583F);
            this.xrLabel9.Name = "xrLabel9";
            this.xrLabel9.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel9.SizeF = new System.Drawing.SizeF(100F, 23F);
            this.xrLabel9.StylePriority.UseFont = false;
            this.xrLabel9.Text = "Phone:";
            // 
            // xrLabel6
            // 
            this.xrLabel6.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold);
            this.xrLabel6.LocationFloat = new DevExpress.Utils.PointFloat(446.8333F, 298.4583F);
            this.xrLabel6.Name = "xrLabel6";
            this.xrLabel6.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel6.SizeF = new System.Drawing.SizeF(115.6667F, 23F);
            this.xrLabel6.StylePriority.UseFont = false;
            this.xrLabel6.Text = "Customer PO:";
            // 
            // lb_phone
            // 
            this.lb_phone.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "service_report_data.cust_phone")});
            this.lb_phone.Font = new System.Drawing.Font("Arial", 9.75F);
            this.lb_phone.LocationFloat = new DevExpress.Utils.PointFloat(100F, 360.9583F);
            this.lb_phone.Name = "lb_phone";
            this.lb_phone.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lb_phone.SizeF = new System.Drawing.SizeF(100F, 23F);
            this.lb_phone.StylePriority.UseFont = false;
            // 
            // lb_fax
            // 
            this.lb_fax.Font = new System.Drawing.Font("Arial", 9.75F);
            this.lb_fax.LocationFloat = new DevExpress.Utils.PointFloat(100F, 385.9582F);
            this.lb_fax.Name = "lb_fax";
            this.lb_fax.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lb_fax.SizeF = new System.Drawing.SizeF(100F, 23F);
            this.lb_fax.StylePriority.UseFont = false;
            // 
            // xrLabel10
            // 
            this.xrLabel10.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold);
            this.xrLabel10.LocationFloat = new DevExpress.Utils.PointFloat(0F, 385.9582F);
            this.xrLabel10.Name = "xrLabel10";
            this.xrLabel10.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel10.SizeF = new System.Drawing.SizeF(100F, 23F);
            this.xrLabel10.StylePriority.UseFont = false;
            this.xrLabel10.Text = "Fax:";
            // 
            // lb_wo
            // 
            this.lb_wo.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "service_report_data.woprog_bvwo")});
            this.lb_wo.Font = new System.Drawing.Font("Arial", 9.75F);
            this.lb_wo.LocationFloat = new DevExpress.Utils.PointFloat(562.5F, 248.4583F);
            this.lb_wo.Name = "lb_wo";
            this.lb_wo.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lb_wo.SizeF = new System.Drawing.SizeF(159.375F, 23F);
            this.lb_wo.StylePriority.UseFont = false;
            // 
            // lb_custpo
            // 
            this.lb_custpo.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "service_report_data.woprog_custpo")});
            this.lb_custpo.Font = new System.Drawing.Font("Arial", 9.75F);
            this.lb_custpo.LocationFloat = new DevExpress.Utils.PointFloat(562.5F, 298.4583F);
            this.lb_custpo.Name = "lb_custpo";
            this.lb_custpo.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lb_custpo.SizeF = new System.Drawing.SizeF(159.375F, 23F);
            this.lb_custpo.StylePriority.UseFont = false;
            // 
            // lb_attn
            // 
            this.lb_attn.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "service_report_data.attn")});
            this.lb_attn.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold);
            this.lb_attn.LocationFloat = new DevExpress.Utils.PointFloat(100F, 248.4583F);
            this.lb_attn.Multiline = true;
            this.lb_attn.Name = "lb_attn";
            this.lb_attn.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lb_attn.SizeF = new System.Drawing.SizeF(300F, 109.2916F);
            this.lb_attn.StylePriority.UseFont = false;
            // 
            // lb_date
            // 
            this.lb_date.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "service_report_data.this_date")});
            this.lb_date.Font = new System.Drawing.Font("Arial", 9.75F);
            this.lb_date.LocationFloat = new DevExpress.Utils.PointFloat(562.5F, 321.4583F);
            this.lb_date.Name = "lb_date";
            this.lb_date.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lb_date.SizeF = new System.Drawing.SizeF(159.375F, 21.70825F);
            this.lb_date.StylePriority.UseFont = false;
            // 
            // xrLabel4
            // 
            this.xrLabel4.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold);
            this.xrLabel4.LocationFloat = new DevExpress.Utils.PointFloat(446.8333F, 248.4583F);
            this.xrLabel4.Name = "xrLabel4";
            this.xrLabel4.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel4.SizeF = new System.Drawing.SizeF(115.6667F, 23F);
            this.xrLabel4.StylePriority.UseFont = false;
            this.xrLabel4.Text = "Work Order:";
            // 
            // xrLabel5
            // 
            this.xrLabel5.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold);
            this.xrLabel5.LocationFloat = new DevExpress.Utils.PointFloat(446.8333F, 273.4583F);
            this.xrLabel5.Name = "xrLabel5";
            this.xrLabel5.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel5.SizeF = new System.Drawing.SizeF(115.6667F, 23F);
            this.xrLabel5.StylePriority.UseFont = false;
            this.xrLabel5.Text = "Quote #:";
            // 
            // lb_job
            // 
            this.lb_job.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "service_report_data.quotenum")});
            this.lb_job.Font = new System.Drawing.Font("Arial", 9.75F);
            this.lb_job.LocationFloat = new DevExpress.Utils.PointFloat(562.5F, 273.4583F);
            this.lb_job.Name = "lb_job";
            this.lb_job.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lb_job.SizeF = new System.Drawing.SizeF(159.375F, 23F);
            this.lb_job.StylePriority.UseFont = false;
            // 
            // xrLabel3
            // 
            this.xrLabel3.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold);
            this.xrLabel3.LocationFloat = new DevExpress.Utils.PointFloat(0F, 248.4583F);
            this.xrLabel3.Name = "xrLabel3";
            this.xrLabel3.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel3.SizeF = new System.Drawing.SizeF(100F, 23F);
            this.xrLabel3.StylePriority.UseFont = false;
            this.xrLabel3.Text = "Attn:";
            // 
            // xrLabel1
            // 
            this.xrLabel1.Font = new System.Drawing.Font("Arial", 16F, System.Drawing.FontStyle.Bold);
            this.xrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(3.083293F, 199.7083F);
            this.xrLabel1.Name = "xrLabel1";
            this.xrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel1.SizeF = new System.Drawing.SizeF(722.9166F, 21.95834F);
            this.xrLabel1.StylePriority.UseFont = false;
            this.xrLabel1.StylePriority.UseTextAlignment = false;
            this.xrLabel1.Text = "Packing Slip / Service Report";
            this.xrLabel1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // xrLabel2
            // 
            this.xrLabel2.Font = new System.Drawing.Font("Tahoma", 18F);
            this.xrLabel2.LocationFloat = new DevExpress.Utils.PointFloat(347.9167F, 51.00001F);
            this.xrLabel2.Multiline = true;
            this.xrLabel2.Name = "xrLabel2";
            this.xrLabel2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel2.SizeF = new System.Drawing.SizeF(283.3333F, 67.79167F);
            this.xrLabel2.StylePriority.UseFont = false;
            this.xrLabel2.Text = "Electronic Repair & Replacement Centre";
            // 
            // lb_address
            // 
            this.lb_address.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "service_report_data.addy")});
            this.lb_address.Font = new System.Drawing.Font("Arial", 9.75F);
            this.lb_address.LocationFloat = new DevExpress.Utils.PointFloat(3.083165F, 154.7083F);
            this.lb_address.Multiline = true;
            this.lb_address.Name = "lb_address";
            this.lb_address.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lb_address.SizeF = new System.Drawing.SizeF(722.9167F, 45.00005F);
            this.lb_address.StylePriority.UseFont = false;
            this.lb_address.StylePriority.UseTextAlignment = false;
            this.lb_address.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // xrLine1
            // 
            this.xrLine1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 131.7083F);
            this.xrLine1.Name = "xrLine1";
            this.xrLine1.SizeF = new System.Drawing.SizeF(726F, 22.99998F);
            // 
            // xrLine2
            // 
            this.xrLine2.LocationFloat = new DevExpress.Utils.PointFloat(3.083293F, 221.6667F);
            this.xrLine2.Name = "xrLine2";
            this.xrLine2.SizeF = new System.Drawing.SizeF(722.9166F, 23F);
            // 
            // woprog_id
            // 
            this.woprog_id.Name = "woprog_id";
            // 
            // BottomMargin
            // 
            this.BottomMargin.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel11});
            this.BottomMargin.HeightF = 102F;
            this.BottomMargin.Name = "BottomMargin";
            this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrLabel11
            // 
            this.xrLabel11.Font = new System.Drawing.Font("Arial", 9F);
            this.xrLabel11.LocationFloat = new DevExpress.Utils.PointFloat(3.178914E-05F, 0F);
            this.xrLabel11.Name = "xrLabel11";
            this.xrLabel11.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel11.SizeF = new System.Drawing.SizeF(725.9999F, 31.33333F);
            this.xrLabel11.StylePriority.UseFont = false;
            this.xrLabel11.StylePriority.UseTextAlignment = false;
            this.xrLabel11.Text = "Subject to standard Terms & Conditions - available upon request";
            this.xrLabel11.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // ds_service_report1
            // 
            this.ds_service_report1.DataSetName = "ds_service_report";
            this.ds_service_report1.EnforceConstraints = false;
            this.ds_service_report1.RemotingFormat = System.Data.SerializationFormat.Binary;
            this.ds_service_report1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // data_ta1
            // 
            this.data_ta1.ClearBeforeFill = true;
            // 
            // Param1
            // 
            this.Param1.Name = "Param1";
            this.Param1.Type = typeof(int);
            this.Param1.ValueInfo = "40999";
            // 
            // sr_main
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin});
            this.DataAdapter = this.data_ta1;
            this.DataMember = "service_report_data";
            this.DataSource = this.ds_service_report1;
            this.Margins = new System.Drawing.Printing.Margins(65, 59, 451, 102);
            this.Parameters.AddRange(new DevExpress.XtraReports.Parameters.Parameter[] {
            this.woprog_id,
            this.Param1});
            this.Version = "14.2";
            this.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.sr_main_BeforePrint);
            ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ds_service_report1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

			}
		private void sr_main_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
			{
			var _tools = new Toolbox();
			var _adapter = new Nesi.Web.Reports.service_report.ds_service_reportTableAdapters.service_report_data_ta();
			_adapter.Fill(this.ds_service_report1.service_report_data, Convert.ToInt32(woprog_id.Value));
			}

		#endregion
		}
	}
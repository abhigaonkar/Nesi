using System;
using DevExpress.XtraReports.UI;
using Nesi.Web.Reports.PurchaseOrder;

namespace nesi.core.print
	{
	/// <summary>
	/// Summary description for podetailsdisp
	/// </summary>
	public class podetailsdisp : XtraReport
		{
		private DetailBand Detail;
		private TopMarginBand TopMargin;
		private BottomMarginBand BottomMargin;
		private podetails podetails1;
		private DevExpress.XtraReports.Parameters.Parameter detailspoprogid;  
		
        private ReportHeaderBand ReportHeader;
		private XRTable xrTable2;
		private XRTableRow xrTableRow2;
		private XRTableCell xrTableCell4; 
		private XRTableCell xrTableCell12; 
		private XRControlStyle xrControlStyle1;
		private ReportFooterBand ReportFooter;
		private XRLabel xrLabelGSTInfo;
		private XRLabel lblNet;
		private XRLabel lblFreight;
		private XRLabel lblHST;
		private XRLabel lblTotal;
		private XRTableCell xrTableCell15;
		private XRTableCell xrTableCell16;
		private XRLabel xrLabelNotes;
		private XRLabel xrLabel1;
		private XRLabel xrLabel2;
		private XRLabel labelHST;
		private XRControlStyle xrControlStyle2;
		private XRLabel xrLabel3;
		private XRLabel xrLabel4;
		private XRLabel xrLabel5;
		private XRLabel xrLabel7;
		private XRLabel xrLabel8;
		private XRLabel xrLabel9;
		private XRLabel xrLabel10;
		private XRLabel xrLabel11;
		private XRTableCell xrTableCell1;
		private XRTableCell xrTableCell2;
		private XRTableCell xrTableCell3;
		private XRTableCell xrTableCell6;
		private XRLabel xrLabel12;
		private XRLabel xrLabel6;
		private XRBarCode xrBarCode2;
        private XRLabel xrLabel13;
        private DevExpress.XtraReports.Parameters.Parameter print_preview;
		private XRRichText xrSameGreat;

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		public podetailsdisp()
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
			DevExpress.XtraPrinting.BarCode.Code39ExtendedGenerator code39ExtendedGenerator1 = new DevExpress.XtraPrinting.BarCode.Code39ExtendedGenerator();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(podetailsdisp));
			this.Detail = new DevExpress.XtraReports.UI.DetailBand();
			this.xrLabel13 = new DevExpress.XtraReports.UI.XRLabel();
			this.xrBarCode2 = new DevExpress.XtraReports.UI.XRBarCode();
			this.xrLabel6 = new DevExpress.XtraReports.UI.XRLabel();
			this.xrTable2 = new DevExpress.XtraReports.UI.XRTable();
			this.xrTableRow2 = new DevExpress.XtraReports.UI.XRTableRow();
			this.xrTableCell6 = new DevExpress.XtraReports.UI.XRTableCell();
			this.xrTableCell4 = new DevExpress.XtraReports.UI.XRTableCell();
			this.xrTableCell12 = new DevExpress.XtraReports.UI.XRTableCell();
			this.xrTableCell15 = new DevExpress.XtraReports.UI.XRTableCell();
			this.xrTableCell16 = new DevExpress.XtraReports.UI.XRTableCell();
			this.xrTableCell1 = new DevExpress.XtraReports.UI.XRTableCell();
			this.xrTableCell2 = new DevExpress.XtraReports.UI.XRTableCell();
			this.xrTableCell3 = new DevExpress.XtraReports.UI.XRTableCell();
			this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
			this.xrLabelNotes = new DevExpress.XtraReports.UI.XRLabel();
			this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
			this.podetails1 = new Nesi.Web.Reports.PurchaseOrder.podetails();
			this.detailspoprogid = new DevExpress.XtraReports.Parameters.Parameter();
			this.ReportHeader = new DevExpress.XtraReports.UI.ReportHeaderBand();
			this.xrLabel12 = new DevExpress.XtraReports.UI.XRLabel();
			this.xrLabel11 = new DevExpress.XtraReports.UI.XRLabel();
			this.xrLabel10 = new DevExpress.XtraReports.UI.XRLabel();
			this.xrLabel9 = new DevExpress.XtraReports.UI.XRLabel();
			this.xrLabel8 = new DevExpress.XtraReports.UI.XRLabel();
			this.xrLabel7 = new DevExpress.XtraReports.UI.XRLabel();
			this.xrLabel5 = new DevExpress.XtraReports.UI.XRLabel();
			this.xrLabel4 = new DevExpress.XtraReports.UI.XRLabel();
			this.xrControlStyle1 = new DevExpress.XtraReports.UI.XRControlStyle();
			this.ReportFooter = new DevExpress.XtraReports.UI.ReportFooterBand();
			this.xrLabel3 = new DevExpress.XtraReports.UI.XRLabel();
			this.labelHST = new DevExpress.XtraReports.UI.XRLabel();
			this.xrLabel2 = new DevExpress.XtraReports.UI.XRLabel();
			this.xrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
			this.lblTotal = new DevExpress.XtraReports.UI.XRLabel();
			this.lblHST = new DevExpress.XtraReports.UI.XRLabel();
			this.lblFreight = new DevExpress.XtraReports.UI.XRLabel();
			this.lblNet = new DevExpress.XtraReports.UI.XRLabel();
			this.xrLabelGSTInfo = new DevExpress.XtraReports.UI.XRLabel();
			this.xrControlStyle2 = new DevExpress.XtraReports.UI.XRControlStyle();
			this.print_preview = new DevExpress.XtraReports.Parameters.Parameter();
			this.xrSameGreat = new DevExpress.XtraReports.UI.XRRichText();
			((System.ComponentModel.ISupportInitialize)(this.xrTable2)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.podetails1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.xrSameGreat)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
			// 
			// Detail
			// 
			this.Detail.BackColor = System.Drawing.Color.White;
			this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel13,
            this.xrBarCode2,
            this.xrLabel6,
            this.xrTable2});
			this.Detail.EvenStyleName = "xrControlStyle2";
			this.Detail.HeightF = 72.58333F;
			this.Detail.Name = "Detail";
			this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
			this.Detail.StylePriority.UseBackColor = false;
			this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
			// 
			// xrLabel13
			// 
			this.xrLabel13.CanShrink = true;
			this.xrLabel13.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[_notes]")});
			this.xrLabel13.Font = new System.Drawing.Font("Arial", 8.25F);
			this.xrLabel13.LocationFloat = new DevExpress.Utils.PointFloat(9.999974F, 45.50002F);
			this.xrLabel13.Multiline = true;
			this.xrLabel13.Name = "xrLabel13";
			this.xrLabel13.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
			this.xrLabel13.SizeF = new System.Drawing.SizeF(588.7634F, 23F);
			this.xrLabel13.StylePriority.UseFont = false;
			this.xrLabel13.StylePriority.UseTextAlignment = false;
			this.xrLabel13.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
			// 
			// xrBarCode2
			// 
			this.xrBarCode2.Alignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
			this.xrBarCode2.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[po_details_part_no]")});
			this.xrBarCode2.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.xrBarCode2.LocationFloat = new DevExpress.Utils.PointFloat(0F, 22.50001F);
			this.xrBarCode2.Module = 1F;
			this.xrBarCode2.Name = "xrBarCode2";
			this.xrBarCode2.Padding = new DevExpress.XtraPrinting.PaddingInfo(10, 10, 0, 0, 100F);
			this.xrBarCode2.ShowText = false;
			this.xrBarCode2.SizeF = new System.Drawing.SizeF(188.8989F, 23F);
			this.xrBarCode2.StylePriority.UseFont = false;
			this.xrBarCode2.StylePriority.UseTextAlignment = false;
			code39ExtendedGenerator1.WideNarrowRatio = 3F;
			this.xrBarCode2.Symbology = code39ExtendedGenerator1;
			this.xrBarCode2.Text = "1-10071";
			this.xrBarCode2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
			this.xrBarCode2.TextFormatString = "1-{0}";
			// 
			// xrLabel6
			// 
			this.xrLabel6.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[po_details_description]")});
			this.xrLabel6.Font = new System.Drawing.Font("Arial", 8.25F);
			this.xrLabel6.LocationFloat = new DevExpress.Utils.PointFloat(188.8989F, 22.50001F);
			this.xrLabel6.Name = "xrLabel6";
			this.xrLabel6.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
			this.xrLabel6.SizeF = new System.Drawing.SizeF(409.8646F, 23F);
			this.xrLabel6.StylePriority.UseFont = false;
			this.xrLabel6.StylePriority.UseTextAlignment = false;
			this.xrLabel6.Text = "xrLabel6";
			this.xrLabel6.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
			// 
			// xrTable2
			// 
			this.xrTable2.BorderColor = System.Drawing.Color.LightGray;
			this.xrTable2.Borders = DevExpress.XtraPrinting.BorderSide.None;
			this.xrTable2.BorderWidth = 1F;
			this.xrTable2.EvenStyleName = "xrControlStyle1";
			this.xrTable2.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
			this.xrTable2.Name = "xrTable2";
			this.xrTable2.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow2});
			this.xrTable2.SizeF = new System.Drawing.SizeF(742.8588F, 22.50001F);
			this.xrTable2.StylePriority.UseBorderColor = false;
			this.xrTable2.StylePriority.UseBorders = false;
			this.xrTable2.StylePriority.UseBorderWidth = false;
			// 
			// xrTableRow2
			// 
			this.xrTableRow2.Borders = ((DevExpress.XtraPrinting.BorderSide)((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Right)));
			this.xrTableRow2.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell6,
            this.xrTableCell4,
            this.xrTableCell12,
            this.xrTableCell15,
            this.xrTableCell16,
            this.xrTableCell1,
            this.xrTableCell2,
            this.xrTableCell3});
			this.xrTableRow2.Name = "xrTableRow2";
			this.xrTableRow2.Padding = new DevExpress.XtraPrinting.PaddingInfo(1, 1, 0, 0, 100F);
			this.xrTableRow2.StylePriority.UseBorders = false;
			this.xrTableRow2.StylePriority.UsePadding = false;
			this.xrTableRow2.Weight = 0.56666666666666665D;
			// 
			// xrTableCell6
			// 
			this.xrTableCell6.Borders = DevExpress.XtraPrinting.BorderSide.None;
			this.xrTableCell6.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[po_details_rec_no]")});
			this.xrTableCell6.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.xrTableCell6.Name = "xrTableCell6";
			this.xrTableCell6.StylePriority.UseBorders = false;
			this.xrTableCell6.StylePriority.UseFont = false;
			this.xrTableCell6.StylePriority.UseTextAlignment = false;
			this.xrTableCell6.Text = "xrTableCell6";
			this.xrTableCell6.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
			this.xrTableCell6.Weight = 0.088117479934578052D;
			// 
			// xrTableCell4
			// 
			this.xrTableCell4.Borders = DevExpress.XtraPrinting.BorderSide.None;
			this.xrTableCell4.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[po_details_part_no]"),
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Visible", "Iif([po_details_part_no] = 0, False, ?)")});
			this.xrTableCell4.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.xrTableCell4.Name = "xrTableCell4";
			this.xrTableCell4.StylePriority.UseBorders = false;
			this.xrTableCell4.StylePriority.UseFont = false;
			this.xrTableCell4.StylePriority.UseTextAlignment = false;
			this.xrTableCell4.Text = "xrTableCell4";
			this.xrTableCell4.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
			this.xrTableCell4.Weight = 0.33669653316325932D;
			// 
			// xrTableCell12
			// 
			this.xrTableCell12.Borders = DevExpress.XtraPrinting.BorderSide.None;
			this.xrTableCell12.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[po_details_vendor_part_no]"),
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Visible", "Iif([po_details_part_no] = 0, False, ?)")});
			this.xrTableCell12.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.xrTableCell12.Name = "xrTableCell12";
			this.xrTableCell12.StylePriority.UseBorders = false;
			this.xrTableCell12.StylePriority.UseFont = false;
			this.xrTableCell12.StylePriority.UseTextAlignment = false;
			this.xrTableCell12.Text = "xrTableCell12";
			this.xrTableCell12.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
			this.xrTableCell12.Weight = 1.0488982961255304D;
			// 
			// xrTableCell15
			// 
			this.xrTableCell15.Borders = DevExpress.XtraPrinting.BorderSide.None;
			this.xrTableCell15.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[WOProg_BVWO]"),
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Visible", "Iif([po_details_part_no] = 0, False, ?)")});
			this.xrTableCell15.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.xrTableCell15.Name = "xrTableCell15";
			this.xrTableCell15.StylePriority.UseBorders = false;
			this.xrTableCell15.StylePriority.UseFont = false;
			this.xrTableCell15.StylePriority.UseTextAlignment = false;
			this.xrTableCell15.Text = "xrTableCell15";
			this.xrTableCell15.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
			this.xrTableCell15.TextFormatString = "{0}";
			this.xrTableCell15.Weight = 0.33680732888681009D;
			// 
			// xrTableCell16
			// 
			this.xrTableCell16.Borders = DevExpress.XtraPrinting.BorderSide.None;
			this.xrTableCell16.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[po_details_date_expected]"),
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Visible", "Iif([po_details_part_no] = 0, False, ?)")});
			this.xrTableCell16.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.xrTableCell16.Name = "xrTableCell16";
			this.xrTableCell16.StylePriority.UseBorders = false;
			this.xrTableCell16.StylePriority.UseFont = false;
			this.xrTableCell16.StylePriority.UseTextAlignment = false;
			this.xrTableCell16.Text = "xrTableCell16";
			this.xrTableCell16.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
			this.xrTableCell16.TextFormatString = "{0:dd/MM/yyyy}";
			this.xrTableCell16.Weight = 0.33137541445530128D;
			// 
			// xrTableCell1
			// 
			this.xrTableCell1.Borders = DevExpress.XtraPrinting.BorderSide.None;
			this.xrTableCell1.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[po_details_qty_ordered]")});
			this.xrTableCell1.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.xrTableCell1.Name = "xrTableCell1";
			this.xrTableCell1.StylePriority.UseBackColor = false;
			this.xrTableCell1.StylePriority.UseBorders = false;
			this.xrTableCell1.StylePriority.UseFont = false;
			this.xrTableCell1.Text = "xrTableCell1";
			this.xrTableCell1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
			this.xrTableCell1.TextFormatString = "{0:#.00}";
			this.xrTableCell1.Weight = 0.25635617581134951D;
			this.xrTableCell1.HtmlItemCreated += new DevExpress.XtraReports.UI.HtmlEventHandler(this.xrTableCell1_HtmlItemCreated);
			// 
			// xrTableCell2
			// 
			this.xrTableCell2.Borders = DevExpress.XtraPrinting.BorderSide.None;
			this.xrTableCell2.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[po_details_cost]")});
			this.xrTableCell2.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.xrTableCell2.Name = "xrTableCell2";
			this.xrTableCell2.StylePriority.UseBackColor = false;
			this.xrTableCell2.StylePriority.UseBorders = false;
			this.xrTableCell2.StylePriority.UseFont = false;
			this.xrTableCell2.Text = "xrTableCell2";
			this.xrTableCell2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
			this.xrTableCell2.TextFormatString = "{0:$0.000}";
			this.xrTableCell2.Weight = 0.2767190259514426D;
			this.xrTableCell2.HtmlItemCreated += new DevExpress.XtraReports.UI.HtmlEventHandler(this.xrTableCell2_HtmlItemCreated_1);
			// 
			// xrTableCell3
			// 
			this.xrTableCell3.Borders = DevExpress.XtraPrinting.BorderSide.None;
			this.xrTableCell3.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[ExtPrice]")});
			this.xrTableCell3.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.xrTableCell3.Name = "xrTableCell3";
			this.xrTableCell3.StylePriority.UseBackColor = false;
			this.xrTableCell3.StylePriority.UseBorders = false;
			this.xrTableCell3.StylePriority.UseFont = false;
			this.xrTableCell3.Text = "xrTableCell3";
			this.xrTableCell3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
			this.xrTableCell3.TextFormatString = "{0:$0.000}";
			this.xrTableCell3.Weight = 0.30043200728095804D;
			this.xrTableCell3.HtmlItemCreated += new DevExpress.XtraReports.UI.HtmlEventHandler(this.xrTableCell3_HtmlItemCreated);
			// 
			// TopMargin
			// 
			this.TopMargin.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabelNotes});
			this.TopMargin.HeightF = 83F;
			this.TopMargin.Name = "TopMargin";
			this.TopMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
			this.TopMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
			// 
			// xrLabelNotes
			// 
			this.xrLabelNotes.CanShrink = true;
			this.xrLabelNotes.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.xrLabelNotes.LocationFloat = new DevExpress.Utils.PointFloat(0F, 59.00003F);
			this.xrLabelNotes.Multiline = true;
			this.xrLabelNotes.Name = "xrLabelNotes";
			this.xrLabelNotes.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
			this.xrLabelNotes.SizeF = new System.Drawing.SizeF(534.6764F, 23.99997F);
			this.xrLabelNotes.StylePriority.UseBackColor = false;
			this.xrLabelNotes.StylePriority.UseFont = false;
			this.xrLabelNotes.Text = "xrLabelNotes";
			// 
			// BottomMargin
			// 
			this.BottomMargin.Name = "BottomMargin";
			this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
			this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
			// 
			// podetails1
			// 
			this.podetails1.DataSetName = "podetails";
			this.podetails1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
			// 
			// detailspoprogid
			// 
			this.detailspoprogid.Name = "detailspoprogid";
			this.detailspoprogid.Type = typeof(int);
			this.detailspoprogid.ValueInfo = "0";
			// 
			// ReportHeader
			// 
			this.ReportHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel12,
            this.xrLabel11,
            this.xrLabel10,
            this.xrLabel9,
            this.xrLabel8,
            this.xrLabel7,
            this.xrLabel5,
            this.xrLabel4});
			this.ReportHeader.HeightF = 27.79168F;
			this.ReportHeader.Name = "ReportHeader";
			// 
			// xrLabel12
			// 
			this.xrLabel12.BackColor = System.Drawing.Color.DimGray;
			this.xrLabel12.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
			this.xrLabel12.Name = "xrLabel12";
			this.xrLabel12.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
			this.xrLabel12.SizeF = new System.Drawing.SizeF(21.99999F, 17.79167F);
			this.xrLabel12.StylePriority.UseBackColor = false;
			// 
			// xrLabel11
			// 
			this.xrLabel11.BackColor = System.Drawing.Color.DimGray;
			this.xrLabel11.ForeColor = System.Drawing.Color.White;
			this.xrLabel11.LocationFloat = new DevExpress.Utils.PointFloat(610.2217F, 0F);
			this.xrLabel11.Name = "xrLabel11";
			this.xrLabel11.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
			this.xrLabel11.SizeF = new System.Drawing.SizeF(57.62927F, 17.79167F);
			this.xrLabel11.StylePriority.UseBackColor = false;
			this.xrLabel11.StylePriority.UseForeColor = false;
			this.xrLabel11.StylePriority.UseTextAlignment = false;
			this.xrLabel11.Text = "Price";
			this.xrLabel11.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
			// 
			// xrLabel10
			// 
			this.xrLabel10.BackColor = System.Drawing.Color.DimGray;
			this.xrLabel10.ForeColor = System.Drawing.Color.White;
			this.xrLabel10.LocationFloat = new DevExpress.Utils.PointFloat(667.8509F, 0F);
			this.xrLabel10.Name = "xrLabel10";
			this.xrLabel10.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
			this.xrLabel10.SizeF = new System.Drawing.SizeF(75.09137F, 17.79167F);
			this.xrLabel10.StylePriority.UseBackColor = false;
			this.xrLabel10.StylePriority.UseForeColor = false;
			this.xrLabel10.StylePriority.UseTextAlignment = false;
			this.xrLabel10.Text = "Ext. Price";
			this.xrLabel10.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
			// 
			// xrLabel9
			// 
			this.xrLabel9.BackColor = System.Drawing.Color.DimGray;
			this.xrLabel9.ForeColor = System.Drawing.Color.White;
			this.xrLabel9.LocationFloat = new DevExpress.Utils.PointFloat(106.0619F, 0F);
			this.xrLabel9.Name = "xrLabel9";
			this.xrLabel9.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
			this.xrLabel9.SizeF = new System.Drawing.SizeF(261.875F, 17.79167F);
			this.xrLabel9.StylePriority.UseBackColor = false;
			this.xrLabel9.StylePriority.UseForeColor = false;
			this.xrLabel9.StylePriority.UseTextAlignment = false;
			this.xrLabel9.Text = "Vendor Part #";
			this.xrLabel9.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
			// 
			// xrLabel8
			// 
			this.xrLabel8.BackColor = System.Drawing.Color.DimGray;
			this.xrLabel8.ForeColor = System.Drawing.Color.White;
			this.xrLabel8.LocationFloat = new DevExpress.Utils.PointFloat(367.9369F, 0F);
			this.xrLabel8.Name = "xrLabel8";
			this.xrLabel8.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
			this.xrLabel8.SizeF = new System.Drawing.SizeF(84.08954F, 17.79167F);
			this.xrLabel8.StylePriority.UseBackColor = false;
			this.xrLabel8.StylePriority.UseForeColor = false;
			this.xrLabel8.StylePriority.UseTextAlignment = false;
			this.xrLabel8.Text = "WO";
			this.xrLabel8.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
			// 
			// xrLabel7
			// 
			this.xrLabel7.BackColor = System.Drawing.Color.DimGray;
			this.xrLabel7.ForeColor = System.Drawing.Color.White;
			this.xrLabel7.LocationFloat = new DevExpress.Utils.PointFloat(452.0264F, 0F);
			this.xrLabel7.Name = "xrLabel7";
			this.xrLabel7.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
			this.xrLabel7.SizeF = new System.Drawing.SizeF(82.64996F, 17.79167F);
			this.xrLabel7.StylePriority.UseBackColor = false;
			this.xrLabel7.StylePriority.UseForeColor = false;
			this.xrLabel7.StylePriority.UseTextAlignment = false;
			this.xrLabel7.Text = "Date Req";
			this.xrLabel7.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
			// 
			// xrLabel5
			// 
			this.xrLabel5.BackColor = System.Drawing.Color.DimGray;
			this.xrLabel5.ForeColor = System.Drawing.Color.White;
			this.xrLabel5.LocationFloat = new DevExpress.Utils.PointFloat(534.7598F, 0F);
			this.xrLabel5.Name = "xrLabel5";
			this.xrLabel5.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
			this.xrLabel5.SizeF = new System.Drawing.SizeF(75.46191F, 17.79167F);
			this.xrLabel5.StylePriority.UseBackColor = false;
			this.xrLabel5.StylePriority.UseForeColor = false;
			this.xrLabel5.StylePriority.UseTextAlignment = false;
			this.xrLabel5.Text = "Qty Req";
			this.xrLabel5.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
			// 
			// xrLabel4
			// 
			this.xrLabel4.BackColor = System.Drawing.Color.DimGray;
			this.xrLabel4.ForeColor = System.Drawing.Color.White;
			this.xrLabel4.LocationFloat = new DevExpress.Utils.PointFloat(21.99999F, 0F);
			this.xrLabel4.Name = "xrLabel4";
			this.xrLabel4.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
			this.xrLabel4.SizeF = new System.Drawing.SizeF(84.06191F, 17.79167F);
			this.xrLabel4.StylePriority.UseBackColor = false;
			this.xrLabel4.StylePriority.UseForeColor = false;
			this.xrLabel4.StylePriority.UseTextAlignment = false;
			this.xrLabel4.Text = "Part No";
			this.xrLabel4.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
			// 
			// xrControlStyle1
			// 
			this.xrControlStyle1.BackColor = System.Drawing.Color.Gainsboro;
			this.xrControlStyle1.Name = "xrControlStyle1";
			this.xrControlStyle1.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
			// 
			// ReportFooter
			// 
			this.ReportFooter.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrSameGreat,
            this.xrLabel3,
            this.labelHST,
            this.xrLabel2,
            this.xrLabel1,
            this.lblTotal,
            this.lblHST,
            this.lblFreight,
            this.lblNet,
            this.xrLabelGSTInfo});
			this.ReportFooter.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.ReportFooter.HeightF = 235.2916F;
			this.ReportFooter.Name = "ReportFooter";
			this.ReportFooter.PrintAtBottom = true;
			this.ReportFooter.StylePriority.UseFont = false;
			this.ReportFooter.StylePriority.UseTextAlignment = false;
			this.ReportFooter.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
			// 
			// xrLabel3
			// 
			this.xrLabel3.BackColor = System.Drawing.Color.DimGray;
			this.xrLabel3.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.xrLabel3.ForeColor = System.Drawing.Color.White;
			this.xrLabel3.LocationFloat = new DevExpress.Utils.PointFloat(0.08335114F, 203.1666F);
			this.xrLabel3.Name = "xrLabel3";
			this.xrLabel3.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
			this.xrLabel3.SizeF = new System.Drawing.SizeF(632.5399F, 23.00002F);
			this.xrLabel3.StylePriority.UseBackColor = false;
			this.xrLabel3.StylePriority.UseFont = false;
			this.xrLabel3.StylePriority.UseForeColor = false;
			this.xrLabel3.StylePriority.UseTextAlignment = false;
			this.xrLabel3.Text = "Total:";
			this.xrLabel3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
			// 
			// labelHST
			// 
			this.labelHST.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.labelHST.LocationFloat = new DevExpress.Utils.PointFloat(600.7202F, 173.8332F);
			this.labelHST.Name = "labelHST";
			this.labelHST.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
			this.labelHST.SizeF = new System.Drawing.SizeF(31.90314F, 21.20832F);
			this.labelHST.StylePriority.UseFont = false;
			this.labelHST.StylePriority.UseTextAlignment = false;
			this.labelHST.Text = "HST";
			this.labelHST.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
			this.labelHST.Visible = false;
			// 
			// xrLabel2
			// 
			this.xrLabel2.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.xrLabel2.LocationFloat = new DevExpress.Utils.PointFloat(572.2067F, 150.5415F);
			this.xrLabel2.Name = "xrLabel2";
			this.xrLabel2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
			this.xrLabel2.SizeF = new System.Drawing.SizeF(60.41669F, 21.20832F);
			this.xrLabel2.StylePriority.UseFont = false;
			this.xrLabel2.StylePriority.UseTextAlignment = false;
			this.xrLabel2.Text = "FREIGHT";
			this.xrLabel2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
			// 
			// xrLabel1
			// 
			this.xrLabel1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.xrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(544.0834F, 126.1666F);
			this.xrLabel1.Name = "xrLabel1";
			this.xrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
			this.xrLabel1.SizeF = new System.Drawing.SizeF(88.53998F, 21.20832F);
			this.xrLabel1.StylePriority.UseFont = false;
			this.xrLabel1.StylePriority.UseTextAlignment = false;
			this.xrLabel1.Text = "NET AMOUNT";
			this.xrLabel1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
			// 
			// lblTotal
			// 
			this.lblTotal.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblTotal.LocationFloat = new DevExpress.Utils.PointFloat(638.851F, 203.0416F);
			this.lblTotal.Name = "lblTotal";
			this.lblTotal.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
			this.lblTotal.SizeF = new System.Drawing.SizeF(104.0913F, 22.24998F);
			this.lblTotal.StylePriority.UseFont = false;
			this.lblTotal.StylePriority.UseTextAlignment = false;
			this.lblTotal.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
			// 
			// lblHST
			// 
			this.lblHST.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblHST.LocationFloat = new DevExpress.Utils.PointFloat(638.851F, 173.8332F);
			this.lblHST.Name = "lblHST";
			this.lblHST.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
			this.lblHST.SizeF = new System.Drawing.SizeF(104.0079F, 21.20832F);
			this.lblHST.StylePriority.UseFont = false;
			this.lblHST.StylePriority.UseTextAlignment = false;
			this.lblHST.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
			this.lblHST.Visible = false;
			// 
			// lblFreight
			// 
			this.lblFreight.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblFreight.LocationFloat = new DevExpress.Utils.PointFloat(638.8509F, 150.5415F);
			this.lblFreight.Name = "lblFreight";
			this.lblFreight.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
			this.lblFreight.SizeF = new System.Drawing.SizeF(104.008F, 21.20832F);
			this.lblFreight.StylePriority.UseFont = false;
			this.lblFreight.StylePriority.UseTextAlignment = false;
			this.lblFreight.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
			// 
			// lblNet
			// 
			this.lblNet.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblNet.LocationFloat = new DevExpress.Utils.PointFloat(638.8509F, 126.1666F);
			this.lblNet.Name = "lblNet";
			this.lblNet.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
			this.lblNet.SizeF = new System.Drawing.SizeF(104.0914F, 21.20832F);
			this.lblNet.StylePriority.UseFont = false;
			this.lblNet.StylePriority.UseTextAlignment = false;
			this.lblNet.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
			// 
			// xrLabelGSTInfo
			// 
			this.xrLabelGSTInfo.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.xrLabelGSTInfo.LocationFloat = new DevExpress.Utils.PointFloat(0F, 188.4999F);
			this.xrLabelGSTInfo.Name = "xrLabelGSTInfo";
			this.xrLabelGSTInfo.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
			this.xrLabelGSTInfo.SizeF = new System.Drawing.SizeF(238.5417F, 14.66668F);
			this.xrLabelGSTInfo.StylePriority.UseFont = false;
			this.xrLabelGSTInfo.StylePriority.UseTextAlignment = false;
			this.xrLabelGSTInfo.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft;
			// 
			// xrControlStyle2
			// 
			this.xrControlStyle2.BackColor = System.Drawing.Color.WhiteSmoke;
			this.xrControlStyle2.Name = "xrControlStyle2";
			this.xrControlStyle2.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
			// 
			// print_preview
			// 
			this.print_preview.Name = "print_preview";
			this.print_preview.Type = typeof(bool);
			this.print_preview.ValueInfo = "False";
			// 
			// xrSameGreat
			// 
			this.xrSameGreat.CanShrink = true;
			this.xrSameGreat.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.xrSameGreat.LocationFloat = new DevExpress.Utils.PointFloat(10.00001F, 10.00001F);
			this.xrSameGreat.Name = "xrSameGreat";
			this.xrSameGreat.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
			this.xrSameGreat.SerializableRtfString = resources.GetString("xrSameGreat.SerializableRtfString");
			this.xrSameGreat.SizeF = new System.Drawing.SizeF(729.9999F, 105.2917F);
			// 
			// podetailsdisp
			// 
			this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.ReportHeader,
            this.ReportFooter});
			this.DataMember = "po_details_current";
			this.DataSource = this.podetails1;
			this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Margins = new System.Drawing.Printing.Margins(50, 50, 83, 100);
			this.Parameters.AddRange(new DevExpress.XtraReports.Parameters.Parameter[] {
            this.detailspoprogid,
            this.print_preview});
			this.StyleSheet.AddRange(new DevExpress.XtraReports.UI.XRControlStyle[] {
            this.xrControlStyle1,
            this.xrControlStyle2});
			this.Version = "19.2";
			this.Watermark.Font = new System.Drawing.Font("Verdana", 80F);
			this.Watermark.ShowBehind = false;
			this.Watermark.Text = "REPRINT";
			this.Watermark.TextTransparency = 175;
			this.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.podisplay_BeforePrint);
			((System.ComponentModel.ISupportInitialize)(this.xrTable2)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.podetails1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.xrSameGreat)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this)).EndInit();

			}

		#endregion

		private void podisplay_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
			{
			var tools = new Toolbox();
			var hst = 0.13;
			double taxes = 0;
			double total = 0;
			var intpoprogid = Convert.ToInt32(detailspoprogid.Value);
			var poprog = new NePOProg(intpoprogid);
			var POBusinessUnit = new NeBusinessUnit(poprog.business_unit_id);
			xrSameGreat.Html = string.IsNullOrEmpty(POBusinessUnit.RebrandMemo) ? "" : POBusinessUnit.RebrandMemo;
			xrSameGreat.Visible = POBusinessUnit.RebrandMemo != "";
			var country = POBusinessUnit.country;
			var buisnessnumber = POBusinessUnit.BusinessNumber;
			var sql = "SELECT IFNULL(SUM(po_details_qty_ordered * po_details_cost),0) AS SUMOFITEMS FROM po_details_current WHERE po_details_poprog_id = " + intpoprogid;
			var netamt = tools.getSQL_double(@"SELECT IFNULL(SUM(po_details_qty_ordered * po_details_cost),0) AS SUMOFITEMS FROM po_details_current  WHERE po_details_poprog_id =@v0", new object[] { intpoprogid.ToString() });
			lblNet.Text = netamt.ToString("C2");
			try
				{
				var desc = poprog.poprog_order_description;
				if (desc != "")
					{
					desc = "*** " + desc;
					}
				xrLabelNotes.Text = desc;
				}
			catch { }

			if (country == "CDN")
				{
				taxes = netamt * hst;
				lblHST.Text = taxes.ToString("C2");
				xrLabelGSTInfo.Text = "Business Number " + buisnessnumber;
				}
			else
				{
				lblHST.Visible = false;
				labelHST.Visible = false;
				}

			total = netamt;
			lblTotal.Text = total.ToString("C2");

//        NePOProg tempo = new NePOProg(Convert.ToInt32(intpoprogid.ToString()));
			
               

			if ((poprog.poprog_status == 1) && (!(bool)print_preview.Value))
				{

				xrTableCell2.Visible = false;
				xrTableCell3.Visible = false;
				//      xrTableCell6.Visible = false;
				//       xrTableCell10.Visible = false;
				lblHST.Text = "XXXXXXX";
				lblTotal.Text = "XXXXXXX";
				lblNet.Text = "XXXXXXX";
				}

			var detailstableadapster = new Nesi.Web.Reports.PurchaseOrder.podetailsTableAdapters.po_details_currentTableAdapter();
			detailstableadapster.Fill(podetails1.po_details_current, Convert.ToInt32(detailspoprogid.Value));



			}

		private void xrTableCell1_HtmlItemCreated(object sender, HtmlEventArgs e)
			{
            if (e.Brick.Text == "0.00")
                {
				e.ContentCell.InnerText = "";
				}

			}

    

		private void xrTableCell2_HtmlItemCreated_1(object sender, HtmlEventArgs e)
			{
			if (e.Brick.Text == "0.00")
				{
				e.ContentCell.InnerText = "";
				}
			}

		private void xrTableCell3_HtmlItemCreated(object sender, HtmlEventArgs e)
			{
            if (e.Brick.Text == "0.00")
                {
				e.ContentCell.InnerText = "";
				}
			}

   

		}
	}
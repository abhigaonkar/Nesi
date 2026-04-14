using System;
using DevExpress.XtraReports.UI;
using Nesi.Web.Reports.QuotePrintout;

namespace nesi.core.print
	{
	/// <summary>
	/// Summary description for ERQuoteDetail
	/// </summary>
	public class ERQuoteDetail : XtraReport
		{
		private DetailBand Detail; 
		private TopMarginBand TopMargin;
		private BottomMarginBand BottomMargin;
		private dsERQuoteDetail dsERQuoteDetail1;
		private Nesi.Web.Reports.QuotePrintout.dsERQuoteDetailTableAdapters.ERQuote_DetailsTableAdapter erQuote_DetailsTableAdapter1;
		private ReportFooterBand ReportFooter;
		private XRLabel xrLabel2;
		private XRLabel xrLabel3;
		private XRLabel xrLabel4;
		private XRLabel xrLabel5;
		private XRLabel xrLabel6;
		private XRLabel xrLabel7;
		private XRLabel xrLabel8;
		private DevExpress.XtraReports.Parameters.Parameter quoteid;
		private DevExpress.XtraReports.Parameters.Parameter rev;
		private ReportHeaderBand ReportHeader;
		private XRLabel xrLabel1;
		private XRLabel xrLabel9;
		private XRLabel xrLabel10;
		private XRLabel xrLabel11;
		private XRLabel xrLabel12;
		private XRControlStyle xrControlStyle1;
		private XRPanel xrPanel1;
		private FormattingRule formattingRule1;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		public ERQuoteDetail()
			{
			InitializeComponent();
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
			this.xrPanel1 = new DevExpress.XtraReports.UI.XRPanel();
			this.xrLabel5 = new DevExpress.XtraReports.UI.XRLabel();
			this.xrLabel2 = new DevExpress.XtraReports.UI.XRLabel();
			this.xrLabel3 = new DevExpress.XtraReports.UI.XRLabel();
			this.xrLabel4 = new DevExpress.XtraReports.UI.XRLabel();
			this.xrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
			this.xrLabel10 = new DevExpress.XtraReports.UI.XRLabel();
			this.xrLabel12 = new DevExpress.XtraReports.UI.XRLabel();
			this.xrLabel8 = new DevExpress.XtraReports.UI.XRLabel();
			this.xrLabel7 = new DevExpress.XtraReports.UI.XRLabel();
			this.xrLabel6 = new DevExpress.XtraReports.UI.XRLabel();
			this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
			this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
			this.dsERQuoteDetail1 = new dsERQuoteDetail();
			this.erQuote_DetailsTableAdapter1 = new Nesi.Web.Reports.QuotePrintout.dsERQuoteDetailTableAdapters.ERQuote_DetailsTableAdapter();
			this.ReportFooter = new DevExpress.XtraReports.UI.ReportFooterBand();
			this.quoteid = new DevExpress.XtraReports.Parameters.Parameter();
			this.rev = new DevExpress.XtraReports.Parameters.Parameter();
			this.ReportHeader = new DevExpress.XtraReports.UI.ReportHeaderBand();
			this.xrLabel11 = new DevExpress.XtraReports.UI.XRLabel();
			this.xrLabel9 = new DevExpress.XtraReports.UI.XRLabel();
			this.xrControlStyle1 = new DevExpress.XtraReports.UI.XRControlStyle();
			this.formattingRule1 = new DevExpress.XtraReports.UI.FormattingRule();
			((System.ComponentModel.ISupportInitialize)(this.dsERQuoteDetail1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
			// 
			// Detail
			// 
			this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
																						this.xrPanel1});
			this.Detail.HeightF = 54.58336F;
			this.Detail.Name = "Detail";
			this.Detail.OddStyleName = "xrControlStyle1";
			this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
			this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
			// 
			// xrPanel1
			// 
			this.xrPanel1.BorderColor = System.Drawing.Color.DimGray;
			this.xrPanel1.Borders = (((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top)
																			| DevExpress.XtraPrinting.BorderSide.Right)
																			| DevExpress.XtraPrinting.BorderSide.Bottom);
			this.xrPanel1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
																						this.xrLabel5,
																						this.xrLabel2,
																						this.xrLabel3,
																						this.xrLabel4,
																						this.xrLabel1,
																						this.xrLabel10,
																						this.xrLabel12});
			this.xrPanel1.LocationFloat = new DevExpress.Utils.PointFloat(1.916631F, 0F);
			this.xrPanel1.Name = "xrPanel1";
			this.xrPanel1.SizeF = new System.Drawing.SizeF(712.1667F, 54.58336F);
			this.xrPanel1.StylePriority.UseBorderColor = false;
			this.xrPanel1.StylePriority.UseBorders = false;
			// 
			// xrLabel5
			// 
			this.xrLabel5.Borders = DevExpress.XtraPrinting.BorderSide.None;
			this.xrLabel5.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
																							new DevExpress.XtraReports.UI.XRBinding("Text", null, "ERQuote_Details.notes")});
			this.xrLabel5.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Italic);
			this.xrLabel5.LocationFloat = new DevExpress.Utils.PointFloat(66.99982F, 29.08335F);
			this.xrLabel5.Name = "xrLabel5";
			this.xrLabel5.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
			this.xrLabel5.SizeF = new System.Drawing.SizeF(566.6668F, 22.99998F);
			this.xrLabel5.StylePriority.UseBorders = false;
			this.xrLabel5.StylePriority.UseFont = false;
			this.xrLabel5.Text = "xrLabel5";
			// 
			// xrLabel2
			// 
			this.xrLabel2.Borders = DevExpress.XtraPrinting.BorderSide.None;
			this.xrLabel2.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
																							new DevExpress.XtraReports.UI.XRBinding("Text", null, "ERQuote_Details.part_no")});
			this.xrLabel2.Font = new System.Drawing.Font("Arial", 10F);
			this.xrLabel2.LocationFloat = new DevExpress.Utils.PointFloat(66.99982F, 6.08333F);
			this.xrLabel2.Name = "xrLabel2";
			this.xrLabel2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
			this.xrLabel2.SizeF = new System.Drawing.SizeF(83.33333F, 23F);
			this.xrLabel2.StylePriority.UseBorders = false;
			this.xrLabel2.StylePriority.UseFont = false;
			this.xrLabel2.StylePriority.UseTextAlignment = false;
			this.xrLabel2.Text = "xrLabel2";
			this.xrLabel2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
			// 
			// xrLabel3
			// 
			this.xrLabel3.Borders = DevExpress.XtraPrinting.BorderSide.None;
			this.xrLabel3.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
																							new DevExpress.XtraReports.UI.XRBinding("Text", null, "ERQuote_Details.serial")});
			this.xrLabel3.Font = new System.Drawing.Font("Arial", 10F);
			this.xrLabel3.LocationFloat = new DevExpress.Utils.PointFloat(150.3332F, 6.08333F);
			this.xrLabel3.Name = "xrLabel3";
			this.xrLabel3.Padding = new DevExpress.XtraPrinting.PaddingInfo(4, 4, 0, 0, 100F);
			this.xrLabel3.SizeF = new System.Drawing.SizeF(100F, 23F);
			this.xrLabel3.StylePriority.UseBorders = false;
			this.xrLabel3.StylePriority.UseFont = false;
			this.xrLabel3.StylePriority.UsePadding = false;
			this.xrLabel3.StylePriority.UseTextAlignment = false;
			this.xrLabel3.Text = "xrLabel3";
			this.xrLabel3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
			// 
			// xrLabel4
			// 
			this.xrLabel4.Borders = DevExpress.XtraPrinting.BorderSide.None;
			this.xrLabel4.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
																							new DevExpress.XtraReports.UI.XRBinding("Text", null, "ERQuote_Details.description")});
			this.xrLabel4.Font = new System.Drawing.Font("Arial", 10F);
			this.xrLabel4.LocationFloat = new DevExpress.Utils.PointFloat(250.3332F, 6.08333F);
			this.xrLabel4.Name = "xrLabel4";
			this.xrLabel4.Padding = new DevExpress.XtraPrinting.PaddingInfo(8, 4, 0, 0, 100F);
			this.xrLabel4.SizeF = new System.Drawing.SizeF(369.0833F, 23F);
			this.xrLabel4.StylePriority.UseBorders = false;
			this.xrLabel4.StylePriority.UseFont = false;
			this.xrLabel4.StylePriority.UsePadding = false;
			this.xrLabel4.Text = "xrLabel4";
			// 
			// xrLabel1
			// 
			this.xrLabel1.Borders = DevExpress.XtraPrinting.BorderSide.None;
			this.xrLabel1.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
																							new DevExpress.XtraReports.UI.XRBinding("Text", null, "ERQuote_Details.consignment_id")});
			this.xrLabel1.Font = new System.Drawing.Font("Arial", 10F);
			this.xrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(2.083333F, 6.08333F);
			this.xrLabel1.Name = "xrLabel1";
			this.xrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
			this.xrLabel1.SizeF = new System.Drawing.SizeF(64.91648F, 22.99998F);
			this.xrLabel1.StylePriority.UseBorders = false;
			this.xrLabel1.StylePriority.UseFont = false;
			this.xrLabel1.StylePriority.UseTextAlignment = false;
			this.xrLabel1.Text = "xrLabel1";
			this.xrLabel1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
			// 
			// xrLabel10
			// 
			this.xrLabel10.Borders = DevExpress.XtraPrinting.BorderSide.None;
			this.xrLabel10.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
																								new DevExpress.XtraReports.UI.XRBinding("Text", null, "ERQuote_Details.repair_price", "{0:$0.00}")});
			this.xrLabel10.Font = new System.Drawing.Font("Arial", 9.75F);
			this.xrLabel10.LocationFloat = new DevExpress.Utils.PointFloat(623.0831F, 6.08333F);
			this.xrLabel10.Name = "xrLabel10";
			this.xrLabel10.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
			this.xrLabel10.SizeF = new System.Drawing.SizeF(85.08325F, 23.00002F);
			this.xrLabel10.StylePriority.UseBorders = false;
			this.xrLabel10.StylePriority.UseFont = false;
			this.xrLabel10.StylePriority.UseTextAlignment = false;
			this.xrLabel10.Text = "xrLabel10";
			this.xrLabel10.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
			// 
			// xrLabel12
			// 
			this.xrLabel12.Borders = DevExpress.XtraPrinting.BorderSide.None;
			this.xrLabel12.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold);
			this.xrLabel12.LocationFloat = new DevExpress.Utils.PointFloat(2.083333F, 29.08335F);
			this.xrLabel12.Name = "xrLabel12";
			this.xrLabel12.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
			this.xrLabel12.SizeF = new System.Drawing.SizeF(64.91648F, 23.00002F);
			this.xrLabel12.StylePriority.UseBorders = false;
			this.xrLabel12.StylePriority.UseFont = false;
			this.xrLabel12.Text = "Notes:";
			// 
			// xrLabel8
			// 
			this.xrLabel8.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
			this.xrLabel8.LocationFloat = new DevExpress.Utils.PointFloat(252.0833F, 3.178914E-05F);
			this.xrLabel8.Name = "xrLabel8";
			this.xrLabel8.Padding = new DevExpress.XtraPrinting.PaddingInfo(8, 4, 0, 0, 100F);
			this.xrLabel8.SizeF = new System.Drawing.SizeF(371.1666F, 23F);
			this.xrLabel8.StylePriority.UseFont = false;
			this.xrLabel8.StylePriority.UsePadding = false;
			this.xrLabel8.StylePriority.UseTextAlignment = false;
			this.xrLabel8.Text = "Description";
			this.xrLabel8.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
			// 
			// xrLabel7
			// 
			this.xrLabel7.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
			this.xrLabel7.LocationFloat = new DevExpress.Utils.PointFloat(152.0833F, 0F);
			this.xrLabel7.Name = "xrLabel7";
			this.xrLabel7.Padding = new DevExpress.XtraPrinting.PaddingInfo(4, 4, 0, 0, 100F);
			this.xrLabel7.SizeF = new System.Drawing.SizeF(100F, 23F);
			this.xrLabel7.StylePriority.UseFont = false;
			this.xrLabel7.StylePriority.UsePadding = false;
			this.xrLabel7.StylePriority.UseTextAlignment = false;
			this.xrLabel7.Text = "Serial No";
			this.xrLabel7.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
			// 
			// xrLabel6
			// 
			this.xrLabel6.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
			this.xrLabel6.LocationFloat = new DevExpress.Utils.PointFloat(68.75F, 3.178914E-05F);
			this.xrLabel6.Name = "xrLabel6";
			this.xrLabel6.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
			this.xrLabel6.SizeF = new System.Drawing.SizeF(83.33333F, 23F);
			this.xrLabel6.StylePriority.UseFont = false;
			this.xrLabel6.StylePriority.UseTextAlignment = false;
			this.xrLabel6.Text = "Master ID";
			this.xrLabel6.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
			// 
			// TopMargin
			// 
			this.TopMargin.HeightF = 0F;
			this.TopMargin.Name = "TopMargin";
			this.TopMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
			this.TopMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
			// 
			// BottomMargin
			// 
			this.BottomMargin.HeightF = 6F;
			this.BottomMargin.Name = "BottomMargin";
			this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
			this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
			// 
			// dsERQuoteDetail1
			// 
			this.dsERQuoteDetail1.DataSetName = "dsERQuoteDetail";
			this.dsERQuoteDetail1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
			// 
			// erQuote_DetailsTableAdapter1
			// 
			this.erQuote_DetailsTableAdapter1.ClearBeforeFill = true;
			// 
			// ReportFooter
			// 
			this.ReportFooter.HeightF = 4.166667F;
			this.ReportFooter.Name = "ReportFooter";
			// 
			// quoteid
			// 
			this.quoteid.Name = "quoteid";
			this.quoteid.Type = typeof(int);
			// 
			// rev
			// 
			this.rev.Name = "rev";
			this.rev.Type = typeof(int);
			// 
			// ReportHeader
			// 
			this.ReportHeader.BackColor = System.Drawing.Color.DimGray;
			this.ReportHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
																							this.xrLabel11,
																							this.xrLabel9,
																							this.xrLabel6,
																							this.xrLabel7,
																							this.xrLabel8});
			this.ReportHeader.ForeColor = System.Drawing.Color.White;
			this.ReportHeader.HeightF = 23.00003F;
			this.ReportHeader.Name = "ReportHeader";
			this.ReportHeader.StylePriority.UseBackColor = false;
			this.ReportHeader.StylePriority.UseForeColor = false;
			// 
			// xrLabel11
			// 
			this.xrLabel11.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
			this.xrLabel11.LocationFloat = new DevExpress.Utils.PointFloat(623.2499F, 0F);
			this.xrLabel11.Name = "xrLabel11";
			this.xrLabel11.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
			this.xrLabel11.SizeF = new System.Drawing.SizeF(90.83337F, 23F);
			this.xrLabel11.StylePriority.UseFont = false;
			this.xrLabel11.StylePriority.UseTextAlignment = false;
			this.xrLabel11.Text = "Repair Price";
			this.xrLabel11.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
			// 
			// xrLabel9
			// 
			this.xrLabel9.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
			this.xrLabel9.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
			this.xrLabel9.Name = "xrLabel9";
			this.xrLabel9.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
			this.xrLabel9.SizeF = new System.Drawing.SizeF(68.75F, 23F);
			this.xrLabel9.StylePriority.UseFont = false;
			this.xrLabel9.StylePriority.UseTextAlignment = false;
			this.xrLabel9.Text = "Repair ID";
			this.xrLabel9.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
			// 
			// xrControlStyle1
			// 
			this.xrControlStyle1.BackColor = System.Drawing.Color.WhiteSmoke;
			this.xrControlStyle1.Name = "xrControlStyle1";
			this.xrControlStyle1.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
			// 
			// formattingRule1
			// 
			this.formattingRule1.Name = "formattingRule1";
			// 
			// ERQuoteDetail
			// 
			this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
																		this.Detail,
																		this.TopMargin,
																		this.BottomMargin,
																		this.ReportFooter,
																		this.ReportHeader});
			this.DataAdapter = this.erQuote_DetailsTableAdapter1;
			this.DataMember = "ERQuote_Details";
			this.DataSource = this.dsERQuoteDetail1;
			this.FormattingRuleSheet.AddRange(new DevExpress.XtraReports.UI.FormattingRule[] {
																								this.formattingRule1});
			this.Margins = new System.Drawing.Printing.Margins(57, 77, 0, 6);
			this.Parameters.AddRange(new DevExpress.XtraReports.Parameters.Parameter[] {
																							this.quoteid,
																							this.rev});
			this.StyleSheet.AddRange(new DevExpress.XtraReports.UI.XRControlStyle[] {
																						this.xrControlStyle1});
			this.Version = "11.2";
			this.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.ERQuoteDetail_BeforePrint);
			((System.ComponentModel.ISupportInitialize)(this.dsERQuoteDetail1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this)).EndInit();

			}

		#endregion


		private void ERQuoteDetail_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
			{
			var quotedetailadapter = new Nesi.Web.Reports.QuotePrintout.dsERQuoteDetailTableAdapters.ERQuote_DetailsTableAdapter();
			quotedetailadapter.Fill(dsERQuoteDetail1.ERQuote_Details, Convert.ToInt32(quoteid.Value), Convert.ToInt32(rev.Value));


			}
		}
	}
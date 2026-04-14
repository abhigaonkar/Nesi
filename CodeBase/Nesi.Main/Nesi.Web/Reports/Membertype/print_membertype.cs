using System;
using DevExpress.XtraReports.UI;
using Nesi.Web.Reports.Membertype;

namespace nesi.core.print
	{
	/// <summary>
	/// Summary description for print_membertype
	/// </summary>
	public class print_membertype : XtraReport
		{
		private DetailBand Detail;
		private TopMarginBand TopMargin;
		private BottomMarginBand BottomMargin; 
		private XRLabel lbltitle;
		private XRLabel xrLabel1;
		private XRLabel lbl_hdr;
		private XRPictureBox xrPictureBox1;
		private PageFooterBand PageFooter;
		private XRPageInfo xrPageInfo1;
		private XRLabel xrLabel3;
		private XRLabel lblreports;
		private XRLabel xrLabel2;
		private ReportHeaderBand ReportHeader;
		private print_membertype_data print_membertype_data1;
		private Nesi.Web.Reports.Membertype.print_membertype_dataTableAdapters.core_responsibilitiesTableAdapter core_responsibilitiesTableAdapter1;
		private XRLabel xrLabel5;
		private XRLabel xrLabel4;
		private DevExpress.XtraReports.Parameters.Parameter mtid;
		private DevExpress.XtraReports.Parameters.Parameter prov;
		private XRLabel xrLabel8;
		private PageHeaderBand PageHeader;
		private XRLabel xrLabel9;
		private XRLabel lblobjective;
		private XRLabel xrLabel6;
		private XRSubreport xrSubreport1;
		private XRLabel xrLabel7;
		private XRLabel lbldate;
		private ReportFooterBand ReportFooter;
		private XRLabel xrLabel11;
		private XRLabel xrLabel10;
		private XRLabel lblquarterly;
		private XRLabel lblweekly;
		private XRLabel lblas_required;
		private XRLabel lblmonthly;
		private XRLabel lblannually;
		private DevExpress.XtraReports.Parameters.Parameter is_sub;
		private XRLabel lbldaily;
		private XRLabel xrLabel12;
		private XRLabel xrLabel14;
		private XRLabel xrLabel13;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		public print_membertype()
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
            string resourceFileName = "print_membertype.resx";
            DevExpress.XtraReports.UI.XRSummary xrSummary1 = new DevExpress.XtraReports.UI.XRSummary();
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.xrLabel12 = new DevExpress.XtraReports.UI.XRLabel();
            this.lbldaily = new DevExpress.XtraReports.UI.XRLabel();
            this.lblquarterly = new DevExpress.XtraReports.UI.XRLabel();
            this.lblweekly = new DevExpress.XtraReports.UI.XRLabel();
            this.lblas_required = new DevExpress.XtraReports.UI.XRLabel();
            this.lblmonthly = new DevExpress.XtraReports.UI.XRLabel();
            this.lblannually = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel11 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel5 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel4 = new DevExpress.XtraReports.UI.XRLabel();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.xrPictureBox1 = new DevExpress.XtraReports.UI.XRPictureBox();
            this.lbl_hdr = new DevExpress.XtraReports.UI.XRLabel();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.xrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
            this.lbltitle = new DevExpress.XtraReports.UI.XRLabel();
            this.PageFooter = new DevExpress.XtraReports.UI.PageFooterBand();
            this.lbldate = new DevExpress.XtraReports.UI.XRLabel();
            this.xrPageInfo1 = new DevExpress.XtraReports.UI.XRPageInfo();
            this.xrLabel3 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel2 = new DevExpress.XtraReports.UI.XRLabel();
            this.lblreports = new DevExpress.XtraReports.UI.XRLabel();
            this.ReportHeader = new DevExpress.XtraReports.UI.ReportHeaderBand();
            this.xrLabel14 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel13 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrSubreport1 = new DevExpress.XtraReports.UI.XRSubreport();
            this.xrLabel7 = new DevExpress.XtraReports.UI.XRLabel();
            this.lblobjective = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel6 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel8 = new DevExpress.XtraReports.UI.XRLabel();
            this.print_membertype_data1 = new print_membertype_data();
            this.core_responsibilitiesTableAdapter1 = new Nesi.Web.Reports.Membertype.print_membertype_dataTableAdapters.core_responsibilitiesTableAdapter();
            this.mtid = new DevExpress.XtraReports.Parameters.Parameter();
            this.prov = new DevExpress.XtraReports.Parameters.Parameter();
            this.PageHeader = new DevExpress.XtraReports.UI.PageHeaderBand();
            this.xrLabel9 = new DevExpress.XtraReports.UI.XRLabel();
            this.ReportFooter = new DevExpress.XtraReports.UI.ReportFooterBand();
            this.xrLabel10 = new DevExpress.XtraReports.UI.XRLabel();
            this.is_sub = new DevExpress.XtraReports.Parameters.Parameter();
            ((System.ComponentModel.ISupportInitialize)(this.print_membertype_data1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel12,
            this.lbldaily,
            this.lblquarterly,
            this.lblweekly,
            this.lblas_required,
            this.lblmonthly,
            this.lblannually,
            this.xrLabel11,
            this.xrLabel5,
            this.xrLabel4});
            this.Detail.HeightF = 43.01398F;
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrLabel12
            // 
            this.xrLabel12.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.xrLabel12.LocationFloat = new DevExpress.Utils.PointFloat(0.0002543131F, 0F);
            this.xrLabel12.Name = "xrLabel12";
            this.xrLabel12.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel12.SizeF = new System.Drawing.SizeF(32.29142F, 24.33336F);
            this.xrLabel12.StylePriority.UseFont = false;
            xrSummary1.FormatString = "{0:#,#}";
            xrSummary1.Func = DevExpress.XtraReports.UI.SummaryFunc.RecordNumber;
            xrSummary1.Running = DevExpress.XtraReports.UI.SummaryRunning.Report;
            this.xrLabel12.Summary = xrSummary1;
            this.xrLabel12.SummaryCalculated += new DevExpress.XtraReports.UI.TextFormatEventHandler(this.xrLabel12_SummaryCalculated);
            // 
            // lbldaily
            // 
            this.lbldaily.CanShrink = true;
            this.lbldaily.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "core_responsibilities.daily")});
            this.lbldaily.Font = new System.Drawing.Font("Arial", 8F);
            this.lbldaily.LocationFloat = new DevExpress.Utils.PointFloat(42.29207F, 23.29178F);
            this.lbldaily.Multiline = true;
            this.lbldaily.Name = "lbldaily";
            this.lbldaily.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lbldaily.SizeF = new System.Drawing.SizeF(594.1666F, 2.083336F);
            this.lbldaily.StylePriority.UseFont = false;
            this.lbldaily.Text = "lbldaily";
            this.lbldaily.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.lbldaily_BeforePrint);
            // 
            // lblquarterly
            // 
            this.lblquarterly.CanShrink = true;
            this.lblquarterly.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "core_responsibilities.quarterly")});
            this.lblquarterly.Font = new System.Drawing.Font("Arial", 8F);
            this.lblquarterly.LocationFloat = new DevExpress.Utils.PointFloat(42.29161F, 29.54178F);
            this.lblquarterly.Multiline = true;
            this.lblquarterly.Name = "lblquarterly";
            this.lblquarterly.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblquarterly.SizeF = new System.Drawing.SizeF(594.1663F, 2.083334F);
            this.lblquarterly.StylePriority.UseFont = false;
            this.lblquarterly.Text = "lblquarterly";
            this.lblquarterly.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.lblquarterly_BeforePrint);
            // 
            // lblweekly
            // 
            this.lblweekly.CanShrink = true;
            this.lblweekly.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "core_responsibilities.weekly")});
            this.lblweekly.Font = new System.Drawing.Font("Arial", 8F);
            this.lblweekly.LocationFloat = new DevExpress.Utils.PointFloat(42.29202F, 25.37511F);
            this.lblweekly.Multiline = true;
            this.lblweekly.Name = "lblweekly";
            this.lblweekly.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblweekly.SizeF = new System.Drawing.SizeF(594.1666F, 2.083334F);
            this.lblweekly.StylePriority.UseFont = false;
            this.lblweekly.Text = "lblweekly";
            this.lblweekly.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.lblweekly_BeforePrint);
            // 
            // lblas_required
            // 
            this.lblas_required.CanShrink = true;
            this.lblas_required.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "core_responsibilities.as_required")});
            this.lblas_required.Font = new System.Drawing.Font("Arial", 8F);
            this.lblas_required.LocationFloat = new DevExpress.Utils.PointFloat(42.29161F, 33.70845F);
            this.lblas_required.Multiline = true;
            this.lblas_required.Name = "lblas_required";
            this.lblas_required.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblas_required.SizeF = new System.Drawing.SizeF(594.1653F, 2.083332F);
            this.lblas_required.StylePriority.UseFont = false;
            this.lblas_required.Text = "lblas_required";
            this.lblas_required.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.lblas_required_BeforePrint);
            // 
            // lblmonthly
            // 
            this.lblmonthly.CanShrink = true;
            this.lblmonthly.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "core_responsibilities.monthly")});
            this.lblmonthly.Font = new System.Drawing.Font("Arial", 8F);
            this.lblmonthly.LocationFloat = new DevExpress.Utils.PointFloat(42.29161F, 27.45844F);
            this.lblmonthly.Multiline = true;
            this.lblmonthly.Name = "lblmonthly";
            this.lblmonthly.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblmonthly.SizeF = new System.Drawing.SizeF(594.1666F, 2.083334F);
            this.lblmonthly.StylePriority.UseFont = false;
            this.lblmonthly.Text = "lblmonthly";
            this.lblmonthly.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.lblmonthly_BeforePrint);
            // 
            // lblannually
            // 
            this.lblannually.CanShrink = true;
            this.lblannually.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "core_responsibilities.annually")});
            this.lblannually.Font = new System.Drawing.Font("Arial", 8F);
            this.lblannually.LocationFloat = new DevExpress.Utils.PointFloat(42.29161F, 31.62508F);
            this.lblannually.Multiline = true;
            this.lblannually.Name = "lblannually";
            this.lblannually.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblannually.SizeF = new System.Drawing.SizeF(594.1666F, 2.083336F);
            this.lblannually.StylePriority.UseFont = false;
            this.lblannually.Text = "lblannually";
            this.lblannually.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.lblannually_BeforePrint);
            // 
            // xrLabel11
            // 
            this.xrLabel11.CanShrink = true;
            this.xrLabel11.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "core_responsibilities.review_question")});
            this.xrLabel11.Font = new System.Drawing.Font("Arial", 8F);
            this.xrLabel11.LocationFloat = new DevExpress.Utils.PointFloat(42.29183F, 19.12508F);
            this.xrLabel11.Multiline = true;
            this.xrLabel11.Name = "xrLabel11";
            this.xrLabel11.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel11.SizeF = new System.Drawing.SizeF(594.1668F, 2.083334F);
            this.xrLabel11.StylePriority.UseFont = false;
            this.xrLabel11.Text = "xrLabel11";
            this.xrLabel11.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrLabel11_BeforePrint);
            // 
            // xrLabel5
            // 
            this.xrLabel5.CanShrink = true;
            this.xrLabel5.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "core_responsibilities.description")});
            this.xrLabel5.Font = new System.Drawing.Font("Arial", 8F);
            this.xrLabel5.LocationFloat = new DevExpress.Utils.PointFloat(42.29183F, 9.916656F);
            this.xrLabel5.Multiline = true;
            this.xrLabel5.Name = "xrLabel5";
            this.xrLabel5.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel5.SizeF = new System.Drawing.SizeF(594.1665F, 2.083333F);
            this.xrLabel5.StylePriority.UseFont = false;
            this.xrLabel5.Text = "xrLabel5";
            // 
            // xrLabel4
            // 
            this.xrLabel4.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "core_responsibilities.core_responsibility")});
            this.xrLabel4.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.xrLabel4.LocationFloat = new DevExpress.Utils.PointFloat(32.29167F, 0F);
            this.xrLabel4.Name = "xrLabel4";
            this.xrLabel4.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel4.SizeF = new System.Drawing.SizeF(604.1664F, 7.375F);
            this.xrLabel4.Text = "xrLabel4";
            // 
            // TopMargin
            // 
            this.TopMargin.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrPictureBox1,
            this.lbl_hdr});
            this.TopMargin.HeightF = 139F;
            this.TopMargin.Name = "TopMargin";
            this.TopMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.TopMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrPictureBox1
            // 
            this.xrPictureBox1.ImageUrl = Toolbox.app_setting("Domain") + @"/images/Logos/nesi-logo-blue-invert.png";
            this.xrPictureBox1.LocationFloat = new DevExpress.Utils.PointFloat(9.536743E-05F, 10.41668F);
            this.xrPictureBox1.Name = "xrPictureBox1";
            this.xrPictureBox1.SizeF = new System.Drawing.SizeF(179.1667F, 128.125F);
            this.xrPictureBox1.Sizing = DevExpress.XtraPrinting.ImageSizeMode.ZoomImage;
            // 
            // lbl_hdr
            // 
            this.lbl_hdr.Font = new System.Drawing.Font("Arial", 9F);
            this.lbl_hdr.LocationFloat = new DevExpress.Utils.PointFloat(179.1668F, 59.375F);
            this.lbl_hdr.Name = "lbl_hdr";
            this.lbl_hdr.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lbl_hdr.SizeF = new System.Drawing.SizeF(470.8333F, 23F);
            this.lbl_hdr.StylePriority.UseFont = false;
            this.lbl_hdr.StylePriority.UseTextAlignment = false;
            this.lbl_hdr.Text = "Job Description";
            this.lbl_hdr.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            // 
            // BottomMargin
            // 
            this.BottomMargin.HeightF = 100F;
            this.BottomMargin.Name = "BottomMargin";
            this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrLabel1
            // 
            this.xrLabel1.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold);
            this.xrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(9.536743E-05F, 48.16672F);
            this.xrLabel1.Name = "xrLabel1";
            this.xrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel1.SizeF = new System.Drawing.SizeF(100F, 23F);
            this.xrLabel1.StylePriority.UseFont = false;
            this.xrLabel1.Text = "Member Type:";
            // 
            // lbltitle
            // 
            this.lbltitle.Font = new System.Drawing.Font("Arial", 9.75F);
            this.lbltitle.LocationFloat = new DevExpress.Utils.PointFloat(100.0001F, 48.16672F);
            this.lbltitle.Name = "lbltitle";
            this.lbltitle.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lbltitle.SizeF = new System.Drawing.SizeF(549.9999F, 22.99996F);
            this.lbltitle.StylePriority.UseFont = false;
            this.lbltitle.Text = "lbltitle";
            // 
            // PageFooter
            // 
            this.PageFooter.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.lbldate,
            this.xrPageInfo1});
            this.PageFooter.HeightF = 50F;
            this.PageFooter.Name = "PageFooter";
            // 
            // lbldate
            // 
            this.lbldate.Font = new System.Drawing.Font("Arial", 9.75F);
            this.lbldate.LocationFloat = new DevExpress.Utils.PointFloat(547.9167F, 9.999974F);
            this.lbldate.Name = "lbldate";
            this.lbldate.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lbldate.SizeF = new System.Drawing.SizeF(100F, 23F);
            this.lbldate.StylePriority.UseFont = false;
            this.lbldate.StylePriority.UseTextAlignment = false;
            this.lbldate.Text = "lbldate";
            this.lbldate.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            // 
            // xrPageInfo1
            // 
            this.xrPageInfo1.Font = new System.Drawing.Font("Arial", 9.75F);
            this.xrPageInfo1.LocationFloat = new DevExpress.Utils.PointFloat(271.875F, 9.999974F);
            this.xrPageInfo1.Name = "xrPageInfo1";
            this.xrPageInfo1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrPageInfo1.SizeF = new System.Drawing.SizeF(100F, 23F);
            this.xrPageInfo1.StylePriority.UseFont = false;
            this.xrPageInfo1.StylePriority.UseTextAlignment = false;
            this.xrPageInfo1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // xrLabel3
            // 
            this.xrLabel3.BackColor = System.Drawing.Color.Black;
            this.xrLabel3.Font = new System.Drawing.Font("Arial", 14F);
            this.xrLabel3.ForeColor = System.Drawing.Color.White;
            this.xrLabel3.LocationFloat = new DevExpress.Utils.PointFloat(0.0002543131F, 10.00001F);
            this.xrLabel3.Name = "xrLabel3";
            this.xrLabel3.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel3.SizeF = new System.Drawing.SizeF(650F, 23F);
            this.xrLabel3.StylePriority.UseBackColor = false;
            this.xrLabel3.StylePriority.UseFont = false;
            this.xrLabel3.StylePriority.UseForeColor = false;
            this.xrLabel3.StylePriority.UseTextAlignment = false;
            this.xrLabel3.Text = "Job Description";
            this.xrLabel3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // xrLabel2
            // 
            this.xrLabel2.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold);
            this.xrLabel2.LocationFloat = new DevExpress.Utils.PointFloat(9.536743E-05F, 71.16664F);
            this.xrLabel2.Name = "xrLabel2";
            this.xrLabel2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel2.SizeF = new System.Drawing.SizeF(100F, 23F);
            this.xrLabel2.StylePriority.UseFont = false;
            this.xrLabel2.Text = "Reports To:";
            // 
            // lblreports
            // 
            this.lblreports.Font = new System.Drawing.Font("Arial", 9.75F);
            this.lblreports.LocationFloat = new DevExpress.Utils.PointFloat(100F, 71.16674F);
            this.lblreports.Name = "lblreports";
            this.lblreports.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblreports.SizeF = new System.Drawing.SizeF(550F, 23F);
            this.lblreports.StylePriority.UseFont = false;
            this.lblreports.Text = "lbltitle";
            // 
            // ReportHeader
            // 
            this.ReportHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel14,
            this.xrLabel13,
            this.xrSubreport1,
            this.xrLabel7,
            this.lblobjective,
            this.xrLabel6,
            this.xrLabel8,
            this.xrLabel3,
            this.lblreports,
            this.lbltitle,
            this.xrLabel2,
            this.xrLabel1});
            this.ReportHeader.HeightF = 273.1251F;
            this.ReportHeader.Name = "ReportHeader";
            // 
            // xrLabel14
            // 
            this.xrLabel14.Font = new System.Drawing.Font("Arial", 8F);
            this.xrLabel14.LocationFloat = new DevExpress.Utils.PointFloat(305.2083F, 221.6667F);
            this.xrLabel14.Multiline = true;
            this.xrLabel14.Name = "xrLabel14";
            this.xrLabel14.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel14.SizeF = new System.Drawing.SizeF(253.125F, 51.45834F);
            this.xrLabel14.StylePriority.UseFont = false;
            this.xrLabel14.Text = "DOM: Director of Operations (Mature Branches)\r\nISR: Inside Sales Rep\r\nOSR: Outsid" +
    "e Sales Rep\r\nRAM: Regional Account Manager";
            // 
            // xrLabel13
            // 
            this.xrLabel13.Font = new System.Drawing.Font("Arial", 8F);
            this.xrLabel13.LocationFloat = new DevExpress.Utils.PointFloat(32.29167F, 221.6667F);
            this.xrLabel13.Multiline = true;
            this.xrLabel13.Name = "xrLabel13";
            this.xrLabel13.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel13.SizeF = new System.Drawing.SizeF(253.125F, 51.45834F);
            this.xrLabel13.StylePriority.UseFont = false;
            this.xrLabel13.Text = "BM: Branch Manager\r\nPM: Project Manager\r\nDM: Department Manager\r\nRM: Regional Man" +
    "ager\r\n";
            // 
            // xrSubreport1
            // 
            this.xrSubreport1.LocationFloat = new DevExpress.Utils.PointFloat(0.0001271566F, 167.7916F);
            this.xrSubreport1.Name = "xrSubreport1";
            this.xrSubreport1.SizeF = new System.Drawing.SizeF(649.9999F, 23.00002F);
            // 
            // xrLabel7
            // 
            this.xrLabel7.BackColor = System.Drawing.Color.DarkGray;
            this.xrLabel7.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold);
            this.xrLabel7.ForeColor = System.Drawing.Color.White;
            this.xrLabel7.LocationFloat = new DevExpress.Utils.PointFloat(0F, 139.5833F);
            this.xrLabel7.Name = "xrLabel7";
            this.xrLabel7.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel7.SizeF = new System.Drawing.SizeF(649.9999F, 23.00002F);
            this.xrLabel7.StylePriority.UseBackColor = false;
            this.xrLabel7.StylePriority.UseFont = false;
            this.xrLabel7.StylePriority.UseForeColor = false;
            this.xrLabel7.StylePriority.UseTextAlignment = false;
            this.xrLabel7.Text = "Qualifications:";
            this.xrLabel7.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // lblobjective
            // 
            this.lblobjective.Font = new System.Drawing.Font("Arial", 9.75F);
            this.lblobjective.LocationFloat = new DevExpress.Utils.PointFloat(100.0001F, 94.16663F);
            this.lblobjective.Name = "lblobjective";
            this.lblobjective.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblobjective.SizeF = new System.Drawing.SizeF(550F, 23F);
            this.lblobjective.StylePriority.UseFont = false;
            this.lblobjective.Text = "lbltitle";
            // 
            // xrLabel6
            // 
            this.xrLabel6.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold);
            this.xrLabel6.LocationFloat = new DevExpress.Utils.PointFloat(0F, 94.16663F);
            this.xrLabel6.Name = "xrLabel6";
            this.xrLabel6.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel6.SizeF = new System.Drawing.SizeF(100F, 23F);
            this.xrLabel6.StylePriority.UseFont = false;
            this.xrLabel6.Text = "Objective:";
            // 
            // xrLabel8
            // 
            this.xrLabel8.BackColor = System.Drawing.Color.DarkGray;
            this.xrLabel8.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold);
            this.xrLabel8.ForeColor = System.Drawing.Color.White;
            this.xrLabel8.LocationFloat = new DevExpress.Utils.PointFloat(0F, 198.6667F);
            this.xrLabel8.Name = "xrLabel8";
            this.xrLabel8.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel8.SizeF = new System.Drawing.SizeF(650.0001F, 23F);
            this.xrLabel8.StylePriority.UseBackColor = false;
            this.xrLabel8.StylePriority.UseFont = false;
            this.xrLabel8.StylePriority.UseForeColor = false;
            this.xrLabel8.StylePriority.UseTextAlignment = false;
            this.xrLabel8.Text = "Core Responsibilities:";
            this.xrLabel8.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // print_membertype_data1
            // 
            this.print_membertype_data1.DataSetName = "print_membertype_data";
            this.print_membertype_data1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // core_responsibilitiesTableAdapter1
            // 
            this.core_responsibilitiesTableAdapter1.ClearBeforeFill = true;
            // 
            // mtid
            // 
            this.mtid.Name = "mtid";
            this.mtid.Type = typeof(short);
            this.mtid.ValueInfo = "0";
            // 
            // prov
            // 
            this.prov.Name = "prov";
            this.prov.ValueInfo = "ON";
            // 
            // PageHeader
            // 
            this.PageHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel9});
            this.PageHeader.HeightF = 40.625F;
            this.PageHeader.Name = "PageHeader";
            this.PageHeader.PrintOn = DevExpress.XtraReports.UI.PrintOnPages.NotWithReportHeader;
            // 
            // xrLabel9
            // 
            this.xrLabel9.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold);
            this.xrLabel9.LocationFloat = new DevExpress.Utils.PointFloat(0F, 10.00001F);
            this.xrLabel9.Name = "xrLabel9";
            this.xrLabel9.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel9.SizeF = new System.Drawing.SizeF(283.3333F, 23F);
            this.xrLabel9.StylePriority.UseFont = false;
            this.xrLabel9.Text = "Core Responsibilities Cont\'d...";
            // 
            // ReportFooter
            // 
            this.ReportFooter.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel10});
            this.ReportFooter.HeightF = 47.91667F;
            this.ReportFooter.Name = "ReportFooter";
            // 
            // xrLabel10
            // 
            this.xrLabel10.Font = new System.Drawing.Font("Arial", 8F);
            this.xrLabel10.LocationFloat = new DevExpress.Utils.PointFloat(0.0002543131F, 9.999974F);
            this.xrLabel10.Name = "xrLabel10";
            this.xrLabel10.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel10.SizeF = new System.Drawing.SizeF(647.9165F, 31.33334F);
            this.xrLabel10.StylePriority.UseFont = false;
            this.xrLabel10.Text = "The core responsibilities and qualifications for every membertype may change from" +
    " time to time without notice.  This document is not an employment contract.";
            // 
            // is_sub
            // 
            this.is_sub.Name = "is_sub";
            this.is_sub.Type = typeof(bool);
            this.is_sub.ValueInfo = "False";
            // 
            // print_membertype
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.PageFooter,
            this.ReportHeader,
            this.PageHeader,
            this.ReportFooter});
            this.DataAdapter = this.core_responsibilitiesTableAdapter1;
            this.DataMember = "core_responsibilities";
            this.DataSource = this.print_membertype_data1;
            this.Margins = new System.Drawing.Printing.Margins(93, 89, 139, 100);
            this.Parameters.AddRange(new DevExpress.XtraReports.Parameters.Parameter[] {
            this.mtid,
            this.is_sub,
            this.prov});
            this.Version = "14.2";
            this.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.print_membertype_BeforePrint);
            ((System.ComponentModel.ISupportInitialize)(this.print_membertype_data1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

			}

		#endregion

		private void print_membertype_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
			{
			var cr_tableadapater = new Nesi.Web.Reports.Membertype.print_membertype_dataTableAdapters.core_responsibilitiesTableAdapter();
			cr_tableadapater.Fill(print_membertype_data1.core_responsibilities, Convert.ToInt32(mtid.Value));

			var q = new print_qualifications();
			q.Parameters[0].Value = Convert.ToInt32(mtid.Value);
			q.Parameters[1].Value = prov.Value.ToString();
			xrSubreport1.ReportSource = q;

			if (Convert.ToBoolean(is_sub.Value) == true)
				{
				TopMargin.Visible = false;
				ReportHeader.Visible = false;
				ReportFooter.Visible = false;
				PageHeader.Visible = false;
				PageFooter.Visible = false;

				}

//		ERQuoteDetail erreport = new ERQuoteDetail();
//		erreport.Parameters[0].Value = Convert.ToInt32(this.quoteid.Value);
//		erreport.Parameters[1].Value = Convert.ToInt32(this.rev.Value);
//		this.xrSubreport1.ReportSource = erreport;

			}

		private void xrLabel11_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
			{

			var l = (XRLabel)sender;
			if (l.Text != "")
				{
				l.Text = "Evaluation Question: " + l.Text;
				}
			}

		private void lbldaily_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
			{
			var l = (XRLabel)sender;
			if (l.Text != "")
				{
				l.Text = "Daily: " + Environment.NewLine + l.Text.Trim();
				}
			}
		private void lblweekly_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
			{
			var l = (XRLabel)sender;
			if (l.Text != "")
				{
				l.Text = "Weekly: " + Environment.NewLine + l.Text.Trim();
				}
			}

		private void lblmonthly_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
			{
			var l = (XRLabel)sender;
			if (l.Text != "")
				{
				l.Text = "Monthly: " + Environment.NewLine + l.Text.Trim();
				}
			}

		private void lblquarterly_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
			{
			var l = (XRLabel)sender;
			if (l.Text != "")
				{
				l.Text = "Quarterly: " + Environment.NewLine + l.Text.Trim();
				}
			}

		private void lblannually_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
			{
			var l = (XRLabel)sender;
			if (l.Text != "")
				{
				l.Text = "Annually: " + Environment.NewLine + l.Text.Trim();
				}
			}

		private void lblas_required_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
			{
			var l = (XRLabel)sender;
			if (l.Text != "")
				{
				l.Text = "As Required: " + Environment.NewLine + l.Text.Trim();
				}
			}

		private void xrLabel12_SummaryCalculated(object sender, TextFormatEventArgs e)
			{
			e.Text = ((Convert.ToInt16(e.Value) ) + ".");

			}

	

	
		}
	}
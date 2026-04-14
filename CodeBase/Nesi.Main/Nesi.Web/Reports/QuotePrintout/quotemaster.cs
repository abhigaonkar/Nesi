using System;
using System.Drawing;
using DevExpress.Utils;
using DevExpress.XtraReports.UI;
using NESI.Common.Models;
using Nesi.Web.Reports.QuotePrintout;
using System.Drawing.Printing;

namespace nesi.core.print
{
	/// <summary>
	/// Summary description for test
	/// </summary>
	public class quotemaster : XtraReport
	{
		private DetailBand Detail;
		private TopMarginBand TopMargin;
		private BottomMarginBand BottomMargin;
		private ReportHeaderBand ReportHeader;
		private ReportFooterBand ReportFooter;
		private XRPanel xrPanel1;
		private XRPictureBox xrPictureBox1;
		private XRLabel xrLabel4;
		private XRLabel xrLabel5;
		private XRLabel xrLabel6;
		private XRLabel xrLabel7;
		private XRLabel xrLabel8;
		private XRLabel xrLabel12;
		private XRLabel xrLabel13;
		private XRLabel xrLabel11;
		private XRLabel xrLabel10;
		private XRLabel xrLabel17;
		private XRLabel xrLabel18;
		private XRLabel xrLabel16;
		private XRLabel xrLabel14;
		private XRLabel xrLabel19;
		private XRLabel xrLabel24;
		private XRLabel xrLabel28;
		private XRLabel xrLabel27;
		private XRLabel xrLabel30;
		private XRLabel xrLabel29;
		private XRLabel xrLabel33;
		private XRLabel xrLabel32;
		private XRLabel xrLabel9;
		private XRLabel xrLabel35;
		private DevExpress.XtraReports.Parameters.Parameter quoteid;
		private DevExpress.XtraReports.Parameters.Parameter rev;
		private XRLabel xrLabel57;
		private XRPictureBox xrPictureBox4;
		private XRPictureBox xrPictureBox5;
		private XRLabel xrLabel1;
		private XRLabel xrLabel2;
		private XRLabel xrLabel3;
		private XRLabel xrLabel25;
		private XRLabel xrLabel26;
		private DevExpress.XtraReports.Parameters.Parameter type;
		private XRLabel xrLabel63;
		private XRLabel xrLabel64;
		private XRLabel xrLabel65;
		private XRLabel xrLabel67;
		private XRLabel xrLabel68;
		private XRLabel xrLabel69;
		private XRLabel lblCustomerTerm;
		private XRLabel xrLabel73;
		private XRLabel xrLabel74;
		private XRLabel lbldaysofcredit;
		private dsquotemaster dsquotemaster1;
		private XRLabel xrLabel70;
		private XRLabel xrLabel71;
		private XRLabel lblpricestringoffset;
		private XRLabel lblpricestring;
		private XRPanel signingPanel;
		private XRLabel xrLabel72;
		private XRLine xrLine2;
		private XRLabel xrLabel56;
		private XRLabel xrLabel58;
		private XRLine xrLine1;
		private PageFooterBand PageFooter;
		private PageHeaderBand PageHeader;
		private XRLabel xrLabel20;
		private XRLabel xrLabel15;
		private XRLabel xrLabel23;
		private XRPictureBox xrPictureBox3;
		private XRLabel xrLabel34;
		private XRPanel xrPanel2;
		private XRPictureBox xrPictureBox2;
		private XRPageInfo xrPageInfo1;
		private XRLabel lb_contact_title;
		private XRLabel lb_contact_title_lb;
		private DevExpress.XtraReports.Parameters.Parameter signoff;
		private XRLabel xrLabel21;
		private XRLabel xrLabel22;
		private XRPanel preJuly18thTandC;
		private XRPanel xrPanel8;
		private XRLabel xrLabel53;
		private XRLabel xrLabel46;
		private XRPanel xrPanel12;
		private XRLabel lbCopperPricesNo;
		private XRLabel lbCopperPrices;
		private XRPanel xrPanel4;
		private XRLabel xrLabel39;
		private XRLabel xrLabel38;
		private XRPanel xrPanel11;
		private XRLabel xrLabel47;
		private XRLabel xrLabel52;
		private XRPanel xrPanel7;
		private XRLabel xrLabel45;
		private XRLabel xrLabel44;
		private XRPanel xrPanel9;
		private XRLabel xrLabel49;
		private XRLabel xrLabel48;
		private XRPanel xrPanel3;
		private XRLabel xrLabel37;
		private XRLabel xrLabel36;
		private XRLabel xrLabel36m;
		private XRPanel xrPanel6;
		private XRLabel xrLabel43;
		private XRLabel xrLabel42;
		private XRPanel xrPanel5;
		private XRLabel xrLabel41;
		private XRLabel xrLabel40;
		private XRLabel lb_training;
		private XRLabel lb_equipment;
		private XRLabel lb_equipment_no;
		private XRLabel lb_lien;
		private XRLabel lb_lien_no;
		private XRLabel lb_training_no;
		private XRLabel repemail;
		private XRLabel remitAdddress;
		public DevExpress.XtraReports.Parameters.Parameter remitToAddress;
		private XRPanel postJuly18TandC;
		private XRLabel xrLabel59;
		private XRLabel xrLabel60;
		private XRLabel xrLabel61;
		private XRLabel xrLabel62;
		private XRRichText xrSameGreat;
		private XRLabel QuoteValidPeriod;
		private XRLabel QuoteMaterialValidPeriod;
		private XRPageBreak pb;
		private XRRichText standardTandC;
		private Nesi.Web.Reports.QuotePrintout.dsQuotenotesTableAdapters.quote_notesTableAdapter quote_notesTableAdapter;
		private dsQuotenotes dsQuotenotes1;
		private Nesi.Web.Reports.QuotePrintout.quote_detailsTableAdapters.quote_extratextTableAdapter quote_extratextTableAdapter;
		private quote_details quote_details1;
		private dsERQuoteDetail dsERQuoteDetail1;
		private Nesi.Web.Reports.QuotePrintout.dsERQuoteDetailTableAdapters.ERQuote_DetailsTableAdapter eRQuote_DetailsTableAdapter;
		private XRSubreport notesReport;
		private XRSubreport scopeReport;
        private XRLabel xrTermsConditions;


        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

		public quotemaster()
		{
			InitializeComponent();
            //AddTermsConditionsFooter();
            LoadResource();
		}

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(quotemaster));
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.notesReport = new DevExpress.XtraReports.UI.XRSubreport();
            this.scopeReport = new DevExpress.XtraReports.UI.XRSubreport();
            this.quoteid = new DevExpress.XtraReports.Parameters.Parameter();
            this.rev = new DevExpress.XtraReports.Parameters.Parameter();
            this.signoff = new DevExpress.XtraReports.Parameters.Parameter();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.ReportHeader = new DevExpress.XtraReports.UI.ReportHeaderBand();
            this.remitAdddress = new DevExpress.XtraReports.UI.XRLabel();
            this.repemail = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel22 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel21 = new DevExpress.XtraReports.UI.XRLabel();
            this.lb_contact_title_lb = new DevExpress.XtraReports.UI.XRLabel();
            this.lb_contact_title = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel71 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel70 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel65 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel64 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel9 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel33 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel32 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel30 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel29 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel28 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel27 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel24 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel19 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel17 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel18 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel16 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel14 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel12 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel13 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel11 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel10 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel8 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel7 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel6 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel5 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel4 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrPanel1 = new DevExpress.XtraReports.UI.XRPanel();
            this.xrLabel63 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel26 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel25 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel3 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel2 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrPictureBox1 = new DevExpress.XtraReports.UI.XRPictureBox();
            this.remitToAddress = new DevExpress.XtraReports.Parameters.Parameter();
            this.ReportFooter = new DevExpress.XtraReports.UI.ReportFooterBand();
            this.QuoteMaterialValidPeriod = new DevExpress.XtraReports.UI.XRLabel();
            this.pb = new DevExpress.XtraReports.UI.XRPageBreak();
            this.postJuly18TandC = new DevExpress.XtraReports.UI.XRPanel();
            this.standardTandC = new DevExpress.XtraReports.UI.XRRichText();
            this.QuoteValidPeriod = new DevExpress.XtraReports.UI.XRLabel();
            this.lbldaysofcredit = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel74 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel73 = new DevExpress.XtraReports.UI.XRLabel();
            this.lblCustomerTerm = new DevExpress.XtraReports.UI.XRLabel();
            this.xrTermsConditions = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel69 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel68 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel67 = new DevExpress.XtraReports.UI.XRLabel();
            this.lblpricestringoffset = new DevExpress.XtraReports.UI.XRLabel();
            this.lblpricestring = new DevExpress.XtraReports.UI.XRLabel();
            this.xrPictureBox5 = new DevExpress.XtraReports.UI.XRPictureBox();
            this.xrPictureBox4 = new DevExpress.XtraReports.UI.XRPictureBox();
            this.xrLabel57 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel35 = new DevExpress.XtraReports.UI.XRLabel();
            this.signingPanel = new DevExpress.XtraReports.UI.XRPanel();
            this.xrLabel59 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel60 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel61 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel62 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrSameGreat = new DevExpress.XtraReports.UI.XRRichText();
            this.xrLabel72 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLine2 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLabel56 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel58 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLine1 = new DevExpress.XtraReports.UI.XRLine();
            this.preJuly18thTandC = new DevExpress.XtraReports.UI.XRPanel();
            this.xrPanel8 = new DevExpress.XtraReports.UI.XRPanel();
            this.xrLabel53 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel46 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrPanel12 = new DevExpress.XtraReports.UI.XRPanel();
            this.lb_training = new DevExpress.XtraReports.UI.XRLabel();
            this.lb_equipment = new DevExpress.XtraReports.UI.XRLabel();
            this.lb_equipment_no = new DevExpress.XtraReports.UI.XRLabel();
            this.lb_lien = new DevExpress.XtraReports.UI.XRLabel();
            this.lb_lien_no = new DevExpress.XtraReports.UI.XRLabel();
            this.lb_training_no = new DevExpress.XtraReports.UI.XRLabel();
            this.lbCopperPricesNo = new DevExpress.XtraReports.UI.XRLabel();
            this.lbCopperPrices = new DevExpress.XtraReports.UI.XRLabel();
            this.xrPanel4 = new DevExpress.XtraReports.UI.XRPanel();
            this.xrLabel39 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel38 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrPanel11 = new DevExpress.XtraReports.UI.XRPanel();
            this.xrLabel47 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel52 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrPanel7 = new DevExpress.XtraReports.UI.XRPanel();
            this.xrLabel45 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel44 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrPanel9 = new DevExpress.XtraReports.UI.XRPanel();
            this.xrLabel49 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel48 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrPanel3 = new DevExpress.XtraReports.UI.XRPanel();
            this.xrLabel37 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel36m = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel36 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrPanel6 = new DevExpress.XtraReports.UI.XRPanel();
            this.xrLabel43 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel42 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrPanel5 = new DevExpress.XtraReports.UI.XRPanel();
            this.xrLabel41 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel40 = new DevExpress.XtraReports.UI.XRLabel();
            this.type = new DevExpress.XtraReports.Parameters.Parameter();
            this.dsquotemaster1 = new Nesi.Web.Reports.QuotePrintout.dsquotemaster();
            this.PageFooter = new DevExpress.XtraReports.UI.PageFooterBand();
            this.xrPanel2 = new DevExpress.XtraReports.UI.XRPanel();
            this.xrPictureBox2 = new DevExpress.XtraReports.UI.XRPictureBox();
            this.xrPageInfo1 = new DevExpress.XtraReports.UI.XRPageInfo();
            this.PageHeader = new DevExpress.XtraReports.UI.PageHeaderBand();
            this.xrLabel20 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel15 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel23 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrPictureBox3 = new DevExpress.XtraReports.UI.XRPictureBox();
            this.xrLabel34 = new DevExpress.XtraReports.UI.XRLabel();
            this.quote_notesTableAdapter = new Nesi.Web.Reports.QuotePrintout.dsQuotenotesTableAdapters.quote_notesTableAdapter();
            this.dsQuotenotes1 = new Nesi.Web.Reports.QuotePrintout.dsQuotenotes();
            this.quote_extratextTableAdapter = new Nesi.Web.Reports.QuotePrintout.quote_detailsTableAdapters.quote_extratextTableAdapter();
            this.quote_details1 = new Nesi.Web.Reports.QuotePrintout.quote_details();
            this.dsERQuoteDetail1 = new Nesi.Web.Reports.QuotePrintout.dsERQuoteDetail();
            this.eRQuote_DetailsTableAdapter = new Nesi.Web.Reports.QuotePrintout.dsERQuoteDetailTableAdapters.ERQuote_DetailsTableAdapter();
            ((System.ComponentModel.ISupportInitialize)(this.standardTandC)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrSameGreat)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dsquotemaster1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dsQuotenotes1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.quote_details1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dsERQuoteDetail1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.notesReport,
            this.scopeReport});
            this.Detail.Dpi = 96F;
            this.Detail.HeightF = 162.9853F;
            this.Detail.HierarchyPrintOptions.Indent = 0F;
            this.Detail.Name = "Detail";
            this.Detail.StylePriority.UsePadding = false;
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopJustify;
            // 
            // notesReport
            // 
            this.notesReport.CanShrink = true;
            this.notesReport.Dpi = 96F;
            this.notesReport.LocationFloat = new DevExpress.Utils.PointFloat(10F, 84.00348F);
            this.notesReport.Name = "notesReport";
            this.notesReport.ParameterBindings.Add(new DevExpress.XtraReports.UI.ParameterBinding("quoteid", null, null));
            this.notesReport.ParameterBindings.Add(new DevExpress.XtraReports.UI.ParameterBinding("rev", null, null));
            this.notesReport.ReportSource = new nesi.core.print.quotenotes();
            this.notesReport.SizeF = new System.Drawing.SizeF(693.7441F, 68.98181F);
            // 
            // scopeReport
            // 
            this.scopeReport.CanShrink = true;
            this.scopeReport.Dpi = 96F;
            this.scopeReport.LocationFloat = new DevExpress.Utils.PointFloat(10F, 10F);
            this.scopeReport.Name = "scopeReport";
            this.scopeReport.ParameterBindings.Add(new DevExpress.XtraReports.UI.ParameterBinding("quoteid", null, null));
            this.scopeReport.ParameterBindings.Add(new DevExpress.XtraReports.UI.ParameterBinding("rev", null, null));
            this.scopeReport.ParameterBindings.Add(new DevExpress.XtraReports.UI.ParameterBinding("signoff", null, null));
            this.scopeReport.ReportSource = new nesi.core.print.quotedetail();
            this.scopeReport.SizeF = new System.Drawing.SizeF(693.1373F, 53F);
            // 
            // quoteid
            // 
            this.quoteid.Name = "quoteid";
            this.quoteid.Type = typeof(int);
            this.quoteid.ValueInfo = "0";
            // 
            // rev
            // 
            this.rev.Name = "rev";
            this.rev.Type = typeof(int);
            this.rev.ValueInfo = "0";
            // 
            // signoff
            // 
            this.signoff.Name = "signoff";
            this.signoff.Type = typeof(short);
            this.signoff.ValueInfo = "0";
            // 
            // TopMargin
            // 
            this.TopMargin.Dpi = 96F;
            this.TopMargin.HeightF = 96F;
            this.TopMargin.Name = "TopMargin";
            this.TopMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 96F);
            this.TopMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // BottomMargin
            // 
            this.BottomMargin.Dpi = 96F;
            this.BottomMargin.HeightF = 48F;
            this.BottomMargin.Name = "BottomMargin";
            this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 96F);
            this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // ReportHeader
            // 
            this.ReportHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.remitAdddress,
            this.repemail,
            this.xrLabel22,
            this.xrLabel21,
            this.lb_contact_title_lb,
            this.lb_contact_title,
            this.xrLabel71,
            this.xrLabel70,
            this.xrLabel65,
            this.xrLabel64,
            this.xrLabel9,
            this.xrLabel33,
            this.xrLabel32,
            this.xrLabel30,
            this.xrLabel29,
            this.xrLabel28,
            this.xrLabel27,
            this.xrLabel24,
            this.xrLabel19,
            this.xrLabel17,
            this.xrLabel18,
            this.xrLabel16,
            this.xrLabel14,
            this.xrLabel12,
            this.xrLabel13,
            this.xrLabel11,
            this.xrLabel10,
            this.xrLabel8,
            this.xrLabel7,
            this.xrLabel6,
            this.xrLabel5,
            this.xrLabel4,
            this.xrPanel1});
            this.ReportHeader.Dpi = 96F;
            this.ReportHeader.HeightF = 528.12F;
            this.ReportHeader.KeepTogether = true;
            this.ReportHeader.Name = "ReportHeader";
            // 
            // remitAdddress
            // 
            this.remitAdddress.BackColor = System.Drawing.Color.Transparent;
            this.remitAdddress.Dpi = 96F;
            this.remitAdddress.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "?remitToAddress")});
            this.remitAdddress.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.remitAdddress.LocationFloat = new DevExpress.Utils.PointFloat(304.4616F, 395.5064F);
            this.remitAdddress.Multiline = true;
            this.remitAdddress.Name = "remitAdddress";
            this.remitAdddress.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
            this.remitAdddress.SizeF = new System.Drawing.SizeF(396.4226F, 77.07996F);
            this.remitAdddress.StylePriority.UseBackColor = false;
            this.remitAdddress.StylePriority.UseFont = false;
            this.remitAdddress.StylePriority.UseTextAlignment = false;
            this.remitAdddress.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // repemail
            // 
            this.repemail.BackColor = System.Drawing.Color.Transparent;
            this.repemail.Dpi = 96F;
            this.repemail.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "Iif(Not IsNullOrEmpty([bdm]),\'Sales rep: \' +[bdm],\'\')")});
            this.repemail.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.repemail.LocationFloat = new DevExpress.Utils.PointFloat(388.2934F, 329.8664F);
            this.repemail.Multiline = true;
            this.repemail.Name = "repemail";
            this.repemail.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
            this.repemail.SizeF = new System.Drawing.SizeF(311.147F, 32.11996F);
            this.repemail.StylePriority.UseBackColor = false;
            this.repemail.StylePriority.UseFont = false;
            this.repemail.StylePriority.UseTextAlignment = false;
            this.repemail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.repemail.Visible = false;
            // 
            // xrLabel22
            // 
            this.xrLabel22.BackColor = System.Drawing.Color.Transparent;
            this.xrLabel22.CanGrow = false;
            this.xrLabel22.CanShrink = true;
            this.xrLabel22.Dpi = 96F;
            this.xrLabel22.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Address_Addr3]")});
            this.xrLabel22.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.xrLabel22.LocationFloat = new DevExpress.Utils.PointFloat(25.09092F, 208.5601F);
            this.xrLabel22.Name = "xrLabel22";
            this.xrLabel22.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
            this.xrLabel22.SizeF = new System.Drawing.SizeF(324.2024F, 16.11964F);
            this.xrLabel22.StylePriority.UseBackColor = false;
            this.xrLabel22.StylePriority.UseFont = false;
            this.xrLabel22.StylePriority.UseTextAlignment = false;
            this.xrLabel22.Text = "xrLabel22";
            this.xrLabel22.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel21
            // 
            this.xrLabel21.BackColor = System.Drawing.Color.Transparent;
            this.xrLabel21.CanGrow = false;
            this.xrLabel21.CanShrink = true;
            this.xrLabel21.Dpi = 96F;
            this.xrLabel21.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Address_Addr2]")});
            this.xrLabel21.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.xrLabel21.LocationFloat = new DevExpress.Utils.PointFloat(25.81156F, 190.48F);
            this.xrLabel21.Name = "xrLabel21";
            this.xrLabel21.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
            this.xrLabel21.SizeF = new System.Drawing.SizeF(324.2024F, 18.07997F);
            this.xrLabel21.StylePriority.UseBackColor = false;
            this.xrLabel21.StylePriority.UseFont = false;
            this.xrLabel21.StylePriority.UseTextAlignment = false;
            this.xrLabel21.Text = "xrLabel21";
            this.xrLabel21.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // lb_contact_title_lb
            // 
            this.lb_contact_title_lb.BackColor = System.Drawing.Color.Transparent;
            this.lb_contact_title_lb.Dpi = 96F;
            this.lb_contact_title_lb.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.lb_contact_title_lb.LocationFloat = new DevExpress.Utils.PointFloat(23.99988F, 315.2396F);
            this.lb_contact_title_lb.Name = "lb_contact_title_lb";
            this.lb_contact_title_lb.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
            this.lb_contact_title_lb.SizeF = new System.Drawing.SizeF(67.99999F, 15.07986F);
            this.lb_contact_title_lb.StylePriority.UseBackColor = false;
            this.lb_contact_title_lb.StylePriority.UseFont = false;
            this.lb_contact_title_lb.StylePriority.UseTextAlignment = false;
            this.lb_contact_title_lb.Text = "Title:";
            this.lb_contact_title_lb.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // lb_contact_title
            // 
            this.lb_contact_title.Dpi = 96F;
            this.lb_contact_title.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Contact_Title]")});
            this.lb_contact_title.Font = new System.Drawing.Font("Arial", 9F);
            this.lb_contact_title.LocationFloat = new DevExpress.Utils.PointFloat(98.72044F, 315.2396F);
            this.lb_contact_title.Name = "lb_contact_title";
            this.lb_contact_title.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
            this.lb_contact_title.SizeF = new System.Drawing.SizeF(248.7411F, 17.58685F);
            this.lb_contact_title.StylePriority.UseFont = false;
            this.lb_contact_title.StylePriority.UseTextAlignment = false;
            this.lb_contact_title.Text = "lb_contact_title";
            this.lb_contact_title.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel71
            // 
            this.xrLabel71.CanShrink = true;
            this.xrLabel71.Dpi = 96F;
            this.xrLabel71.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Customer_Spec_Doc]")});
            this.xrLabel71.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel71.LocationFloat = new DevExpress.Utils.PointFloat(128.8386F, 363.9865F);
            this.xrLabel71.Multiline = true;
            this.xrLabel71.Name = "xrLabel71";
            this.xrLabel71.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
            this.xrLabel71.SizeF = new System.Drawing.SizeF(572.7664F, 29.5199F);
            this.xrLabel71.StylePriority.UseFont = false;
            this.xrLabel71.StylePriority.UseTextAlignment = false;
            this.xrLabel71.Text = "xrLabel71";
            this.xrLabel71.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel70
            // 
            this.xrLabel70.BackColor = System.Drawing.Color.Transparent;
            this.xrLabel70.Dpi = 96F;
            this.xrLabel70.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.xrLabel70.LocationFloat = new DevExpress.Utils.PointFloat(23.99999F, 363.9866F);
            this.xrLabel70.Name = "xrLabel70";
            this.xrLabel70.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
            this.xrLabel70.SizeF = new System.Drawing.SizeF(104.8386F, 29.51987F);
            this.xrLabel70.StylePriority.UseBackColor = false;
            this.xrLabel70.StylePriority.UseFont = false;
            this.xrLabel70.StylePriority.UseTextAlignment = false;
            this.xrLabel70.Text = "Customer Spec:";
            this.xrLabel70.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel65
            // 
            this.xrLabel65.Dpi = 96F;
            this.xrLabel65.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[FaxArea]")});
            this.xrLabel65.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel65.LocationFloat = new DevExpress.Utils.PointFloat(378.7247F, 248.7198F);
            this.xrLabel65.Name = "xrLabel65";
            this.xrLabel65.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
            this.xrLabel65.SizeF = new System.Drawing.SizeF(322.8801F, 16.15996F);
            this.xrLabel65.StylePriority.UseFont = false;
            this.xrLabel65.StylePriority.UseTextAlignment = false;
            this.xrLabel65.Text = "xrLabel65";
            this.xrLabel65.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // xrLabel64
            // 
            this.xrLabel64.Dpi = 96F;
            this.xrLabel64.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[PhoneArea]")});
            this.xrLabel64.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel64.LocationFloat = new DevExpress.Utils.PointFloat(378.7248F, 229.6399F);
            this.xrLabel64.Name = "xrLabel64";
            this.xrLabel64.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
            this.xrLabel64.SizeF = new System.Drawing.SizeF(322.88F, 18.07988F);
            this.xrLabel64.StylePriority.UseFont = false;
            this.xrLabel64.StylePriority.UseTextAlignment = false;
            this.xrLabel64.Text = "xrLabel64";
            this.xrLabel64.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // xrLabel9
            // 
            this.xrLabel9.BackColor = System.Drawing.Color.Transparent;
            this.xrLabel9.Dpi = 96F;
            this.xrLabel9.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Address_Postal]")});
            this.xrLabel9.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.xrLabel9.LocationFloat = new DevExpress.Utils.PointFloat(25.09092F, 243.7598F);
            this.xrLabel9.Name = "xrLabel9";
            this.xrLabel9.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
            this.xrLabel9.SizeF = new System.Drawing.SizeF(324.2024F, 18.07997F);
            this.xrLabel9.StylePriority.UseBackColor = false;
            this.xrLabel9.StylePriority.UseFont = false;
            this.xrLabel9.StylePriority.UseTextAlignment = false;
            this.xrLabel9.Text = "xrLabel9";
            this.xrLabel9.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel33
            // 
            this.xrLabel33.BackColor = System.Drawing.Color.Transparent;
            this.xrLabel33.Dpi = 96F;
            this.xrLabel33.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.xrLabel33.LocationFloat = new DevExpress.Utils.PointFloat(24.98615F, 261.8398F);
            this.xrLabel33.Name = "xrLabel33";
            this.xrLabel33.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
            this.xrLabel33.SizeF = new System.Drawing.SizeF(67.01379F, 18.0799F);
            this.xrLabel33.StylePriority.UseBackColor = false;
            this.xrLabel33.StylePriority.UseFont = false;
            this.xrLabel33.StylePriority.UseTextAlignment = false;
            this.xrLabel33.Text = "Phone:";
            this.xrLabel33.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel32
            // 
            this.xrLabel32.BackColor = System.Drawing.Color.Transparent;
            this.xrLabel32.Dpi = 96F;
            this.xrLabel32.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.xrLabel32.LocationFloat = new DevExpress.Utils.PointFloat(24.98615F, 280.9197F);
            this.xrLabel32.Name = "xrLabel32";
            this.xrLabel32.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
            this.xrLabel32.SizeF = new System.Drawing.SizeF(67.01379F, 16.15997F);
            this.xrLabel32.StylePriority.UseBackColor = false;
            this.xrLabel32.StylePriority.UseFont = false;
            this.xrLabel32.StylePriority.UseTextAlignment = false;
            this.xrLabel32.Text = "Fax:";
            this.xrLabel32.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel30
            // 
            this.xrLabel30.BackColor = System.Drawing.Color.Transparent;
            this.xrLabel30.Dpi = 96F;
            this.xrLabel30.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.xrLabel30.LocationFloat = new DevExpress.Utils.PointFloat(23.99997F, 347.9064F);
            this.xrLabel30.Name = "xrLabel30";
            this.xrLabel30.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
            this.xrLabel30.SizeF = new System.Drawing.SizeF(67.99993F, 15.07999F);
            this.xrLabel30.StylePriority.UseBackColor = false;
            this.xrLabel30.StylePriority.UseFont = false;
            this.xrLabel30.StylePriority.UseTextAlignment = false;
            this.xrLabel30.Text = "Email:";
            this.xrLabel30.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel29
            // 
            this.xrLabel29.BackColor = System.Drawing.Color.Transparent;
            this.xrLabel29.Dpi = 96F;
            this.xrLabel29.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Contact_Email]")});
            this.xrLabel29.Font = new System.Drawing.Font("Arial", 9F);
            this.xrLabel29.LocationFloat = new DevExpress.Utils.PointFloat(97.99994F, 348.9065F);
            this.xrLabel29.Name = "xrLabel29";
            this.xrLabel29.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
            this.xrLabel29.SizeF = new System.Drawing.SizeF(249.4616F, 15.08005F);
            this.xrLabel29.StylePriority.UseBackColor = false;
            this.xrLabel29.StylePriority.UseFont = false;
            this.xrLabel29.StylePriority.UseTextAlignment = false;
            this.xrLabel29.Text = "xrLabel29";
            this.xrLabel29.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel28
            // 
            this.xrLabel28.BackColor = System.Drawing.Color.Transparent;
            this.xrLabel28.Dpi = 96F;
            this.xrLabel28.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Contact_CellPhone]")});
            this.xrLabel28.Font = new System.Drawing.Font("Arial", 9F);
            this.xrLabel28.LocationFloat = new DevExpress.Utils.PointFloat(97.99994F, 332.8265F);
            this.xrLabel28.Name = "xrLabel28";
            this.xrLabel28.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
            this.xrLabel28.SizeF = new System.Drawing.SizeF(249.4616F, 15.08002F);
            this.xrLabel28.StylePriority.UseBackColor = false;
            this.xrLabel28.StylePriority.UseFont = false;
            this.xrLabel28.StylePriority.UseTextAlignment = false;
            this.xrLabel28.Text = "xrLabel28";
            this.xrLabel28.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel27
            // 
            this.xrLabel27.BackColor = System.Drawing.Color.Transparent;
            this.xrLabel27.Dpi = 96F;
            this.xrLabel27.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.xrLabel27.LocationFloat = new DevExpress.Utils.PointFloat(23.9999F, 331.8264F);
            this.xrLabel27.Name = "xrLabel27";
            this.xrLabel27.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
            this.xrLabel27.SizeF = new System.Drawing.SizeF(67.99995F, 15.08005F);
            this.xrLabel27.StylePriority.UseBackColor = false;
            this.xrLabel27.StylePriority.UseFont = false;
            this.xrLabel27.StylePriority.UseTextAlignment = false;
            this.xrLabel27.Text = "Phone:";
            this.xrLabel27.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel24
            // 
            this.xrLabel24.BackColor = System.Drawing.Color.Transparent;
            this.xrLabel24.Dpi = 96F;
            this.xrLabel24.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Bold);
            this.xrLabel24.LocationFloat = new DevExpress.Utils.PointFloat(27.97244F, 504.1201F);
            this.xrLabel24.Name = "xrLabel24";
            this.xrLabel24.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
            this.xrLabel24.SizeF = new System.Drawing.SizeF(673.6323F, 22.07996F);
            this.xrLabel24.StylePriority.UseBackColor = false;
            this.xrLabel24.StylePriority.UseFont = false;
            this.xrLabel24.StylePriority.UseTextAlignment = false;
            this.xrLabel24.Text = "We are pleased to submit the following for your evaluation:";
            this.xrLabel24.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel19
            // 
            this.xrLabel19.BackColor = System.Drawing.Color.Transparent;
            this.xrLabel19.Dpi = 96F;
            this.xrLabel19.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.xrLabel19.LocationFloat = new DevExpress.Utils.PointFloat(23.99994F, 299.1598F);
            this.xrLabel19.Name = "xrLabel19";
            this.xrLabel19.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
            this.xrLabel19.SizeF = new System.Drawing.SizeF(68.00002F, 15.07986F);
            this.xrLabel19.StylePriority.UseBackColor = false;
            this.xrLabel19.StylePriority.UseFont = false;
            this.xrLabel19.StylePriority.UseTextAlignment = false;
            this.xrLabel19.Text = "Attn:";
            this.xrLabel19.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel17
            // 
            this.xrLabel17.BackColor = System.Drawing.Color.Transparent;
            this.xrLabel17.Dpi = 96F;
            this.xrLabel17.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Member_NEEmail]")});
            this.xrLabel17.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.xrLabel17.LocationFloat = new DevExpress.Utils.PointFloat(389.7374F, 313.1601F);
            this.xrLabel17.Name = "xrLabel17";
            this.xrLabel17.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
            this.xrLabel17.SizeF = new System.Drawing.SizeF(311.147F, 15.08002F);
            this.xrLabel17.StylePriority.UseBackColor = false;
            this.xrLabel17.StylePriority.UseFont = false;
            this.xrLabel17.StylePriority.UseTextAlignment = false;
            this.xrLabel17.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // xrLabel18
            // 
            this.xrLabel18.BackColor = System.Drawing.Color.Transparent;
            this.xrLabel18.Dpi = 96F;
            this.xrLabel18.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[last_print_date]")});
            this.xrLabel18.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.xrLabel18.LocationFloat = new DevExpress.Utils.PointFloat(389.7372F, 281.9997F);
            this.xrLabel18.Name = "xrLabel18";
            this.xrLabel18.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
            this.xrLabel18.SizeF = new System.Drawing.SizeF(311.1475F, 15.08002F);
            this.xrLabel18.StylePriority.UseBackColor = false;
            this.xrLabel18.StylePriority.UseFont = false;
            this.xrLabel18.StylePriority.UseTextAlignment = false;
            this.xrLabel18.Text = "xrLabel18";
            this.xrLabel18.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrLabel18.TextFormatString = "{0:MMMM d, yyyy}";
            // 
            // xrLabel16
            // 
            this.xrLabel16.BackColor = System.Drawing.Color.Transparent;
            this.xrLabel16.Dpi = 96F;
            this.xrLabel16.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Member_FirstName]")});
            this.xrLabel16.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.xrLabel16.LocationFloat = new DevExpress.Utils.PointFloat(389.7372F, 297.0797F);
            this.xrLabel16.Name = "xrLabel16";
            this.xrLabel16.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
            this.xrLabel16.SizeF = new System.Drawing.SizeF(311.1471F, 15.08002F);
            this.xrLabel16.StylePriority.UseBackColor = false;
            this.xrLabel16.StylePriority.UseFont = false;
            this.xrLabel16.StylePriority.UseTextAlignment = false;
            this.xrLabel16.Text = "xrLabel16";
            this.xrLabel16.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // xrLabel14
            // 
            this.xrLabel14.BackColor = System.Drawing.Color.Transparent;
            this.xrLabel14.Dpi = 96F;
            this.xrLabel14.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Contact_Name]")});
            this.xrLabel14.Font = new System.Drawing.Font("Arial", 9F);
            this.xrLabel14.LocationFloat = new DevExpress.Utils.PointFloat(98.00004F, 300.1599F);
            this.xrLabel14.Name = "xrLabel14";
            this.xrLabel14.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
            this.xrLabel14.SizeF = new System.Drawing.SizeF(249.4615F, 15.07986F);
            this.xrLabel14.StylePriority.UseBackColor = false;
            this.xrLabel14.StylePriority.UseFont = false;
            this.xrLabel14.StylePriority.UseTextAlignment = false;
            this.xrLabel14.Text = "xrLabel14";
            this.xrLabel14.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel12
            // 
            this.xrLabel12.BackColor = System.Drawing.Color.Transparent;
            this.xrLabel12.Dpi = 96F;
            this.xrLabel12.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[postal]")});
            this.xrLabel12.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.xrLabel12.LocationFloat = new DevExpress.Utils.PointFloat(378.7247F, 210.5601F);
            this.xrLabel12.Name = "xrLabel12";
            this.xrLabel12.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
            this.xrLabel12.SizeF = new System.Drawing.SizeF(322.8801F, 18.07993F);
            this.xrLabel12.StylePriority.UseBackColor = false;
            this.xrLabel12.StylePriority.UseFont = false;
            this.xrLabel12.StylePriority.UseTextAlignment = false;
            this.xrLabel12.Text = "xrLabel12";
            this.xrLabel12.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // xrLabel13
            // 
            this.xrLabel13.BackColor = System.Drawing.Color.Transparent;
            this.xrLabel13.Dpi = 96F;
            this.xrLabel13.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[name]")});
            this.xrLabel13.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.xrLabel13.LocationFloat = new DevExpress.Utils.PointFloat(378.7248F, 153.3201F);
            this.xrLabel13.Name = "xrLabel13";
            this.xrLabel13.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
            this.xrLabel13.SizeF = new System.Drawing.SizeF(322.8802F, 18.08F);
            this.xrLabel13.StylePriority.UseBackColor = false;
            this.xrLabel13.StylePriority.UseFont = false;
            this.xrLabel13.StylePriority.UseTextAlignment = false;
            this.xrLabel13.Text = "xrLabel13";
            this.xrLabel13.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // xrLabel11
            // 
            this.xrLabel11.BackColor = System.Drawing.Color.Transparent;
            this.xrLabel11.Dpi = 96F;
            this.xrLabel11.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[City]")});
            this.xrLabel11.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.xrLabel11.LocationFloat = new DevExpress.Utils.PointFloat(378.7248F, 191.4799F);
            this.xrLabel11.Name = "xrLabel11";
            this.xrLabel11.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
            this.xrLabel11.SizeF = new System.Drawing.SizeF(322.8802F, 18.07997F);
            this.xrLabel11.StylePriority.UseBackColor = false;
            this.xrLabel11.StylePriority.UseFont = false;
            this.xrLabel11.StylePriority.UseTextAlignment = false;
            this.xrLabel11.Text = "xrLabel11";
            this.xrLabel11.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // xrLabel10
            // 
            this.xrLabel10.BackColor = System.Drawing.Color.Transparent;
            this.xrLabel10.Dpi = 96F;
            this.xrLabel10.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Address]")});
            this.xrLabel10.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.xrLabel10.LocationFloat = new DevExpress.Utils.PointFloat(378.7248F, 172.4001F);
            this.xrLabel10.Name = "xrLabel10";
            this.xrLabel10.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
            this.xrLabel10.SizeF = new System.Drawing.SizeF(322.8802F, 18.07999F);
            this.xrLabel10.StylePriority.UseBackColor = false;
            this.xrLabel10.StylePriority.UseFont = false;
            this.xrLabel10.StylePriority.UseTextAlignment = false;
            this.xrLabel10.Text = "xrLabel10";
            this.xrLabel10.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // xrLabel8
            // 
            this.xrLabel8.BackColor = System.Drawing.Color.Transparent;
            this.xrLabel8.Dpi = 96F;
            this.xrLabel8.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[faxn]")});
            this.xrLabel8.Font = new System.Drawing.Font("Arial", 9F);
            this.xrLabel8.LocationFloat = new DevExpress.Utils.PointFloat(97.99994F, 280.9197F);
            this.xrLabel8.Name = "xrLabel8";
            this.xrLabel8.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
            this.xrLabel8.SizeF = new System.Drawing.SizeF(249.4616F, 16.15991F);
            this.xrLabel8.StylePriority.UseBackColor = false;
            this.xrLabel8.StylePriority.UseFont = false;
            this.xrLabel8.StylePriority.UseTextAlignment = false;
            this.xrLabel8.Text = "xrLabel8";
            this.xrLabel8.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel7
            // 
            this.xrLabel7.BackColor = System.Drawing.Color.Transparent;
            this.xrLabel7.Dpi = 96F;
            this.xrLabel7.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[phonen]")});
            this.xrLabel7.Font = new System.Drawing.Font("Arial", 9F);
            this.xrLabel7.LocationFloat = new DevExpress.Utils.PointFloat(97.99994F, 261.8398F);
            this.xrLabel7.Name = "xrLabel7";
            this.xrLabel7.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
            this.xrLabel7.SizeF = new System.Drawing.SizeF(249.4616F, 18.0799F);
            this.xrLabel7.StylePriority.UseBackColor = false;
            this.xrLabel7.StylePriority.UseFont = false;
            this.xrLabel7.StylePriority.UseTextAlignment = false;
            this.xrLabel7.Text = "xrLabel7";
            this.xrLabel7.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel6
            // 
            this.xrLabel6.BackColor = System.Drawing.Color.Transparent;
            this.xrLabel6.Dpi = 96F;
            this.xrLabel6.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Address_City]")});
            this.xrLabel6.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.xrLabel6.LocationFloat = new DevExpress.Utils.PointFloat(25.09094F, 224.6797F);
            this.xrLabel6.Name = "xrLabel6";
            this.xrLabel6.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
            this.xrLabel6.SizeF = new System.Drawing.SizeF(324.2024F, 18.07999F);
            this.xrLabel6.StylePriority.UseBackColor = false;
            this.xrLabel6.StylePriority.UseFont = false;
            this.xrLabel6.StylePriority.UseTextAlignment = false;
            this.xrLabel6.Text = "xrLabel6";
            this.xrLabel6.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel5
            // 
            this.xrLabel5.BackColor = System.Drawing.Color.Transparent;
            this.xrLabel5.Dpi = 96F;
            this.xrLabel5.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Address_Addr1]")});
            this.xrLabel5.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.xrLabel5.LocationFloat = new DevExpress.Utils.PointFloat(25.09092F, 172.4001F);
            this.xrLabel5.Name = "xrLabel5";
            this.xrLabel5.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
            this.xrLabel5.SizeF = new System.Drawing.SizeF(324.2024F, 18.07999F);
            this.xrLabel5.StylePriority.UseBackColor = false;
            this.xrLabel5.StylePriority.UseFont = false;
            this.xrLabel5.StylePriority.UseTextAlignment = false;
            this.xrLabel5.Text = "xrLabel5";
            this.xrLabel5.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel4
            // 
            this.xrLabel4.BackColor = System.Drawing.Color.Transparent;
            this.xrLabel4.CanGrow = false;
            this.xrLabel4.CanShrink = true;
            this.xrLabel4.Dpi = 96F;
            this.xrLabel4.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Customer_Name]")});
            this.xrLabel4.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.xrLabel4.LocationFloat = new DevExpress.Utils.PointFloat(25.09092F, 153.3201F);
            this.xrLabel4.Name = "xrLabel4";
            this.xrLabel4.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
            this.xrLabel4.SizeF = new System.Drawing.SizeF(324.2024F, 18.08F);
            this.xrLabel4.StylePriority.UseBackColor = false;
            this.xrLabel4.StylePriority.UseFont = false;
            this.xrLabel4.StylePriority.UseTextAlignment = false;
            this.xrLabel4.Text = "xrLabel4";
            this.xrLabel4.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrPanel1
            // 
            this.xrPanel1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.xrPanel1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel63,
            this.xrLabel26,
            this.xrLabel25,
            this.xrLabel3,
            this.xrLabel2,
            this.xrLabel1,
            this.xrPictureBox1});
            this.xrPanel1.Dpi = 96F;
            this.xrPanel1.LocationFloat = new DevExpress.Utils.PointFloat(1.013901F, 1F);
            this.xrPanel1.Name = "xrPanel1";
            this.xrPanel1.SizeF = new System.Drawing.SizeF(718.986F, 134F);
            this.xrPanel1.StylePriority.UseBackColor = false;
            // 
            // xrLabel63
            // 
            this.xrLabel63.Dpi = 96F;
            this.xrLabel63.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Customer_Name]")});
            this.xrLabel63.Font = new System.Drawing.Font("Arial", 9.75F);
            this.xrLabel63.ForeColor = System.Drawing.Color.DimGray;
            this.xrLabel63.LocationFloat = new DevExpress.Utils.PointFloat(25.27964F, 64.76001F);
            this.xrLabel63.Name = "xrLabel63";
            this.xrLabel63.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
            this.xrLabel63.SizeF = new System.Drawing.SizeF(432.6395F, 20.07999F);
            this.xrLabel63.StylePriority.UseFont = false;
            this.xrLabel63.StylePriority.UseForeColor = false;
            this.xrLabel63.Text = "xrLabel63";
            // 
            // xrLabel26
            // 
            this.xrLabel26.Dpi = 96F;
            this.xrLabel26.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[revision]")});
            this.xrLabel26.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel26.LocationFloat = new DevExpress.Utils.PointFloat(123.6398F, 41.68002F);
            this.xrLabel26.Name = "xrLabel26";
            this.xrLabel26.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
            this.xrLabel26.SizeF = new System.Drawing.SizeF(186F, 19.08F);
            this.xrLabel26.StylePriority.UseFont = false;
            this.xrLabel26.StylePriority.UseTextAlignment = false;
            this.xrLabel26.Text = "xrLabel26";
            this.xrLabel26.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft;
            // 
            // xrLabel25
            // 
            this.xrLabel25.Dpi = 96F;
            this.xrLabel25.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[quote_id]")});
            this.xrLabel25.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel25.LocationFloat = new DevExpress.Utils.PointFloat(123.6398F, 19.6F);
            this.xrLabel25.Name = "xrLabel25";
            this.xrLabel25.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
            this.xrLabel25.SizeF = new System.Drawing.SizeF(186F, 22.08F);
            this.xrLabel25.StylePriority.UseFont = false;
            this.xrLabel25.StylePriority.UseTextAlignment = false;
            this.xrLabel25.Text = "xrLabel25";
            this.xrLabel25.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft;
            // 
            // xrLabel3
            // 
            this.xrLabel3.Dpi = 96F;
            this.xrLabel3.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[job_description]")});
            this.xrLabel3.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Bold);
            this.xrLabel3.ForeColor = System.Drawing.Color.DimGray;
            this.xrLabel3.LocationFloat = new DevExpress.Utils.PointFloat(23.63977F, 87.95999F);
            this.xrLabel3.Name = "xrLabel3";
            this.xrLabel3.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
            this.xrLabel3.SizeF = new System.Drawing.SizeF(475.3603F, 46.03998F);
            this.xrLabel3.StylePriority.UseFont = false;
            this.xrLabel3.StylePriority.UseForeColor = false;
            this.xrLabel3.Text = "xrLabel3";
            // 
            // xrLabel2
            // 
            this.xrLabel2.Dpi = 96F;
            this.xrLabel2.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel2.LocationFloat = new DevExpress.Utils.PointFloat(27.63977F, 41.68002F);
            this.xrLabel2.Name = "xrLabel2";
            this.xrLabel2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
            this.xrLabel2.SizeF = new System.Drawing.SizeF(95.99999F, 19.08F);
            this.xrLabel2.StylePriority.UseFont = false;
            this.xrLabel2.StylePriority.UseTextAlignment = false;
            this.xrLabel2.Text = "Version:";
            this.xrLabel2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft;
            // 
            // xrLabel1
            // 
            this.xrLabel1.Dpi = 96F;
            this.xrLabel1.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(27.63977F, 19.6F);
            this.xrLabel1.Name = "xrLabel1";
            this.xrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
            this.xrLabel1.SizeF = new System.Drawing.SizeF(95.99999F, 22.08F);
            this.xrLabel1.StylePriority.UseFont = false;
            this.xrLabel1.StylePriority.UseTextAlignment = false;
            this.xrLabel1.Text = "Quote:";
            this.xrLabel1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft;
            // 
            // xrPictureBox1
            // 
            this.xrPictureBox1.Dpi = 96F;
            this.xrPictureBox1.LocationFloat = new DevExpress.Utils.PointFloat(503.436F, 9.600009F);
            this.xrPictureBox1.Name = "xrPictureBox1";
            this.xrPictureBox1.SizeF = new System.Drawing.SizeF(188.6874F, 114.8F);
            this.xrPictureBox1.Sizing = DevExpress.XtraPrinting.ImageSizeMode.ZoomImage;
            // 
            // remitToAddress
            // 
            this.remitToAddress.Description = "Parameter1";
            this.remitToAddress.Name = "remitToAddress";
            // 
            // ReportFooter
            // 
            this.ReportFooter.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.QuoteMaterialValidPeriod,
            this.pb,
            this.postJuly18TandC,
            this.lbldaysofcredit,
            this.xrLabel74,
            this.xrLabel73,
            this.lblCustomerTerm,
            this.xrTermsConditions,
            this.xrLabel69,
            this.xrLabel68,
            this.xrLabel67,
            this.lblpricestringoffset,
            this.lblpricestring,
            this.xrPictureBox5,
            this.xrPictureBox4,
            this.xrLabel57,
            this.xrLabel35,
            this.signingPanel,
            this.preJuly18thTandC});
            this.ReportFooter.Dpi = 96F;
            this.ReportFooter.HeightF = 1292.517F;
            this.ReportFooter.Name = "ReportFooter";
            // 
            // QuoteMaterialValidPeriod
            // 
            this.QuoteMaterialValidPeriod.Dpi = 96F;
            this.QuoteMaterialValidPeriod.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.QuoteMaterialValidPeriod.LocationFloat = new DevExpress.Utils.PointFloat(21.29342F, 915.7198F);
            this.QuoteMaterialValidPeriod.Multiline = true;
            this.QuoteMaterialValidPeriod.Name = "QuoteMaterialValidPeriod";
            this.QuoteMaterialValidPeriod.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
            this.QuoteMaterialValidPeriod.SizeF = new System.Drawing.SizeF(677.5306F, 32.08002F);
            this.QuoteMaterialValidPeriod.StylePriority.UseFont = false;
            this.QuoteMaterialValidPeriod.Text = "The material prices used are based on current market values and may fluctuate bey" +
    "ond our control and as such they are valid for 5 days from the quote sent date.";
            this.QuoteMaterialValidPeriod.Visible = false;
            // 
            // pb
            // 
            this.pb.Dpi = 96F;
            this.pb.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.pb.Name = "pb";
            // 
            // postJuly18TandC
            // 
            this.postJuly18TandC.CanShrink = true;
            this.postJuly18TandC.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.standardTandC,
            this.QuoteValidPeriod});
            this.postJuly18TandC.Dpi = 96F;
            this.postJuly18TandC.LocationFloat = new DevExpress.Utils.PointFloat(21.69406F, 786.1896F);
            this.postJuly18TandC.Name = "postJuly18TandC";
            this.postJuly18TandC.SizeF = new System.Drawing.SizeF(679.1901F, 127.179F);
            // 
            // standardTandC
            // 
            this.standardTandC.CanShrink = true;
            this.standardTandC.Dpi = 96F;
            this.standardTandC.Font = new System.Drawing.Font("Arial", 9.75F);
            this.standardTandC.LocationFloat = new DevExpress.Utils.PointFloat(0F, 10F);
            this.standardTandC.Name = "standardTandC";
            this.standardTandC.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
            this.standardTandC.SerializableRtfString = resources.GetString("standardTandC.SerializableRtfString");
            this.standardTandC.SizeF = new System.Drawing.SizeF(675.1687F, 39.1087F);
            this.standardTandC.StylePriority.UseFont = false;
            // 
            // QuoteValidPeriod
            // 
            this.QuoteValidPeriod.Dpi = 96F;
            this.QuoteValidPeriod.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.QuoteValidPeriod.KeepTogether = true;
            this.QuoteValidPeriod.LocationFloat = new DevExpress.Utils.PointFloat(4.246216F, 65.099F);
            this.QuoteValidPeriod.Multiline = true;
            this.QuoteValidPeriod.Name = "QuoteValidPeriod";
            this.QuoteValidPeriod.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
            this.QuoteValidPeriod.SizeF = new System.Drawing.SizeF(600F, 36.24F);
            this.QuoteValidPeriod.StylePriority.UseFont = false;
            this.QuoteValidPeriod.Text = "Quote valid period";
            this.QuoteValidPeriod.Visible = false;
            // 
            // lbldaysofcredit
            // 
            this.lbldaysofcredit.Dpi = 96F;
            this.lbldaysofcredit.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[quote_term_net]")});
            this.lbldaysofcredit.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbldaysofcredit.LocationFloat = new DevExpress.Utils.PointFloat(144.9192F, 104.5736F);
            this.lbldaysofcredit.Multiline = true;
            this.lbldaysofcredit.Name = "lbldaysofcredit";
            this.lbldaysofcredit.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
            this.lbldaysofcredit.SizeF = new System.Drawing.SizeF(555.965F, 18.07983F);
            this.lbldaysofcredit.StylePriority.UseFont = false;
            this.lbldaysofcredit.StylePriority.UseTextAlignment = false;
            this.lbldaysofcredit.Text = "lbldaysofcredit";
            this.lbldaysofcredit.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel74
            // 
            this.xrLabel74.BackColor = System.Drawing.Color.Transparent;
            this.xrLabel74.Dpi = 96F;
            this.xrLabel74.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.xrLabel74.LocationFloat = new DevExpress.Utils.PointFloat(26.97244F, 104.5736F);
            this.xrLabel74.Name = "xrLabel74";
            this.xrLabel74.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
            this.xrLabel74.SizeF = new System.Drawing.SizeF(104.5865F, 18.07983F);
            this.xrLabel74.StylePriority.UseBackColor = false;
            this.xrLabel74.StylePriority.UseFont = false;
            this.xrLabel74.StylePriority.UseTextAlignment = false;
            this.xrLabel74.Text = "Payment Terms:";
            this.xrLabel74.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel73
            // 
            this.xrLabel73.BackColor = System.Drawing.Color.Transparent;
            this.xrLabel73.Dpi = 96F;
            this.xrLabel73.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.xrLabel73.LocationFloat = new DevExpress.Utils.PointFloat(26.97244F, 86.49371F);
            this.xrLabel73.Name = "xrLabel73";
            this.xrLabel73.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
            this.xrLabel73.SizeF = new System.Drawing.SizeF(104.5866F, 18.07983F);
            this.xrLabel73.StylePriority.UseBackColor = false;
            this.xrLabel73.StylePriority.UseFont = false;
            this.xrLabel73.StylePriority.UseTextAlignment = false;
            this.xrLabel73.Text = "Billing Schedule:";
            this.xrLabel73.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // lblCustomerTerm
            // 
            this.lblCustomerTerm.Dpi = 96F;
            this.lblCustomerTerm.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCustomerTerm.KeepTogether = true;
            this.lblCustomerTerm.LocationFloat = new DevExpress.Utils.PointFloat(144.9192F, 86.49371F);
            this.lblCustomerTerm.Multiline = true;
            this.lblCustomerTerm.Name = "lblCustomerTerm";
            this.lblCustomerTerm.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
            this.lblCustomerTerm.SizeF = new System.Drawing.SizeF(554.5212F, 18.07983F);
            this.lblCustomerTerm.StylePriority.UseFont = false;
            this.lblCustomerTerm.StylePriority.UseTextAlignment = false;
            this.lblCustomerTerm.Text = "lblCustomerTerm";
            this.lblCustomerTerm.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrTermsConditions
            // 
            this.xrTermsConditions.Dpi = 96F;
            this.xrTermsConditions.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTermsConditions.KeepTogether = true;
            this.xrTermsConditions.LocationFloat = new DevExpress.Utils.PointFloat(144.9192F, 86.49371F);
            this.xrTermsConditions.Multiline = true;
            this.xrTermsConditions.Name = "xrTermsConditions";
            this.xrTermsConditions.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
            this.xrTermsConditions.SizeF = new System.Drawing.SizeF(554.5212F, 18.07983F);
            this.xrTermsConditions.StylePriority.UseFont = false;
            this.xrTermsConditions.StylePriority.UseTextAlignment = false;
            this.xrTermsConditions.Text = "";
            this.xrTermsConditions.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel69
            // 
            this.xrLabel69.BackColor = System.Drawing.Color.Transparent;
            this.xrLabel69.Dpi = 96F;
            this.xrLabel69.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.xrLabel69.LocationFloat = new DevExpress.Utils.PointFloat(168.9191F, 68.41388F);
            this.xrLabel69.Name = "xrLabel69";
            this.xrLabel69.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
            this.xrLabel69.SizeF = new System.Drawing.SizeF(328.0948F, 18.07983F);
            this.xrLabel69.StylePriority.UseBackColor = false;
            this.xrLabel69.StylePriority.UseFont = false;
            this.xrLabel69.StylePriority.UseTextAlignment = false;
            this.xrLabel69.Text = "% Due Prior to Start of Project";
            this.xrLabel69.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel68
            // 
            this.xrLabel68.BackColor = System.Drawing.Color.Transparent;
            this.xrLabel68.Dpi = 96F;
            this.xrLabel68.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.xrLabel68.LocationFloat = new DevExpress.Utils.PointFloat(26.97244F, 68.41388F);
            this.xrLabel68.Name = "xrLabel68";
            this.xrLabel68.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
            this.xrLabel68.SizeF = new System.Drawing.SizeF(116.9467F, 18.07983F);
            this.xrLabel68.StylePriority.UseBackColor = false;
            this.xrLabel68.StylePriority.UseFont = false;
            this.xrLabel68.StylePriority.UseTextAlignment = false;
            this.xrLabel68.Text = "Percentage Down:";
            this.xrLabel68.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel67
            // 
            this.xrLabel67.Dpi = 96F;
            this.xrLabel67.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[percentage_down]")});
            this.xrLabel67.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel67.LocationFloat = new DevExpress.Utils.PointFloat(144.9192F, 68.41388F);
            this.xrLabel67.Name = "xrLabel67";
            this.xrLabel67.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
            this.xrLabel67.SizeF = new System.Drawing.SizeF(23.99998F, 18.07983F);
            this.xrLabel67.StylePriority.UseFont = false;
            this.xrLabel67.StylePriority.UseTextAlignment = false;
            this.xrLabel67.Text = "xrLabel67";
            this.xrLabel67.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // lblpricestringoffset
            // 
            this.lblpricestringoffset.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lblpricestringoffset.Dpi = 96F;
            this.lblpricestringoffset.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblpricestringoffset.LocationFloat = new DevExpress.Utils.PointFloat(21.29341F, 177.2536F);
            this.lblpricestringoffset.Name = "lblpricestringoffset";
            this.lblpricestringoffset.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
            this.lblpricestringoffset.SizeF = new System.Drawing.SizeF(122.6257F, 22.07996F);
            this.lblpricestringoffset.StylePriority.UseBackColor = false;
            this.lblpricestringoffset.StylePriority.UseForeColor = false;
            this.lblpricestringoffset.StylePriority.UseTextAlignment = false;
            this.lblpricestringoffset.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // lblpricestring
            // 
            this.lblpricestring.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lblpricestring.Dpi = 96F;
            this.lblpricestring.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblpricestring.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblpricestring.LocationFloat = new DevExpress.Utils.PointFloat(144.9192F, 177.2536F);
            this.lblpricestring.Name = "lblpricestring";
            this.lblpricestring.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
            this.lblpricestring.SizeF = new System.Drawing.SizeF(554.6669F, 22.07996F);
            this.lblpricestring.StylePriority.UseBackColor = false;
            this.lblpricestring.StylePriority.UseFont = false;
            this.lblpricestring.StylePriority.UseForeColor = false;
            this.lblpricestring.StylePriority.UseTextAlignment = false;
            this.lblpricestring.Text = "pricestring";
            this.lblpricestring.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrPictureBox5
            // 
            this.xrPictureBox5.Dpi = 96F;
            this.xrPictureBox5.ImageUrl = "~\\images\\reports\\filler\\mc.jpg";
            this.xrPictureBox5.LocationFloat = new DevExpress.Utils.PointFloat(394.0139F, 134.3336F);
            this.xrPictureBox5.Name = "xrPictureBox5";
            this.xrPictureBox5.SizeF = new System.Drawing.SizeF(57.63983F, 31.99999F);
            this.xrPictureBox5.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage;
            // 
            // xrPictureBox4
            // 
            this.xrPictureBox4.Dpi = 96F;
            this.xrPictureBox4.ImageUrl = "~\\images\\reports\\filler\\visa.jpg";
            this.xrPictureBox4.LocationFloat = new DevExpress.Utils.PointFloat(228.0138F, 134.3336F);
            this.xrPictureBox4.Name = "xrPictureBox4";
            this.xrPictureBox4.SizeF = new System.Drawing.SizeF(75.63989F, 32.00006F);
            this.xrPictureBox4.Sizing = DevExpress.XtraPrinting.ImageSizeMode.StretchImage;
            // 
            // xrLabel57
            // 
            this.xrLabel57.BackColor = System.Drawing.Color.Transparent;
            this.xrLabel57.Dpi = 96F;
            this.xrLabel57.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel57.LocationFloat = new DevExpress.Utils.PointFloat(25.98621F, 42.93335F);
            this.xrLabel57.Name = "xrLabel57";
            this.xrLabel57.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
            this.xrLabel57.SizeF = new System.Drawing.SizeF(674.898F, 22.07996F);
            this.xrLabel57.StylePriority.UseBackColor = false;
            this.xrLabel57.StylePriority.UseFont = false;
            this.xrLabel57.StylePriority.UseTextAlignment = false;
            this.xrLabel57.Text = "Project Specific Terms:";
            this.xrLabel57.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel35
            // 
            this.xrLabel35.BackColor = System.Drawing.Color.Transparent;
            this.xrLabel35.CanShrink = true;
            this.xrLabel35.Dpi = 96F;
            this.xrLabel35.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel35.LocationFloat = new DevExpress.Utils.PointFloat(21.29341F, 207.0136F);
            this.xrLabel35.Name = "xrLabel35";
            this.xrLabel35.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
            this.xrLabel35.SizeF = new System.Drawing.SizeF(677.5307F, 22.0799F);
            this.xrLabel35.StylePriority.UseBackColor = false;
            this.xrLabel35.StylePriority.UseFont = false;
            this.xrLabel35.StylePriority.UseTextAlignment = false;
            this.xrLabel35.Text = "Conditions:";
            this.xrLabel35.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // signingPanel
            // 
            this.signingPanel.CanShrink = true;
            this.signingPanel.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel59,
            this.xrLabel60,
            this.xrLabel61,
            this.xrLabel62,
            this.xrSameGreat,
            this.xrLabel72,
            this.xrLine2,
            this.xrLabel56,
            this.xrLabel58,
            this.xrLine1});
            this.signingPanel.Dpi = 96F;
            this.signingPanel.LocationFloat = new DevExpress.Utils.PointFloat(20.79871F, 947.7998F);
            this.signingPanel.Name = "signingPanel";
            this.signingPanel.SizeF = new System.Drawing.SizeF(677.0255F, 334.717F);
            // 
            // xrLabel59
            // 
            this.xrLabel59.BackColor = System.Drawing.Color.Transparent;
            this.xrLabel59.Dpi = 96F;
            this.xrLabel59.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel59.LocationFloat = new DevExpress.Utils.PointFloat(0.8953495F, 127.7276F);
            this.xrLabel59.Name = "xrLabel59";
            this.xrLabel59.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
            this.xrLabel59.SizeF = new System.Drawing.SizeF(598.4933F, 22.07983F);
            this.xrLabel59.StylePriority.UseBackColor = false;
            this.xrLabel59.StylePriority.UseFont = false;
            this.xrLabel59.StylePriority.UseTextAlignment = false;
            this.xrLabel59.Text = "Sincerely,";
            this.xrLabel59.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel60
            // 
            this.xrLabel60.BackColor = System.Drawing.Color.Transparent;
            this.xrLabel60.Dpi = 96F;
            this.xrLabel60.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[Member_FirstName]")});
            this.xrLabel60.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel60.LocationFloat = new DevExpress.Utils.PointFloat(0.8953495F, 150.8074F);
            this.xrLabel60.Name = "xrLabel60";
            this.xrLabel60.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
            this.xrLabel60.SizeF = new System.Drawing.SizeF(324.2933F, 22.08002F);
            this.xrLabel60.StylePriority.UseBackColor = false;
            this.xrLabel60.StylePriority.UseFont = false;
            this.xrLabel60.StylePriority.UseTextAlignment = false;
            this.xrLabel60.Text = "xrLabel16";
            this.xrLabel60.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel61
            // 
            this.xrLabel61.BackColor = System.Drawing.Color.Transparent;
            this.xrLabel61.Dpi = 96F;
            this.xrLabel61.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel61.LocationFloat = new DevExpress.Utils.PointFloat(0F, 172.8885F);
            this.xrLabel61.Name = "xrLabel61";
            this.xrLabel61.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
            this.xrLabel61.SizeF = new System.Drawing.SizeF(598.6686F, 22.07977F);
            this.xrLabel61.StylePriority.UseBackColor = false;
            this.xrLabel61.StylePriority.UseFont = false;
            this.xrLabel61.StylePriority.UseTextAlignment = false;
            this.xrLabel61.Text = "Project Manager";
            this.xrLabel61.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel62
            // 
            this.xrLabel62.BackColor = System.Drawing.Color.Transparent;
            this.xrLabel62.Dpi = 96F;
            this.xrLabel62.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[name]")});
            this.xrLabel62.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel62.LocationFloat = new DevExpress.Utils.PointFloat(0.8953495F, 194.9684F);
            this.xrLabel62.Name = "xrLabel62";
            this.xrLabel62.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
            this.xrLabel62.SizeF = new System.Drawing.SizeF(323.307F, 22.07996F);
            this.xrLabel62.StylePriority.UseBackColor = false;
            this.xrLabel62.StylePriority.UseFont = false;
            this.xrLabel62.StylePriority.UseTextAlignment = false;
            this.xrLabel62.Text = "xrLabel13";
            this.xrLabel62.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrSameGreat
            // 
            this.xrSameGreat.CanShrink = true;
            this.xrSameGreat.Dpi = 96F;
            this.xrSameGreat.Font = new System.Drawing.Font("Arial", 9.75F);
            this.xrSameGreat.LocationFloat = new DevExpress.Utils.PointFloat(0.8953552F, 231.8812F);
            this.xrSameGreat.Name = "xrSameGreat";
            this.xrSameGreat.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
            this.xrSameGreat.SerializableRtfString = resources.GetString("xrSameGreat.SerializableRtfString");
            this.xrSameGreat.SizeF = new System.Drawing.SizeF(675.1687F, 102.8358F);
            // 
            // xrLabel72
            // 
            this.xrLabel72.BackColor = System.Drawing.Color.Transparent;
            this.xrLabel72.Dpi = 96F;
            this.xrLabel72.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel72.LocationFloat = new DevExpress.Utils.PointFloat(411.3185F, 93.90909F);
            this.xrLabel72.Name = "xrLabel72";
            this.xrLabel72.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
            this.xrLabel72.SizeF = new System.Drawing.SizeF(262.9592F, 22.07983F);
            this.xrLabel72.StylePriority.UseBackColor = false;
            this.xrLabel72.StylePriority.UseFont = false;
            this.xrLabel72.StylePriority.UseTextAlignment = false;
            this.xrLabel72.Text = "Purchase Order";
            this.xrLabel72.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // xrLine2
            // 
            this.xrLine2.Dpi = 96F;
            this.xrLine2.LocationFloat = new DevExpress.Utils.PointFloat(469.4397F, 74.90907F);
            this.xrLine2.Name = "xrLine2";
            this.xrLine2.SizeF = new System.Drawing.SizeF(204.838F, 15F);
            // 
            // xrLabel56
            // 
            this.xrLabel56.AutoWidth = true;
            this.xrLabel56.BackColor = System.Drawing.Color.Transparent;
            this.xrLabel56.Dpi = 96F;
            this.xrLabel56.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel56.KeepTogether = true;
            this.xrLabel56.LocationFloat = new DevExpress.Utils.PointFloat(0F, 25.44F);
            this.xrLabel56.Name = "xrLabel56";
            this.xrLabel56.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
            this.xrLabel56.SizeF = new System.Drawing.SizeF(672.9455F, 49.46906F);
            this.xrLabel56.StylePriority.UseBackColor = false;
            this.xrLabel56.StylePriority.UseFont = false;
            this.xrLabel56.StylePriority.UseTextAlignment = false;
            this.xrLabel56.Text = "I have read the terms and conditions and agree on the price quoted to complete th" +
    "e above work:";
            this.xrLabel56.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel58
            // 
            this.xrLabel58.BackColor = System.Drawing.Color.Transparent;
            this.xrLabel58.Dpi = 96F;
            this.xrLabel58.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel58.LocationFloat = new DevExpress.Utils.PointFloat(5.495012F, 93.90909F);
            this.xrLabel58.Name = "xrLabel58";
            this.xrLabel58.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
            this.xrLabel58.SizeF = new System.Drawing.SizeF(350.4801F, 22.07983F);
            this.xrLabel58.StylePriority.UseBackColor = false;
            this.xrLabel58.StylePriority.UseFont = false;
            this.xrLabel58.StylePriority.UseTextAlignment = false;
            this.xrLabel58.Text = "Customer Signature";
            this.xrLabel58.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLine1
            // 
            this.xrLine1.Dpi = 96F;
            this.xrLine1.LocationFloat = new DevExpress.Utils.PointFloat(5.495012F, 74.90911F);
            this.xrLine1.Name = "xrLine1";
            this.xrLine1.SizeF = new System.Drawing.SizeF(212.6398F, 15F);
            // 
            // preJuly18thTandC
            // 
            this.preJuly18thTandC.CanShrink = true;
            this.preJuly18thTandC.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrPanel8,
            this.xrPanel12,
            this.xrPanel4,
            this.xrPanel11,
            this.xrPanel7,
            this.xrPanel9,
            this.xrPanel3,
            this.xrPanel6,
            this.xrPanel5});
            this.preJuly18thTandC.Dpi = 96F;
            this.preJuly18thTandC.LocationFloat = new DevExpress.Utils.PointFloat(21.69406F, 233.0001F);
            this.preJuly18thTandC.Name = "preJuly18thTandC";
            this.preJuly18thTandC.SizeF = new System.Drawing.SizeF(681.6482F, 553.1895F);
            // 
            // xrPanel8
            // 
            this.xrPanel8.CanShrink = true;
            this.xrPanel8.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel53,
            this.xrLabel46});
            this.xrPanel8.Dpi = 96F;
            this.xrPanel8.LocationFloat = new DevExpress.Utils.PointFloat(3.044739F, 251.0908F);
            this.xrPanel8.Name = "xrPanel8";
            this.xrPanel8.SizeF = new System.Drawing.SizeF(675.652F, 58.18182F);
            // 
            // xrLabel53
            // 
            this.xrLabel53.AnchorVertical = DevExpress.XtraReports.UI.VerticalAnchorStyles.Top;
            this.xrLabel53.BackColor = System.Drawing.Color.Transparent;
            this.xrLabel53.CanShrink = true;
            this.xrLabel53.Dpi = 96F;
            this.xrLabel53.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel53.ForeColor = System.Drawing.Color.DimGray;
            this.xrLabel53.LocationFloat = new DevExpress.Utils.PointFloat(3.706589F, 2.181946F);
            this.xrLabel53.Name = "xrLabel53";
            this.xrLabel53.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
            this.xrLabel53.SizeF = new System.Drawing.SizeF(25F, 15F);
            this.xrLabel53.StylePriority.UseBackColor = false;
            this.xrLabel53.StylePriority.UseFont = false;
            this.xrLabel53.StylePriority.UseForeColor = false;
            this.xrLabel53.StylePriority.UsePadding = false;
            this.xrLabel53.StylePriority.UseTextAlignment = false;
            this.xrLabel53.Text = "6.";
            this.xrLabel53.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrLabel46
            // 
            this.xrLabel46.AnchorVertical = DevExpress.XtraReports.UI.VerticalAnchorStyles.Top;
            this.xrLabel46.BackColor = System.Drawing.Color.Transparent;
            this.xrLabel46.CanShrink = true;
            this.xrLabel46.Dpi = 96F;
            this.xrLabel46.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel46.ForeColor = System.Drawing.Color.DimGray;
            this.xrLabel46.KeepTogether = true;
            this.xrLabel46.LocationFloat = new DevExpress.Utils.PointFloat(39.36365F, 2.181885F);
            this.xrLabel46.Name = "xrLabel46";
            this.xrLabel46.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
            this.xrLabel46.SizeF = new System.Drawing.SizeF(631.9752F, 53.0799F);
            this.xrLabel46.StylePriority.UseBackColor = false;
            this.xrLabel46.StylePriority.UseFont = false;
            this.xrLabel46.StylePriority.UseForeColor = false;
            this.xrLabel46.StylePriority.UsePadding = false;
            this.xrLabel46.StylePriority.UseTextAlignment = false;
            this.xrLabel46.Text = resources.GetString("xrLabel46.Text");
            this.xrLabel46.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrPanel12
            // 
            this.xrPanel12.CanShrink = true;
            this.xrPanel12.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.lb_training,
            this.lb_equipment,
            this.lb_equipment_no,
            this.lb_lien,
            this.lb_lien_no,
            this.lb_training_no,
            this.lbCopperPricesNo,
            this.lbCopperPrices});
            this.xrPanel12.Dpi = 96F;
            this.xrPanel12.LocationFloat = new DevExpress.Utils.PointFloat(3.044756F, 407.0623F);
            this.xrPanel12.Name = "xrPanel12";
            this.xrPanel12.SizeF = new System.Drawing.SizeF(675.6519F, 146.1272F);
            // 
            // lb_training
            // 
            this.lb_training.AnchorVertical = DevExpress.XtraReports.UI.VerticalAnchorStyles.Top;
            this.lb_training.BackColor = System.Drawing.Color.Transparent;
            this.lb_training.CanShrink = true;
            this.lb_training.Dpi = 96F;
            this.lb_training.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_training.ForeColor = System.Drawing.Color.DimGray;
            this.lb_training.KeepTogether = true;
            this.lb_training.LocationFloat = new DevExpress.Utils.PointFloat(40F, 85F);
            this.lb_training.Name = "lb_training";
            this.lb_training.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
            this.lb_training.SizeF = new System.Drawing.SizeF(633F, 27F);
            this.lb_training.StylePriority.UseBackColor = false;
            this.lb_training.StylePriority.UseFont = false;
            this.lb_training.StylePriority.UseForeColor = false;
            this.lb_training.StylePriority.UsePadding = false;
            this.lb_training.StylePriority.UseTextAlignment = false;
            this.lb_training.Text = "Any site specific training and orientation not included in this quote shall be re" +
    "garded as additional to this quote and will be billed at standard rates.";
            this.lb_training.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // lb_equipment
            // 
            this.lb_equipment.AnchorVertical = DevExpress.XtraReports.UI.VerticalAnchorStyles.Top;
            this.lb_equipment.BackColor = System.Drawing.Color.Transparent;
            this.lb_equipment.CanShrink = true;
            this.lb_equipment.Dpi = 96F;
            this.lb_equipment.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_equipment.ForeColor = System.Drawing.Color.DimGray;
            this.lb_equipment.KeepTogether = true;
            this.lb_equipment.LocationFloat = new DevExpress.Utils.PointFloat(40F, 113F);
            this.lb_equipment.Name = "lb_equipment";
            this.lb_equipment.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
            this.lb_equipment.SizeF = new System.Drawing.SizeF(633F, 27F);
            this.lb_equipment.StylePriority.UseBackColor = false;
            this.lb_equipment.StylePriority.UseFont = false;
            this.lb_equipment.StylePriority.UseForeColor = false;
            this.lb_equipment.StylePriority.UsePadding = false;
            this.lb_equipment.StylePriority.UseTextAlignment = false;
            this.lb_equipment.Text = resources.GetString("lb_equipment.Text");
            this.lb_equipment.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // lb_equipment_no
            // 
            this.lb_equipment_no.AnchorVertical = DevExpress.XtraReports.UI.VerticalAnchorStyles.Top;
            this.lb_equipment_no.BackColor = System.Drawing.Color.Transparent;
            this.lb_equipment_no.CanShrink = true;
            this.lb_equipment_no.Dpi = 96F;
            this.lb_equipment_no.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_equipment_no.ForeColor = System.Drawing.Color.DimGray;
            this.lb_equipment_no.LocationFloat = new DevExpress.Utils.PointFloat(3.706589F, 113F);
            this.lb_equipment_no.Multiline = true;
            this.lb_equipment_no.Name = "lb_equipment_no";
            this.lb_equipment_no.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
            this.lb_equipment_no.SizeF = new System.Drawing.SizeF(25F, 15F);
            this.lb_equipment_no.StylePriority.UseBackColor = false;
            this.lb_equipment_no.StylePriority.UseFont = false;
            this.lb_equipment_no.StylePriority.UseForeColor = false;
            this.lb_equipment_no.StylePriority.UsePadding = false;
            this.lb_equipment_no.StylePriority.UseTextAlignment = false;
            this.lb_equipment_no.Text = "12.\r\n";
            this.lb_equipment_no.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // lb_lien
            // 
            this.lb_lien.AnchorVertical = DevExpress.XtraReports.UI.VerticalAnchorStyles.Top;
            this.lb_lien.BackColor = System.Drawing.Color.Transparent;
            this.lb_lien.CanShrink = true;
            this.lb_lien.Dpi = 96F;
            this.lb_lien.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_lien.ForeColor = System.Drawing.Color.DimGray;
            this.lb_lien.KeepTogether = true;
            this.lb_lien.LocationFloat = new DevExpress.Utils.PointFloat(40F, 43F);
            this.lb_lien.Name = "lb_lien";
            this.lb_lien.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
            this.lb_lien.SizeF = new System.Drawing.SizeF(633F, 42F);
            this.lb_lien.StylePriority.UseBackColor = false;
            this.lb_lien.StylePriority.UseFont = false;
            this.lb_lien.StylePriority.UseForeColor = false;
            this.lb_lien.StylePriority.UsePadding = false;
            this.lb_lien.StylePriority.UseTextAlignment = false;
            this.lb_lien.Text = resources.GetString("lb_lien.Text");
            this.lb_lien.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.lb_lien.Visible = false;
            // 
            // lb_lien_no
            // 
            this.lb_lien_no.AnchorVertical = DevExpress.XtraReports.UI.VerticalAnchorStyles.Top;
            this.lb_lien_no.BackColor = System.Drawing.Color.Transparent;
            this.lb_lien_no.CanShrink = true;
            this.lb_lien_no.Dpi = 96F;
            this.lb_lien_no.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_lien_no.ForeColor = System.Drawing.Color.DimGray;
            this.lb_lien_no.LocationFloat = new DevExpress.Utils.PointFloat(5F, 43F);
            this.lb_lien_no.Name = "lb_lien_no";
            this.lb_lien_no.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
            this.lb_lien_no.SizeF = new System.Drawing.SizeF(25F, 15F);
            this.lb_lien_no.StylePriority.UseBackColor = false;
            this.lb_lien_no.StylePriority.UseFont = false;
            this.lb_lien_no.StylePriority.UseForeColor = false;
            this.lb_lien_no.StylePriority.UsePadding = false;
            this.lb_lien_no.StylePriority.UseTextAlignment = false;
            this.lb_lien_no.Text = "10.";
            this.lb_lien_no.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.lb_lien_no.Visible = false;
            // 
            // lb_training_no
            // 
            this.lb_training_no.AnchorVertical = DevExpress.XtraReports.UI.VerticalAnchorStyles.Top;
            this.lb_training_no.BackColor = System.Drawing.Color.Transparent;
            this.lb_training_no.CanShrink = true;
            this.lb_training_no.Dpi = 96F;
            this.lb_training_no.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_training_no.ForeColor = System.Drawing.Color.DimGray;
            this.lb_training_no.LocationFloat = new DevExpress.Utils.PointFloat(3.706589F, 85F);
            this.lb_training_no.Name = "lb_training_no";
            this.lb_training_no.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
            this.lb_training_no.SizeF = new System.Drawing.SizeF(25F, 15F);
            this.lb_training_no.StylePriority.UseBackColor = false;
            this.lb_training_no.StylePriority.UseFont = false;
            this.lb_training_no.StylePriority.UseForeColor = false;
            this.lb_training_no.StylePriority.UsePadding = false;
            this.lb_training_no.StylePriority.UseTextAlignment = false;
            this.lb_training_no.Text = "11.";
            this.lb_training_no.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // lbCopperPricesNo
            // 
            this.lbCopperPricesNo.AnchorVertical = DevExpress.XtraReports.UI.VerticalAnchorStyles.Top;
            this.lbCopperPricesNo.BackColor = System.Drawing.Color.Transparent;
            this.lbCopperPricesNo.CanShrink = true;
            this.lbCopperPricesNo.Dpi = 96F;
            this.lbCopperPricesNo.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbCopperPricesNo.ForeColor = System.Drawing.Color.DimGray;
            this.lbCopperPricesNo.LocationFloat = new DevExpress.Utils.PointFloat(4.999994F, 0F);
            this.lbCopperPricesNo.Name = "lbCopperPricesNo";
            this.lbCopperPricesNo.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
            this.lbCopperPricesNo.SizeF = new System.Drawing.SizeF(25F, 15F);
            this.lbCopperPricesNo.StylePriority.UseBackColor = false;
            this.lbCopperPricesNo.StylePriority.UseFont = false;
            this.lbCopperPricesNo.StylePriority.UseForeColor = false;
            this.lbCopperPricesNo.StylePriority.UsePadding = false;
            this.lbCopperPricesNo.StylePriority.UseTextAlignment = false;
            this.lbCopperPricesNo.Text = "9.";
            this.lbCopperPricesNo.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // lbCopperPrices
            // 
            this.lbCopperPrices.AnchorVertical = DevExpress.XtraReports.UI.VerticalAnchorStyles.Top;
            this.lbCopperPrices.BackColor = System.Drawing.Color.Transparent;
            this.lbCopperPrices.CanShrink = true;
            this.lbCopperPrices.Dpi = 96F;
            this.lbCopperPrices.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbCopperPrices.ForeColor = System.Drawing.Color.DimGray;
            this.lbCopperPrices.KeepTogether = true;
            this.lbCopperPrices.LocationFloat = new DevExpress.Utils.PointFloat(40F, 0F);
            this.lbCopperPrices.Name = "lbCopperPrices";
            this.lbCopperPrices.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
            this.lbCopperPrices.SizeF = new System.Drawing.SizeF(633F, 42F);
            this.lbCopperPrices.StylePriority.UseBackColor = false;
            this.lbCopperPrices.StylePriority.UseFont = false;
            this.lbCopperPrices.StylePriority.UseForeColor = false;
            this.lbCopperPrices.StylePriority.UsePadding = false;
            this.lbCopperPrices.StylePriority.UseTextAlignment = false;
            this.lbCopperPrices.Text = resources.GetString("lbCopperPrices.Text");
            this.lbCopperPrices.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrPanel4
            // 
            this.xrPanel4.CanShrink = true;
            this.xrPanel4.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel39,
            this.xrLabel38});
            this.xrPanel4.Dpi = 96F;
            this.xrPanel4.LocationFloat = new DevExpress.Utils.PointFloat(3.044739F, 54.36346F);
            this.xrPanel4.Name = "xrPanel4";
            this.xrPanel4.SizeF = new System.Drawing.SizeF(677.5307F, 36F);
            // 
            // xrLabel39
            // 
            this.xrLabel39.AnchorVertical = DevExpress.XtraReports.UI.VerticalAnchorStyles.Top;
            this.xrLabel39.BackColor = System.Drawing.Color.Transparent;
            this.xrLabel39.CanShrink = true;
            this.xrLabel39.Dpi = 96F;
            this.xrLabel39.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel39.ForeColor = System.Drawing.Color.DimGray;
            this.xrLabel39.LocationFloat = new DevExpress.Utils.PointFloat(3.706589F, 0F);
            this.xrLabel39.Name = "xrLabel39";
            this.xrLabel39.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
            this.xrLabel39.SizeF = new System.Drawing.SizeF(25F, 15F);
            this.xrLabel39.StylePriority.UseBackColor = false;
            this.xrLabel39.StylePriority.UseFont = false;
            this.xrLabel39.StylePriority.UseForeColor = false;
            this.xrLabel39.StylePriority.UsePadding = false;
            this.xrLabel39.StylePriority.UseTextAlignment = false;
            this.xrLabel39.Text = "2.";
            this.xrLabel39.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrLabel38
            // 
            this.xrLabel38.AnchorVertical = DevExpress.XtraReports.UI.VerticalAnchorStyles.Top;
            this.xrLabel38.BackColor = System.Drawing.Color.Transparent;
            this.xrLabel38.CanShrink = true;
            this.xrLabel38.Dpi = 96F;
            this.xrLabel38.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel38.ForeColor = System.Drawing.Color.DimGray;
            this.xrLabel38.LocationFloat = new DevExpress.Utils.PointFloat(39.36365F, 6.103516E-05F);
            this.xrLabel38.Name = "xrLabel38";
            this.xrLabel38.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
            this.xrLabel38.SizeF = new System.Drawing.SizeF(635.1223F, 31.4546F);
            this.xrLabel38.StylePriority.UseBackColor = false;
            this.xrLabel38.StylePriority.UseFont = false;
            this.xrLabel38.StylePriority.UseForeColor = false;
            this.xrLabel38.StylePriority.UsePadding = false;
            this.xrLabel38.StylePriority.UseTextAlignment = false;
            this.xrLabel38.Text = "Customer\'s acceptance of the proposal shall constitute the contract for the perfo" +
    "rmance of the described work, terms and conditions.";
            this.xrLabel38.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrPanel11
            // 
            this.xrPanel11.CanShrink = true;
            this.xrPanel11.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel47,
            this.xrLabel52});
            this.xrPanel11.Dpi = 96F;
            this.xrPanel11.LocationFloat = new DevExpress.Utils.PointFloat(3.044739F, 352.4726F);
            this.xrPanel11.Name = "xrPanel11";
            this.xrPanel11.SizeF = new System.Drawing.SizeF(675.6519F, 54.58969F);
            // 
            // xrLabel47
            // 
            this.xrLabel47.AnchorVertical = DevExpress.XtraReports.UI.VerticalAnchorStyles.Top;
            this.xrLabel47.BackColor = System.Drawing.Color.Transparent;
            this.xrLabel47.CanShrink = true;
            this.xrLabel47.Dpi = 96F;
            this.xrLabel47.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel47.ForeColor = System.Drawing.Color.DimGray;
            this.xrLabel47.LocationFloat = new DevExpress.Utils.PointFloat(3.706589F, 0F);
            this.xrLabel47.Name = "xrLabel47";
            this.xrLabel47.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
            this.xrLabel47.SizeF = new System.Drawing.SizeF(25F, 15F);
            this.xrLabel47.StylePriority.UseBackColor = false;
            this.xrLabel47.StylePriority.UseFont = false;
            this.xrLabel47.StylePriority.UseForeColor = false;
            this.xrLabel47.StylePriority.UsePadding = false;
            this.xrLabel47.StylePriority.UseTextAlignment = false;
            this.xrLabel47.Text = "8.";
            this.xrLabel47.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrLabel52
            // 
            this.xrLabel52.AnchorVertical = DevExpress.XtraReports.UI.VerticalAnchorStyles.Top;
            this.xrLabel52.BackColor = System.Drawing.Color.Transparent;
            this.xrLabel52.CanShrink = true;
            this.xrLabel52.Dpi = 96F;
            this.xrLabel52.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel52.ForeColor = System.Drawing.Color.DimGray;
            this.xrLabel52.KeepTogether = true;
            this.xrLabel52.LocationFloat = new DevExpress.Utils.PointFloat(40F, 0F);
            this.xrLabel52.Name = "xrLabel52";
            this.xrLabel52.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
            this.xrLabel52.SizeF = new System.Drawing.SizeF(632.5244F, 51.18187F);
            this.xrLabel52.StylePriority.UseBackColor = false;
            this.xrLabel52.StylePriority.UseFont = false;
            this.xrLabel52.StylePriority.UseForeColor = false;
            this.xrLabel52.StylePriority.UsePadding = false;
            this.xrLabel52.StylePriority.UseTextAlignment = false;
            this.xrLabel52.Text = resources.GetString("xrLabel52.Text");
            this.xrLabel52.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrPanel7
            // 
            this.xrPanel7.CanShrink = true;
            this.xrPanel7.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel45,
            this.xrLabel44});
            this.xrPanel7.Dpi = 96F;
            this.xrPanel7.LocationFloat = new DevExpress.Utils.PointFloat(3.044739F, 219.2725F);
            this.xrPanel7.Name = "xrPanel7";
            this.xrPanel7.SizeF = new System.Drawing.SizeF(677.5307F, 31.81824F);
            // 
            // xrLabel45
            // 
            this.xrLabel45.AnchorVertical = DevExpress.XtraReports.UI.VerticalAnchorStyles.Top;
            this.xrLabel45.BackColor = System.Drawing.Color.Transparent;
            this.xrLabel45.CanShrink = true;
            this.xrLabel45.Dpi = 96F;
            this.xrLabel45.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel45.ForeColor = System.Drawing.Color.DimGray;
            this.xrLabel45.LocationFloat = new DevExpress.Utils.PointFloat(3.706589F, 0.7273254F);
            this.xrLabel45.Name = "xrLabel45";
            this.xrLabel45.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
            this.xrLabel45.SizeF = new System.Drawing.SizeF(25F, 15F);
            this.xrLabel45.StylePriority.UseBackColor = false;
            this.xrLabel45.StylePriority.UseFont = false;
            this.xrLabel45.StylePriority.UseForeColor = false;
            this.xrLabel45.StylePriority.UsePadding = false;
            this.xrLabel45.StylePriority.UseTextAlignment = false;
            this.xrLabel45.Text = "5.";
            this.xrLabel45.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrLabel44
            // 
            this.xrLabel44.AnchorVertical = DevExpress.XtraReports.UI.VerticalAnchorStyles.Top;
            this.xrLabel44.BackColor = System.Drawing.Color.Transparent;
            this.xrLabel44.CanShrink = true;
            this.xrLabel44.Dpi = 96F;
            this.xrLabel44.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel44.ForeColor = System.Drawing.Color.DimGray;
            this.xrLabel44.KeepTogether = true;
            this.xrLabel44.LocationFloat = new DevExpress.Utils.PointFloat(39.36365F, 0.7272949F);
            this.xrLabel44.Name = "xrLabel44";
            this.xrLabel44.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
            this.xrLabel44.SizeF = new System.Drawing.SizeF(631.9752F, 29.07994F);
            this.xrLabel44.StylePriority.UseBackColor = false;
            this.xrLabel44.StylePriority.UseFont = false;
            this.xrLabel44.StylePriority.UseForeColor = false;
            this.xrLabel44.StylePriority.UsePadding = false;
            this.xrLabel44.StylePriority.UseTextAlignment = false;
            this.xrLabel44.Text = "Quote is contingent upon having 10 business days from the time the work is awarde" +
    "d until job commences in order to procure materials and appropriately plan the w" +
    "ork.";
            this.xrLabel44.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrPanel9
            // 
            this.xrPanel9.CanShrink = true;
            this.xrPanel9.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel49,
            this.xrLabel48});
            this.xrPanel9.Dpi = 96F;
            this.xrPanel9.LocationFloat = new DevExpress.Utils.PointFloat(3.044739F, 309.2726F);
            this.xrPanel9.Name = "xrPanel9";
            this.xrPanel9.SizeF = new System.Drawing.SizeF(677.5307F, 43.20001F);
            // 
            // xrLabel49
            // 
            this.xrLabel49.AnchorVertical = DevExpress.XtraReports.UI.VerticalAnchorStyles.Top;
            this.xrLabel49.BackColor = System.Drawing.Color.Transparent;
            this.xrLabel49.CanShrink = true;
            this.xrLabel49.Dpi = 96F;
            this.xrLabel49.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel49.ForeColor = System.Drawing.Color.DimGray;
            this.xrLabel49.LocationFloat = new DevExpress.Utils.PointFloat(3.706589F, 5.160095F);
            this.xrLabel49.Name = "xrLabel49";
            this.xrLabel49.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
            this.xrLabel49.SizeF = new System.Drawing.SizeF(25F, 15F);
            this.xrLabel49.StylePriority.UseBackColor = false;
            this.xrLabel49.StylePriority.UseFont = false;
            this.xrLabel49.StylePriority.UseForeColor = false;
            this.xrLabel49.StylePriority.UsePadding = false;
            this.xrLabel49.StylePriority.UseTextAlignment = false;
            this.xrLabel49.Text = "7.";
            this.xrLabel49.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrLabel48
            // 
            this.xrLabel48.AnchorVertical = DevExpress.XtraReports.UI.VerticalAnchorStyles.Top;
            this.xrLabel48.BackColor = System.Drawing.Color.Transparent;
            this.xrLabel48.CanShrink = true;
            this.xrLabel48.Dpi = 96F;
            this.xrLabel48.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel48.ForeColor = System.Drawing.Color.DimGray;
            this.xrLabel48.KeepTogether = true;
            this.xrLabel48.LocationFloat = new DevExpress.Utils.PointFloat(38.45453F, 1.272705F);
            this.xrLabel48.Name = "xrLabel48";
            this.xrLabel48.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
            this.xrLabel48.SizeF = new System.Drawing.SizeF(634.7631F, 38.4F);
            this.xrLabel48.StylePriority.UseBackColor = false;
            this.xrLabel48.StylePriority.UseFont = false;
            this.xrLabel48.StylePriority.UseForeColor = false;
            this.xrLabel48.StylePriority.UsePadding = false;
            this.xrLabel48.StylePriority.UseTextAlignment = false;
            this.xrLabel48.Text = resources.GetString("xrLabel48.Text");
            this.xrLabel48.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrPanel3
            // 
            this.xrPanel3.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel37,
            this.xrLabel36m,
            this.xrLabel36});
            this.xrPanel3.Dpi = 96F;
            this.xrPanel3.LocationFloat = new DevExpress.Utils.PointFloat(3.044756F, 0F);
            this.xrPanel3.Name = "xrPanel3";
            this.xrPanel3.SizeF = new System.Drawing.SizeF(675.6519F, 46.22725F);
            // 
            // xrLabel37
            // 
            this.xrLabel37.AnchorVertical = DevExpress.XtraReports.UI.VerticalAnchorStyles.Top;
            this.xrLabel37.BackColor = System.Drawing.Color.Transparent;
            this.xrLabel37.CanShrink = true;
            this.xrLabel37.Dpi = 96F;
            this.xrLabel37.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel37.ForeColor = System.Drawing.Color.DimGray;
            this.xrLabel37.LocationFloat = new DevExpress.Utils.PointFloat(3.706589F, 2.181885F);
            this.xrLabel37.Name = "xrLabel37";
            this.xrLabel37.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
            this.xrLabel37.SizeF = new System.Drawing.SizeF(25F, 15F);
            this.xrLabel37.StylePriority.UseBackColor = false;
            this.xrLabel37.StylePriority.UseFont = false;
            this.xrLabel37.StylePriority.UseForeColor = false;
            this.xrLabel37.StylePriority.UsePadding = false;
            this.xrLabel37.StylePriority.UseTextAlignment = false;
            this.xrLabel37.Text = "1.";
            this.xrLabel37.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrLabel36m
            // 
            this.xrLabel36m.AnchorVertical = DevExpress.XtraReports.UI.VerticalAnchorStyles.Top;
            this.xrLabel36m.BackColor = System.Drawing.Color.Transparent;
            this.xrLabel36m.Dpi = 96F;
            this.xrLabel36m.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel36m.ForeColor = System.Drawing.Color.DimGray;
            this.xrLabel36m.LocationFloat = new DevExpress.Utils.PointFloat(39.36365F, 19.18188F);
            this.xrLabel36m.Multiline = true;
            this.xrLabel36m.Name = "xrLabel36m";
            this.xrLabel36m.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
            this.xrLabel36m.SizeF = new System.Drawing.SizeF(633.8538F, 26F);
            this.xrLabel36m.StylePriority.UseBackColor = false;
            this.xrLabel36m.StylePriority.UseFont = false;
            this.xrLabel36m.StylePriority.UseForeColor = false;
            this.xrLabel36m.StylePriority.UsePadding = false;
            this.xrLabel36m.StylePriority.UseTextAlignment = false;
            this.xrLabel36m.Text = "The material prices used are based on current market values and may fluctuate bey" +
    "ond our control and as such they are valid for 5 days from the quote sent date.";
            this.xrLabel36m.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.xrLabel36m.Visible = false;
            // 
            // xrLabel36
            // 
            this.xrLabel36.AnchorVertical = DevExpress.XtraReports.UI.VerticalAnchorStyles.Top;
            this.xrLabel36.BackColor = System.Drawing.Color.Transparent;
            this.xrLabel36.Dpi = 96F;
            this.xrLabel36.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel36.ForeColor = System.Drawing.Color.DimGray;
            this.xrLabel36.LocationFloat = new DevExpress.Utils.PointFloat(39.36365F, 2.181824F);
            this.xrLabel36.Name = "xrLabel36";
            this.xrLabel36.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
            this.xrLabel36.SizeF = new System.Drawing.SizeF(633.8538F, 14.00006F);
            this.xrLabel36.StylePriority.UseBackColor = false;
            this.xrLabel36.StylePriority.UseFont = false;
            this.xrLabel36.StylePriority.UseForeColor = false;
            this.xrLabel36.StylePriority.UsePadding = false;
            this.xrLabel36.StylePriority.UseTextAlignment = false;
            this.xrLabel36.Text = "Price is valid for acceptance for the next 30 days.";
            this.xrLabel36.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrPanel6
            // 
            this.xrPanel6.CanShrink = true;
            this.xrPanel6.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel43,
            this.xrLabel42});
            this.xrPanel6.Dpi = 96F;
            this.xrPanel6.LocationFloat = new DevExpress.Utils.PointFloat(3.044739F, 136.5453F);
            this.xrPanel6.Name = "xrPanel6";
            this.xrPanel6.SizeF = new System.Drawing.SizeF(677.5308F, 82.72726F);
            // 
            // xrLabel43
            // 
            this.xrLabel43.AnchorVertical = DevExpress.XtraReports.UI.VerticalAnchorStyles.Top;
            this.xrLabel43.BackColor = System.Drawing.Color.Transparent;
            this.xrLabel43.CanShrink = true;
            this.xrLabel43.Dpi = 96F;
            this.xrLabel43.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel43.ForeColor = System.Drawing.Color.DimGray;
            this.xrLabel43.KeepTogether = true;
            this.xrLabel43.LocationFloat = new DevExpress.Utils.PointFloat(3.706589F, 1.272736F);
            this.xrLabel43.Name = "xrLabel43";
            this.xrLabel43.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
            this.xrLabel43.SizeF = new System.Drawing.SizeF(25F, 15F);
            this.xrLabel43.StylePriority.UseBackColor = false;
            this.xrLabel43.StylePriority.UseFont = false;
            this.xrLabel43.StylePriority.UseForeColor = false;
            this.xrLabel43.StylePriority.UsePadding = false;
            this.xrLabel43.StylePriority.UseTextAlignment = false;
            this.xrLabel43.Text = "4.";
            this.xrLabel43.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrLabel42
            // 
            this.xrLabel42.AnchorVertical = DevExpress.XtraReports.UI.VerticalAnchorStyles.Top;
            this.xrLabel42.BackColor = System.Drawing.Color.Transparent;
            this.xrLabel42.CanShrink = true;
            this.xrLabel42.Dpi = 96F;
            this.xrLabel42.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel42.ForeColor = System.Drawing.Color.DimGray;
            this.xrLabel42.KeepTogether = true;
            this.xrLabel42.LocationFloat = new DevExpress.Utils.PointFloat(38.45452F, 3.647324F);
            this.xrLabel42.Name = "xrLabel42";
            this.xrLabel42.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
            this.xrLabel42.SizeF = new System.Drawing.SizeF(634.7631F, 79.07994F);
            this.xrLabel42.StylePriority.UseBackColor = false;
            this.xrLabel42.StylePriority.UseFont = false;
            this.xrLabel42.StylePriority.UseForeColor = false;
            this.xrLabel42.StylePriority.UsePadding = false;
            this.xrLabel42.StylePriority.UseTextAlignment = false;
            this.xrLabel42.Text = resources.GetString("xrLabel42.Text");
            this.xrLabel42.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrPanel5
            // 
            this.xrPanel5.CanShrink = true;
            this.xrPanel5.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel41,
            this.xrLabel40});
            this.xrPanel5.Dpi = 96F;
            this.xrPanel5.LocationFloat = new DevExpress.Utils.PointFloat(3.044739F, 93.09077F);
            this.xrPanel5.Name = "xrPanel5";
            this.xrPanel5.SizeF = new System.Drawing.SizeF(677.5307F, 43.45448F);
            // 
            // xrLabel41
            // 
            this.xrLabel41.AnchorVertical = DevExpress.XtraReports.UI.VerticalAnchorStyles.Top;
            this.xrLabel41.BackColor = System.Drawing.Color.Transparent;
            this.xrLabel41.CanShrink = true;
            this.xrLabel41.Dpi = 96F;
            this.xrLabel41.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel41.ForeColor = System.Drawing.Color.DimGray;
            this.xrLabel41.LocationFloat = new DevExpress.Utils.PointFloat(3.706589F, 2.244385F);
            this.xrLabel41.Name = "xrLabel41";
            this.xrLabel41.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
            this.xrLabel41.SizeF = new System.Drawing.SizeF(25F, 15F);
            this.xrLabel41.StylePriority.UseBackColor = false;
            this.xrLabel41.StylePriority.UseFont = false;
            this.xrLabel41.StylePriority.UseForeColor = false;
            this.xrLabel41.StylePriority.UsePadding = false;
            this.xrLabel41.StylePriority.UseTextAlignment = false;
            this.xrLabel41.Text = "3.";
            this.xrLabel41.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrLabel40
            // 
            this.xrLabel40.AnchorVertical = DevExpress.XtraReports.UI.VerticalAnchorStyles.Top;
            this.xrLabel40.BackColor = System.Drawing.Color.Transparent;
            this.xrLabel40.CanShrink = true;
            this.xrLabel40.Dpi = 96F;
            this.xrLabel40.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel40.ForeColor = System.Drawing.Color.DimGray;
            this.xrLabel40.KeepTogether = true;
            this.xrLabel40.LocationFloat = new DevExpress.Utils.PointFloat(38.29338F, 2.244385F);
            this.xrLabel40.Name = "xrLabel40";
            this.xrLabel40.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
            this.xrLabel40.SizeF = new System.Drawing.SizeF(634.9243F, 41.20998F);
            this.xrLabel40.StylePriority.UseBackColor = false;
            this.xrLabel40.StylePriority.UseFont = false;
            this.xrLabel40.StylePriority.UseForeColor = false;
            this.xrLabel40.StylePriority.UsePadding = false;
            this.xrLabel40.StylePriority.UseTextAlignment = false;
            this.xrLabel40.Text = resources.GetString("xrLabel40.Text");
            this.xrLabel40.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // type
            // 
            this.type.Name = "type";
            // 
            // dsquotemaster1
            // 
            this.dsquotemaster1.DataSetName = "dsquotemaster";
            this.dsquotemaster1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // PageFooter
            // 
            this.PageFooter.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrPanel2});
            this.PageFooter.Dpi = 96F;
            this.PageFooter.HeightF = 62.00001F;
            this.PageFooter.Name = "PageFooter";
            // 
            // xrPanel2
            // 
            this.xrPanel2.AnchorVertical = DevExpress.XtraReports.UI.VerticalAnchorStyles.Bottom;
            this.xrPanel2.BackColor = System.Drawing.Color.WhiteSmoke;
            this.xrPanel2.CanGrow = false;
            this.xrPanel2.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrPictureBox2,
            this.xrPageInfo1});
            this.xrPanel2.Dpi = 96F;
            this.xrPanel2.LocationFloat = new DevExpress.Utils.PointFloat(13.5F, 5F);
            this.xrPanel2.Name = "xrPanel2";
            this.xrPanel2.SizeF = new System.Drawing.SizeF(702.9592F, 48F);
            this.xrPanel2.StylePriority.UseBackColor = false;
            // 
            // xrPictureBox2
            // 
            this.xrPictureBox2.Dpi = 96F;
            this.xrPictureBox2.ImageUrl = "~\\images\\Logos\\spark_Logo.png";
            this.xrPictureBox2.LocationFloat = new DevExpress.Utils.PointFloat(614.1554F, 3.600035F);
            this.xrPictureBox2.Name = "xrPictureBox2";
            this.xrPictureBox2.SizeF = new System.Drawing.SizeF(73.48004F, 43.67999F);
            this.xrPictureBox2.Sizing = DevExpress.XtraPrinting.ImageSizeMode.ZoomImage;
            // 
            // xrPageInfo1
            // 
            this.xrPageInfo1.Dpi = 96F;
            this.xrPageInfo1.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrPageInfo1.ForeColor = System.Drawing.Color.DimGray;
            this.xrPageInfo1.LocationFloat = new DevExpress.Utils.PointFloat(9.599998F, 6.599976F);
            this.xrPageInfo1.Name = "xrPageInfo1";
            this.xrPageInfo1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
            this.xrPageInfo1.SizeF = new System.Drawing.SizeF(58.00002F, 36.80005F);
            this.xrPageInfo1.StylePriority.UseFont = false;
            this.xrPageInfo1.StylePriority.UseForeColor = false;
            this.xrPageInfo1.StylePriority.UseTextAlignment = false;
            this.xrPageInfo1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // PageHeader
            // 
            this.PageHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel20,
            this.xrLabel15,
            this.xrLabel23,
            this.xrPictureBox3,
            this.xrLabel34});
            this.PageHeader.Dpi = 96F;
            this.PageHeader.HeightF = 66.42914F;
            this.PageHeader.Name = "PageHeader";
            this.PageHeader.PrintOn = DevExpress.XtraReports.UI.PrintOnPages.NotWithReportHeader;
            // 
            // xrLabel20
            // 
            this.xrLabel20.BackColor = System.Drawing.Color.Transparent;
            this.xrLabel20.Dpi = 96F;
            this.xrLabel20.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.xrLabel20.LocationFloat = new DevExpress.Utils.PointFloat(24F, 36.86792F);
            this.xrLabel20.Name = "xrLabel20";
            this.xrLabel20.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
            this.xrLabel20.SizeF = new System.Drawing.SizeF(60F, 16.54541F);
            this.xrLabel20.StylePriority.UseBackColor = false;
            this.xrLabel20.StylePriority.UseFont = false;
            this.xrLabel20.StylePriority.UseTextAlignment = false;
            this.xrLabel20.Text = "Version:";
            this.xrLabel20.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel15
            // 
            this.xrLabel15.Dpi = 96F;
            this.xrLabel15.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[revision]")});
            this.xrLabel15.Font = new System.Drawing.Font("Arial", 10F);
            this.xrLabel15.LocationFloat = new DevExpress.Utils.PointFloat(84F, 36.86786F);
            this.xrLabel15.Name = "xrLabel15";
            this.xrLabel15.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
            this.xrLabel15.SizeF = new System.Drawing.SizeF(322.054F, 16.54547F);
            this.xrLabel15.StylePriority.UseFont = false;
            this.xrLabel15.StylePriority.UseTextAlignment = false;
            this.xrLabel15.Text = "xrLabel15";
            this.xrLabel15.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel23
            // 
            this.xrLabel23.BackColor = System.Drawing.Color.Transparent;
            this.xrLabel23.Dpi = 96F;
            this.xrLabel23.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[quote_id]")});
            this.xrLabel23.Font = new System.Drawing.Font("Arial", 10F);
            this.xrLabel23.LocationFloat = new DevExpress.Utils.PointFloat(84F, 10F);
            this.xrLabel23.Name = "xrLabel23";
            this.xrLabel23.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
            this.xrLabel23.SizeF = new System.Drawing.SizeF(325.3333F, 17.41333F);
            this.xrLabel23.StylePriority.UseBackColor = false;
            this.xrLabel23.StylePriority.UseFont = false;
            this.xrLabel23.StylePriority.UseTextAlignment = false;
            this.xrLabel23.Text = "xrLabel1";
            this.xrLabel23.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrPictureBox3
            // 
            this.xrPictureBox3.Dpi = 96F;
            this.xrPictureBox3.LocationFloat = new DevExpress.Utils.PointFloat(624.4764F, 10F);
            this.xrPictureBox3.Name = "xrPictureBox3";
            this.xrPictureBox3.SizeF = new System.Drawing.SizeF(73.4801F, 43.67999F);
            this.xrPictureBox3.Sizing = DevExpress.XtraPrinting.ImageSizeMode.ZoomImage;
            // 
            // xrLabel34
            // 
            this.xrLabel34.BackColor = System.Drawing.Color.Transparent;
            this.xrLabel34.Dpi = 96F;
            this.xrLabel34.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.xrLabel34.LocationFloat = new DevExpress.Utils.PointFloat(24F, 10F);
            this.xrLabel34.Name = "xrLabel34";
            this.xrLabel34.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 96F);
            this.xrLabel34.SizeF = new System.Drawing.SizeF(53.66664F, 17.41333F);
            this.xrLabel34.StylePriority.UseBackColor = false;
            this.xrLabel34.StylePriority.UseFont = false;
            this.xrLabel34.StylePriority.UseTextAlignment = false;
            this.xrLabel34.Text = "Quote:";
            this.xrLabel34.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // quote_notesTableAdapter
            // 
            this.quote_notesTableAdapter.ClearBeforeFill = true;
            // 
            // dsQuotenotes1
            // 
            this.dsQuotenotes1.DataSetName = "dsQuotenotes";
            this.dsQuotenotes1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // quote_extratextTableAdapter
            // 
            this.quote_extratextTableAdapter.ClearBeforeFill = true;
            // 
            // quote_details1
            // 
            this.quote_details1.DataSetName = "quote_details";
            this.quote_details1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // dsERQuoteDetail1
            // 
            this.dsERQuoteDetail1.DataSetName = "dsERQuoteDetail";
            this.dsERQuoteDetail1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // eRQuote_DetailsTableAdapter
            // 
            this.eRQuote_DetailsTableAdapter.ClearBeforeFill = true;
            // 
            // quotemaster
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.ReportHeader,
            this.ReportFooter,
            this.PageFooter,
            this.PageHeader});
            this.ComponentStorage.AddRange(new System.ComponentModel.IComponent[] {
            this.dsQuotenotes1,
            this.quote_details1,
            this.dsERQuoteDetail1});
            this.DataMember = "quote_forprintout";
            this.DataSource = this.dsquotemaster1;
            this.DefaultPrinterSettingsUsing.UsePaperKind = true;
            this.Dpi = 96F;
            this.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Underline);
            this.Margins = new System.Drawing.Printing.Margins(48, 48, 96, 48);
            this.PageHeight = 1056;
            this.PageWidth = 816;
            this.Parameters.AddRange(new DevExpress.XtraReports.Parameters.Parameter[] {
            this.quoteid,
            this.rev,
            this.type,
            this.signoff,
            this.remitToAddress});
            this.ReportUnit = DevExpress.XtraReports.UI.ReportUnit.Pixels;
            this.SnapGridSize = 12.5F;
            this.Version = "19.2";
            this.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.quotemaster_BeforePrint);
            ((System.ComponentModel.ISupportInitialize)(this.standardTandC)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrSameGreat)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dsquotemaster1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dsQuotenotes1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.quote_details1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dsERQuoteDetail1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

		}


		#endregion

		private void quotemaster_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
		{
			var quoteId = Convert.ToInt32(quoteid.Value);
			var quoteRevision = Convert.ToInt32(rev.Value);
			var quoteSignoff = Convert.ToInt16(signoff.Value);

			var _quote = new quote(quoteId, quoteRevision);

			var QuoteBusinessUnit = new NeBusinessUnit(_quote.business_unit_id);
			var QuotedByEmployee = new NeMember(_quote.quoted_by);
			var quote_adapter = new Nesi.Web.Reports.QuotePrintout.dsquotemasterTableAdapters.quote_forprintoutTableAdapter();

			quote_adapter.Fill(dsquotemaster1.quote_forprintout, Convert.ToInt32(quoteid.Value), Convert.ToInt32(rev.Value));

			xrLabel61.Text = QuotedByEmployee.Type; // Membertype Name

			var DateJuly18th2020 = new DateTime(2020, 7, 18);
			var DateOctober8th2020 = new DateTime(2020, 10, 8);
			var DateApril22nd2020 = new DateTime(2022, 4, 22);
			var materialValidVisibleDate = new DateTime(2022, 4, 22);

			preJuly18thTandC.Visible = _quote.PrintedDate < DateJuly18th2020;
			postJuly18TandC.Visible = _quote.PrintedDate >= DateJuly18th2020 || _quote.PrintedDate == null;
			QuoteValidPeriod.Visible = _quote.PrintedDate >= DateOctober8th2020 || _quote.PrintedDate == null;

			if (QuoteValidPeriod.Visible)
			{
				var materialValidVisible = _quote.LastFaxedDate == null || _quote.LastFaxedDate > materialValidVisibleDate;
				QuoteValidPeriod.Text = $@"Price is valid for acceptance for the next {_quote.ValidPeriod} days.{(materialValidVisible
											? "\n\rThe material prices used are based on current market values and may fluctuate beyond our control and as such they are valid for 5 days from the quote sent date."
											: "")}";
				if (_quote.PrintedDate >= DateApril22nd2020)
				{
					//QuoteMaterialValidPeriod.Visible = true;
				}
			}
			xrPictureBox1.ImageUrl = Toolbox.app_setting("Domain") + @"/images/Logos/" + QuoteBusinessUnit.logo_file;
			xrPictureBox2.ImageUrl = Toolbox.app_setting("Domain") + @"/images/Logos/" + QuoteBusinessUnit.logo_file;
			xrPictureBox3.ImageUrl = Toolbox.app_setting("Domain") + @"/images/Logos/" + QuoteBusinessUnit.logo_file;
			xrSameGreat.Html = string.IsNullOrEmpty(QuoteBusinessUnit.RebrandMemo) ? "" : QuoteBusinessUnit.RebrandMemo;
			xrSameGreat.Visible = QuoteBusinessUnit.RebrandMemo != "";
			var price1 = Convert.ToDouble(GetCurrentColumnValue("quoted_price").ToString());
			var price2 = Convert.ToDouble(GetCurrentColumnValue("price_to").ToString());
			if (!_quote.IncludeTitle)
			{
				lb_contact_title.Visible = false;
				lb_contact_title_lb.Visible = false;
				xrLabel19.TopF = xrLabel19.TopF + 18;
				xrLabel14.TopF = xrLabel14.TopF + 18;
			}
			var slideHeight = postJuly18TandC.HeightF;

			if (postJuly18TandC.Visible)
			{//STD/Mat/Signing
				postJuly18TandC.TopF = preJuly18thTandC.TopF; // Set base position for the conditions
				QuoteMaterialValidPeriod.TopF = postJuly18TandC.TopF + slideHeight;
				signingPanel.TopF = postJuly18TandC.TopF + slideHeight + QuoteMaterialValidPeriod.HeightF;
			}
			if (price1 > 100000)
			{
				lb_lien.Visible = true;
				lb_lien_no.Visible = true;
			}
			else
			{
				lb_training_no.Text = "10.";
				lb_equipment_no.Text = "11.";
			}
			if (type.Value.ToString() == "w" || type.Value.ToString() == "draft")
			{
				lblpricestring.Visible = true;
				if (Report.GetCurrentColumnValue("type").ToString() == "Price From - To")
				{
					lblpricestring.Text = "Our price from :..." + string.Format("{0:C}", price1) + " ... to " + string.Format("{0:C}", price2);
				}
				else if (Report.GetCurrentColumnValue("type").ToString() != "")
				{
					lblpricestring.Text = "Our " + GetCurrentColumnValue("type") + " :..... " + string.Format("{0:C}", price1);
				}
				else
				{
					lblpricestring.Text = "Our Price :..... " + string.Format("{0:C}", price1);
				}
				if (GetCurrentColumnValue("us_currency").ToString() == "1")
				{
					lblpricestring.Text += " USD";
				}
				if (GetCurrentColumnValue("country").ToString() == OpsCountry.Canada)
				{
					lblpricestring.Text += " Taxes Extra";
				}
				if (QuoteBusinessUnit.is_er) // if it's an ER work order.  Look for the consignment items on the QUote
				{
					xrPanel1.BackColor = Color.WhiteSmoke;
					xrPanel2.BackColor = Color.WhiteSmoke;
					xrPictureBox1.Visible = false;
					xrPictureBox2.Visible = false;
					xrPictureBox3.Visible = false;

					lblpricestringoffset.Visible = false;
					xrLabel67.Visible = false;
					xrLabel68.Visible = false;
					xrLabel69.Visible = false;
					lblpricestring.Visible = false;
					var erreport = new ERQuoteDetail();
					erreport.Parameters[0].Value = Convert.ToInt32(quoteid.Value);
					erreport.Parameters[1].Value = Convert.ToInt32(rev.Value);
					scopeReport.ReportSource = erreport;

					xrPanel8.Visible = false;
					xrPanel9.Visible = false;
					xrPanel11.Visible = false;
					xrPanel12.Visible = false;

					xrLabel42.Text = "Unless otherwise stated in the quote or the project is bound to a specific timeframe stated in the quote, the price is based on work being completed during regular business hours, in a continuous fashion.";
					xrLabel44.Text = "Work performed that is not specified herein shall be regarded as additional to this quote.  These will be billed at our standard time and material rates or quoted separately.  ";

				}

			}
			else
			{
				lblpricestring.Visible = false;
				lblpricestringoffset.Visible = false;
			}

			if (Convert.ToString(GetCurrentColumnValue("custom_term").ToString()) != "")
			{
				lblCustomerTerm.Text = GetCurrentColumnValue("custom_term").ToString();
			}
			else
			{
				lblCustomerTerm.Text = "Not Applicable";
			}

			var QuoteScope = new quotedetail();
			QuoteScope.Parameters[0].Value = quoteId;
			QuoteScope.Parameters[1].Value = quoteRevision;
			QuoteScope.Parameters[2].Value = quoteSignoff;
			scopeReport.ReportSource = QuoteScope;

			var QuoteNotesAndAdders = new quotenotes();
			QuoteNotesAndAdders.Parameters[0].Value = quoteId;
			QuoteNotesAndAdders.Parameters[1].Value = quoteRevision;
			notesReport.ReportSource = QuoteNotesAndAdders;

            // Set the terms_conditions text from the resource file
        }

		public static string getRemitAddress(int businessUnitId)
		{
			string address = "";


			var oakvilleBusinessUnit = 1; // Choosing Oakville as it's historically always been the main branch.
			var bu = new NeBusinessUnit(businessUnitId);
			var modelBusinessUnit = businessUnitId != oakvilleBusinessUnit
										? new NeBusinessUnit(oakvilleBusinessUnit)
										: bu;
			//
			// Below code following the existing logics.
			//

			// (1) Set a regular one for all-non-usa branch
			if (NeBusinessUnit.GetbuCountry(businessUnitId.ToString()) != OpsCountry.UnitedStates)
			{
				var parentTaxEntity = new NeTaxEntity(bu.tax_entity_id);
				address = $@"Please remit payment to:
{parentTaxEntity.public_name} 
c/o Spark Power Corp.
{modelBusinessUnit.address},
{modelBusinessUnit.city}, {modelBusinessUnit.provstate}, {modelBusinessUnit.country}
{modelBusinessUnit.postal}
";
			}

			// (2) override the regular one: for all of usa branches.
			if (!string.IsNullOrEmpty(bu.remit_to_address))
			{
				address = @"Please remit payment to:
" + bu.remit_to_address.Replace("|", "\r\n") + "\n\n";
			}

			return address;
		}

		private void LoadResource()
		{
			//System.Resources.ResourceManager resources = global::Resources.quotemaster.ResourceManager;
			//this.xrLabel46.Text = resources.GetString("xrLabel46.Text");
			//this.lb_equipment.Text = resources.GetString("lb_equipment.Text");
			//this.lb_lien.Text = resources.GetString("lb_lien.Text");
			//this.xrLabel54.Text = resources.GetString("xrLabel54.Text");
			//this.xrLabel52.Text = resources.GetString("xrLabel52.Text");
			//this.xrLabel48.Text = resources.GetString("xrLabel48.Text");
			//this.xrLabel42.Text = resources.GetString("xrLabel42.Text");
			//this.xrLabel40.Text = resources.GetString("xrLabel40.Text");
			//this.xrPictureBox3.Image = ((System.Drawing.Image)(resources.GetObject("xrPictureBox3.Image")));
		}
        private void AddTermsConditionsFooter()
        {
            // Initialize the XRLabel for terms_conditions
            xrTermsConditions = new XRLabel
            {
                Name = "xrTermsConditions",
                Text = string.Empty, // Placeholder, will be set dynamically
                BoundsF = new RectangleF(0, 0, 700, 30), // Adjust size and position as needed
                Font = new Font("Arial", 10, FontStyle.Regular),
                TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter
            };

            // Add the label to the PageFooter or ReportFooter
            if (PageFooter == null)
            {
                PageFooter = new PageFooterBand
                {
                    HeightF = 50 // Adjust the height as needed
                };
                Bands.Add(PageFooter);
            }

            PageFooter.Controls.Add(xrTermsConditions);
        }
    }
}
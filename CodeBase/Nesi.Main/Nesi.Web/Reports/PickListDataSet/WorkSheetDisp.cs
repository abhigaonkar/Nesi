using System;
using System.Data;
using DevExpress.XtraReports.UI;
using System.Drawing;
using Nesi.Web.Reports.PickListDataSet;
using Nesi.Web.Reports.QuotePrintout;
using NESI.BLL.Pages.Quotes;

namespace nesi.core.print
	{
	/// <summary>
	/// Summary description for WorkSheetDisp
	/// </summary>
	public class WorkSheetDisp : XtraReport
		{
		private DetailBand Detail;
		private TopMarginBand TopMargin;
		private BottomMarginBand BottomMargin; 
		private WorkSheet workSheet1;
		private DevExpress.XtraReports.Parameters.Parameter qid;
		private DevExpress.XtraReports.Parameters.Parameter qrev;
		private DevExpress.XtraReports.Parameters.Parameter mid;
		private GroupHeaderBand GroupHeader1;
		private XRTable xrTable2;
		private XRTableRow xrTableRow2;
		private XRTableCell xrTableCell5;
		private XRTableCell xrTableCell6;
		private XRTable xrTable1;
		private XRTableRow xrTableRow1;
		private XRTableCell xrTableCell1;
		private XRTableCell xrTableCell3;
		private XRTableCell xrTableCell8;
		private XRTableCell xrTableCell11;
		private XRTableCell xrTableCell12;
		private XRTableCell xrTableCell13;
		private XRTableCell xrTableCell14;
		private XRTableCell xrTableCell16;
		private XRLabel lblDescription;
		private XRPictureBox xrPictureBox2;
		private XRLabel xrLabel11;
		private XRLabel xrLabel10;
		private XRLabel xrLabel25;
		private XRLabel xrLabel26;
		private XRLabel lblCustomer;
		private XRPanel xrPanel1;
		private ReportHeaderBand ReportHeader;
		private PageFooterBand PageFooter;
		private XRPanel xrPanel2;
		private XRPictureBox xrPictureBox1;
		private XRPageInfo xrPageInfo1;
		private PageHeaderBand PageHeader;
		private XRPictureBox xrPictureBox3;
		private XRLabel xrLabel34;
		private XRLabel xrLabel12;
		private XRLabel xrLabel14;
		private XRLabel lblQuotedDisp;
		private XRLabel xrLabel15;
		private XRLabel lblTMDisp;
		private XRLabel xrLabel17;
		private XRLabel lbl_benchextddiff;
		public string TM_Sell;
		public string Adjusted_sell;
		public string stuff;
		private XRLabel lblHeaderName;
		private XRControlStyle xrControlStyle1;
		private XRControlStyle xrControlStyle2;
		private dsQuotenotes dsQuotenotes1;
		private GroupFooterBand GroupFooter1;
		private XRLabel xrLabel4;
		private XRLabel xrLabel5;
		private XRLine xrLine1;
		private XRTableCell xrTableCell4;
		private XRTableCell xrTableCell10;
		private XRLabel xrLabel1;
		private XRLabel lblTotalCustomDisp;
		private XRLabel lblTotalItemsDisp;
		private XRLabel lblCostDisp;
		private XRLabel xrLabel19;
		private XRLabel xrLabel18;
		private XRLabel xrLabel16;
		private XRLabel lblTotalQuotedLaborDisp;
		private XRLabel xrLabel30;
		private XRLabel lblTotalLaborDisp;
		private XRLabel xrLabel28;
		private XRLabel lblTotalMaterialDisp;
		private XRLabel xrLabel24;
		private XRLabel lblTotalMargAbovDisp;
		private XRLabel xrLabel22;
		private XRLabel lblMargAbovCosDisp;
		private XRLabel xrLabel20;
		NeMember myMember;
		private const string strViewCostPriv = "58";
		private XRTableCell xrTableCell2;
		private XRPanel xrPanel3;
		private ReportFooterBand ReportFooter;
		private XRLabel xrLabel27;
		private XRLine xrLine2;
		private XRLabel xrLabel21;
		private XRLabel xrLabel23;
		private XRLabel xrLabel2;
		private XRTableCell xrTableCell7;
		private XRTableCell xrTableCell9;
		private CalculatedField margin;
		private XRLabel xrLabel3;
		private XRLabel xrLabel6;
		private XRLabel xrLabel7;
		private XRLabel xrLabel8; //View Costs
        private XRLabel xrlbl_quoted_price;
        private XRLabel xrLabel13;
        private XRTable xrTable3;
        private XRTableRow xrTableRow3;
        private XRTableCell xrTableCell15;
        private XRTableRow xrTableRow4;
        private XRTableCell xrTableCell17;
        private XRTableRow xrTableRow5;
        private XRTableCell xrTableCell18;
        private XRTableRow xrTableRow6;
        private XRTableCell xrTableCell19;
        private XRTableRow xrTableRow7;
        private XRTableCell xrTableCell20;
        private XRTableRow xrTableRow8;
        private XRTableCell xrTableCell21;
        private XRTableRow xrTableRow9;
        private XRTableCell xrTableCell22;
        private XRTableRow xrTableRow10;
        private XRTableCell xrTableCell23;
        private XRTableRow xrTableRow11;
        private XRTableCell xrTableCell24;
        private XRTableRow xrTableRow12;
        private XRTableCell xrTableCell25;
        private XRTableRow xrTableRow13;
        private XRTableCell xrTableCell26;
        private XRTableRow xrTableRow14;
        private XRTableCell xrTableCell27;

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;



		public WorkSheetDisp()
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
            string resourceFileName = "WorkSheetDisp.resx";
            System.Resources.ResourceManager resources = global::Resources.WorkSheetDisp.ResourceManager;
            DevExpress.XtraReports.UI.XRSummary xrSummary1 = new DevExpress.XtraReports.UI.XRSummary();
            DevExpress.XtraReports.UI.XRSummary xrSummary2 = new DevExpress.XtraReports.UI.XRSummary();
            DevExpress.XtraReports.UI.XRSummary xrSummary3 = new DevExpress.XtraReports.UI.XRSummary();
            DevExpress.XtraReports.UI.XRSummary xrSummary4 = new DevExpress.XtraReports.UI.XRSummary();
            DevExpress.XtraReports.UI.XRSummary xrSummary5 = new DevExpress.XtraReports.UI.XRSummary();
            DevExpress.XtraReports.UI.XRSummary xrSummary6 = new DevExpress.XtraReports.UI.XRSummary();
            DevExpress.XtraReports.UI.XRSummary xrSummary7 = new DevExpress.XtraReports.UI.XRSummary();
            DevExpress.XtraReports.UI.XRSummary xrSummary8 = new DevExpress.XtraReports.UI.XRSummary();
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.xrLabel2 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrTable2 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow2 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell12 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell5 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell13 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell14 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell6 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell4 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell16 = new DevExpress.XtraReports.UI.XRTableCell();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.workSheet1 = new WorkSheet();
            this.qid = new DevExpress.XtraReports.Parameters.Parameter();
            this.qrev = new DevExpress.XtraReports.Parameters.Parameter();
            this.mid = new DevExpress.XtraReports.Parameters.Parameter();
            this.GroupHeader1 = new DevExpress.XtraReports.UI.GroupHeaderBand();
            this.xrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrTable1 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow1 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell1 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell8 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell2 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell3 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell10 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell11 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell7 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableCell9 = new DevExpress.XtraReports.UI.XRTableCell();
            this.lblDescription = new DevExpress.XtraReports.UI.XRLabel();
            this.xrPictureBox2 = new DevExpress.XtraReports.UI.XRPictureBox();
            this.xrLabel11 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel10 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel25 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel26 = new DevExpress.XtraReports.UI.XRLabel();
            this.lblCustomer = new DevExpress.XtraReports.UI.XRLabel();
            this.xrPanel1 = new DevExpress.XtraReports.UI.XRPanel();
            this.xrLabel12 = new DevExpress.XtraReports.UI.XRLabel();
            this.ReportHeader = new DevExpress.XtraReports.UI.ReportHeaderBand();
            this.lblTotalQuotedLaborDisp = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel30 = new DevExpress.XtraReports.UI.XRLabel();
            this.lblTotalLaborDisp = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel28 = new DevExpress.XtraReports.UI.XRLabel();
            this.lblTotalMaterialDisp = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel24 = new DevExpress.XtraReports.UI.XRLabel();
            this.lblTotalMargAbovDisp = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel22 = new DevExpress.XtraReports.UI.XRLabel();
            this.lblMargAbovCosDisp = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel20 = new DevExpress.XtraReports.UI.XRLabel();
            this.lblTotalCustomDisp = new DevExpress.XtraReports.UI.XRLabel();
            this.lblTotalItemsDisp = new DevExpress.XtraReports.UI.XRLabel();
            this.lblCostDisp = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel19 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel18 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel16 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel17 = new DevExpress.XtraReports.UI.XRLabel();
            this.lbl_benchextddiff = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel15 = new DevExpress.XtraReports.UI.XRLabel();
            this.lblTMDisp = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel14 = new DevExpress.XtraReports.UI.XRLabel();
            this.lblQuotedDisp = new DevExpress.XtraReports.UI.XRLabel();
            this.PageFooter = new DevExpress.XtraReports.UI.PageFooterBand();
            this.xrPanel2 = new DevExpress.XtraReports.UI.XRPanel();
            this.xrPictureBox1 = new DevExpress.XtraReports.UI.XRPictureBox();
            this.xrPageInfo1 = new DevExpress.XtraReports.UI.XRPageInfo();
            this.PageHeader = new DevExpress.XtraReports.UI.PageHeaderBand();
            this.xrPanel3 = new DevExpress.XtraReports.UI.XRPanel();
            this.xrPictureBox3 = new DevExpress.XtraReports.UI.XRPictureBox();
            this.xrLabel34 = new DevExpress.XtraReports.UI.XRLabel();
            this.lblHeaderName = new DevExpress.XtraReports.UI.XRLabel();
            this.xrControlStyle1 = new DevExpress.XtraReports.UI.XRControlStyle();
            this.xrControlStyle2 = new DevExpress.XtraReports.UI.XRControlStyle();
            this.dsQuotenotes1 = new dsQuotenotes();
            this.GroupFooter1 = new DevExpress.XtraReports.UI.GroupFooterBand();
            this.xrLabel8 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel3 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLine1 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLabel5 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel4 = new DevExpress.XtraReports.UI.XRLabel();
            this.ReportFooter = new DevExpress.XtraReports.UI.ReportFooterBand();
            this.xrLabel7 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel6 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel27 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLine2 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLabel21 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel23 = new DevExpress.XtraReports.UI.XRLabel();
            this.margin = new DevExpress.XtraReports.UI.CalculatedField();
            this.xrlbl_quoted_price = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel13 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrTable3 = new DevExpress.XtraReports.UI.XRTable();
            this.xrTableRow3 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell15 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow4 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell17 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow5 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell18 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow6 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell19 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow7 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell20 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow8 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell21 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow9 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell22 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow10 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell23 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow11 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell24 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow12 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell25 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow13 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell26 = new DevExpress.XtraReports.UI.XRTableCell();
            this.xrTableRow14 = new DevExpress.XtraReports.UI.XRTableRow();
            this.xrTableCell27 = new DevExpress.XtraReports.UI.XRTableCell();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.workSheet1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dsQuotenotes1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel2,
            this.xrTable2});
            this.Detail.EvenStyleName = "xrControlStyle1";
            this.Detail.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Detail.HeightF = 23F;
            this.Detail.Name = "Detail";
            this.Detail.OddStyleName = "xrControlStyle2";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.Detail.Scripts.OnBeforePrint = "Detail_BeforePrint";
            this.Detail.StylePriority.UseFont = false;
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            this.Detail.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.Detail_BeforePrint);
            // 
            // xrLabel2
            // 
            this.xrLabel2.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "quote_worksheet.margin", "{0:0.0%}")});
            this.xrLabel2.LocationFloat = new DevExpress.Utils.PointFloat(935.0974F, 0F);
            this.xrLabel2.Name = "xrLabel2";
            this.xrLabel2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel2.SizeF = new System.Drawing.SizeF(61.61066F, 21.875F);
            this.xrLabel2.StylePriority.UseTextAlignment = false;
            this.xrLabel2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrTable2
            // 
            this.xrTable2.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTable2.LocationFloat = new DevExpress.Utils.PointFloat(10F, 0F);
            this.xrTable2.Name = "xrTable2";
            this.xrTable2.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow2});
            this.xrTable2.SizeF = new System.Drawing.SizeF(925.0974F, 21.875F);
            this.xrTable2.StylePriority.UseFont = false;
            // 
            // xrTableRow2
            // 
            this.xrTableRow2.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell12,
            this.xrTableCell5,
            this.xrTableCell13,
            this.xrTableCell14,
            this.xrTableCell6,
            this.xrTableCell4,
            this.xrTableCell16});
            this.xrTableRow2.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableRow2.Name = "xrTableRow2";
            this.xrTableRow2.StylePriority.UseFont = false;
            this.xrTableRow2.Weight = 1D;
            // 
            // xrTableCell12
            // 
            this.xrTableCell12.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "quote_worksheet.part_no")});
            this.xrTableCell12.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell12.Name = "xrTableCell12";
            this.xrTableCell12.StylePriority.UseFont = false;
            this.xrTableCell12.StylePriority.UseTextAlignment = false;
            this.xrTableCell12.Text = "xrTableCell12";
            this.xrTableCell12.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell12.Weight = 0.28446561386547725D;
            // 
            // xrTableCell5
            // 
            this.xrTableCell5.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "quote_worksheet.description")});
            this.xrTableCell5.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell5.Multiline = true;
            this.xrTableCell5.Name = "xrTableCell5";
            this.xrTableCell5.StylePriority.UseFont = false;
            this.xrTableCell5.StylePriority.UseTextAlignment = false;
            this.xrTableCell5.Text = "[description]";
            this.xrTableCell5.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell5.Weight = 1.7410918144229812D;
            // 
            // xrTableCell13
            // 
            this.xrTableCell13.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "quote_worksheet.qty", "{0:#.00}")});
            this.xrTableCell13.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell13.Name = "xrTableCell13";
            this.xrTableCell13.StylePriority.UseFont = false;
            this.xrTableCell13.StylePriority.UseTextAlignment = false;
            this.xrTableCell13.Text = "xrTableCell13";
            this.xrTableCell13.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell13.Weight = 0.21097589726300756D;
            // 
            // xrTableCell14
            // 
            this.xrTableCell14.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "quote_worksheet.cost", "{0:c2}")});
            this.xrTableCell14.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell14.Name = "xrTableCell14";
            this.xrTableCell14.StylePriority.UseFont = false;
            this.xrTableCell14.StylePriority.UseTextAlignment = false;
            this.xrTableCell14.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell14.Weight = 0.3562005213726025D;
            this.xrTableCell14.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrTableCell14_BeforePrint);
            // 
            // xrTableCell6
            // 
            this.xrTableCell6.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "quote_worksheet.original_sell", "{0:c2}")});
            this.xrTableCell6.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell6.Name = "xrTableCell6";
            this.xrTableCell6.StylePriority.UseFont = false;
            this.xrTableCell6.StylePriority.UseTextAlignment = false;
            this.xrTableCell6.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell6.Weight = 0.35620052481429409D;
            // 
            // xrTableCell4
            // 
            this.xrTableCell4.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "quote_worksheet.extTandM", "{0:c2}")});
            this.xrTableCell4.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell4.Name = "xrTableCell4";
            this.xrTableCell4.StylePriority.UseFont = false;
            this.xrTableCell4.StylePriority.UseTextAlignment = false;
            this.xrTableCell4.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell4.Weight = 0.356200524814294D;
            // 
            // xrTableCell16
            // 
            this.xrTableCell16.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "quote_worksheet.extended_per", "{0:c2}")});
            this.xrTableCell16.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrTableCell16.Name = "xrTableCell16";
            this.xrTableCell16.StylePriority.UseFont = false;
            this.xrTableCell16.StylePriority.UseTextAlignment = false;
            this.xrTableCell16.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell16.Weight = 0.35620050757415722D;
            this.xrTableCell16.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrTableCell16_BeforePrint);
            // 
            // TopMargin
            // 
            this.TopMargin.HeightF = 27F;
            this.TopMargin.Name = "TopMargin";
            this.TopMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.TopMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // BottomMargin
            // 
            this.BottomMargin.HeightF = 12.5F;
            this.BottomMargin.Name = "BottomMargin";
            this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // workSheet1
            // 
            this.workSheet1.DataSetName = "WorkSheet";
            this.workSheet1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // qid
            // 
            this.qid.Name = "qid";
            this.qid.Type = typeof(int);
            this.qid.ValueInfo = "0";
            // 
            // qrev
            // 
            this.qrev.Name = "qrev";
            this.qrev.Type = typeof(int);
            this.qrev.ValueInfo = "0";
            // 
            // mid
            // 
            this.mid.Name = "mid";
            // 
            // GroupHeader1
            // 
            this.GroupHeader1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel1,
            this.xrTable1});
            this.GroupHeader1.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GroupHeader1.GroupFields.AddRange(new DevExpress.XtraReports.UI.GroupField[] {
            new DevExpress.XtraReports.UI.GroupField("sortid", DevExpress.XtraReports.UI.XRColumnSortOrder.Ascending)});
            this.GroupHeader1.HeightF = 56.25F;
            this.GroupHeader1.Name = "GroupHeader1";
            this.GroupHeader1.StylePriority.UseFont = false;
            // 
            // xrLabel1
            // 
            this.xrLabel1.BackColor = System.Drawing.Color.Gray;
            this.xrLabel1.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "quote_worksheet.section")});
            this.xrLabel1.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel1.ForeColor = System.Drawing.Color.White;
            this.xrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrLabel1.Multiline = true;
            this.xrLabel1.Name = "xrLabel1";
            this.xrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel1.SizeF = new System.Drawing.SizeF(1003F, 25F);
            this.xrLabel1.StylePriority.UseBackColor = false;
            this.xrLabel1.StylePriority.UseFont = false;
            this.xrLabel1.StylePriority.UseForeColor = false;
            this.xrLabel1.StylePriority.UseTextAlignment = false;
            this.xrLabel1.Text = "xrLabel1";
            this.xrLabel1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrTable1
            // 
            this.xrTable1.BackColor = System.Drawing.Color.LightGray;
            this.xrTable1.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.xrTable1.ForeColor = System.Drawing.Color.White;
            this.xrTable1.LocationFloat = new DevExpress.Utils.PointFloat(3.973643E-05F, 25F);
            this.xrTable1.Name = "xrTable1";
            this.xrTable1.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow1});
            this.xrTable1.SizeF = new System.Drawing.SizeF(1003F, 25F);
            this.xrTable1.StylePriority.UseBackColor = false;
            this.xrTable1.StylePriority.UseFont = false;
            this.xrTable1.StylePriority.UseForeColor = false;
            // 
            // xrTableRow1
            // 
            this.xrTableRow1.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell1,
            this.xrTableCell8,
            this.xrTableCell2,
            this.xrTableCell3,
            this.xrTableCell10,
            this.xrTableCell11,
            this.xrTableCell7,
            this.xrTableCell9});
            this.xrTableRow1.Name = "xrTableRow1";
            this.xrTableRow1.Weight = 1D;
            // 
            // xrTableCell1
            // 
            this.xrTableCell1.BackColor = System.Drawing.Color.LightGray;
            this.xrTableCell1.CanGrow = false;
            this.xrTableCell1.Name = "xrTableCell1";
            this.xrTableCell1.StylePriority.UseBackColor = false;
            this.xrTableCell1.StylePriority.UseTextAlignment = false;
            this.xrTableCell1.Text = "Part No";
            this.xrTableCell1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell1.Weight = 0.324043255121734D;
            // 
            // xrTableCell8
            // 
            this.xrTableCell8.BackColor = System.Drawing.Color.LightGray;
            this.xrTableCell8.CanGrow = false;
            this.xrTableCell8.Name = "xrTableCell8";
            this.xrTableCell8.StylePriority.UseBackColor = false;
            this.xrTableCell8.StylePriority.UseTextAlignment = false;
            this.xrTableCell8.Text = "Description";
            this.xrTableCell8.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell8.Weight = 1.7410915619934351D;
            // 
            // xrTableCell2
            // 
            this.xrTableCell2.BackColor = System.Drawing.Color.LightGray;
            this.xrTableCell2.CanGrow = false;
            this.xrTableCell2.Name = "xrTableCell2";
            this.xrTableCell2.StylePriority.UseBackColor = false;
            this.xrTableCell2.StylePriority.UseTextAlignment = false;
            this.xrTableCell2.Text = "Qty";
            this.xrTableCell2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell2.Weight = 0.21097590372014508D;
            // 
            // xrTableCell3
            // 
            this.xrTableCell3.BackColor = System.Drawing.Color.LightGray;
            this.xrTableCell3.CanGrow = false;
            this.xrTableCell3.Name = "xrTableCell3";
            this.xrTableCell3.StylePriority.UseBackColor = false;
            this.xrTableCell3.StylePriority.UseTextAlignment = false;
            this.xrTableCell3.Text = "Cost";
            this.xrTableCell3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell3.Weight = 0.35620050200685416D;
            // 
            // xrTableCell10
            // 
            this.xrTableCell10.BackColor = System.Drawing.Color.LightGray;
            this.xrTableCell10.CanGrow = false;
            this.xrTableCell10.Name = "xrTableCell10";
            this.xrTableCell10.StylePriority.UseBackColor = false;
            this.xrTableCell10.StylePriority.UseTextAlignment = false;
            this.xrTableCell10.Text = "Bench";
            this.xrTableCell10.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell10.Weight = 0.35620052625681325D;
            // 
            // xrTableCell11
            // 
            this.xrTableCell11.BackColor = System.Drawing.Color.LightGray;
            this.xrTableCell11.CanGrow = false;
            this.xrTableCell11.Name = "xrTableCell11";
            this.xrTableCell11.StylePriority.UseBackColor = false;
            this.xrTableCell11.StylePriority.UseTextAlignment = false;
            this.xrTableCell11.Text = "Ext\'d";
            this.xrTableCell11.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell11.Weight = 0.35620050686840787D;
            // 
            // xrTableCell7
            // 
            this.xrTableCell7.BackColor = System.Drawing.Color.LightGray;
            this.xrTableCell7.Name = "xrTableCell7";
            this.xrTableCell7.StylePriority.UseBackColor = false;
            this.xrTableCell7.StylePriority.UseTextAlignment = false;
            this.xrTableCell7.Text = "~Ext\'d";
            this.xrTableCell7.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrTableCell7.Weight = 0.35620050686840782D;
            // 
            // xrTableCell9
            // 
            this.xrTableCell9.BackColor = System.Drawing.Color.LightGray;
            this.xrTableCell9.Name = "xrTableCell9";
            this.xrTableCell9.StylePriority.UseBackColor = false;
            this.xrTableCell9.StylePriority.UseTextAlignment = false;
            this.xrTableCell9.Text = "Margin";
            this.xrTableCell9.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrTableCell9.Weight = 0.26874310019134034D;
            // 
            // lblDescription
            // 
            this.lblDescription.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold);
            this.lblDescription.ForeColor = System.Drawing.Color.Black;
            this.lblDescription.LocationFloat = new DevExpress.Utils.PointFloat(21.49976F, 102.9583F);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblDescription.SizeF = new System.Drawing.SizeF(727.3743F, 26.62496F);
            this.lblDescription.StylePriority.UseFont = false;
            this.lblDescription.StylePriority.UseForeColor = false;
            this.lblDescription.Text = "lblDescription";
            // 
            // xrPictureBox2
            // 
            this.xrPictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("xrPictureBox2.Image")));
            this.xrPictureBox2.LocationFloat = new DevExpress.Utils.PointFloat(751.3334F, 10.00001F);
            this.xrPictureBox2.Name = "xrPictureBox2";
            this.xrPictureBox2.SizeF = new System.Drawing.SizeF(245.0833F, 127.6251F);
            this.xrPictureBox2.Sizing = DevExpress.XtraPrinting.ImageSizeMode.ZoomImage;
            // 
            // xrLabel11
            // 
            this.xrLabel11.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel11.LocationFloat = new DevExpress.Utils.PointFloat(21.49976F, 39.16667F);
            this.xrLabel11.Name = "xrLabel11";
            this.xrLabel11.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel11.SizeF = new System.Drawing.SizeF(100F, 23F);
            this.xrLabel11.StylePriority.UseFont = false;
            this.xrLabel11.StylePriority.UseTextAlignment = false;
            this.xrLabel11.Text = "Quote:";
            this.xrLabel11.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft;
            // 
            // xrLabel10
            // 
            this.xrLabel10.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel10.LocationFloat = new DevExpress.Utils.PointFloat(21.49976F, 62.16669F);
            this.xrLabel10.Name = "xrLabel10";
            this.xrLabel10.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel10.SizeF = new System.Drawing.SizeF(99.99999F, 19.875F);
            this.xrLabel10.StylePriority.UseFont = false;
            this.xrLabel10.StylePriority.UseTextAlignment = false;
            this.xrLabel10.Text = "Version:";
            this.xrLabel10.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft;
            // 
            // xrLabel25
            // 
            this.xrLabel25.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding(this.qid, "Text", "")});
            this.xrLabel25.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel25.LocationFloat = new DevExpress.Utils.PointFloat(121.4998F, 39.16667F);
            this.xrLabel25.Name = "xrLabel25";
            this.xrLabel25.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel25.SizeF = new System.Drawing.SizeF(193.75F, 23F);
            this.xrLabel25.StylePriority.UseFont = false;
            this.xrLabel25.StylePriority.UseTextAlignment = false;
            this.xrLabel25.Text = "xrLabel25";
            this.xrLabel25.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft;
            // 
            // xrLabel26
            // 
            this.xrLabel26.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding(this.qrev, "Text", "")});
            this.xrLabel26.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel26.LocationFloat = new DevExpress.Utils.PointFloat(121.4998F, 62.16669F);
            this.xrLabel26.Name = "xrLabel26";
            this.xrLabel26.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel26.SizeF = new System.Drawing.SizeF(193.75F, 19.875F);
            this.xrLabel26.StylePriority.UseFont = false;
            this.xrLabel26.StylePriority.UseTextAlignment = false;
            this.xrLabel26.Text = "xrLabel26";
            this.xrLabel26.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft;
            // 
            // lblCustomer
            // 
            this.lblCustomer.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCustomer.ForeColor = System.Drawing.Color.DimGray;
            this.lblCustomer.LocationFloat = new DevExpress.Utils.PointFloat(21.12459F, 82.04168F);
            this.lblCustomer.Name = "lblCustomer";
            this.lblCustomer.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblCustomer.SizeF = new System.Drawing.SizeF(450.6661F, 20.91666F);
            this.lblCustomer.StylePriority.UseFont = false;
            this.lblCustomer.StylePriority.UseForeColor = false;
            this.lblCustomer.Text = "lblCustomer";
            // 
            // xrPanel1
            // 
            this.xrPanel1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.xrPanel1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrPictureBox2,
            this.xrLabel12,
            this.lblCustomer,
            this.xrLabel26,
            this.xrLabel25,
            this.xrLabel10,
            this.xrLabel11,
            this.lblDescription});
            this.xrPanel1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 10.00001F);
            this.xrPanel1.Name = "xrPanel1";
            this.xrPanel1.SizeF = new System.Drawing.SizeF(1003F, 139.5833F);
            this.xrPanel1.StylePriority.UseBackColor = false;
            // 
            // xrLabel12
            // 
            this.xrLabel12.BackColor = System.Drawing.Color.Transparent;
            this.xrLabel12.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel12.ForeColor = System.Drawing.Color.Black;
            this.xrLabel12.LocationFloat = new DevExpress.Utils.PointFloat(21.49976F, 10.00001F);
            this.xrLabel12.Name = "xrLabel12";
            this.xrLabel12.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel12.SizeF = new System.Drawing.SizeF(341.7643F, 29.16667F);
            this.xrLabel12.StylePriority.UseBackColor = false;
            this.xrLabel12.StylePriority.UseFont = false;
            this.xrLabel12.StylePriority.UseForeColor = false;
            this.xrLabel12.StylePriority.UseTextAlignment = false;
            this.xrLabel12.Text = "Worksheet";
            this.xrLabel12.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // ReportHeader
            // 
            this.ReportHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrlbl_quoted_price,
            this.xrLabel13,
            this.lblTotalQuotedLaborDisp,
            this.xrLabel30,
            this.lblTotalLaborDisp,
            this.xrLabel28,
            this.lblTotalMaterialDisp,
            this.xrLabel24,
            this.lblTotalMargAbovDisp,
            this.xrLabel22,
            this.lblMargAbovCosDisp,
            this.xrLabel20,
            this.lblTotalCustomDisp,
            this.lblTotalItemsDisp,
            this.lblCostDisp,
            this.xrLabel19,
            this.xrLabel18,
            this.xrLabel16,
            this.xrLabel17,
            this.lbl_benchextddiff,
            this.xrLabel15,
            this.lblTMDisp,
            this.xrLabel14,
            this.lblQuotedDisp,
            this.xrPanel1});
            this.ReportHeader.HeightF = 365.3333F;
            this.ReportHeader.Name = "ReportHeader";
            // 
            // lblTotalQuotedLaborDisp
            // 
            this.lblTotalQuotedLaborDisp.Font = new System.Drawing.Font("Arial", 8F);
            this.lblTotalQuotedLaborDisp.LocationFloat = new DevExpress.Utils.PointFloat(897.1812F, 341.0831F);
            this.lblTotalQuotedLaborDisp.Name = "lblTotalQuotedLaborDisp";
            this.lblTotalQuotedLaborDisp.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblTotalQuotedLaborDisp.SizeF = new System.Drawing.SizeF(97.15271F, 16.74997F);
            this.lblTotalQuotedLaborDisp.StylePriority.UseFont = false;
            this.lblTotalQuotedLaborDisp.StylePriority.UseTextAlignment = false;
            this.lblTotalQuotedLaborDisp.Text = "xrLabel25";
            this.lblTotalQuotedLaborDisp.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel30
            // 
            this.xrLabel30.Font = new System.Drawing.Font("Arial", 8F);
            this.xrLabel30.LocationFloat = new DevExpress.Utils.PointFloat(684.6812F, 341.0831F);
            this.xrLabel30.Name = "xrLabel30";
            this.xrLabel30.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel30.SizeF = new System.Drawing.SizeF(212.4995F, 16.74994F);
            this.xrLabel30.StylePriority.UseFont = false;
            this.xrLabel30.StylePriority.UseTextAlignment = false;
            this.xrLabel30.Text = "Total Hours Quoted:";
            this.xrLabel30.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // lblTotalLaborDisp
            // 
            this.lblTotalLaborDisp.Font = new System.Drawing.Font("Arial", 8F);
            this.lblTotalLaborDisp.LocationFloat = new DevExpress.Utils.PointFloat(897.1806F, 324.3331F);
            this.lblTotalLaborDisp.Name = "lblTotalLaborDisp";
            this.lblTotalLaborDisp.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblTotalLaborDisp.SizeF = new System.Drawing.SizeF(97.15271F, 16.74997F);
            this.lblTotalLaborDisp.StylePriority.UseFont = false;
            this.lblTotalLaborDisp.StylePriority.UseTextAlignment = false;
            this.lblTotalLaborDisp.Text = "xrLabel25";
            this.lblTotalLaborDisp.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel28
            // 
            this.xrLabel28.Font = new System.Drawing.Font("Arial", 8F);
            this.xrLabel28.LocationFloat = new DevExpress.Utils.PointFloat(684.6805F, 324.3331F);
            this.xrLabel28.Name = "xrLabel28";
            this.xrLabel28.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel28.SizeF = new System.Drawing.SizeF(212.5F, 16.74994F);
            this.xrLabel28.StylePriority.UseFont = false;
            this.xrLabel28.StylePriority.UseTextAlignment = false;
            this.xrLabel28.Text = "Total ~Ext\'d Labour:";
            this.xrLabel28.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // lblTotalMaterialDisp
            // 
            this.lblTotalMaterialDisp.Font = new System.Drawing.Font("Arial", 8F);
            this.lblTotalMaterialDisp.LocationFloat = new DevExpress.Utils.PointFloat(897.1812F, 307.5832F);
            this.lblTotalMaterialDisp.Name = "lblTotalMaterialDisp";
            this.lblTotalMaterialDisp.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblTotalMaterialDisp.SizeF = new System.Drawing.SizeF(97.15271F, 16.74997F);
            this.lblTotalMaterialDisp.StylePriority.UseFont = false;
            this.lblTotalMaterialDisp.StylePriority.UseTextAlignment = false;
            this.lblTotalMaterialDisp.Text = "xrLabel25";
            this.lblTotalMaterialDisp.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel24
            // 
            this.xrLabel24.Font = new System.Drawing.Font("Arial", 8F);
            this.xrLabel24.LocationFloat = new DevExpress.Utils.PointFloat(682.5003F, 307.5832F);
            this.xrLabel24.Name = "xrLabel24";
            this.xrLabel24.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel24.SizeF = new System.Drawing.SizeF(214.6808F, 16.74994F);
            this.xrLabel24.StylePriority.UseFont = false;
            this.xrLabel24.StylePriority.UseTextAlignment = false;
            this.xrLabel24.Text = "Total ~Ext\'d Material:";
            this.xrLabel24.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // lblTotalMargAbovDisp
            // 
            this.lblTotalMargAbovDisp.Font = new System.Drawing.Font("Arial", 8F);
            this.lblTotalMargAbovDisp.LocationFloat = new DevExpress.Utils.PointFloat(897.1812F, 290.8332F);
            this.lblTotalMargAbovDisp.Name = "lblTotalMargAbovDisp";
            this.lblTotalMargAbovDisp.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblTotalMargAbovDisp.SizeF = new System.Drawing.SizeF(97.15271F, 16.74997F);
            this.lblTotalMargAbovDisp.StylePriority.UseFont = false;
            this.lblTotalMargAbovDisp.StylePriority.UseTextAlignment = false;
            this.lblTotalMargAbovDisp.Text = "xrLabel25";
            this.lblTotalMargAbovDisp.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel22
            // 
            this.xrLabel22.Font = new System.Drawing.Font("Arial", 8F);
            this.xrLabel22.LocationFloat = new DevExpress.Utils.PointFloat(684.6812F, 290.8332F);
            this.xrLabel22.Name = "xrLabel22";
            this.xrLabel22.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel22.SizeF = new System.Drawing.SizeF(212.4996F, 16.74994F);
            this.xrLabel22.StylePriority.UseFont = false;
            this.xrLabel22.StylePriority.UseTextAlignment = false;
            this.xrLabel22.Text = "Margin $ (from quoted price):";
            this.xrLabel22.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // lblMargAbovCosDisp
            // 
            this.lblMargAbovCosDisp.Font = new System.Drawing.Font("Arial", 8F);
            this.lblMargAbovCosDisp.LocationFloat = new DevExpress.Utils.PointFloat(897.1806F, 274.0833F);
            this.lblMargAbovCosDisp.Name = "lblMargAbovCosDisp";
            this.lblMargAbovCosDisp.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblMargAbovCosDisp.SizeF = new System.Drawing.SizeF(97.15271F, 16.74997F);
            this.lblMargAbovCosDisp.StylePriority.UseFont = false;
            this.lblMargAbovCosDisp.StylePriority.UseTextAlignment = false;
            this.lblMargAbovCosDisp.Text = "xrLabel25";
            this.lblMargAbovCosDisp.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel20
            // 
            this.xrLabel20.Font = new System.Drawing.Font("Arial", 8F);
            this.xrLabel20.LocationFloat = new DevExpress.Utils.PointFloat(684.6805F, 274.0833F);
            this.xrLabel20.Name = "xrLabel20";
            this.xrLabel20.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel20.SizeF = new System.Drawing.SizeF(212.5F, 16.74994F);
            this.xrLabel20.StylePriority.UseFont = false;
            this.xrLabel20.StylePriority.UseTextAlignment = false;
            this.xrLabel20.Text = "Margin % (from quoted price):";
            this.xrLabel20.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // lblTotalCustomDisp
            // 
            this.lblTotalCustomDisp.Font = new System.Drawing.Font("Arial", 8F);
            this.lblTotalCustomDisp.LocationFloat = new DevExpress.Utils.PointFloat(897.1812F, 257.3333F);
            this.lblTotalCustomDisp.Name = "lblTotalCustomDisp";
            this.lblTotalCustomDisp.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblTotalCustomDisp.SizeF = new System.Drawing.SizeF(97.15271F, 16.74997F);
            this.lblTotalCustomDisp.StylePriority.UseFont = false;
            this.lblTotalCustomDisp.StylePriority.UseTextAlignment = false;
            this.lblTotalCustomDisp.Text = "xrLabel25";
            this.lblTotalCustomDisp.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // lblTotalItemsDisp
            // 
            this.lblTotalItemsDisp.Font = new System.Drawing.Font("Arial", 8F);
            this.lblTotalItemsDisp.LocationFloat = new DevExpress.Utils.PointFloat(897.1807F, 240.5833F);
            this.lblTotalItemsDisp.Name = "lblTotalItemsDisp";
            this.lblTotalItemsDisp.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblTotalItemsDisp.SizeF = new System.Drawing.SizeF(97.15271F, 16.75003F);
            this.lblTotalItemsDisp.StylePriority.UseFont = false;
            this.lblTotalItemsDisp.StylePriority.UseTextAlignment = false;
            this.lblTotalItemsDisp.Text = "xrLabel25";
            this.lblTotalItemsDisp.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // lblCostDisp
            // 
            this.lblCostDisp.Font = new System.Drawing.Font("Arial", 8F);
            this.lblCostDisp.LocationFloat = new DevExpress.Utils.PointFloat(897.181F, 223.8334F);
            this.lblCostDisp.Name = "lblCostDisp";
            this.lblCostDisp.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblCostDisp.SizeF = new System.Drawing.SizeF(97.15271F, 16.75003F);
            this.lblCostDisp.StylePriority.UseFont = false;
            this.lblCostDisp.StylePriority.UseTextAlignment = false;
            this.lblCostDisp.Text = "N/A";
            this.lblCostDisp.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel19
            // 
            this.xrLabel19.Font = new System.Drawing.Font("Arial", 8F);
            this.xrLabel19.LocationFloat = new DevExpress.Utils.PointFloat(682.5003F, 257.3333F);
            this.xrLabel19.Name = "xrLabel19";
            this.xrLabel19.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel19.SizeF = new System.Drawing.SizeF(214.6808F, 16.74994F);
            this.xrLabel19.StylePriority.UseFont = false;
            this.xrLabel19.StylePriority.UseTextAlignment = false;
            this.xrLabel19.Text = "Total Custom Items:";
            this.xrLabel19.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // xrLabel18
            // 
            this.xrLabel18.Font = new System.Drawing.Font("Arial", 8F);
            this.xrLabel18.LocationFloat = new DevExpress.Utils.PointFloat(682.4999F, 240.5833F);
            this.xrLabel18.Name = "xrLabel18";
            this.xrLabel18.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel18.SizeF = new System.Drawing.SizeF(214.6808F, 16.74997F);
            this.xrLabel18.StylePriority.UseFont = false;
            this.xrLabel18.StylePriority.UseTextAlignment = false;
            this.xrLabel18.Text = "Total Items:";
            this.xrLabel18.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // xrLabel16
            // 
            this.xrLabel16.Font = new System.Drawing.Font("Arial", 8F);
            this.xrLabel16.LocationFloat = new DevExpress.Utils.PointFloat(682.5001F, 223.8334F);
            this.xrLabel16.Name = "xrLabel16";
            this.xrLabel16.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel16.SizeF = new System.Drawing.SizeF(214.6808F, 16.74994F);
            this.xrLabel16.StylePriority.UseFont = false;
            this.xrLabel16.StylePriority.UseTextAlignment = false;
            this.xrLabel16.Text = "Total Cost:";
            this.xrLabel16.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // xrLabel17
            // 
            this.xrLabel17.Font = new System.Drawing.Font("Arial", 8F);
            this.xrLabel17.LocationFloat = new DevExpress.Utils.PointFloat(682.5001F, 207.0835F);
            this.xrLabel17.Name = "xrLabel17";
            this.xrLabel17.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel17.SizeF = new System.Drawing.SizeF(214.6808F, 16.74989F);
            this.xrLabel17.StylePriority.UseFont = false;
            this.xrLabel17.StylePriority.UseTextAlignment = false;
            this.xrLabel17.Text = "Bench & Extd Difference:";
            this.xrLabel17.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // lbl_benchextddiff
            // 
            this.lbl_benchextddiff.Font = new System.Drawing.Font("Arial", 8F);
            this.lbl_benchextddiff.LocationFloat = new DevExpress.Utils.PointFloat(897.1808F, 207.0834F);
            this.lbl_benchextddiff.Name = "lbl_benchextddiff";
            this.lbl_benchextddiff.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lbl_benchextddiff.SizeF = new System.Drawing.SizeF(97.15265F, 16.74998F);
            this.lbl_benchextddiff.StylePriority.UseFont = false;
            this.lbl_benchextddiff.StylePriority.UseTextAlignment = false;
            this.lbl_benchextddiff.Text = "xrLabel25";
            this.lbl_benchextddiff.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel15
            // 
            this.xrLabel15.Font = new System.Drawing.Font("Arial", 8F);
            this.xrLabel15.LocationFloat = new DevExpress.Utils.PointFloat(682.4997F, 190.3336F);
            this.xrLabel15.Name = "xrLabel15";
            this.xrLabel15.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel15.SizeF = new System.Drawing.SizeF(214.6809F, 16.74998F);
            this.xrLabel15.StylePriority.UseFont = false;
            this.xrLabel15.StylePriority.UseTextAlignment = false;
            this.xrLabel15.Text = "Total Benchmark Sell (Extd):";
            this.xrLabel15.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // lblTMDisp
            // 
            this.lblTMDisp.Font = new System.Drawing.Font("Arial", 8F);
            this.lblTMDisp.LocationFloat = new DevExpress.Utils.PointFloat(897.1806F, 190.3336F);
            this.lblTMDisp.Name = "lblTMDisp";
            this.lblTMDisp.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblTMDisp.SizeF = new System.Drawing.SizeF(97.15259F, 16.74986F);
            this.lblTMDisp.StylePriority.UseFont = false;
            this.lblTMDisp.StylePriority.UseTextAlignment = false;
            this.lblTMDisp.Text = "xrLabel25";
            this.lblTMDisp.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel14
            // 
            this.xrLabel14.Font = new System.Drawing.Font("Arial", 8F);
            this.xrLabel14.LocationFloat = new DevExpress.Utils.PointFloat(682.4998F, 173.5835F);
            this.xrLabel14.Name = "xrLabel14";
            this.xrLabel14.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel14.SizeF = new System.Drawing.SizeF(214.6809F, 16.75F);
            this.xrLabel14.StylePriority.UseFont = false;
            this.xrLabel14.StylePriority.UseTextAlignment = false;
            this.xrLabel14.Text = "Total Quoted (~Extd):";
            this.xrLabel14.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // lblQuotedDisp
            // 
            this.lblQuotedDisp.Font = new System.Drawing.Font("Arial", 8F);
            this.lblQuotedDisp.LocationFloat = new DevExpress.Utils.PointFloat(897.1808F, 173.5835F);
            this.lblQuotedDisp.Name = "lblQuotedDisp";
            this.lblQuotedDisp.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblQuotedDisp.SizeF = new System.Drawing.SizeF(97.15271F, 16.75F);
            this.lblQuotedDisp.StylePriority.UseFont = false;
            this.lblQuotedDisp.StylePriority.UseTextAlignment = false;
            this.lblQuotedDisp.Text = "xrLabel25";
            this.lblQuotedDisp.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // PageFooter
            // 
            this.PageFooter.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrPanel2});
            this.PageFooter.HeightF = 89F;
            this.PageFooter.Name = "PageFooter";
            // 
            // xrPanel2
            // 
            this.xrPanel2.AnchorVertical = DevExpress.XtraReports.UI.VerticalAnchorStyles.Bottom;
            this.xrPanel2.BackColor = System.Drawing.Color.WhiteSmoke;
            this.xrPanel2.CanGrow = false;
            this.xrPanel2.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrPictureBox1,
            this.xrPageInfo1});
            this.xrPanel2.LocationFloat = new DevExpress.Utils.PointFloat(0F, 20.00001F);
            this.xrPanel2.Name = "xrPanel2";
            this.xrPanel2.SizeF = new System.Drawing.SizeF(1003F, 58.33333F);
            this.xrPanel2.StylePriority.UseBackColor = false;
            // 
            // xrPictureBox1
            // 
            this.xrPictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("xrPictureBox1.Image")));
            this.xrPictureBox1.LocationFloat = new DevExpress.Utils.PointFloat(926.7081F, 10.00004F);
            this.xrPictureBox1.Name = "xrPictureBox1";
            this.xrPictureBox1.SizeF = new System.Drawing.SizeF(66.29199F, 38.33331F);
            this.xrPictureBox1.Sizing = DevExpress.XtraPrinting.ImageSizeMode.ZoomImage;
            // 
            // xrPageInfo1
            // 
            this.xrPageInfo1.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrPageInfo1.ForeColor = System.Drawing.Color.DimGray;
            this.xrPageInfo1.LocationFloat = new DevExpress.Utils.PointFloat(10.00007F, 9.999943F);
            this.xrPageInfo1.Name = "xrPageInfo1";
            this.xrPageInfo1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrPageInfo1.SizeF = new System.Drawing.SizeF(60.41669F, 38.33339F);
            this.xrPageInfo1.StylePriority.UseFont = false;
            this.xrPageInfo1.StylePriority.UseForeColor = false;
            this.xrPageInfo1.StylePriority.UseTextAlignment = false;
            this.xrPageInfo1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // PageHeader
            // 
            this.PageHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrPanel3});
            this.PageHeader.HeightF = 94.83337F;
            this.PageHeader.Name = "PageHeader";
            this.PageHeader.PrintOn = DevExpress.XtraReports.UI.PrintOnPages.NotWithReportHeader;
            // 
            // xrPanel3
            // 
            this.xrPanel3.BackColor = System.Drawing.Color.WhiteSmoke;
            this.xrPanel3.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrPictureBox3,
            this.xrLabel34,
            this.lblHeaderName});
            this.xrPanel3.LocationFloat = new DevExpress.Utils.PointFloat(7.152557E-05F, 10.00001F);
            this.xrPanel3.Name = "xrPanel3";
            this.xrPanel3.SizeF = new System.Drawing.SizeF(1003F, 75F);
            this.xrPanel3.StylePriority.UseBackColor = false;
            // 
            // xrPictureBox3
            // 
            this.xrPictureBox3.Image = ((System.Drawing.Image)(resources.GetObject("xrPictureBox3.Image")));
            this.xrPictureBox3.LocationFloat = new DevExpress.Utils.PointFloat(889.2643F, 8.666675F);
            this.xrPictureBox3.Name = "xrPictureBox3";
            this.xrPictureBox3.SizeF = new System.Drawing.SizeF(103.7357F, 56.33332F);
            this.xrPictureBox3.Sizing = DevExpress.XtraPrinting.ImageSizeMode.ZoomImage;
            // 
            // xrLabel34
            // 
            this.xrLabel34.BackColor = System.Drawing.Color.WhiteSmoke;
            this.xrLabel34.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel34.ForeColor = System.Drawing.Color.Black;
            this.xrLabel34.LocationFloat = new DevExpress.Utils.PointFloat(9.999998F, 8.666675F);
            this.xrLabel34.Name = "xrLabel34";
            this.xrLabel34.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel34.SizeF = new System.Drawing.SizeF(870.7081F, 31.66666F);
            this.xrLabel34.StylePriority.UseBackColor = false;
            this.xrLabel34.StylePriority.UseFont = false;
            this.xrLabel34.StylePriority.UseForeColor = false;
            this.xrLabel34.StylePriority.UseTextAlignment = false;
            this.xrLabel34.Text = "Worksheet";
            this.xrLabel34.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // lblHeaderName
            // 
            this.lblHeaderName.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lblHeaderName.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHeaderName.ForeColor = System.Drawing.Color.DimGray;
            this.lblHeaderName.LocationFloat = new DevExpress.Utils.PointFloat(9.99995F, 40.33334F);
            this.lblHeaderName.Name = "lblHeaderName";
            this.lblHeaderName.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblHeaderName.SizeF = new System.Drawing.SizeF(851.9583F, 24.66666F);
            this.lblHeaderName.StylePriority.UseBackColor = false;
            this.lblHeaderName.StylePriority.UseFont = false;
            this.lblHeaderName.StylePriority.UseForeColor = false;
            this.lblHeaderName.StylePriority.UseTextAlignment = false;
            this.lblHeaderName.Text = "lblCustomer";
            this.lblHeaderName.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrControlStyle1
            // 
            this.xrControlStyle1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.xrControlStyle1.Name = "xrControlStyle1";
            this.xrControlStyle1.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            // 
            // xrControlStyle2
            // 
            this.xrControlStyle2.Name = "xrControlStyle2";
            this.xrControlStyle2.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            // 
            // dsQuotenotes1
            // 
            this.dsQuotenotes1.DataSetName = "dsQuotenotes";
            this.dsQuotenotes1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // GroupFooter1
            // 
            this.GroupFooter1.BorderWidth = 0F;
            this.GroupFooter1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel8,
            this.xrLabel3,
            this.xrLine1,
            this.xrLabel5,
            this.xrLabel4});
            this.GroupFooter1.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GroupFooter1.HeightF = 32.08332F;
            this.GroupFooter1.KeepTogether = true;
            this.GroupFooter1.Name = "GroupFooter1";
            this.GroupFooter1.StylePriority.UseBorders = false;
            this.GroupFooter1.StylePriority.UseBorderWidth = false;
            this.GroupFooter1.StylePriority.UseFont = false;
            // 
            // xrLabel8
            // 
            this.xrLabel8.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "quote_worksheet.cost")});
            this.xrLabel8.LocationFloat = new DevExpress.Utils.PointFloat(565.0974F, 3.208351F);
            this.xrLabel8.Name = "xrLabel8";
            this.xrLabel8.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel8.SizeF = new System.Drawing.SizeF(100F, 19.79167F);
            this.xrLabel8.StylePriority.UseTextAlignment = false;
            xrSummary1.FormatString = "{0:c2}";
            xrSummary1.Func = DevExpress.XtraReports.UI.SummaryFunc.Custom;
            xrSummary1.Running = DevExpress.XtraReports.UI.SummaryRunning.Group;
            this.xrLabel8.Summary = xrSummary1;
            this.xrLabel8.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrLabel8.SummaryGetResult += new DevExpress.XtraReports.UI.SummaryGetResultHandler(this.xrLabel8_SummaryGetResult);
            // 
            // xrLabel3
            // 
            this.xrLabel3.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "quote_worksheet.margin")});
            this.xrLabel3.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel3.LocationFloat = new DevExpress.Utils.PointFloat(935.0974F, 3.208351F);
            this.xrLabel3.Name = "xrLabel3";
            this.xrLabel3.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.xrLabel3.SizeF = new System.Drawing.SizeF(61.31921F, 19.79167F);
            this.xrLabel3.StylePriority.UseFont = false;
            this.xrLabel3.StylePriority.UsePadding = false;
            this.xrLabel3.StylePriority.UseTextAlignment = false;
            xrSummary2.FormatString = "{0:0.0%}";
            xrSummary2.Func = DevExpress.XtraReports.UI.SummaryFunc.Custom;
            xrSummary2.Running = DevExpress.XtraReports.UI.SummaryRunning.Group;
            this.xrLabel3.Summary = xrSummary2;
            this.xrLabel3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrLabel3.SummaryGetResult += new DevExpress.XtraReports.UI.SummaryGetResultHandler(this.xrLabel3_SummaryGetResult);
            this.xrLabel3.SummaryReset += new System.EventHandler(this.xrLabel3_SummaryReset);
            this.xrLabel3.SummaryRowChanged += new System.EventHandler(this.xrLabel3_SummaryRowChanged);
            // 
            // xrLine1
            // 
            this.xrLine1.LocationFloat = new DevExpress.Utils.PointFloat(565.0974F, 0F);
            this.xrLine1.Name = "xrLine1";
            this.xrLine1.SizeF = new System.Drawing.SizeF(431.3199F, 3.208333F);
            // 
            // xrLabel5
            // 
            this.xrLabel5.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "quote_worksheet.extended_per")});
            this.xrLabel5.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel5.LocationFloat = new DevExpress.Utils.PointFloat(845.0974F, 3.208351F);
            this.xrLabel5.Name = "xrLabel5";
            this.xrLabel5.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.xrLabel5.SizeF = new System.Drawing.SizeF(89.99994F, 19.79167F);
            this.xrLabel5.StylePriority.UseFont = false;
            this.xrLabel5.StylePriority.UsePadding = false;
            this.xrLabel5.StylePriority.UseTextAlignment = false;
            xrSummary3.FormatString = "{0:C2}";
            xrSummary3.Running = DevExpress.XtraReports.UI.SummaryRunning.Group;
            this.xrLabel5.Summary = xrSummary3;
            this.xrLabel5.Text = "xrLabel5";
            this.xrLabel5.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // xrLabel4
            // 
            this.xrLabel4.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "quote_worksheet.extTandM")});
            this.xrLabel4.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel4.LocationFloat = new DevExpress.Utils.PointFloat(751.3336F, 3.208351F);
            this.xrLabel4.Name = "xrLabel4";
            this.xrLabel4.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.xrLabel4.SizeF = new System.Drawing.SizeF(93.76385F, 19.79167F);
            this.xrLabel4.StylePriority.UseFont = false;
            this.xrLabel4.StylePriority.UsePadding = false;
            this.xrLabel4.StylePriority.UseTextAlignment = false;
            xrSummary4.FormatString = "{0:C2}";
            xrSummary4.Running = DevExpress.XtraReports.UI.SummaryRunning.Group;
            this.xrLabel4.Summary = xrSummary4;
            this.xrLabel4.Text = "xrLabel4";
            this.xrLabel4.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // ReportFooter
            // 
            this.ReportFooter.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel7,
            this.xrLabel6,
            this.xrLabel27,
            this.xrLine2,
            this.xrLabel21,
            this.xrLabel23,
            this.xrTable3});
            this.ReportFooter.HeightF = 395.8333F;
            this.ReportFooter.KeepTogether = true;
            this.ReportFooter.Name = "ReportFooter";
            // 
            // xrLabel7
            // 
            this.xrLabel7.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "quote_worksheet.cost")});
            this.xrLabel7.Font = new System.Drawing.Font("Arial", 8.25F);
            this.xrLabel7.LocationFloat = new DevExpress.Utils.PointFloat(575.0975F, 4.583359F);
            this.xrLabel7.Name = "xrLabel7";
            this.xrLabel7.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel7.SizeF = new System.Drawing.SizeF(89.99994F, 22.91667F);
            this.xrLabel7.StylePriority.UseFont = false;
            this.xrLabel7.StylePriority.UseTextAlignment = false;
            xrSummary5.FormatString = "{0:c2}";
            xrSummary5.Func = DevExpress.XtraReports.UI.SummaryFunc.Custom;
            xrSummary5.Running = DevExpress.XtraReports.UI.SummaryRunning.Report;
            this.xrLabel7.Summary = xrSummary5;
            this.xrLabel7.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            this.xrLabel7.SummaryGetResult += new DevExpress.XtraReports.UI.SummaryGetResultHandler(this.xrLabel7_SummaryGetResult);
            // 
            // xrLabel6
            // 
            this.xrLabel6.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "quote_worksheet.margin")});
            this.xrLabel6.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel6.LocationFloat = new DevExpress.Utils.PointFloat(935.0976F, 4.583359F);
            this.xrLabel6.Name = "xrLabel6";
            this.xrLabel6.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.xrLabel6.SizeF = new System.Drawing.SizeF(61.61047F, 22.91667F);
            this.xrLabel6.StylePriority.UseFont = false;
            this.xrLabel6.StylePriority.UsePadding = false;
            this.xrLabel6.StylePriority.UseTextAlignment = false;
            xrSummary6.FormatString = "{0:0.0%}";
            xrSummary6.Func = DevExpress.XtraReports.UI.SummaryFunc.Custom;
            xrSummary6.Running = DevExpress.XtraReports.UI.SummaryRunning.Report;
            this.xrLabel6.Summary = xrSummary6;
            this.xrLabel6.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            this.xrLabel6.SummaryGetResult += new DevExpress.XtraReports.UI.SummaryGetResultHandler(this.xrLabel6_SummaryGetResult);
            this.xrLabel6.SummaryReset += new System.EventHandler(this.xrLabel6_SummaryReset);
            this.xrLabel6.SummaryRowChanged += new System.EventHandler(this.xrLabel6_SummaryRowChanged);
            // 
            // xrLabel27
            // 
            this.xrLabel27.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel27.LocationFloat = new DevExpress.Utils.PointFloat(427.5973F, 4.500008F);
            this.xrLabel27.Name = "xrLabel27";
            this.xrLabel27.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel27.SizeF = new System.Drawing.SizeF(137.5F, 23F);
            this.xrLabel27.StylePriority.UseFont = false;
            this.xrLabel27.StylePriority.UseTextAlignment = false;
            this.xrLabel27.Text = "Worksheet Total:";
            this.xrLabel27.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // xrLine2
            // 
            this.xrLine2.LocationFloat = new DevExpress.Utils.PointFloat(565.0973F, 1.375008F);
            this.xrLine2.Name = "xrLine2";
            this.xrLine2.SizeF = new System.Drawing.SizeF(431.6108F, 3.208333F);
            // 
            // xrLabel21
            // 
            this.xrLabel21.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "quote_worksheet.extTandM")});
            this.xrLabel21.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel21.LocationFloat = new DevExpress.Utils.PointFloat(751.3334F, 4.583359F);
            this.xrLabel21.Name = "xrLabel21";
            this.xrLabel21.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.xrLabel21.SizeF = new System.Drawing.SizeF(93.76398F, 22.91667F);
            this.xrLabel21.StylePriority.UseFont = false;
            this.xrLabel21.StylePriority.UsePadding = false;
            this.xrLabel21.StylePriority.UseTextAlignment = false;
            xrSummary7.FormatString = "{0:C2}";
            xrSummary7.Running = DevExpress.XtraReports.UI.SummaryRunning.Report;
            this.xrLabel21.Summary = xrSummary7;
            this.xrLabel21.Text = "xrLabel4";
            this.xrLabel21.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // xrLabel23
            // 
            this.xrLabel23.DataBindings.AddRange(new DevExpress.XtraReports.UI.XRBinding[] {
            new DevExpress.XtraReports.UI.XRBinding("Text", null, "quote_worksheet.extended_per")});
            this.xrLabel23.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel23.LocationFloat = new DevExpress.Utils.PointFloat(845.0974F, 4.583359F);
            this.xrLabel23.Name = "xrLabel23";
            this.xrLabel23.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.xrLabel23.SizeF = new System.Drawing.SizeF(90.00012F, 22.91667F);
            this.xrLabel23.StylePriority.UseFont = false;
            this.xrLabel23.StylePriority.UsePadding = false;
            this.xrLabel23.StylePriority.UseTextAlignment = false;
            xrSummary8.FormatString = "{0:C2}";
            xrSummary8.Running = DevExpress.XtraReports.UI.SummaryRunning.Report;
            this.xrLabel23.Summary = xrSummary8;
            this.xrLabel23.Text = "xrLabel5";
            this.xrLabel23.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // margin
            // 
            this.margin.DataMember = "quote_worksheet";
            this.margin.Expression = "([extended_per]-([cost]*[qty]))/[extended_per]";
            this.margin.Name = "margin";
            // 
            // xrlbl_quoted_price
            // 
            this.xrlbl_quoted_price.Font = new System.Drawing.Font("Arial", 8F);
            this.xrlbl_quoted_price.LocationFloat = new DevExpress.Utils.PointFloat(897.1805F, 156.8335F);
            this.xrlbl_quoted_price.Name = "xrlbl_quoted_price";
            this.xrlbl_quoted_price.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrlbl_quoted_price.SizeF = new System.Drawing.SizeF(97.15271F, 16.75F);
            this.xrlbl_quoted_price.StylePriority.UseFont = false;
            this.xrlbl_quoted_price.StylePriority.UseTextAlignment = false;
            this.xrlbl_quoted_price.Text = "xrLabel25";
            this.xrlbl_quoted_price.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel13
            // 
            this.xrLabel13.Font = new System.Drawing.Font("Arial", 8F);
            this.xrLabel13.LocationFloat = new DevExpress.Utils.PointFloat(682.4996F, 156.8335F);
            this.xrLabel13.Name = "xrLabel13";
            this.xrLabel13.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel13.SizeF = new System.Drawing.SizeF(214.6809F, 16.75F);
            this.xrLabel13.StylePriority.UseFont = false;
            this.xrLabel13.StylePriority.UseTextAlignment = false;
            this.xrLabel13.Text = "Quoted Price (Gen Info Tab ):";
            this.xrLabel13.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // xrTable3
            // 
            this.xrTable3.Font = new System.Drawing.Font("Calibri", 9.75F);
            this.xrTable3.LocationFloat = new DevExpress.Utils.PointFloat(37.375F, 67.70834F);
            this.xrTable3.Name = "xrTable3";
            this.xrTable3.Rows.AddRange(new DevExpress.XtraReports.UI.XRTableRow[] {
            this.xrTableRow3,
            this.xrTableRow4,
            this.xrTableRow5,
            this.xrTableRow6,
            this.xrTableRow7,
            this.xrTableRow8,
            this.xrTableRow9,
            this.xrTableRow10,
            this.xrTableRow11,
            this.xrTableRow12,
            this.xrTableRow13,
            this.xrTableRow14});
            this.xrTable3.SizeF = new System.Drawing.SizeF(527.7223F, 300F);
            this.xrTable3.StylePriority.UseFont = false;
            // 
            // xrTableRow3
            // 
            this.xrTableRow3.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell15});
            this.xrTableRow3.Name = "xrTableRow3";
            this.xrTableRow3.Weight = 1D;
            // 
            // xrTableCell15
            // 
            this.xrTableCell15.BackColor = System.Drawing.Color.Black;
            this.xrTableCell15.ForeColor = System.Drawing.Color.White;
            this.xrTableCell15.Name = "xrTableCell15";
            this.xrTableCell15.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 0, 0, 0, 100F);
            this.xrTableCell15.StylePriority.UseBackColor = false;
            this.xrTableCell15.StylePriority.UseForeColor = false;
            this.xrTableCell15.StylePriority.UsePadding = false;
            this.xrTableCell15.StylePriority.UseTextAlignment = false;
            this.xrTableCell15.Text = "Cost Price Level Legend";
            this.xrTableCell15.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell15.Weight = 3D;
            // 
            // xrTableRow4
            // 
            this.xrTableRow4.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell17});
            this.xrTableRow4.Name = "xrTableRow4";
            this.xrTableRow4.Weight = 1D;
            // 
            // xrTableCell17
            // 
            this.xrTableCell17.Name = "xrTableCell17";
            this.xrTableCell17.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 0, 0, 0, 100F);
            this.xrTableCell17.StylePriority.UsePadding = false;
            this.xrTableCell17.StylePriority.UseTextAlignment = false;
            this.xrTableCell17.Text = "Unknown";
            this.xrTableCell17.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell17.Weight = 3D;
            // 
            // xrTableRow5
            // 
            this.xrTableRow5.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell18});
            this.xrTableRow5.Name = "xrTableRow5";
            this.xrTableRow5.Weight = 1D;
            // 
            // xrTableCell18
            // 
            this.xrTableCell18.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(153)))), ((int)(((byte)(0)))));
            this.xrTableCell18.ForeColor = System.Drawing.Color.White;
            this.xrTableCell18.Name = "xrTableCell18";
            this.xrTableCell18.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 0, 0, 0, 100F);
            this.xrTableCell18.StylePriority.UseBackColor = false;
            this.xrTableCell18.StylePriority.UseForeColor = false;
            this.xrTableCell18.StylePriority.UsePadding = false;
            this.xrTableCell18.StylePriority.UseTextAlignment = false;
            this.xrTableCell18.Text = "Current On Hand Stock Cost";
            this.xrTableCell18.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell18.Weight = 3D;
            // 
            // xrTableRow6
            // 
            this.xrTableRow6.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell19});
            this.xrTableRow6.Name = "xrTableRow6";
            this.xrTableRow6.Weight = 1D;
            // 
            // xrTableCell19
            // 
            this.xrTableCell19.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(29)))), ((int)(((byte)(115)))), ((int)(((byte)(115)))));
            this.xrTableCell19.ForeColor = System.Drawing.Color.White;
            this.xrTableCell19.Name = "xrTableCell19";
            this.xrTableCell19.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 0, 0, 0, 100F);
            this.xrTableCell19.StylePriority.UseBackColor = false;
            this.xrTableCell19.StylePriority.UseForeColor = false;
            this.xrTableCell19.StylePriority.UsePadding = false;
            this.xrTableCell19.StylePriority.UseTextAlignment = false;
            this.xrTableCell19.Text = "From Last Cut PO for Stock (Within 2 years)";
            this.xrTableCell19.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell19.Weight = 3D;
            // 
            // xrTableRow7
            // 
            this.xrTableRow7.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell20});
            this.xrTableRow7.Name = "xrTableRow7";
            this.xrTableRow7.Weight = 1D;
            // 
            // xrTableCell20
            // 
            this.xrTableCell20.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(153)))), ((int)(((byte)(153)))));
            this.xrTableCell20.ForeColor = System.Drawing.Color.White;
            this.xrTableCell20.Name = "xrTableCell20";
            this.xrTableCell20.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 0, 0, 0, 100F);
            this.xrTableCell20.StylePriority.UseBackColor = false;
            this.xrTableCell20.StylePriority.UseForeColor = false;
            this.xrTableCell20.StylePriority.UsePadding = false;
            this.xrTableCell20.StylePriority.UseTextAlignment = false;
            this.xrTableCell20.Text = "From Saved Vendor Pricing (Within 2 years)";
            this.xrTableCell20.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell20.Weight = 3D;
            // 
            // xrTableRow8
            // 
            this.xrTableRow8.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell21});
            this.xrTableRow8.Name = "xrTableRow8";
            this.xrTableRow8.Weight = 1D;
            // 
            // xrTableCell21
            // 
            this.xrTableCell21.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(92)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.xrTableCell21.ForeColor = System.Drawing.Color.White;
            this.xrTableCell21.Name = "xrTableCell21";
            this.xrTableCell21.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 0, 0, 0, 100F);
            this.xrTableCell21.StylePriority.UseBackColor = false;
            this.xrTableCell21.StylePriority.UseForeColor = false;
            this.xrTableCell21.StylePriority.UsePadding = false;
            this.xrTableCell21.StylePriority.UseTextAlignment = false;
            this.xrTableCell21.Text = "From Last Cut PO for a Work Order (Within 2 years)";
            this.xrTableCell21.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell21.Weight = 3D;
            // 
            // xrTableRow9
            // 
            this.xrTableRow9.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell22});
            this.xrTableRow9.Name = "xrTableRow9";
            this.xrTableRow9.Weight = 1D;
            // 
            // xrTableCell22
            // 
            this.xrTableCell22.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(111)))), ((int)(((byte)(0)))));
            this.xrTableCell22.ForeColor = System.Drawing.Color.White;
            this.xrTableCell22.Name = "xrTableCell22";
            this.xrTableCell22.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 0, 0, 0, 100F);
            this.xrTableCell22.StylePriority.UseBackColor = false;
            this.xrTableCell22.StylePriority.UseForeColor = false;
            this.xrTableCell22.StylePriority.UsePadding = false;
            this.xrTableCell22.StylePriority.UseTextAlignment = false;
            this.xrTableCell22.Text = "From Last Cut PO for Stock at another branch in your region (Within 2 years)";
            this.xrTableCell22.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell22.Weight = 3D;
            // 
            // xrTableRow10
            // 
            this.xrTableRow10.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell23});
            this.xrTableRow10.Name = "xrTableRow10";
            this.xrTableRow10.Weight = 1D;
            // 
            // xrTableCell23
            // 
            this.xrTableCell23.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(143)))), ((int)(((byte)(48)))));
            this.xrTableCell23.ForeColor = System.Drawing.Color.White;
            this.xrTableCell23.Name = "xrTableCell23";
            this.xrTableCell23.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 0, 0, 0, 100F);
            this.xrTableCell23.StylePriority.UseBackColor = false;
            this.xrTableCell23.StylePriority.UseForeColor = false;
            this.xrTableCell23.StylePriority.UsePadding = false;
            this.xrTableCell23.StylePriority.UseTextAlignment = false;
            this.xrTableCell23.Text = "From Last Cut PO for Stock at another branch in your country (Within 2 years)";
            this.xrTableCell23.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell23.Weight = 3D;
            // 
            // xrTableRow11
            // 
            this.xrTableRow11.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell24});
            this.xrTableRow11.Name = "xrTableRow11";
            this.xrTableRow11.Weight = 1D;
            // 
            // xrTableCell24
            // 
            this.xrTableCell24.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(153)))), ((int)(((byte)(102)))));
            this.xrTableCell24.Name = "xrTableCell24";
            this.xrTableCell24.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 0, 0, 0, 100F);
            this.xrTableCell24.StylePriority.UseBackColor = false;
            this.xrTableCell24.StylePriority.UsePadding = false;
            this.xrTableCell24.StylePriority.UseTextAlignment = false;
            this.xrTableCell24.Text = "From Saved Vendor Pricing at another branch in your region (Newer than 2 years)";
            this.xrTableCell24.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell24.Weight = 3D;
            // 
            // xrTableRow12
            // 
            this.xrTableRow12.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell25});
            this.xrTableRow12.Name = "xrTableRow12";
            this.xrTableRow12.Weight = 1D;
            // 
            // xrTableCell25
            // 
            this.xrTableCell25.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(48)))), ((int)(((byte)(48)))));
            this.xrTableCell25.ForeColor = System.Drawing.Color.White;
            this.xrTableCell25.Name = "xrTableCell25";
            this.xrTableCell25.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 0, 0, 0, 100F);
            this.xrTableCell25.StylePriority.UseBackColor = false;
            this.xrTableCell25.StylePriority.UseForeColor = false;
            this.xrTableCell25.StylePriority.UsePadding = false;
            this.xrTableCell25.StylePriority.UseTextAlignment = false;
            this.xrTableCell25.Text = "From Saved Vendor Pricing at another branch in your country (Newer than 2 years)";
            this.xrTableCell25.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell25.Weight = 3D;
            // 
            // xrTableRow13
            // 
            this.xrTableRow13.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell26});
            this.xrTableRow13.Name = "xrTableRow13";
            this.xrTableRow13.Weight = 1D;
            // 
            // xrTableCell26
            // 
            this.xrTableCell26.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.xrTableCell26.ForeColor = System.Drawing.Color.White;
            this.xrTableCell26.Name = "xrTableCell26";
            this.xrTableCell26.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 0, 0, 0, 100F);
            this.xrTableCell26.StylePriority.UseBackColor = false;
            this.xrTableCell26.StylePriority.UseForeColor = false;
            this.xrTableCell26.StylePriority.UsePadding = false;
            this.xrTableCell26.StylePriority.UseTextAlignment = false;
            this.xrTableCell26.Text = "From Last Cut PO for Stock at another branch in another country (Within 2 years)";
            this.xrTableCell26.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell26.Weight = 3D;
            // 
            // xrTableRow14
            // 
            this.xrTableRow14.Cells.AddRange(new DevExpress.XtraReports.UI.XRTableCell[] {
            this.xrTableCell27});
            this.xrTableRow14.Name = "xrTableRow14";
            this.xrTableRow14.Weight = 1D;
            // 
            // xrTableCell27
            // 
            this.xrTableCell27.BackColor = System.Drawing.Color.Red;
            this.xrTableCell27.ForeColor = System.Drawing.Color.White;
            this.xrTableCell27.Name = "xrTableCell27";
            this.xrTableCell27.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 0, 0, 0, 100F);
            this.xrTableCell27.StylePriority.UseBackColor = false;
            this.xrTableCell27.StylePriority.UseForeColor = false;
            this.xrTableCell27.StylePriority.UsePadding = false;
            this.xrTableCell27.StylePriority.UseTextAlignment = false;
            this.xrTableCell27.Text = "From Saved Vendor Pricing at another branch in another country";
            this.xrTableCell27.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrTableCell27.Weight = 3D;
            // 
            // WorkSheetDisp
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.ReportHeader,
            this.GroupHeader1,
            this.PageFooter,
            this.PageHeader,
            this.GroupFooter1,
            this.ReportFooter});
            this.CalculatedFields.AddRange(new DevExpress.XtraReports.UI.CalculatedField[] {
            this.margin});
            this.DataMember = "quote_worksheet";
            this.DataSource = this.workSheet1;
            this.Landscape = true;
            this.Margins = new System.Drawing.Printing.Margins(47, 40, 27, 12);
            this.PageHeight = 850;
            this.PageWidth = 1100;
            this.Parameters.AddRange(new DevExpress.XtraReports.Parameters.Parameter[] {
            this.qid,
            this.qrev,
            this.mid});
            this.ScriptsSource = resources.GetString("$this.ScriptsSource");
            this.StyleSheet.AddRange(new DevExpress.XtraReports.UI.XRControlStyle[] {
            this.xrControlStyle1,
            this.xrControlStyle2});
            this.Version = "17.1";
            this.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.picklist_BeforePrint);
            ((System.ComponentModel.ISupportInitialize)(this.xrTable2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.workSheet1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dsQuotenotes1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrTable3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

			}

		#endregion

		private void picklist_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
			{
			var quotes = new quote(Convert.ToInt32(qid.Value + qrev.Value.ToString()));
			lblDescription.Text = quotes.txtJobDescription;
			lblCustomer.Text = quotes.txtCustomerName;
			lblHeaderName.Text = qid.Value + " V" + qrev.Value;
        
			var worksheet_adapter = new Nesi.Web.Reports.PickListDataSet.WorkSheetTableAdapters.quote_worksheetTableAdapter();
			worksheet_adapter.Fill(workSheet1.quote_worksheet, Convert.ToInt32(qid.Value), Convert.ToInt32(qrev.Value), mid.Value.ToString());

			if (stuff == "85")
				{
				lblQuotedDisp.Text = Adjusted_sell;
				lblTMDisp.Text = TM_Sell;
				var margin = Math.Round((Convert.ToDouble(Adjusted_sell.Replace("$", "")) - Convert.ToDouble(TM_Sell.Replace("$", ""))) / Convert.ToDouble(TM_Sell.Replace("$", "")), 2);
				margin = margin * 100;
				lbl_benchextddiff.Text = margin + " %";
				}
			else
				{
				xrLabel4.Visible = false;
				xrLabel5.Visible = false;
				lblQuotedDisp.Visible = false;
				lbl_benchextddiff.Visible = false;
				lblTMDisp.Visible = false;
				xrTableCell14.Visible = false;
				xrTableCell16.Visible = false;
				xrTableCell4.Visible = false;
				xrTableCell6.Visible = false;
				}
			try
				{
				FillforQuote(qid.Value.ToString(), qrev.Value.ToString(), Convert.ToInt32(mid.Value));
				}
			catch { }

			}

		private void xrTableCell16_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
			{
			object objxt = GetCurrentColumnValue("extended_per") != null ? GetCurrentColumnValue("extended_per").ToString() : "0";
			object tm = GetCurrentColumnValue("extTandM") != null ? GetCurrentColumnValue("extTandM").ToString() : "0";

			var tc = (XRTableCell)Detail.FindControl("xrTableCell16", true);
			if (Convert.ToDouble(objxt) < (Convert.ToDouble(tm) * 0.99))
				{
				tc.ForeColor = System.Drawing.Color.Red;
				}
			else if (Convert.ToDouble(objxt) * 0.99 > Convert.ToDouble(tm))
				{
				tc.ForeColor = System.Drawing.Color.DarkGreen;
				}
			else
				{
				tc.ForeColor = System.Drawing.Color.Black;
				}
		
			}

		private void Detail_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
			{

	
			}

		protected void FillforQuote(string _id, string _rev, int _member_id)
			{
			using(var conn = Toolbox.connect())
				{
			var intID = Convert.ToInt32(_id + _rev);
			var quoteObj = new quote(intID);
            

			myMember = Toolbox.do_handle_authentication(65);
			double totalTM = 0;
			double totalQuote = 0;
			double totalMaterial = 0;
			double totalLabor = 0;
			var linecount = 0;
			var custlinecount = 0;
			double total_cost = 0;
			var isAllowedCost = myMember.AuthenticatedForPrivilege(58);
			var see_cost_a = isAllowedCost ? "A.cost" : "IF(IS_EXCLUDE(part_no), A.cost, 0)";
			var see_cost_c = isAllowedCost ? "C.cost" : "IF(IS_EXCLUDE(part_no), C.cost, 0)";
            xrlbl_quoted_price.Text = quote.quoted_amount(Toolbox.do_open_conn(), Convert.ToInt32(_id)).ToString("C2");
            var sql = string.Format(@"
(
SELECT 
	A.id, 
	A.section_id, 
	A.part_no, 
	A.code vend_part_no, 
	A.description description, 
	A.original_sell sell, 
	{0} cost, 
	A.qty, 
	(A.original_sell* A.qty * (1-(quote_worksheet_discount/100))) extTandM, 
	extended_per, 
	'PlaceHolder' workorder, 
	quote_worksheet_part_requested include, 
	B.section section, 
	b.id sectionid, 
	'1' dateex, 
	1 as qtyrec, 
	0 as emptyval,
	0 wo_detail_current_billtypeid,
	1.0 as qty_per_part,
	1 AS wo_detail_current_rec_no,
    0 as active,
    0 AS Division_ID,
    quote_worksheet_discount AS wo_detail_current_discount,
    0 AS qty_avail,
    0 AS track_part ,
	'' origin,
    '' reqdate , 0 Trans,cost_level,
	if(a.line_number=0,a.id,a.line_number) line_number
FROM 
	quote_worksheet A
LEFT JOIN quote_section B ON a.section_id = b.id
WHERE 
	A.quote_id = @v0 AND 
	A.revision = @v1  AND 
	A.is_checked=1 AND
    B.is_checked=1 AND    
	B.section REGEXP '^[0-9]'
order by line_number 
)
UNION
(
  SELECT
	C.id,
	C.section_id,
	C.part_no,
	C.code vend_part_no,
	C.description description,
	C.original_sell sell,
	{1} cost,
	C.qty,
	ROUND((ROUND(C.original_sell,3) * C.qty * (1-(quote_worksheet_discount/100))),2) extTandM,
	extended_per,
	'PlaceHolder' workorder, 
	quote_worksheet_part_requested include,
	D.section section,
	D.id sectionid,
	'1' dateex,
	1 as qtyrec,
	0 as emptyval,
	0 wo_detail_current_billtypeid,
	1.0 as qty_per_part,
	1 AS wo_detail_current_rec_no,
    0 as active,
    0 AS Division_ID,
    quote_worksheet_discount AS wo_detail_current_discount,
    0 AS qty_avail,
    0 AS track_part,
	'' origin,
    '' as reqdate,
 0,
cost_level, 
	if(c.line_number=0,c.id,c.line_number) line_number
FROM
	quote_worksheet C
LEFT JOIN quote_section D ON C.section_id = D.id
WHERE
	C.quote_id = @v0 AND
	C.revision = @v1 AND 
	c.is_checked=1 AND
    D.is_checked=1 AND  
	D.section REGEXP '^[^0-9]'
order by line_number
)", see_cost_a, see_cost_c);

			var quoteworksheet = Toolbox.doSQL_dt(conn, sql, new object[] { _id, _rev});
			var quoteworksheet_dv = quoteworksheet.DefaultView;
			quoteworksheet_dv.Sort = "section, line_number";
			var show_margin = true;
			var showCosts = myMember.authenticated_for("priv", 58);
			double hourstotal = 0;
			foreach (DataRow row in quoteworksheet_dv.Table.Rows)
				{
				linecount++;
				var	partNoStr		= Toolbox.ReturnBlankIfNull_string(row["part_no"]);
				var partNo			= Toolbox.ReturnZeroIfNull_int(row["part_no"]);
				var cost			= Toolbox.ReturnZeroIfNull_double(row["cost"]);
				var qty				= Toolbox.ReturnZeroIfNull_double(row["qty"]);
				var extdPer			= Toolbox.ReturnZeroIfNull_double(row["extended_per"]);
				var extTm			= Toolbox.ReturnZeroIfNull_double(row["extTandM"]);
				if (cost == 0)
					{
					show_margin = false;
					}

				if (partNoStr == "")
					{
					custlinecount++;
					}
				totalTM += extTm;
				total_cost += cost * qty;
				totalQuote += extdPer;

				if (partNo >= 990000 && partNo < 2000000)
					{
					totalLabor += extdPer;
					hourstotal += qty;
					}
				else if(partNo < 990000)
					{
					totalMaterial += extdPer;
					}
				}
			var bu		                 = new NeBusinessUnit(quoteObj.business_unit_id);
			lblTotalItemsDisp.Text       = linecount.ToString();
			lblTotalCustomDisp.Text      = custlinecount.ToString();
			lblQuotedDisp.Text           = totalQuote.ToString("C2");
			lblTMDisp.Text               = totalTM.ToString("C2");
			lbl_benchextddiff.Text       = (totalQuote - totalTM).ToString("C2");
			lblTotalMaterialDisp.Text    = totalMaterial.ToString("C2");
			lblTotalLaborDisp.Text       = totalLabor.ToString("C2");
			lblTotalQuotedLaborDisp.Text = hourstotal.ToString();
			lblCostDisp.Text             = showCosts ? NESI.BLL.Pages.Quotes.NeQuote.WorksheetTotalCost(conn, quoteObj.QuoteID, quoteObj.Revision, NESI.BLL.Pages.Quotes.NeQuote.TotalType.All).ToString("C2") : "N/A";
			xrPictureBox2.ImageUrl       = Toolbox.app_setting("Domain") + @"/images/Logos/" + bu.logo_file;
            xrPictureBox1.ImageUrl       =Toolbox.app_setting("Domain") + @"/images/Logos/" + bu.logo_file;
            xrPictureBox3.ImageUrl       =Toolbox.app_setting("Domain") + @"/images/Logos/" + bu.logo_file;

            if (show_margin == false)
				{
				lblMargAbovCosDisp.Text = "N/A with $0 Cost";
				    lblTotalMargAbovDisp.Text = "N/A with $0 Cost";

            }
			else
				{
				if (myMember.AuthenticatedForPrivilege(81))
					{
					try
						{
						lblMargAbovCosDisp.Text = Toolbox.doSQL_double(conn, @"SELECT IFNULL(((@v2-sum(cost*qty))/@v2),0) as margin FROM quote_worksheet  WHERE quote_id = @v0 AND revision = @v1 and is_checked=1",
new object[] { quoteObj.QuoteID,quoteObj.Revision, quoteObj.Price }).ToString("P2");
						}
					catch
						{
						lblMargAbovCosDisp.Text = "N/A";
						}
					lblTotalMargAbovDisp.Text = Toolbox.doSQL_double(conn, @"SELECT IFNULL((@v2-sum(cost*qty)),0) as margin FROM quote_worksheet  WHERE quote_id = @v0 AND revision = @v1  and is_checked=1",
					    new object[] { quoteObj.QuoteID, quoteObj.Revision, quoteObj.Price }).ToString("C2");
                }
				}
				}
			}
		double total_sell = 0;
		double total_cost = 0;
		double ttotal_sell = 0;
		double ttotal_cost = 0;

		private void xrLabel3_SummaryReset(object sender, EventArgs e)
			{
			total_cost = 0;
			total_sell = 0;
			}

		private void xrLabel3_SummaryRowChanged(object sender, EventArgs e)
			{
			total_cost += (Convert.ToDouble(GetCurrentColumnValue("qty")) * Convert.ToDouble(GetCurrentColumnValue("cost")));
			total_sell += Convert.ToDouble(GetCurrentColumnValue("extended_per"));
			}

		private void xrLabel3_SummaryGetResult(object sender, SummaryGetResultEventArgs e)
			{
			e.Result = ((total_sell - total_cost) / total_sell);
			e.Handled = true;
			}

		private void xrLabel6_SummaryReset(object sender, EventArgs e)
			{
			ttotal_cost = 0;
			ttotal_sell = 0;
			}

		private void xrLabel6_SummaryGetResult(object sender, SummaryGetResultEventArgs e)
			{
			e.Result = ((ttotal_sell - ttotal_cost) / ttotal_sell);
			e.Handled = true;
			} 

		private void xrLabel6_SummaryRowChanged(object sender, EventArgs e)
			{
      
			ttotal_cost += (Convert.ToDouble(GetCurrentColumnValue("qty")) * Convert.ToDouble(GetCurrentColumnValue("cost")));
			ttotal_sell += Convert.ToDouble(GetCurrentColumnValue("extended_per"));
			}

		private void xrLabel7_SummaryGetResult(object sender, SummaryGetResultEventArgs e)
			{
			e.Result = ttotal_cost;
			e.Handled = true;
			}

		private void xrLabel8_SummaryGetResult(object sender, SummaryGetResultEventArgs e)
			{
			e.Result = total_cost;
			e.Handled = true;
			}

        private void xrTableCell14_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
        
            var bg = new Color();
            var fg = new Color();
            switch (GetCurrentColumnValue("cost_level").ToString())
            {
                case "0": // Non Existant
                    bg = ColorTranslator.FromHtml("#fff");
                    fg = ColorTranslator.FromHtml("#000");
                    break;
                case "1": // Moving
                    bg = ColorTranslator.FromHtml("#090");
                    fg = ColorTranslator.FromHtml("#fff");
                    break;
                case "2": // PO Cut to Inventory within 2 years
                    bg = ColorTranslator.FromHtml("#1D7373");
                    fg = ColorTranslator.FromHtml("#fff");
                    break;
                case "3": // Price table for branch
                    bg = ColorTranslator.FromHtml("#099");
                    fg = ColorTranslator.FromHtml("#fff");
                    break;
                case "4": // PO cut to work order
                    bg = ColorTranslator.FromHtml("#5ccccc");
                    fg = ColorTranslator.FromHtml("#007");
                    break;
                case "5": // PO cut to Regional inventory
                    bg = ColorTranslator.FromHtml("#a66f00");
                    fg = ColorTranslator.FromHtml("#fff");
                    break;
                case "6": // PO cut to Country Inventory
                    bg = ColorTranslator.FromHtml("#bf8f30");
                    fg = ColorTranslator.FromHtml("#fff");
                    break;
                case "7": // Regional price table;
                    bg = ColorTranslator.FromHtml("#f96");
                    fg = ColorTranslator.FromHtml("#fff");
                    break;
                case "8": // Country price table
                    bg = ColorTranslator.FromHtml("#bf3030");
                    fg = ColorTranslator.FromHtml("#fff");
                    break;
                case "9": // PO cut to other country
                    bg = ColorTranslator.FromHtml("#a60000");
                    fg = ColorTranslator.FromHtml("#fff");
                    break;
                case "10": // Other country price table
                    bg = ColorTranslator.FromHtml("#f00");
                    fg = ColorTranslator.FromHtml("#fff");
                    break;
            }
            xrTableCell14.BackColor = bg;
            xrTableCell14.ForeColor = fg;
            
            
        }
    }
	}
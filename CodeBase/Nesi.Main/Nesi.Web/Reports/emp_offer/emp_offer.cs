using System;
using DevExpress.XtraReports.UI;
using Nesi.Web.Reports.emp_offer;

namespace nesi.core.print
	{
	/// <summary>
	/// Summary description for emp_offer
	/// </summary>
	public class emp_offer : XtraReport
		{
		private DetailBand Detail;
		private TopMarginBand TopMargin;
		private BottomMarginBand BottomMargin;
		private dt_emp_offer dt_emp_offer1;
		private dt_emp_offer dt_emp_offer2;
		private dt_emp_offer dt_emp_offer3;
		private dt_emp_offer dt_emp_offer4;
		private DevExpress.XtraReports.Parameters.Parameter moid;
		private XRPictureBox xrPictureBox1;
		private ReportHeaderBand ReportHeader;
		private XRLabel lblheader;
		private XRLabel xrLabel1;
		private XRLabel xrLabel2;
		private FormattingRule formattingRule1;
		private XRLabel lblwearepleased;
		private XRLabel lblname1;
		private XRLabel xrLabel5;
		private XRSubreport xrSubreport1;
		private XRLabel xrLabel6;
		private ReportFooterBand ReportFooter;
		private XRLabel lblwage;
		private XRLabel lblcomp;
		private XRLabel xrLabel12;
		private XRLabel xrLabel14;
		private XRLabel xrLabel15;
		private XRLabel xrLabel16;
		private XRLabel Date;
		private XRLine xrLine2;
		private XRLabel lblsigapplicant;
		private XRLine xrLine1;
		private XRLabel xrLabel18;
		private XRLabel lbladd2;
		private XRLabel lbladd1;
		private XRPageInfo xrPageInfo1;
		private PageFooterBand PageFooter;
		private XRPanel pnl_terms;
		private XRRichText sig_prelude;
		private XRLabel xrLabel17;
		private XRPanel pnl_member;
		private XRLabel lbl_benefits;
		private XRLabel xrLabel8;
		private XRLabel xrLabel7;
		private XRLabel xrLabel21;
		private XRLabel xrLabel23;
		private XRLabel xrLabel22;
		private XRLabel rt_vacation;
		private XRLabel xrLabel25;
		private XRLabel xrLabel10;
		private XRLabel xrLabel9;
		private XRLabel xrLabel13;
		private DevExpress.XtraReports.Parameters.Parameter jd;
		private XRLabel lbl_dear_name;
		private XRLabel xrLabel3;
		private XRLabel xrLabel20;
		private XRLabel lblcomp_final;
		private XRLabel xrLabel26;
		private XRLabel xrLabel24;
		private XRLabel xrLabel4;
		private XRLabel xrLabel33;
		private XRLabel xrLabel31;
		private XRLabel xrLabel30;
		private XRLabel xrLabel29;
		private XRLabel xrLabel28;
		private XRLabel xrLabel27;
		private XRLabel xrLabel39;
		private XRLabel xrLabel38;
		private XRLabel xrLabel37;
		private XRLabel xrLabel36;
		private XRLabel xrLabel35;
		private XRLabel xrLabel34;
		private XRLabel xrLabel32;
		private XRLabel xrlbl_governinglaw;
		private XRPanel pnl_comp;
		private XRLabel xrLabel40;
		private XRLabel lbl_milestone_header;
		private XRSubreport xrSubreport_milestones;
		private XRLabel xrLabel11;
		private XRLabel xrLabel19;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		public emp_offer()
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
            string resourceFileName = "emp_offer.resx";
            System.Resources.ResourceManager resources = global::Resources.emp_offer.ResourceManager;
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.xrLabel21 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel23 = new DevExpress.XtraReports.UI.XRLabel();
            this.pnl_member = new DevExpress.XtraReports.UI.XRPanel();
            this.xrLabel9 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel10 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel14 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel12 = new DevExpress.XtraReports.UI.XRLabel();
            this.lblcomp = new DevExpress.XtraReports.UI.XRLabel();
            this.lblwage = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel6 = new DevExpress.XtraReports.UI.XRLabel();
            this.rt_vacation = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel7 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel8 = new DevExpress.XtraReports.UI.XRLabel();
            this.lbl_benefits = new DevExpress.XtraReports.UI.XRLabel();
            this.pnl_terms = new DevExpress.XtraReports.UI.XRPanel();
            this.xrLabel32 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrlbl_governinglaw = new DevExpress.XtraReports.UI.XRLabel();
            this.sig_prelude = new DevExpress.XtraReports.UI.XRRichText();
            this.xrLabel17 = new DevExpress.XtraReports.UI.XRLabel();
            this.Date = new DevExpress.XtraReports.UI.XRLabel();
            this.lblsigapplicant = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLine2 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLine1 = new DevExpress.XtraReports.UI.XRLine();
            this.xrLabel18 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel33 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel39 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel31 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel36 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel30 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel38 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel29 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel37 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel28 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel35 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel27 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel4 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel24 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel26 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel34 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel25 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel15 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel22 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel16 = new DevExpress.XtraReports.UI.XRLabel();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.lblheader = new DevExpress.XtraReports.UI.XRLabel();
            this.xrPictureBox1 = new DevExpress.XtraReports.UI.XRPictureBox();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.dt_emp_offer1 = new dt_emp_offer();
            this.dt_emp_offer2 = new dt_emp_offer();
            this.dt_emp_offer3 = new dt_emp_offer();
            this.dt_emp_offer4 = new dt_emp_offer();
            this.moid = new DevExpress.XtraReports.Parameters.Parameter();
            this.ReportHeader = new DevExpress.XtraReports.UI.ReportHeaderBand();
            this.lbl_dear_name = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel13 = new DevExpress.XtraReports.UI.XRLabel();
            this.lbladd2 = new DevExpress.XtraReports.UI.XRLabel();
            this.lbladd1 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel5 = new DevExpress.XtraReports.UI.XRLabel();
            this.lblwearepleased = new DevExpress.XtraReports.UI.XRLabel();
            this.lblname1 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel2 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrSubreport1 = new DevExpress.XtraReports.UI.XRSubreport();
            this.formattingRule1 = new DevExpress.XtraReports.UI.FormattingRule();
            this.ReportFooter = new DevExpress.XtraReports.UI.ReportFooterBand();
            this.lbl_milestone_header = new DevExpress.XtraReports.UI.XRLabel();
            this.xrSubreport_milestones = new DevExpress.XtraReports.UI.XRSubreport();
            this.xrLabel11 = new DevExpress.XtraReports.UI.XRLabel();
            this.pnl_comp = new DevExpress.XtraReports.UI.XRPanel();
            this.xrLabel40 = new DevExpress.XtraReports.UI.XRLabel();
            this.lblcomp_final = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel20 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel3 = new DevExpress.XtraReports.UI.XRLabel();
            this.PageFooter = new DevExpress.XtraReports.UI.PageFooterBand();
            this.xrLabel19 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrPageInfo1 = new DevExpress.XtraReports.UI.XRPageInfo();
            this.jd = new DevExpress.XtraReports.Parameters.Parameter();
            ((System.ComponentModel.ISupportInitialize)(this.sig_prelude)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dt_emp_offer1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dt_emp_offer2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dt_emp_offer3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dt_emp_offer4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel21,
            this.xrLabel23,
            this.pnl_member,
            this.xrLabel14,
            this.xrLabel12,
            this.lblcomp,
            this.lblwage,
            this.xrLabel6,
            this.rt_vacation,
            this.xrLabel7,
            this.xrLabel8,
            this.lbl_benefits,
            this.pnl_terms});
            this.Detail.HeightF = 977.875F;
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrLabel21
            // 
            this.xrLabel21.CanShrink = true;
            this.xrLabel21.Font = new System.Drawing.Font("Arial", 8F);
            this.xrLabel21.LocationFloat = new DevExpress.Utils.PointFloat(9.999911F, 221.2085F);
            this.xrLabel21.Multiline = true;
            this.xrLabel21.Name = "xrLabel21";
            this.xrLabel21.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel21.SizeF = new System.Drawing.SizeF(632.9166F, 16.75003F);
            this.xrLabel21.StylePriority.UseFont = false;
            this.xrLabel21.Text = resources.GetString("xrLabel21.Text");
            // 
            // xrLabel23
            // 
            this.xrLabel23.CanShrink = true;
            this.xrLabel23.Font = new System.Drawing.Font("Arial", 8F);
            this.xrLabel23.LocationFloat = new DevExpress.Utils.PointFloat(9.999911F, 266.4585F);
            this.xrLabel23.Multiline = true;
            this.xrLabel23.Name = "xrLabel23";
            this.xrLabel23.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel23.SizeF = new System.Drawing.SizeF(633.3334F, 14.66666F);
            this.xrLabel23.StylePriority.UseFont = false;
            this.xrLabel23.Text = resources.GetString("xrLabel23.Text");
            // 
            // pnl_member
            // 
            this.pnl_member.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel9,
            this.xrLabel10});
            this.pnl_member.LocationFloat = new DevExpress.Utils.PointFloat(1.388635F, 165.0278F);
            this.pnl_member.Name = "pnl_member";
            this.pnl_member.SizeF = new System.Drawing.SizeF(646.5278F, 25.54169F);
            // 
            // xrLabel9
            // 
            this.xrLabel9.BackColor = System.Drawing.Color.Empty;
            this.xrLabel9.Font = new System.Drawing.Font("Arial", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))));
            this.xrLabel9.ForeColor = System.Drawing.Color.Black;
            this.xrLabel9.LocationFloat = new DevExpress.Utils.PointFloat(0F, 3.374908F);
            this.xrLabel9.Name = "xrLabel9";
            this.xrLabel9.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel9.SizeF = new System.Drawing.SizeF(640.2084F, 4.833298F);
            this.xrLabel9.StylePriority.UseBackColor = false;
            this.xrLabel9.StylePriority.UseFont = false;
            this.xrLabel9.StylePriority.UseForeColor = false;
            this.xrLabel9.StylePriority.UseTextAlignment = false;
            this.xrLabel9.Text = "Probationary Period";
            this.xrLabel9.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel10
            // 
            this.xrLabel10.CanShrink = true;
            this.xrLabel10.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel10.LocationFloat = new DevExpress.Utils.PointFloat(8.958404F, 13.12497F);
            this.xrLabel10.Multiline = true;
            this.xrLabel10.Name = "xrLabel10";
            this.xrLabel10.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel10.SizeF = new System.Drawing.SizeF(633.7501F, 7.625061F);
            this.xrLabel10.StylePriority.UseFont = false;
            // 
            // xrLabel14
            // 
            this.xrLabel14.BackColor = System.Drawing.Color.Empty;
            this.xrLabel14.Font = new System.Drawing.Font("Arial", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))));
            this.xrLabel14.ForeColor = System.Drawing.Color.Black;
            this.xrLabel14.LocationFloat = new DevExpress.Utils.PointFloat(0F, 242.4169F);
            this.xrLabel14.Name = "xrLabel14";
            this.xrLabel14.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel14.SizeF = new System.Drawing.SizeF(649.9999F, 18.83334F);
            this.xrLabel14.StylePriority.UseBackColor = false;
            this.xrLabel14.StylePriority.UseFont = false;
            this.xrLabel14.StylePriority.UseForeColor = false;
            this.xrLabel14.StylePriority.UseTextAlignment = false;
            this.xrLabel14.Text = "Termination of the Employment Agreement";
            this.xrLabel14.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel12
            // 
            this.xrLabel12.BackColor = System.Drawing.Color.Empty;
            this.xrLabel12.Font = new System.Drawing.Font("Arial", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))));
            this.xrLabel12.ForeColor = System.Drawing.Color.Black;
            this.xrLabel12.LocationFloat = new DevExpress.Utils.PointFloat(0F, 196.125F);
            this.xrLabel12.Name = "xrLabel12";
            this.xrLabel12.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel12.SizeF = new System.Drawing.SizeF(649.9999F, 18.83334F);
            this.xrLabel12.StylePriority.UseBackColor = false;
            this.xrLabel12.StylePriority.UseFont = false;
            this.xrLabel12.StylePriority.UseForeColor = false;
            this.xrLabel12.StylePriority.UseTextAlignment = false;
            this.xrLabel12.Text = "The Company Policies";
            this.xrLabel12.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // lblcomp
            // 
            this.lblcomp.CanShrink = true;
            this.lblcomp.Font = new System.Drawing.Font("Arial", 9F);
            this.lblcomp.LocationFloat = new DevExpress.Utils.PointFloat(9.583251F, 47.95837F);
            this.lblcomp.Multiline = true;
            this.lblcomp.Name = "lblcomp";
            this.lblcomp.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblcomp.SizeF = new System.Drawing.SizeF(633.3334F, 2F);
            this.lblcomp.StylePriority.UseFont = false;
            this.lblcomp.Text = "lblcomp";
            // 
            // lblwage
            // 
            this.lblwage.CanShrink = true;
            this.lblwage.Font = new System.Drawing.Font("Arial", 9F);
            this.lblwage.LocationFloat = new DevExpress.Utils.PointFloat(9.583251F, 24.95838F);
            this.lblwage.Multiline = true;
            this.lblwage.Name = "lblwage";
            this.lblwage.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblwage.SizeF = new System.Drawing.SizeF(633.3334F, 12.58333F);
            this.lblwage.StylePriority.UseFont = false;
            this.lblwage.Text = "lblwage";
            // 
            // xrLabel6
            // 
            this.xrLabel6.BackColor = System.Drawing.Color.Empty;
            this.xrLabel6.Font = new System.Drawing.Font("Arial", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))));
            this.xrLabel6.ForeColor = System.Drawing.Color.Black;
            this.xrLabel6.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrLabel6.Name = "xrLabel6";
            this.xrLabel6.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel6.SizeF = new System.Drawing.SizeF(650F, 18.83334F);
            this.xrLabel6.StylePriority.UseBackColor = false;
            this.xrLabel6.StylePriority.UseFont = false;
            this.xrLabel6.StylePriority.UseForeColor = false;
            this.xrLabel6.StylePriority.UseTextAlignment = false;
            this.xrLabel6.Text = "Compensation";
            this.xrLabel6.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // rt_vacation
            // 
            this.rt_vacation.CanShrink = true;
            this.rt_vacation.Font = new System.Drawing.Font("Arial", 9F);
            this.rt_vacation.LocationFloat = new DevExpress.Utils.PointFloat(10.34707F, 95.61124F);
            this.rt_vacation.Multiline = true;
            this.rt_vacation.Name = "rt_vacation";
            this.rt_vacation.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.rt_vacation.SizeF = new System.Drawing.SizeF(630.9028F, 18.83335F);
            this.rt_vacation.StylePriority.UseFont = false;
            this.rt_vacation.Text = "rt_vacation";
            // 
            // xrLabel7
            // 
            this.xrLabel7.BackColor = System.Drawing.Color.Empty;
            this.xrLabel7.Font = new System.Drawing.Font("Arial", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))));
            this.xrLabel7.ForeColor = System.Drawing.Color.Black;
            this.xrLabel7.LocationFloat = new DevExpress.Utils.PointFloat(0.3471586F, 69.56948F);
            this.xrLabel7.Name = "xrLabel7";
            this.xrLabel7.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel7.SizeF = new System.Drawing.SizeF(647.5695F, 18.83334F);
            this.xrLabel7.StylePriority.UseBackColor = false;
            this.xrLabel7.StylePriority.UseFont = false;
            this.xrLabel7.StylePriority.UseForeColor = false;
            this.xrLabel7.StylePriority.UseTextAlignment = false;
            this.xrLabel7.Text = "Vacation";
            this.xrLabel7.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel8
            // 
            this.xrLabel8.BackColor = System.Drawing.Color.Empty;
            this.xrLabel8.Font = new System.Drawing.Font("Arial", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))));
            this.xrLabel8.ForeColor = System.Drawing.Color.Black;
            this.xrLabel8.LocationFloat = new DevExpress.Utils.PointFloat(0.3471586F, 121.4445F);
            this.xrLabel8.Name = "xrLabel8";
            this.xrLabel8.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel8.SizeF = new System.Drawing.SizeF(647.5695F, 18.83334F);
            this.xrLabel8.StylePriority.UseBackColor = false;
            this.xrLabel8.StylePriority.UseFont = false;
            this.xrLabel8.StylePriority.UseForeColor = false;
            this.xrLabel8.StylePriority.UseTextAlignment = false;
            this.xrLabel8.Text = "Benefits";
            this.xrLabel8.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // lbl_benefits
            // 
            this.lbl_benefits.CanShrink = true;
            this.lbl_benefits.Font = new System.Drawing.Font("Arial", 9F);
            this.lbl_benefits.LocationFloat = new DevExpress.Utils.PointFloat(9.93042F, 145.5278F);
            this.lbl_benefits.Multiline = true;
            this.lbl_benefits.Name = "lbl_benefits";
            this.lbl_benefits.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lbl_benefits.SizeF = new System.Drawing.SizeF(630.9028F, 8.416687F);
            this.lbl_benefits.StylePriority.UseFont = false;
            this.lbl_benefits.Text = "lbl_benefits";
            // 
            // pnl_terms
            // 
            this.pnl_terms.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel32,
            this.xrlbl_governinglaw,
            this.sig_prelude,
            this.xrLabel17,
            this.Date,
            this.lblsigapplicant,
            this.xrLine2,
            this.xrLine1,
            this.xrLabel18,
            this.xrLabel33,
            this.xrLabel39,
            this.xrLabel31,
            this.xrLabel36,
            this.xrLabel30,
            this.xrLabel38,
            this.xrLabel29,
            this.xrLabel37,
            this.xrLabel28,
            this.xrLabel35,
            this.xrLabel27,
            this.xrLabel4,
            this.xrLabel24,
            this.xrLabel26,
            this.xrLabel34,
            this.xrLabel25,
            this.xrLabel15,
            this.xrLabel22,
            this.xrLabel16});
            this.pnl_terms.KeepTogether = false;
            this.pnl_terms.LocationFloat = new DevExpress.Utils.PointFloat(0F, 281.1251F);
            this.pnl_terms.Name = "pnl_terms";
            this.pnl_terms.SizeF = new System.Drawing.SizeF(647.9167F, 676.5F);
            // 
            // xrLabel32
            // 
            this.xrLabel32.BackColor = System.Drawing.Color.Empty;
            this.xrLabel32.Font = new System.Drawing.Font("Arial", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))));
            this.xrLabel32.ForeColor = System.Drawing.Color.Black;
            this.xrLabel32.LocationFloat = new DevExpress.Utils.PointFloat(0F, 368.4167F);
            this.xrLabel32.Name = "xrLabel32";
            this.xrLabel32.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel32.SizeF = new System.Drawing.SizeF(587.0834F, 18.83337F);
            this.xrLabel32.StylePriority.UseBackColor = false;
            this.xrLabel32.StylePriority.UseFont = false;
            this.xrLabel32.StylePriority.UseForeColor = false;
            this.xrLabel32.StylePriority.UseTextAlignment = false;
            this.xrLabel32.Text = "Governing Law";
            this.xrLabel32.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrlbl_governinglaw
            // 
            this.xrlbl_governinglaw.Font = new System.Drawing.Font("Arial", 8F);
            this.xrlbl_governinglaw.LocationFloat = new DevExpress.Utils.PointFloat(7.916578F, 387.2501F);
            this.xrlbl_governinglaw.Name = "xrlbl_governinglaw";
            this.xrlbl_governinglaw.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrlbl_governinglaw.SizeF = new System.Drawing.SizeF(629.5834F, 23F);
            this.xrlbl_governinglaw.StylePriority.UseFont = false;
            this.xrlbl_governinglaw.Text = "This agreement shall be governed by the laws of the province of Ontario and the l" +
    "aws of Canada in force in Ontario.";
            // 
            // sig_prelude
            // 
            this.sig_prelude.Font = new System.Drawing.Font("Arial", 8F);
            this.sig_prelude.KeepTogether = true;
            this.sig_prelude.LocationFloat = new DevExpress.Utils.PointFloat(7.916662F, 429.0834F);
            this.sig_prelude.Name = "sig_prelude";
            this.sig_prelude.SerializableRtfString = resources.GetString("sig_prelude.SerializableRtfString");
            this.sig_prelude.SizeF = new System.Drawing.SizeF(637.9167F, 5.083206F);
            this.sig_prelude.StylePriority.UseFont = false;
            // 
            // xrLabel17
            // 
            this.xrLabel17.BackColor = System.Drawing.Color.Empty;
            this.xrLabel17.Font = new System.Drawing.Font("Arial", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))));
            this.xrLabel17.ForeColor = System.Drawing.Color.Black;
            this.xrLabel17.LocationFloat = new DevExpress.Utils.PointFloat(0F, 410.2501F);
            this.xrLabel17.Name = "xrLabel17";
            this.xrLabel17.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel17.SizeF = new System.Drawing.SizeF(637.4998F, 18.83331F);
            this.xrLabel17.StylePriority.UseBackColor = false;
            this.xrLabel17.StylePriority.UseFont = false;
            this.xrLabel17.StylePriority.UseForeColor = false;
            this.xrLabel17.StylePriority.UseTextAlignment = false;
            this.xrLabel17.Text = "Acceptance";
            this.xrLabel17.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // Date
            // 
            this.Date.LocationFloat = new DevExpress.Utils.PointFloat(387.4998F, 643.5833F);
            this.Date.Name = "Date";
            this.Date.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.Date.SizeF = new System.Drawing.SizeF(204.1667F, 16.66667F);
            this.Date.StylePriority.UseTextAlignment = false;
            this.Date.Text = "Date";
            this.Date.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // lblsigapplicant
            // 
            this.lblsigapplicant.LocationFloat = new DevExpress.Utils.PointFloat(14.58309F, 643.5833F);
            this.lblsigapplicant.Name = "lblsigapplicant";
            this.lblsigapplicant.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblsigapplicant.SizeF = new System.Drawing.SizeF(192.7083F, 16.66667F);
            this.lblsigapplicant.Text = "lblsigapplicant";
            // 
            // xrLine2
            // 
            this.xrLine2.LocationFloat = new DevExpress.Utils.PointFloat(385.4165F, 635.2498F);
            this.xrLine2.Name = "xrLine2";
            this.xrLine2.SizeF = new System.Drawing.SizeF(206.25F, 2.083282F);
            // 
            // xrLine1
            // 
            this.xrLine1.LocationFloat = new DevExpress.Utils.PointFloat(12.49976F, 635.2498F);
            this.xrLine1.Name = "xrLine1";
            this.xrLine1.SizeF = new System.Drawing.SizeF(196.875F, 2F);
            // 
            // xrLabel18
            // 
            this.xrLabel18.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold);
            this.xrLabel18.KeepTogether = true;
            this.xrLabel18.LocationFloat = new DevExpress.Utils.PointFloat(12.49978F, 503.0831F);
            this.xrLabel18.Name = "xrLabel18";
            this.xrLabel18.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel18.SizeF = new System.Drawing.SizeF(629.5834F, 66.74994F);
            this.xrLabel18.StylePriority.UseFont = false;
            this.xrLabel18.Text = resources.GetString("xrLabel18.Text");
            // 
            // xrLabel33
            // 
            this.xrLabel33.BackColor = System.Drawing.Color.Empty;
            this.xrLabel33.Font = new System.Drawing.Font("Arial", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))));
            this.xrLabel33.ForeColor = System.Drawing.Color.Black;
            this.xrLabel33.LocationFloat = new DevExpress.Utils.PointFloat(2.083166F, 471.7498F);
            this.xrLabel33.Name = "xrLabel33";
            this.xrLabel33.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel33.SizeF = new System.Drawing.SizeF(629.5834F, 18.83325F);
            this.xrLabel33.StylePriority.UseBackColor = false;
            this.xrLabel33.StylePriority.UseFont = false;
            this.xrLabel33.StylePriority.UseForeColor = false;
            this.xrLabel33.StylePriority.UseTextAlignment = false;
            this.xrLabel33.Text = "Acceptance Clause";
            this.xrLabel33.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel39
            // 
            this.xrLabel39.Font = new System.Drawing.Font("Arial", 8F);
            this.xrLabel39.LocationFloat = new DevExpress.Utils.PointFloat(8.263737F, 345.4167F);
            this.xrLabel39.Name = "xrLabel39";
            this.xrLabel39.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel39.SizeF = new System.Drawing.SizeF(633.3334F, 23F);
            this.xrLabel39.StylePriority.UseFont = false;
            this.xrLabel39.Text = resources.GetString("xrLabel39.Text");
            // 
            // xrLabel31
            // 
            this.xrLabel31.BackColor = System.Drawing.Color.Empty;
            this.xrLabel31.Font = new System.Drawing.Font("Arial", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))));
            this.xrLabel31.ForeColor = System.Drawing.Color.Black;
            this.xrLabel31.LocationFloat = new DevExpress.Utils.PointFloat(1.73592F, 326.5832F);
            this.xrLabel31.Name = "xrLabel31";
            this.xrLabel31.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel31.SizeF = new System.Drawing.SizeF(640.9723F, 15.36115F);
            this.xrLabel31.StylePriority.UseBackColor = false;
            this.xrLabel31.StylePriority.UseFont = false;
            this.xrLabel31.StylePriority.UseForeColor = false;
            this.xrLabel31.StylePriority.UseTextAlignment = false;
            this.xrLabel31.Text = "Changes";
            this.xrLabel31.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel36
            // 
            this.xrLabel36.Font = new System.Drawing.Font("Arial", 8F);
            this.xrLabel36.LocationFloat = new DevExpress.Utils.PointFloat(7.91662F, 300.8054F);
            this.xrLabel36.Name = "xrLabel36";
            this.xrLabel36.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel36.SizeF = new System.Drawing.SizeF(633.3334F, 23F);
            this.xrLabel36.StylePriority.UseFont = false;
            this.xrLabel36.Text = resources.GetString("xrLabel36.Text");
            // 
            // xrLabel30
            // 
            this.xrLabel30.BackColor = System.Drawing.Color.Empty;
            this.xrLabel30.Font = new System.Drawing.Font("Arial", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))));
            this.xrLabel30.ForeColor = System.Drawing.Color.Black;
            this.xrLabel30.LocationFloat = new DevExpress.Utils.PointFloat(0F, 281.9722F);
            this.xrLabel30.Name = "xrLabel30";
            this.xrLabel30.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel30.SizeF = new System.Drawing.SizeF(647.9167F, 14.5F);
            this.xrLabel30.StylePriority.UseBackColor = false;
            this.xrLabel30.StylePriority.UseFont = false;
            this.xrLabel30.StylePriority.UseForeColor = false;
            this.xrLabel30.StylePriority.UseTextAlignment = false;
            this.xrLabel30.Text = "Waiver";
            this.xrLabel30.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel38
            // 
            this.xrLabel38.Font = new System.Drawing.Font("Arial", 8F);
            this.xrLabel38.LocationFloat = new DevExpress.Utils.PointFloat(7.916705F, 258.9721F);
            this.xrLabel38.Name = "xrLabel38";
            this.xrLabel38.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel38.SizeF = new System.Drawing.SizeF(633.3334F, 23F);
            this.xrLabel38.StylePriority.UseFont = false;
            this.xrLabel38.Text = resources.GetString("xrLabel38.Text");
            // 
            // xrLabel29
            // 
            this.xrLabel29.BackColor = System.Drawing.Color.Empty;
            this.xrLabel29.Font = new System.Drawing.Font("Arial", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))));
            this.xrLabel29.ForeColor = System.Drawing.Color.Black;
            this.xrLabel29.LocationFloat = new DevExpress.Utils.PointFloat(0F, 240.1387F);
            this.xrLabel29.Name = "xrLabel29";
            this.xrLabel29.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel29.SizeF = new System.Drawing.SizeF(647.9167F, 18.83334F);
            this.xrLabel29.StylePriority.UseBackColor = false;
            this.xrLabel29.StylePriority.UseFont = false;
            this.xrLabel29.StylePriority.UseForeColor = false;
            this.xrLabel29.StylePriority.UseTextAlignment = false;
            this.xrLabel29.Text = "Severability";
            this.xrLabel29.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel37
            // 
            this.xrLabel37.Font = new System.Drawing.Font("Arial", 8F);
            this.xrLabel37.LocationFloat = new DevExpress.Utils.PointFloat(6.666504F, 217.1388F);
            this.xrLabel37.Multiline = true;
            this.xrLabel37.Name = "xrLabel37";
            this.xrLabel37.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel37.SizeF = new System.Drawing.SizeF(633.3334F, 23F);
            this.xrLabel37.StylePriority.UseFont = false;
            this.xrLabel37.Text = resources.GetString("xrLabel37.Text");
            // 
            // xrLabel28
            // 
            this.xrLabel28.BackColor = System.Drawing.Color.Empty;
            this.xrLabel28.Font = new System.Drawing.Font("Arial", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))));
            this.xrLabel28.ForeColor = System.Drawing.Color.Black;
            this.xrLabel28.LocationFloat = new DevExpress.Utils.PointFloat(0F, 198.3053F);
            this.xrLabel28.Name = "xrLabel28";
            this.xrLabel28.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel28.SizeF = new System.Drawing.SizeF(647.9167F, 18.83334F);
            this.xrLabel28.StylePriority.UseBackColor = false;
            this.xrLabel28.StylePriority.UseFont = false;
            this.xrLabel28.StylePriority.UseForeColor = false;
            this.xrLabel28.StylePriority.UseTextAlignment = false;
            this.xrLabel28.Text = "Entire Agreement and Construction";
            this.xrLabel28.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel35
            // 
            this.xrLabel35.Font = new System.Drawing.Font("Arial", 8F);
            this.xrLabel35.LocationFloat = new DevExpress.Utils.PointFloat(10.41667F, 175.3053F);
            this.xrLabel35.Name = "xrLabel35";
            this.xrLabel35.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel35.SizeF = new System.Drawing.SizeF(633.3334F, 23F);
            this.xrLabel35.StylePriority.UseFont = false;
            this.xrLabel35.Text = resources.GetString("xrLabel35.Text");
            // 
            // xrLabel27
            // 
            this.xrLabel27.BackColor = System.Drawing.Color.Empty;
            this.xrLabel27.Font = new System.Drawing.Font("Arial", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))));
            this.xrLabel27.ForeColor = System.Drawing.Color.Black;
            this.xrLabel27.LocationFloat = new DevExpress.Utils.PointFloat(0F, 156.4718F);
            this.xrLabel27.Name = "xrLabel27";
            this.xrLabel27.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel27.SizeF = new System.Drawing.SizeF(647.9167F, 18.83334F);
            this.xrLabel27.StylePriority.UseBackColor = false;
            this.xrLabel27.StylePriority.UseFont = false;
            this.xrLabel27.StylePriority.UseForeColor = false;
            this.xrLabel27.StylePriority.UseTextAlignment = false;
            this.xrLabel27.Text = "Assignment";
            this.xrLabel27.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel4
            // 
            this.xrLabel4.Font = new System.Drawing.Font("Arial", 8F);
            this.xrLabel4.LocationFloat = new DevExpress.Utils.PointFloat(7.499886F, 95.75011F);
            this.xrLabel4.Name = "xrLabel4";
            this.xrLabel4.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel4.SizeF = new System.Drawing.SizeF(633.3334F, 7.375F);
            this.xrLabel4.StylePriority.UseFont = false;
            this.xrLabel4.Text = "If you have a disability that requires an accommodation to assist you, please con" +
    "tact your supervisor listed in this agreement.";
            // 
            // xrLabel24
            // 
            this.xrLabel24.BackColor = System.Drawing.Color.Empty;
            this.xrLabel24.Font = new System.Drawing.Font("Arial", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))));
            this.xrLabel24.ForeColor = System.Drawing.Color.Black;
            this.xrLabel24.LocationFloat = new DevExpress.Utils.PointFloat(0F, 76.91675F);
            this.xrLabel24.Name = "xrLabel24";
            this.xrLabel24.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel24.SizeF = new System.Drawing.SizeF(587.0834F, 18.83337F);
            this.xrLabel24.StylePriority.UseBackColor = false;
            this.xrLabel24.StylePriority.UseFont = false;
            this.xrLabel24.StylePriority.UseForeColor = false;
            this.xrLabel24.StylePriority.UseTextAlignment = false;
            this.xrLabel24.Text = "Disability Notice";
            this.xrLabel24.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel26
            // 
            this.xrLabel26.BackColor = System.Drawing.Color.Empty;
            this.xrLabel26.Font = new System.Drawing.Font("Arial", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))));
            this.xrLabel26.ForeColor = System.Drawing.Color.Black;
            this.xrLabel26.LocationFloat = new DevExpress.Utils.PointFloat(0F, 114.6385F);
            this.xrLabel26.Name = "xrLabel26";
            this.xrLabel26.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel26.SizeF = new System.Drawing.SizeF(647.9167F, 18.83334F);
            this.xrLabel26.StylePriority.UseBackColor = false;
            this.xrLabel26.StylePriority.UseFont = false;
            this.xrLabel26.StylePriority.UseForeColor = false;
            this.xrLabel26.StylePriority.UseTextAlignment = false;
            this.xrLabel26.Text = "Safety";
            this.xrLabel26.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel34
            // 
            this.xrLabel34.Font = new System.Drawing.Font("Arial", 8F);
            this.xrLabel34.LocationFloat = new DevExpress.Utils.PointFloat(7.499886F, 133.4718F);
            this.xrLabel34.Name = "xrLabel34";
            this.xrLabel34.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel34.SizeF = new System.Drawing.SizeF(633.3334F, 23F);
            this.xrLabel34.StylePriority.UseFont = false;
            this.xrLabel34.Text = "You understand and agree that safety is a top priority of the Company.  You agree" +
    " to work safely and to comply with all safety laws and regulations as well as sa" +
    "fety policies of the Company.";
            // 
            // xrLabel25
            // 
            this.xrLabel25.CanShrink = true;
            this.xrLabel25.Font = new System.Drawing.Font("Arial", 8F);
            this.xrLabel25.LocationFloat = new DevExpress.Utils.PointFloat(7.499918F, 59.12507F);
            this.xrLabel25.Multiline = true;
            this.xrLabel25.Name = "xrLabel25";
            this.xrLabel25.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel25.SizeF = new System.Drawing.SizeF(630.4167F, 17.79169F);
            this.xrLabel25.StylePriority.UseFont = false;
            this.xrLabel25.Text = resources.GetString("xrLabel25.Text");
            // 
            // xrLabel15
            // 
            this.xrLabel15.BackColor = System.Drawing.Color.Empty;
            this.xrLabel15.Font = new System.Drawing.Font("Arial", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))));
            this.xrLabel15.ForeColor = System.Drawing.Color.Black;
            this.xrLabel15.LocationFloat = new DevExpress.Utils.PointFloat(0F, 40.29174F);
            this.xrLabel15.Name = "xrLabel15";
            this.xrLabel15.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel15.SizeF = new System.Drawing.SizeF(643.75F, 18.83334F);
            this.xrLabel15.StylePriority.UseBackColor = false;
            this.xrLabel15.StylePriority.UseFont = false;
            this.xrLabel15.StylePriority.UseForeColor = false;
            this.xrLabel15.StylePriority.UseTextAlignment = false;
            this.xrLabel15.Text = "Use of Computers and Communication Equipment";
            this.xrLabel15.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel22
            // 
            this.xrLabel22.CanShrink = true;
            this.xrLabel22.Font = new System.Drawing.Font("Arial", 8F);
            this.xrLabel22.LocationFloat = new DevExpress.Utils.PointFloat(7.499918F, 25.625F);
            this.xrLabel22.Multiline = true;
            this.xrLabel22.Name = "xrLabel22";
            this.xrLabel22.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel22.SizeF = new System.Drawing.SizeF(633.7501F, 14.66669F);
            this.xrLabel22.StylePriority.UseFont = false;
            this.xrLabel22.Text = resources.GetString("xrLabel22.Text");
            // 
            // xrLabel16
            // 
            this.xrLabel16.BackColor = System.Drawing.Color.Empty;
            this.xrLabel16.Font = new System.Drawing.Font("Arial", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))));
            this.xrLabel16.ForeColor = System.Drawing.Color.Black;
            this.xrLabel16.LocationFloat = new DevExpress.Utils.PointFloat(0F, 4.166646F);
            this.xrLabel16.Name = "xrLabel16";
            this.xrLabel16.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel16.SizeF = new System.Drawing.SizeF(536.4582F, 18.83334F);
            this.xrLabel16.StylePriority.UseBackColor = false;
            this.xrLabel16.StylePriority.UseFont = false;
            this.xrLabel16.StylePriority.UseForeColor = false;
            this.xrLabel16.StylePriority.UseTextAlignment = false;
            this.xrLabel16.Text = "Protection of Company Interest";
            this.xrLabel16.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // TopMargin
            // 
            this.TopMargin.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.lblheader,
            this.xrPictureBox1});
            this.TopMargin.HeightF = 138.5F;
            this.TopMargin.Name = "TopMargin";
            this.TopMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.TopMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // lblheader
            // 
            this.lblheader.Font = new System.Drawing.Font("Arial", 8F);
            this.lblheader.LocationFloat = new DevExpress.Utils.PointFloat(198.9585F, 47.87501F);
            this.lblheader.Name = "lblheader";
            this.lblheader.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblheader.SizeF = new System.Drawing.SizeF(441.0414F, 23F);
            this.lblheader.StylePriority.UseFont = false;
            this.lblheader.StylePriority.UseTextAlignment = false;
            this.lblheader.Text = "lblheader";
            this.lblheader.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleRight;
            // 
            // xrPictureBox1
            // 
            this.xrPictureBox1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 32.25001F);
            this.xrPictureBox1.Name = "xrPictureBox1";
            this.xrPictureBox1.SizeF = new System.Drawing.SizeF(198.9585F, 106.25F);
            this.xrPictureBox1.Sizing = DevExpress.XtraPrinting.ImageSizeMode.ZoomImage;
            // 
            // BottomMargin
            // 
            this.BottomMargin.HeightF = 0F;
            this.BottomMargin.Name = "BottomMargin";
            this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.BottomMargin.StylePriority.UseTextAlignment = false;
            this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // dt_emp_offer1
            // 
            this.dt_emp_offer1.DataSetName = "dt_emp_offer";
            this.dt_emp_offer1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // dt_emp_offer2
            // 
            this.dt_emp_offer2.DataSetName = "dt_emp_offer";
            this.dt_emp_offer2.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // dt_emp_offer3
            // 
            this.dt_emp_offer3.DataSetName = "dt_emp_offer";
            this.dt_emp_offer3.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // dt_emp_offer4
            // 
            this.dt_emp_offer4.DataSetName = "dt_emp_offer";
            this.dt_emp_offer4.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // moid
            // 
            this.moid.Name = "moid";
            this.moid.Type = typeof(int);
            this.moid.ValueInfo = "0";
            // 
            // ReportHeader
            // 
            this.ReportHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.lbl_dear_name,
            this.xrLabel13,
            this.lbladd2,
            this.lbladd1,
            this.xrLabel5,
            this.lblwearepleased,
            this.lblname1,
            this.xrLabel2,
            this.xrLabel1});
            this.ReportHeader.HeightF = 305.375F;
            this.ReportHeader.Name = "ReportHeader";
            // 
            // lbl_dear_name
            // 
            this.lbl_dear_name.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold);
            this.lbl_dear_name.LocationFloat = new DevExpress.Utils.PointFloat(0F, 139.5833F);
            this.lbl_dear_name.Name = "lbl_dear_name";
            this.lbl_dear_name.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lbl_dear_name.SizeF = new System.Drawing.SizeF(429.1667F, 18.83332F);
            this.lbl_dear_name.StylePriority.UseFont = false;
            this.lbl_dear_name.Text = "lblname1";
            // 
            // xrLabel13
            // 
            this.xrLabel13.Font = new System.Drawing.Font("Arial", 9F);
            this.xrLabel13.LocationFloat = new DevExpress.Utils.PointFloat(10.41667F, 278.2083F);
            this.xrLabel13.Multiline = true;
            this.xrLabel13.Name = "xrLabel13";
            this.xrLabel13.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel13.SizeF = new System.Drawing.SizeF(629.5832F, 8.416687F);
            this.xrLabel13.StylePriority.UseFont = false;
            this.xrLabel13.Text = resources.GetString("xrLabel13.Text");
            // 
            // lbladd2
            // 
            this.lbladd2.LocationFloat = new DevExpress.Utils.PointFloat(0F, 94.08327F);
            this.lbladd2.Name = "lbladd2";
            this.lbladd2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lbladd2.SizeF = new System.Drawing.SizeF(429.1667F, 17.79167F);
            this.lbladd2.Text = "lbladd2";
            // 
            // lbladd1
            // 
            this.lbladd1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 75.24995F);
            this.lbladd1.Name = "lbladd1";
            this.lbladd1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lbladd1.SizeF = new System.Drawing.SizeF(429.1667F, 18.83333F);
            this.lbladd1.Text = "lbladd1";
            // 
            // xrLabel5
            // 
            this.xrLabel5.BackColor = System.Drawing.Color.Empty;
            this.xrLabel5.Font = new System.Drawing.Font("Arial", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))));
            this.xrLabel5.ForeColor = System.Drawing.Color.Black;
            this.xrLabel5.LocationFloat = new DevExpress.Utils.PointFloat(0F, 253.125F);
            this.xrLabel5.Name = "xrLabel5";
            this.xrLabel5.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel5.SizeF = new System.Drawing.SizeF(650F, 18.83334F);
            this.xrLabel5.StylePriority.UseBackColor = false;
            this.xrLabel5.StylePriority.UseFont = false;
            this.xrLabel5.StylePriority.UseForeColor = false;
            this.xrLabel5.StylePriority.UseTextAlignment = false;
            this.xrLabel5.Text = "Responsibilities";
            this.xrLabel5.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // lblwearepleased
            // 
            this.lblwearepleased.Font = new System.Drawing.Font("Arial", 9F);
            this.lblwearepleased.LocationFloat = new DevExpress.Utils.PointFloat(0F, 229.5F);
            this.lblwearepleased.Name = "lblwearepleased";
            this.lblwearepleased.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblwearepleased.SizeF = new System.Drawing.SizeF(639.9999F, 9.458328F);
            this.lblwearepleased.StylePriority.UseFont = false;
            this.lblwearepleased.Text = "We are pleased to offer the position of xxx";
            // 
            // lblname1
            // 
            this.lblname1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 56.41664F);
            this.lblname1.Name = "lblname1";
            this.lblname1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblname1.SizeF = new System.Drawing.SizeF(429.1667F, 18.83332F);
            this.lblname1.Text = "lblname1";
            // 
            // xrLabel2
            // 
            this.xrLabel2.LocationFloat = new DevExpress.Utils.PointFloat(0F, 25.08335F);
            this.xrLabel2.Name = "xrLabel2";
            this.xrLabel2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel2.SizeF = new System.Drawing.SizeF(259.375F, 18.83334F);
            this.xrLabel2.Text = "xrLabel2";
            // 
            // xrLabel1
            // 
            this.xrLabel1.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold);
            this.xrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(1.041603F, 196.0833F);
            this.xrLabel1.Name = "xrLabel1";
            this.xrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel1.SizeF = new System.Drawing.SizeF(600F, 17.79166F);
            this.xrLabel1.StylePriority.UseFont = false;
            this.xrLabel1.Text = "Re: Offer of Employment ";
            // 
            // xrSubreport1
            // 
            this.xrSubreport1.LocationFloat = new DevExpress.Utils.PointFloat(1.666562F, 34.99997F);
            this.xrSubreport1.Name = "xrSubreport1";
            this.xrSubreport1.SizeF = new System.Drawing.SizeF(641.6667F, 23F);
            // 
            // formattingRule1
            // 
            this.formattingRule1.Name = "formattingRule1";
            // 
            // ReportFooter
            // 
            this.ReportFooter.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.lbl_milestone_header,
            this.xrSubreport_milestones,
            this.xrLabel11,
            this.pnl_comp,
            this.xrLabel3,
            this.xrSubreport1});
            this.ReportFooter.HeightF = 250F;
            this.ReportFooter.Name = "ReportFooter";
            // 
            // lbl_milestone_header
            // 
            this.lbl_milestone_header.Font = new System.Drawing.Font("Arial", 8F);
            this.lbl_milestone_header.LocationFloat = new DevExpress.Utils.PointFloat(6.249809F, 80.91668F);
            this.lbl_milestone_header.Name = "lbl_milestone_header";
            this.lbl_milestone_header.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lbl_milestone_header.SizeF = new System.Drawing.SizeF(629.5834F, 2.166664F);
            this.lbl_milestone_header.StylePriority.UseFont = false;
            this.lbl_milestone_header.Text = "Milestone Objectives are defined to provide longer term direction apart from day " +
    "to day responsibilities.  You understand that milestone definitions and dates ma" +
    "y change as required by the Company.  ";
            // 
            // xrSubreport_milestones
            // 
            this.xrSubreport_milestones.LocationFloat = new DevExpress.Utils.PointFloat(6.249873F, 88.63894F);
            this.xrSubreport_milestones.Name = "xrSubreport_milestones";
            this.xrSubreport_milestones.SizeF = new System.Drawing.SizeF(623.7501F, 17.44449F);
            // 
            // xrLabel11
            // 
            this.xrLabel11.BackColor = System.Drawing.Color.Empty;
            this.xrLabel11.Font = new System.Drawing.Font("Arial", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))));
            this.xrLabel11.ForeColor = System.Drawing.Color.Black;
            this.xrLabel11.LocationFloat = new DevExpress.Utils.PointFloat(0F, 70.49993F);
            this.xrLabel11.Name = "xrLabel11";
            this.xrLabel11.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel11.SizeF = new System.Drawing.SizeF(629.5834F, 2.777779F);
            this.xrLabel11.StylePriority.UseBackColor = false;
            this.xrLabel11.StylePriority.UseFont = false;
            this.xrLabel11.StylePriority.UseForeColor = false;
            this.xrLabel11.StylePriority.UseTextAlignment = false;
            this.xrLabel11.Text = "Milestone Objectives";
            this.xrLabel11.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            this.xrLabel11.Visible = false;
            // 
            // pnl_comp
            // 
            this.pnl_comp.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel40,
            this.lblcomp_final,
            this.xrLabel20});
            this.pnl_comp.LocationFloat = new DevExpress.Utils.PointFloat(0F, 117.0833F);
            this.pnl_comp.Name = "pnl_comp";
            this.pnl_comp.SizeF = new System.Drawing.SizeF(643.75F, 122.9167F);
            // 
            // xrLabel40
            // 
            this.xrLabel40.Font = new System.Drawing.Font("Arial", 8F);
            this.xrLabel40.LocationFloat = new DevExpress.Utils.PointFloat(6.249809F, 89.91667F);
            this.xrLabel40.Multiline = true;
            this.xrLabel40.Name = "xrLabel40";
            this.xrLabel40.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel40.SizeF = new System.Drawing.SizeF(629.5834F, 23F);
            this.xrLabel40.StylePriority.UseFont = false;
            this.xrLabel40.Text = resources.GetString("xrLabel40.Text");
            // 
            // lblcomp_final
            // 
            this.lblcomp_final.CanShrink = true;
            this.lblcomp_final.Font = new System.Drawing.Font("Arial", 9F);
            this.lblcomp_final.LocationFloat = new DevExpress.Utils.PointFloat(6.249809F, 48.25001F);
            this.lblcomp_final.Multiline = true;
            this.lblcomp_final.Name = "lblcomp_final";
            this.lblcomp_final.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblcomp_final.SizeF = new System.Drawing.SizeF(633.3333F, 22.99999F);
            this.lblcomp_final.StylePriority.UseFont = false;
            this.lblcomp_final.Text = "lblcomp";
            // 
            // xrLabel20
            // 
            this.xrLabel20.BackColor = System.Drawing.Color.Empty;
            this.xrLabel20.Font = new System.Drawing.Font("Arial", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))));
            this.xrLabel20.ForeColor = System.Drawing.Color.Black;
            this.xrLabel20.LocationFloat = new DevExpress.Utils.PointFloat(2.083166F, 9.999974F);
            this.xrLabel20.Name = "xrLabel20";
            this.xrLabel20.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel20.SizeF = new System.Drawing.SizeF(627.5002F, 18.83334F);
            this.xrLabel20.StylePriority.UseBackColor = false;
            this.xrLabel20.StylePriority.UseFont = false;
            this.xrLabel20.StylePriority.UseForeColor = false;
            this.xrLabel20.StylePriority.UseTextAlignment = false;
            this.xrLabel20.Text = "Appendix B:  Incentive Compensation Plan.";
            this.xrLabel20.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel3
            // 
            this.xrLabel3.BackColor = System.Drawing.Color.Empty;
            this.xrLabel3.Font = new System.Drawing.Font("Arial", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))));
            this.xrLabel3.ForeColor = System.Drawing.Color.Black;
            this.xrLabel3.LocationFloat = new DevExpress.Utils.PointFloat(0F, 9.999974F);
            this.xrLabel3.Name = "xrLabel3";
            this.xrLabel3.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel3.SizeF = new System.Drawing.SizeF(650F, 18.83334F);
            this.xrLabel3.StylePriority.UseBackColor = false;
            this.xrLabel3.StylePriority.UseFont = false;
            this.xrLabel3.StylePriority.UseForeColor = false;
            this.xrLabel3.StylePriority.UseTextAlignment = false;
            this.xrLabel3.Text = "Appendix A:  Responsibilities (can be altered as required by your supervisor).";
            this.xrLabel3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // PageFooter
            // 
            this.PageFooter.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel19,
            this.xrPageInfo1});
            this.PageFooter.HeightF = 67.70834F;
            this.PageFooter.Name = "PageFooter";
            // 
            // xrLabel19
            // 
            this.xrLabel19.Font = new System.Drawing.Font("Arial", 8F, System.Drawing.FontStyle.Italic);
            this.xrLabel19.LocationFloat = new DevExpress.Utils.PointFloat(0F, 23.95833F);
            this.xrLabel19.Name = "xrLabel19";
            this.xrLabel19.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel19.SizeF = new System.Drawing.SizeF(248.9583F, 23F);
            this.xrLabel19.StylePriority.UseFont = false;
            this.xrLabel19.Text = "xrLabel19";
            // 
            // xrPageInfo1
            // 
            this.xrPageInfo1.LocationFloat = new DevExpress.Utils.PointFloat(264.5833F, 23.95833F);
            this.xrPageInfo1.Name = "xrPageInfo1";
            this.xrPageInfo1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrPageInfo1.SizeF = new System.Drawing.SizeF(100F, 23F);
            this.xrPageInfo1.StylePriority.UseTextAlignment = false;
            this.xrPageInfo1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // jd
            // 
            this.jd.Name = "jd";
            this.jd.Type = typeof(bool);
            this.jd.ValueInfo = "False";
            // 
            // emp_offer
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.ReportHeader,
            this.ReportFooter,
            this.PageFooter});
            this.DataMember = "member_offers";
            this.DataSource = this.dt_emp_offer4;
            this.Font = new System.Drawing.Font("Arial", 9.75F);
            this.FormattingRuleSheet.AddRange(new DevExpress.XtraReports.UI.FormattingRule[] {
            this.formattingRule1});
            this.Margins = new System.Drawing.Printing.Margins(94, 106, 138, 0);
            this.Parameters.AddRange(new DevExpress.XtraReports.Parameters.Parameter[] {
            this.moid,
            this.jd});
            this.Version = "17.1";
            this.VerticalContentSplitting = DevExpress.XtraPrinting.VerticalContentSplitting.Smart;
            this.Watermark.Font = new System.Drawing.Font("Verdana", 48F, System.Drawing.FontStyle.Bold);
            this.Watermark.Text = "DRAFT - NOT APPROVED";
            this.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.emp_offer_BeforePrint);
            ((System.ComponentModel.ISupportInitialize)(this.sig_prelude)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dt_emp_offer1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dt_emp_offer2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dt_emp_offer3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dt_emp_offer4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

			}
	 
		#endregion

		private void emp_offer_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
			{

			var _tools			= new Toolbox();
			var current_user	= Toolbox.do_handle_authentication(1);
			if (!current_user.isContact)
				{
				var can_see_all_offers = current_user.AuthenticatedForPrivilege(152);
				var mo_ta = new Nesi.Web.Reports.emp_offer.dt_emp_offerTableAdapters.member_offersTableAdapter();
				mo_ta.Fill(dt_emp_offer4.member_offers, Convert.ToInt32(moid.Value));
				var mo = new NeMemberOffer(Convert.ToInt32(moid.Value));

				if (mo.enteredby != current_user.id &&
					!can_see_all_offers &&
					mo.memberid != current_user.id &&
					mo.reports_to != current_user.id &&
					!NeMember.is_supervisor(mo.memberid, current_user.id)
				)
					{
					foreach (XRControl c in ReportHeader.Controls) 
						{
						c.Visible = false;
						}
					foreach (XRControl c in Detail.Controls)
						{
						c.Visible = false;
						}
					foreach (XRControl c in ReportFooter.Controls)
						{
						c.Visible = false;
						}
					lblheader.Visible = false;
					xrPictureBox1.Visible = false;

					}

				var cr = new dt_emp_offer_cr();
				cr.Parameters[0].Value = Convert.ToInt32(mo.id);
				cr.Parameters[1].Value = Convert.ToBoolean(jd.Value);
				xrSubreport1.ReportSource = cr;

				var ms = new rpt_milestones();
				ms.Parameters[0].Value = Convert.ToInt32(mo.id);
				xrSubreport_milestones.ReportSource = ms;


				if ((mo.status == "Released") || (mo.status == "Approved") || (mo.status == "Accepted") || (mo.status == "Awaiting Start Date"))
					{
					Watermark.Text = "";
					} 
				}

			}

		private void xrLabel2_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
			{
         
			}

	

	

	
		}
	}
using DevExpress.XtraReports.UI;

namespace nesi.core.print
	{
	/// <summary>
	/// Summary description for emp_review
	/// </summary>
	public class emp_review : XtraReport
		{
		private DetailBand Detail; 
		private TopMarginBand TopMargin;
		private BottomMarginBand BottomMargin;
		private XRPictureBox xrPictureBox1;
		private XRLabel lblheader;
		private ReportHeaderBand ReportHeader;
		private XRLabel lblby;
		private XRLabel lblmt;
		private XRLabel lbldate;
		private XRLabel lblname;
		private XRLabel xrLabel4;
		private XRLabel xrLabel3;
		private XRLabel xrLabel2;
		private XRLabel xrLabel1;
		private PageFooterBand PageFooter;
		private XRLabel lblfooterdate;
		private XRPageInfo xrPageInfo1;
		private XRSubreport rptcr;
		private XRLabel xrLabel6;
		private XRLabel xrLabel5;
		private XRSubreport rptgeneral;
		private DevExpress.XtraReports.Parameters.Parameter id;
		private XRRichText xrRichText3;
		private XRRichText xrRichText2;
		private XRRichText xrRichText1;
		private XRLabel xrLabel11;
		private XRLabel xrLabel10;
		private XRLabel xrLabel9;
		private XRLabel xrLabel8;
		private XRLabel xrLabel7;
		private DevExpress.XtraReports.Parameters.Parameter isworksheet;
		private DevExpress.XtraReports.Parameters.Parameter mid;
		private DevExpress.XtraReports.Parameters.Parameter mtid;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		public emp_review()
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
            string resourceFileName = "emp_review.resx";
            System.Resources.ResourceManager resources = global::Resources.emp_review.ResourceManager;
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.xrRichText3 = new DevExpress.XtraReports.UI.XRRichText();
            this.xrRichText2 = new DevExpress.XtraReports.UI.XRRichText();
            this.rptcr = new DevExpress.XtraReports.UI.XRSubreport();
            this.xrLabel6 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel5 = new DevExpress.XtraReports.UI.XRLabel();
            this.rptgeneral = new DevExpress.XtraReports.UI.XRSubreport();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.xrPictureBox1 = new DevExpress.XtraReports.UI.XRPictureBox();
            this.lblheader = new DevExpress.XtraReports.UI.XRLabel();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.xrPageInfo1 = new DevExpress.XtraReports.UI.XRPageInfo();
            this.lblfooterdate = new DevExpress.XtraReports.UI.XRLabel();
            this.ReportHeader = new DevExpress.XtraReports.UI.ReportHeaderBand();
            this.xrRichText1 = new DevExpress.XtraReports.UI.XRRichText();
            this.lblby = new DevExpress.XtraReports.UI.XRLabel();
            this.lblmt = new DevExpress.XtraReports.UI.XRLabel();
            this.lbldate = new DevExpress.XtraReports.UI.XRLabel();
            this.lblname = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel4 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel3 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel2 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
            this.PageFooter = new DevExpress.XtraReports.UI.PageFooterBand();
            this.xrLabel11 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel10 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel9 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel8 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel7 = new DevExpress.XtraReports.UI.XRLabel();
            this.id = new DevExpress.XtraReports.Parameters.Parameter();
            this.isworksheet = new DevExpress.XtraReports.Parameters.Parameter();
            this.mid = new DevExpress.XtraReports.Parameters.Parameter();
            this.mtid = new DevExpress.XtraReports.Parameters.Parameter();
            ((System.ComponentModel.ISupportInitialize)(this.xrRichText3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrRichText2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrRichText1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrRichText3,
            this.xrRichText2,
            this.rptcr,
            this.xrLabel6,
            this.xrLabel5,
            this.rptgeneral});
            this.Detail.HeightF = 210.4167F;
            this.Detail.Name = "Detail";
            this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrRichText3
            // 
            this.xrRichText3.Font = new System.Drawing.Font("Times New Roman", 9.75F);
            this.xrRichText3.KeepTogether = true;
            this.xrRichText3.LocationFloat = new DevExpress.Utils.PointFloat(1.589457E-05F, 144.0833F);
            this.xrRichText3.Name = "xrRichText3";
            this.xrRichText3.SerializableRtfString = resources.GetString("xrRichText3.SerializableRtfString");
            this.xrRichText3.SizeF = new System.Drawing.SizeF(900F, 23F);
            this.xrRichText3.StylePriority.UseFont = false;
            // 
            // xrRichText2
            // 
            this.xrRichText2.Font = new System.Drawing.Font("Times New Roman", 9.75F);
            this.xrRichText2.KeepTogether = true;
            this.xrRichText2.LocationFloat = new DevExpress.Utils.PointFloat(1.589457E-05F, 33.41665F);
            this.xrRichText2.Name = "xrRichText2";
            this.xrRichText2.SerializableRtfString = resources.GetString("xrRichText2.SerializableRtfString");
            this.xrRichText2.SizeF = new System.Drawing.SizeF(900F, 23F);
            this.xrRichText2.StylePriority.UseFont = false;
            // 
            // rptcr
            // 
            this.rptcr.LocationFloat = new DevExpress.Utils.PointFloat(0F, 179.5F);
            this.rptcr.Name = "rptcr";
            this.rptcr.SizeF = new System.Drawing.SizeF(900F, 23F);
            // 
            // xrLabel6
            // 
            this.xrLabel6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(88)))), ((int)(((byte)(88)))));
            this.xrLabel6.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Bold);
            this.xrLabel6.ForeColor = System.Drawing.Color.White;
            this.xrLabel6.LocationFloat = new DevExpress.Utils.PointFloat(0F, 108.5833F);
            this.xrLabel6.Name = "xrLabel6";
            this.xrLabel6.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel6.SizeF = new System.Drawing.SizeF(900F, 20.91665F);
            this.xrLabel6.StylePriority.UseBackColor = false;
            this.xrLabel6.StylePriority.UseFont = false;
            this.xrLabel6.StylePriority.UseForeColor = false;
            this.xrLabel6.StylePriority.UseTextAlignment = false;
            this.xrLabel6.Text = "Membertype Review:";
            this.xrLabel6.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // xrLabel5
            // 
            this.xrLabel5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(88)))), ((int)(((byte)(88)))), ((int)(((byte)(88)))));
            this.xrLabel5.Font = new System.Drawing.Font("Arial", 11F, System.Drawing.FontStyle.Bold);
            this.xrLabel5.ForeColor = System.Drawing.Color.White;
            this.xrLabel5.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrLabel5.Name = "xrLabel5";
            this.xrLabel5.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel5.SizeF = new System.Drawing.SizeF(899.9999F, 18.83333F);
            this.xrLabel5.StylePriority.UseBackColor = false;
            this.xrLabel5.StylePriority.UseFont = false;
            this.xrLabel5.StylePriority.UseForeColor = false;
            this.xrLabel5.StylePriority.UseTextAlignment = false;
            this.xrLabel5.Text = "General Review:";
            this.xrLabel5.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
            // 
            // rptgeneral
            // 
            this.rptgeneral.LocationFloat = new DevExpress.Utils.PointFloat(0F, 68.79167F);
            this.rptgeneral.Name = "rptgeneral";
            this.rptgeneral.SizeF = new System.Drawing.SizeF(900F, 23F);
            // 
            // TopMargin
            // 
            this.TopMargin.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrPictureBox1,
            this.lblheader});
            this.TopMargin.HeightF = 116.6667F;
            this.TopMargin.Name = "TopMargin";
            this.TopMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.TopMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrPictureBox1
            // 
            this.xrPictureBox1.ImageUrl = Toolbox.app_setting("Domain") + @"/images/Logos/nesi -logo-blue-invert.png";
            this.xrPictureBox1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 10.00001F);
            this.xrPictureBox1.Name = "xrPictureBox1";
            this.xrPictureBox1.SizeF = new System.Drawing.SizeF(152.0834F, 106.25F);
            this.xrPictureBox1.Sizing = DevExpress.XtraPrinting.ImageSizeMode.ZoomImage;
            // 
            // lblheader
            // 
            this.lblheader.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold);
            this.lblheader.LocationFloat = new DevExpress.Utils.PointFloat(264.5833F, 33.33333F);
            this.lblheader.Name = "lblheader";
            this.lblheader.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblheader.SizeF = new System.Drawing.SizeF(635.4166F, 23F);
            this.lblheader.StylePriority.UseFont = false;
            this.lblheader.StylePriority.UseTextAlignment = false;
            this.lblheader.Text = "lblheader";
            this.lblheader.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            // 
            // BottomMargin
            // 
            this.BottomMargin.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrPageInfo1,
            this.lblfooterdate});
            this.BottomMargin.HeightF = 54.16667F;
            this.BottomMargin.Name = "BottomMargin";
            this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
            this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrPageInfo1
            // 
            this.xrPageInfo1.Font = new System.Drawing.Font("Arial", 9.75F);
            this.xrPageInfo1.LocationFloat = new DevExpress.Utils.PointFloat(375F, 9.999974F);
            this.xrPageInfo1.Name = "xrPageInfo1";
            this.xrPageInfo1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrPageInfo1.SizeF = new System.Drawing.SizeF(100F, 23F);
            this.xrPageInfo1.StylePriority.UseFont = false;
            this.xrPageInfo1.StylePriority.UseTextAlignment = false;
            this.xrPageInfo1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // lblfooterdate
            // 
            this.lblfooterdate.Font = new System.Drawing.Font("Arial", 9.75F);
            this.lblfooterdate.LocationFloat = new DevExpress.Utils.PointFloat(700.4167F, 9.999974F);
            this.lblfooterdate.Name = "lblfooterdate";
            this.lblfooterdate.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblfooterdate.SizeF = new System.Drawing.SizeF(189.5833F, 23F);
            this.lblfooterdate.StylePriority.UseFont = false;
            this.lblfooterdate.StylePriority.UseTextAlignment = false;
            this.lblfooterdate.Text = "lblfooterdate";
            this.lblfooterdate.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            // 
            // ReportHeader
            // 
            this.ReportHeader.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrRichText1,
            this.lblby,
            this.lblmt,
            this.lbldate,
            this.lblname,
            this.xrLabel4,
            this.xrLabel3,
            this.xrLabel2,
            this.xrLabel1});
            this.ReportHeader.HeightF = 125F;
            this.ReportHeader.Name = "ReportHeader";
            // 
            // xrRichText1
            // 
            this.xrRichText1.Font = new System.Drawing.Font("Arial", 9.75F);
            this.xrRichText1.LocationFloat = new DevExpress.Utils.PointFloat(1.589457E-05F, 95.79166F);
            this.xrRichText1.Name = "xrRichText1";
            this.xrRichText1.SerializableRtfString = resources.GetString("xrRichText1.SerializableRtfString");
            this.xrRichText1.SizeF = new System.Drawing.SizeF(900F, 23F);
            this.xrRichText1.StylePriority.UseFont = false;
            // 
            // lblby
            // 
            this.lblby.Font = new System.Drawing.Font("Arial", 9.75F);
            this.lblby.LocationFloat = new DevExpress.Utils.PointFloat(116.6667F, 47.66668F);
            this.lblby.Name = "lblby";
            this.lblby.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblby.SizeF = new System.Drawing.SizeF(375F, 18.83333F);
            this.lblby.StylePriority.UseFont = false;
            this.lblby.StylePriority.UseTextAlignment = false;
            this.lblby.Text = "Reviewed By:";
            this.lblby.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // lblmt
            // 
            this.lblmt.Font = new System.Drawing.Font("Arial", 9.75F);
            this.lblmt.LocationFloat = new DevExpress.Utils.PointFloat(116.6667F, 66.5F);
            this.lblmt.Name = "lblmt";
            this.lblmt.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblmt.SizeF = new System.Drawing.SizeF(375F, 18.83334F);
            this.lblmt.StylePriority.UseFont = false;
            this.lblmt.StylePriority.UseTextAlignment = false;
            this.lblmt.Text = "Membertype:";
            this.lblmt.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // lbldate
            // 
            this.lbldate.Font = new System.Drawing.Font("Arial", 9.75F);
            this.lbldate.LocationFloat = new DevExpress.Utils.PointFloat(116.6667F, 10.00001F);
            this.lbldate.Name = "lbldate";
            this.lbldate.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lbldate.SizeF = new System.Drawing.SizeF(375F, 18.83333F);
            this.lbldate.StylePriority.UseFont = false;
            this.lbldate.StylePriority.UseTextAlignment = false;
            this.lbldate.Text = "Date:";
            this.lbldate.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // lblname
            // 
            this.lblname.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold);
            this.lblname.LocationFloat = new DevExpress.Utils.PointFloat(116.6667F, 28.83333F);
            this.lblname.Name = "lblname";
            this.lblname.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.lblname.SizeF = new System.Drawing.SizeF(375F, 18.83334F);
            this.lblname.StylePriority.UseFont = false;
            this.lblname.StylePriority.UseTextAlignment = false;
            this.lblname.Text = "Employee Name:";
            this.lblname.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrLabel4
            // 
            this.xrLabel4.Font = new System.Drawing.Font("Arial", 9.75F);
            this.xrLabel4.LocationFloat = new DevExpress.Utils.PointFloat(0F, 66.5F);
            this.xrLabel4.Name = "xrLabel4";
            this.xrLabel4.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel4.SizeF = new System.Drawing.SizeF(116.6667F, 18.83334F);
            this.xrLabel4.StylePriority.UseFont = false;
            this.xrLabel4.StylePriority.UseTextAlignment = false;
            this.xrLabel4.Text = "Membertype:";
            this.xrLabel4.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrLabel3
            // 
            this.xrLabel3.Font = new System.Drawing.Font("Arial", 9.75F);
            this.xrLabel3.LocationFloat = new DevExpress.Utils.PointFloat(0F, 47.66668F);
            this.xrLabel3.Name = "xrLabel3";
            this.xrLabel3.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel3.SizeF = new System.Drawing.SizeF(116.6667F, 18.83333F);
            this.xrLabel3.StylePriority.UseFont = false;
            this.xrLabel3.StylePriority.UseTextAlignment = false;
            this.xrLabel3.Text = "Reviewed By:";
            this.xrLabel3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrLabel2
            // 
            this.xrLabel2.Font = new System.Drawing.Font("Arial", 9.75F);
            this.xrLabel2.LocationFloat = new DevExpress.Utils.PointFloat(0F, 28.83333F);
            this.xrLabel2.Name = "xrLabel2";
            this.xrLabel2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel2.SizeF = new System.Drawing.SizeF(116.6667F, 18.83333F);
            this.xrLabel2.StylePriority.UseFont = false;
            this.xrLabel2.StylePriority.UseTextAlignment = false;
            this.xrLabel2.Text = "Employee Name:";
            this.xrLabel2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // xrLabel1
            // 
            this.xrLabel1.Font = new System.Drawing.Font("Arial", 9.75F);
            this.xrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 10.00001F);
            this.xrLabel1.Name = "xrLabel1";
            this.xrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel1.SizeF = new System.Drawing.SizeF(116.6667F, 18.83333F);
            this.xrLabel1.StylePriority.UseFont = false;
            this.xrLabel1.StylePriority.UseTextAlignment = false;
            this.xrLabel1.Text = "Date:";
            this.xrLabel1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
            // 
            // PageFooter
            // 
            this.PageFooter.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel11,
            this.xrLabel10,
            this.xrLabel9,
            this.xrLabel8,
            this.xrLabel7});
            this.PageFooter.HeightF = 45.83333F;
            this.PageFooter.Name = "PageFooter";
            this.PageFooter.PrintOn = ((DevExpress.XtraReports.UI.PrintOnPages)((DevExpress.XtraReports.UI.PrintOnPages.NotWithReportHeader | DevExpress.XtraReports.UI.PrintOnPages.NotWithReportFooter)));
            // 
            // xrLabel11
            // 
            this.xrLabel11.Font = new System.Drawing.Font("Arial", 8F);
            this.xrLabel11.LocationFloat = new DevExpress.Utils.PointFloat(743.7501F, 9.999974F);
            this.xrLabel11.Name = "xrLabel11";
            this.xrLabel11.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel11.SizeF = new System.Drawing.SizeF(146.2499F, 16.75F);
            this.xrLabel11.StylePriority.UseFont = false;
            this.xrLabel11.StylePriority.UseTextAlignment = false;
            this.xrLabel11.Text = "5- Always Exceeds";
            this.xrLabel11.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopRight;
            // 
            // xrLabel10
            // 
            this.xrLabel10.Font = new System.Drawing.Font("Arial", 8F);
            this.xrLabel10.LocationFloat = new DevExpress.Utils.PointFloat(570.8333F, 9.999974F);
            this.xrLabel10.Name = "xrLabel10";
            this.xrLabel10.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel10.SizeF = new System.Drawing.SizeF(156.25F, 16.75F);
            this.xrLabel10.StylePriority.UseFont = false;
            this.xrLabel10.StylePriority.UseTextAlignment = false;
            this.xrLabel10.Text = "4 - Sometimes Exceeds";
            this.xrLabel10.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // xrLabel9
            // 
            this.xrLabel9.Font = new System.Drawing.Font("Arial", 8F);
            this.xrLabel9.LocationFloat = new DevExpress.Utils.PointFloat(358.3333F, 9.999974F);
            this.xrLabel9.Name = "xrLabel9";
            this.xrLabel9.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel9.SizeF = new System.Drawing.SizeF(156.25F, 16.75F);
            this.xrLabel9.StylePriority.UseFont = false;
            this.xrLabel9.StylePriority.UseTextAlignment = false;
            this.xrLabel9.Text = "3- Regularly Meets";
            this.xrLabel9.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
            // 
            // xrLabel8
            // 
            this.xrLabel8.Font = new System.Drawing.Font("Arial", 8F);
            this.xrLabel8.LocationFloat = new DevExpress.Utils.PointFloat(162.5F, 9.999974F);
            this.xrLabel8.Name = "xrLabel8";
            this.xrLabel8.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel8.SizeF = new System.Drawing.SizeF(125F, 16.75F);
            this.xrLabel8.StylePriority.UseFont = false;
            this.xrLabel8.Text = "2 - Sometimes Meets";
            // 
            // xrLabel7
            // 
            this.xrLabel7.Font = new System.Drawing.Font("Arial", 8F);
            this.xrLabel7.LocationFloat = new DevExpress.Utils.PointFloat(0F, 9.999974F);
            this.xrLabel7.Name = "xrLabel7";
            this.xrLabel7.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel7.SizeF = new System.Drawing.SizeF(100F, 16.75F);
            this.xrLabel7.StylePriority.UseFont = false;
            this.xrLabel7.Text = "1-Does Not Meet";
            // 
            // id
            // 
            this.id.Name = "id";
            this.id.Type = typeof(int);
            this.id.ValueInfo = "0";
            // 
            // isworksheet
            // 
            this.isworksheet.Name = "isworksheet";
            // 
            // mid
            // 
            this.mid.Name = "mid";
            this.mid.Type = typeof(int);
            this.mid.ValueInfo = "0";
            // 
            // mtid
            // 
            this.mtid.Name = "mtid";
            this.mtid.Type = typeof(int);
            this.mtid.ValueInfo = "0";
            // 
            // emp_review
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.ReportHeader,
            this.PageFooter});
            this.Landscape = true;
            this.Margins = new System.Drawing.Printing.Margins(100, 100, 117, 54);
            this.PageHeight = 850;
            this.PageWidth = 1100;
            this.Parameters.AddRange(new DevExpress.XtraReports.Parameters.Parameter[] {
            this.id,
            this.isworksheet,
            this.mid,
            this.mtid});
            this.Version = "14.2";
            this.Watermark.Text = "DRAFT - Not Final";
            this.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.emp_review_BeforePrint);
            ((System.ComponentModel.ISupportInitialize)(this.xrRichText3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrRichText2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.xrRichText1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

			}

		#endregion

		private void emp_review_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
			{
			if (isworksheet.Value.ToString() == "1")
				{
				Watermark.Text = "DRAFT - Not Final";
				}
			else
				{
				Watermark.Text = "";
				}

			if (mid.Value.ToString() != "0")
				{
				var gen = new general_review_data();
				gen.Parameters[0].Value = (int)id.Value;
				rptgeneral.ReportSource = gen;

				var cr = new membertype_review_data();
				cr.Parameters[0].Value = (int)mid.Value;
				cr.Parameters[1].Value = 0;
			
				rptcr.ReportSource = cr;
				}
			else if (mtid.Value.ToString() != "0")
				{
				var gen = new general_review_data();
				gen.Parameters[0].Value = (int)id.Value;
				rptgeneral.ReportSource = gen;

				var cr = new membertype_review_data();
				cr.Parameters[0].Value = (int)mid.Value;
				cr.Parameters[1].Value = 0;
				cr.Parameters[2].Value = (int)mtid.Value;
				rptcr.ReportSource = cr;
				}
			else
				{
				var gen = new general_review_final();
				gen.Parameters[0].Value = (int)id.Value;
				gen.Parameters["is_worksheet"].Value = isworksheet.Value.ToString();
				rptgeneral.ReportSource = gen;

				var cr = new membertype_review_final();
				cr.Parameters[0].Value = (int)id.Value;
				cr.Parameters["is_worksheet"].Value = isworksheet.Value.ToString();
				rptcr.ReportSource = cr;

				}

/*		else if (this.isworksheet.Value.ToString() == "1")
		{
			if (this.mtid.Value.ToString() != "0")
			{
				general_review_data gen = new general_review_data();
				gen.Parameters[0].Value = (int)id.Value;
				rptgeneral.ReportSource = gen;

				membertype_review_data cr = new membertype_review_data();
				cr.Parameters[0].Value = (int)mid.Value;
				cr.Parameters[1].Value = 0;
				cr.Parameters[2].Value = (int)mtid.Value;
				rptcr.ReportSource = cr;
			}
			else
			{
				general_review_data gen = new general_review_data();
				gen.Parameters[0].Value = (int)id.Value;
				rptgeneral.ReportSource = gen;

				membertype_review_data cr = new membertype_review_data();
				cr.Parameters[0].Value = 0;
				cr.Parameters[1].Value = (int)id.Value;
				cr.Parameters[2].Value = 0;
				rptcr.ReportSource = cr;
			}
		}
 
		else if (this.isworksheet.Value.ToString() == "0")
		{
			general_review_final gen = new general_review_final();
			gen.Parameters[0].Value = (int)id.Value;
			rptgeneral.ReportSource = gen;

			membertype_review_final cr = new membertype_review_final();
			cr.Parameters[0].Value = (int)id.Value;
			
			rptcr.ReportSource = cr;
		}
	*/	
		
		

		
		

			}
		}
	}
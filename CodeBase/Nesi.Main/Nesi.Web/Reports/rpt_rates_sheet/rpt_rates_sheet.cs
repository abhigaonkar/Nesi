using System;
using DevExpress.XtraReports.UI;
using Nesi.Web.Reports.rpt_rates_sheet;

namespace nesi.core.print
	{
	/// <summary>
	/// Summary description for XtraReport1
	/// </summary>
	public class rpt_rates_sheet : XtraReport
		{
		private DetailBand Detail;
		private TopMarginBand TopMargin;
		private BottomMarginBand BottomMargin;
		private data data1;
		private Nesi.Web.Reports.rpt_rates_sheet.dataTableAdapters.da_bench_rates da_bench_rates1;
		private XRLabel xrLabel2;
		private XRLabel xrLabel1; 
	
		private DevExpress.XtraReports.Parameters.Parameter business_unit_id;
		private DevExpress.XtraReports.Parameters.Parameter customerid;
		private XRLabel lbltitle;
		private data data2;
		private XRPictureBox xrPictureBox1;
		private XRLabel branch_info;
		private XRLabel xrLabel7;
		private XRLabel xrLabel8;
		private XRPictureBox xrPictureBox2;
		private ReportFooterBand ReportFooter;
		private XRCrossBandBox xrCrossBandBox1;
		private XRLabel xrLabel3;
		private XRLabel xrLabel4;
		private XRLabel xrLabel5;
		private XRRichText xrRichText1;
		private XRLabel xrLabel6;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null; 

		public rpt_rates_sheet(int business_unit_id, int customerid)
		    {
			InitializeComponent();
			da_bench_rates1.Fill(data2.dt_benchrates, business_unit_id, customerid);
			var comp = new NeBusinessUnit(business_unit_id);
			if (customerid != 0)
				{
				var cust = new NECustomer(Convert.ToInt32(customerid));
				lbltitle.Text = cust.Customer_Name + Environment.NewLine + Environment.NewLine;
				lbltitle.Text += comp.name + " Chargeout Rates " + Environment.NewLine + " As of: " + DateTime.Today.ToString("yyyy-MM-dd");
				}
			else
				{
				lbltitle.Text = comp.name + " Chargeout Rates " + Environment.NewLine + " As of: " + DateTime.Today.ToString("yyyy-MM-dd");
            }

		    xrPictureBox1.ImageUrl = Toolbox.app_setting("Domain") + @"/images/Logos/" + comp.logo_file;
		    xrPictureBox2.ImageUrl = Toolbox.app_setting("Domain") + @"/images/Logos/" + comp.logo_file;
		    xrLabel7.BackColor = System.Drawing.ColorTranslator.FromHtml(comp.header_color);
		    xrLabel8.BackColor = System.Drawing.ColorTranslator.FromHtml(comp.header_color);
		    xrLabel6.Text = comp.web_domain;
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(rpt_rates_sheet));
			this.Detail = new DevExpress.XtraReports.UI.DetailBand();
			this.xrLabel2 = new DevExpress.XtraReports.UI.XRLabel();
			this.xrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
			this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
			this.xrLabel7 = new DevExpress.XtraReports.UI.XRLabel();
			this.xrLabel8 = new DevExpress.XtraReports.UI.XRLabel();
			this.branch_info = new DevExpress.XtraReports.UI.XRLabel();
			this.xrPictureBox1 = new DevExpress.XtraReports.UI.XRPictureBox();
			this.lbltitle = new DevExpress.XtraReports.UI.XRLabel();
			this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
			this.da_bench_rates1 = new Nesi.Web.Reports.rpt_rates_sheet.dataTableAdapters.da_bench_rates();
			this.data1 = new Nesi.Web.Reports.rpt_rates_sheet.data();
			this.business_unit_id = new DevExpress.XtraReports.Parameters.Parameter();
			this.customerid = new DevExpress.XtraReports.Parameters.Parameter();
			this.data2 = new Nesi.Web.Reports.rpt_rates_sheet.data();
			this.xrPictureBox2 = new DevExpress.XtraReports.UI.XRPictureBox();
			this.ReportFooter = new DevExpress.XtraReports.UI.ReportFooterBand();
			this.xrLabel6 = new DevExpress.XtraReports.UI.XRLabel();
			this.xrRichText1 = new DevExpress.XtraReports.UI.XRRichText();
			this.xrLabel5 = new DevExpress.XtraReports.UI.XRLabel();
			this.xrLabel4 = new DevExpress.XtraReports.UI.XRLabel();
			this.xrLabel3 = new DevExpress.XtraReports.UI.XRLabel();
			this.xrCrossBandBox1 = new DevExpress.XtraReports.UI.XRCrossBandBox();
			((System.ComponentModel.ISupportInitialize)(this.data1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.data2)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.xrRichText1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
			// 
			// Detail
			// 
			this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel2,
            this.xrLabel1});
			this.Detail.HeightF = 25.08335F;
			this.Detail.Name = "Detail";
			this.Detail.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
			this.Detail.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
			// 
			// xrLabel2
			// 
			this.xrLabel2.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[DataTable1.__rate]")});
			this.xrLabel2.Font = new System.Drawing.Font("Arial", 11F);
			this.xrLabel2.LocationFloat = new DevExpress.Utils.PointFloat(553.8159F, 0F);
			this.xrLabel2.Name = "xrLabel2";
			this.xrLabel2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
			this.xrLabel2.SizeF = new System.Drawing.SizeF(144.8506F, 23F);
			this.xrLabel2.StylePriority.UseFont = false;
			this.xrLabel2.StylePriority.UseTextAlignment = false;
			this.xrLabel2.Text = "xrLabel2";
			this.xrLabel2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
			this.xrLabel2.BeforePrint += new System.Drawing.Printing.PrintEventHandler(this.xrLabel2_BeforePrint);
			// 
			// xrLabel1
			// 
			this.xrLabel1.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[DataTable1.type_name]")});
			this.xrLabel1.Font = new System.Drawing.Font("Arial", 11F);
			this.xrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(57.47375F, 0F);
			this.xrLabel1.Name = "xrLabel1";
			this.xrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
			this.xrLabel1.SizeF = new System.Drawing.SizeF(496.3421F, 23F);
			this.xrLabel1.StylePriority.UseFont = false;
			this.xrLabel1.Text = "xrLabel1";
			// 
			// TopMargin
			// 
			this.TopMargin.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel7,
            this.xrLabel8,
            this.branch_info,
            this.xrPictureBox1,
            this.lbltitle});
			this.TopMargin.HeightF = 288.5417F;
			this.TopMargin.Name = "TopMargin";
			this.TopMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
			this.TopMargin.StylePriority.UseBackColor = false;
			this.TopMargin.StylePriority.UseTextAlignment = false;
			this.TopMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
			// 
			// xrLabel7
			// 
			this.xrLabel7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(149)))), ((int)(((byte)(59)))));
			this.xrLabel7.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
			this.xrLabel7.ForeColor = System.Drawing.Color.White;
			this.xrLabel7.LocationFloat = new DevExpress.Utils.PointFloat(14.76542F, 254.1667F);
			this.xrLabel7.Name = "xrLabel7";
			this.xrLabel7.Padding = new DevExpress.XtraPrinting.PaddingInfo(5, 2, 0, 0, 100F);
			this.xrLabel7.SizeF = new System.Drawing.SizeF(539.0504F, 24.37505F);
			this.xrLabel7.StylePriority.UseBackColor = false;
			this.xrLabel7.StylePriority.UseFont = false;
			this.xrLabel7.StylePriority.UseForeColor = false;
			this.xrLabel7.StylePriority.UsePadding = false;
			this.xrLabel7.StylePriority.UseTextAlignment = false;
			this.xrLabel7.Text = "          Service Type";
			this.xrLabel7.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleLeft;
			// 
			// xrLabel8
			// 
			this.xrLabel8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(149)))), ((int)(((byte)(59)))));
			this.xrLabel8.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
			this.xrLabel8.ForeColor = System.Drawing.Color.White;
			this.xrLabel8.LocationFloat = new DevExpress.Utils.PointFloat(553.8159F, 254.1667F);
			this.xrLabel8.Name = "xrLabel8";
			this.xrLabel8.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
			this.xrLabel8.SizeF = new System.Drawing.SizeF(145.8002F, 24.37502F);
			this.xrLabel8.StylePriority.UseBackColor = false;
			this.xrLabel8.StylePriority.UseFont = false;
			this.xrLabel8.StylePriority.UseForeColor = false;
			this.xrLabel8.StylePriority.UseTextAlignment = false;
			this.xrLabel8.Text = "$ Rate / Hour";
			this.xrLabel8.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
			// 
			// branch_info
			// 
			this.branch_info.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.branch_info.LocationFloat = new DevExpress.Utils.PointFloat(246.2499F, 64.58335F);
			this.branch_info.Multiline = true;
			this.branch_info.Name = "branch_info";
			this.branch_info.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
			this.branch_info.SizeF = new System.Drawing.SizeF(272.9167F, 92.29165F);
			this.branch_info.StylePriority.UseFont = false;
			this.branch_info.Text = "branch_info";
			// 
			// xrPictureBox1
			// 
			this.xrPictureBox1.LocationFloat = new DevExpress.Utils.PointFloat(13.81588F, 43.75F);
			this.xrPictureBox1.Name = "xrPictureBox1";
			this.xrPictureBox1.SizeF = new System.Drawing.SizeF(214.7257F, 113.125F);
			this.xrPictureBox1.Sizing = DevExpress.XtraPrinting.ImageSizeMode.ZoomImage;
			// 
			// lbltitle
			// 
			this.lbltitle.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
			this.lbltitle.LocationFloat = new DevExpress.Utils.PointFloat(16.66427F, 162.88F);
			this.lbltitle.Multiline = true;
			this.lbltitle.Name = "lbltitle";
			this.lbltitle.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
			this.lbltitle.SizeF = new System.Drawing.SizeF(684.8508F, 70.91667F);
			this.lbltitle.StylePriority.UseBorderWidth = false;
			this.lbltitle.StylePriority.UseFont = false;
			this.lbltitle.StylePriority.UseTextAlignment = false;
			this.lbltitle.Text = "lbltitle";
			this.lbltitle.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
			// 
			// BottomMargin
			// 
			this.BottomMargin.HeightF = 26.04167F;
			this.BottomMargin.Name = "BottomMargin";
			this.BottomMargin.Padding = new DevExpress.XtraPrinting.PaddingInfo(0, 0, 0, 0, 100F);
			this.BottomMargin.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopLeft;
			// 
			// da_bench_rates1
			// 
			this.da_bench_rates1.ClearBeforeFill = true;
			// 
			// data1
			// 
			this.data1.DataSetName = "data";
			this.data1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
			// 
			// business_unit_id
			// 
			this.business_unit_id.Description = "Parameter1";
			this.business_unit_id.Name = "business_unit_id";
			this.business_unit_id.Type = typeof(short);
			this.business_unit_id.ValueInfo = "0";
			// 
			// customerid
			// 
			this.customerid.Description = "Parameter1";
			this.customerid.Name = "customerid";
			this.customerid.Type = typeof(short);
			this.customerid.ValueInfo = "0";
			// 
			// data2
			// 
			this.data2.DataSetName = "data";
			this.data2.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
			// 
			// xrPictureBox2
			// 
			this.xrPictureBox2.LocationFloat = new DevExpress.Utils.PointFloat(634.8904F, 312.6666F);
			this.xrPictureBox2.Name = "xrPictureBox2";
			this.xrPictureBox2.SizeF = new System.Drawing.SizeF(64.72571F, 30.83331F);
			this.xrPictureBox2.Sizing = DevExpress.XtraPrinting.ImageSizeMode.ZoomImage;
			// 
			// ReportFooter
			// 
			this.ReportFooter.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel6,
            this.xrRichText1,
            this.xrLabel5,
            this.xrLabel4,
            this.xrLabel3,
            this.xrPictureBox2});
			this.ReportFooter.HeightF = 366.6667F;
			this.ReportFooter.Name = "ReportFooter";
			this.ReportFooter.PrintAtBottom = true;
			// 
			// xrLabel6
			// 
			this.xrLabel6.Font = new System.Drawing.Font("Segoe UI", 9.75F);
			this.xrLabel6.LocationFloat = new DevExpress.Utils.PointFloat(278.1251F, 300.6248F);
			this.xrLabel6.Name = "xrLabel6";
			this.xrLabel6.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
			this.xrLabel6.SizeF = new System.Drawing.SizeF(162.5F, 23.00002F);
			this.xrLabel6.StylePriority.UseFont = false;
			this.xrLabel6.StylePriority.UseTextAlignment = false;
			this.xrLabel6.Text = "www.newelectric.com";
			this.xrLabel6.TextAlignment = DevExpress.XtraPrinting.TextAlignment.TopCenter;
			// 
			// xrRichText1
			// 
			this.xrRichText1.Font = new System.Drawing.Font("Arial", 8F);
			this.xrRichText1.LocationFloat = new DevExpress.Utils.PointFloat(15.7149F, 10.00001F);
			this.xrRichText1.Name = "xrRichText1";
			this.xrRichText1.SerializableRtfString = resources.GetString("xrRichText1.SerializableRtfString");
			this.xrRichText1.SizeF = new System.Drawing.SizeF(682.9515F, 210.8333F);
			this.xrRichText1.StylePriority.UseFont = false;
			// 
			// xrLabel5
			// 
			this.xrLabel5.BackColor = System.Drawing.Color.WhiteSmoke;
			this.xrLabel5.Font = new System.Drawing.Font("Arial", 14F);
			this.xrLabel5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(149)))), ((int)(((byte)(59)))));
			this.xrLabel5.LocationFloat = new DevExpress.Utils.PointFloat(15.7149F, 220.8333F);
			this.xrLabel5.Multiline = true;
			this.xrLabel5.Name = "xrLabel5";
			this.xrLabel5.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
			this.xrLabel5.SizeF = new System.Drawing.SizeF(685.8002F, 35.49998F);
			this.xrLabel5.StylePriority.UseBackColor = false;
			this.xrLabel5.StylePriority.UseFont = false;
			this.xrLabel5.StylePriority.UseForeColor = false;
			this.xrLabel5.StylePriority.UseTextAlignment = false;
			this.xrLabel5.Text = "24 HR SERVICE";
			this.xrLabel5.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
			// 
			// xrLabel4
			// 
			this.xrLabel4.BackColor = System.Drawing.Color.WhiteSmoke;
			this.xrLabel4.Font = new System.Drawing.Font("Arial", 22F);
			this.xrLabel4.LocationFloat = new DevExpress.Utils.PointFloat(15.7149F, 256.3333F);
			this.xrLabel4.Multiline = true;
			this.xrLabel4.Name = "xrLabel4";
			this.xrLabel4.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
			this.xrLabel4.SizeF = new System.Drawing.SizeF(685.8002F, 32.375F);
			this.xrLabel4.StylePriority.UseBackColor = false;
			this.xrLabel4.StylePriority.UseFont = false;
			this.xrLabel4.StylePriority.UseTextAlignment = false;
			this.xrLabel4.Text = "1-833-775-7697";
			this.xrLabel4.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
			// 
			// xrLabel3
			// 
			this.xrLabel3.Font = new System.Drawing.Font("Arial", 8F);
			this.xrLabel3.LocationFloat = new DevExpress.Utils.PointFloat(14.76548F, 323.6249F);
			this.xrLabel3.Name = "xrLabel3";
			this.xrLabel3.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
			this.xrLabel3.SizeF = new System.Drawing.SizeF(588.6337F, 19.87499F);
			this.xrLabel3.StylePriority.UseFont = false;
			this.xrLabel3.StylePriority.UseTextAlignment = false;
			this.xrLabel3.Text = "* Rates are current for the date listed above and may change without notice.  ";
			this.xrLabel3.TextAlignment = DevExpress.XtraPrinting.TextAlignment.BottomLeft;
			// 
			// xrCrossBandBox1
			// 
			this.xrCrossBandBox1.AnchorVertical = ((DevExpress.XtraReports.UI.VerticalAnchorStyles)((DevExpress.XtraReports.UI.VerticalAnchorStyles.Top | DevExpress.XtraReports.UI.VerticalAnchorStyles.Bottom)));
			this.xrCrossBandBox1.EndBand = this.ReportFooter;
			this.xrCrossBandBox1.EndPointFloat = new DevExpress.Utils.PointFloat(0F, 359.3749F);
			this.xrCrossBandBox1.Name = "xrCrossBandBox1";
			this.xrCrossBandBox1.StartBand = this.TopMargin;
			this.xrCrossBandBox1.StartPointFloat = new DevExpress.Utils.PointFloat(0F, 29.16667F);
			this.xrCrossBandBox1.WidthF = 717.0001F;
			// 
			// rpt_rates_sheet
			// 
			this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin,
            this.ReportFooter});
			this.CrossBandControls.AddRange(new DevExpress.XtraReports.UI.XRCrossBandControl[] {
            this.xrCrossBandBox1});
			this.DataAdapter = this.da_bench_rates1;
			this.DataMember = "dt_benchrates";
			this.DataSource = this.data2;
			this.Margins = new System.Drawing.Printing.Margins(54, 75, 289, 26);
			this.Parameters.AddRange(new DevExpress.XtraReports.Parameters.Parameter[] {
            this.business_unit_id,
            this.customerid});
			this.Version = "19.2";
			((System.ComponentModel.ISupportInitialize)(this.data1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.data2)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.xrRichText1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this)).EndInit();

			}

		#endregion

		private void xrLabel2_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
			{
		
			}
		}
	}